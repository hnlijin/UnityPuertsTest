using System.Collections;
using System.Collections.Generic;

namespace EGame.Core
{
    // 四连消
    public class SameColorXiaoJudgeRule : IJudgeRule
    {
        private JudgeSystem _system = null;
        private int _selectedElementX = -1, _selectedElementY = -1;
        private GameElementType _selectedElementType = GameElementType.Null;
        private int _otherElementImageId = -1;
        public int selectedElementX { get { return this._selectedElementX; } }
        public int selectedElementY { get { return this._selectedElementY; } }
        public GameElementType selectedElementType { get { return this._selectedElementType; } }

        public SameColorXiaoJudgeRule(JudgeSystem system) {
            this._system = system;
        }

        public bool IsMathch() {
            this._selectedElementType = GameElementType.Null;
            this._otherElementImageId = -1;
            if (this._system.judgeType != JudgeType.Active) {
                this._selectedElementX = -1;
                this._selectedElementY = -1;
                return false;
            }
            this._selectedElementX = this._system.selectedElement.x;
            this._selectedElementY = this._system.selectedElement.y;
            GameElement[,] elements = this._system.game.gameElements;
            var element = elements[this._selectedElementX, this._selectedElementY];
            if (element.elementType == GameElementType.Same) {
                this._selectedElementType = GameElementType.Same;
                this._otherElementImageId = this._system.otherElement.imageId;
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
            if (this._selectedElementType == GameElementType.Same) {
                List<GameElement> clearElements = new List<GameElement>();
                var cols = this._system.game.elementCols;
                var rows = this._system.game.elementRows;
                GameElement[,] elements = this._system.game.gameElements;
                var element = elements[this._selectedElementX, this._selectedElementY];
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
