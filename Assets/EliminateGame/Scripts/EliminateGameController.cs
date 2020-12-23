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

    public void CreateGameElementImageView(int x, int y, GameElementView elementView) {
        var index = this._random.Next(0, this.elementImageViews.Length);
        elementView.UpdateImageView(elementImageViews[index].imageId, elementImageViews[index].sprite);
    }

    public Vector3 ConvertElementToUnityPos(int x, int y)
    {
        return new Vector3((x - this.game.elementCols / 2) * 0.63f, (y - this.game.elementRows / 2) * 0.6f, 0);
    }
}
