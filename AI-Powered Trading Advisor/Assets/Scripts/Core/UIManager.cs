using UnityEngine;
using TMPro;
using System.Text;

public class UIManager : MonoBehaviour
{
    [Header("TMP UI References")]
    public TextMeshProUGUI marketText;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI aiAdviceText;

    private MarketManager market;

    void Start()
    {
        market = FindObjectOfType<MarketManager>();
        if (market != null)
            MarketManager.MarketUpdated += RefreshUI;
        RefreshUI();
    }

    void OnDestroy()
    {
        MarketManager.MarketUpdated -= RefreshUI;
    }

    void RefreshUI()
    {
        if (market == null)
            return;

        StringBuilder sb = new StringBuilder();
        foreach (var c in market.commodities)
            sb.AppendLine($"{c.name} - ${c.price:F2} | Owned: {c.quantityOwned}");

        marketText.text = sb.ToString();
        moneyText.text = $"Money: ${GameManager.Instance.playerMoney:F2}";
    }

    public void UpdateAIAdvice(string advice)
    {
        aiAdviceText.text = $"AI Tip: {advice}";
    }
}
