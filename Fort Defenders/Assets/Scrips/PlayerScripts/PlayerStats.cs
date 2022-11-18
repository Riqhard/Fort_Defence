using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{

    public int playerGold;
    public int playerFood;
    public GameUI gameUI;

    private void Start()
    {
        gameUI.ChangeGoldAmount(playerGold);
        gameUI.ChangeFoodAmount(playerFood);
    }

    public void GiveGold(int amount)
    {
        playerGold += amount;
        gameUI.ChangeGoldAmount(amount);
    }
    public void TakeGold(int amount)
    {
        playerGold -= amount;
        gameUI.ChangeGoldAmount(-amount);
    }

    public void GiveFood(int amount)
    {
        playerFood += amount;
        gameUI.ChangeFoodAmount(amount);
    }
    public void TakeFood(int amount)
    {
        playerFood -= amount;
        gameUI.ChangeFoodAmount(-amount);
    }
}
