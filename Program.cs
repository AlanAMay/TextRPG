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
        static void Main()
        {
            //Console.Clear();
            MainMenuLoop();
        }
        public static bool enterGame = false;
        enum MainLoopOptions
        {
            PlayGame = 1,
            QuitGame = 2
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
                            var game = new Game(playerCharacter);
                            game.PlayGame();
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
                        playerCharacter = new PlayerCharacter(userNameInput, Warrior, CrossRoad);
                        break;
                    case 2:
                        playerCharacter = new PlayerCharacter(userNameInput, Rogue, CrossRoad);
                        break;
                    case 3:
                        playerCharacter = new PlayerCharacter(userNameInput, Mage, CrossRoad);
                        break;
                    default:
                        Console.WriteLine("Invalid Option, returning to main menu");
                        playerCharacter = null;
                        return;
                }
            } while (playerCharacter == null);
        }
    }
}