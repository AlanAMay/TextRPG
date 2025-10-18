using System.Diagnostics.CodeAnalysis;

namespace TextRPGOne
{
    partial class Program
    {
        public enum PrimaryStatType
        {
            Strength,
            Dexterity,
            Intelligence
        }
        class CharacterSpec
        {
            private string _name;
            private string _description;
            private int _health;
            private int _mana;
            private Stats _stats;
            private PrimaryStatType _primaryStat;
            private Move[] _moveSet;

            public string Name { get => _name; [MemberNotNull(nameof(_name))] set => _name = value; }
            public string Description { get => _description; [MemberNotNull(nameof(_description))] set => _description = value; }
            public int Health { get => _health; set => _health = value; }
            public int Mana { get => _mana; set => _mana = value; }
            public Stats Stats { get => _stats; [MemberNotNull(nameof(_stats))] set => _stats = value; }
            public PrimaryStatType PrimaryStat { get => _primaryStat; }
            public Move[] MoveSet { get => _moveSet; [MemberNotNull(nameof(_moveSet))] set => _moveSet = value; }

            public CharacterSpec(string Name, string Description, PrimaryStatType PrimaryStat, Stats Stats, Move[] MoveSet)
            {
                this.Name = Name;
                this.Description = Description;
                this._primaryStat = PrimaryStat;
                this.Stats = Stats;
                this.MoveSet = MoveSet;
                this.Health = 100 + Stats.Constitution * 10;
                this.Mana = 100 + Stats.Intelligence * 10;
            }
        }
        static Move Slash = new Move("Slash", "You swing your sword", 20, 0);
        static Move Block = new Move("Block", "Block Chance Up", 0, 10);
        static Move WarCry = new Move("War Cry", "STR Up", 0, 10);
        static Move Furry = new Move("Furry", "You unleash multiple slashes against the enemy", 0, 10);
        static Move[] WarriorMoveSet = {
            Slash,
            Block,
            WarCry,
            Furry
        };
        static Stats WarriorStats = new Stats(14/*STR*/, 8/*DEX*/, 8/*INT*/, 12/*CON*/, 8/*WIS*/, 10/*LUK*/)/*60 Total*/;
        static CharacterSpec Warrior = new CharacterSpec("Warrior", "The Warrior is a stout class with decent damage", PrimaryStatType.Strength, WarriorStats, WarriorMoveSet);

        static Move Stab = new Move("Stab", "You gouge them with your blade", 28, 0);
        static Move Dodge = new Move("Dodge", "You try to avoid the next attack", 0, 0);
        static Move[] RogueMoveSet = {
            Stab,
            Dodge
        };
        static Stats RogueStats = new Stats(8/*STR*/, 16/*DEX*/, 8/*INT*/, 6/*CON*/, 10/*WIS*/, 12/*LUK*/)/*60 Total*/;
        static CharacterSpec Rogue = new CharacterSpec("Rogue", "The Rogue is a high damage class with low health", PrimaryStatType.Dexterity, RogueStats, RogueMoveSet);

        static Move Fireball = new Move("Fireball", "You hurl a ball of fire", 35, 20);
        static Move Barrier = new Move("Barrier", "You cast a barrier around yourself", 0, 35);
        static Move[] MageMoveSet = {
            Fireball,
            Barrier
        };
        static Stats MageStats = new Stats(6/*STR*/, 6/*DEX*/, 16/*INT*/, 8/*CON*/, 12/*WIS*/, 12/*LUK*/)/*60 Total*/;
        static CharacterSpec Mage = new CharacterSpec("Mage", "The Mage is busted", PrimaryStatType.Intelligence, MageStats, MageMoveSet);
    }
}