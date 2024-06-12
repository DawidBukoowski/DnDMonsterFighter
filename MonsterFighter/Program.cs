using MonsterFighter;

Monster bandit = new("Bandit", 12, 11,
    11, 12, 12, 10, 10, 10, [
    new("Scimitar", 3, [new("1d6+1", DamageType.Slashing)]),
    new("Light Crossbow", 3, [new("1d8-1", DamageType.Piercing)])
]);
Monster blood_hawk = new("Blood Hawk", 12, 7,
    6, 14, 10, 3, 14, 5, [
    new("Beak", 4, [new("1d4+2", DamageType.Piercing)])
]);
Monster blood_tool_harpy = new("Blood-Toll Harpy", 11, 9,
    12, 13, 10, 6, 11, 13, [
    new("Multiattack", 0, [new("", DamageType.Piercing)], AttackType.Multi,[
        new("Bite", 3, [new("1d4+1", DamageType.Piercing)]),
        new("Claws", 3, [new("1d4+1", DamageType.Slashing)])
    ])
]);
Monster flumph = new("Flumph", 12, 7,
    6, 15, 10, 14, 14, 11, [
    new("Tendrils", 4, [
        new("1d4+2", DamageType.Piercing),
        new("1d4", DamageType.Acid),
        ])
    ], vulnerabilities: [DamageType.Psychic]);
Monster flying_snake = new("Flying Snake", 14, 50,
    4, 18, 11, 2, 12, 5, [
    new("Bite", 6, [
        new(1, DamageType.Piercing),
        new("3d4", DamageType.Poison),
        ])
    ], vulnerabilities: [DamageType.Psychic]);
Monster mastiff = new("Mastiff", 12, 120,
    13, 14, 12, 3, 12, 7, [
        new("Bite", 3, [
            new("1d6+1", DamageType.Piercing)
        ], condition: Condition.Prone, savingThrowDC: 11, savingThrowAttribute: MonsterFighter.Attribute.Strength)
    ]);
Attack psi_attack = new(
    "Psi-Imbued Blade",
    6,
    [
        new("2d6+3", DamageType.Psychic)
    ],
    condition: Condition.Frightened,
    savingThrowDC: 15,
    savingThrowAttribute: MonsterFighter.Attribute.Wisdom);

Monster mercane = new("Mercane", 13, 75,
    16, 10, 15, 18, 16, 15, [
        new("Multiattack", 0, [new("", DamageType.Piercing)], AttackType.Multi, [
            psi_attack, psi_attack, psi_attack
        ])
    ]);
FightSimulator.SimulateFight(mastiff, mercane);

