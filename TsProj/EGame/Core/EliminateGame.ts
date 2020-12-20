import {FSM} from "./Common/FSM";
import {GameElement, GameElementType, IGameElementView} from "./GameElement/GameElement";
import {FillBarrierState} from "./GameState/FillBarrierState";
import {FillElementState} from "./GameState/FillElementState";
import { InitState } from "./GameState/InitState";

export interface IEliminateGameController
{
    CreateGameElementView(x: number, y: number, elementType: GameElementType): IGameElementView;
    ReplaceGameElementView(oldX: number, oldY: number, targetX: number, targetY: number, onlyUpdateName: boolean): void;
}

export class EliminateGame
{
    public fillTime: number = 0.1;

    private _elementRows: number = 8;
    private _elementCols: number = 8;
    private _fsm: FSM = null;
    private _grids: Array<Array<GameElement>> = null;
    private _elements: Array<Array<GameElement>> = null;
    private _gameController: IEliminateGameController = null;

    public get gameElements(): Array<Array<GameElement>> { return this._elements; }
    public get elementCols(): number { return this._elementCols; }
    public get elementRows(): number { return this._elementRows; }
    public get gameController(): IEliminateGameController { return this._gameController; }
    
    public constructor() {
        this._fsm = new FSM();
    }

    public SetGameElements(elements: Array<Array<GameElement>>) {
        this._elements = elements;
    }

    public SetGrids(grids: Array<Array<GameElement>>) {
        this._grids = grids;
    }

    public OnCreateBarrierComplete() {
        this._fsm.ChangeState(new FillElementState(this._fsm, this));
    }

    public Init(elementRows: number, elementCols: number, gameController: IEliminateGameController): void {
        this._elementRows = elementRows;
        this._elementCols = elementCols;
        this._gameController = gameController;
        this._fsm.SetNextState(new FillBarrierState(this._fsm, this));
        this._fsm.ChangeState(new InitState(this._fsm, this));
    }

    public Start() {
    }

    public Update(deltaTime: number) {
        this._fsm.Update(deltaTime);
    }

    public Destory() {
        this._fsm.Destory();
    }
}