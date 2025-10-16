using System;
using Spectre.Console;

namespace TextRPGOne
{
    partial class Program
    {
        //TODO Work on Bugs during character creation that brick/dupicate locations the game.
        //TODO Add Some Basic Equipment/Consumables and Item Types
        //TODO Add Inventory to USE/EQUIP/TRASH Items
        //TODO Add new combat mecanics with new damage types
        //TODO Add Skills and skill levels
        partial class Game
        {
            private static Random random = new Random();
            private PlayerCharacter player;

            public Game(PlayerCharacter player)
            {
                this.player = player;
            }

            public void PlayGame()
            {
                Console.Clear();
                InitializeEnemies();
                InitializeLocations();

                while (player.Health > 0)
                {
                    Console.Clear();
                    player.CurrentLocation.DisplayImage(player);
                    InGameMenu();
                }

                Console.WriteLine("Game Over! Returning to main menu...");
            }
            private void EnemyEncounter()
            {
                // Check if there are any enemies in this location
                if (player.CurrentLocation.PossibleEnemies.Count == 0)
                {
                    FindItem();
                    return;
                }
                Console.Clear();
                int randomEnemy = Random.Shared.Next(player.CurrentLocation.PossibleEnemies.Count);
                NPC enemyTemplate = player.CurrentLocation.PossibleEnemies[randomEnemy];
                NPC currentEnemy = enemyTemplate.Clone();

                // Create combat log list
                List<string> combatLog = new List<string>();
                combatLog.Add($"You encounter a {currentEnemy.Name}!");

                AnsiConsole.Status()
                .Start($"You encounter a {currentEnemy.Name}!", ctx =>
                    {
                        Thread.Sleep(2000);
                    });

                // Clear any buffered input that accumulated during loading
                while (Console.KeyAvailable)
                {
                    Console.ReadKey(true);
                }

                // Roll initiative ONCE at the start of combat
                int playerInitRoll = random.Next(1, 21) + player.Initiative;
                int enemyInitRoll = random.Next(1, 21) + currentEnemy.Initiative;
                bool playerGoesFirst;

                // Handle ties INIT
                while (playerInitRoll == enemyInitRoll)
                {
                    playerInitRoll = random.Next(1, 21) + player.Initiative;
                    enemyInitRoll = random.Next(1, 21) + currentEnemy.Initiative;
                }

                playerGoesFirst = playerInitRoll > enemyInitRoll;

                while (player.Health > 0 && currentEnemy.Health > 0)
                {
                    void PlayersAttack()
                    {
                        Console.Clear();
                        DrawNPCEncounterBar(currentEnemy);
                        DrawPlayerEncounterBar(player);

                        // Display combat log before action
                        if (combatLog.Count > 0)
                        {
                            Console.WriteLine("\n[Combat Log]");
                            foreach (var log in combatLog)
                            {
                                AnsiConsole.MarkupLine(log);
                            }
                            Console.WriteLine();
                        }

                        bool moveCastValid = false;
                        do
                        {
                            string actionSelected = DrawPlayerActions(player);

                            // Extract the actual move name from the display format "[ManaCost] MoveName"
                            string playerMoveName = actionSelected;
                            if (actionSelected.StartsWith("[") && actionSelected.Contains("] "))
                            {
                                int endBracket = actionSelected.IndexOf("] ");
                                playerMoveName = actionSelected.Substring(endBracket + 2);
                            }

                            Move playerAction = player.MoveSet.First(m => m.Name == playerMoveName);
                            if (playerAction.ManaCost <= player.Mana)
                            {
                                AnsiConsole.Status()
                                    .Start($"{playerAction.Description}", ctx =>
                                    {
                                        AnsiConsole.MarkupLine($"{player.Name} is Attcking...");
                                        Thread.Sleep(2000);
                                    });

                                // Clear any buffered input that accumulated during animation
                                while (Console.KeyAvailable)
                                {
                                    Console.ReadKey(true);
                                }

                                player.Mana -= playerAction.ManaCost;
                                moveCastValid = true;

                                //Player Damage Calc
                                int baseDamage = (player.PrimaryStat + playerAction.Damage) / 2;
                                int playerDamage = baseDamage + Random.Shared.Next(-3, 4);
                                currentEnemy.Health -= playerDamage;

                                // Add to combat log
                                combatLog.Add($"{player.Name} uses {playerAction.Name}!");
                                combatLog.Add($"Dealt [red]{playerDamage}[/] damage!");

                                // Update display immediately after player attack
                                Console.Clear();
                                DrawNPCEncounterBar(currentEnemy);
                                DrawPlayerEncounterBar(player);

                                Console.WriteLine("\n[Combat Log]");
                                foreach (var log in combatLog)
                                {
                                    AnsiConsole.MarkupLine(log);
                                }
                                Console.WriteLine();

                                Thread.Sleep(1500);
                                break;
                            }
                            else
                            {
                                Console.WriteLine("You don't have enough mana");
                                Console.ReadKey();
                                continue;
                            }
                        } while (moveCastValid == true);
                    }

                    void EnemyAttack()
                    {
                        // Initial Display
                        Console.Clear();
                        DrawNPCEncounterBar(currentEnemy);
                        DrawPlayerEncounterBar(player);

                        // Display combat log before action
                        if (combatLog.Count > 0)
                        {
                            Console.WriteLine("\n[Combat Log]");
                            foreach (var log in combatLog)
                            {
                                AnsiConsole.MarkupLine(log);
                            }
                            Console.WriteLine();
                        }

                        int randomEnemyMove = random.Next(currentEnemy.MoveSet.Length);
                        Move npcAction = currentEnemy.MoveSet[randomEnemyMove];

                        AnsiConsole.Status()
                            .Start($"{npcAction.Description}", ctx =>
                            {
                                AnsiConsole.MarkupLine($"Enemy Attcking...");
                                Thread.Sleep(2000);
                            });

                        // Clear any buffered input that accumulated during animation
                        while (Console.KeyAvailable)
                        {
                            Console.ReadKey(true);
                        }

                        //Enemy Damage Calc
                        int enemyBaseDamage = (currentEnemy.PrimaryStat + npcAction.Damage) / 2;
                        int npcDamage = enemyBaseDamage + Random.Shared.Next(-2, 3);
                        player.Health -= npcDamage;

                        // Add to combat log
                        combatLog.Add($"{currentEnemy.Name} uses {npcAction.Name}!");
                        combatLog.Add($"You take [red]{npcDamage}[/] damage!");

                        // Update display immediately after enemy attack
                        Console.Clear();
                        DrawNPCEncounterBar(currentEnemy);
                        DrawPlayerEncounterBar(player);

                        Console.WriteLine("\n[Combat Log]");
                        foreach (var log in combatLog)
                        {
                            AnsiConsole.MarkupLine(log);
                        }
                        Console.WriteLine();

                        Thread.Sleep(1000);
                    }

                    // Execute attacks based on initiative (determined once at start)
                    if (playerGoesFirst)
                    {
                        if (player.Health > 0)
                        {
                            PlayersAttack();
                        }
                        if (currentEnemy.Health > 0)
                        {
                            EnemyAttack();
                        }
                    }
                    else
                    {
                        if (currentEnemy.Health > 0)
                        {
                            EnemyAttack();
                        }
                        if (player.Health > 0)
                        {
                            PlayersAttack();
                        }
                    }


                    // Check for combat end
                    if (currentEnemy.Health <= 0)
                    {
                        Console.Clear();
                        DrawNPCEncounterBar(currentEnemy);
                        DrawPlayerEncounterBar(player);

                        // Display final combat log
                        Console.WriteLine("\n[Combat Log]");
                        foreach (var log in combatLog)
                        {
                            AnsiConsole.MarkupLine(log);
                        }

                        AnsiConsole.MarkupLine($"\n[yellow]You defeated the {currentEnemy.Name}![/]");
                        AnsiConsole.MarkupLine($"[yellow]You found {currentEnemy.GoldDrop} gold![/]");

                        player.ExpAdd(currentEnemy.ExpDrop);

                        var droppedLoot = LootTableHandler.DropItems(currentEnemy.LootTable);
                        droppedLoot.ForEach(Item => Console.WriteLine(Item.Name));

                        player.Inventory.AddRange(droppedLoot);

                        Console.WriteLine();
                        Console.ReadKey();
                        break;
                    }
                    else if (player.Health <= 0)
                    {
                        Console.Clear();
                        DrawNPCEncounterBar(currentEnemy);
                        DrawPlayerEncounterBar(player);

                        Console.WriteLine("\n[Combat Log]");
                        foreach (var log in combatLog)
                        {
                            AnsiConsole.MarkupLine(log);
                        }

                        AnsiConsole.MarkupLine($"\n[red]You were defeated![/]");
                        Console.ReadKey();
                        break;
                    }
                }
            }
            private void FindItem()
            {
                player.Inventory.Add(ItemDatabase.Get(ItemName.Rock));
            }

            private void Explore()
            {
                int randomOneHundred = Random.Shared.Next(101);
                AnsiConsole.Status()
                    .Start("Exploring...", ctx =>
                    {
                        Thread.Sleep(2000);
                    });

                // Clear any buffered input that accumulated during animation
                while (Console.KeyAvailable)
                {
                    Console.ReadKey(true);
                }

                if (randomOneHundred <= 100)
                {
                    EnemyEncounter();
                }
                else { FindItem(); }
            }

            private void InGameMenu()
            {
                AnsiConsole.Write(new Columns(
                new Text("c. Character Stats"),
                new Text("i. Inventory"),
                new Text("t. Travel"),
                new Text("e. Explore")
                ));
                string playerChoice = Console.ReadLine() ?? "";

                while (playerChoice != "c" && playerChoice != "i" && playerChoice != "t" && playerChoice != "e")
                {
                    Console.Write($"{playerChoice} is not a valid option\n");
                    playerChoice = Console.ReadLine() ?? "";
                    continue;
                }
                switch (playerChoice)
                {
                    case "c":
                        DrawCharacterStats(player);
                        Console.ReadKey();
                        break;
                    case "i":
                        OpenInventory(player);
                        break;
                    case "t":
                        LocationOptions();
                        break;
                    case "e":
                        Explore();
                        break;
                    default:
                        // Invalid command already handled above
                        break;
                }
            }

            private void LocationOptions()
            {
                for (int i = 0; i < player.CurrentLocation.ConnectedLocations.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {player.CurrentLocation.ConnectedLocations[i].Name}");
                }

                int playerChoice;
                bool isValid;
                do
                {
                    string input = Console.ReadLine() ?? "";
                    bool isParsed = int.TryParse(input, out playerChoice);
                    bool isInRange = playerChoice >= 1 && playerChoice <= player.CurrentLocation.ConnectedLocations.Count;
                    isValid = isParsed && isInRange;

                    if (!isValid)
                    {
                        return;
                    }
                } while (!isValid);

                Console.Clear();
                player.CurrentLocation = player.CurrentLocation.ConnectedLocations[playerChoice - 1];
                player.CurrentLocation.DisplayImage(player);
            }
        }
    }
}
