using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            Huge
        }

        public BlockType Type;
        public int X;
        public int Y;
        public int Z;
    }
}
