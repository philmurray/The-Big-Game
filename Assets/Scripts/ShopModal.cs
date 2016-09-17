using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.Scripts.DataStructures;
using System.Collections.Generic;

public class ShopModal : MonoBehaviour {
    
    public Text GoldText;
    public Text CrystalsText;
    public Button RestockButton;
    public UpgradesContainer UpgradesContainer;

    public List<Transform> UpgradeSpots;

    public List<StoreUpgrade> AllAvailableUpgrades;

    void Start()
    {
        Refresh();
        RestockUpgrades();
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

    public void UpdateCrystals()
    {
        CrystalsText.text = "Crystals: " + GameController.instance.ActivePlayerState.Crystals.ToString();
        RestockButton.gameObject.SetActive(GameController.instance.ActivePlayerState.Crystals > 0);
    }

    public void Refresh()
    {
        UpdateBlocks();
        UpdateGold();
        UpdateCrystals();
        UpgradesContainer.Refresh();
    }

    private void RemoveUpgradeFromlist(List<PlayerState.Upgrade> list, PlayerState.Upgrade upgrade)
    {
        list.Remove(upgrade);
        switch (upgrade)
        {
            case PlayerState.Upgrade.block_health_two:
                list.Remove(PlayerState.Upgrade.block_health_three);
                break;
            case PlayerState.Upgrade.block_health_three:
                list.Remove(PlayerState.Upgrade.block_health_one);
                list.Remove(PlayerState.Upgrade.block_health_two);
                break;
            case PlayerState.Upgrade.block_weight_two:
                list.Remove(PlayerState.Upgrade.block_weight_three);
                break;
            case PlayerState.Upgrade.block_weight_three:
                list.Remove(PlayerState.Upgrade.block_weight_one);
                list.Remove(PlayerState.Upgrade.block_weight_two);
                break;
            case PlayerState.Upgrade.cat_weight_two:
                list.Remove(PlayerState.Upgrade.cat_weight_three);
                break;
            case PlayerState.Upgrade.cat_weight_three:
                list.Remove(PlayerState.Upgrade.cat_weight_one);
                list.Remove(PlayerState.Upgrade.cat_weight_two);
                break;
            case PlayerState.Upgrade.cross_speed_two:
                list.Remove(PlayerState.Upgrade.cross_speed_three);
                break;
            case PlayerState.Upgrade.cross_speed_three:
                list.Remove(PlayerState.Upgrade.cross_speed_one);
                list.Remove(PlayerState.Upgrade.cross_speed_two);
                break;
            case PlayerState.Upgrade.flag_two:
                list.Remove(PlayerState.Upgrade.flag_three);
                break;
            case PlayerState.Upgrade.flag_three:
                list.Remove(PlayerState.Upgrade.flag_one);
                list.Remove(PlayerState.Upgrade.flag_two);
                break;
        }
    }

    public void RestockUpgrades()
    {
        var currentAvailableUpgrades = AllAvailableUpgrades.ConvertAll(p => p.Upgrade);

        foreach (var upgrade in GameController.instance.ActivePlayerState.Upgrades)
        {
            RemoveUpgradeFromlist(currentAvailableUpgrades, upgrade);
        }

        foreach (Transform storeUpgrade in UpgradeSpots)
        {
            if (storeUpgrade.childCount > 0)
            {
                Destroy(storeUpgrade.GetChild(0).gameObject);
            }

            var upgrade = currentAvailableUpgrades[Random.Range(0, currentAvailableUpgrades.Count)];
            Instantiate(AllAvailableUpgrades.Find(u => u.Upgrade == upgrade), storeUpgrade, false);
            RemoveUpgradeFromlist(currentAvailableUpgrades, upgrade);
        }
    }

    public void BuyRestock()
    {
        if (GameController.instance.ActivePlayerState.Crystals > 0)
        {
            GameController.instance.ActivePlayerState.Crystals--;
            RestockUpgrades();
            Refresh();
        }
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
