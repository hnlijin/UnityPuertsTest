using System.Collections;
using System.Collections.Generic;

namespace EGame.Core
{
    public interface IEliminateGameController
    {
        IGameElementView CreateGameElementView(int x, int y, GameElementType elementType);
        void ReplaceGameElementView(int oldX, int oldY, int targetX, int targetY, bool onlyUpdateName = false);
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
        
        public EliminateGame() {
            this._fsm = new FSM();
        }

        public GameElement[,] gameElements {
            get { return this._elements; }
        }
        public int elementCols {
            get { return this._elementCols; } 
        }
        public int elementRows {
            get { return this._elementRows; }
        }
        public IEliminateGameController gameController {
            get { return this._gameController; }
        }

        public void SetGameElements(GameElement[,] elements) {
            this._elements = elements;
        }

        public void SetGrids(GameElement[,] grids) {
            this._grids = grids;
        }

        public void OnCreateBarrierComplete() {
            this._fsm.ChangeState(new FillElementState(this._fsm, this));
        }

        public void Init(int elementRows, int elementCols, IEliminateGameController gameController) {
            this._elementRows = elementRows;
            this._elementCols = elementCols;
            this._gameController = gameController;
            this._fsm.SetNextState(new FillBarrierState(this._fsm, this));
            this._fsm.ChangeState(new InitState(this._fsm, this));
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