import { EliminateGame, IEliminateGameController } from "./EGame/Core/EliminateGame";
import { GameElementType, IGameElementImageView, IGameElementView } from "./EGame/Core/GameElement/GameElement";
import {UnityEngine} from 'csharp'

class GameElementView implements IGameElementView {
    private _x: number;
    private _y: number;
    private _elementType: number;
    public constructor(x: number, y: number, elementType: number) {
        this._x = x;
        this._y = y;
        this._elementType = elementType;
    }
    public MoveElement(targetX: number, targetY: number, time: number): void {
    }
    public imageView: IGameElementImageView;
    public SetImageView(x: number, y: number): void {
    }
    public DestroyView(): void {
    }
}

export class EliminateGameController extends UnityEngine.MonoBehaviour implements IEliminateGameController
{
    public game: EliminateGame = new EliminateGame();
    private _gameController: UnityEngine.Component = null;
    private _elementContainer: UnityEngine.GameObject = null;
    private _elementViews: any = {};

    public CreateGameElementView(x: number, y: number, elementType: GameElementType): IGameElementView
    {
        // this._gameController.SendMessage("CreateGameElementView", x + "," + y + "," + elementType);
        // if (elementType == GameElementType.Grid) {
            return null;
        // }
        // var view = new GameElementView(x, y, elementType);
        // this._elementViews[x + "_" + y] = view;
        // return view;
    }

    public ReplaceGameElementView(oldX: number, oldY: number, targetX: number, targetY: number, onlyUpdateName: boolean = false) {

    }
    
    public Awake() {
        this.game.Init(5, 7, this);

        this._elementContainer = UnityEngine.GameObject.Find("ElementContainer");
        this._gameController = this._elementContainer.GetComponent("EliminateGameController");
    }

    public Start() {
        this.game.Start();
    }
}
