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

        #region tranform
        
        public int PositionX;
        public int PositionY;
        public int PositionZ;

        public int Orientation;
        

        #endregion
    }
}
