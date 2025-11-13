using UnityEngine;
using TMPro;

public class UIButtonHandler : MonoBehaviour
{
    public TextMeshProUGUI buttonLabel;
    public string commodityName;
    private MarketManager market;

    void Start()
    {
        market = FindObjectOfType<MarketManager>();
    }

    public void OnBuy()
    {
        var commodity = market.commodities.Find(c => c.name == commodityName);
        if (commodity == null)
            return;

        bool success = GameManager.Instance.BuyCommodity(commodity);
        if (success)
            Debug.Log($"[Trade] Bought 1x {commodityName} for ${commodity.price:F2}");
        else
            Debug.Log("[Trade] Not enough money to buy " + commodityName + "!");
    }

    public void OnSell()
    {
        var commodity = market.commodities.Find(c => c.name == commodityName);
        if (commodity == null)
            return;

        bool success = GameManager.Instance.SellCommodity(commodity);
        if (success)
            Debug.Log($"[Trade] Sold 1x {commodityName} for ${commodity.price:F2}");
        else
            Debug.Log("[Trade] No " + commodityName + " to sell!");
    }
}
