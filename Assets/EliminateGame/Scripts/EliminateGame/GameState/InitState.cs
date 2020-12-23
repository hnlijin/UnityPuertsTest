using System.Collections;
using System.Collections.Generic;

namespace EGame.Core
{
    // 初始化网格和空元素状态
    public class InitState : IState
    {
        public string name { get { return "InitState"; } }
        private FSM _fsm = null;
        private EliminateGame _game = null;

        public InitState(FSM fsm, EliminateGame game) {
            this._fsm = fsm;
            this._game = game;
        }

        public void Enter()
        {
            //初始化网格，甜品实例
            int cols = this._game.elementCols;
            int rows = this._game.elementRows;
            GameElement[,] elements = new GameElement[cols, rows];
            GameElement[,] grids = new GameElement[cols, rows];
            for (int i = 0; i < cols; i++) {
                for (int j = 0; j < rows; j++) {
                    var element = new GameElement();
                    var view = this._game.gameController.CreateGameElementView(i, j, GameElementType.Empty);
                    element.Init(i, j, GameElementType.Empty);
                    element.elementView = view;
                    elements[i, j] = element;

                    var grid = new GameElement();
                    grid.Init(i, j, GameElementType.Grid);
                    var gridView = this._game.gameController.CreateGameElementView(i, j, GameElementType.Grid);
                    grid.elementView = gridView;
                    grids[i, j] = grid;
                }
            }
            this._game.SetGameElements(elements);
            this._game.SetGrids(grids);
            this._fsm.NextState();
        }

        public void Update(float deltaTime)
        {
            
        }

        public void Exit() {

        }
    }   
}
