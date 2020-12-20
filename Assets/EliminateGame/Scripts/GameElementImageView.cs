using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameElementImageView : MonoBehaviour, EGame.Core.IGameElementImageView
{
    public int imageId { set; get; }
    public SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RemoveImage()
    {
        this.spriteRenderer.sprite = null;
    }
}
