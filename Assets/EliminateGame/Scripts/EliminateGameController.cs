using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EGame.Core;

[System.Serializable]
public struct ElementImageView
{
    public int imageId;
    public Sprite sprite;
}

public class EliminateGameController : MonoBehaviour , IEliminateGameController
{
    public GameObject elementContainer;
    public GameObject gridContainer;
    public GameElementView[] elementPrefabs;
    public ElementImageView[] elementImageViews;
    private EliminateGame game = new EliminateGame();
    private Dictionary<GameElementType, GameElementView> _dictElementPrefabs = null;
    private System.Random _random = new System.Random();

    public IGameElementView CreateGameElementView(int x, int y, GameElementType elementType)
    {
        var elementPrefab = this._dictElementPrefabs[elementType];
        if (elementPrefab != null) {
            var elementName = "element:" + x + "_" + y;
            var parent = this.elementContainer.transform;
            if (elementType == GameElementType.Grid) {
                parent = this.gridContainer.transform;
                elementName = "grid:" + x + "_" + y;
            }
            var view = Instantiate(elementPrefab, this.ConvertElementToUnityPos(x, y), Quaternion.identity, parent);
            view.Init(x, y);
            view.gameObject.name = elementName;
            view.gameController = this;
            return view;
        }
        return null;
    }

    public void PressedElement(IGameElementView elementView) {
        this.game.PressedElement(elementView);  
    }

    public void EnterElement(IGameElementView elementView) {
        this.game.EnterElement(elementView);   
    }

    public void ReleaseElement(IGameElementView elementView) {
        this.game.ReleaseElement(elementView);
    }

    public void LogInfo(string info) {
        UnityEngine.Debug.Log("EGame: " + info);
    }
    public void LogWarn(string warn) {
        UnityEngine.Debug.LogWarning("EGame: " + warn);
    }

    public void LogError(string error) {
        UnityEngine.Debug.LogError("EGame:" + error);
    }

    void Awake() {
        this._dictElementPrefabs = new Dictionary<GameElementType, GameElementView>();
        for (int i = 0; i < this.elementPrefabs.Length; i++) {
            var e = this.elementPrefabs[i];
            this._dictElementPrefabs[e.elementType] = e;
        }
        this.game.Init(5, 7, this);
    }

    void Start() {
        this.game.Start();
    }

    void Update() {
        this.game.Update(Time.deltaTime);
    }

    void OnDestory() {
    }

    private List<int> imageIdPool = new List<int>();
    private List<ElementImageView> imagePool = new List<ElementImageView>();

    public void CreateGameElementImageView(int x, int y, GameElementView elementView) {
        int cols = this.game.elementCols;
        int rows = this.game.elementRows;
        var elements = this.game.gameElements;
        this.imagePool.Clear();
        this.imageIdPool.Clear();
        GameElement up1 = null, up2 = null;
        GameElement down1 = null, down2 = null;
        GameElement left1 = null, left2 = null;
        GameElement right1 = null, right2 = null;
        if (y + 1 < rows - 1) up1 = elements[x, y + 1];
        if (y + 2 < rows - 1) up2 = elements[x, y + 2];
        if (y - 1 >= 0) down1 = elements[x, y - 1];
        if (y - 2 >= 0) down2 = elements[x, y - 2];
        if (x - 1 >= 0) left1 = elements[x - 1, y];
        if (x - 2 >= 0) left2 = elements[x - 2, y];
        if (x + 1 < cols - 1) right1 = elements[x + 1, y];
        if (x + 2 < cols - 2) right2 = elements[x + 2, y];
        if (up1 != null && up2 != null && up1.elementType == GameElementType.Normal && up2.elementType == GameElementType.Normal) {
            if (up1.elementView != null && up2.elementView != null && up1.elementView.imageId == up2.elementView.imageId) {
                imageIdPool.Add(up1.elementView.imageId);
            }
        }
        if (down1 != null && down2 != null && down1.elementType == GameElementType.Normal && down2.elementType == GameElementType.Normal) {
            if (down1.elementView != null && down2.elementView != null && down1.elementView.imageId == down2.elementView.imageId) {
                imageIdPool.Add(down1.elementView.imageId);
            }
        }
        if (left1 != null && left2 != null && left1.elementType == GameElementType.Normal && left2.elementType == GameElementType.Normal) {
            if (left1.elementView != null && left2.elementView != null && left1.elementView.imageId == left2.elementView.imageId) {
                imageIdPool.Add(left1.elementView.imageId);
            }
        }
        if (right1 != null && right2 != null && right1.elementType == GameElementType.Normal && right2.elementType == GameElementType.Normal) {
            if (right1.elementView != null && right2.elementView != null && right1.elementView.imageId == right2.elementView.imageId) {
                imageIdPool.Add(right1.elementView.imageId);
            }
        }
        for (int i = 0; i < this.elementImageViews.Length; i++) {
            var image = this.elementImageViews[i];
            if (this.imageIdPool.IndexOf(image.imageId) >= 0) {
                continue;
            }
            this.imagePool.Add(image);
        }
        if (this.imagePool.Count > 0) {
            int index = this._random.Next(0, this.imagePool.Count - 1);
            elementView.UpdateImageView(this.imagePool[index].imageId, this.imagePool[index].sprite);
        } else {
            int index = this._random.Next(0, this.elementImageViews.Length - 1);
            elementView.UpdateImageView(this.elementImageViews[index].imageId, this.elementImageViews[index].sprite);
        }
    }

    public Vector3 ConvertElementToUnityPos(int x, int y)
    {
        return new Vector3((x - this.game.elementCols / 2) * 0.63f, (y - this.game.elementRows / 2) * 0.6f, 0);
    }
}
