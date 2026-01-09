using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class In_Game_Market_Manager : MonoBehaviour
{
    [Header("UI")]
    public GameObject inGameMarketPanel;
    public Transform cardsParent;
    public MarketItemOptionUI cardPrefab;
    public GameObject completePurchaseBtn;
    public TMPTypewriter bubbleText;
    public Color purchaseBtnNormalColor;
    public Color purchaseBtnNotEnoughColor;

    [Header("Buttons")]
    public Button closeInGameMarketBtn;
    Image completePurchaseImage;
    Button completePurchaseButton;

    [Header("Market Pool")]
    public List<InGameMarketData> allMarketItems = new();

    [Header("Spawn")]
    public int spawnCardCount = 3;

    readonly List<MarketItemOptionUI> spawnedCards = new();

    [TextArea(2, 4)]
    public string[] bubbleTextArea_Health;
    public string[] bubbleTextArea_Ammo;
    public string[] bubbleTextArea_RandomSkill;

    InGameMarketData pendingItem;

    private void Awake()
    {
        inGameMarketPanel.SetActive(false);
        completePurchaseImage = completePurchaseBtn.GetComponent<Image>();
        completePurchaseButton = completePurchaseBtn.GetComponent<Button>();
    }

    private void Start()
    {
        closeInGameMarketBtn.onClick.AddListener(CloseInGameMarket);
        completePurchaseButton.onClick.AddListener(ConfirmPurchase);
    }

    public void OpenInGameMarket()
    {
        UI_Canvas.instance.ShouldStopTheGame(true);
        inGameMarketPanel.SetActive(true);

        pendingItem = null;
        completePurchaseBtn.SetActive(false);

        RollCards();
    }

    public void CloseInGameMarket()
    {
        inGameMarketPanel.SetActive(false);
        UI_Canvas.instance.ShouldStopTheGame(false);

        ClearOldCards();

        pendingItem = null;
        completePurchaseBtn.SetActive(false);
    }

    void RollCards()
    {
        ClearOldCards();
        SpawnRandomMarketCards(spawnCardCount);
    }

    void ClearOldCards()
    {
        foreach (var card in spawnedCards)
            Destroy(card.gameObject);

        spawnedCards.Clear();
    }

    void SpawnRandomMarketCards(int count)
    {
        List<InGameMarketData> pool = new(allMarketItems);
        int finalCount = Mathf.Min(count, pool.Count);

        for (int i = 0; i < finalCount; i++)
        {
            int index = Random.Range(0, pool.Count);
            InGameMarketData chosen = pool[index];
            pool.RemoveAt(index);

            MarketItemOptionUI card = Instantiate(cardPrefab, cardsParent);
            card.Setup(chosen, OnMarketItemSelected);

            spawnedCards.Add(card);
        }
    }

    void OnMarketItemSelected(InGameMarketData chosenItem)
    {
        pendingItem = chosenItem;

        ShowPreviewForItem(chosenItem);
        ShowCompletePurchaseButton(chosenItem);
    }

    void ShowPreviewForItem(InGameMarketData item)
    {
        switch (item.type)
        {
            case Market_Item_Type.Market_Refill_Health:
                bubbleText.Play(bubbleTextArea_Health[Random.Range(0, bubbleTextArea_Health.Length)]);
                break;

            case Market_Item_Type.Market_Refill_Ammo:
                bubbleText.Play(bubbleTextArea_Ammo[Random.Range(0, bubbleTextArea_Ammo.Length)]);
                break;

            case Market_Item_Type.Market_Select_RandomSkill:
                bubbleText.Play(bubbleTextArea_RandomSkill[Random.Range(0, bubbleTextArea_RandomSkill.Length)]);
                break;
        }
    }

    public void ConfirmPurchase()
    {
        if (pendingItem == null)
            return;

        if (PlayerController.instance.goldAmount < pendingItem.price)
            return;

        PlayerController.instance.AddGold(-pendingItem.price);
        ApplyMarketItemToPlayer(pendingItem);

        pendingItem = null;
        completePurchaseBtn.SetActive(false);

        CloseInGameMarket();
    }

    void ApplyMarketItemToPlayer(InGameMarketData item)
    {
        switch (item.type)
        {
            case Market_Item_Type.Market_Refill_Health:
                PlayerController.instance.health += 100;
                break;

            case Market_Item_Type.Market_Refill_Ammo:
                PlayerController.instance.ammoAmount += 100;
                break;

            case Market_Item_Type.Market_Select_RandomSkill:
                UI_Canvas.instance.SkillSelectionPanelState(true);
                break;
        }
    }

    void ShowCompletePurchaseButton(InGameMarketData item)
    {
        bool canBuy = PlayerController.instance.goldAmount >= item.price;

        completePurchaseImage.color = canBuy ? purchaseBtnNormalColor : purchaseBtnNotEnoughColor;
        completePurchaseButton.interactable = canBuy;

        completePurchaseBtn.SetActive(true);
    }
}
