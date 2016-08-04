using UnityEngine;
using System.Collections;
using Assets.Scripts.DataStructures;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;

public class GameController : MonoBehaviour
{

    public PlayerState ActivePlayerState {
        get { return ActiveGamePlayer.State; }
    }
    public List<Block> ActivePlayerBlocks {
        get { return ActiveGamePlayer.Blocks; }
    }

    public enum Player {
        One,
        Two
    }
    [Serializable]
    public class GamePlayer {
        public bool IsHuman;
        public Player Player;
        public PlayerState State;
        public List<Block> Blocks;
    }

    public GamePlayer PlayerOne;
    public GamePlayer PlayerTwo;

    public GamePlayer ActiveGamePlayer {
        get {
            if (ActivePlayer == Player.One)
            {
                return PlayerOne;
            }
            else
            {
                return PlayerTwo;
            }
        }
    }

    public Player ActivePlayer;

    public Config Config;

    public static GameController instance;

    void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Config.Initialize();
    }

    public void NextScene() {
        NextScene(false);
    }
    public void NextScene(bool fadeOut)
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "MainMenu":
                SceneManager.LoadScene("Upgrades");
                break;
            case "Upgrades":
                UpgradeSceneController.instance = null;
                SceneManager.LoadScene("Building");
                break;
            case "Building":
                BuildingSceneController.instance = null;
                if (ActivePlayer == Player.One && PlayerTwo.IsHuman)
                {
                    ActivePlayer = Player.Two;
                    SceneManager.LoadScene("Upgrades");
                }
                else
                {
                    SceneManager.LoadScene("Shooting");
                }
                break;
        }
    }
}
