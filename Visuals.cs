using Spectre.Console;
using Spectre.Console.Cli;

namespace TextRPGOne
{
    partial class Program
    {
        static void DrawStatusBar(PlayerCharacter player)
        {
            if (playerCharacter == null) return;

            // Create custom health bar
            int barWidth = 40;

            // Clamp health to 0 minimum
            int currentHealth = Math.Max(0, player.Health);
            int healthFilled = (int)((double)currentHealth / playerCharacter.MaxHealth * barWidth);
            healthFilled = Math.Max(0, Math.Min(healthFilled, barWidth));
            string healthBar = new string('█', healthFilled) + new string('░', barWidth - healthFilled);

            // Clamp mana to 0 minimum
            int currentMana = Math.Max(0, player.Mana);
            int manaFilled = (int)((double)currentMana / playerCharacter.MaxMana * barWidth);
            manaFilled = Math.Max(0, Math.Min(manaFilled, barWidth));
            string manaBar = new string('█', manaFilled) + new string('░', barWidth - manaFilled);

            var grid = new Grid()
                .AddColumn()
                .AddRow($"[yellow]{player.Name}\n[/]")
                .AddRow($"[green]Health:[/] [green]{healthBar}[/] [white]{currentHealth}/{playerCharacter.MaxHealth}[/]")
                .AddRow($"[blue]Mana:[/]   [blue]{manaBar}[/] [white]{currentMana}/{playerCharacter.MaxMana}[/]");

            AnsiConsole.Write(new Padder(grid).Padding(2, 1, 2, 1));
        }
        static void DrawCharacterStats()
        {
            if (playerCharacter == null) return;

            string statsText = $@"
[yellow]Class:[/]        [yellow]{playerCharacter.SpecName}[/]
[cyan]Strength:[/]     [yellow]{playerCharacter.Strength}[/]
[cyan]Dexterity:[/]    [yellow]{playerCharacter.Dexterity}[/]
[cyan]Intelligence:[/] [yellow]{playerCharacter.Intelligence}[/]
[cyan]Constitution:[/] [yellow]{playerCharacter.Constitution}[/]";

            var statsPanel = new Panel(statsText)
                .Header("[bold yellow] Stats [/]")
                .Border(BoxBorder.Rounded)
                .BorderColor(Color.Yellow)
                .Padding(3, 0, 3, 1);
            AnsiConsole.Write(statsPanel);
            Console.WriteLine("Press any key to exit");
        }
        static void DrawPlayerEncounterBar(PlayerCharacter player)
        {
            if (playerCharacter == null) return;

            // Create custom health bar
            int barWidth = 60;

            // Clamp health to 0 minimum
            int currentHealth = Math.Max(0, player.Health);
            int healthFilled = (int)((double)currentHealth / playerCharacter.MaxHealth * barWidth);
            healthFilled = Math.Max(0, Math.Min(healthFilled, barWidth));
            string healthBar = new string('█', healthFilled) + new string('░', barWidth - healthFilled);

            // Clamp mana to 0 minimum
            int currentMana = Math.Max(0, player.Mana);
            int manaFilled = (int)((double)currentMana / playerCharacter.MaxMana * barWidth);
            manaFilled = Math.Max(0, Math.Min(manaFilled, barWidth));
            string manaBar = new string('█', manaFilled) + new string('░', barWidth - manaFilled);

            var grid = new Grid()
                .AddColumn()
                .AddRow($"[yellow]{player.Name}\n[/]")
                .AddRow($"[green]Health:[/] [green]{healthBar}[/] [white]{currentHealth}/{playerCharacter.MaxHealth}[/]")
                .AddRow($"[blue]Mana:[/]   [blue]{manaBar}[/] [white]{currentMana}/{playerCharacter.MaxMana}[/]");

            AnsiConsole.Write(new Padder(grid).Padding(3, 1));
            Console.WriteLine();
        }
        static void DrawNPCEncounterBar(NPC npc)
        {
            // Create custom health bar
            int barWidth = 60;

            // Clamp health to 0 minimum
            int currentHealth = Math.Max(0, npc.Health);
            int healthFilled = (int)((double)currentHealth / npc.MaxHealth * barWidth);
            healthFilled = Math.Max(0, Math.Min(healthFilled, barWidth));
            string healthBar = new string('█', healthFilled) + new string('░', barWidth - healthFilled);

            // Clamp mana to 0 minimum
            int currentMana = Math.Max(0, npc.Mana);
            int manaFilled = (int)((double)currentMana / npc.MaxMana * barWidth);
            manaFilled = Math.Max(0, Math.Min(manaFilled, barWidth));
            string manaBar = new string('█', manaFilled) + new string('░', barWidth - manaFilled);

            var grid = new Grid()
                .AddColumn()
                .AddRow($"[red]{npc.Name}\n[/]")
                .AddRow($"[red]Health:[/] [red]{healthBar}[/] [white]{currentHealth}/{npc.MaxHealth}[/]")
                .AddRow($"[blue]Mana:[/]   [blue]{manaBar}[/] [white]{currentMana}/{npc.MaxMana}[/]");

            AnsiConsole.Write(new Padder(grid).Padding(3, 1));
            Console.WriteLine();
        }
        static string DrawPlayerActions(PlayerCharacter player)
        {
            var moveSetChoices = new List<string>();

            foreach (var move in player.MoveSet)
            {
                if (move.ManaCost != 0)
                {
                    // Escape square brackets to prevent Spectre.Console markup parsing
                    string displayName = $"[[{move.ManaCost}]] {move.Name}";
                    moveSetChoices.Add(displayName);
                }
                else
                {
                    moveSetChoices.Add(move.Name);
                }
            }


            var actions = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("It's your move...")
                .PageSize(4)
                .MoreChoicesText("[grey](Move up and down to reveal more actions)[/]")
                .AddChoices(moveSetChoices));
            return actions;
        }
    }
}