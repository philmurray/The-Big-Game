using UnityEngine;
using System.Collections;

public class FlagBehavior : MonoBehaviour {
    public float PowerAffect;
    public bool Destroyed;
    
    private RealBlockBehavior _blockBehavior;

    void Start()
    {
        _blockBehavior = GetComponent<RealBlockBehavior>();
        Destroyed = false;
    }

    void FixedUpdate()
    {
        if (!Destroyed)
        {
            if (Vector3.Dot(transform.up, Vector3.up) < 0.75f)
            {
                OnDestroyed();
            }
        }

    }

    public float GetPowerAffect(GameController.Player player)
    {
        var affect = PowerAffect;
        foreach (var upgradeOptions in GameController.instance.GetPlayer(player).State.FindUpgradesWithOption("AffectsFlagPower"))
        {
            affect *= float.Parse(upgradeOptions["AffectsFlagPower"]);
        }
        return affect;
    }

    public void OnDestroyed()
    {
        if (_blockBehavior != null)
        {
            Destroyed = true;
            _blockBehavior.OnDestroyed();
        }
    }
}
