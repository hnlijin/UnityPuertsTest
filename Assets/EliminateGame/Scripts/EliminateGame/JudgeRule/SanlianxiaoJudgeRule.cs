using System.Collections;
using System.Collections.Generic;

namespace EGame.Core
{
    // 垂直三连消
    public class SanlianxiaoJudgeRule : IJudgeRule
    {
        private JudgeSystem _system = null;
        private Direction _direction = Direction.Null;
        private int _selectedElementX = -1, _selectedElementY = -1;
        private GameElementType _selectedElementType = GameElementType.Null;
        public int selectedElementX { get { return this._selectedElementX; } }
        public int selectedElementY { get { return this._selectedElementY; } }
        public GameElementType selectedElementType { get { return this._selectedElementType; } }

        public SanlianxiaoJudgeRule(JudgeSystem system) {
            this._system = system;
        }

        public bool IsMathch() {
            this._direction = Direction.Null;
            this._selectedElementX = this._system.selectedElement.x;
            this._selectedElementY = this._system.selectedElement.y;
            this._selectedElementType = GameElementType.Null;
            if (this._system.leftSameImageList.Count + this._system.rightSameImageList.Count >= 2 && this._system.upSameImageList.Count + this._system.downSameImageList.Count >= 2) {
                this._selectedElementType = GameElementType.Normal;
                this._direction = Direction.Cross;
                return true;
            }
            if (this._system.leftSameImageList.Count + this._system.rightSameImageList.Count >= 2) {
                this._selectedElementType = GameElementType.Normal;
                this._direction = Direction.Horizontal;
                return true;
            }
            if (this._system.upSameImageList.Count + this._system.downSameImageList.Count >= 2) {
                this._selectedElementType = GameElementType.Normal;
                this._direction = Direction.Vertical;
                return true;
            }
            this._selectedElementX = -1;
            this._selectedElementY = -1;
            return false;
        }

        public ChangeElement[] GetChangeElements() {
            return null;
        }

        public NewElement[] GetNewElements() {
            return null;
        }

        public GameElement[] GetClearElements() {
            if (this._direction == Direction.Cross) {
                int count1 = this._system.leftSameImageList.Count + this._system.rightSameImageList.Count;
                int count2 = this._system.upSameImageList.Count + this._system.downSameImageList.Count;
                if (count1 >= 2 && count2 >= 2) {
                    GameElement[] clearElements = new GameElement[count1 + count2 + 1];
                    GameElement[,] elements = this._system.game.gameElements;
                    int count = this._system.leftSameImageList.Count + this._system.rightSameImageList.Count;
                    int index = 0;
                    clearElements[index] = elements[this._selectedElementX, this._selectedElementY];
                    index += 1;
                    for (int i = 0; i < this._system.leftSameImageList.Count; i++) {
                        clearElements[index] = this._system.leftSameImageList[i];
                        index += 1;
                    }
                    for (int j = 0; j < this._system.rightSameImageList.Count; j++) {
                        clearElements[index] = this._system.rightSameImageList[j];
                        index += 1;
                    }
                    for (int i = 0; i < this._system.upSameImageList.Count; i++) {
                        clearElements[index] = this._system.upSameImageList[i];
                        index += 1;
                    }
                    for (int j = 0; j < this._system.downSameImageList.Count; j++) {
                        clearElements[index] = this._system.downSameImageList[j];
                        index += 1;
                    }
                    return clearElements;
                }
            } else if (this._direction == Direction.Horizontal) {
                int count = this._system.leftSameImageList.Count + this._system.rightSameImageList.Count;
                if (count >= 2) {
                    GameElement[] clearElements = new GameElement[count + 1];
                    GameElement[,] elements = this._system.game.gameElements;
                    int index = 0;
                    clearElements[index] = elements[this._selectedElementX, this._selectedElementY];
                    index += 1;
                    for (int i = 0; i < this._system.leftSameImageList.Count; i++) {
                        clearElements[index] = this._system.leftSameImageList[i];
                        index += 1;
                    }
                    for (int j = 0; j < this._system.rightSameImageList.Count; j++) {
                        clearElements[index] = this._system.rightSameImageList[j];
                        index += 1;
                    }
                    return clearElements;
                }
            } else if (this._direction == Direction.Vertical) {
                int count = this._system.upSameImageList.Count + this._system.downSameImageList.Count;
                if (count >= 2) {
                    GameElement[] clearElements = new GameElement[count + 1];
                    GameElement[,] elements = this._system.game.gameElements;
                    int index = 0;
                    clearElements[index] = elements[this._selectedElementX, this._selectedElementY];
                    index += 1;
                    for (int i = 0; i < this._system.upSameImageList.Count; i++) {
                        clearElements[index] = this._system.upSameImageList[i];
                        index += 1;
                    }
                    for (int j = 0; j < this._system.downSameImageList.Count; j++) {
                        clearElements[index] = this._system.downSameImageList[j];
                        index += 1;
                    }
                    return clearElements;
                }
            }
            return null;
        }
    }
}

