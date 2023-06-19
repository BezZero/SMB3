using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    public int coinCount;

    void Start()
    {
        coinCount = PlayerPrefs.GetInt("CoinCount", 0); // 0 is the default value
    }

    public void BuyItem()
    {
        int itemCost = 10;

        if (coinCount >= itemCost)
        {
            coinCount -= itemCost;
            PlayerPrefs.SetInt("CoinCount", coinCount);
        }
    }
}
