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

            public IEnumerable<Item> GetAllEquippedItems()
            {
                if (Helmet != null) yield return Helmet;
                if (Shoulders != null) yield return Shoulders;
                if (Chest != null) yield return Chest;
                if (Waist != null) yield return Waist;
                if (Legs != null) yield return Legs;
                if (Boots != null) yield return Boots;
                if (Necklace != null) yield return Necklace;
                if (RingOne != null) yield return RingOne;
                if (RingTwo != null) yield return RingTwo;
                if (MainHand != null) yield return MainHand;
                if (OffHand != null) yield return OffHand;
            }

            public List<Item> Equip(Item item)
            {
                List<Item> unequippedItems = new List<Item>();

                switch (item.EquipmentType)
                {
                    case EquipmentType.Helmet:
                        SlotHandler(item, ref Helmet, out Item helmetUnequip);
                        if (helmetUnequip != null) unequippedItems.Add(helmetUnequip);
                        break;
                    case EquipmentType.Shoulders:
                        SlotHandler(item, ref Shoulders, out Item shouldersUnequip);
                        if (shouldersUnequip != null) unequippedItems.Add(shouldersUnequip);
                        break;
                    case EquipmentType.Chest:
                        SlotHandler(item, ref Chest, out Item chestUnequip);
                        if (chestUnequip != null) unequippedItems.Add(chestUnequip);
                        break;
                    case EquipmentType.Waist:
                        SlotHandler(item, ref Waist, out Item waistUnequip);
                        if (waistUnequip != null) unequippedItems.Add(waistUnequip);
                        break;
                    case EquipmentType.Legs:
                        SlotHandler(item, ref Legs, out Item legsUnequip);
                        if (legsUnequip != null) unequippedItems.Add(legsUnequip);
                        break;
                    case EquipmentType.Boots:
                        SlotHandler(item, ref Boots, out Item bootsUnequip);
                        if (bootsUnequip != null) unequippedItems.Add(bootsUnequip);
                        break;
                    case EquipmentType.Necklace:
                        SlotHandler(item, ref Necklace, out Item necklaceUnequip);
                        if (necklaceUnequip != null) unequippedItems.Add(necklaceUnequip);
                        break;
                    case EquipmentType.Ring:
                        RingSlotHandler(item, out Item ringUnequip);
                        if (ringUnequip != null) unequippedItems.Add(ringUnequip);
                        break;
                    case EquipmentType.OneHandWeapon:
                    case EquipmentType.TwoHandWeapon:
                        WeaponSlotHandler(item, out Item unequipOne, out Item unequipTwo);
                        if (unequipOne != null) unequippedItems.Add(unequipOne);
                        if (unequipTwo != null) unequippedItems.Add(unequipTwo);
                        break;
                }

                return unequippedItems;
            }
            private void SlotHandler(Item item, ref Item slot, out Item unequipped)
            {
                unequipped = null;

                if (slot == null)
                {
                    slot = item;
                    return;
                }
                else
                {
                    Console.WriteLine($"Unequip {slot.Name}? (y/n)");
                    Console.WriteLine($"{slot.Stats}");

                    bool isValid;
                    string input;
                    do
                    {
                        input = Console.ReadLine()?.ToLower() ?? "";
                        isValid = input == "y" || input == "n";

                        if (!isValid)
                        {
                            Console.WriteLine("Invalid choice. Please enter y or n.");
                        }
                    } while (!isValid);

                    if (input == "y")
                    {
                        unequipped = slot;
                        slot = item;
                    }
                    else
                    {
                        unequipped = item; // Return the new item back to inventory
                    }
                }
            }
            private void RingSlotHandler(Item item, out Item unequipped)
            {
                unequipped = null;

                if (RingOne == null)
                {
                    RingOne = item;
                    return;
                }
                else if (RingTwo == null)
                {
                    RingTwo = item;
                    return;
                }
                else
                {
                    Console.WriteLine($"Which ring would you like to replace?");
                    Console.WriteLine($"1. {RingOne.Name}");
                    Console.WriteLine($"2. {RingTwo.Name}");

                    int playerChoice;
                    bool isValid;
                    do
                    {
                        string input = Console.ReadLine() ?? "";
                        bool isParsed = int.TryParse(input, out playerChoice);
                        bool isInRange = playerChoice >= 1 && playerChoice <= 2;
                        isValid = isParsed && isInRange;

                        if (!isValid)
                        {
                            Console.WriteLine("Invalid choice. Please enter 1 or 2.");
                        }
                    } while (!isValid);

                    switch (playerChoice)
                    {
                        case 1:
                            unequipped = RingOne;
                            RingOne = item;
                            return;
                        case 2:
                            unequipped = RingTwo;
                            RingTwo = item;
                            return;
                        default:
                            unequipped = item;
                            return;
                    }
                }
            }
            private void WeaponSlotHandler(Item item, out Item unequipOne, out Item unequipTwo)
            {
                unequipOne = null;
                unequipTwo = null;

                if (item.EquipmentType == EquipmentType.OneHandWeapon)
                {
                    // If mainHand is empty, equip there
                    if (MainHand == null)
                    {
                        MainHand = item;
                        return;
                    }
                    // If mainHand is full but offHand is empty, equip to offHand
                    else if (OffHand == null)
                    {
                        OffHand = item;
                        return;
                    }
                    // Both slots full - ask which to replace
                    else
                    {
                        Console.WriteLine($"Which weapon would you like to replace?");
                        Console.WriteLine($"1. {MainHand.Name}");
                        Console.WriteLine($"2. {OffHand.Name}");

                        int playerChoice;
                        bool isValid;
                        do
                        {
                            string input = Console.ReadLine() ?? "";
                            bool isParsed = int.TryParse(input, out playerChoice);
                            bool isInRange = playerChoice >= 1 && playerChoice <= 2;
                            isValid = isParsed && isInRange;

                            if (!isValid)
                            {
                                Console.WriteLine("Invalid choice. Please enter 1 or 2.");
                            }
                        } while (!isValid);

                        switch (playerChoice)
                        {
                            case 1:
                                unequipOne = MainHand;
                                MainHand = item;
                                return;
                            case 2:
                                unequipOne = OffHand;
                                OffHand = item;
                                return;
                            default:
                                unequipOne = item;
                                return;
                        }
                    }
                }
                else if (item.EquipmentType == EquipmentType.TwoHandWeapon)
                {
                    // If both hands empty, equip the two-hander
                    if (MainHand == null && OffHand == null)
                    {
                        MainHand = item;
                        return;
                    }
                    // If hands are occupied, ask for confirmation
                    else
                    {
                        string message = "Would you like to unequip ";
                        if (MainHand != null && OffHand != null)
                        {
                            message += $"{MainHand.Name} and {OffHand.Name}";
                        }
                        else if (MainHand != null)
                        {
                            message += $"{MainHand.Name}";
                        }
                        else
                        {
                            message += $"{OffHand.Name}";
                        }
                        message += "? (y/n)";
                        Console.WriteLine(message);

                        bool isValid;
                        string input;
                        do
                        {
                            input = Console.ReadLine()?.ToLower() ?? "";
                            isValid = input == "y" || input == "n";

                            if (!isValid)
                            {
                                Console.WriteLine("Invalid choice. Please enter y or n.");
                            }
                        } while (!isValid);

                        if (input == "y")
                        {
                            // Return both items that were equipped
                            unequipOne = MainHand;
                            unequipTwo = OffHand;
                            MainHand = item;
                            OffHand = null;
                            return;
                        }
                        else
                        {
                            unequipOne = item; // Return the new item back to inventory
                            return;
                        }
                    }
                }
            }
        }
    }
}