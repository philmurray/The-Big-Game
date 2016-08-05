using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ShootingSceneController : MonoBehaviour {

    public static ShootingSceneController instance;

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
    public enum States { Intro, PlayerShoot, ResolveShots, Summary };
    public States State = States.Intro;

    [Serializable]
    public class PlayerObjects {
        public GameController.Player Player;
        public BlockContainer BlockContainer;
    }
    public List<PlayerObjects> PlayersList;
    public Dictionary<GameController.Player, PlayerObjects> PlayersDictionary = new Dictionary<GameController.Player, PlayerObjects>();

    void Start() {
        foreach (var p in PlayersList) {
            PlayersDictionary.Add(p.Player, p);
            p.BlockContainer.SetBlocks(GameController.instance.GetPlayer(p.Player).Blocks);
        }
    }

}
