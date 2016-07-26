using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.DataStructures
{
    [Serializable]
    public class PlayerState
    {
        public enum Upgrade {
            block_health_one,
            block_health_two,
            block_health_three,
            block_weight_one,
            block_weight_two,
            block_weight_three,
            cat_size_one,
            cat_size_two,
            cat_size_three,
            cat_weight_one,
            cat_weight_two,
            cat_weight_three,
            cross_speed_one,
            cross_speed_two,
            cross_speed_three,
            flag_one,
            flag_two,
            flag_three,
            block_small,
            block_medium,
            block_large,
            block_huge,
            flag,
            crystal
        };

        public int AvailableSmallBlocks;
        public int AvailableMediumBlocks;
        public int AvailableLargeBlocks;
        public int AvailableHugeBlocks;

        public int AvailableFlags;
        public int AvailableCrystals;

        public List<Upgrade> Upgrades;

        public float BlockHealth;
    }
}
