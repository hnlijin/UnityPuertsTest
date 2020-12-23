using System.Collections;
using System.Collections.Generic;
using System;

namespace EGame.Core
{
    // 交换元素状态
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
        private float _timeout = 0;
        private TimeoutCallback _timeoutCallback = null;

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
                this.CheckSelectElementError();
                if (this.JudgeAround(this._pressedElement, this._enterElement)) {
                    this.ExchangeElement();
                } else {
                    this.ResetMoveElementState();
                }
            }
        }

        private void CheckSelectElementError() {
            GameElement[,] elements = this._game.gameElements;
            var element1 = elements[this._pressedElement.x, this._pressedElement.y];
            var element2 = elements[this._enterElement.x, this._enterElement.y];
            if (element1.x != this._pressedElement.x || element1.y != this._pressedElement.y) {
                this._game.gameController.LogInfo("PressedElement: " + string.Format(", P: {0}/{1}", this._pressedElement.x, this._pressedElement.y));
                this._game.gameController.LogError("Element: Pos Error, " + string.Format(", P: {0}/{1}", element2.x, element2.y));
            }
            if (element2.x != this._enterElement.x || element2.y != this._enterElement.y) {
                this._game.gameController.LogInfo("EnterElement: " + string.Format(", P: {0}/{1}", this._enterElement.x, this._enterElement.y));
                this._game.gameController.LogError("Element: Pos Error, " + string.Format(", P: {0}/{1}", element2.x, element2.y));
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
            IJudgeRule judge = this._game.judgeSystem.StartJudge(this._pressedElement, this._enterElement, JudgeType.Active);
            if (judge == null) {
                judge = this._game.judgeSystem.StartJudge(this._enterElement, this._pressedElement, JudgeType.Active);
            }
            if (judge == null) {
                this.RevertElement();
            } else {
                this._fsm.ChangeState(new ExeJudgeState(this._fsm, this._game, judge, ExeJudgeFrom.Exchange));
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

        private void SetTimeout(float time, TimeoutCallback callback) {
            if (this._timeoutCallback != null || this._timeout > 0) return;
            this._timeout = time;
            this._timeoutCallback = callback;
        }

        public void Update(float deltaTime)
        {
            if (this._timeout > 0) {
                this._timeout -= deltaTime;
                if (this._timeout <= 0) {
                    if (this._timeoutCallback != null) {
                        var callback = this._timeoutCallback;
                        this._timeoutCallback = null;
                        callback();
                    }
                }
            }
        }

        public void Exit() {
            this._element1X = this._element1Y = this._element2X = this._element2Y = 0;
            this._game.onPressedElement -= this.OnPressedElement;
            this._game.onEnterElement -= this.OnEnterElement;
            this._game.onReleaseElement -= this.OnReleaseElement;
            this._timeout = 0;
            this._timeoutCallback = null;
        }
    }
}
