using System.Diagnostics.CodeAnalysis;

namespace TextRPGOne
{
    partial class Program
    {
        class NPC
        {
            private string _name;
            private string _description;
            private int _health;
            private int _maxHealth;
            private int _mana;
            private int _maxMana;
            private int _strength;
            private int _dexterity;
            private int _intelligence;
            private int _constitution;
            private int _initiative;
            private Move[] _moveSet;
            private LootTableItem[] _lootTable;
            private int _goldDrop;
            private int _level;
            private int _expDrop;
            private PrimaryStatType _primaryStat;

            public string Name { get => _name; [MemberNotNull(nameof(_name))] set => _name = value; }
            public string Description { get => _description; [MemberNotNull(nameof(_description))] set => _description = value; }
            public int Health { get => _health; set => _health = value; }
            public int MaxHealth { get => _maxHealth; set => _maxHealth = value; }
            public int Mana { get => _mana; set => _mana = value; }
            public int MaxMana { get => _maxMana; set => _maxMana = value; }
            public int Strength { get => _strength; set => _strength = value; }
            public int Dexterity { get => _dexterity; set => _dexterity = value; }
            public int Intelligence { get => _intelligence; set => _intelligence = value; }
            public int Constitution { get => _constitution; set => _constitution = value; }
            public int Initiative { get => _initiative; set => _initiative = value; }
            public Move[] MoveSet { get => _moveSet; [MemberNotNull(nameof(_moveSet))] set => _moveSet = value; }
            public LootTableItem[] LootTable { get => _lootTable; [MemberNotNull(nameof(_lootTable))] set => _lootTable = value; }
            public int GoldDrop { get => _goldDrop; set => _goldDrop = value; }
            public int Level { get => _level; set => _level = value; }
            public int ExpDrop { get => _expDrop; set => _expDrop = value; }
            public int PrimaryStat
            {
                get =>
                _primaryStat switch
                {
                    PrimaryStatType.Strength => this.Strength,
                    PrimaryStatType.Dexterity => this.Dexterity,
                    PrimaryStatType.Intelligence => this.Intelligence,
                    _ => 0
                };
            }
            public int SetInitiative()
            {
                Initiative = (Dexterity - 10) / 2;
                return Initiative;
            }
            public NPC Clone()
            {
                return new NPC(Name, Description, _primaryStat, Strength, Dexterity, Intelligence, Constitution, Level, MoveSet, LootTable, GoldDrop);
            }
            public NPC(string Name, string Description, PrimaryStatType PrimaryStat, int Strength, int Dexterity, int Intelligence, int Constitution, int Level, Move[] MoveSet, LootTableItem[] LootTable, int GoldDrop = 0)
            {
                this.Name = Name;
                this.Description = Description;
                this._primaryStat = PrimaryStat;
                this.Strength = Strength;
                this.Dexterity = Dexterity;
                this.Intelligence = Intelligence;
                this.Constitution = Constitution;
                this.MoveSet = MoveSet;
                foreach (Move move in this.MoveSet)
                {
                    move.Description = move.Description.Replace("__", this.Name);
                }
                this.Health = Constitution * 10;
                this.Mana = Intelligence * 10;
                this.MaxHealth = this.Health;
                this.MaxMana = this.Mana;
                this.LootTable = LootTable;
                this.GoldDrop = GoldDrop;
                this.Level = Level;
                this.ExpDrop = (int)(100 * Math.Pow(1.07, this.Level));
            }
        }

        static Move Bite = new Move("Bite", "The __ sinks its teeth into you", 12, 0);
        static Move Club = new Move("Club", "The __ swings a wooden club", 15, 0);
        static Move Tackle = new Move("Tackle", "The __ tackles you", 10, 0);


        static Move[] goblinMoves = { Club };
        static LootTableItem[] goblinLoot = { new LootTableItem(ItemType.HealthPotion, 50), new LootTableItem(ItemType.Rock, 100) };
        static NPC Goblin = new NPC("Goblin", "A small green creature", PrimaryStatType.Strength, 10/*STR*/, 6/*DEX*/, 4/*INT*/, 10/*CON*/, 1/*LVL*/, goblinMoves, goblinLoot, 5/*Gold*/);
        static Move[] wolfMoves = { Bite };
        static LootTableItem[] wolfLoot = { new LootTableItem(ItemType.ManaPotion, 50), new LootTableItem(ItemType.Rock, 100) };
        static NPC Wolf = new NPC("Wolf", "A hungry forest predator", PrimaryStatType.Dexterity, 6/*STR*/, 12/*DEX*/, 4/*INT*/, 6/*CON*/, 1/*LVL*/, wolfMoves, wolfLoot, 0/*Gold*/);
    }
}


/* Handle Enemy EXP
  Option 1: Based on Enemy Level (most common)
  // Enemy drops EXP roughly equal to what player needs at that level
  expDrop = (int)(100 * Math.Pow(1.07, enemyLevel))

  // Or a fraction if you want multiple kills per level
  expDrop = (int)(50 * Math.Pow(1.07, enemyLevel))  // ~2 kills per level
  expDrop = (int)(33 * Math.Pow(1.07, enemyLevel))  // ~3 kills per level

  Option 2: Fixed percentage of player's current requirement
  // Enemy gives 20-30% of what player needs, based on player level
  expDrop = (int)(100 * Math.Pow(1.07, playerLevel) * 0.25)

  Option 3: Differential scaling (risk/reward for fighting higher level enemies)
  int levelDiff = enemyLevel - playerLevel;
  float multiplier = 1.0f + (levelDiff * 0.1f); // +10% per level difference
  expDrop = (int)(100 * Math.Pow(1.07, enemyLevel) * multiplier);
*/