using System.Collections;
using System.Collections.Generic;
using System;

namespace EGame.Core
{
    // 填充元素, 不支持障碍物
    public class FillElementState : IState
    {
        public string name { get { return "FillElementState"; } }
        private FSM _fsm;
        private EliminateGame _game = null;
        private bool _startFill = false;
        private float _frameTime = 0.1f;
        private float _maxFillTime = 0.1f;
        private List<GameElement> _crossElements = new List<GameElement>();

        public FillElementState(FSM fsm, EliminateGame game) {
            this._fsm = fsm;
            this._game = game;
        }

        public void Enter()
        {
            this._startFill = true;
            this._frameTime = 0.1f;
            this._maxFillTime = 0.1f;
            this._crossElements.Clear();
            // this.FillElement(out this._maxFillTime);
        }

        private void FillEnd() {
            if (this._fsm.hasNextState) {
                this._fsm.NextState();
            } else {
                this._game.OnFillElementComplete();
            }
        }

        private bool FillElement(out float maxFillTime) 
        {
            int cols = this._game.elementCols;
            int rows = this._game.elementRows;
            GameElement[,] elements = this._game.gameElements;
            bool filledNotFinished = false; 
            maxFillTime = 0;
            for (int y = 1; y < rows; y++) {
                for (int x = 0; x < cols; x++) {
                    GameElement element = elements[x, y];
                    if (element.CanMove()) {
                        GameElement emptyElement = this.FindEndElement(x, y, true, this._crossElements);
                        if (emptyElement.y != y) {
                            float fillTime = ((y - emptyElement.y) + 1) * this._game.fillTime;
                            if (fillTime > maxFillTime) maxFillTime = fillTime;
                            element.MoveElement(emptyElement.x, emptyElement.y, fillTime, this.PlayDropBackAnimation, this._crossElements.ToArray());
                            var tempElement = elements[x, y - 1];
                            elements[emptyElement.x, emptyElement.y] = element;
                            emptyElement.MoveElement(x, y, 0, null, null);
                            elements[x, y] = emptyElement;
                            filledNotFinished = true;
                        }
                    }
                }
            }
            if (filledNotFinished == true && this._game.enableSyncFill == false) {
                return filledNotFinished;
            }
            for (int i = 0; i < cols; i++) { // 最上面一行
                // 游戏场景的第一行
                int x = i, y = rows - 1;
                GameElement e = elements[x, y];
                if (e.elementType == GameElementType.Empty) {
                    // 找到最终要落下的点, 更新元素
                    GameElement targetElement = this.FindEndElement(x, y, true, this._crossElements);
                    var view = this._game.gameController.CreateGameElementView(x, y + 1, GameElementType.Normal);
                    targetElement.elementView = view;
                    // 配置元素属性
                    targetElement.Init(targetElement.x, targetElement.y, GameElementType.Normal);
                    float fillTime = ((y - targetElement.y) + 1) * this._game.fillTime;
                    if (fillTime > maxFillTime) maxFillTime = fillTime;
                    targetElement.MoveElement(targetElement.x, targetElement.y, fillTime, this.PlayDropBackAnimation, this._crossElements.ToArray());
                    if (targetElement.elementView != null && targetElement.elementType == GameElementType.Normal) {
                        targetElement.elementView.CreateImageView(targetElement.x, targetElement.y);
                    }
                    filledNotFinished = true;
                }
            }
            return filledNotFinished;
        }

        private GameElement FindEndElement(int x, int y, bool enableCrossBarrier, List<GameElement> crossElements) {
            int cols = this._game.elementCols;
            GameElement[,] elements = this._game.gameElements;
            if (enableCrossBarrier == true && crossElements != null) crossElements.Clear();
            for (int j = y; j >= 0; j--) {
                GameElement element = elements[x, j];
                GameElement leftElement = null, rightElement = null;
                GameElement nextLeftElement = null, nextRightElement = null;
                if (x > 0) leftElement = elements[x - 1, y];
                if (x < cols - 1) rightElement = elements[x + 1, y];
                if (x > 0 && y > 0) nextLeftElement = elements[x - 1, y - 1];
                if (x < cols - 1 && y > 0) nextRightElement = elements[x + 1, y - 1];
                if (enableCrossBarrier == true && element.elementType == GameElementType.Empty) {
                // 如果左元素为障碍物，就检查障碍物下方是否可以下落
                    if (leftElement != null && nextLeftElement != null) { 
                        if (leftElement.elementType == GameElementType.Barrier && nextLeftElement.elementType == GameElementType.Empty) {
                            crossElements.Add(element);
                            crossElements.Add(nextLeftElement);
                            GameElement e = this.FindEndElement(nextLeftElement.x, nextLeftElement.y, false, null);
                            return e;
                        }
                    // 如果右元素为障碍物，就检查障碍物下方是否可以下落
                    } else if (rightElement != null && nextRightElement != null) { 
                        if (rightElement.elementType == GameElementType.Barrier && nextRightElement.elementType == GameElementType.Empty) {
                            crossElements.Add(element);
                            crossElements.Add(nextRightElement);
                            GameElement e = this.FindEndElement(nextRightElement.x, nextRightElement.y, false, null);
                            return e;
                        }
                    }
                }
                if (j > 0) {
                    GameElement nextElement = elements[x, j - 1];
                    if (element.elementType == GameElementType.Empty && nextElement.elementType != GameElementType.Empty) {
                        return element;
                    }
                } else if (element.elementType == GameElementType.Empty) {
                    return element;
                }
            }
            return elements[x, y];
        }

        private void PlayDropBackAnimation(IGameElementView target) {
            GameElement[,] elements = this._game.gameElements;
            GameElement element = elements[target.x, target.y];
            this._game.gameController.LogInfo("PlayDropBackAnimation: " + string.Format("{0}/{1}", target.x, target.y));
            element.elementView.PlayAnimation(GameElementAnimation.DropBack, null);
        }

        public void Update(float deltaTime)
        {
            if (this._startFill == true) {
                this._frameTime += deltaTime;
                if (this._frameTime >= this._game.fillTime) {
                    this._frameTime = 0;
                    if (!this.FillElement(out this._maxFillTime)) {
                        this._startFill = false;
                        this._game.SetTimeout(this._maxFillTime, this.FillEnd);
                    }
                }
            }
        }

        public void Exit() {
            this._startFill = false;
            this._frameTime = 0f;
            this._maxFillTime = 0.1f;
        }
    }
}

