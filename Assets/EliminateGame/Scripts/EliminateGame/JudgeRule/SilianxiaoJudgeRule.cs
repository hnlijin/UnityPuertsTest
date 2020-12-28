using System.Collections;
using System.Collections.Generic;

namespace EGame.Core
{
    // 四连消
    public class SilianxiaoJudgeRule : IJudgeRule
    {
        private JudgeSystem _system = null;

        public SilianxiaoJudgeRule(JudgeSystem system) {
            this._system = system;
        }

        public JudgeResult IsMathch() {
            if (this._system.leftSameImageList.Count + this._system.rightSameImageList.Count >= 3) {
                var result = this._system.CreateJudgeResult();
                result.direction = Direction.Horizontal;
                result.selectedElementX = this._system.selectedElement.x;
                result.selectedElementY = this._system.selectedElement.y;
                result.selectedElementType = GameElementType.Normal;
                result.clearElements = this.GetClearElements(result);
                result.newElements = this.GetNewElements(result);
                result.changeElements = this.GetChangeElements(result);
                return result;
            }
            if (this._system.upSameImageList.Count + this._system.downSameImageList.Count >= 3) {
                var result = this._system.CreateJudgeResult();
                result.direction = Direction.Vertical;
                result.selectedElementX = this._system.selectedElement.x;
                result.selectedElementY = this._system.selectedElement.y;
                result.selectedElementType = GameElementType.Normal;
                result.clearElements = this.GetClearElements(result);
                result.newElements = this.GetNewElements(result);
                result.changeElements = this.GetChangeElements(result);
                return result;
            }
            return null;
        }

        private ChangeElement[] GetChangeElements(JudgeResult result) {
            return null;
        }

        private NewElement[] GetNewElements(JudgeResult result) {
            if (result.direction == Direction.Horizontal || result.direction == Direction.Vertical) {
                GameElement[,] elements = this._system.game.gameElements;
                NewElement[] newElements = new NewElement[1];
                newElements[0] = new NewElement();
                newElements[0].oldElement = elements[result.selectedElementX, result.selectedElementY];
                newElements[0].newElementType = result.direction == Direction.Horizontal ? GameElementType.AnyRow : GameElementType.AnyColumn;
                return newElements;
            }
            return null;
        }

        private GameElement[] GetClearElements(JudgeResult result) {
            if (result.direction == Direction.Horizontal) {
                int count = this._system.leftSameImageList.Count + this._system.rightSameImageList.Count;
                if (count >= 2) {
                    GameElement[] clearElements = new GameElement[count + 1];
                    GameElement[,] elements = this._system.game.gameElements;
                    int index = 0;
                    clearElements[index] = elements[result.selectedElementX, result.selectedElementY];
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
            } else if (result.direction == Direction.Vertical) {
                int count = this._system.upSameImageList.Count + this._system.downSameImageList.Count;
                if (count >= 2) {
                    GameElement[] clearElements = new GameElement[count + 1];
                    GameElement[,] elements = this._system.game.gameElements;
                    int index = 0;
                    clearElements[index] = elements[result.selectedElementX, result.selectedElementY];
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
