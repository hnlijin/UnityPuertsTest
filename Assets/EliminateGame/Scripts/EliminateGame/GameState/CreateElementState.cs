using System.Collections;
using System.Collections.Generic;
using System;

namespace EGame.Core
{
    public class CreateElementState : IState
    {
        private FSM _fsm;
        private EliminateGame _game;
        private bool _startFill = false;
        private float _frameTime = 0.1f;

        public CreateElementState(FSM fsm, EliminateGame game) {
            this._fsm = fsm;
            this._game = game;
        }

        public void Enter()
        {
            this._startFill = true;
        }

        private bool FillElement() 
        {
            int cols = this._game.elementCols;
            int rows = this._game.elementRows;
            GameElement[,] elements = this._game.gameElements;
            bool filledNotFinished = false; // 本次填充是否完成,是否满足填充条件
            for (int y = 1; y < rows; y++)
            {
                for (int x = 0; x < cols; x++)
                {
                    GameElement element = elements[x, y]; // 得到甜品的虚拟坐标 
                    if (element.CanMove()) // 可以移动(即当前是普通甜品)，则往下填充
                    {
                        // 当前元素的下一行同列元素
                        GameElement nextRowElement = elements[x, y - 1];
                        // 当前元素的下一行左/右列元素, 
                        GameElement nextRowElement_Left = null;
                        GameElement nextRowElement_Right = null;
                        // 当前甜品的同行左/右侧
                        GameElement sweetLeft = null;
                        GameElement sweetRight = null;
                        // 特殊情况最右侧无右侧，最左侧无左侧
                        // 第0列~倒数第2列
                        if (x >= 0 && x < cols - 1) {
                            nextRowElement_Right = elements[x + 1, y - 1];
                            sweetRight = elements[x + 1, y];
                        }
                        // 第1列~倒数第1列
                        if (x >= 1 && x < cols) {
                            nextRowElement_Left = elements[x - 1, y - 1];
                            sweetLeft = elements[x - 1, y];
                        }
                        // 当前甜品的下一行同列甜品 是否为空  (垂直填充)
                        if (nextRowElement.elementType == GameElementType.Empty)
                        {
                            // 当前甜品往下移动
                            this._game.gameController.ReplaceGameElementView(x, y, x, y - 1, true);
                            element.MoveElement(x, y - 1, this._game.fillTime);
                            var tempElement = elements[x, y - 1];
                            this._game.gameController.ReplaceGameElementView(x, y - 1, x, y);
                            // 修改位置信息
                            elements[x, y - 1] = element;
                            // 原来甜品位置 为空
                            tempElement.Init(x, y, GameElementType.Empty);
                            elements[x, y] = tempElement;
                            filledNotFinished = true;
                        }
                        // 右斜向填充   存在右下方甜品  且当前甜品右下方甜品类型为空  且当前甜品右侧为障碍
                        else if (nextRowElement_Right != null && nextRowElement_Right.elementType == GameElementType.Empty 
                        && sweetRight.elementType == GameElementType.Barrier)
                        {
                            // 当前甜品往右下移动
                            this._game.gameController.ReplaceGameElementView(x, y, x + 1, y - 1, true);
                            element.MoveElement(x + 1, y - 1, this._game.fillTime);
                            var tempElement = elements[x + 1, y - 1];
                            this._game.gameController.ReplaceGameElementView(x + 1, y - 1, x, y);
                            // 修改位置信息
                            elements[x + 1, y - 1] = element;
                            // 原来甜品位置 为空
                            tempElement.Init(x, y, GameElementType.Empty);
                            elements[x, y] = tempElement;
                            filledNotFinished = true;
                        }
                        // 左斜向填充  存在左下方甜品  且当前甜品左下方甜品类型为空  且当前甜品左侧为障碍
                        else if (nextRowElement_Left != null && nextRowElement_Left.elementType == GameElementType.Empty 
                        && sweetLeft.elementType == GameElementType.Barrier)
                        {
                            // 当前甜品往左下移动
                            this._game.gameController.ReplaceGameElementView(x, y, x - 1, y - 1, true);
                            element.MoveElement(x - 1, y - 1, this._game.fillTime);
                            var tempElement = elements[x - 1, y - 1];
                            this._game.gameController.ReplaceGameElementView(x - 1, y - 1, x, y);
                            // 修改位置信息
                            elements[x - 1, y - 1] = element;
                            // 原来甜品位置 为空
                             tempElement.Init(x, y, GameElementType.Empty);
                            elements[x, y] = tempElement;
                            filledNotFinished = true;
                        }
                    }
                }
            }
            for (int i = 0; i < cols; i++)  // 最上面一行
            {
                // 游戏场景的第一行
                int x = i, y = rows - 1;
                GameElement element = elements[x, y];
                if (element.elementType == GameElementType.Empty)
                {
                    // 在游戏场景的第一行的上一行, 创建元素
                    var view = this._game.gameController.CreateGameElementView(x, y, GameElementType.Normal);
                    element.elementView = view;
                    // 配置元素属性
                    element.Init(x, y, GameElementType.Normal);
                    element.MoveElement(x, y, this._game.fillTime);
                    if (element.elementView.imageView != null) {
                        element.elementView.SetImageView(x, y);
                    }
                    filledNotFinished = true;
                }
            }
            return filledNotFinished;
        }

        public void Update(float deltaTime)
        {
            if (this._startFill == true) {
                this._frameTime += deltaTime;
                if (this._frameTime >= 0.15f) {
                    this._frameTime = 0;
                    if (!this.FillElement()) {
                        this._startFill = false;
                    }
                }
            }
        }

        public void Exit() {
            this._startFill = false;
            this._frameTime = 0f;
        }
    }
}

