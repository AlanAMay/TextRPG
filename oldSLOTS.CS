// namespace TextRPGOne
// {
//     partial class Program
//     {
//         class Slots
//         {
//             private Item _helmet;
//             private Item _shoulders;
//             private Item _chest;
//             private Item _waist;
//             private Item _legs;
//             private Item _boots;
//             private Item _necklace;
//             private Item _ringOne;
//             private Item _ringTwo;
//             private Item _mainHand;
//             private Item _offHand;
//             public Item Helmet;
//             public Item Shoulders;
//             public Item Chest;
//             public Item Waist;
//             public Item Legs;
//             public Item Boots;
//             public Item Necklace;
//             public Item RingOne;
//             public Item RingTwo;
//             public Item MainHand;
//             public Item OffHand;
//             public Item Equip(Item item)
//             {
//                 switch (item.EquipmentType)
//                 {
//                     case EquipmentType.Helmet:
//                         return SlotHandler(item, Helmet);
//                     case EquipmentType.Shoulders:
//                         return SlotHandler(item, Shoulders);
//                     case EquipmentType.Chest:
//                         return SlotHandler(item, Chest);
//                     case EquipmentType.Waist:
//                         return SlotHandler(item, Waist);
//                     case EquipmentType.Legs:
//                         return SlotHandler(item, Legs);
//                     case EquipmentType.Boots:
//                         return SlotHandler(item, Boots);
//                     case EquipmentType.Necklace:
//                         return SlotHandler(item, Necklace);
//                     case EquipmentType.Ring:
//                         return RingSlotHandler(item, RingOne, RingTwo);
//                     case EquipmentType.OneHandWeapon:
//                         return WeapsonSlotHandler(item, MainHand, OffHand, out unequipOne, out unequipTwo);
//                     case EquipmentType.TwoHandWeapon:
//                         return WeapsonSlotHandler(item, MainHand, OffHand, out unequipOne, out unequipTwo);
//                 }

//             }
//             private Item SlotHandler(Item item, Item slot)
//             {
//                 if (this.slot == null)
//                 {
//                     slot = item;
//                     return null;
//                 }
//                 else
//                 {
//                     Console.WriteLine($"Unequip {slot}\n{slot.Stats}");
//                     Console.WriteLine("y/n");
//                     if (Console.ReadKey("y"))
//                     {
//                         Item unequip = slot;
//                         slot = item;
//                         return unequip;
//                     }
//                     else return item;
//                 }
//             }
//             private Item RingSlotHandler(Item item, Item slotOne, Item slotTwo)
//             {
//                 if (this.slotOne == null)
//                 {
//                     slotOne = item;
//                     return null;
//                 }
//                 else if (this.slotTwo == null)
//                 {
//                     slotTwo = item;
//                     return null;
//                 }
//                 else
//                 {
//                     Console.WriteLine($"Which ring would you like to replace?");
//                     Console.WriteLine($"1. {slotOne.Name}");
//                     Console.WriteLine($"2. {slotTwo.Name}");

//                     int playerChoice = Console.ReadLine();
//                     bool isValid;
//                     do
//                     {
//                         string input = Console.ReadLine() ?? "";
//                         bool isParsed = int.TryParse(input, out playerChoice);
//                         bool isInRange = playerChoice >= 1 && playerChoice <= 3;
//                         isValid = isParsed && isInRange;

//                         if (!isValid)
//                         {
//                             return;
//                         }
//                     } while (!isValid);

//                     switch (playerChoice)
//                     {
//                         case 1:
//                             Item unequip = slotOne;
//                             slotOne = item;
//                             return unequip;
//                             break;
//                         case 2:
//                             Item unequip = slotTwo;
//                             slotTwo = item;
//                             return unequip;
//                             break;
//                         default:
//                             Console.WriteLine("Invalid Option, returning");
//                             return;
//                     }
//                 }
//             }
//             private Item WeapsonSlotHandler(Item item, Item mainHand, Item offHand)
//             {
//                 if (item.EquipmentType == EquipmentType.OneHandWeapon)
//                 {
//                     if (mainHand == null)
//                     {
//                         mainHand = item;
//                         return null;
//                     }
//                     else if (offHand == null)
//                     {
//                         offHand = item;
//                         return null;
//                     }
//                     else
//                     {
//                         Console.WriteLine($"Which weapon would you like to replace?");
//                         Console.WriteLine($"1. {slotOne.Name}");
//                         Console.WriteLine($"2. {slotTwo.Name}");

//                         int playerChoice = Console.ReadLine();
//                         bool isValid;
//                         do
//                         {
//                             string input = Console.ReadLine() ?? "";
//                             bool isParsed = int.TryParse(input, out playerChoice);
//                             bool isInRange = playerChoice >= 1 && playerChoice <= 3;
//                             isValid = isParsed && isInRange;

//                             if (!isValid)
//                             {
//                                 return;
//                             }
//                         } while (!isValid);

//                         switch (playerChoice)
//                         {
//                             case 1:
//                                 Item unequip = slotOne;
//                                 slotOne = item;
//                                 return unequip;
//                                 break;
//                             case 2:
//                                 Item unequip = slotTwo;
//                                 slotTwo = item;
//                                 return unequip;
//                                 break;
//                             default:
//                                 Console.WriteLine("Invalid Option, returning");
//                                 return;
//                         }
//                     }
//                 }
//                 if (item.EquipmentType == EquipmentType.TwoHandWeapon)
//                 {
//                     if (mainHand && OffHand == null)
//                     {
//                         mainHand = item;
//                         return null;
//                     }
//                     else if (mainHand && OffHand != null)
//                     {
//                         string message = Console.WriteLine($"Would you like to unequip {slotOne.Name}");
//                         if (OffHand != null)
//                         {
//                             message += $" and {slotTwo.Name}?";
//                         }
//                         else message += "?";
//                         Console.WriteLine("y/n");

//                         int playerChoice = Console.ReadLine();
//                         bool isValid;
//                         do
//                         {
//                             string input = Console.ReadLine() ?? "";
//                             bool isParsed = int.TryParse(input, out playerChoice);
//                             bool isInRange = playerChoice >= 1 && playerChoice <= 3;
//                             isValid = isParsed && isInRange;

//                             if (!isValid)
//                             {
//                                 return;
//                             }
//                         } while (!isValid);

//                         switch (playerChoice)
//                         {
//                             case 1:
//                                 Item unequip = slotOne;
//                                 slotOne = item;
//                                 return unequip;
//                                 break;
//                             case 2:
//                                 Item unequip = slotTwo;
//                                 slotTwo = item;
//                                 return unequip;
//                                 break;
//                             default:
//                                 Console.WriteLine("Invalid Option, returning");
//                                 return;
//                         }
//                     }
//                 }
//             }
//         }
//     }
// }