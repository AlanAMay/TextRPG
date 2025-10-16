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

            public string Name { get => _name; [MemberNotNull(nameof(_name))] set => _name = value; }
            public string Description { get => _description; [MemberNotNull(nameof(_description))] set => _description = value; }
            public Guid Id { get => _id; }
            public ItemType ItemType { get => _itemType; [MemberNotNull(nameof(_itemType))] set => _itemType = value; }
            public EquipmentType EquipmentType { get => _equipmentType; [MemberNotNull(nameof(_equipmentType))] set => _equipmentType = value; }
            public Stats Stats { get => _stats; [MemberNotNull(nameof(_stats))] set => _stats = value; }
            public ConsumableType ConsumableType { get => _consumableType; [MemberNotNull(nameof(_consumableType))] set => _consumableType = value; }
            public ResourceType ResourceType { get => _resourceType; [MemberNotNull(nameof(_resourceType))] set => _resourceType = value; }
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
        }
        enum ItemName
        {
            HealthPotion,
            ManaPotion,
            RustySword,
            WoodenStaff,
            RustyDagger,
            Rock
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
        class ItemDatabase
        {
            public static Dictionary<ItemName, Item> _definitions = new()
            {
                {ItemName.HealthPotion , new Item("Health Potion", "A small vial to restore health", ItemType.Consumable, ConsumableType.Potion)},
                {ItemName.ManaPotion , new Item("Mana Potion", "A small vial to restore mana", ItemType.Consumable, ConsumableType.Potion)},
                {ItemName.RustySword , new Item("Rusty Sword", "An old rusty sword", ItemType.Equipment, EquipmentType.OneHandWeapon)},
                {ItemName.WoodenStaff , new Item("Wooden Staff", "A old wood stick", ItemType.Equipment, EquipmentType.TwoHandWeapon)},
                {ItemName.RustyDagger , new Item("Rusty Dagger", "A small rusted blade", ItemType.Equipment, EquipmentType.OneHandWeapon)},
                {ItemName.Rock , new Item("Rock", "It's a rock (Trash it)", ItemType.Resources, ResourceType.Mining)}
            };
            public static Item Get(ItemName name) => _definitions[name];
        }

    }
}