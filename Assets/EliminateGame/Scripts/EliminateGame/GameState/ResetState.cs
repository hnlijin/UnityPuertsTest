using System.Collections;
using System.Collections.Generic;

namespace EGame.Core
{
    // 重置已有元素
    public class ResetState : IState
    {
        public string name { get { return "InitState"; } }
        private FSM _fsm = null;
        private EliminateGame _game = null;

        public ResetState(FSM fsm, EliminateGame game) {
            this._fsm = fsm;
            this._game = game;
        }

        public void Enter()
        {
            int cols = this._game.elementCols;
            int rows = this._game.elementRows;
            GameElement[,] elements = this._game.gameElements;
            for (int i = 0; i < cols; i++) {
                for (int j = 0; j < rows; j++) {
                    GameElement element = elements[i, j];
                    var view = this._game.gameController.CreateGameElementView(i, j, GameElementType.Empty);
                    element.Init(i, j, GameElementType.Empty);
                    element.elementView = view;
                }
            }
            this._fsm.NextState();
        }

        public void Update(float deltaTime)
        {
            
        }

        public void Exit() {

        }
    }   
}
