using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.DataStructures
{
    [Serializable]
    public class PlayerState
    {
        public int AvailableSmallBlocks;
        public int AvailableMediumBlocks;
        public int AvailableLargeBlocks;
        public int AvailableHugeBlocks;
        public int AvailableFlags;
        public int AvailableCrystals;

        public List<Upgrade> Upgrades;

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
                case Upgrade.cat_size_one:
                case Upgrade.cat_size_two:
                case Upgrade.cat_size_three:
                    ApplyTieredUpdate(upgrade, new List<Upgrade> { Upgrade.cat_size_one, Upgrade.cat_size_two, Upgrade.cat_size_three });
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
                    AvailableSmallBlocks++;
                    break;
                case Upgrade.block_medium:
                    AvailableMediumBlocks++;
                    break;
                case Upgrade.block_large:
                    AvailableLargeBlocks++;
                    break;
                case Upgrade.block_huge:
                    AvailableHugeBlocks++;
                    break;
                case Upgrade.flag:
                    AvailableFlags++;
                    break;
                case Upgrade.crystal:
                    AvailableCrystals++;
                    break;
            }
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
    }
}
