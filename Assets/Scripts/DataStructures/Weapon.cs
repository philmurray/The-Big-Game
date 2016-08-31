using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.DataStructures
{
    [Serializable]
    public class Weapon
    {
        public enum WeaponType
        {
            Catapult,
            Crossbow
        }

        public WeaponType Type;
        public float HorizontalAngle;
        public float VerticalAngle;
        public float Power;

        public float ProjectileMass(GameController.Player player)
        {
            float mass = GameController.instance.Config.WeaponsDictionary[Type].ProjectileMass;
            foreach (var upgradeOptions in GameController.instance.GetPlayer(player).State.FindUpgradesWithOption("AffectsProjectileMass"))
            {
                if (upgradeOptions["AffectsProjectileMass"].Split(' ').Contains(Type.ToString()))
                {
                    mass *= float.Parse(upgradeOptions["MassModifier"]);
                }
            }
            return mass;
        }

        public float ProjectileDamage(GameController.Player player)
        {
            float damage = GameController.instance.Config.WeaponsDictionary[Type].ProjectileDamage;
            foreach (var upgradeOptions in GameController.instance.GetPlayer(player).State.FindUpgradesWithOption("AffectsProjectileDamage"))
            {
                if (upgradeOptions["AffectsProjectileDamage"].Split(' ').Contains(Type.ToString()))
                {
                    damage *= float.Parse(upgradeOptions["DamageModifier"]);
                }
            }
            return damage;
        }
    }
}
