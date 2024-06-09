namespace MonsterFighter
{
    public class Monster(string name, int armorClass, int hitPoints, int dexterity, List<Attack> attacks, List<DamageType>? immunities = null, List<DamageType>? resistances = null, List<DamageType>? vulnerabilities = null)
    {
        public string Name { get; set; } = name;
        public int ArmorClass { get; set; } = armorClass;
        public int MaxHitPoints { get; set; } = hitPoints;
        public int CurrentHitPoints { get; set; } = hitPoints;
        public int Dexterity { get; set; } = dexterity;
        public List<Attack> Attacks { get; set; } = attacks;
        public List<DamageType> Immunities { get; set; } = immunities ?? [];
        public List<DamageType> Resistances { get; set; } = resistances ?? [];
        public List<DamageType> Vulnerabilities { get; set; } = vulnerabilities ?? [];
        public int DexterityModifier => (Dexterity - 10) / 2;

        public void Heal()
        {
            CurrentHitPoints = MaxHitPoints;
        }

        public int RollInitiative(Random rand)
        {
            int roll = rand.Next(1, 21);
            int total = roll + DexterityModifier;
            Console.WriteLine($"{Name} rolled {total} ({roll}+{DexterityModifier}) for initiative.");
            return total;
        }

        public string GetCurrentHP()
        {
            return $"{CurrentHitPoints}/{MaxHitPoints}";
        }
    }
}
