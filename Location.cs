using System.Diagnostics.CodeAnalysis;
using Spectre.Console;
using Spectre.Console.Cli;

namespace TextRPGOne
{
    partial class Program
    {
        class Location
        {
            private string _name;
            private string _description;
            private List<Location> _connectedLocations = new List<Location>();
            private List<NPC> _possibleEnemies = new List<NPC>();
            private bool _isSafeZone;
            private string _imagePath;

            public string Name { get => _name; [MemberNotNull(nameof(_name))] set => _name = value; }
            public string Description { get => _description; [MemberNotNull(nameof(_description))] set => _description = value; }
            public List<Location> ConnectedLocations { get => _connectedLocations; [MemberNotNull(nameof(_connectedLocations))] set => _connectedLocations = value; }
            public List<NPC> PossibleEnemies { get => _possibleEnemies; [MemberNotNull(nameof(_possibleEnemies))] set => _possibleEnemies = value; }
            public bool IsSafeZone { get => _isSafeZone; set => _isSafeZone = value; }
            public string ImagePath { get => _imagePath; [MemberNotNull(nameof(_imagePath))] set => _imagePath = value; }
            public void DisplayImage()
            {
                if (playerCharacter == null) return;
                if (ImagePath == "") { return; }
                var rule = new Rule(Name);
                rule.Style = Style.Parse("grey");
                var bottomRule = new Rule();
                bottomRule.Style = Style.Parse("grey");
                AnsiConsole.Write(rule);
                ImageHandler.DisplayAsciiLocation(ImagePath);

                var rows = new List<Text>()
                {
                new Text(Description, new Style(Color.Grey, Color.Black))
                };

                // Renders each item with own style
                AnsiConsole.Write(new Rows(rows));
                AnsiConsole.Write(bottomRule);
                DrawStatusBar(playerCharacter);
                Console.WriteLine();
            }
            public Location(string Name, string Description, string ImagePath, bool IsSafeZone = false)
            {
                this.Name = Name;
                this.Description = Description;
                this.ImagePath = ImagePath;
                this.IsSafeZone = IsSafeZone;
            }
        }
        static Location CrossRoad = new Location(
            "Cross Roads",
            "A dusty intersection where several paths meet. A weathered signpost points toward the town.",
            "images/crossroads.jpg --threshold 65",
            true
        );
        static Location Town = new Location(
            "Town",
            "A cozy village surrounded by wooden fences. Shops line the streets and townsfolk go about their day with warm smiles.",
            "images/town.jpg --threshold 75",
            true
        );

        static Location NorthForest = new Location(
            "Northern Forest",
            "A cool, quiet forest where tall pines stretch toward the sky. The air smells of sap and earth.",
            "images/northforest.jpg --threshold 60"
        );
        static Location SouthForest = new Location(
            "Southern Forest",
            "A warm forest filled with sunlight and birdsong. The underbrush is thick, making travel slow.",
            "images/southforest.jpeg --threshold 75"
        );
        static Location DarkForest = new Location(
            "Dark Forest",
            "An eerie part of the woods where light struggles to reach the ground. Every sound feels too close.",
            "images/darkforest.jpg --threshold 75"
        );
        static Location WitchesHut = new Location(
            "Witche's Hut",
            "A small crooked hut hidden among twisted trees. Strange trinkets hang from the porch.",
            "images/witchhut.jpeg --threshold 85"
        );
        static Location OldRoad = new Location(
            "Old Road",
            "A worn cobblestone path winding through the forest. It looks like few travelers use it anymore.",
            "images/oldroad.png --threshold 100"
        );
        static Location Mountains = new Location(
            "Mountains",
            "Steep rocky slopes rise above the forest. The wind is cold and the air smells faintly of snow.",
            "images/mountains.jpg --threshold 85"
        );

        static void InitializeLocations()
        {
            CrossRoad.ConnectedLocations.Add(Town);
            CrossRoad.ConnectedLocations.Add(DarkForest);
            Town.ConnectedLocations.Add(CrossRoad);
            Town.ConnectedLocations.Add(NorthForest);
            Town.ConnectedLocations.Add(SouthForest);
            Town.ConnectedLocations.Add(OldRoad);
            DarkForest.ConnectedLocations.Add(CrossRoad);
            NorthForest.ConnectedLocations.Add(Town);
            SouthForest.ConnectedLocations.Add(Town);
            SouthForest.ConnectedLocations.Add(WitchesHut);
            WitchesHut.ConnectedLocations.Add(SouthForest);
            OldRoad.ConnectedLocations.Add(Town);
            OldRoad.ConnectedLocations.Add(Mountains);
            Mountains.ConnectedLocations.Add(OldRoad);

        }
        static void InitializeEnemies()
        {
            DarkForest.PossibleEnemies.Add(Goblin);
            DarkForest.PossibleEnemies.Add(Wolf);
        }
        static void LocationOptions()
        {
            if (playerCharacter == null) return;

            for (int i = 0; i < playerCharacter.CurrentLocation.ConnectedLocations.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {playerCharacter.CurrentLocation.ConnectedLocations[i].Name}");
            }

            int playerChoice;
            bool isValid;
            do
            {
                string input = Console.ReadLine() ?? "";
                bool isParsed = int.TryParse(input, out playerChoice);
                bool isInRange = playerChoice >= 1 && playerChoice <= playerCharacter.CurrentLocation.ConnectedLocations.Count;
                isValid = isParsed && isInRange;

                if (!isValid)
                {
                    return;
                }
            } while (!isValid);

            Console.Clear();
            playerCharacter.CurrentLocation = playerCharacter.CurrentLocation.ConnectedLocations[playerChoice - 1];
            playerCharacter.CurrentLocation.DisplayImage();
        }
    }
}