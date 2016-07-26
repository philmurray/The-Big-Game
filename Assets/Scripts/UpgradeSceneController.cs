using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Assets.Scripts.DataStructures;

public class UpgradeSceneController : MonoBehaviour {

    public enum States { Intro, Playing, Stopping };
    public States SceneState = States.Intro;
    private float StateStart;
    private CanvasGroup HUD;

    public float PlayTime;

    public List<FallingUpgrade> Upgrades;

    private List<GameObject> _upgradeGameObjects = new List<GameObject>();

    [Serializable]
    public class FallingUpgrade
    {
        public PlayerState.Upgrade Upgrade;
        public Sprite Sprite;
        public float Rarity;
    }

    void Start() {
        HUD = GameObject.FindGameObjectWithTag("HUD").GetComponent<CanvasGroup>();
        HideHUD();
    }

    IEnumerator SpawnUpgrades() {
        while (Time.fixedTime - StateStart < PlayTime) {
            //make some upgrades fall
            yield return new WaitForSeconds(1);
        }
        SceneState = States.Stopping;
        StateStart = Time.fixedTime;
    }

    public void StartGame()
    {
        Destroy(GameObject.FindGameObjectWithTag("Modal"));
        ShowHUD();
        SceneState = States.Playing;
        StateStart = Time.fixedTime;
    }

    private void HideHUD() {
        HUD.alpha = 0;
    }

    private void ShowHUD() {
        HUD.alpha = 1;
    }
}
