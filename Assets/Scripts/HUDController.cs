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
    private Dictionary<PlayerState.Upgrade, Sprite> _upgradesDictionary;
    private Dictionary<PlayerState.Upgrade, Sprite> UpgradesDictionary {
        get
        {
            if (_upgradesDictionary == null) {
                _upgradesDictionary = new Dictionary<PlayerState.Upgrade, Sprite>();
                Upgrades.ForEach(u => _upgradesDictionary.Add(u.Upgrade, u.Sprite));
            }
            return _upgradesDictionary;
        }
    }

    private List<GameObject> _upgradeGameObjects = new List<GameObject>();

    [Serializable]
    public class HUDUpgrade {
        public PlayerState.Upgrade Upgrade;
        public Sprite Sprite;
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
        float bottom = 0;
        foreach (var upgrade in GameController.instance.ActivePlayerState.Upgrades)
        {
            GameObject upgradeInstance = Instantiate(UpgradeGameObject);
            Image image = upgradeInstance.GetComponent<Image>();
            image.sprite = UpgradesDictionary[upgrade];

            RectTransform transform = upgradeInstance.GetComponent<RectTransform>();
            transform.SetParent(UpgradeBox, false);
            transform.anchoredPosition = new Vector3(0,bottom,0);
            transform.localScale = Vector3.one;
            bottom += transform.rect.height;
        }
    }
}
