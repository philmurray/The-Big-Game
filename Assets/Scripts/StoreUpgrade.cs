﻿using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using Assets.Scripts.DataStructures;

public class StoreUpgrade : MonoBehaviour {

    public PlayerState.Upgrade Upgrade;

    public bool Buyable;
    public int MaxAvailable;
    public int MinAvailable;
    public int Cost;
    public Text CostText;

    public Block.BlockType AvailableType;
    public Text AvailableText;

    public Button AddButton;
    public Button RemoveButton;

    void Start()
    {
        CostText.text = Cost > 0 ? Cost.ToString() + " Gold" : "";
        Refresh();
    }

    public void Refresh()
    {
        if (AvailableText != null)
        {
            AvailableText.text = GameController.instance.ActivePlayerState.GetAvailableBlocks(AvailableType).ToString();
        }
        if (AddButton != null)
        {
            bool active = Cost <= GameController.instance.ActivePlayerState.Gold;
            if (active)
            {
                if (AvailableText != null)
                {
                    active = GameController.instance.ActivePlayerState.GetAvailableBlocks(AvailableType) < MaxAvailable;
                }
                else
                {
                    active = !GameController.instance.ActivePlayerState.Upgrades.Contains(Upgrade);
                }
            }
            AddButton.interactable = active;
        }
        if (RemoveButton != null)
        {
            RemoveButton.gameObject.SetActive(GameController.instance.ActivePlayerState.GetAvailableBlocks(AvailableType) > MinAvailable);
        }
    }

    public void Buy()
    {
        if (GameController.instance.ActivePlayerState.Gold >= Cost)
        {
            BuildingSceneController.instance.ShopModal.AddUpgrade(Upgrade, Cost);
        }
    }

    public void Remove()
    {
        if (GameController.instance.ActivePlayerState.GetAvailableBlocks(AvailableType) > 0)
        {
            BuildingSceneController.instance.ShopModal.RemoveUpgrade(Upgrade, Cost);
        }
    }
}
