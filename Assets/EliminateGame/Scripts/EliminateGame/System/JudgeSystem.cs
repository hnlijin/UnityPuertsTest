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
        private IGameElementView _pressedElement = null;
        private IGameElementView _enterElement = null;

        public List<GameElement> leftSameImageList { get { return this._leftSameImageList; } }
        public List<GameElement> rightSameImageList { get { return this._rightSameImageList; } }
        public List<GameElement> upSameImageList { get { return this._upSameImageList; } }
        public List<GameElement> downSameImageList { get { return this._downSameImageList; } }
        public IGameElementView pressedElement { get { return this._pressedElement; }}
        public IGameElementView enterElement { get { return this._enterElement; }}

        public EliminateGame game { get { return this._game; } }

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

        public IJudgeRule StartJudge(IGameElementView pressedElement, IGameElementView enterElement) {
            this._pressedElement = pressedElement;
            this._enterElement = enterElement;
            this.ClearJudgeEnv();
            this.InitJudgeEnv();
            for (int i = 0; i < this._judgeRules.Count; i++) {
                if (this._judgeRules[i].IsMathch()) {
                    return this._judgeRules[i];
                }
            }
            return null;
        }

        private void InitJudgeEnv() {
            
            GameElement[,] elements = this._game.gameElements;
            var pressedElement = this._pressedElement;
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

        private void ClearJudgeEnv() {
            this._leftSameImageList.RemoveRange(0, this._leftSameImageList.Count);
            this._rightSameImageList.RemoveRange(0, this._rightSameImageList.Count);
            this._upSameImageList.RemoveRange(0, this._upSameImageList.Count);
            this._downSameImageList.RemoveRange(0, this._downSameImageList.Count);
        }
    }   
}
