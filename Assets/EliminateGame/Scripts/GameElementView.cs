using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EGame.Core;

public class GameElementView : MonoBehaviour, IGameElementView
{
    public GameElementType elementType;
    public IGameElementImageView imageView { set; get; }
    public EliminateGameController gameController { set; get; }
    private GameElementImageView _gameImageView = null;
    private Coroutine _moveCoroutine = null;

    void Awake() {
        this._gameImageView = GetComponent<GameElementImageView>();
        this.imageView = _gameImageView;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveElement(int targetX, int targetY, float time)
    {
        if (this._moveCoroutine != null) StopCoroutine(this._moveCoroutine);
        this._moveCoroutine = StartCoroutine(MoveCoroutine(targetX, targetY, time));
    }

    IEnumerator MoveCoroutine(int targetX, int targetY, float time)
    {
        //实际坐标
        Vector3 endPos = this.gameController.ConvertElementToUnityPos(targetX, targetY);
        Vector3 startPos = this.transform.position;
        //每帧移动一点
        for (float i = 0; i < time; i += Time.deltaTime) {
            this.transform.position = Vector3.Lerp(startPos, endPos, i / time);
            yield return 0;
        }
        this.transform.position = endPos;
        yield return 0;
    }

    public void SetImageView(int x, int y)
    {
        gameController.SetGameElementImageView(x, y, this._gameImageView);
    }

    public void DestroyView() {
        Destroy(this.gameObject);
    }
}
