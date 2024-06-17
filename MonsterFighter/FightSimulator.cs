using System.Security.Cryptography;

namespace MonsterFighter
{
    public static class FightSimulator
    {
        static readonly Random rand = new();

        public static void SimulateFight(Monster monster1, Monster monster2)
        {
            DetermineInitative(monster1, monster2, out Monster first, out Monster second);

            int roundCounter = 1;
            while (monster1.CurrentHitPoints > 0 && monster2.CurrentHitPoints > 0)
            {
                Console.WriteLine($"--------------- Round {roundCounter++} ---------------");
                Console.WriteLine($"{monster1.Name} ({monster1.GetCurrentHP()}) vs. {monster2.Name} ({monster2.GetCurrentHP()})");
                Console.WriteLine($"---------------------------------------");

                PerformTurn(first, second);
                if (second.CurrentHitPoints <= 0) break;
                PerformTurn(second, first);
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

        private static void PerformTurn(Monster attacker, Monster defender)
        {
            if (attacker.CurrentHitPoints <= 0) return;
            if (attacker.HasCondition(Condition.Charmed) || attacker.HasCondition(Condition.Incapacitated))
            {
                Console.WriteLine($"{attacker.Name} cannot attack!");
                return;
            }
            if (attacker.HasCondition(Condition.Prone) && rand.Next(2) == 0)
            {
                attacker.RemoveCondition(Condition.Prone);
                Console.WriteLine($"{attacker.Name} gets up from being prone and forgoes the attack.");
                return;
            }
            Attack chosenAttack = attacker.Attacks[rand.Next(attacker.Attacks.Count)];
            PerformAttack(attacker, defender, chosenAttack);
            attacker.ResolveRecurringEffects(rand);
        }

        private static void PerformAttack(Monster attacker, Monster defender, Attack attack)
        {
            if (attack.Type == AttackType.Multi)
            {
                foreach (var subAttack in attack.SubAttacks)
                {
                    ExecuteSingleAttack(attacker, defender, subAttack);
                    if (defender.CurrentHitPoints <= 0) break;
                }
            }
            else
            {
                ExecuteSingleAttack(attacker, defender, attack);
            }
        }
        private static void DetermineInitative(Monster monster1, Monster monster2, out Monster first, out Monster second)
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
        private static void ExecuteSingleAttack(Monster attacker, Monster defender, Attack attack)
        {
            bool advantage = defender.HasCondition(Condition.Blinded) || defender.HasCondition(Condition.Prone) || defender.HasCondition(Condition.Restrained) ||
                             defender.HasCondition(Condition.Shocked) || defender.HasCondition(Condition.Stunned) || attacker.HasCondition(Condition.Invisible);
            bool disadvantage = attacker.HasCondition(Condition.Blinded) || attacker.HasCondition(Condition.Frightened) || attacker.HasCondition(Condition.Poisoned) ||
                                attacker.HasCondition(Condition.Prone) || attacker.HasCondition(Condition.Restrained) || defender.HasCondition(Condition.Invisible);

            int roll = RollWithAdvantageOrDisadvantage(advantage, disadvantage);
            int toHit = roll + attack.BonusToHit;
            Console.WriteLine($"{attacker.Name} attacks with {attack.Name}: {toHit} ({roll}+{attack.BonusToHit}) to hit!");

            if (roll == 1)
            {
                Console.WriteLine("Critical miss!");
                return;
            }
            bool isCritical = roll == 20 || defender.HasCondition(Condition.Paralyzed) || defender.HasCondition(Condition.Unconscious);
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
                if (damageComponent.SavingThrowDC.HasValue && damageComponent.SavingThrowAttribute.HasValue)
                {
                    RollAgainstConditionalDamage(damageComponent, defender);
                }
                else
                {
                    int damage = damageComponent.RollDamage(rand);
                    if (isCritical) damage *= 2;
                    damage = ApplyDamageModifiers(defender, damageComponent.DamageType, damage);

                    defender.CurrentHitPoints -= damage;
                    Console.WriteLine($"Hit! {defender.Name} takes {damage} {damageComponent.DamageType} damage!");
                }
            }
            ConditionComponent condition = attack.ConditionComponent;
            if (condition != null && !defender.HasCondition(condition.Condition))
            {
                if (condition.SavingThrowDC > 0) RollAgainstCondition(condition, defender);
                else defender.ApplyCondition(condition.Condition);
            }

            if (attack.RecurringEffect != null && !defender.RecurringEffects.Contains(attack.RecurringEffect))
            {
                defender.ApplyRecurringEffect(attack.RecurringEffect);
            }
        }
        private static void RollAgainstCondition(ConditionComponent conditionComponent, Monster defender)
        {
            int roll = defender.RollSave(conditionComponent.SavingThrowAttribute, conditionComponent.SavingThrowDC, rand);
            if (roll >= conditionComponent.SavingThrowDC)
            {
                Console.WriteLine($"{defender.Name} saves against being {conditionComponent.Condition}.");
            }
            else
            {
                Console.WriteLine($"{defender.Name} fails against being {conditionComponent.Condition}.");
                defender.ApplyCondition(conditionComponent.Condition);
            }
        }

        private static void RollAgainstConditionalDamage(DamageComponent damageComponent, Monster defender)
        {
            int roll = rand.Next(1, 21);
            int modifier = defender.GetModifier(damageComponent.SavingThrowAttribute.Value);
            int total = roll + modifier;
            string sign = modifier >= 0 ? "+" : "";
            Console.WriteLine($"{defender.Name} rolled {total} ({roll}{sign}{modifier}) on a DC {damageComponent.SavingThrowDC} {damageComponent.SavingThrowAttribute} saving throw.");
            if (total < damageComponent.SavingThrowDC.Value)
            {
                int damage = damageComponent.RollDamage(rand);
                damage = ApplyDamageModifiers(defender, damageComponent.DamageType, damage);
                defender.CurrentHitPoints -= damage;
                Console.WriteLine($"{defender.Name} fails the saving throw and takes {damage} {damageComponent.DamageType} damage!");
            }
            else
            {
                Console.WriteLine($"{defender.Name} succeeds the saving throw and takes no additional damage.");
            }
        }

        private static int ApplyDamageModifiers(Monster defender, DamageType damageType, int damage)
        {
            if (defender.Immunities.Contains(damageType)) return 0;
            if (defender.Resistances.Contains(damageType)) return damage / 2;
            if (defender.Vulnerabilities.Contains(damageType)) return damage * 2;
            return damage;
        }

        private static int RollWithAdvantageOrDisadvantage(bool advantage, bool disadvantage)
        {
            int roll1 = rand.Next(1, 21);
            int roll2 = rand.Next(1, 21);
            if (advantage && !disadvantage) return Math.Max(roll1, roll2);
            else if (!advantage && disadvantage) return Math.Min(roll1, roll2);
            else return roll1;
        }
    }
}