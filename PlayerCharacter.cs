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
            // private int _strength;
            // private int _dexterity;
            // private int _intelligence;
            // private int _constitution;
            private int _initiative;
            private Stats _stats;
            private PrimaryStatType _primaryStat;
            private int _money;
            private int _level;
            private int _exp;
            private int _maxExp;
            private List<Item> _inventory = new List<Item>();
            private List<Item> _slots = new List<Item>();
            private Move[] _moveSet;
            private Location _currentLocation;

            public string Name { get => _name; [MemberNotNull(nameof(_name))] set => _name = value; }
            public string SpecName { get => _specName; [MemberNotNull(nameof(_specName))] set => _specName = value; }
            public int Health { get => _health; set => _health = value; }
            public int MaxHealth { get => _maxHealth; set => _maxHealth = value; }
            public int Mana { get => _mana; set => _mana = value; }
            public int MaxMana { get => _maxMana; set => _maxMana = value; }
            // public int Strength { get => _strength; set => _strength = value; }
            // public int Dexterity { get => _dexterity; set => _dexterity = value; }
            // public int Intelligence { get => _intelligence; set => _intelligence = value; }
            // public int Constitution { get => _constitution; set => _constitution = value; }
            public int Initiative { get => _initiative; set => _initiative = value; }
            public Stats Stats { get => _stats; [MemberNotNull(nameof(_stats))] set => _stats = value; }
            public int PrimaryStat
            {
                get =>
                    _primaryStat switch
                    {
                        PrimaryStatType.Strength => this.Stats.Strength,
                        PrimaryStatType.Dexterity => this.Stats.Dexterity,
                        PrimaryStatType.Intelligence => this.Stats.Intelligence,
                        _ => 0
                    };
            }
            public int Money { get => _money; set => _money = value; }
            public int Level { get => _level; set => _level = value; }
            public int Exp { get => _exp; set => _exp = value; }
            public int MaxExp { get => _maxExp; set => _maxExp = value; }
            public List<Item> Inventory { get => _inventory; [MemberNotNull(nameof(_inventory))] set => _inventory = value; }
            private List<Item> Slots { get => _slots; [MemberNotNull(nameof(_slots))] set => _slots = value; }
            public Move[] MoveSet { get => _moveSet; [MemberNotNull(nameof(_moveSet))] set => _moveSet = value; }
            public Location CurrentLocation { get => _currentLocation; [MemberNotNull(nameof(_currentLocation))] set => _currentLocation = value; }
            private int SetInitiative()
            {
                Initiative = (Stats.Dexterity - 10) / 2;
                return Initiative;
            }
            public PlayerCharacter(string Name, CharacterSpec Spec, Location StartingLocation)
            {
                this.Name = Name;
                this.SpecName = Spec.Name;
                this._stats = new Stats(Spec.Stats.Strength, Spec.Stats.Dexterity, Spec.Stats.Intelligence, Spec.Stats.Constitution, Spec.Stats.Wisdom, Spec.Stats.Luck);
                this._primaryStat = Spec.PrimaryStat;
                this.MoveSet = Spec.MoveSet;
                this.CurrentLocation = StartingLocation;
                this.Health = 100 + this.Stats.Constitution * 10;
                this.Mana = 100 + this.Stats.Intelligence * 10;
                this.MaxHealth = this.Health;
                this.MaxMana = this.Mana;
                this.Money = 0;
                this.Level = 1;
                this.MaxExp = (int)(100 * Math.Pow(1.07, Level));
                this.MaxExp -= 7;
                SetInitiative();
            }
            public void ExpAdd(int exp)
            {
                this.Exp += exp;
                if (this.Exp >= this.MaxExp)
                {
                    this.Level++;
                    int expOver = this.Exp - this.MaxExp;
                    this.Exp = 0;
                    this.Exp += expOver;
                    this.MaxExp = (int)(100 * Math.Pow(1.07, Level));
                }
            }

        }

        static void OpenInventory(PlayerCharacter player)
        {
            player.Inventory.ForEach(item => Console.WriteLine(item.Name));
            Console.WriteLine();
            Console.WriteLine(player.Money);
            Console.ReadKey();
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