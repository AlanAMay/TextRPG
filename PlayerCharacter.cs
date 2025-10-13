using System.Diagnostics.CodeAnalysis;

namespace TextRPGOne
{
    partial class Program
    {
        class PlayerCharacter
        {
            private string _name;
            private string _specName;
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
            private Location _currentLocation;
            private PrimaryStatType _primaryStat;

            public string Name { get => _name; [MemberNotNull(nameof(_name))] set => _name = value; }
            public string SpecName { get => _specName; [MemberNotNull(nameof(_specName))] set => _specName = value; }
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
            public Location CurrentLocation { get => _currentLocation; [MemberNotNull(nameof(_currentLocation))] set => _currentLocation = value; }
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
            private int SetInitiative()
            {
                Initiative = (Dexterity - 10) / 2;
                return Initiative;
            }
            public PlayerCharacter(string Name, string SpecName, PrimaryStatType PrimaryStat, int Strength, int Dexterity, int Intelligence, int Constitution, Move[] MoveSet, Location StartingLocation)
            {
                this.Name = Name;
                this.SpecName = SpecName;
                this._primaryStat = PrimaryStat;
                this.Strength = Strength;
                this.Dexterity = Dexterity;
                this.Intelligence = Intelligence;
                this.Constitution = Constitution;
                this.MoveSet = MoveSet;
                this.CurrentLocation = StartingLocation;
                this.Health = 100 + Constitution * 10;
                this.Mana = 100 + Intelligence * 10;
                this.MaxHealth = this.Health;
                this.MaxMana = this.Mana;
            }

        }
        static PlayerCharacter? playerCharacter;

        // static void PlayerStats()
        // {
        //     Console.WriteLine(playerCharacter.Strength);
        //     Console.WriteLine(playerCharacter.Dexterity);
        //     Console.WriteLine(playerCharacter.Inteintelligence);
        //     Console.WriteLine(playerCharacter.Constitution);
        //     Console.WriteLine(playerCharacter.Initiative);
        //     Console.WriteLine();
        // }
    }
}