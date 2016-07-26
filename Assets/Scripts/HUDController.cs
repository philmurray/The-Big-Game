using UnityEngine;
using System.Collections;
using Assets.Scripts.DataStructures;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class HUDController : MonoBehaviour {
    
    public Text SmallBlockCount;
    public Text MediumBlockCount;
    public Text LargeBlockCount;
    public Text HugeBlockCount;
    public Text FlagCount;
    public Text CrystalCount;
    public GameObject UpgradeGameObject;

    public RectTransform UpgradeBox;
    public List<HUDUpgrade> Upgrades;
    private Dictionary<PlayerState.Upgrade, Sprite> _upgrades;

    private List<GameObject> _upgradeGameObjects = new List<GameObject>();

    [Serializable]
    public class HUDUpgrade {
        public PlayerState.Upgrade Upgrade;
        public Sprite Sprite;
    }

    // Update is called once per frame
    void Start () {
        _upgrades = new Dictionary<PlayerState.Upgrade, Sprite>();
        Upgrades.ForEach(u => _upgrades.Add(u.Upgrade, u.Sprite));

        Refresh();
	}

    void Refresh() {
        SmallBlockCount.text = GameController.instance.PlayerState.AvailableSmallBlocks.ToString();
        MediumBlockCount.text = GameController.instance.PlayerState.AvailableMediumBlocks.ToString();
        LargeBlockCount.text = GameController.instance.PlayerState.AvailableLargeBlocks.ToString();
        HugeBlockCount.text = GameController.instance.PlayerState.AvailableHugeBlocks.ToString();
        FlagCount.text = GameController.instance.PlayerState.AvailableFlags.ToString();
        CrystalCount.text = GameController.instance.PlayerState.AvailableCrystals.ToString();

        _upgradeGameObjects.ForEach(o => Destroy(o));
        float bottom = 0;
        foreach (var upgrade in GameController.instance.PlayerState.Upgrades)
        {
            GameObject upgradeInstance = Instantiate(UpgradeGameObject);
            Image image = upgradeInstance.GetComponent<Image>();
            image.sprite = _upgrades[upgrade];

            RectTransform transform = upgradeInstance.GetComponent<RectTransform>();
            transform.parent = UpgradeBox;
            transform.anchoredPosition = new Vector3(0,bottom,0);
            transform.localScale = Vector3.one;
            bottom += transform.rect.height;
        }
    }
}
