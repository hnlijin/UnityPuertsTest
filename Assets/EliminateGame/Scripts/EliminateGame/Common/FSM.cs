using System.Collections;
using System.Collections.Generic;

namespace EGame.Core
{
    public interface IState
    {
        void Enter();
        void Update(float deltaTime);
        void Exit();
    }

    public class FSM
    {
        private IState _preState = null;
        private IState _currState = null;
        private IState _nextState = null;

        public bool hasNextState {
            get { return this._nextState != null; }
        }

        public void ChangeState(IState state) {
            if (this._currState != null && this._currState == state) {
                return;
            }
            if (this._preState!=null) {
                this._preState.Exit();
            }
            this._preState = this._currState;
            this._currState = state;
            this._currState.Enter();
        }

        public void SetNextState(IState state) {
            this._nextState = state;
        }

        public void NextState() {
            if (this._nextState != null) {
                this.ChangeState(this._nextState);
                this._nextState = null;
            }
        }

        public void Update(float deltaTime) {
            if (this._currState != null) {
                this._currState.Update(deltaTime);
            }
        }

        public void Destory() {

        }
    }   
}
