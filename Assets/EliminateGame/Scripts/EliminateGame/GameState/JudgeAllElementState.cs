using System.Collections;
using System.Collections.Generic;

namespace EGame.Core
{
    // 判定所有元素状态
    public class JudgeAllElementState : IState
    {
        public string name { get { return "JudgeAllElementState"; } }
        private FSM _fsm = null;
        private EliminateGame _game = null;
        private float _timeout = 0;
        private Callback _timeoutCallback = null;
        List<JudgeResult> _judges = new List<JudgeResult>();

        public JudgeAllElementState(FSM fsm, EliminateGame game) {
            this._fsm = fsm;
            this._game = game;
        }

        public void Enter()
        {
            this.CheckElements();
        }

        private void CheckElements() {
            int cols = this._game.elementCols;
            int rows = this._game.elementRows;
            GameElement[,] elements = this._game.gameElements;
            this._judges.Clear();
            for (int i = 0; i < cols; i++) {
                for (int j = 0; j < rows; j++) {
                    var e = elements[i, j];
                    JudgeResult judgeResult = this._game.judgeSystem.StartJudge(e.elementView, null, JudgeType.System);
                    if (judgeResult != null) {
                        this._judges.Add(judgeResult);
                    }
                }
            }
            if (this._judges.Count > 0) {
                var exeJudgeState = new ExeJudgeState(this._fsm, this._game);
                exeJudgeState.SetData(this._judges.ToArray(), ExeJudgeFrom.Judge);
                this._fsm.ChangeState(exeJudgeState);
            } else {
                this._fsm.ChangeState(new ExchangeElementState(this._fsm, this._game));
            }
        }

        private void SetTimeout(float time, Callback callback) {
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
            this._judges.Clear();
        }
    }   
}
