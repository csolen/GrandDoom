using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LuckyWheelController : MonoBehaviour
{
    public GameObject luckyWheelsPanel;
    public RectTransform wheel;
    public Button spinButton;
    public TextMeshProUGUI buttonLabel;

    public int sliceCount = 6;
    public float spinDuration = 4.5f;
    public int minFullRotations = 7;
    public int maxFullRotations = 12;

    public float pointerAngle = 90f;
    public float wheelDirection = -1f;

    private bool isSpinning = false;
    private bool resultReady = false;
    private int lastResultIndex = -1;

    private void Start()
    {
        spinButton.onClick.AddListener(OnSpinButtonPressed);
        SetButtonState_Spin();
    }

    private void Update()
    {
        if (PlayerPrefs.GetInt("Open_SpinWheel") == 1)
        {
            OpenWheelMenu();
        }
        else
        {
            CloseWheelMenu();
        }
    }

    private void OnSpinButtonPressed()
    {
        if (isSpinning) return;

        if (!resultReady)
            StartCoroutine(SpinRoutine());
        else
            CollectRewardAndReset();
    }

    private IEnumerator SpinRoutine()
    {
        isSpinning = true;
        resultReady = false;
        spinButton.gameObject.SetActive(false);

        float sliceAngle = 360f / sliceCount;
        lastResultIndex = Random.Range(0, sliceCount);

        float sliceCenter = lastResultIndex * sliceAngle + sliceAngle / 2f;
        float targetAngle = pointerAngle - sliceCenter;

        int fullRotations = Random.Range(minFullRotations, maxFullRotations + 1);
        float totalAngle = (fullRotations * 360f + targetAngle) * wheelDirection;

        float startAngle = wheel.eulerAngles.z;
        float endAngle = startAngle + totalAngle;

        float t = 0f;
        while (t < spinDuration)
        {
            t += Time.deltaTime;
            float n = Mathf.Clamp01(t / spinDuration);

            float eased = EaseOutCubic(n);

            float currentZ = Mathf.Lerp(startAngle, endAngle, eased);
            wheel.rotation = Quaternion.Euler(0, 0, currentZ);

            yield return null;
        }

        wheel.rotation = Quaternion.Euler(0, 0, endAngle);

        isSpinning = false;
        resultReady = true;

        SetButtonState_Collect();
        spinButton.gameObject.SetActive(true);
    }

    private float EaseOutCubic(float t)
    {
        t = Mathf.Clamp01(t);
        t = 1f - Mathf.Pow(1f - t, 3f);
        return t;
    }

    private void CollectRewardAndReset()
    {
        resultReady = false;
        SetButtonState_Spin();
    }

    private void SetButtonState_Spin()
    {
        buttonLabel.text = "Spin";
    }

    private void SetButtonState_Collect()
    {
        buttonLabel.text = "Collect";
    }

    public void OpenWheelMenu()
    {
        PlayerPrefs.SetInt("Open_SpinWheel", 1);
        GameTester.Instance.ShowCursorInEditor(true);
        luckyWheelsPanel.SetActive(true);
    }

    public void CloseWheelMenu()
    {
        PlayerPrefs.SetInt("Open_SpinWheel", 0);
        GameTester.Instance.ShowCursorInEditor(false);
        luckyWheelsPanel.SetActive(false);
    }
}
