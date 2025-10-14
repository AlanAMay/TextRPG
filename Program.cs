using System;
using System.IO;
using Microsoft.VisualBasic;
using System.Diagnostics;
using Spectre.Console;
using Spectre.Console.Cli;

namespace TextRPGOne
{
    partial class Program
    {
        public static Random random = new Random();
        static void Main()
        {
            //Console.Clear();
            InitializeEnemies();
            MainMenuLoop();
        }
        public static bool enterGame = false;
        enum MainLoopOptions
        {
            PlayGame = 1,
            QuitGame = 2
        }
        static void EnemyEncounter()
        {
            if (playerCharacter == null || playerCharacter.CurrentLocation == null) return;

            // Check if there are any enemies in this location
            if (playerCharacter.CurrentLocation.PossibleEnemies.Count == 0)
            {
                FindItem();
                return;
            }
            Console.Clear();
            int randomEnemy = Random.Shared.Next(playerCharacter.CurrentLocation.PossibleEnemies.Count);
            NPC enemyTemplate = playerCharacter.CurrentLocation.PossibleEnemies[randomEnemy];
            NPC currentEnemy = enemyTemplate.Clone();

            // Create combat log list
            List<string> combatLog = new List<string>();
            combatLog.Add($"You encounter a {currentEnemy.Name}!");

            AnsiConsole.Status()
            .Start($"You encounter a {currentEnemy.Name}!", ctx =>
                {
                    Thread.Sleep(3500);
                });

            // Roll initiative ONCE at the start of combat
            int playerInitRoll = random.Next(1, 21) + playerCharacter.Initiative;
            int enemyInitRoll = random.Next(1, 21) + currentEnemy.Initiative;
            bool playerGoesFirst;

            // Handle ties INIT
            while (playerInitRoll == enemyInitRoll)
            {
                playerInitRoll = random.Next(1, 21) + playerCharacter.Initiative;
                enemyInitRoll = random.Next(1, 21) + currentEnemy.Initiative;
            }

            playerGoesFirst = playerInitRoll > enemyInitRoll;

            while (playerCharacter.Health > 0 && currentEnemy.Health > 0)
            {
                void PlayersAttack()
                {
                    if (playerCharacter == null) return;
                    Console.Clear();
                    DrawNPCEncounterBar(currentEnemy);
                    DrawPlayerEncounterBar(playerCharacter);

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
                        string actionSelected = DrawPlayerActions(playerCharacter);

                        // Extract the actual move name from the display format "[ManaCost] MoveName"
                        string playerMoveName = actionSelected;
                        if (actionSelected.StartsWith("[") && actionSelected.Contains("] "))
                        {
                            int endBracket = actionSelected.IndexOf("] ");
                            playerMoveName = actionSelected.Substring(endBracket + 2);
                        }

                        Move playerAction = playerCharacter.MoveSet.First(m => m.Name == playerMoveName);
                        if (playerAction.ManaCost <= playerCharacter.Mana)
                        {
                            AnsiConsole.Status()
                                .Start($"{playerAction.Description}", ctx =>
                                {
                                    AnsiConsole.MarkupLine($"{playerCharacter.Name} is Attcking...");
                                    Thread.Sleep(3500);
                                });
                            playerCharacter.Mana -= playerAction.ManaCost;
                            moveCastValid = true;

                            //Player Damage Calc
                            int baseDamage = (playerCharacter.PrimaryStat + playerAction.Damage) / 2;
                            int playerDamage = baseDamage + Random.Shared.Next(-3, 4);
                            currentEnemy.Health -= playerDamage;

                            // Add to combat log
                            combatLog.Add($"{playerCharacter.Name} uses {playerAction.Name}!");
                            combatLog.Add($"Dealt [red]{playerDamage}[/] damage!");

                            // Update display immediately after player attack
                            Console.Clear();
                            DrawNPCEncounterBar(currentEnemy);
                            DrawPlayerEncounterBar(playerCharacter);

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
                    // Pick Enemy
                    if (playerCharacter == null) return;
                    // Initial Display
                    Console.Clear();
                    DrawNPCEncounterBar(currentEnemy);
                    DrawPlayerEncounterBar(playerCharacter);

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
                            Thread.Sleep(3500);
                        });

                    //Enemy Damage Calc
                    int enemyBaseDamage = (currentEnemy.PrimaryStat + npcAction.Damage) / 2;
                    int npcDamage = enemyBaseDamage + Random.Shared.Next(-2, 3);
                    playerCharacter.Health -= npcDamage;

                    // Add to combat log
                    combatLog.Add($"{currentEnemy.Name} uses {npcAction.Name}!");
                    combatLog.Add($"You take [red]{npcDamage}[/] damage!");

                    // Update display immediately after enemy attack
                    Console.Clear();
                    DrawNPCEncounterBar(currentEnemy);
                    DrawPlayerEncounterBar(playerCharacter);

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
                    if (playerCharacter.Health > 0)
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
                    if (playerCharacter.Health > 0)
                    {
                        PlayersAttack();
                    }
                }


                // Check for combat end
                if (currentEnemy.Health <= 0)
                {
                    Console.Clear();
                    DrawNPCEncounterBar(currentEnemy);
                    DrawPlayerEncounterBar(playerCharacter);

                    // Display final combat log
                    Console.WriteLine("\n[Combat Log]");
                    foreach (var log in combatLog)
                    {
                        AnsiConsole.MarkupLine(log);
                    }

                    //TODO Add enemy item drops /  Store instances of items in Character Invetory
                    AnsiConsole.MarkupLine($"\n[yellow]You defeated the {currentEnemy.Name}![/]");
                    AnsiConsole.MarkupLine($"[yellow]You found {currentEnemy.GoldDrop} gold![/]");


                    Console.WriteLine();
                    Console.ReadKey();
                    break;
                }
                else if (playerCharacter.Health <= 0)
                {
                    Console.Clear();
                    DrawNPCEncounterBar(currentEnemy);
                    DrawPlayerEncounterBar(playerCharacter);

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
        static void FindItem()
        {
            Console.ReadLine();
        }

        static void Explore()
        {
            int randomOneHundred = Random.Shared.Next(101);
            AnsiConsole.Status()
                .Start("Exploring...", ctx =>
                {
                    Thread.Sleep(3500);
                });
            if (randomOneHundred <= 100)
            {
                EnemyEncounter();
            }
            else { FindItem(); }
        }

        static void InGameMenu()
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
                    DrawCharacterStats();
                    Console.ReadKey();
                    break;
                case "i":
                    OpenInventory();
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
        static MainLoopOptions? ShowMenu()
        {
            Console.WriteLine("");
            Console.WriteLine("╔════════════════════════════════╗");
            Console.WriteLine("║     Welcome to TextRPG!        ║");
            Console.WriteLine("╚════════════════════════════════╝\n");
            Console.WriteLine("1. Play Game");
            Console.WriteLine("2. Quit Game");
            var playerChoice = int.Parse(Console.ReadLine() ?? "");

            if (playerChoice >= 1 && playerChoice <= 2)
            {
                MainLoopOptions option = (MainLoopOptions)playerChoice;
                Console.Clear();
                return option;
            }
            else
            {
                Console.WriteLine("Invalid Choice");
                return null;
            }
        }
        static void MainMenuLoop()
        {
            bool running = true;
            while (running)
            {
                var choice = ShowMenu();
                switch (choice)
                {
                    case MainLoopOptions.PlayGame:
                        CreateCharacter();  // Create character first
                        if (playerCharacter != null)  // Only play if character was created successfully
                        {
                            PlayGame(playerCharacter);
                        }
                        break;
                    case MainLoopOptions.QuitGame:
                        running = false;
                        break;
                }
            }
        }

        static void CreateCharacter()
        {
            do
            {
                Console.Clear();
                Console.WriteLine("Enter a Username");
                string userNameInput = Console.ReadLine() ?? "";

                while (userNameInput == "" || userNameInput.Length < 3)
                {
                    Console.WriteLine("Invalid Username (must be more than 2 characters)");
                    userNameInput = Console.ReadLine() ?? "";
                }

                Console.WriteLine("\nWhat Class would you like to play?\n");
                Console.WriteLine("1. Play the Warrior");
                Console.WriteLine(Warrior.Description);
                Console.WriteLine($"Health: {Warrior.Health}, Mana: {Warrior.Mana}\n");
                Console.WriteLine("2. Play the Rogue");
                Console.WriteLine(Rogue.Description);
                Console.WriteLine($"Health: {Rogue.Health}, Mana: {Rogue.Mana}\n");
                Console.WriteLine("3. Play the Mage");
                Console.WriteLine(Mage.Description);
                Console.WriteLine($"Health: {Mage.Health}, Mana: {Mage.Mana}\n");

                int playerChoice;
                bool isValid;
                do
                {
                    string input = Console.ReadLine() ?? "";
                    bool isParsed = int.TryParse(input, out playerChoice);
                    bool isInRange = playerChoice >= 1 && playerChoice <= 3;
                    isValid = isParsed && isInRange;

                    if (!isValid)
                    {
                        return;
                    }
                } while (!isValid);

                switch (playerChoice)
                {
                    case 1:
                        // TODO: Move primary stat into Spec and pass whole class spec as argument.
                        playerCharacter = new PlayerCharacter(userNameInput, Warrior.Name, PrimaryStatType.Strength, Warrior.Strength, Warrior.Dexterity, Warrior.Intelligence, Warrior.Constitution, Warrior.MoveSet, CrossRoad);
                        break;
                    case 2:
                        playerCharacter = new PlayerCharacter(userNameInput, Rogue.Name, PrimaryStatType.Dexterity, Rogue.Strength, Rogue.Dexterity, Rogue.Intelligence, Rogue.Constitution, Rogue.MoveSet, CrossRoad);
                        break;
                    case 3:
                        playerCharacter = new PlayerCharacter(userNameInput, Mage.Name, PrimaryStatType.Intelligence, Mage.Strength, Mage.Dexterity, Mage.Intelligence, Mage.Constitution, Mage.MoveSet, CrossRoad);
                        break;
                    default:
                        Console.WriteLine("Invalid Option, returning to main menu");
                        playerCharacter = null;
                        return;
                }
            } while (playerCharacter == null);
        }

        static void PlayGame(PlayerCharacter player)
        {
            Console.Clear();
            InitializeLocations();

            while (player.Health > 0)
            {
                Console.Clear();
                player.CurrentLocation.DisplayImage();
                InGameMenu();
            }

            Console.WriteLine("Game Over! Returning to main menu...");
        }
    }
}