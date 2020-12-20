using System.Collections;
using System.Collections.Generic;

namespace EGame.Core
{
    public enum GameElementType
    {
        Grid = -1, // 空格子，只用于格子背景
        Empty,
        Normal, // 普通元素: 可以移动
        Barrier, // 障碍元素: 不可移动
        Any, // 任意元素: 可整行、列匹配
        Same, // 同色元素: 可消除同一种颜色所有甜点
    }

    public interface IGameElementImageView {
        int imageId { set; get; }
        void RemoveImage();
    }

    public interface IGameElementView {
        void MoveElement(int targetX, int targetY, float time);
        IGameElementImageView imageView { set; get; }
        void SetImageView(int x, int y);
        void DestroyView();
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
            this._elementType == GameElementType.Grid) {
                return false;
            }
            return true;
        }

        public void MoveElement(int targetX, int targetY, float time) {
            this._x = targetX;
            this._y = targetY;
            if (this.CanMove()) {
                this._elementView.MoveElement(targetX, targetY, time);
            }
        }
    }
}