using System.Collections;
using System.Collections.Generic;

namespace EGame.Core
{
    // 整列或行连消
    public class HangOrLiexiaoJudgeRule : IJudgeRule
    {
        private JudgeSystem _system = null;
        private int _selectedElementX = -1, _selectedElementY = -1;
        private GameElementType _selectedElementType = GameElementType.Null;
        public int selectedElementX { get { return this._selectedElementX; } }
        public int selectedElementY { get { return this._selectedElementY; } }
        public GameElementType selectedElementType { get { return this._selectedElementType; } }

        public HangOrLiexiaoJudgeRule(JudgeSystem system) {
            this._system = system;
        }

        public bool IsMathch() {
            this._selectedElementType = GameElementType.Null;
            if (this._system.judgeType != JudgeType.Active) {
                this._selectedElementX = -1;
                this._selectedElementY = -1;
                return false;
            }
            this._selectedElementX = this._system.selectedElement.x;
            this._selectedElementY = this._system.selectedElement.y;
            GameElement[,] elements = this._system.game.gameElements;
            var element = elements[this._selectedElementX, this._selectedElementY];
            if (element.elementType == GameElementType.AnyRow) {
                this._selectedElementType = element.elementType;
                return true;
            } else if (element.elementType == GameElementType.AnyColumn) {
                this._selectedElementType = element.elementType;
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
            if (this._selectedElementType == GameElementType.AnyColumn) {
                var cols = this._system.game.elementCols;
                GameElement[] clearElements = new GameElement[cols];
                GameElement[,] elements = this._system.game.gameElements;
                for (int i = 0; i < cols; i++) {
                    var element = elements[i, this._selectedElementY];
                    if (element.CanMove()) {
                        clearElements[i] = element;
                    }
                }
                return clearElements;
            } else if (this._selectedElementType == GameElementType.AnyRow) {
                var rows = this._system.game.elementRows;
                GameElement[] clearElements = new GameElement[rows];
                GameElement[,] elements = this._system.game.gameElements;
                for (int j = 0; j < rows; j++) {
                    var element = elements[this._selectedElementX, j];
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
