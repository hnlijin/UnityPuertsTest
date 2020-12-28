using System.Collections;
using System.Collections.Generic;

namespace EGame.Core
{
    public delegate void TimeoutCallback();

    public class TimerInfo {
        private float _time = 0;
        private float _totalTime = 0;
        private TimeoutCallback _callback = null;
        public TimeoutCallback callback { get { return this._callback; } }

        public void Init(float time, TimeoutCallback callback) { 
            this._time = time; 
            this._callback = callback; 
            this._totalTime = 0; 
        }
        public void Update(float deltaTime) {
            this._totalTime += deltaTime;
            if (this._totalTime >= this._time && this._callback != null) {
                var callback = this._callback;
                this._callback = null;
                callback();
            }
        }
        public bool IsCanClean() {
            return this._callback == null;
        }
        public void Reset() {
            this._time = 0;
            this._totalTime = 0;
            this._callback = null;
        }
    }

    public class Timer
    {
        private List<TimerInfo> _timeouts = new List<TimerInfo>();
        private bool _timeoutLock = false;
        private List<TimerInfo> _lockTimeouts = new List<TimerInfo>();
        private List<TimerInfo> _preDelTimeouts = new List<TimerInfo>();
        private List<TimeoutCallback> _preDelCallback = new List<TimeoutCallback>();
        private Queue<TimerInfo> _timerInfoPool = new Queue<TimerInfo>();

        public void SetTimeout(float time, TimeoutCallback callback) {
            if (callback == null) return;
            for (int i = 0; i < this._timeouts.Count; i++) {
                if(this._timeouts[i].callback == callback) return;
            }
            var timerInfo = this.CreateTimerInfo();
            timerInfo.Init(time, callback);
            if (this._timeoutLock == false) {
                this._timeouts.Add(timerInfo);
            } else {
                this._lockTimeouts.Add(timerInfo);
            }
        }

        public void ClearTimeout(TimeoutCallback callback) {
            if (callback == null) return;
            if (this._timeoutLock == false) {
                for (int i = 0; i < this._timeouts.Count; i++) {
                    if(this._timeouts[i].callback == callback) {
                        this._timeouts.Remove(this._timeouts[i]);
                        this.AddTimerInfo(this._timeouts[i]);
                        break;
                    }
                }
            } else {
                this._preDelCallback.Add(callback);
            }
        }

        public void Update(float deltaTime) {
            this._timeoutLock = true;
            for (int i = 0; i < this._timeouts.Count; i++) {
                var timerInfo = this._timeouts[i];
                timerInfo.Update(deltaTime);
                if (timerInfo.IsCanClean() == true){
                    this._preDelTimeouts.Add(timerInfo);
                }
            }
            for (int j = 0; j < this._preDelTimeouts.Count; j++) {
                var timerInfo = this._preDelTimeouts[j];
                this._timeouts.Remove(timerInfo);
                this.AddTimerInfo(timerInfo);
            }
            this._preDelTimeouts.Clear();
            this._timeouts.AddRange(this._lockTimeouts);
            this._lockTimeouts.Clear();
            this._timeoutLock = false;
            for (int j = 0; j < this._preDelCallback.Count; j++) {
                var callback = this._preDelCallback[j];
                this.ClearTimeout(callback);
            }
            this._preDelCallback.Clear();
        }

        private TimerInfo CreateTimerInfo() {
            if (this._timerInfoPool.Count > 0) {
                return this._timerInfoPool.Dequeue();
            }
            return new TimerInfo();
        }

        private void AddTimerInfo(TimerInfo timerInfo) {
            timerInfo.Reset();
            this._timerInfoPool.Enqueue(timerInfo);
        }
    }
}
