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
        public float MinorHorizontalAngle;
        public float MajorHorizontalAngle;
        public float VerticalAngle;
        public float Power;
    }
}
