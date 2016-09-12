using UnityEngine;
using System.Collections;
using Assets.Scripts.DataStructures;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class HUDController : MonoBehaviour {

    public static HUDController instance;

    void Awake()
    {
        instance = this;
    }

    public Text SmallBlockCount;
    public Text MediumBlockCount;
    public Text LargeBlockCount;
    public Text HugeBlockCount;
    public Text FlagCount;
    public Text CrystalCount;

    public RectTransform UpgradeBox;
    public RectTransform BlockUpgradeBox;
    public List<HUDUpgrade> Upgrades;
    private Dictionary<PlayerState.Upgrade, GameObject> _upgradesDictionary;
    private Dictionary<PlayerState.Upgrade, GameObject> UpgradesDictionary {
        get
        {
            if (_upgradesDictionary == null) {
                _upgradesDictionary = new Dictionary<PlayerState.Upgrade, GameObject>();
                Upgrades.ForEach(u => _upgradesDictionary.Add(u.Upgrade, u.GameObject));
            }
            return _upgradesDictionary;
        }
    }

    private List<GameObject> _upgradeGameObjects = new List<GameObject>();

    [Serializable]
    public class HUDUpgrade {
        public PlayerState.Upgrade Upgrade;
        public GameObject GameObject;
    }

    // Update is called once per frame
    void Start () {
        Refresh();
	}

    public void Refresh() {
        SmallBlockCount.text = GameController.instance.ActivePlayerState.AvailableSmallBlocks.ToString();
        MediumBlockCount.text = GameController.instance.ActivePlayerState.AvailableMediumBlocks.ToString();
        LargeBlockCount.text = GameController.instance.ActivePlayerState.AvailableLargeBlocks.ToString();
        HugeBlockCount.text = GameController.instance.ActivePlayerState.AvailableHugeBlocks.ToString();
        FlagCount.text = GameController.instance.ActivePlayerState.AvailableFlags.ToString();
        CrystalCount.text = GameController.instance.ActivePlayerState.AvailableCrystals.ToString();

        _upgradeGameObjects.ForEach(o => Destroy(o));
        AddUpgrades(UpgradeBox, GameController.instance.ActivePlayerState.Upgrades);

        List<PlayerState.Upgrade> blockUpgrades = new List<PlayerState.Upgrade>();
        foreach (var block in GameController.instance.ActivePlayerBlocks)
        {
            if (block.Type == Block.BlockType.Flag)
            {
                blockUpgrades.Add(PlayerState.Upgrade.flag);
            }
        }
        AddUpgrades(BlockUpgradeBox, blockUpgrades);
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
