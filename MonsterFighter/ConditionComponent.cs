namespace MonsterFighter
{

    public class ConditionComponent(Condition condition, int savingThrowDC, Attribute savingThrowAttribute, int duration = 0)
    {
        public Condition Condition { get; set; } = condition;
        public int SavingThrowDC { get; set; } = savingThrowDC;
        public Attribute SavingThrowAttribute { get; set; } = savingThrowAttribute;
        public int Duration { get; set; } = duration;
    }

}