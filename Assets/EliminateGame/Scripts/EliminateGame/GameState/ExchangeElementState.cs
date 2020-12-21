using System.Collections;
using System.Collections.Generic;
using System;

namespace EGame.Core
{
    public class ExchangeElementState : IState
    {
        public string name { get { return "ExchangeElementState"; } }
        private FSM _fsm = null;
        private EliminateGame _game = null;
        private int _element1X = 0, _element1Y = 0;
        private int _element2X = 0, _element2Y = 0;
        private bool _nowCanMoveElement = true;          // 元素是否可以滑动
        private IGameElementView _pressedElement = null; // 按下元素
        private IGameElementView _enterElement = null;   // 滑入元素

        public ExchangeElementState(FSM fsm, EliminateGame game) {
            this._fsm = fsm;
            this._game = game;
        }

        public void Enter()
        {
            this._game.onPressedElement += this.OnPressedElement;
            this._game.onEnterElement += this.OnEnterElement;
            this._game.onReleaseElement += this.OnReleaseElement;
        }

        private void OnPressedElement(IGameElementView element) {
            if (this._nowCanMoveElement == false) return;
            this._pressedElement = element;
            this._enterElement = null;
        }

        private void OnEnterElement(IGameElementView element)
        {
            if (this._nowCanMoveElement == false || this._pressedElement == null) return;
            this._enterElement = element;
        }

        private void OnReleaseElement(IGameElementView element)
        {
            if (this._pressedElement != null && this._enterElement != null && this._nowCanMoveElement == true) {
                this._nowCanMoveElement = false;
                this._game.gameController.LogInfo("PressedElement: " + string.Format(", P: {0}/{1}", this._pressedElement.x, this._pressedElement.y));
                this._game.gameController.LogInfo("EnterElement: " + string.Format(", P: {0}/{1}", this._enterElement.x, this._enterElement.y));
                if (this.JudgeAround(this._pressedElement, this._enterElement)) {
                    this.ExchangeElement();
                } else {
                    this.ResetMoveElementState();
                }
            }
        }

        private void ExchangeElement() {
            GameElement[,] elements = this._game.gameElements;
            var element1 = elements[this._pressedElement.x, this._pressedElement.y];
            var element2 = elements[this._enterElement.x, this._enterElement.y];
            if (element1.CanMove() && element2.CanMove()) {
                this._element1X = element1.x; this._element1Y = element1.y;
                this._element2X = element2.x; this._element2Y = element2.y;
                element1.MoveElement(this._element2X, this._element2Y, this._game.fillTime, this.PressedElementMoveEnd);
                element2.MoveElement(this._element1X, this._element1Y, this._game.fillTime, null);
                elements[element1.x, element1.y] = element1;
                elements[element2.x, element2.y] = element2;
                return;
            }
            this.ResetMoveElementState();
        }

        private void PressedElementMoveEnd(IGameElementView view) {
            IJudgeRule judge = this._game.judgeSystem.StartJudge(this._pressedElement);
            if (judge == null) {
                this.RevertElement();
            } else {
                this.ResetMoveElementState();
                GameElement[] clearElements = judge.GetClearElements();
                this.ClearElement(clearElements);
                if (judge.newElementType > GameElementType.Null) {
                    GameElement[,] elements = this._game.gameElements;
                    var element1 = elements[this._element2X, this._element2Y];
                    element1.Init(element1.x, element1.y, judge.newElementType);
                    if (judge.newElementType == GameElementType.Normal) {
                        element1.elementView.CreateImageView(element1.x, element1.y);
                    }
                }
                this._fsm.ChangeState(new FillElementState(this._fsm, this._game));
            }
        }

        private void ClearElement(GameElement[] clearElements) {
            if (clearElements == null) return;
            for (int i = 0;  i < clearElements.Length; i++) {
                GameElement e = clearElements[i];
                e.Init(e.x, e.y, GameElementType.Empty);
                e.elementView = this._game.gameController.CreateGameElementView(e.x, e.y, GameElementType.Empty);
            }
        }

        private void RevertElement() {
            GameElement[,] elements = this._game.gameElements;
            var element1 = elements[this._element2X, this._element2Y];
            var element2 = elements[this._element1X, this._element1Y];
            element1.MoveElement(this._element1X, this._element1Y, this._game.fillTime * 2f, this.RevertElementMoveEnd);
            elements[element1.x, element1.y] = element1;
            element2.MoveElement(this._element2X, this._element2Y, this._game.fillTime * 2f, null);
            elements[element2.x, element2.y] = element2;
        }

        /** 判断两个元素是否相邻 */
        private bool JudgeAround(IGameElementView element1, IGameElementView element2)
        {
            bool result = false;
            if (element1.x == element2.x && Math.Abs(element1.y - element2.y) == 1)
                result = true;
            else if (element1.y == element2.y && Math.Abs(element1.x - element2.x) == 1)
                result = true;
            return result;
        }

        private void RevertElementMoveEnd(IGameElementView view) {
            this.ResetMoveElementState();
        }

        private void ResetMoveElementState() {
            this._pressedElement = null;
            this._enterElement = null;
            this._nowCanMoveElement = true;
        }

        public void Update(float deltaTime)
        {
            
        }

        public void Exit() {
            this._element1X = this._element1Y = this._element2X = this._element2Y = 0;
            this._game.onPressedElement -= this.OnPressedElement;
            this._game.onEnterElement -= this.OnEnterElement;
            this._game.onReleaseElement -= this.OnReleaseElement;
        }
    }
}
