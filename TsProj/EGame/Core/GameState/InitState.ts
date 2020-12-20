import { FSM, IState } from "../Common/FSM";
import { EliminateGame } from "../EliminateGame";
import { GameElement, GameElementType } from "../GameElement/GameElement";

export class InitState implements IState
{
    public name: string = "InitState";
    private _fsm: FSM = null;
    private _game: EliminateGame = null;

    public constructor(fsm: FSM, game: EliminateGame) {
        this._fsm = fsm;
        this._game = game;
    }

    public Enter()
    {
        //初始化网格，甜品实例
        let cols = this._game.elementCols;
        let rows = this._game.elementRows;
        let elements: Array<Array<GameElement>> = new Array<Array<GameElement>>();
        let grids: Array<Array<GameElement>> = new Array<Array<GameElement>>();
        for (let i = 0; i < cols; i++) {
            elements[i] = Array<GameElement>();
            grids[i] = Array<GameElement>();
            for (let j = 0; j < rows; j++) {
                var element = new GameElement();
                var view = this._game.gameController.CreateGameElementView(i, j, GameElementType.Empty);
                element.Init(i, j, GameElementType.Empty);
                element.elementView = view;
                elements[i][j] = element;

                var grid = new GameElement();
                grid.Init(i, j, GameElementType.Grid);
                var gridView = this._game.gameController.CreateGameElementView(i, j, GameElementType.Grid);
                grid.elementView = gridView;
                grids[i][j] = grid;
            }
        }
        this._game.SetGameElements(elements);
        this._game.SetGrids(grids);
        this._fsm.NextState();
    }

    public Update(deltaTime: number)
    {
    }

    public Exit()
    {
    }
} 
