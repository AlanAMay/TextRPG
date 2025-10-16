namespace TextRPGOne
{
    partial class Program
    {
        class Slots
        {
            private Item _helmet;
            private Item _shoulders;
            private Item _chest;
            private Item _waist;
            private Item _legs;
            private Item _boots;
            private Item _necklace;
            private Item _ringOne;
            private Item _ringTwo;
            private Item _mainHand;
            private Item _offHand;
            public Item Helmet;
            public Item Shoulders;
            public Item Chest;
            public Item Waist;
            public Item Legs;
            public Item Boots;
            public Item Necklace;
            public Item RingOne;
            public Item RingTwo;
            public Item MainHand;
            public Item OffHand;
            public Item Equip(Item item)
            {
                switch (item)
                {
                    case item.EquipmentType == EquipmentType.Helmet:
                        return EquipHandler(item, Helmet);
                    case item.EquipmentType == EquipmentType.Shoulders:
                        return EquipHandler(item, Shoulders);
                    case item.EquipmentType == EquipmentType.Chest:
                        return EquipHandler(item, Chest);
                    case item.EquipmentType == EquipmentType.Waist:
                        return EquipHandler(item, Waist);
                    case item.EquipmentType == EquipmentType.Legs:
                        return EquipHandler(item, Legs);
                    case item.EquipmentType == EquipmentType.Boots:
                        return EquipHandler(item, Boots);
                    case item.EquipmentType == EquipmentType.Necklace:
                        return EquipHandler(item, Necklace);
                    case item.EquipmentType == EquipmentType.Ring:
                        if (RingTwo == null)
                        {
                            return EquipHandler(item, RingTwo);
                        }
                        else
                        {
                            return EquipHandler(item, RingOne);
                        }

                }


                //TODO Finish Adding equip handlers, make seperate handlers for rings / weapons.
                //     Ring,
                // OneHandWeapon,
                // TwoHandWeapon
            }
            private Item EquipHandler(Item item, Item slot)
            {
                if (this.slot == null)
                {
                    slot = item;
                    return null;
                }
                else
                {
                    Console.WriteLine($"Unequip {slot}\n{slot.Stats}");
                    Console.WriteLine("y/n");
                    if (Console.ReadKey("y"))
                    {
                        Item unequip = slot;
                        slot = item;
                        return unequip;
                    }
                }
            }
        }
    }
}