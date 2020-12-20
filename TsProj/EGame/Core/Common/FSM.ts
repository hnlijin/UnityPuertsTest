export interface IState
{
    name: string;
    Enter(): void;
    Update(deltaTime: number): void;
    Exit(): void
}

export class FSM
{
    private _preState: IState = null;
    private _currState: IState = null;
    private _nextState: IState = null;
    private _changeCallback: Function = null;

    public get hasNextState(): boolean { return this._nextState != null; }

    public SetChangeCallback(callback: Function) {
        this._changeCallback = callback;
    }

    public ChangeState(state: IState) {
        if (this._currState != null && this._currState == state) {
            return;
        }
        if (this._preState!=null) {
            this._preState.Exit();
        }
        this._preState = this._currState;
        this._currState = state;
        if (this._changeCallback instanceof Function) {
            this._changeCallback(this._preState, this._currState);
        }
        this._currState.Enter();
    }

    public SetNextState(state: IState): void {
        this._nextState = state;
    }

    public NextState(): void {
        if (this._nextState != null) {
            this.ChangeState(this._nextState);
            this._nextState = null;
        }
    }

    public Update(deltaTime: number): void {
        if (this._currState != null) {
            this._currState.Update(deltaTime);
        }
    }

    public Destory(): void {
    }
}   