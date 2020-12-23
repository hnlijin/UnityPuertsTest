using System.Collections;
using System.Collections.Generic;

namespace EGame.Core
{
    public enum ExeJudgeFrom
    {
        Null,
        Exchange,
        Judge
    }
    
    // 执行判定状态
    public class ExeJudgeState : IState
    {
        public string name { get { return "ExeJudgeState"; } }
        private FSM _fsm = null;
        private EliminateGame _game = null;
        private ExeJudgeFrom _from = ExeJudgeFrom.Null;
        private IJudgeRule _judge = null;
        private float _timeout = 0;
        private TimeoutCallback _timeoutCallback = null;

        public ExeJudgeState(FSM fsm, EliminateGame game, IJudgeRule judge, ExeJudgeFrom from) {
            this._fsm = fsm;
            this._game = game;
            this._judge = judge;
            this._from = from;
        }

        public void Enter()
        {
            GameElement[] clearElements = this._judge.GetClearElements();
            if (clearElements != null) {
                int len = clearElements.Length;
                int clearCount = 0;
                for (int i = 0; i < len; i++) {
                    GameElement e = clearElements[i];
                    if (e == null) {
                        clearCount += 1;
                        continue;
                    }
                    e.elementView.PlayAnimation(GameElementAnimation.Disappear, (IGameElementView view, string name) => {
                        this.ClearElement(e);
                        clearCount += 1;
                        if (clearCount == len) {
                            this.ExeNewElements();
                        }
                    });
                }
            } else {
                this.ExeNewElements();
            }
        }

        private void ExeNewElements() {
            NewElement[] newElements = this._judge.GetNewElements();
            if (newElements != null) {
                GameElement[,] elements = this._game.gameElements;
                for (int i = 0; i < newElements.Length; i++) {
                    GameElement element = newElements[i].oldElement;
                    GameElementType newElementType = newElements[i].newElementType;
                    element.Init(element.x, element.y, newElementType);
                    element.elementView = this._game.gameController.CreateGameElementView(element.x, element.y, newElementType);
                }
            }
            this.ExeJudgeEnd();
        }

        private void ExeJudgeEnd() {
            this._game.judgeSystem.ClearJudgeEnv();
            if (this._from == ExeJudgeFrom.Judge || this._from == ExeJudgeFrom.Exchange) {
                this.SetTimeout(0.1f, () => {
                    this._fsm.SetNextState(new JudgeAllElementState(this._fsm, this._game));
                    this._fsm.ChangeState(new FillElementState(this._fsm, this._game));
                }); 
            }
        }

        private void ClearElement(GameElement clearElement) {
            if (clearElement == null) return;
            clearElement.Init(clearElement.x, clearElement.y, GameElementType.Empty);
            clearElement.elementView = this._game.gameController.CreateGameElementView(clearElement.x, clearElement.y, GameElementType.Empty);
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

        }
    }   
}
