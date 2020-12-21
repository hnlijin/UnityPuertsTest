using System.Collections;
using System.Collections.Generic;

namespace EGame.Core
{
    public class SanlianxiaoJudgeRule : IJudgeRule
    {
        private JudgeSystem _system = null;
        public SanlianxiaoJudgeRule(JudgeSystem system) {
            this._system = system;
        }
        public bool IsMathch() {
            if (this._system.leftSameImageList.Count + this._system.rightSameImageList.Count >= 2) {
                return true;
            }
            if (this._system.upSameImageList.Count + this._system.downSameImageList.Count >= 2) {
                return true;
            }
            return false;
        }
        public GameElementType newElementType { get { return GameElementType.Null; } }
        public GameElement[] GetClearElements() {
            int count1 = this._system.leftSameImageList.Count + this._system.rightSameImageList.Count;
            if (count1 >= 2) {
                GameElement[] clearElements = new GameElement[count1 + 1];
                GameElement[,] elements = this._system.game.gameElements;
                IGameElementView pressedElement = this._system.pressedElement;
                int index = 0;
                for (int i = 0; i < this._system.leftSameImageList.Count; i++) {
                    clearElements[index] = this._system.leftSameImageList[i];
                    index += 1;
                }
                clearElements[index] = elements[pressedElement.x, pressedElement.y];
                index += 1;
                for (int j = 0; j < this._system.rightSameImageList.Count; j++) {
                    clearElements[index] = this._system.rightSameImageList[j];
                    index += 1;
                }
                return clearElements;
            }
            int count2 = this._system.upSameImageList.Count + this._system.downSameImageList.Count;
            if (count2 >= 2) {
                GameElement[] clearElements = new GameElement[count2 + 1];
                GameElement[,] elements = this._system.game.gameElements;
                IGameElementView pressedElement = this._system.pressedElement;
                int index = 0;
                for (int i = 0; i < this._system.upSameImageList.Count; i++) {
                    clearElements[index] = this._system.upSameImageList[i];
                    index += 1;
                }
                clearElements[index] = elements[pressedElement.x, pressedElement.y];
                index += 1;
                for (int j = 0; j < this._system.downSameImageList.Count; j++) {
                    clearElements[index] = this._system.downSameImageList[j];
                    index += 1;
                }
                return clearElements;
            }
            return null;
        }
    }
}

