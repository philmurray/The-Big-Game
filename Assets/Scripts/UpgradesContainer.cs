using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.DataStructures;
using System;

public class UpgradesContainer : MonoBehaviour {

    public List<HUDUpgrade> Upgrades;
    private Dictionary<PlayerState.Upgrade, GameObject> _upgradesDictionary;
    private Dictionary<PlayerState.Upgrade, GameObject> UpgradesDictionary
    {
        get
        {
            if (_upgradesDictionary == null)
            {
                _upgradesDictionary = new Dictionary<PlayerState.Upgrade, GameObject>();
                Upgrades.ForEach(u => _upgradesDictionary.Add(u.Upgrade, u.GameObject));
            }
            return _upgradesDictionary;
        }
    }

    private List<GameObject> _upgradeGameObjects = new List<GameObject>();

    [Serializable]
    public class HUDUpgrade
    {
        public PlayerState.Upgrade Upgrade;
        public GameObject GameObject;
    }

    public void Refresh() {
        _upgradeGameObjects.ForEach(o => Destroy(o));

        List<PlayerState.Upgrade> upgrades = new List<PlayerState.Upgrade>(GameController.instance.ActivePlayerState.Upgrades);
        if (GameController.instance.ActivePlayerBlocks.Exists(b => b.Type == Block.BlockType.Flag))
        {
            upgrades.Add(PlayerState.Upgrade.flag);
        }
        AddUpgrades((RectTransform)transform, upgrades);
    }

    private void AddUpgrades(RectTransform parent, List<PlayerState.Upgrade> upgrades)
    {
        float bottom = 0;
        foreach (var upgrade in upgrades)
        {
            GameObject upgradeInstance = Instantiate(UpgradesDictionary[upgrade]);

            RectTransform transform = upgradeInstance.GetComponent<RectTransform>();
            transform.SetParent(parent, false);
            transform.anchoredPosition = new Vector3(0, bottom, 0);
            transform.localScale = Vector3.one;
            bottom += transform.rect.height;

            _upgradeGameObjects.Add(upgradeInstance);
        }
    }
}
