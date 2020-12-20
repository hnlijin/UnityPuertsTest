using System.Collections;
using System.Collections.Generic;
using System;

namespace EGame.Core
{
    public class CreateBarrierState : IState
    {
        private FSM _fsm = null;
        private EliminateGame _game = null;

        public CreateBarrierState(FSM fsm, EliminateGame game) {
            this._fsm = fsm;
            this._game = game;
        }

        public void Enter()
        {
            Random random = new Random();
            int barrierCount = 10;
            int cols = this._game.elementCols;
            int rows = this._game.elementRows;
            GameElement[,] elements = this._game.gameElements;
            for (int i = 0; i < barrierCount; i++)
            {
                int x = random.Next(0, cols);
                int y = random.Next(0, rows);
                var element = elements[x, y];
                var view = this._game.gameController.CreateGameElementView(x, y, GameElementType.Barrier);
                element.Init(x, y, GameElementType.Barrier);
                element.elementView = view;
            }
            this._game.OnCreateBarrierComplete();
        }

        public void Update(float deltaTime)
        {
            
        }

        public void Exit()
        {

        }
    }   
}
