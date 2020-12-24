using System.Collections;
using System.Collections.Generic;

namespace EGame.Core
{
    public enum GameElementType
    {
        Null = -2, // 此类型为NULL, 用于消除后没有生成新的元素
        Grid = -1, // 空格子，只用于格子背景
        Empty,
        Normal, // 普通元素: 可以移动
        Barrier, // 障碍元素: 不可移动
        AnyRow, // 消除任意行元素: 可整行消除
        AnyColumn, // 消除任意列元素: 可整列消除
        Same, // 消除同色元素: 可消除同一种颜色的所有元素
    }

    public class GameElementAnimation
    {
        public static string DropBack = "dropBack"; // 下落回弹
        public static string Appear = "appear"; // 出现动画
        public static string Disappear = "disappear";  // 消失动画
    }

    public delegate void IElementMoveEndCallback(IGameElementView target);
    public delegate void IAnimationPlayCompleteCallback(IGameElementView target, string name);

    public interface IGameElementView {
        int x { get; }
        int y { get; }
        void Init(int x, int y);
        void MoveElement(int targetX, int targetY, float time, IElementMoveEndCallback callback, GameElement[] paths);
        void DestroyView();
        int imageId { set; get; }
        void CreateImageView(int x, int y);
        void DestroyImageView();
        void PlayAnimation(string name, IAnimationPlayCompleteCallback callback);
    }

    public class GameElement
    {
        private int _x;
        private int _y;
        private GameElementType _elementType; 
        private IGameElementView _elementView;

        public int x {
            get { return this._x; }
        }

        public int y {
            get { return this._y; }
        }

        public GameElementType elementType {
            get { return this._elementType; }
        }

        public IGameElementView elementView {
            set { 
                if (this._elementView != null && this._elementView != value) {
                    this._elementView.DestroyView();
                }
                this._elementView = value;
            }
            get { return this._elementView; }
        }

        public void Init(int x, int y, GameElementType elementType) {
            this._x = x;
            this._y = y;
            this._elementType = elementType;
        }

        public bool CanMove() {
            if (this._elementType == GameElementType.Barrier || 
            this._elementType == GameElementType.Empty || 
            this._elementType == GameElementType.Grid ||
            this._elementType == GameElementType.Null) {
                return false;
            }
            return true;
        }

        public void MoveElement(int targetX, int targetY, float time, IElementMoveEndCallback callback, GameElement[] paths) {
            this._x = targetX;
            this._y = targetY;
            if (this.CanMove()) {
                this._elementView.MoveElement(targetX, targetY, time, callback, paths);
            } else if (callback != null) {
                callback(null);
            }
        }
    }
}