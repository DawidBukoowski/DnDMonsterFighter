using MonsterFighter;

Monster bandit = new("Bandit", 12, 11, 12, [
    new("Scimitar", 3, [new("1d6+1", DamageType.Slashing)]),
    new("Light Crossbow", 3, [new("1d8-1", DamageType.Piercing)])
]);
Monster bloodHawk = new("Blood Hawk", 12, 7, 14, [
    new("Beak", 4, [new("1d4+2", DamageType.Piercing)])
]);
Monster btharpy = new("Blood-Toll Harpy", 11, 9, 13, [
    new("Multiattack", 0, [new("", DamageType.Piercing)], AttackType.Multi,[
        new("Bite", 3, [new("1d4+1", DamageType.Piercing)]),
        new("Claws", 3, [new("1d4+1", DamageType.Slashing)])
    ])
]);

Monster flumph = new("Flumph", 12, 7, 15, [
    new("Tendrils", 4, [
        new("1d4+2", DamageType.Piercing),
        new("1d4", DamageType.Acid),
        ])
], vulnerabilities: [DamageType.Psychic]);

FightSimulator.SimulateFight(flumph, btharpy);

