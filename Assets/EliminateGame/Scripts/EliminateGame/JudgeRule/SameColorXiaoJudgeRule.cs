using System.Collections;
using System.Collections.Generic;

namespace EGame.Core
{
    // 四连消
    public class SameColorXiaoJudgeRule : IJudgeRule
    {
        private JudgeSystem _system = null;
        private int _otherElementImageId = -1;

        public SameColorXiaoJudgeRule(JudgeSystem system) {
            this._system = system;
        }

        public JudgeResult IsMathch() {
            this._otherElementImageId = -1;
            if (this._system.judgeType != JudgeType.Active) {
                return null;
            }
            GameElement[,] elements = this._system.game.gameElements;
            var element = elements[this._system.selectedElement.x, this._system.selectedElement.y];
            if (element.elementType == GameElementType.Same) {
                this._otherElementImageId = this._system.otherElement.imageId;
                var result = this._system.CreateJudgeResult();
                result.direction = Direction.All;
                result.selectedElementX = this._system.selectedElement.x;
                result.selectedElementY = this._system.selectedElement.y;
                result.selectedElementType = GameElementType.Same;
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
            return null;
        }

        private GameElement[] GetClearElements(JudgeResult result) {
            if (result.selectedElementType == GameElementType.Same) {
                List<GameElement> clearElements = new List<GameElement>();
                var cols = this._system.game.elementCols;
                var rows = this._system.game.elementRows;
                GameElement[,] elements = this._system.game.gameElements;
                var element = elements[result.selectedElementX, result.selectedElementY];
                clearElements.Add(element);
                for (int i = 0; i < cols; i++) {
                    for (int j = 0; j < rows; j++) {
                        var e = elements[i, j];
                        if (e.elementView.imageId == this._otherElementImageId) {
                            clearElements.Add(e);
                        }
                    }
                }
                return clearElements.ToArray();
            }
            return null;
        }
    }
}
