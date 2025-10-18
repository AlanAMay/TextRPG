using System.Diagnostics.CodeAnalysis;
using Spectre.Console;

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
            private int _initiative;
            private Stats _specStats;
            private Stats _stats;
            private PrimaryStatType _primaryStat;
            private int _money;
            private int _level;
            private int _exp;
            private int _maxExp;
            private List<Item> _inventory = new List<Item>();
            private Slots _slots = new Slots();
            private Move[] _moveSet;
            private Location _currentLocation;
            private Resistances _resistances = new Resistances();
            private List<DOTEffect> _activeDoTs = new List<DOTEffect>();
            private int _remainingActions;
            private int _maxActions;

            public string Name { get => _name; [MemberNotNull(nameof(_name))] set => _name = value; }
            public string SpecName { get => _specName; [MemberNotNull(nameof(_specName))] set => _specName = value; }
            public int Health { get => _health; set => _health = value; }
            public int MaxHealth { get => _maxHealth; set => _maxHealth = value; }
            public int Mana { get => _mana; set => _mana = value; }
            public int MaxMana { get => _maxMana; set => _maxMana = value; }
            public int Initiative { get => _initiative; set => _initiative = value; }
            public Stats SpecStats { get => _specStats; }
            public Stats Stats
            {
                get
                {
                    // Start with base spec stats
                    int str = _specStats.Strength;
                    int dex = _specStats.Dexterity;
                    int intel = _specStats.Intelligence;
                    int con = _specStats.Constitution;
                    int wis = _specStats.Wisdom;
                    int luck = _specStats.Luck;

                    // Add equipment stats
                    foreach (var item in _slots.GetAllEquippedItems())
                    {
                        str += item.Stats.Strength;
                        dex += item.Stats.Dexterity;
                        intel += item.Stats.Intelligence;
                        con += item.Stats.Constitution;
                        wis += item.Stats.Wisdom;
                        luck += item.Stats.Luck;
                    }

                    return new Stats(str, dex, intel, con, wis, luck);
                }
            }
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
            public PrimaryStatType PrimaryStatType { get => _primaryStat; }
            public int Money { get => _money; set => _money = value; }
            public int Level { get => _level; set => _level = value; }
            public int Exp { get => _exp; set => _exp = value; }
            public int MaxExp { get => _maxExp; set => _maxExp = value; }
            public List<Item> Inventory { get => _inventory; [MemberNotNull(nameof(_inventory))] set => _inventory = value; }
            public Slots EqupimentSlots { get => _slots; [MemberNotNull(nameof(_slots))] set => _slots = value; }
            public Move[] MoveSet { get => _moveSet; [MemberNotNull(nameof(_moveSet))] set => _moveSet = value; }
            public Location CurrentLocation { get => _currentLocation; [MemberNotNull(nameof(_currentLocation))] set => _currentLocation = value; }
            public Resistances Resistances { get => _resistances; [MemberNotNull(nameof(_resistances))] set => _resistances = value; }
            public List<DOTEffect> ActiveDoTs { get => _activeDoTs; [MemberNotNull(nameof(_activeDoTs))] set => _activeDoTs = value; }
            public int RemainingActions { get => _remainingActions; set => _remainingActions = value; }
            public int MaxActions { get => _maxActions; set => _maxActions = value; }
            private int SetInitiative()
            {
                Initiative = (Stats.Dexterity - 10) / 2;
                return Initiative;
            }
            public PlayerCharacter(string Name, CharacterSpec Spec, Location StartingLocation)
            {
                this.Name = Name;
                this.SpecName = Spec.Name;
                this._specStats = new Stats(Spec.Stats.Strength, Spec.Stats.Dexterity, Spec.Stats.Intelligence, Spec.Stats.Constitution, Spec.Stats.Wisdom, Spec.Stats.Luck);
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
                this.MaxActions = 10; // Default starting actions per dungeon floor
                this.RemainingActions = this.MaxActions;
                SetInitiative();
            }

            public bool UseAction()
            {
                if (RemainingActions > 0)
                {
                    RemainingActions--;
                    return true;
                }
                return false;
            }

            public void ResetActions()
            {
                RemainingActions = MaxActions;
            }
            public void ExpAdd(int exp)
            {
                this.Exp += exp;
                if (this.Exp >= this.MaxExp)
                {
                    int expOver = this.Exp - this.MaxExp;
                    this.Exp = 0;
                    this.Exp += expOver;
                    this.MaxExp = (int)(100 * Math.Pow(1.07, Level + 1));

                    // Call the level up stat allocation
                    LevelUp();
                }
            }

            public void LevelUp()
            {
                this.Level++;
                int pointsToAllocate = 5;

                AnsiConsole.Clear();
                AnsiConsole.Write(
                    new FigletText("LEVEL UP!")
                        .Centered()
                        .Color(Color.Gold1));

                AnsiConsole.MarkupLine($"\n[yellow]You reached level {this.Level}![/]");
                AnsiConsole.MarkupLine($"[green]You have {pointsToAllocate} stat points to allocate.[/]\n");

                // Track how many points allocated to each stat
                int strPoints = 0;
                int dexPoints = 0;
                int intPoints = 0;
                int conPoints = 0;
                int wisPoints = 0;
                int luckPoints = 0;

                while (pointsToAllocate > 0)
                {
                    AnsiConsole.Clear();
                    AnsiConsole.MarkupLine($"[yellow]Level Up - Points Remaining: {pointsToAllocate}[/]\n");

                    // Show current base stats and pending increases
                    var table = new Table();
                    table.AddColumn("Stat");
                    table.AddColumn("Current");
                    table.AddColumn("Pending");
                    table.AddColumn("New Total");

                    table.AddRow("Strength", _specStats.Strength.ToString(), $"[green]+{strPoints}[/]",
                        (_specStats.Strength + strPoints).ToString());
                    table.AddRow("Dexterity", _specStats.Dexterity.ToString(), $"[green]+{dexPoints}[/]",
                        (_specStats.Dexterity + dexPoints).ToString());
                    table.AddRow("Intelligence", _specStats.Intelligence.ToString(), $"[green]+{intPoints}[/]",
                        (_specStats.Intelligence + intPoints).ToString());
                    table.AddRow("Constitution", _specStats.Constitution.ToString(), $"[green]+{conPoints}[/]",
                        (_specStats.Constitution + conPoints).ToString());
                    table.AddRow("Wisdom", _specStats.Wisdom.ToString(), $"[green]+{wisPoints}[/]",
                        (_specStats.Wisdom + wisPoints).ToString());
                    table.AddRow("Luck", _specStats.Luck.ToString(), $"[green]+{luckPoints}[/]",
                        (_specStats.Luck + luckPoints).ToString());

                    AnsiConsole.Write(table);
                    Console.WriteLine();

                    // Let player choose which stat to increase
                    var choices = new List<string>
                    {
                        "Strength",
                        "Dexterity",
                        "Intelligence",
                        "Constitution",
                        "Wisdom",
                        "Luck"
                    };

                    if (strPoints > 0 || dexPoints > 0 || intPoints > 0 ||
                        conPoints > 0 || wisPoints > 0 || luckPoints > 0)
                    {
                        choices.Add("[dim]<< Reset Points[/]");
                    }

                    var selection = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                            .Title("[yellow]Allocate a point to which stat?[/]")
                            .AddChoices(choices));

                    if (selection == "[dim]<< Reset Points[/]")
                    {
                        // Reset all allocations
                        pointsToAllocate += strPoints + dexPoints + intPoints + conPoints + wisPoints + luckPoints;
                        strPoints = dexPoints = intPoints = conPoints = wisPoints = luckPoints = 0;
                        continue;
                    }

                    // Allocate the point
                    switch (selection)
                    {
                        case "Strength": strPoints++; break;
                        case "Dexterity": dexPoints++; break;
                        case "Intelligence": intPoints++; break;
                        case "Constitution": conPoints++; break;
                        case "Wisdom": wisPoints++; break;
                        case "Luck": luckPoints++; break;
                    }

                    pointsToAllocate--;
                }

                // Confirm allocation
                AnsiConsole.Clear();
                AnsiConsole.MarkupLine("[yellow]Confirm your stat allocation?[/]\n");

                var confirmTable = new Table();
                confirmTable.AddColumn("Stat");
                confirmTable.AddColumn("Increase");
                confirmTable.AddColumn("New Value");

                if (strPoints > 0) confirmTable.AddRow("Strength", $"+{strPoints}", (_specStats.Strength + strPoints).ToString());
                if (dexPoints > 0) confirmTable.AddRow("Dexterity", $"+{dexPoints}", (_specStats.Dexterity + dexPoints).ToString());
                if (intPoints > 0) confirmTable.AddRow("Intelligence", $"+{intPoints}", (_specStats.Intelligence + intPoints).ToString());
                if (conPoints > 0) confirmTable.AddRow("Constitution", $"+{conPoints}", (_specStats.Constitution + conPoints).ToString());
                if (wisPoints > 0) confirmTable.AddRow("Wisdom", $"+{wisPoints}", (_specStats.Wisdom + wisPoints).ToString());
                if (luckPoints > 0) confirmTable.AddRow("Luck", $"+{luckPoints}", (_specStats.Luck + luckPoints).ToString());

                AnsiConsole.Write(confirmTable);
                Console.WriteLine();

                var confirm = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .AddChoices("Confirm", "Redo Allocation"));

                if (confirm == "Redo Allocation")
                {
                    LevelUp(); // Recursively call to start over
                    return;
                }

                // Apply the stat increases to _specStats
                _specStats.Strength += strPoints;
                _specStats.Dexterity += dexPoints;
                _specStats.Intelligence += intPoints;
                _specStats.Constitution += conPoints;
                _specStats.Wisdom += wisPoints;
                _specStats.Luck += luckPoints;

                // Update max HP/Mana based on new constitution/intelligence
                this.MaxHealth = 100 + this.Stats.Constitution * 10;
                this.MaxMana = 100 + this.Stats.Intelligence * 10;
                this.Health = this.MaxHealth; // Full heal on level up
                this.Mana = this.MaxMana;

                AnsiConsole.MarkupLine("\n[green]Stats updated successfully![/]");
                AnsiConsole.MarkupLine("[green]HP and Mana fully restored![/]");
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }

        }

        static void OpenInventory(PlayerCharacter player)
        {
            if (player.Inventory.Count == 0)
            {
                AnsiConsole.MarkupLine("[yellow]Your inventory is empty.[/]");
                Console.WriteLine("\nPress any key to return...");
                Console.ReadKey();
                return;
            }

            while (true)
            {
                AnsiConsole.Clear();

                // Display money at top
                AnsiConsole.MarkupLine($"[yellow]Gold:[/] [gold1]{player.Money}[/]\n");

                // Create a selection prompt with formatted items
                var inventoryChoices = new List<string>();
                foreach (var item in player.Inventory)
                {
                    string itemDisplay = FormatInventoryItem(item);
                    inventoryChoices.Add(itemDisplay);
                }
                inventoryChoices.Add("[dim]<< Back[/]");

                var selection = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[yellow]Inventory[/]")
                        .PageSize(15)
                        .MoreChoicesText("[grey](Move up and down to see more items)[/]")
                        .AddChoices(inventoryChoices));

                if (selection == "[dim]<< Back[/]")
                    break;

                // Find the selected item
                int selectedIndex = inventoryChoices.IndexOf(selection);
                if (selectedIndex >= 0 && selectedIndex < player.Inventory.Count)
                {
                    Item selectedItem = player.Inventory[selectedIndex];
                    HandleItemActions(player, selectedItem);
                }
            }
        }

        static string FormatInventoryItem(Item item)
        {
            string subType = item.ItemType switch
            {
                ItemType.Equipment => item.EquipmentType.ToString(),
                ItemType.Consumable => item.ConsumableType.ToString(),
                ItemType.Resources => item.ResourceType.ToString(),
                _ => ""
            };

            return $"{item.Name,-30} [dim]{subType}[/]";
        }

        static void HandleItemActions(PlayerCharacter player, Item item)
        {
            while (true)
            {
                AnsiConsole.Clear();

                // Display item details
                var panel = new Panel($"[yellow]{item.Description}[/]\n\n[dim]Type: {item.ItemType} - {GetItemSubType(item)}[/]")
                    .Header($"[bold yellow] {item.Name} [/]")
                    .Border(BoxBorder.Rounded)
                    .BorderColor(Color.Yellow);
                AnsiConsole.Write(panel);
                Console.WriteLine();

                // Build action list based on item type
                var actions = new List<string>();

                if (item.ItemType == ItemType.Consumable)
                    actions.Add("Use");

                if (item.ItemType == ItemType.Equipment)
                    actions.Add("Equip");

                actions.Add("Drop");
                actions.Add("Examine");
                actions.Add("<< Back");

                var action = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[yellow]What would you like to do?[/]")
                        .AddChoices(actions));

                switch (action)
                {
                    case "Use":
                        UseItem(player, item);
                        return; // Return to inventory after use
                    case "Equip":
                        EquipItem(player, item);
                        return; // Return to inventory after equip
                    case "Drop":
                        if (DropItem(player, item))
                            return; // Return to inventory if item was dropped
                        break; // Stay in action menu if drop was cancelled
                    case "Examine":
                        ExamineItem(item);
                        break; // Stay in action menu
                    case "<< Back":
                        return; // Return to inventory
                }
            }
        }

        static string GetItemSubType(Item item)
        {
            return item.ItemType switch
            {
                ItemType.Equipment => item.EquipmentType.ToString(),
                ItemType.Consumable => item.ConsumableType.ToString(),
                ItemType.Resources => item.ResourceType.ToString(),
                _ => "Unknown"
            };
        }

        static void UseItem(PlayerCharacter player, Item item)
        {
            if (item.ItemType != ItemType.Consumable)
            {
                AnsiConsole.MarkupLine("[red]This item cannot be used![/]");
                Console.ReadKey();
                return;
            }

            // Handle different consumable types
            switch (item.ConsumableType)
            {
                case ConsumableType.Potion:
                    UsePotionItem(player, item);
                    break;
                case ConsumableType.Food:
                    UseFoodItem(player, item);
                    break;
                case ConsumableType.Scroll:
                    AnsiConsole.MarkupLine("[yellow]Scrolls are not yet implemented.[/]");
                    Console.ReadKey();
                    return;
                case ConsumableType.Enchantments:
                    AnsiConsole.MarkupLine("[yellow]Enchantments are not yet implemented.[/]");
                    Console.ReadKey();
                    return;
            }

            // Remove item from inventory after use
            player.Inventory.Remove(item);
        }

        static void UsePotionItem(PlayerCharacter player, Item item)
        {
            // Determine potion effect based on name (this could be expanded with item properties)
            if (item.Name.Contains("Health"))
            {
                int healAmount = 50; // Could be a property of the item
                int actualHeal = Math.Min(healAmount, player.MaxHealth - player.Health);
                player.Health += actualHeal;

                AnsiConsole.MarkupLine($"[green]You drink the {item.Name} and restore {actualHeal} health![/]");
                AnsiConsole.MarkupLine($"[green]Current Health: {player.Health}/{player.MaxHealth}[/]");
            }
            else if (item.Name.Contains("Mana"))
            {
                int manaAmount = 50; // Could be a property of the item
                int actualRestore = Math.Min(manaAmount, player.MaxMana - player.Mana);
                player.Mana += actualRestore;

                AnsiConsole.MarkupLine($"[cyan]You drink the {item.Name} and restore {actualRestore} mana![/]");
                AnsiConsole.MarkupLine($"[cyan]Current Mana: {player.Mana}/{player.MaxMana}[/]");
            }

            Console.ReadKey();
        }

        static void UseFoodItem(PlayerCharacter player, Item item)
        {
            // Food typically restores health slowly or provides buffs
            int healAmount = 30; // Could be a property of the item
            int actualHeal = Math.Min(healAmount, player.MaxHealth - player.Health);
            player.Health += actualHeal;

            AnsiConsole.MarkupLine($"[green]You eat the {item.Name} and restore {actualHeal} health![/]");
            AnsiConsole.MarkupLine($"[green]Current Health: {player.Health}/{player.MaxHealth}[/]");
            Console.ReadKey();
        }

        static void EquipItem(PlayerCharacter player, Item item)
        {
            if (item.ItemType != ItemType.Equipment)
            {
                AnsiConsole.MarkupLine("[red]This item cannot be equipped![/]");
                Console.ReadKey();
                return;
            }

            AnsiConsole.Clear();
            AnsiConsole.MarkupLine($"[yellow]Equipping {item.Name}...[/]\n");

            // Use the Slots.Equip method which returns list of unequipped items
            List<Item> unequippedItems = player.EqupimentSlots.Equip(item);

            // Remove the equipped item from inventory
            player.Inventory.Remove(item);

            // Add any unequipped items back to inventory
            if (unequippedItems.Count > 0)
            {
                foreach (var unequippedItem in unequippedItems)
                {
                    player.Inventory.Add(unequippedItem);
                }

                AnsiConsole.MarkupLine($"\n[green]{item.Name} equipped successfully![/]");
                AnsiConsole.MarkupLine($"[yellow]Unequipped items returned to inventory:[/]");
                foreach (var unequippedItem in unequippedItems)
                {
                    AnsiConsole.MarkupLine($"  - {unequippedItem.Name}");
                }
            }
            else
            {
                AnsiConsole.MarkupLine($"\n[green]{item.Name} equipped successfully![/]");
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        static bool DropItem(PlayerCharacter player, Item item)
        {
            var confirm = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title($"[red]Are you sure you want to drop {item.Name}?[/]")
                    .AddChoices("Yes", "No"));

            if (confirm == "Yes")
            {
                player.Inventory.Remove(item);
                AnsiConsole.MarkupLine($"[yellow]You dropped {item.Name}.[/]");
                Console.ReadKey();
                return true;
            }
            return false;
        }

        static void ExamineItem(Item item)
        {
            AnsiConsole.Clear();

            var details = new Panel(BuildItemDetails(item))
                .Header($"[bold yellow] {item.Name} [/]")
                .Border(BoxBorder.Double)
                .BorderColor(Color.Yellow)
                .Padding(2, 1);

            AnsiConsole.Write(details);
            Console.WriteLine("\nPress any key to return...");
            Console.ReadKey();
        }

        static string BuildItemDetails(Item item)
        {
            var details = new System.Text.StringBuilder();

            details.AppendLine($"[yellow]Description:[/]");
            details.AppendLine($"  {item.Description}\n");

            details.AppendLine($"[cyan]Type:[/] {item.ItemType}");
            details.AppendLine($"[cyan]Category:[/] {GetItemSubType(item)}\n");

            if (item.ItemType == ItemType.Equipment)
            {
                details.AppendLine($"[green]Stats:[/]");
                if (item.Stats.Strength > 0)
                    details.AppendLine($"  Strength: +{item.Stats.Strength}");
                if (item.Stats.Dexterity > 0)
                    details.AppendLine($"  Dexterity: +{item.Stats.Dexterity}");
                if (item.Stats.Intelligence > 0)
                    details.AppendLine($"  Intelligence: +{item.Stats.Intelligence}");
                if (item.Stats.Constitution > 0)
                    details.AppendLine($"  Constitution: +{item.Stats.Constitution}");
                if (item.Stats.Wisdom > 0)
                    details.AppendLine($"  Wisdom: +{item.Stats.Wisdom}");
                if (item.Stats.Luck > 0)
                    details.AppendLine($"  Luck: +{item.Stats.Luck}");
            }

            return details.ToString();
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