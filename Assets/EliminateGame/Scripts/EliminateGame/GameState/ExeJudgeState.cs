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
        private JudgeResult[] _judgeResults = null;
        private float _timeout = 0;
        private Callback _timeoutCallback = null;
        private List<GameElement> allClearElements = new List<GameElement>();
        private List<NewElement> allNewElements = new List<NewElement>();

        public ExeJudgeState(FSM fsm, EliminateGame game) {
            this._fsm = fsm;
            this._game = game;
        }

        public void SetData(JudgeResult[] judgeResults, ExeJudgeFrom from) {
            this._judgeResults = judgeResults;
            this._from = from;
        }

        public void Enter()
        {
            allClearElements.Clear();
            for (int i = 0; i < this._judgeResults.Length; i++) {
                if (this._judgeResults[i] == null) continue;
                GameElement[] clearElements = this._judgeResults[i].clearElements;
                if (clearElements != null && clearElements.Length > 0) {
                    for (int j = 0; j < clearElements.Length; j++) {
                        if (!allClearElements.Contains(clearElements[j])) {
                            allClearElements.Add(clearElements[j]);
                        }
                    }
                }
            }
            if (allClearElements.Count >= 0) {
                int count = allClearElements.Count;
                int clearCount = 0;
                for (int i = 0; i < count; i++) {
                    GameElement e = allClearElements[i];
                    if (e == null) {
                        clearCount += 1;
                        continue;
                    }
                    e.elementView.PlayAnimation(GameElementAnimation.Disappear, (IGameElementView view, string name) => {
                        this.ClearElement(e);
                        clearCount += 1;
                        if (clearCount == count) {
                            this.ExeNewElements();
                        }
                    });
                }
            } else {
                this.ExeNewElements();
            }
        }

        private void ExeNewElements() {
            allNewElements.Clear();
            for (int i = 0; i < this._judgeResults.Length; i++) {
                if (this._judgeResults[i] == null) continue;
                NewElement[] newElements = this._judgeResults[i].newElements;
                if (newElements != null && newElements.Length > 0) {
                    for (int j = 0; j < newElements.Length; j++) {
                        if (!allNewElements.Contains(newElements[j])) {
                            allNewElements.Add(newElements[j]);
                        }
                    }
                }
            }
            if (allNewElements.Count > 0) {
                GameElement[,] elements = this._game.gameElements;
                for (int i = 0; i < allNewElements.Count; i++) {
                    GameElement element = allNewElements[i].oldElement;
                    GameElementType newElementType = allNewElements[i].newElementType;
                    element.Init(element.x, element.y, newElementType);
                    element.elementView = this._game.gameController.CreateGameElementView(element.x, element.y, newElementType);
                }
            }
            this.ExeJudgeEnd();
        }

        private void ExeJudgeEnd() {
            if (this._judgeResults.Length > 0) {
                for (int i = 0; i < this._judgeResults.Length; i++) {
                    var result = this._judgeResults[i];
                    this._game.judgeSystem.RecoveryJudgeResult(result);
                }
            }
            this._judgeResults = null;
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
            allClearElements.Clear();
            allNewElements.Clear();
        }
    }   
}
