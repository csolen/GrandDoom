using UnityEngine;

public class CrosshairRecoil : MonoBehaviour
{
    public static CrosshairRecoil instance;

    public float recoilIncrease = 1f;
    public float maxRecoil = 1f;
    public float returnSpeed = 5f;

    private float currentRecoil = 0f;
    private Vector3 baseScale;

    private void Awake()
    {
        instance = this;
        baseScale = transform.localScale;
    }

    private void Update()
    {
        currentRecoil = Mathf.MoveTowards(currentRecoil, 0f, returnSpeed * Time.deltaTime);

        float scaleMultiplier = 1f + currentRecoil;
        transform.localScale = baseScale * scaleMultiplier;
    }

    public void OnShoot()
    {
        currentRecoil += recoilIncrease;
        currentRecoil = Mathf.Clamp(currentRecoil, 0f, maxRecoil);
    }
}
