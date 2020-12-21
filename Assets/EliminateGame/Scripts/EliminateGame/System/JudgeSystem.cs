using System.Collections;
using System.Collections.Generic;

namespace EGame.Core
{
    public interface IJudgeRule {
        bool IsMathch();
        GameElementType newElementType { get; }
        GameElement[] GetClearElements();
    }

    /** 判定系统 */
    public class JudgeSystem
    {
        private List<GameElement> _leftSameImageList;
        private List<GameElement> _rightSameImageList;
        private List<GameElement> _upSameImageList;
        private List<GameElement> _downSameImageList;

        private EliminateGame _game = null;
        private List<IJudgeRule> _judgeRules = null;

        public JudgeSystem(EliminateGame game) {
            this._game = game;
            this._leftSameImageList = new List<GameElement>();
            this._rightSameImageList = new List<GameElement>();
            this._upSameImageList = new List<GameElement>();
            this._downSameImageList = new List<GameElement>();
            this._judgeRules = new List<IJudgeRule>();
        }

        public void RegJudgeRule(IJudgeRule rule) {
            this._judgeRules.Add(rule);
        }

        public void RemoveJudgeRule(IJudgeRule rule) {
            if (this._judgeRules.Contains(rule)) {
                this._judgeRules.Remove(rule);
            }
        }

        public IJudgeRule StartJudge() {
            for (int i = 0; i < this._judgeRules.Count; i++) {
                if (this._judgeRules[i].IsMathch()) {
                    return this._judgeRules[i];
                }
            }
            return null;
        }

        private void InitJudgeEv() {
            GameElement[,] elements = this._game.gameElements;
            var pressedElement = this._game.pressedElement;
            for (int i = pressedElement.x - 1; i >= 0; i--) {
                GameElement e = elements[i, pressedElement.y];
                if (pressedElement.imageId == e.elementView.imageId) {
                    this._leftSameImageList.Add(e);
                }
            }
            for (int i = pressedElement.x + 1; i < this._game.elementCols; i++) {
                GameElement e = elements[i, pressedElement.y];
                if (pressedElement.imageId == e.elementView.imageId) {
                    this._rightSameImageList.Add(e);
                }
            }
            for (int j = pressedElement.y - 1; j >= 0; j--) {
                GameElement e = elements[pressedElement.x, j];
                if (pressedElement.imageId == e.elementView.imageId) {
                    this._upSameImageList.Add(e);
                }
            }
            for (int j = pressedElement.y + 1; j < this._game.elementRows; j++) {
                GameElement e = elements[pressedElement.x, j];
                if (pressedElement.imageId == e.elementView.imageId) {
                    this._downSameImageList.Add(e);
                }
            }
        }
    }   
}
