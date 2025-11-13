using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public float playerMoney = 1000f;

    void Awake() => Instance = this;

    public bool BuyCommodity(Commodity c)
    {
        if (playerMoney >= c.price)
        {
            playerMoney -= c.price;
            c.quantityOwned++;
            return true;
        }
        return false;
    }

    public bool SellCommodity(Commodity c)
    {
        if (c.quantityOwned > 0)
        {
            playerMoney += c.price;
            c.quantityOwned--;
            return true;
        }
        return false;
    }
}