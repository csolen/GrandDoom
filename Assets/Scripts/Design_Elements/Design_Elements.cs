using UnityEngine;

public class Design_Elements : MonoBehaviour
{
    public SpriteRenderer sr;
    public SpriteRenderer srFront;

    [SerializeField] private GameObject designElementHolder;
    [SerializeField] private Animator anim;

    public GameObject nextStageSr;

    private void Awake()
    {
        srFront.enabled = false;
    }

    public void DesignElementGotHit()
    {
        anim.SetTrigger("gotHit");

        if (nextStageSr != null)
        {
            Instantiate(nextStageSr, designElementHolder.transform.position, Quaternion.identity);
            Destroy(designElementHolder);
        }

    }


    
}
