using System.Collections.Generic;
using UnityEngine;

public class In_Game_Market_Manager : MonoBehaviour
{
    [Header("UI")]
    public GameObject inGameMarketPanel;
    public Transform cardsParent;
    public MarketItemOptionUI cardPrefab;

    [Header("Market Pool")]
    public List<InGameMarketData> allMarketItems = new();

    [Header("Spawn")]
    public int spawnCardCount = 3;

    bool isMenuOpen;
    readonly List<MarketItemOptionUI> spawnedCards = new();

    private void Update()
    {
        if (isMenuOpen)
            return;

        if (PlayerPrefs.GetInt("Open_InGameMarket") == 1)
        {
            OpenInGameMarket();
        }
    }

    public void OpenInGameMarket()
    {
        PlayerPrefs.SetInt("Open_InGameMarket", 1);

        isMenuOpen = true;

        GameTester.Instance.ShouldStopTheGame(true);

        inGameMarketPanel.SetActive(true);

        RollCards();
    }

    public void CloseInGameMarket()
    {
        PlayerPrefs.SetInt("Open_InGameMarket", 0);

        inGameMarketPanel.SetActive(false);

        isMenuOpen = false;

        GameTester.Instance.ShouldStopTheGame(false);

        ClearOldCards();
    }

    void RollCards()
    {
        ClearOldCards();
        SpawnRandomMarketCards(spawnCardCount);
    }

    void ClearOldCards()
    {
        foreach (var card in spawnedCards)
        {
            if (card != null)
                Destroy(card.gameObject);
        }
        spawnedCards.Clear();
    }

    void SpawnRandomMarketCards(int count)
    {
        if (allMarketItems == null || allMarketItems.Count == 0)
        {
            CloseInGameMarket();
            return;
        }

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
        ApplyMarketItemToPlayer(chosenItem);
        CloseInGameMarket();
    }

    void ApplyMarketItemToPlayer(InGameMarketData item)
    {
        if (item == null) return;

        switch (item.type)
        {
            case Market_Item_Type.Market_Refill_Health:
                Debug.Log("Health");
                break;

            case Market_Item_Type.Market_Refill_Ammo:
                Debug.Log("Ammo");
                break;

            case Market_Item_Type.Market_Select_RandomSkill:
                Debug.Log("Skill");
                //PlayerPrefs.SetInt("Open_Roguelike", 1);
                break;
        }
    }
}
