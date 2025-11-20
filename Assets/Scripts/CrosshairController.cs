using UnityEngine;

public class CrosshairRecoil : MonoBehaviour
{
    public static CrosshairRecoil instance;

    [Header("Recoil Values")]
    public float recoilIncrease = 0.25f; // her mermi ne kadar büyütsün
    public float maxRecoil = 1.5f;       // max büyüme katsayı
    public float returnSpeed = 5f;       // geri dönüş hızı

    private float currentRecoil = 0f;
    private Vector3 baseScale;

    private void Awake()
    {
        instance = this;
        baseScale = transform.localScale; // orijinal boyut
    }

    private void Update()
    {
        // recoil yavaşça azalır
        currentRecoil = Mathf.MoveTowards(currentRecoil, 0f, returnSpeed * Time.deltaTime);

        // scale hesaplama
        float scaleMultiplier = 1f + currentRecoil;
        transform.localScale = baseScale * scaleMultiplier;
    }

    public void OnShoot()
    {
        currentRecoil += recoilIncrease;
        currentRecoil = Mathf.Clamp(currentRecoil, 0f, maxRecoil);
    }
}
