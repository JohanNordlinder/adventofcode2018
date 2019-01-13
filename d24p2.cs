using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2018
{
    [TestClass]
    public class D24P2
    {

        [TestMethod]
        public void TestRun()
        {
            var groups = new List<Group>();
            groups.Add(new Group {
                Id = 1,
                Faction = Factions.IMMUNE_SYSTEM,
                Units = 17,
                HitPoints = 5390,
                Immunities = new List<Element>() { },
                Weaknesses = new List<Element>() { Element.RADIATION, Element.BLUDGEONING },
                AttackDamage = 4507,
                AttackType = Element.FIRE,
                Initiative = 2
            });
            groups.Add(new Group
            {
                Id = 2,
                Faction = Factions.IMMUNE_SYSTEM,
                Units = 989,
                HitPoints = 1274,
                Immunities = new List<Element>() { Element.FIRE },
                Weaknesses = new List<Element>() { Element.BLUDGEONING, Element.SLASHING },
                AttackDamage = 25,
                AttackType = Element.SLASHING,
                Initiative = 3
            });
            groups.Add(new Group
            {
                Id = 1,
                Faction = Factions.INFECTION,
                Units = 801,
                HitPoints = 4706,
                Immunities = new List<Element>() { },
                Weaknesses = new List<Element>() { Element.RADIATION },
                AttackDamage = 116,
                AttackType = Element.BLUDGEONING,
                Initiative = 1
            });
            groups.Add(new Group
            {
                Id = 2,
                Faction = Factions.INFECTION,
                Units = 4485,
                HitPoints = 2961,
                Immunities = new List<Element>() { Element.RADIATION },
                Weaknesses = new List<Element>() { Element.FIRE, Element.COLD },
                AttackDamage = 12,
                AttackType = Element.SLASHING,
                Initiative = 4
            });

            Assert.AreEqual(51, RunChallenge(groups));
        }

        [TestMethod]
        public void RealRun()
        {
            var groups = new List<Group>();

            //4400 units each with 10384 hit points (weak to slashing) with an attack that does 21 radiation damage at initiative 16
            groups.Add(new Group
            {
                Id = 1,
                Faction = Factions.IMMUNE_SYSTEM,
                Units = 4400,
                HitPoints = 10384,
                Immunities = new List<Element>() { },
                Weaknesses = new List<Element>() { Element.SLASHING },
                AttackDamage = 21,
                AttackType = Element.RADIATION,
                Initiative = 16
            });
            //974 units each with 9326 hit points (weak to radiation) with an attack that does 86 cold damage at initiative 19
            groups.Add(new Group
            {
                Id = 2,
                Faction = Factions.IMMUNE_SYSTEM,
                Units = 974,
                HitPoints = 9326,
                Immunities = new List<Element>() { },
                Weaknesses = new List<Element>() { Element.RADIATION },
                AttackDamage = 86,
                AttackType = Element.COLD,
                Initiative = 19
            });
            //543 units each with 2286 hit points with an attack that does 34 cold damage at initiative 13
            groups.Add(new Group
            {
                Id = 3,
                Faction = Factions.IMMUNE_SYSTEM,
                Units = 543,
                HitPoints = 2286,
                Immunities = new List<Element>() { },
                Weaknesses = new List<Element>() { },
                AttackDamage = 34,
                AttackType = Element.COLD,
                Initiative = 13
            });

            //47 units each with 4241 hit points(weak to slashing, cold; immune to radiation) with an attack that does 889 cold damage at initiative 10
            groups.Add(new Group
            {
                Id = 4,
                Faction = Factions.IMMUNE_SYSTEM,
                Units = 47,
                HitPoints = 4241,
                Immunities = new List<Element>() { Element.RADIATION },
                Weaknesses = new List<Element>() { Element.SLASHING, Element.COLD },
                AttackDamage = 889,
                AttackType = Element.COLD,
                Initiative = 10
            });
            //5986 units each with 4431 hit points with an attack that does 6 cold damage at initiative 8
            groups.Add(new Group
            {
                Id = 5,
                Faction = Factions.IMMUNE_SYSTEM,
                Units = 5986,
                HitPoints = 4431,
                Immunities = new List<Element>() { },
                Weaknesses = new List<Element>() {  },
                AttackDamage = 6,
                AttackType = Element.COLD,
                Initiative = 8
            });
            //688 units each with 1749 hit points(immune to slashing, radiation) with an attack that does 23 cold damage at initiative 7
            groups.Add(new Group
            {
                Id = 6,
                Faction = Factions.IMMUNE_SYSTEM,
                Units = 688,
                HitPoints = 1749,
                Immunities = new List<Element>() { Element.SLASHING, Element.RADIATION },
                Weaknesses = new List<Element>() { },
                AttackDamage = 23,
                AttackType = Element.COLD,
                Initiative = 7
            });
            //61 units each with 1477 hit points with an attack that does 235 fire damage at initiative 1
            groups.Add(new Group
            {
                Id = 7,
                Faction = Factions.IMMUNE_SYSTEM,
                Units = 61,
                HitPoints = 1477,
                Immunities = new List<Element>() { },
                Weaknesses = new List<Element>() { },
                AttackDamage = 235,
                AttackType = Element.FIRE,
                Initiative = 1
            });
            //505 units each with 9333 hit points(weak to slashing, cold) with an attack that does 174 radiation damage at initiative 9
            groups.Add(new Group
            {
                Id = 8,
                Faction = Factions.IMMUNE_SYSTEM,
                Units = 505,
                HitPoints = 9333,
                Immunities = new List<Element>() { },
                Weaknesses = new List<Element>() { Element.SLASHING, Element.COLD },
                AttackDamage = 174,
                AttackType = Element.RADIATION,
                Initiative = 9
            });
            //3745 units each with 8367 hit points(immune to fire, slashing, radiation; weak to cold) with an attack that does 21 bludgeoning damage at initiative 3
            groups.Add(new Group
            {
                Id = 9,
                Faction = Factions.IMMUNE_SYSTEM,
                Units = 3745,
                HitPoints = 8367,
                Immunities = new List<Element>() { Element.FIRE, Element.SLASHING, Element.RADIATION },
                Weaknesses = new List<Element>() { Element.COLD },
                AttackDamage = 21,
                AttackType = Element.BLUDGEONING,
                Initiative = 3
            });
            //111 units each with 3482 hit points with an attack that does 311 cold damage at initiative 15
            groups.Add(new Group
            {
                Id = 10,
                Faction = Factions.IMMUNE_SYSTEM,
                Units = 111,
                HitPoints = 3482,
                Immunities = new List<Element>() { },
                Weaknesses = new List<Element>() { },
                AttackDamage = 311,
                AttackType = Element.COLD,
                Initiative = 15
            });

            //2891 units each with 32406 hit points(weak to fire, bludgeoning) with an attack that does 22 slashing damage at initiative 2
            groups.Add(new Group
            {
                Id = 1,
                Faction = Factions.INFECTION,
                Units = 2891,
                HitPoints = 32406,
                Immunities = new List<Element>() { },
                Weaknesses = new List<Element>() { Element.FIRE, Element.BLUDGEONING },
                AttackDamage = 22,
                AttackType = Element.SLASHING,
                Initiative = 2
            });
            //1698 units each with 32906 hit points(weak to radiation) with an attack that does 27 fire damage at initiative 17
            groups.Add(new Group
            {
                Id = 2,
                Faction = Factions.INFECTION,
                Units = 1698,
                HitPoints = 32906,
                Immunities = new List<Element>() { },
                Weaknesses = new List<Element>() { Element.RADIATION },
                AttackDamage = 27,
                AttackType = Element.FIRE,
                Initiative = 17
            });
            //395 units each with 37715 hit points(immune to fire) with an attack that does 183 cold damage at initiative 6
            groups.Add(new Group
            {
                Id = 3,
                Faction = Factions.INFECTION,
                Units = 395,
                HitPoints = 37715,
                Immunities = new List<Element>() { Element.FIRE },
                Weaknesses = new List<Element>() { },
                AttackDamage = 183,
                AttackType = Element.COLD,
                Initiative = 6
            });
            //3560 units each with 45025 hit points(weak to radiation; immune to fire) with an attack that does 20 cold damage at initiative 14
            groups.Add(new Group
            {
                Id = 4,
                Faction = Factions.INFECTION,
                Units = 3560,
                HitPoints = 45025,
                Immunities = new List<Element>() { Element.FIRE },
                Weaknesses = new List<Element>() { Element.RADIATION },
                AttackDamage = 20,
                AttackType = Element.COLD,
                Initiative = 14
            });
            //2335 units each with 15938 hit points(weak to cold) with an attack that does 13 slashing damage at initiative 11
            groups.Add(new Group
            {
                Id = 5,
                Faction = Factions.INFECTION,
                Units = 2335,
                HitPoints = 15938,
                Immunities = new List<Element>() { },
                Weaknesses = new List<Element>() { Element.COLD },
                AttackDamage = 13,
                AttackType = Element.SLASHING,
                Initiative = 11
            });
            //992 units each with 19604 hit points(immune to slashing, bludgeoning, radiation) with an attack that does 38 radiation damage at initiative 5
            groups.Add(new Group
            {
                Id = 6,
                Faction = Factions.INFECTION,
                Units = 992,
                HitPoints = 19604,
                Immunities = new List<Element>() { Element.SLASHING, Element.BLUDGEONING, Element.RADIATION },
                Weaknesses = new List<Element>() { },
                AttackDamage = 38,
                AttackType = Element.RADIATION,
                Initiative = 5
            });
            //5159 units each with 44419 hit points(immune to slashing; weak to fire) with an attack that does 13 bludgeoning damage at initiative 4
            groups.Add(new Group
            {
                Id = 7,
                Faction = Factions.INFECTION,
                Units = 5159,
                HitPoints = 44419,
                Immunities = new List<Element>() { Element.SLASHING },
                Weaknesses = new List<Element>() { Element.FIRE },
                AttackDamage = 13,
                AttackType = Element.BLUDGEONING,
                Initiative = 4
            });
            //2950 units each with 6764 hit points(weak to slashing) with an attack that does 4 radiation damage at initiative 18
            groups.Add(new Group
            {
                Id = 8,
                Faction = Factions.INFECTION,
                Units = 2950,
                HitPoints = 6764,
                Immunities = new List<Element>() { },
                Weaknesses = new List<Element>() { Element.SLASHING },
                AttackDamage = 4,
                AttackType = Element.RADIATION,
                Initiative = 18
            });
            //6131 units each with 25384 hit points(weak to slashing; immune to bludgeoning, cold) with an attack that does 7 cold damage at initiative 12
            groups.Add(new Group
            {
                Id = 9,
                Faction = Factions.INFECTION,
                Units = 6131,
                HitPoints = 25384,
                Immunities = new List<Element>() { Element.BLUDGEONING, Element.COLD },
                Weaknesses = new List<Element>() { Element.SLASHING },
                AttackDamage = 7,
                AttackType = Element.COLD,
                Initiative = 12
            });
            //94 units each with 29265 hit points(weak to cold, bludgeoning) with an attack that does 588 bludgeoning damage at initiative 20
            groups.Add(new Group
            {
                Id = 10,
                Faction = Factions.INFECTION,
                Units = 94,
                HitPoints = 29265,
                Immunities = new List<Element>() { },
                Weaknesses = new List<Element>() { Element.COLD, Element.BLUDGEONING },
                AttackDamage = 588,
                AttackType = Element.BLUDGEONING,
                Initiative = 20
            });
            Console.WriteLine("Result: " + RunChallenge(groups));
        }

        public enum Factions { IMMUNE_SYSTEM, INFECTION }

        public enum Element { FIRE, COLD, SLASHING, RADIATION, BLUDGEONING }

        public class Group : ICloneable
        {
            public int Id { set; get; }
            public Factions Faction { set; get; }
            public int Units { set; get; }
            public int HitPoints { set; get; }
            public int AttackDamage { set; get; }
            public int OriginalAttackDamage { set; get; }
            public Element AttackType { set; get; }
            public int Initiative { set; get; }
            public int EffectivePower { set; get; }
            public IEnumerable<Element> Weaknesses { set; get; }
            public IEnumerable<Element> Immunities { set; get; }
            public Group SelectedAsTargetBy { set; get; }
            public Group SelectedTarget { set; get; }
            public bool IsDead { set; get; } = false;

            public object Clone()
            {
                return this.MemberwiseClone();
            }

        }
        public int RunChallenge(List<Group> inputGroups)
        {
            inputGroups.ForEach(z =>
            {
                z.OriginalAttackDamage = z.AttackDamage;
            });

            var boost = 0;
            List<Group> groups = null;
            int unitsKilledThisRound;
            int roundWithNoKillings;

            do
            {
                boost++;

                foreach (var group in inputGroups.Where(z => z.Faction == Factions.IMMUNE_SYSTEM))
                {
                    group.AttackDamage = group.OriginalAttackDamage + boost;
                }

                groups = inputGroups.Select(z => (Group) z.Clone()).ToList();
                roundWithNoKillings = 0;

                do
                {
                    unitsKilledThisRound = 0;

                    groups.ForEach(z =>
                    {
                        z.SelectedAsTargetBy = null;
                        z.SelectedTarget = null;
                        z.EffectivePower = z.Units * z.AttackDamage;
                    });

                    // Target selection
                    foreach (var group in groups.Where(z => !z.IsDead).OrderByDescending(z => z.EffectivePower).ThenByDescending(z => z.Initiative))
                    {
                        var selectedTarget = groups
                            .Where(z => !z.IsDead)
                            .Where(z => z.SelectedAsTargetBy == null)
                            .Where(z => z.Faction != group.Faction)
                            .OrderByDescending(z => CalculateDamage(group, z))
                            .ThenByDescending(z => z.EffectivePower)
                            .ThenByDescending(z => z.Initiative)
                            .FirstOrDefault();

                        if (selectedTarget != null)
                        {
                            selectedTarget.SelectedAsTargetBy = group;
                            group.SelectedTarget = selectedTarget;
                        }
                    }

                    //Attacking
                    foreach (var group in groups.Where(z => !z.IsDead).OrderByDescending(z => z.Initiative))
                    {
                        var target = group.SelectedTarget;

                        if (group.IsDead || target == null)
                        { continue; }

                        var killed = CalculateDamage(group, target) / (target.HitPoints);
                        target.Units = killed > target.Units ? 0 : target.Units - killed;
                        target.EffectivePower = target.Units * target.AttackDamage;
                        if (target.Units < 1)
                        {
                            target.IsDead = true;
                        }
                        unitsKilledThisRound += killed;
                    }

                    if (unitsKilledThisRound == 0)
                    {
                        roundWithNoKillings++;
                    }
                    if (roundWithNoKillings > 5)
                    {
                        break;
                    }
                } while (groups.Where(z => !z.IsDead).Select(z => z.Faction).Distinct().Count() > 1);
            } while (!groups.Where(z => !z.IsDead).All(z => z.Faction == Factions.IMMUNE_SYSTEM));

            return groups.Where(z => !z.IsDead && z.Faction == Factions.IMMUNE_SYSTEM).Sum(z => z.Units);
        }

        public static int CalculateDamage(Group attacker, Group attacked)
        {
            var attack = attacker.Units * attacker.AttackDamage;

            if (attacked.Weaknesses.Contains(attacker.AttackType))
            {
                return attack * 2;
            } else if (attacked.Immunities.Contains(attacker.AttackType))
            {
                return 0;
            } else
            {
                return attack;
            }
        }
        
    }
}
