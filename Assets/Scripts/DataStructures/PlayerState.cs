using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.DataStructures
{
    [Serializable]
    public class PlayerState
    {
        public int Gold;
        public int Crystals;

        [Serializable]
        public class AvailableBlock
        {
            public Block.BlockType BlockType;
            public int NumberAvailable;
        }

        public List<AvailableBlock> AvailableBlocksList;

        private Dictionary<Block.BlockType, int> _availableBlocks;
        public Dictionary<Block.BlockType, int> AvailableBlocks
        {
            get
            {
                if (_availableBlocks == null)
                {
                    _availableBlocks = new Dictionary<Block.BlockType, int>();
                    AvailableBlocksList.ForEach(p => _availableBlocks.Add(p.BlockType, p.NumberAvailable));
                }
                return _availableBlocks;
            }
        }

        public List<Upgrade> Upgrades;

        public enum Upgrade {
            block_health_one,
            block_health_two,
            block_health_three,
            block_weight_one,
            block_weight_two,
            block_weight_three,
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

        public List<Dictionary<string, string>> FindUpgradesWithOption(string option)
        {
            return Upgrades
                .FindAll(u => GameController.instance.Config.UpgradesDictionary.ContainsKey(u) && GameController.instance.Config.UpgradesDictionary[u].ContainsKey(option))
                .ConvertAll<Dictionary<string, string>>(u => GameController.instance.Config.UpgradesDictionary[u]);
        }

        public void ApplyUpgrade(Upgrade upgrade)
        {
            switch (upgrade)
            {
                case Upgrade.block_health_one:
                case Upgrade.block_health_two:
                case Upgrade.block_health_three:
                    ApplyTieredUpdate(upgrade, new List<Upgrade> { Upgrade.block_health_one, Upgrade.block_health_two, Upgrade.block_health_three });
                    break;
                case Upgrade.block_weight_one:
                case Upgrade.block_weight_two:
                case Upgrade.block_weight_three:
                    ApplyTieredUpdate(upgrade, new List<Upgrade> { Upgrade.block_weight_one, Upgrade.block_weight_two, Upgrade.block_weight_three });
                    break;
                case Upgrade.cat_weight_one:
                case Upgrade.cat_weight_two:
                case Upgrade.cat_weight_three:
                    ApplyTieredUpdate(upgrade, new List<Upgrade> { Upgrade.cat_weight_one, Upgrade.cat_weight_two, Upgrade.cat_weight_three });
                    break;
                case Upgrade.cross_speed_one:
                case Upgrade.cross_speed_two:
                case Upgrade.cross_speed_three:
                    ApplyTieredUpdate(upgrade, new List<Upgrade> { Upgrade.cross_speed_one, Upgrade.cross_speed_two, Upgrade.cross_speed_three });
                    break;
                case Upgrade.flag_one:
                case Upgrade.flag_two:
                case Upgrade.flag_three:
                    ApplyTieredUpdate(upgrade, new List<Upgrade> { Upgrade.flag_one, Upgrade.flag_two, Upgrade.flag_three });
                    break;
                case Upgrade.block_small:
                    AddAvailableBlocks(Block.BlockType.Small, 1);
                    break;
                case Upgrade.block_medium:
                    AddAvailableBlocks(Block.BlockType.Medium, 1);
                    break;
                case Upgrade.block_large:
                    AddAvailableBlocks(Block.BlockType.Large, 1);
                    break;
                case Upgrade.block_huge:
                    AddAvailableBlocks(Block.BlockType.Huge, 1);
                    break;
                case Upgrade.flag:
                    AddAvailableBlocks(Block.BlockType.Flag, 1);
                    break;
                case Upgrade.crystal:
                    AddAvailableBlocks(Block.BlockType.Crystal, 1);
                    break;
            }
        }

        public void AddAvailableBlocks(Block.BlockType type, int num)
        {
            int avail;
            AvailableBlocks.TryGetValue(type, out avail);
            AvailableBlocks[type] = avail + num;
        }
        public void RemoveAvailableBlocks(Block.BlockType type, int num)
        {
            int avail;
            AvailableBlocks.TryGetValue(type, out avail);
            AvailableBlocks[type] = avail - num;
        }
        public int GetAvailableBlocks(Block.BlockType type)
        {
            int avail;
            AvailableBlocks.TryGetValue(type, out avail);
            return avail;
        }

        private void ApplyTieredUpdate(Upgrade upgrade, List<Upgrade> list)
        {
            var intersect = Upgrades.Intersect(list);
            if (intersect.Count() == 0)
            {
                Upgrades.Add(upgrade);
            }
            else {
                var existingUpgrade = intersect.Single();
                Upgrades.Remove(existingUpgrade);
                int index = Math.Min(list.IndexOf(upgrade) + list.IndexOf(existingUpgrade) + 1, list.Count - 1);
                Upgrades.Add(list[index]);
            }
        }

        public void RemoveUpgrade(Upgrade upgrade)
        {
            switch (upgrade)
            {
                case Upgrade.block_small:
                    RemoveAvailableBlocks(Block.BlockType.Small, 1);
                    break;
                case Upgrade.block_medium:
                    RemoveAvailableBlocks(Block.BlockType.Medium, 1);
                    break;
                case Upgrade.block_large:
                    RemoveAvailableBlocks(Block.BlockType.Large, 1);
                    break;
                case Upgrade.block_huge:
                    RemoveAvailableBlocks(Block.BlockType.Huge, 1);
                    break;
                case Upgrade.flag:
                    RemoveAvailableBlocks(Block.BlockType.Flag, 1);
                    break;
                case Upgrade.crystal:
                    RemoveAvailableBlocks(Block.BlockType.Crystal, 1);
                    break;
                default:
                    Upgrades.Remove(upgrade);
                    break;
            }
        }
    }
}
