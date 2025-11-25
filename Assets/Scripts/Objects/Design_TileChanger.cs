using UnityEngine;

public class Design_TileChanger : MonoBehaviour
{
    public SpriteRenderer sr;

    public Sprite newSprite;

    public GameObject destroyingParticle;

    private void Awake()
    {
        destroyingParticle.SetActive(false);
    }

    public void ChangeSprite()
    {
        sr.sprite = newSprite;
        destroyingParticle.SetActive(true);
        this.enabled = false;
    }
}
