using System.Collections;
using System.Collections.Generic;

namespace EGame.Core
{
    public enum Direction
    {
        Null, // 无
        Horizontal, // 水平
        Vertical, // 垂直
        Cross, // 相交
        All, // 所有方向
    }
    public struct NewElement {
        public GameElement oldElement;
        public GameElementType newElementType;
    }

    public struct ChangeElement {
        public GameElement element;
        public GameElementType newElementType;
    }

    public enum JudgeType
    {
        Null,
        Active,   // 玩家主动触发
        System,   // 系统触发
    }

    public class JudgeResult {
        public Direction direction = Direction.Null;
        public int selectedElementX = -1;
        public int selectedElementY = -1;
        public GameElementType selectedElementType = GameElementType.Null;

        public GameElement[] clearElements = null;
        public NewElement[] newElements = null;
        public ChangeElement[] changeElements = null;

        public void Reset() {
            direction = Direction.Null;
            selectedElementX = -1; selectedElementY = -1;
            selectedElementType = GameElementType.Null;
            clearElements = null;
            newElements = null;
            changeElements = null;
        }
    }

    public interface IJudgeRule {
        JudgeResult IsMathch();
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
        private IGameElementView _selectedElement = null;
        private IGameElementView _otherElement = null;
        private JudgeType _judgeType = JudgeType.Null;
        private Queue<JudgeResult> _judgeResultPool = new Queue<JudgeResult>();

        public List<GameElement> leftSameImageList { get { return this._leftSameImageList; } }
        public List<GameElement> rightSameImageList { get { return this._rightSameImageList; } }
        public List<GameElement> upSameImageList { get { return this._upSameImageList; } }
        public List<GameElement> downSameImageList { get { return this._downSameImageList; } }
        public IGameElementView selectedElement { get { return this._selectedElement; }}
        public IGameElementView otherElement { get { return this._otherElement; }}
        public JudgeType judgeType { get { return this._judgeType; } }

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

        public JudgeResult StartJudge(IGameElementView selectedElement, IGameElementView otherElement, JudgeType judgeType) {
            this.ClearJudgeEnv();
            this._selectedElement = selectedElement;
            this._otherElement = otherElement;
            this._judgeType = judgeType;
            this.InitJudgeEnv();
            JudgeResult result = null;
            for (int i = 0; i < this._judgeRules.Count; i++) {
                result = this._judgeRules[i].IsMathch();
                if (result != null) {
                    break;
                }
            }
            return result;
        }

        private void InitJudgeEnv() {
            GameElement[,] elements = this._game.gameElements;
            var selectedElement = this._selectedElement;
            for (int i = selectedElement.x - 1; i >= 0; i--) {
                GameElement e = elements[i, selectedElement.y];
                if (e.elementType != GameElementType.Normal || selectedElement.imageId != e.elementView.imageId) {
                    break;
                }
                this._leftSameImageList.Add(e);
            }
            for (int i = selectedElement.x + 1; i < this._game.elementCols; i++) {
                GameElement e = elements[i, selectedElement.y];
                if (e.elementType != GameElementType.Normal || selectedElement.imageId != e.elementView.imageId) {
                    break;
                }
                this._rightSameImageList.Add(e);
            }
            for (int j = selectedElement.y - 1; j >= 0; j--) {
                GameElement e = elements[selectedElement.x, j];
                if (e.elementType != GameElementType.Normal || selectedElement.imageId != e.elementView.imageId) {
                    break;
                }
                this._upSameImageList.Add(e);
            }
            for (int j = selectedElement.y + 1; j < this._game.elementRows; j++) {
                GameElement e = elements[selectedElement.x, j];
                if (e.elementType != GameElementType.Normal || selectedElement.imageId != e.elementView.imageId) {
                    break;
                }
                this._downSameImageList.Add(e);
            }
        }

        public void ClearJudgeEnv() {
            this._leftSameImageList.RemoveRange(0, this._leftSameImageList.Count);
            this._rightSameImageList.RemoveRange(0, this._rightSameImageList.Count);
            this._upSameImageList.RemoveRange(0, this._upSameImageList.Count);
            this._downSameImageList.RemoveRange(0, this._downSameImageList.Count);
            this._selectedElement = null;
            this._judgeType = JudgeType.Null;
        }

        public JudgeResult CreateJudgeResult() {
            if (this._judgeResultPool.Count > 0) return this._judgeResultPool.Dequeue();
            return new JudgeResult();
        }

        public void RecoveryJudgeResult(JudgeResult result) {
            if (result == null) return;
            result.Reset();
            this._judgeResultPool.Enqueue(result);
        }
    }   
}
