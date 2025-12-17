using UnityEngine;

public class Design_Elements : MonoBehaviour
{
    public SpriteRenderer sr;
    private SpriteRenderer srBack;

    [SerializeField] private GameObject designElementHolder;
    [SerializeField] private Animator anim;

    public Sprite[] nextStageSr;
    private int hitCount = 0;

    private void Awake()
    {
        srBack = GetComponent<SpriteRenderer>();
        srBack.enabled = false;
    }

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
