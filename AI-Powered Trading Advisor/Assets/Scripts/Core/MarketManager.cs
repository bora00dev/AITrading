using UnityEngine;
using System.Collections.Generic;

public class MarketManager : MonoBehaviour
{
    public List<Commodity> commodities = new List<Commodity>();

    public delegate void OnMarketUpdate();
    public static event OnMarketUpdate MarketUpdated;

    void Start()
    {
        commodities.Add(new Commodity("Gold", 100));
        commodities.Add(new Commodity("Oil", 60));
        commodities.Add(new Commodity("Tech", 200));

        InvokeRepeating(nameof(UpdateMarket), 10f, 15f);
    }

    void UpdateMarket()
    {
        foreach (var c in commodities)
            c.UpdatePrice();

        MarketUpdated?.Invoke();
    }

    public string GetMarketSummary()
    {
        string summary = "";
        foreach (var c in commodities)
            summary += $"{c.name}: ${c.price:F2}\n";
        return summary;
    }
}