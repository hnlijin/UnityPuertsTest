using System.Collections;
using System.Collections.Generic;

namespace EGame.Core
{
    public interface IEliminateGameController
    {
        IGameElementView CreateGameElementView(int x, int y, GameElementType elementType);
        void PressedElement(IGameElementView elementView);
        void EnterElement(IGameElementView elementView);
        void ReleaseElement(IGameElementView elementView);
        
        void LogInfo(string info);
        void LogWarn(string warn);
        void LogError(string error);
    }

    public delegate void Callback();
    public delegate void PressedElement(IGameElementView elementView);
    public delegate void EnterElement(IGameElementView elementView);
    public delegate void ReleaseElement(IGameElementView elementView);

    public class EliminateGame
    {
        public PressedElement onPressedElement = null;
        public EnterElement onEnterElement = null;
        public ReleaseElement onReleaseElement = null;

        private int _elementRows = 8;
        private int _elementCols = 8;
        public float fillTime = 0.1f;
        private FSM _fsm = null;
        private GameElement[,] _grids = null;
        private GameElement[,] _elements = null;
        private IEliminateGameController _gameController = null;        
        private JudgeSystem _judgeSystem = null;
        private Timer _timer = null;

        public GameElement[,] gameElements { get { return this._elements; } }
        public int elementCols { get { return this._elementCols; } }
        public int elementRows { get { return this._elementRows; } }
        public IEliminateGameController gameController { get { return this._gameController; } }
        public FSM fsm { get { return this._fsm; } }
        public JudgeSystem judgeSystem { get { return this._judgeSystem; } }

        public bool enableSyncFill { set; get; } // 开始同步填充：新生和移动同步下落
        
        public EliminateGame() {
            this._timer = new Timer();
            this._fsm = new FSM();
            this._judgeSystem = new JudgeSystem(this);
            this._judgeSystem.RegJudgeRule(new SameColorXiaoJudgeRule(this._judgeSystem));
            this._judgeSystem.RegJudgeRule(new HangOrLiexiaoJudgeRule(this._judgeSystem));
            this._judgeSystem.RegJudgeRule(new WulianxiaoJudgeRule(this._judgeSystem));
            this._judgeSystem.RegJudgeRule(new SilianxiaoJudgeRule(this._judgeSystem));
            this._judgeSystem.RegJudgeRule(new SanlianxiaoJudgeRule(this._judgeSystem));
        }

        public void SetGameElements(GameElement[,] elements) {
            this._elements = elements;
        }

        public void SetGrids(GameElement[,] grids) {
            this._grids = grids;
        }

        public void OnFillBarrierComplete() {
            this._fsm.ChangeState(new FillElementState(this._fsm, this));
        }

        public void OnFillElementComplete() {
            this.enableSyncFill = false;
            this._fsm.SetNextState(new ExchangeElementState(this._fsm, this));
            this._fsm.ChangeState(new JudgeAllElementState(this._fsm, this));
        }

        public void Init(int elementRows, int elementCols, IEliminateGameController gameController) {
            this._elementRows = elementRows;
            this._elementCols = elementCols;
            this._gameController = gameController;
            this.enableSyncFill = true;
            this._fsm.SetNextState(new FillBarrierState(this._fsm, this));
            // this._fsm.SetNextState(new FillElementState(this._fsm, this));
            this._fsm.ChangeState(new InitState(this._fsm, this));
        }

        public void Reset() {
            this._fsm.SetNextState(new FillElementState(this._fsm, this));
            this._fsm.ChangeState(new ResetState(this._fsm, this));
        }

        public void PressedElement(IGameElementView element) {
            if (this.onPressedElement != null) {
                this.onPressedElement(element);
            }
        }

        public void EnterElement(IGameElementView element)
        {
            if (this.onEnterElement != null) {
                this.onEnterElement(element);
            }
        }

        public void ReleaseElement(IGameElementView element)
        {
            if (this.onReleaseElement != null) {
                this.onReleaseElement(element);
            }
        }

        public void SetTimeout(float time, TimeoutCallback callback) {
            this._timer.SetTimeout(time, callback);
        }

        public void ClearTimeout(TimeoutCallback callback) {
            this._timer.ClearTimeout(callback);
        }

        public void Start() {
        }

        public void Update(float deltaTime) {
            this._timer.Update(deltaTime);
            this._fsm.Update(deltaTime);
        }

        public void Destory() {
            this._fsm.Destory();
        }
    }
}