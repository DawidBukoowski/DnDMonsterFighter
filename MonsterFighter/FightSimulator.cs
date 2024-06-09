namespace MonsterFighter
{
    public static class FightSimulator
    {
        public static void SimulateFight(Monster monster1, Monster monster2)
        {
            Random rand = new();
            //inicjatywa
            DetermineInitative(monster1, monster2, rand, out Monster first, out Monster second);
            Console.WriteLine($"{first.Name} goes first!");

            //walka
            int counter = 1;
            while (monster1.CurrentHitPoints > 0 && monster2.CurrentHitPoints > 0)
            {
                Console.WriteLine($"--------------- Round {counter++} ---------------");
                Console.WriteLine($"{monster1.Name} ({monster1.GetCurrentHP()}) vs. {monster2.Name} ({monster2.GetCurrentHP()})");
                Console.WriteLine($"---------------------------------------");

                PerformTurn(first, second, rand);
                if (second.CurrentHitPoints <= 0) break;
                PerformTurn(second, first, rand);
            }
            //wynik
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
        }

        private static void ExecuteSingleAttack(Monster attacker, Monster defender, Attack attack, Random rand, bool advantage = false, bool disadvantage = false)
        {

            int roll;
            if(advantage){
                roll = RollWithAdvantageOrDisadvantage(rand, true);
                Console.WriteLine($"{attacker.Name} attacks with advantage!");
            } else if(disadvantage) {
                roll = RollWithAdvantageOrDisadvantage(rand, false);
                Console.WriteLine($"{attacker.Name} attacks with disadvantage!");
            } else {roll = rand.Next(1, 21);}

            int toHit = roll + attack.BonusToHit;
            Console.WriteLine($"{attacker.Name} attacks with {attack.Name}: {toHit} ({roll}+{attack.BonusToHit}) to hit!");

            if (roll == 1)
            {
                Console.WriteLine("Critical miss!");
                return;
            }
            int damage = attack.RollDamage(rand);

            if (roll == 20)
            {
                damage = ApplyDamageModifiers(defender, attack.DamageType, damage) * 2;
                Console.WriteLine($"Critical hit! {defender.Name} takes {damage} {attack.DamageType} damage!");
            }
            else if (toHit < defender.ArmorClass)
            {
                Console.WriteLine("Miss!");
                return;
            }
            else
            {
                damage = ApplyDamageModifiers(defender, attack.DamageType, damage);
                Console.WriteLine($"Hit! {defender.Name} takes {damage} {attack.DamageType} damage!");
            }
            defender.CurrentHitPoints -= damage;
        }

        private static int ApplyDamageModifiers(Monster defender, DamageType damageType, int damage)
        {
            if (defender.Immunities.Contains(damageType)) return 0;
            if (defender.Resistances.Contains(damageType)) return damage / 2;
            if (defender.Vulnerabilities.Contains(damageType)) return damage * 2;
            return damage;
        }

        private static int RollWithAdvantageOrDisadvantage(Random rand, bool advantage)
        {
            int roll1 = rand.Next(1, 21);
            int roll2 = rand.Next(1, 21);

            return advantage ? Math.Max(roll1, roll2) : Math.Min(roll1, roll2);
        }
    }
}