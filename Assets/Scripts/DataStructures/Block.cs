using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.DataStructures
{
    [Serializable]
    public class Block
    {
        public enum BlockType {
            Base,
            Small,
            Medium,
            Large,
            Huge,
            Flag,
            Crystal,
            SmallBlockPiece,
            LargeBlockPiece
        }

        public BlockType Type;

        public float Mass (GameController.Player player)
        {
            float mass = GameController.instance.Config.BlocksDictionary[Type].Mass;
            foreach (var upgradeOptions in GameController.instance.GetPlayer(player).State.FindUpgradesWithOption("AffectsBlockMass"))
            {
                if (upgradeOptions["AffectsBlockMass"].Split(' ').Contains(Type.ToString()))
                {
                    mass *= float.Parse(upgradeOptions["MassModifier"]);
                }
            }
            return mass;
        }

        public float Health (GameController.Player player)
        {
            float health = GameController.instance.Config.BlocksDictionary[Type].Health;
            foreach (var upgradeOptions in GameController.instance.GetPlayer(player).State.FindUpgradesWithOption("AffectsBlockHealth"))
            {
                if (upgradeOptions["AffectsBlockHealth"].Split(' ').Contains(Type.ToString()))
                {
                    health *= float.Parse(upgradeOptions["HealthModifier"]);
                }
            }
            return health;
        }

        #region tranform

        public Vector3 Position;
        public int Orientation;

        public List<Vector3> Orientations
        {
            get {
                return GameController.instance.Config.BlocksDictionary[Type].Orientations;
            }
        }

        public bool IsSupport {
            get {
                return GameController.instance.Config.BlocksDictionary[Type].IsSupport;
            }
        }

        public Vector3 TransformPosition {
            get
            {
                return Position + (Size - Vector3.one) / 2;
            }
        }
        public Quaternion TransformRotation {
            get {
                return Quaternion.Euler(Orientations[Orientation]);
            }
        }

        public Vector3 InitialSize {
            get {
                return GameController.instance.Config.BlocksDictionary[Type].Size;
            }
        }

        public Vector3 Size
        {
            get
            {
                var tmpSize = InitialSize;

                if (Orientations[Orientation].x == 90)
                {
                    float tmp = tmpSize.y;
                    tmpSize.y = tmpSize.z;
                    tmpSize.z = tmp;
                }
                if (Orientations[Orientation].y == 90)
                {
                    float tmp = tmpSize.x;
                    tmpSize.x = tmpSize.z;
                    tmpSize.z = tmp;
                }
                if (Orientations[Orientation].z == 90)
                {
                    float tmp = tmpSize.x;
                    tmpSize.x = tmpSize.y;
                    tmpSize.y = tmp;
                }
                return tmpSize;
            }
        }
        public int MinX
        {
            get
            {
                return (int)Math.Round(Position.x);
            }
        }
        public float MaxX
        {
            get
            {
                return (int)Math.Round(Position.x + Size.x);
            }
        }

        public float MinY
        {
            get
            {
                return (int)Math.Round(Position.y);
            }
        }
        public float MaxY
        {
            get
            {
                return (int)Math.Round(Position.y + Size.y);
            }
        }

        public float MinZ
        {
            get
            {
                return (int)Math.Round(Position.z);
            }
        }
        public float MaxZ
        {
            get
            {
                return (int)Math.Round(Position.z + Size.z);
            }
        }

        #endregion
    }
}
