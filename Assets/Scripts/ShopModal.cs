using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.Scripts.DataStructures;

public class ShopModal : MonoBehaviour {
    
    public Text GoldText;
    public UpgradesContainer UpgradesContainer;

    void Start()
    {
        Refresh();
    }

    public void UpdateBlocks()
    {
        foreach (var upgrade in GetComponentsInChildren<StoreUpgrade>())
        {
            upgrade.Refresh();
        }
    }

    public void UpdateGold()
    {
        GoldText.text = "Gold: " + GameController.instance.ActivePlayerState.Gold.ToString();
    }

    public void Refresh()
    {
        UpdateBlocks();
        UpdateGold();
        UpgradesContainer.Refresh();
    }


    public void AddUpgrade(PlayerState.Upgrade upgrade, int cost)
    {
        GameController.instance.ActivePlayerState.ApplyUpgrade(upgrade);
        GameController.instance.ActivePlayerState.Gold -= cost;

        Refresh();
    }

    public void RemoveUpgrade(PlayerState.Upgrade upgrade, int cost)
    {
        GameController.instance.ActivePlayerState.RemoveUpgrade(upgrade);
        GameController.instance.ActivePlayerState.Gold += cost;

        Refresh();
    }
}
