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
                            float fillTime = this.GetFillTime(x, y, emptyElement.x, emptyElement.y, this._crossElements);
                            if (fillTime > maxFillTime) maxFillTime = fillTime;
                            elements[emptyElement.x, emptyElement.y] = element;
                            element.MoveElement(emptyElement.x, emptyElement.y, fillTime, this.PlayDropBackAnimation, this._crossElements.ToArray());
                            elements[x, y] = emptyElement;
                            emptyElement.MoveElement(x, y, 0, null, null);
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
                    float fillTime = this.GetFillTime(x, y + 1, targetElement.x, targetElement.y, this._crossElements);
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
                if (x > 0) leftElement = elements[x - 1, j];
                if (x < cols - 1) rightElement = elements[x + 1, j];
                if (x > 0 && j > 0) nextLeftElement = elements[x - 1, j - 1];
                if (x < cols - 1 && j > 0) nextRightElement = elements[x + 1, j - 1];
                if (enableCrossBarrier == true) {
                    // 左倾斜填充：当前元素左边为障碍物且下方为空
                    if (leftElement != null && nextLeftElement != null) {
                        if (leftElement.elementType == GameElementType.Barrier && nextLeftElement.elementType == GameElementType.Empty) {
                            if (element.y != y) crossElements.Add(element); // 防止转折点是自身
                            GameElement targetElement = this.FindEndElement(nextLeftElement.x, nextLeftElement.y, false, null);
                            if (targetElement.y != nextLeftElement.y) crossElements.Add(nextLeftElement); // 防止转折点是目标
                            return targetElement;
                        }
                    }
                    // 右倾斜填充：当前元素右边为障碍物且下方为空
                    if (rightElement != null && nextRightElement != null) { 
                        if (rightElement.elementType == GameElementType.Barrier && nextRightElement.elementType == GameElementType.Empty) {
                            if (element.y != y) crossElements.Add(element); // 防止转折点是自身
                            GameElement targetElement = this.FindEndElement(nextRightElement.x, nextRightElement.y, false, null);
                            if (targetElement.y != nextRightElement.y) crossElements.Add(nextRightElement); // 防止转折点是目标
                            return targetElement;
                        }
                    }
                }
                if (j > 0) {
                    GameElement nextElement = elements[x, j - 1];
                    if (nextElement.elementType != GameElementType.Empty) {
                        if (element.elementType == GameElementType.Empty) {
                            return element;
                        }
                        return elements[x, y];
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
            // this._game.gameController.LogInfo("PlayDropBackAnimation: " + string.Format("{0}/{1}", target.x, target.y));
            element.elementView.PlayAnimation(GameElementAnimation.DropBack, null);
        }

        private float GetFillTime(int initX, int initY, int targetX, int targetY, List<GameElement> paths) {
            if (paths.Count <= 0) {
                return (initY - targetY) * this._game.fillTime;
            }
            int preX = initX, preY = initY;
            float fillTime = 0f;
            for (int i = 0; i < paths.Count; i++) {
                var e = paths[i];
                if (preX == e.x) {
                    fillTime += (preY - e.y) * this._game.fillTime;
                } else if (preX != e.x) {
                    int dy = Math.Abs(preY - e.y);
                    fillTime += Math.Abs(preX - e.x) * this._game.fillTime + (dy - 1) * this._game.fillTime;
                }
                preX = e.x; preY = e.y;
            }
            fillTime += (preY - targetY) * this._game.fillTime;
            return fillTime;
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
            this._crossElements.Clear();
        }
    }
}

