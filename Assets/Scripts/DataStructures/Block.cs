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

        public Vector3 TransformPosition {
            get {
                return Position + (Size - Vector3.one) / 2;
            }
        }

        public Vector3 Size
        {
            get
            {
                return GameController.instance.Config.BlocksDictionary[Type].Size;
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
