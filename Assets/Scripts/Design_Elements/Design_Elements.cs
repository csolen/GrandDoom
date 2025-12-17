using UnityEngine;

public class Design_Elements : MonoBehaviour
{
    public SpriteRenderer sr;

    [SerializeField] private GameObject designElementHolder;
    [SerializeField] private Animator anim;

    public Sprite[] nextStageSr;
    private int hitCount = 0;

    public void DesignElementGotHit()
    {
        if (hitCount >= nextStageSr.Length)
        {
            Destroy(designElementHolder);
        }
        else
        {
            sr.sprite = nextStageSr[hitCount];
            hitCount++;
            anim.SetTrigger("gotHit");
        }
    }


    
}
