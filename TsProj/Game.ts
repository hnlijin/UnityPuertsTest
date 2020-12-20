import { EliminateGame, IEliminateGameController } from "./EGame/Core/EliminateGame";
import { GameElementType, IGameElementImageView, IGameElementView } from "./EGame/Core/GameElement/GameElement";
import {EliminateGameController, GameElementView, UnityEngine} from 'csharp'

class ElementView implements IGameElementView {
    private _x: number;
    private _y: number;
    private _elementType: number;
    private _view: GameElementView;
    public constructor(x: number, y: number, elementType: number, view: GameElementView) {
        this._x = x;
        this._y = y;
        this._elementType = elementType;
        this._view = view;
    }
    public MoveElement(targetX: number, targetY: number, time: number): void {
        this._view.MoveElement(targetX, targetY, time);
    }
    public imageView: IGameElementImageView;
    public SetImageView(x: number, y: number): void {
        this._view.SetImageView(x, y);
    }
    public DestroyView(): void {
        this._view.DestroyView();
    }
}

export class Game implements IEliminateGameController
{
    public game: EliminateGame = new EliminateGame();
    private _gameController: EliminateGameController = null;
    private _elementContainer: UnityEngine.GameObject = null;
    private _elementViews: any = {};

    public CreateGameElementView(x: number, y: number, elementType: GameElementType): IGameElementView
    {
        let gameView = this._gameController.CreateGameElementView(x, y, elementType);
        if (elementType == GameElementType.Grid) {
            return null;
        }
        var view = new ElementView(x, y, elementType, gameView);
        this._elementViews[x + "_" + y] = view;
        return view;
    }

    public ReplaceGameElementView(oldX: number, oldY: number, targetX: number, targetY: number, onlyUpdateName: boolean) {
        this._gameController.ReplaceGameElementView(oldX, oldY, targetX, targetY, onlyUpdateName);
    }
    
    public Awake() {
        this._elementContainer = UnityEngine.GameObject.Find("ElementContainer");
        this._gameController = this._elementContainer.GetComponent("EliminateGameController") as EliminateGameController;
        this.game.Init(5, 7, this);
    }

    public Start() {
        this.game.Start();
    }

    public Update(deltaTime: number) {
        this.game.Update(deltaTime);
    }
}
