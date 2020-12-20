using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Puerts;
using EGame.Core;

[System.Serializable]
public struct ElementImageView
{
    public int imageId;
    public Sprite sprite;
}

public class EliminateGameController : MonoBehaviour //, IEliminateGameController
{
    public GameObject elementContainer;
    public GameObject gridContainer;
    public GameElementView[] elementPrefabs;
    public ElementImageView[] elementImageViews;
    private EliminateGame game = new EliminateGame();
    private Dictionary<GameElementType, GameElementView> _dictElementPrefabs = null;
    private System.Random _random = new System.Random();

    JsEnv jsEnv = null;

    // public IGameElementView CreateGameElementView(int x, int y, GameElementType elementType)
    public GameElementView CreateGameElementView(int x, int y, GameElementType elementType)
    {
        var elementPrefab = this._dictElementPrefabs[elementType];
        if (elementPrefab != null) {
            var elementName = "element:" + x + "," + y;
            var parent = this.elementContainer.transform;
            if (elementType == GameElementType.Grid) {
                parent = this.gridContainer.transform;
                elementName = "grid:" + x + "," + y;
            }
            var view = Instantiate(elementPrefab, ConvertElementToUnityPos(x, y), Quaternion.identity, parent);
            view.gameObject.name = elementName;
            view.gameController = this;
            return view;
        }
        return null;
    }

    public void ReplaceGameElementView(int oldX, int oldY, int targetX, int targetY, bool onlyUpdateName = false) {
        var elementName = "element:" + oldX + "," + oldY;
        var elementView = this.elementContainer.transform.Find(elementName);
        elementView.gameObject.name = "element:" + targetX + "," + targetY;
        if (onlyUpdateName == false) {
            elementView.position = ConvertElementToUnityPos(targetX, targetY);
        }
    }

    public void SetGameElementImageView(int x, int y, GameElementImageView imageView) {        var index = this._random.Next(0, this.elementImageViews.Length);
        imageView.imageId = elementImageViews[index].imageId;
        imageView.spriteRenderer.sprite = elementImageViews[index].sprite;
    }

    public Vector3 ConvertElementToUnityPos(int x, int y)
    {
        return new Vector3((x - this.game.elementCols / 2) * 0.63f, (y - this.game.elementRows / 2) * 0.6f, 0);
    }

    void Awake() {
        this._dictElementPrefabs = new Dictionary<EGame.Core.GameElementType, GameElementView>();
        for (int i = 0; i < this.elementPrefabs.Length; i++) {
            var e = this.elementPrefabs[i];
            this._dictElementPrefabs[e.elementType] = e;
        }
        // this.game.Init(5, 7, this);
    }

    void Start() {
        // this.game.Start();
        jsEnv = new JsEnv();
        jsEnv.Eval("require('main')");
    }

    void Update() {
        // this.game.Update(Time.deltaTime);
        jsEnv.Tick();
    }

    void OnDestory() {
        jsEnv.Dispose();
    }
}
