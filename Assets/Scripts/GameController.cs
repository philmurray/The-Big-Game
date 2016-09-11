using UnityEngine;
using System.Collections;
using Assets.Scripts.DataStructures;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GameController : MonoBehaviour
{

    public PlayerState ActivePlayerState {
        get { return ActiveGamePlayer.State; }
    }
    public List<Block> ActivePlayerBlocks {
        get { return ActiveGamePlayer.Blocks; }
    }
    public Weapon ActivePlayerWeapon
    {
        get { return ActiveGamePlayer.Weapon; }
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
        public Weapon Weapon;
    }

    public GamePlayer PlayerOne;
    public GamePlayer PlayerTwo;

    private Stack<GamePlayer> _playerOneBackup = new Stack<GamePlayer>();
    private Stack<GamePlayer> _playerTwoBackup = new Stack<GamePlayer>();

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
    public Player OtherPlayer {
        get {
            return ActivePlayer == Player.One ? Player.Two : Player.One;
        }
    }

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

    public GamePlayer GetPlayer(Player p) {
        if (p == Player.One)
        {
            return PlayerOne;
        }
        else
        {
            return PlayerTwo;
        }
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
                SceneManager.LoadScene("Building");
                break;
            case "Building":
                if (ActivePlayer == Player.One && PlayerTwo.IsHuman)
                {
                    ActivePlayer = Player.Two;
                    SceneManager.LoadScene("Upgrades");
                }
                else
                {
                    SceneManager.LoadScene("Shooting");
                    SaveState();
                }
                break;
        }
    }
    private void SaveState()
    {
        _playerOneBackup.Push(DeepClone<GamePlayer>(PlayerOne));
        _playerTwoBackup.Push(DeepClone<GamePlayer>(PlayerTwo));
    }
    private void RetrieveState()
    {
        PlayerOne = _playerOneBackup.Pop();
        PlayerTwo = _playerTwoBackup.Pop();
    }

    public static T DeepClone<T>(T obj)
    {
        using (var ms = new MemoryStream())
        {
            var formatter = new BinaryFormatter();
            formatter.Serialize(ms, obj);
            ms.Position = 0;

            return (T)formatter.Deserialize(ms);
        }
    }
}
