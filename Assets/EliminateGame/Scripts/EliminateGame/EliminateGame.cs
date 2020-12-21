using System;
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
        void LogErrr(string error);
    }

    public class EliminateGame
    {
        private int _elementRows = 8;
        private int _elementCols = 8;
        public float fillTime = 0.1f;
        private FSM _fsm = null;
        private GameElement[,] _grids = null;
        private GameElement[,] _elements = null;
        private IEliminateGameController _gameController = null;
        private bool _initComplete = false;              // 初始化是否完成
        private bool _nowCanMoveElement = true;          // 元素是否可以滑动
        private IGameElementView _pressedElement = null; // 按下元素
        private IGameElementView _enterElement = null;   // 滑入元素
        private JudgeSystem _judgeSystem = null;
        
        public EliminateGame() {
            this._fsm = new FSM();
            this._judgeSystem = new JudgeSystem(this);
        }

        public GameElement[,] gameElements { get { return this._elements; } }
        public int elementCols { get { return this._elementCols; } }
        public int elementRows { get { return this._elementRows; } }
        public IEliminateGameController gameController { get { return this._gameController; } }
        public IGameElementView pressedElement { get { return this._pressedElement; } }
        public IGameElementView enterElement { get { return this._enterElement; } }
        public JudgeSystem judgeSystem { get { return this._judgeSystem; } }
        public void SetInitState(bool value) { this._initComplete = value; }

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
            this._initComplete = true;
        }

        public void Init(int elementRows, int elementCols, IEliminateGameController gameController) {
            this._elementRows = elementRows;
            this._elementCols = elementCols;
            this._gameController = gameController;
            this._fsm.SetNextState(new FillBarrierState(this._fsm, this));
            this._fsm.ChangeState(new InitState(this._fsm, this));
        }

        public void PressedElement(IGameElementView element) {
            if (this._initComplete == false || this._nowCanMoveElement == false) return;
            this._pressedElement = element;
            this._enterElement = null;
        }

        public void EnterElement(IGameElementView element)
        {
            if (this._initComplete == false || this._nowCanMoveElement == false || this._pressedElement == null) return;
            this._enterElement = element;
        }

        public void ReleaseElement(IGameElementView element)
        {
            if (this._pressedElement != null && this._enterElement != null && this._nowCanMoveElement == true) {
                this._nowCanMoveElement = false;
                this._gameController.LogInfo("PressedElement: " + string.Format(", P: {0}/{1}", this._pressedElement.x, this._pressedElement.y));
                this._gameController.LogInfo("EnterElement: " + string.Format(", P: {0}/{1}", this._enterElement.x, this._enterElement.y));
                if (this.JudgeAround(this._pressedElement, this._enterElement)) {
                    this._fsm.ChangeState(new ExchangeElementState(this._fsm, this));
                } else {
                    this.ResetMoveElementState();
                }
            }
        }

        /** 判断两个元素是否相邻 */
        public bool JudgeAround(IGameElementView element1, IGameElementView element2)
        {
            bool result = false;
            if (element1.x == element2.x && Math.Abs(element1.y - element2.y) == 1)
                result = true;
            else if (element1.y == element2.y && Math.Abs(element1.x - element2.x) == 1)
                result = true;
            return result;
        }

        public void ResetMoveElementState() {
            this._pressedElement = null;
            this._enterElement = null;
            this._nowCanMoveElement = true;
        }

        public void Start() {
        }

        public void Update(float deltaTime) {
            this._fsm.Update(deltaTime);
        }

        public void Destory() {
            this._fsm.Destory();
        }
    }
}