using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Assets.Scripts.DataStructures;

public class UpgradeSceneController : MonoBehaviour {

    public static UpgradeSceneController instance;

    void Awake()
    {
        instance = this;
    }


    public States SceneState = States.Intro;
    public float PlayTime;
    public float StopTime;
    public float UpgradeTimeBase;
    
    public float UpgradeTimeRandom;
    public List<UpgradeConfig> Upgrades;
    public float UpgradeXRange;
    public float UpgradeY;

    public float VelocityNormal;
    public float VelocityPremium;
    public float StopAtPremium;
    public float StopForPremium;

    private float StateStart;

    public enum States { Intro, Playing, Stopping };
    private float TotalRarity;

    [Serializable]
    public class UpgradeConfig
    {
        public GameObject UpgradeObject;
        public float Rarity;
        public bool IsPremium;
    }

    void Start() {
        HUDController.instance.gameObject.SetActive(false);

        Upgrades.ForEach(u => TotalRarity += u.Rarity);
    }

    IEnumerator SpawnUpgrades() {
        while (Time.fixedTime - StateStart < PlayTime) {
            var upgradeTypeNum = UnityEngine.Random.Range(0, TotalRarity);
            for (int i = 0, l = Upgrades.Count; i<l; i++) {
                var upgrade = Upgrades[i];
                if (upgradeTypeNum <= upgrade.Rarity)
                {
                    GameObject o = Instantiate(upgrade.UpgradeObject, new Vector3(UnityEngine.Random.Range(-UpgradeXRange, UpgradeXRange), UpgradeY, 0), Quaternion.identity) as GameObject;
                    if (upgrade.IsPremium)
                    {
                        o.GetComponent<FallingUpgrade>().SetUpgrade(VelocityPremium, StopAtPremium, StopForPremium);
                    }
                    else
                    {
                        o.GetComponent<FallingUpgrade>().SetUpgrade(VelocityNormal, -1, -1);
                    }
                    break;
                }
                upgradeTypeNum -= upgrade.Rarity;
            }

            yield return new WaitForSeconds(UpgradeTimeBase + UnityEngine.Random.Range(0, UpgradeTimeRandom));
        }
        StartCoroutine(StopGame());
    }

    IEnumerator StopGame() {
        SceneState = States.Stopping;
        StateStart = Time.fixedTime;
        yield return new WaitForSeconds(StopTime);
        GameController.instance.NextScene(true);
    }

    public void StartGame()
    {
        Destroy(GameObject.FindGameObjectWithTag("Modal"));
        HUDController.instance.gameObject.SetActive(true);
        SceneState = States.Playing;
        StateStart = Time.fixedTime;
        StartCoroutine(SpawnUpgrades());
    }

    public void ApplyUpgrade(PlayerState.Upgrade upgrade)
    {
        GameController.instance.ActivePlayerState.ApplyUpgrade(upgrade);
        HUDController.instance.Refresh();
    }
}
