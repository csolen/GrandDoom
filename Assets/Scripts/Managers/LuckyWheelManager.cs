using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LuckyWheelController : MonoBehaviour
{
    public GameObject luckyWheelsPanel;
    public RectTransform wheel;
    public Button spinButton;
    public Button closeButton;
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

    public TextMeshProUGUI[] rewardText;

    private int randomHealth01;
    private int randomHealth02;
    private int randomGold01;
    private int randomGold02;
    private int randomAmmo01;
    private int randomAmmo02;

    private void Awake()
    {
        luckyWheelsPanel.SetActive(false);
    }

    private void Start()
    {
        closeButton.onClick.AddListener(CloseWheelMenu);
        spinButton.onClick.AddListener(OnSpinButtonPressed);
    }

    private void Update()
    {
        if (luckyWheelsPanel.activeInHierarchy)
        {
            return;
        }

        if (PlayerPrefs.GetInt("Open_SpinWheel") == 1)
        {
            OpenWheelMenu();
            isSpinning = false;
            resultReady = false;
            buttonLabel.text = "Spin";
            PlaceRewards();
        }
    }

    private void PlaceRewards()
    {
        randomHealth01 = Random.Range(20, 31);
        randomHealth02 = Random.Range(20, 31);

        rewardText[0].text = "Health <br> " + randomHealth01.ToString();
        rewardText[3].text = "Health <br> " + randomHealth02.ToString();


        randomGold01 = Random.Range(50, 101);
        randomGold02 = Random.Range(50, 101);

        rewardText[1].text = "Gold <br> " + randomGold01.ToString();
        rewardText[4].text = "Gold <br> " + randomGold02.ToString();

        randomAmmo01 = Random.Range(30, 51);
        randomAmmo02 = Random.Range(30, 51);

        rewardText[2].text = "Ammo <br> " + randomAmmo01.ToString();
        rewardText[5].text = "Ammo <br> " + randomAmmo02.ToString();
    }

    private void OnSpinButtonPressed()
    {
        if (isSpinning) return;

        if (!resultReady)
            StartCoroutine(SpinRoutine());
        else
            CollectRewardAndClose();
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

        buttonLabel.text = "Collect";
        spinButton.gameObject.SetActive(true);
    }

    private float EaseOutCubic(float t)
    {
        t = Mathf.Clamp01(t);
        t = 1f - Mathf.Pow(1f - t, 3f);
        return t;
    }

    private void GiveReward(int index)
    {
        switch (index)
        {
            case 0:
                PlayerController.instance.health += randomHealth01;
                break;

            case 1:
                PlayerController.instance.goldAmount += randomGold01;
                break;

            case 2:
                PlayerController.instance.ammoAmount += randomAmmo01;
                break;

            case 3:
                PlayerController.instance.health += randomHealth02;
                break;

            case 4:
                PlayerController.instance.goldAmount += randomGold02;
                break;

            case 5:
                PlayerController.instance.ammoAmount += randomAmmo02;
                break;
        }
    }

    private void CollectRewardAndClose()
    {
        GiveReward(lastResultIndex);

        CloseWheelMenu();
    }

    public void OpenWheelMenu()
    {
        PlayerPrefs.SetInt("Open_SpinWheel", 1);
        luckyWheelsPanel.SetActive(true);
        GameTester.Instance.ShouldStopTheGame(true);
    }

    public void CloseWheelMenu()
    {
        PlayerPrefs.SetInt("Open_SpinWheel", 0);
        luckyWheelsPanel.SetActive(false);
        GameTester.Instance.ShouldStopTheGame(false);
    }

}
