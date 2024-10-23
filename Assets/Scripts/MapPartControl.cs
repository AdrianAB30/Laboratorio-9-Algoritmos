using System.Collections;
using UnityEngine;

public class MapPartControl : MonoBehaviour
{
    private SpriteRenderer mySprite;

    private void Awake()
    {
        mySprite = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    public void SetSprite(Sprite newSprite)
    {
        mySprite.sprite = newSprite;
    }
}
