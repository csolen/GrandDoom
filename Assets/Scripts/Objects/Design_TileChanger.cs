using UnityEngine;

public class Design_TileChanger : MonoBehaviour
{
    public SpriteRenderer sr;

    public Sprite newSprite;

    public void ChangeSprite()
    {
        sr.sprite = newSprite;
        this.enabled = false;
    }
}
