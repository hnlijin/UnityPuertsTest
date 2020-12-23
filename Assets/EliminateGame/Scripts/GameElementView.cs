using System.Collections;
using UnityEngine;
using EGame.Core;

public class GameElementView : MonoBehaviour, IGameElementView
{
    public SpriteRenderer spriteRenderer;
    public int x { get { return this._x; } }
    public int y { get { return this._y; } }
    public GameElementType elementType = GameElementType.Null;
    public EliminateGameController gameController { set; get; }
    public int imageId { set; get; }
    private Coroutine _moveCoroutine = null;
    private IElementMoveEndCallback _moveEndCallback = null;
    private IAnimationPlayCompleteCallback _aniPlayComplete = null;
    private int _x = 0, _y = 0;
    private Animation _animation = null;

    void Awake() {
        this._animation = this.GetComponent<Animation>();
    }
    
    private void OnMouseDown()
    {
        // Debug.Log("OnMouseDown: " + this.gameObject.name + string.Format(", P: {0}/{1}", this.x, this.y));
        this.gameController.PressedElement(this);
    }

    private void OnMouseEnter()
    {
        // Debug.Log("OnMouseEnter: " + this.gameObject.name + string.Format(", P: {0}/{1}", this.x, this.y));
        this.gameController.EnterElement(this);
    }    

    private void OnMouseUp()
    {
        this.gameController.ReleaseElement(this);
    }

    public void Init(int x, int y) {
        this._x = x;
        this._y = y;
    }

    public void PlayAnimation(string name, IAnimationPlayCompleteCallback callback) {
        this._aniPlayComplete = callback;
        if (name == GameElementAnimation.DropBack) {
            if (this._animation != null) {
                this._animation.Play("ElementDropBack");
            } else if (callback != null) {
                Debug.LogError("PlayAnimation: name = " + this.gameObject.name + ", ElementDropBack.name = " + name);
                callback(this, null);
            }
        } else if (name == GameElementAnimation.Disappear) {
            if (this._animation != null) {
                this._animation.Play("ElementBlinkDisappear");
            } else if (callback != null) {
                Debug.LogError("PlayAnimation: name = " + this.gameObject.name + ", ElementBlinkDisappear.name = " + name);
                callback(this, null);
            }
        } else if (callback != null) {
            callback(this, null);
        }
    }

    public void AnimationPlayComplete(string name) {
        // Debug.Log("AnimationPlayComplete: name = " + this.gameObject.name + ", name = " + name);
        var callback = this._aniPlayComplete;
        if (callback != null) {
            this._aniPlayComplete = null;
            callback(this, name);
        }
    }

    public void MoveElement(int targetX, int targetY, float time, IElementMoveEndCallback callback)
    {
        this._x = targetX;
        this._y = targetY;
        this._moveEndCallback = callback;
        this.gameObject.name = "element:" + targetX + "_" + targetY;
        if (this._moveCoroutine != null) {
            StopCoroutine(this._moveCoroutine);
            this._moveCoroutine = null;
        }
        if (time <= 0) {
            Vector3 pos = this.gameController.ConvertElementToUnityPos(targetX, targetY);
            this.transform.position = pos;
            if (this._moveEndCallback != null) {
                var callback1 = this._moveEndCallback;
                this._moveEndCallback = null;
                callback1(this);
            }
            return;
        }
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
        if (this._moveEndCallback != null) {
            var callback1 = this._moveEndCallback;
            this._moveEndCallback = null;
            callback1(this);
        }
        yield break;
    }

    public void CreateImageView(int x, int y)
    {
        this.gameController.CreateGameElementImageView(x, y, this);
    }

    public void UpdateImageView(int imageId, Sprite sprite) {
        this.imageId = imageId;
        if (this.spriteRenderer != null) {
            this.spriteRenderer.sprite = sprite;
        }
    }

    public void DestroyView() {
        Destroy(this.gameObject);
    }

    public void DestroyImageView() {
        this.spriteRenderer.sprite = null;
    }
}
