using UnityEngine;

[System.Serializable]
public class Commodity
{
    public string name;
    public float price;
    public int quantityOwned;

    public Commodity(string name, float startPrice)
    {
        this.name = name;
        this.price = startPrice;
        this.quantityOwned = 0;
    }

    public void UpdatePrice()
    {
        float change = Random.Range(-5f, 5f);
        price = Mathf.Max(1f, price + change);
    }
}