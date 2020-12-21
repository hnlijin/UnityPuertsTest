using System.Collections;
using System.Collections.Generic;

namespace EGame.Core
{
    public class ExchangeElementState : IState
    {
        private FSM _fsm = null;
        private EliminateGame _game = null;
        private int _element1X = 0;
        private int _element1Y = 0;
        private int _element2X = 0;
        private int _element2Y = 0;
        public string name { get { return "ExchangeElementState"; } }

        public ExchangeElementState(FSM fsm, EliminateGame game) {
            this._fsm = fsm;
            this._game = game;
        }

        public void Enter()
        {
            var pressedElement = this._game.pressedElement;
            var enterElement = this._game.enterElement;
            if (this._game.JudgeAround(pressedElement, enterElement)) {
                GameElement[,] elements = this._game.gameElements;
                var element1 = elements[pressedElement.x, pressedElement.y];
                var element2 = elements[enterElement.x, enterElement.y];
                if (element1.CanMove() && element2.CanMove()) {
                    this._element1X = element1.x; this._element1Y = element1.y;
                    this._element2X = element2.x; this._element2Y = element2.y;
                    element1.MoveElement(this._element2X, this._element2Y, this._game.fillTime, this.PressedElementMoveEnd);
                    element2.MoveElement(this._element1X, this._element1Y, this._game.fillTime, null);
                    elements[element1.x, element1.y] = element1;
                    elements[element2.x, element2.y] = element2;
                    return;
                }
            }
            this._game.ResetMoveElementState();
        }

        private void PressedElementMoveEnd(IGameElementView view) {
            IJudgeRule judge = this._game.judgeSystem.StartJudge();
            if (judge == null) {
                this.RevertElement();
            } else {
                this._game.SetInitState(false);
                this._game.ResetMoveElementState();
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

        private void RevertElementMoveEnd(IGameElementView view) {
            this._game.ResetMoveElementState();
        }

        public void Update(float deltaTime)
        {
            
        }

        public void Exit() {
            this._element1X = this._element1Y = this._element2X = this._element2Y = 0;
        }
    }
}
