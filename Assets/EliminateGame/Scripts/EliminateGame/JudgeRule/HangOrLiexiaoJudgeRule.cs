using System.Collections;
using System.Collections.Generic;

namespace EGame.Core
{
    // 整列或行连消
    public class HangOrLiexiaoJudgeRule : IJudgeRule
    {
        private JudgeSystem _system = null;

        public HangOrLiexiaoJudgeRule(JudgeSystem system) {
            this._system = system;
        }

        public JudgeResult IsMathch() {
            if (this._system.judgeType != JudgeType.Active) {
                return null;
            }
            GameElement[,] elements = this._system.game.gameElements;
            var element = elements[this._system.selectedElement.x, this._system.selectedElement.y];
            if (element.elementType == GameElementType.AnyRow) {
                var result = this._system.CreateJudgeResult();
                result.direction = Direction.Horizontal;
                result.selectedElementX = this._system.selectedElement.x;
                result.selectedElementY = this._system.selectedElement.y;
                result.selectedElementType = element.elementType;
                result.clearElements = this.GetClearElements(result);
                result.newElements = this.GetNewElements(result);
                result.changeElements = this.GetChangeElements(result);
                return result;
            } else if (element.elementType == GameElementType.AnyColumn) {
                var result = this._system.CreateJudgeResult();
                result.direction = Direction.Vertical;
                result.selectedElementX = this._system.selectedElement.x;
                result.selectedElementY = this._system.selectedElement.y;
                result.selectedElementType = element.elementType;
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
            if (result.selectedElementType == GameElementType.AnyColumn) {
                var cols = this._system.game.elementCols;
                GameElement[] clearElements = new GameElement[cols];
                GameElement[,] elements = this._system.game.gameElements;
                for (int i = 0; i < cols; i++) {
                    var element = elements[i, result.selectedElementY];
                    if (element.CanMove()) {
                        clearElements[i] = element;
                    }
                }
                return clearElements;
            } else if (result.selectedElementType == GameElementType.AnyRow) {
                var rows = this._system.game.elementRows;
                GameElement[] clearElements = new GameElement[rows];
                GameElement[,] elements = this._system.game.gameElements;
                for (int j = 0; j < rows; j++) {
                    var element = elements[result.selectedElementX, j];
                    if (element.CanMove()) {
                        clearElements[j] = element;
                    }
                }
                return clearElements;
            }
            return null;
        }
    }
}
