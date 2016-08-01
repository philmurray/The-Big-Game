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
            Crystal
        }

        public BlockType Type;
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

        public Vector3 Size
        {
            get
            {
                Vector3 InitialSize = GameController.instance.Config.BlocksDictionary[Type].Size;

                if (Orientations[Orientation].x == 90)
                {
                    float tmp = InitialSize.y;
                    InitialSize.y = InitialSize.z;
                    InitialSize.z = tmp;
                }
                if (Orientations[Orientation].y == 90)
                {
                    float tmp = InitialSize.x;
                    InitialSize.x = InitialSize.z;
                    InitialSize.z = tmp;
                }
                if (Orientations[Orientation].z == 90)
                {
                    float tmp = InitialSize.x;
                    InitialSize.x = InitialSize.y;
                    InitialSize.y = tmp;
                }
                return InitialSize;
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
    }
}
