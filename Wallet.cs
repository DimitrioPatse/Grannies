using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wallet : MonoBehaviour, ISaveable
{
    int money;
    [SerializeField] int specialCollectible;

    public int GetMoney() { return money; }
    public int GetSpecial() { return specialCollectible; }
    public void SpendMoney (int price)
    {
        if(money >= price)
        {
            money -= price;
        }
    }
    public void SpendSpecial(int price)
    {
        if (specialCollectible >= price)
        {
            specialCollectible -= price;
        }
    }

    public void EarnMoney(int new_money)
    {
        money += new_money;
    }
    public void EarnGems(int gems)
    {
        specialCollectible += gems;
    }

    public object CaptureState()
    {
        int[] walletData = new int[] { money, specialCollectible};
        return walletData;
    }

    public void RestoreState(object state)
    {
        int[] walletData = (int[])state;
        money = walletData[0];
        specialCollectible = walletData[1];
    }
}
