import {FSM, IState} from '../Common/FSM';
import {EliminateGame} from '../EliminateGame';
import { GameElementType } from '../GameElement/GameElement';

export class FillBarrierState implements IState
{
    public name: string = "FillBarrierState";
    private _fsm: FSM = null;
    private _game: EliminateGame = null;

    public constructor(fsm: FSM, game: EliminateGame) {
        this._fsm = fsm;
        this._game = game;
    }

    public Enter()
    {
        let barrierCount = 10;
        let cols = this._game.elementCols;
        let rows = this._game.elementRows;
        let elements = this._game.gameElements;
        for (let i = 0; i < barrierCount; i++)
        {
            let x = Math.floor(Math.random() * cols);
            let y = Math.floor(Math.random() * rows);
            let element = elements[x][y];
            let view = this._game.gameController.CreateGameElementView(x, y, GameElementType.Barrier);
            element.Init(x, y, GameElementType.Barrier);
            element.elementView = view;
        }
        this._game.OnCreateBarrierComplete();
    }

    public Update(deltaTime: number)
    {
    }

    public Exit()
    {
    }
}  
