using System.Security.Cryptography;

namespace MonsterFighter
{
    public static class FightSimulator
    {
        public static void SimulateFight(Monster monster1, Monster monster2)
        {
            Random rand = new();
            DetermineInitative(monster1, monster2, rand, out Monster first, out Monster second);

            int roundCounter = 1;
            while (monster1.CurrentHitPoints > 0 && monster2.CurrentHitPoints > 0)
            {
                Console.WriteLine($"--------------- Round {roundCounter++} ---------------");
                Console.WriteLine($"{monster1.Name} ({monster1.GetCurrentHP()}) vs. {monster2.Name} ({monster2.GetCurrentHP()})");
                Console.WriteLine($"---------------------------------------");

                PerformTurn(first, second, rand);
                if (second.CurrentHitPoints <= 0) break;
                PerformTurn(second, first, rand);
            }
            DeclareWinner(monster1, monster2);
        }

        private static void DeclareWinner(Monster monster1, Monster monster2)
        {
            if (monster1.CurrentHitPoints <= 0)
            {
                Console.WriteLine($"{monster1.Name} is defeated. {monster2.Name} wins!");
            }
            else
            {
                Console.WriteLine($"{monster2.Name} is defeated. {monster1.Name} wins!");
            }
        }

        private static void PerformTurn(Monster attacker, Monster defender, Random rand)
        {
            attacker.ResolveStatusEffects(rand);
            if (attacker.CurrentHitPoints <= 0) return;
            Attack chosenAttack = attacker.Attacks[rand.Next(attacker.Attacks.Count)];
            PerformAttack(attacker, defender, chosenAttack, rand);
        }

        private static void PerformAttack(Monster attacker, Monster defender, Attack attack, Random rand)
        {
            if (attack.Type == AttackType.Multi)
            {
                foreach (var subAttack in attack.SubAttacks)
                {
                    ExecuteSingleAttack(attacker, defender, subAttack, rand);
                    if (defender.CurrentHitPoints <= 0) break;
                }
            }
            else
            {
                ExecuteSingleAttack(attacker, defender, attack, rand);
            }
        }

        private static void DetermineInitative(Monster monster1, Monster monster2, Random rand, out Monster first, out Monster second)
        {
            int initiative1 = monster1.RollInitiative(rand);
            int initiative2 = monster2.RollInitiative(rand);

            if (initiative1 > initiative2)
            {
                first = monster1;
                second = monster2;
            }
            else
            {
                first = monster2;
                second = monster1;
            }

            Console.WriteLine($"{first.Name} goes first!");
        }

        private static void ExecuteSingleAttack(Monster attacker, Monster defender, Attack attack, Random rand)
        {
            bool advantage = defender.HasCondition(Condition.Prone);
            bool disadvantage = attacker.HasCondition(Condition.Prone);

            int roll = RollWithAdvantageOrDisadvantage(rand, advantage, disadvantage);
            int toHit = roll + attack.BonusToHit;
            Console.WriteLine($"{attacker.Name} attacks with {attack.Name}: {toHit} ({roll}+{attack.BonusToHit}) to hit!");

            if (roll == 1)
            {
                Console.WriteLine("Critical miss!");
                return;
            }
            bool isCritical = roll == 20;
            if (isCritical)
            {
                Console.WriteLine($"Critical hit!");
            }
            else if (toHit < defender.ArmorClass)
            {
                Console.WriteLine("Miss!");
                return;
            }

            foreach (var damageComponent in attack.DamageComponents)
            {
                int damage = damageComponent.RollDamage(rand);
                if (isCritical) damage *= 2;
                damage = ApplyDamageModifiers(defender, damageComponent.DamageType, damage);

                defender.CurrentHitPoints -= damage;
                Console.WriteLine($"Hit! {defender.Name} takes {damage} {damageComponent.DamageType} damage!");
            }

            if (attack.StatusEffect != null)
            {
                defender.ApplyStatusEffect(attack.StatusEffect.Clone());
                if (attack.StatusEffect.Condition.HasValue)
                {
                    ApplyConditionEffect(attacker, defender, attack.StatusEffect, rand);
                }
            }


        }

        private static void ApplyConditionEffect(Monster attacker, Monster defender, StatusEffect effect, Random rand)
        {
            int roll = rand.Next(1, 21);
            int modifier = defender.GetModifier(effect.SavingThrowAttribute);
            int total = roll + modifier;

            if (total >= effect.SavingThrowDC)
            {
                Console.WriteLine($"{defender.Name} succeeds the saving throw and avoids being {effect.Condition}!");
            }
            else
            {
                Console.WriteLine($"{defender.Name} fails the saving throw and is now {effect.Condition}!");
                defender.Conditions.Add((Condition)effect.Condition);
            }
        }

        private static int ApplyDamageModifiers(Monster defender, DamageType damageType, int damage)
        {
            if (defender.Immunities.Contains(damageType)) return 0;
            if (defender.Resistances.Contains(damageType)) return damage / 2;
            if (defender.Vulnerabilities.Contains(damageType)) return damage * 2;
            return damage;
        }

        private static int RollWithAdvantageOrDisadvantage(Random rand, bool advantage, bool disadvantage)
        {
            int roll1 = rand.Next(1, 21);
            int roll2 = rand.Next(1, 21);
            if (advantage && !disadvantage) return Math.Max(roll1, roll2);
            else if (!advantage && disadvantage) return Math.Min(roll1, roll2);
            else return roll1;
        }
    }
}