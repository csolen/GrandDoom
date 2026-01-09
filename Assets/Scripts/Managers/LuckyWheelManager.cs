using System;
using System.Collections;
using System.Collections.Generic;
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
    public TextMeshProUGUI ticketCount;

    private const int SliceCount = 6;
    public TextMeshProUGUI[] rewardText;

    public float spinDuration = 4.5f;
    public int minFullRotations = 7;
    public int maxFullRotations = 12;

    private const float PointerAngle = 90f;
    private const float WheelDirection = -1f;
    private const int SliceIndexOffset = 0;

    public int spinCostGold = 20;

    public Vector2Int turn1TicketRange = new(1, 4);
    public Vector2Int turn2TicketRange = new(2, 6);
    public Vector2Int turn3TicketRange = new(3, 8);
    public Vector2Int turn4TicketRange = new(5, 10);
    public Vector2Int turn5TicketRange = new(8, 15);

    private const float WheelDefaultZ = 0f;

    public enum RewardType { Gold, Health, Ammo, Bundle }

    [Serializable]
    public class StreakRewardSlot
    {
        public Button button;
        public Image blockerImage;
        public TextMeshProUGUI rewardText;
        public TextMeshProUGUI RequiredTicketText;

        public int requiredTurn = 1;
        public int ticketCost = 0;

        public RewardType fixedType = RewardType.Gold;

        public int minAmount = 1;
        public int maxAmount = 5;

        [HideInInspector] public int chosenAmount;
        [HideInInspector] public bool claimed;
    }

    public StreakRewardSlot[] streakSlots = new StreakRewardSlot[5];

    private enum RunState { ReadyToSpin, CanGoNextTurnOrCashOut, Busted }
    private enum SliceType { Skull, Ticket }

    private class SliceData
    {
        public SliceType type;
        public int tickets;
    }

    private readonly List<SliceData> slices = new();

    private bool isSpinning;
    private int currentTurn;
    private int bankedTickets;
    private RunState state;

    private void Awake()
    {
        luckyWheelsPanel.SetActive(false);
    }

    private void Start()
    {
        spinButton.onClick.AddListener(OnSpinButtonPressed);
        closeButton.onClick.AddListener(CloseWheelMenu);

        for (int i = 0; i < streakSlots.Length; i++)
        {
            int idx = i;
            streakSlots[i].button.onClick.AddListener(() => TryBuyStreakReward(idx));
        }

        RefreshUI();
    }

    private void StartNewRun()
    {
        currentTurn = 1;
        bankedTickets = 0;
        state = RunState.ReadyToSpin;
        isSpinning = false;

        SetupStreakRewardsForRun();
        ResetWheelVisual();
        BuildSlicesForTurn(currentTurn);
        RefreshUI();
    }

    private void BuildSlicesForTurn(int turn)
    {
        slices.Clear();

        int skullCount = Mathf.Clamp(turn, 1, SliceCount - 1);
        int ticketCount = SliceCount - skullCount;

        for (int i = 0; i < skullCount; i++)
            slices.Add(new SliceData { type = SliceType.Skull });

        Vector2Int range = GetTicketRangeForTurn(turn);
        for (int i = 0; i < ticketCount; i++)
            slices.Add(new SliceData
            {
                type = SliceType.Ticket,
                tickets = UnityEngine.Random.Range(range.x, range.y + 1)
            });

        Shuffle(slices);
        RenderWheelLabels();
    }

    private void RenderWheelLabels()
    {
        int n = Mathf.Min(rewardText.Length, SliceCount);
        for (int i = 0; i < n; i++)
        {
            int dataIndex = Mod(i + SliceIndexOffset, SliceCount);
            var d = slices[dataIndex];
            rewardText[i].text = d.type == SliceType.Skull ? "x" : $"Ticket\n{d.tickets}";
        }
    }

    private Vector2Int GetTicketRangeForTurn(int turn)
    {
        if (turn == 1) return turn1TicketRange;
        if (turn == 2) return turn2TicketRange;
        if (turn == 3) return turn3TicketRange;
        if (turn == 4) return turn4TicketRange;
        return turn5TicketRange;
    }

    private bool TryPayGold(int cost)
    {
        if (cost <= 0) return true;
        if (PlayerController.instance.goldAmount < cost) return false;
        PlayerController.instance.goldAmount -= cost;
        return true;
    }

    private void OnSpinButtonPressed()
    {
        if (isSpinning) return;

        if (state == RunState.ReadyToSpin)
        {
            StartCoroutine(SpinRoutine());
            return;
        }

        if (state == RunState.CanGoNextTurnOrCashOut)
        {
            if (currentTurn >= 5) return;

            if (!TryPayGold(spinCostGold))
            {
                RefreshUI();
                return;
            }

            currentTurn++;
            state = RunState.ReadyToSpin;
            ResetWheelVisual();
            BuildSlicesForTurn(currentTurn);
            RefreshUI();
            return;
        }

        if (state == RunState.Busted)
        {
            if (!TryPayGold(spinCostGold)) return;
            StartNewRun();
        }
    }

    private IEnumerator SpinRoutine()
    {
        isSpinning = true;
        spinButton.gameObject.SetActive(false);
        RefreshUI();

        float sliceAngle = 360f / SliceCount;

        int dataIndex = UnityEngine.Random.Range(0, SliceCount);
        int visualIndex = Mod(dataIndex - SliceIndexOffset, SliceCount);

        float sliceCenter = visualIndex * sliceAngle + sliceAngle / 2f;
        float targetAngle = PointerAngle - sliceCenter;

        int fullRotations = UnityEngine.Random.Range(minFullRotations, maxFullRotations + 1);
        float totalAngle = (fullRotations * 360f + targetAngle) * WheelDirection;

        float start = wheel.eulerAngles.z;
        float end = start + totalAngle;

        float t = 0f;
        while (t < spinDuration)
        {
            t += Time.deltaTime;
            float n = Mathf.Clamp01(t / spinDuration);
            float eased = 1f - Mathf.Pow(1f - n, 3f);
            wheel.rotation = Quaternion.Euler(0, 0, Mathf.Lerp(start, end, eased));
            yield return null;
        }

        wheel.rotation = Quaternion.Euler(0, 0, end);

        isSpinning = false;

        ApplyResultByDataIndex(dataIndex);

        spinButton.gameObject.SetActive(true);
        RefreshUI();
    }

    private void ApplyResultByDataIndex(int dataIndex)
    {
        var landed = slices[dataIndex];

        if (landed.type == SliceType.Skull)
        {
            bankedTickets = 0;
            state = RunState.Busted;
            return;
        }

        bankedTickets += landed.tickets;
        state = RunState.CanGoNextTurnOrCashOut;
    }

    private void SetupStreakRewardsForRun()
    {
        foreach (var s in streakSlots)
        {
            s.claimed = false;

            s.chosenAmount = GetRandomMultipleOfFiveInRange(s.minAmount, s.maxAmount);

            s.rewardText.text = $"x{s.chosenAmount}";
            s.RequiredTicketText.text = s.ticketCost > 0 ? "x" + s.ticketCost.ToString() : "Free";
        }
    }

    private int GetRandomMultipleOfFiveInRange(int a, int b)
    {
        int min = Mathf.Min(a, b);
        int max = Mathf.Max(a, b);

        int first = ((min + 4) / 5) * 5;
        int last = (max / 5) * 5;

        if (first > last)
        {
            int snapped = (max / 5) * 5;
            return Mathf.Max(5, snapped);
        }

        int lo = first / 5;
        int hi = last / 5;
        return UnityEngine.Random.Range(lo, hi + 1) * 5;
    }

    private void RefreshStreakUI()
    {
        bool allowClaiming = !isSpinning && state == RunState.CanGoNextTurnOrCashOut;

        foreach (var s in streakSlots)
        {
            bool unlocked = currentTurn >= s.requiredTurn;
            bool affordable = bankedTickets >= s.ticketCost;
            bool canBuy = allowClaiming && unlocked && affordable && !s.claimed;

            s.blockerImage.gameObject.SetActive(!canBuy);
        }
    }

    private void TryBuyStreakReward(int index)
    {
        var s = streakSlots[index];
        if (s.claimed || bankedTickets < s.ticketCost || currentTurn < s.requiredTurn || state != RunState.CanGoNextTurnOrCashOut)
            return;

        bankedTickets -= s.ticketCost;
        GiveReward(s.fixedType, s.chosenAmount);
        s.claimed = true;
        RefreshUI();
    }

    private void GiveReward(RewardType type, int amount)
    {
        if (type == RewardType.Gold) PlayerController.instance.goldAmount += amount;
        else if (type == RewardType.Health) PlayerController.instance.health += amount;
        else if (type == RewardType.Ammo) PlayerController.instance.ammoAmount += amount;
        else if (type == RewardType.Bundle)
        {
            PlayerController.instance.goldAmount += amount;
            PlayerController.instance.health += amount;
            PlayerController.instance.ammoAmount += amount;
        }
    }

    private void RefreshUI()
    {
        ticketCount.text = bankedTickets.ToString();

        if (state == RunState.ReadyToSpin)
        {
            buttonLabel.text = "Spin";
            spinButton.interactable = !isSpinning;
        }
        else if (state == RunState.CanGoNextTurnOrCashOut)
        {
            buttonLabel.text = currentTurn < 5 ? $"Continue? ({spinCostGold}G)" : "Max Turn";
            spinButton.interactable = !isSpinning && currentTurn < 5 && PlayerController.instance.goldAmount >= spinCostGold;
        }
        else
        {
            buttonLabel.text = $"Restart ({spinCostGold}G)";
            spinButton.interactable = !isSpinning && PlayerController.instance.goldAmount >= spinCostGold;
        }

        RefreshStreakUI();
    }

    private void Shuffle<T>(IList<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int j = UnityEngine.Random.Range(i, list.Count);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }

    private int Mod(int a, int m)
    {
        int r = a % m;
        return r < 0 ? r + m : r;
    }

    private void ResetWheelVisual()
    {
        wheel.rotation = Quaternion.Euler(0, 0, WheelDefaultZ);
    }

    public void OpenWheelMenu()
    {
        luckyWheelsPanel.SetActive(true);
        UI_Canvas.instance.ShouldStopTheGame(true);
        StartNewRun();
    }

    public void CloseWheelMenu()
    {
        luckyWheelsPanel.SetActive(false);
        UI_Canvas.instance.ShouldStopTheGame(false);
    }
}
