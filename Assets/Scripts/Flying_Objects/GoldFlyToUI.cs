using UnityEngine;

public class GoldFlyUI : MonoBehaviour
{
    private RectTransform target;
    public float flyTime = 0.5f;

    RectTransform rt;
    RectTransform parentRT;

    Vector2 startPos;
    Vector2 endPos;
    float t;

    void Awake()
    {
        target = GameObject.FindGameObjectWithTag("UI_Spawn_Point_Gold").GetComponent<RectTransform>();
        rt = GetComponent<RectTransform>();
        parentRT = rt.parent as RectTransform;
    }

    void Start()
    {
        if (target == null || parentRT == null)
            return;

        startPos = rt.anchoredPosition;
        endPos = parentRT.InverseTransformPoint(target.position);
        t = 0f;
    }

    void Update()
    {
        if (target == null)
            return;

        t += Time.deltaTime / flyTime;
        float u = Mathf.Clamp01(t);

        rt.anchoredPosition = Vector2.Lerp(startPos, endPos, EaseInOut(u));

        if (u >= 1f)
        {
            Destroy(gameObject);
            PlayerController.instance.AddGold(5);
        }
    }

    float EaseInOut(float x)
    {
        return x < 0.5f
            ? 4f * x * x * x
            : 1f - Mathf.Pow(-2f * x + 2f, 3f) / 2f;
    }
}
