using UnityEngine;
using System.Collections;

public class FlagBehavior : MonoBehaviour {
    public float PowerAffect;

    public float GetPowerAffect(GameController.Player player)
    {
        var affect = PowerAffect;
        foreach (var upgradeOptions in GameController.instance.GetPlayer(player).State.FindUpgradesWithOption("AffectsFlagPower"))
        {
            affect *= float.Parse(upgradeOptions["AffectsFlagPower"]);
        }
        return affect;
    }
}
