using System.Diagnostics.CodeAnalysis;

namespace TextRPGOne
{
    partial class Program
    {
        class Item
        {
            private string _name;
            private string _description;
            private Guid _id;
            private ItemType _itemType;
            private EquipmentType _equipmentType;
            private Stats _stats;
            private ConsumableType _consumableType;
            private ResourceType _resourceType;
            private WeaponType _weaponType;
            private PhysicalDamageType _physicalDamageType;
            private ElementalDamageType? _elementalDamageType;
            private int _elementalDamage;

            public string Name { get => _name; [MemberNotNull(nameof(_name))] set => _name = value; }
            public string Description { get => _description; [MemberNotNull(nameof(_description))] set => _description = value; }
            public Guid Id { get => _id; }
            public ItemType ItemType { get => _itemType; [MemberNotNull(nameof(_itemType))] set => _itemType = value; }
            public EquipmentType EquipmentType { get => _equipmentType; [MemberNotNull(nameof(_equipmentType))] set => _equipmentType = value; }
            public Stats Stats { get => _stats; [MemberNotNull(nameof(_stats))] set => _stats = value; }
            public ConsumableType ConsumableType { get => _consumableType; [MemberNotNull(nameof(_consumableType))] set => _consumableType = value; }
            public ResourceType ResourceType { get => _resourceType; [MemberNotNull(nameof(_resourceType))] set => _resourceType = value; }
            public WeaponType WeaponType { get => _weaponType; [MemberNotNull(nameof(_weaponType))] set => _weaponType = value; }
            public PhysicalDamageType PhysicalDamageType { get => _physicalDamageType; [MemberNotNull(nameof(_physicalDamageType))] set => _physicalDamageType = value; }
            public ElementalDamageType? ElementalDamageType { get => _elementalDamageType; set => _elementalDamageType = value; }
            public int ElementalDamage { get => _elementalDamage; set => _elementalDamage = value; }
            public Item(string ItemName, string ItemDescription, ItemType ItemType, ConsumableType SubType)
            {
                this.Name = ItemName;
                this.Description = ItemDescription;
                Guid newUuid = Guid.NewGuid();
                this.ItemType = ItemType;
                this.ConsumableType = SubType;
                this._id = newUuid;
            }
            public Item(string ItemName, string ItemDescription, ItemType ItemType, ResourceType SubType)
            {
                this.Name = ItemName;
                this.Description = ItemDescription;
                Guid newUuid = Guid.NewGuid();
                this.ItemType = ItemType;
                this.ResourceType = SubType;
                this._id = newUuid;
            }
            public Item(string ItemName, string ItemDescription, ItemType ItemType, EquipmentType SubType, Stats Stats)
            {
                this.Name = ItemName;
                this.Description = ItemDescription;
                Guid newUuid = Guid.NewGuid();
                this.ItemType = ItemType;
                this.EquipmentType = SubType;
                this._id = newUuid;
                this.Stats = Stats;
            }

            // Constructor for weapons with damage types (no elemental)
            public Item(string ItemName, string ItemDescription, ItemType ItemType, EquipmentType SubType, Stats Stats,
                        WeaponType WeaponType, PhysicalDamageType PhysicalDamageType)
            {
                this.Name = ItemName;
                this.Description = ItemDescription;
                Guid newUuid = Guid.NewGuid();
                this.ItemType = ItemType;
                this.EquipmentType = SubType;
                this._id = newUuid;
                this.Stats = Stats;
                this.WeaponType = WeaponType;
                this.PhysicalDamageType = PhysicalDamageType;
                this.ElementalDamageType = null;
                this.ElementalDamage = 0;
            }

            // Constructor for weapons with damage types AND elemental damage
            public Item(string ItemName, string ItemDescription, ItemType ItemType, EquipmentType SubType, Stats Stats,
                        WeaponType WeaponType, PhysicalDamageType PhysicalDamageType,
                        ElementalDamageType ElementalDamageType, int ElementalDamage)
            {
                this.Name = ItemName;
                this.Description = ItemDescription;
                Guid newUuid = Guid.NewGuid();
                this.ItemType = ItemType;
                this.EquipmentType = SubType;
                this._id = newUuid;
                this.Stats = Stats;
                this.WeaponType = WeaponType;
                this.PhysicalDamageType = PhysicalDamageType;
                this.ElementalDamageType = ElementalDamageType;
                this.ElementalDamage = ElementalDamage;
            }
        }

        // Helper methods for elemental damage type categorization
        static bool IsDOT(ElementalDamageType type)
        {
            return type == ElementalDamageType.Poison || type == ElementalDamageType.Acid;
        }

        static bool IsTrueDamage(ElementalDamageType type)
        {
            return type == ElementalDamageType.Light || type == ElementalDamageType.Dark;
        }

        static bool IsBonusDamage(ElementalDamageType type)
        {
            return type == ElementalDamageType.Fire || type == ElementalDamageType.Frost;
        }

        class ItemDatabase
        {
            public static Dictionary<ItemName, Item> _definitions = new()
            {
                {ItemName.HealthPotion , new Item("Health Potion", "A small vial to restore health", ItemType.Consumable, ConsumableType.Potion)},
                {ItemName.ManaPotion , new Item("Mana Potion", "A small vial to restore mana", ItemType.Consumable, ConsumableType.Potion)},
                {ItemName.RustySword , new Item("Rusty Sword", "An old rusty sword", ItemType.Equipment, EquipmentType.OneHandWeapon,
                new Stats(2/*STR*/, 0/*DEX*/, 0/*INT*/, 0/*CON*/, 0/*WIS*/, 0/*LUK*/),
                WeaponType.Sword, PhysicalDamageType.Slashing)},
                {ItemName.WoodenStaff , new Item("Wooden Staff", "A old wood stick", ItemType.Equipment, EquipmentType.TwoHandWeapon,
                new Stats(0/*STR*/, 0/*DEX*/, 2/*INT*/, 0/*CON*/, 0/*WIS*/, 0/*LUK*/),
                WeaponType.Staff, PhysicalDamageType.Bludgeoning)},
                {ItemName.RustyDagger , new Item("Rusty Dagger", "A small rusted blade", ItemType.Equipment, EquipmentType.OneHandWeapon,
                new Stats(0/*STR*/, 1/*DEX*/, 0/*INT*/, 0/*CON*/, 0/*WIS*/, 0/*LUK*/),
                WeaponType.Dagger, PhysicalDamageType.Piercing)},
                {ItemName.Rock , new Item("Rock", "It's a rock (Trash it)", ItemType.Resources, ResourceType.Mining)},
                {ItemName.FlamingSword , new Item("Flaming Sword", "A sword wreathed in flames", ItemType.Equipment, EquipmentType.OneHandWeapon,
                new Stats(4/*STR*/, 0/*DEX*/, 0/*INT*/, 0/*CON*/, 0/*WIS*/, 0/*LUK*/),
                WeaponType.Sword, PhysicalDamageType.Slashing, ElementalDamageType.Fire, 10)},
                {ItemName.FrostBow , new Item("Frost Bow", "A bow that fires icy arrows", ItemType.Equipment, EquipmentType.TwoHandWeapon,
                new Stats(0/*STR*/, 5/*DEX*/, 0/*INT*/, 0/*CON*/, 0/*WIS*/, 0/*LUK*/),
                WeaponType.Bow, PhysicalDamageType.Piercing, ElementalDamageType.Frost, 12)},
                {ItemName.PoisonDagger , new Item("Poison Dagger", "A dagger coated with deadly poison", ItemType.Equipment, EquipmentType.OneHandWeapon,
                new Stats(0/*STR*/, 3/*DEX*/, 0/*INT*/, 0/*CON*/, 0/*WIS*/, 0/*LUK*/),
                WeaponType.Dagger, PhysicalDamageType.Piercing, ElementalDamageType.Poison, 15)},
                {ItemName.LightStaff , new Item("Staff of Light", "A holy staff that channels pure light", ItemType.Equipment, EquipmentType.TwoHandWeapon,
                new Stats(0/*STR*/, 0/*DEX*/, 6/*INT*/, 0/*CON*/, 2/*WIS*/, 0/*LUK*/),
                WeaponType.Staff, PhysicalDamageType.Bludgeoning, ElementalDamageType.Light, 20)},
                {ItemName.Antidote , new Item("Antidote", "Cures poison effects", ItemType.Consumable, ConsumableType.Potion)},
                {ItemName.AlkalineSolution , new Item("Alkaline Solution", "Neutralizes acid effects", ItemType.Consumable, ConsumableType.Potion)},
                {ItemName.FireResistPotion , new Item("Fire Resist Potion", "Grants temporary resistance to fire damage", ItemType.Consumable, ConsumableType.Potion)},
                {ItemName.FrostResistPotion , new Item("Frost Resist Potion", "Grants temporary resistance to frost damage", ItemType.Consumable, ConsumableType.Potion)},
                {ItemName.Poison , new Item("Poison", "A deadly toxin", ItemType.Consumable, ConsumableType.Potion)}
            };
            public static Item Get(ItemName name) => _definitions[name];
        }
        enum ItemName
        {
            HealthPotion,
            ManaPotion,
            RustySword,
            WoodenStaff,
            RustyDagger,
            Rock,
            FlamingSword,
            FrostBow,
            PoisonDagger,
            LightStaff,
            Antidote,
            AlkalineSolution,
            FireResistPotion,
            FrostResistPotion,
            Poison
        }
        enum ItemType
        {
            Consumable,
            Equipment,
            Resources
        }
        enum EquipmentType
        {
            Helmet,
            Shoulders,
            Chest,
            Waist,
            Legs,
            Boots,
            Necklace,
            Ring,
            OneHandWeapon,
            TwoHandWeapon
        }
        enum ConsumableType
        {
            Potion,
            Food,
            Scroll,
            Enchantments
        }
        enum ResourceType
        {
            Mining,
            Hunting,
            Herbalism,
            Crafting,
            Cooking,
            Jewelery

        }
        enum WeaponType
        {
            Sword,
            Shield,
            TwoHandSword,
            Pistol,
            Rifle,
            Dagger,
            Bow,
            Glaive,
            Staff,
            Book,
            KnuckleDuster
        }
        enum PhysicalDamageType
        {
            Bludgeoning,
            Piercing,
            Slashing
        }
        enum ElementalDamageType
        {
            Fire,      // Bonus DMG
            Frost,     // Bonus DMG
            Poison,    // D.O.T
            Acid,      // D.O.T
            Light,     // True DMG
            Dark       // True DMG
        }

    }
}