export enum GameElementType
{
    Grid = -1, // 空格子，只用于格子背景
    Empty,
    Normal, // 普通元素: 可以移动
    Barrier, // 障碍元素: 不可移动
    Any, // 任意元素: 可整行、列匹配
    Same, // 同色元素: 可消除同一种颜色所有甜点
}

export interface IGameElementImageView {
    imageId: number;
    RemoveImage(): void;
}

export interface IGameElementView {
    MoveElement(targetX: number, targetY: number, time: number): void;
    imageView: IGameElementImageView;
    SetImageView(x: number, y: number): void;
    DestroyView(): void;
}

export class GameElement
{
    private _x: number;
    private _y: number;
    private _elementType: GameElementType; 
    private _elementView: IGameElementView;

    public get x() { return this._x; }
    public get y() { return this._y; }
    public get elementType(): GameElementType { return this._elementType; }
    public get elementView() { return this._elementView; }

    public Init(x: number, y: number, elementType: GameElementType): void {
        this._x = x;
        this._y = y;
        this._elementType = elementType;
    }

    public set elementView(value: IGameElementView) {
        if (this._elementView != null && this._elementView != value) {
            this._elementView.DestroyView();
        }
        this._elementView = value;
    }

    public CanMove(): boolean {
        if (this._elementType == GameElementType.Barrier || 
        this._elementType == GameElementType.Empty || 
        this._elementType == GameElementType.Grid) {
            return false;
        }
        return true;
    }

    public MoveElement(targetX: number, targetY: number, time: number) {
        this._x = targetX;
        this._y = targetY;
        if (this.CanMove()) {
            this._elementView.MoveElement(targetX, targetY, time);
        }
    }
}