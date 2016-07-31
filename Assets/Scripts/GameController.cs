using UnityEngine;
using System.Collections;
using Assets.Scripts.DataStructures;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

    public PlayerState PlayerState;
    public List<Block> Blocks;

    public Config Config;

    public static GameController instance;

    void Awake() {
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

    void Start() {
        Config.Initialize();
    }

}
