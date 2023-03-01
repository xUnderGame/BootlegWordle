using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Linq;
using System.IO;

namespace WordleProgram
{
    public class Wordle
    {
        // --------------- Game/Language classes and "global" variables. --------------
        public static langJson lang = JsonSerializer.Deserialize<langJson>(File.OpenText("config/lang.json").ReadToEnd());
        public static string currLang = "en";
        public static string logo = $@"
   `8.`888b                 ,8'  ,o888888o.     8 888888888o.   8 888888888o.      8 8888         8 8888888888  ;
    `8.`888b               ,8'. 8888     `88.   8 8888    `88.  8 8888    `^888.   8 8888         8 8888        ;
     `8.`888b             ,8',8 8888       `8b  8 8888     `88  8 8888        `88. 8 8888         8 8888        ;
      `8.`888b     .b    ,8' 88 8888        `8b 8 8888     ,88  8 8888         `88 8 8888         8 8888        ;
       `8.`888b    88b  ,8'  88 8888         88 8 8888.   ,88'  8 8888          88 8 8888         8 888888888888;
        `8.`888b .`888b,8'   88 8888         88 8 888888888P'   8 8888          88 8 8888         8 8888        ;
         `8.`888b8.`8888'    88 8888        ,8P 8 8888`8b       8 8888         ,88 8 8888         8 8888        ;
          `8.`888`8.`88'     `8 8888       ,8P  8 8888 `8b.     8 8888        ,88' 8 8888         8 8888        ;
           `8.`8' `8,`'       ` 8888     ,88'   8 8888   `8b.   8 8888    ,o88P'   8 8888         8 8888        ;
            `8.`   `8'           `8888888P'     8 8888     `88. 8 888888888P'      8 888888888888 8 888888888888";

        // English language.
        public class en
        {
            public string menuOption1 { get; set; }
            public string menuOption2 { get; set; }
            public string menuOption3 { get; set; }
            public string menuOption4 { get; set; }
            public string gameend { get; set; }
            public string gameover { get; set; }
            public string gamewin { get; set; }
            public string gameword { get; set; }
            public string gamelives { get; set; }
            public string gamename { get; set; }
            public string gametries { get; set; }
            public string thankyou { get; set; }
            public string confirm { get; set; }
            public string historytitle { get; set; }
            public string gameload { get; set; }
            public string historyname { get; set; }
            public string historyattempts { get; set; }
            public string historytime { get; set; }
        }

        // Spanish language.
        public class es
        {
            public string menuOption1 { get; set; }
            public string menuOption2 { get; set; }
            public string menuOption3 { get; set; }
            public string menuOption4 { get; set; }
            public string gameend { get; set; }
            public string gameover { get; set; }
            public string gamewin { get; set; }
            public string gameword { get; set; }
            public string gamelives { get; set; }
            public string gamename { get; set; }
            public string gametries { get; set; }
            public string thankyou { get; set; }
            public string confirm { get; set; }
            public string historytitle { get; set; }
            public string gameload { get; set; }
            public string historyname { get; set; }
            public string historyattempts { get; set; }
            public string historytime { get; set; }
        }

        // Root language, how a language json file should be read.
        public class langJson
        {
            public en en { get; set; }
            public es es { get; set; }
        }

        // The guesses "root" of gameJson.
        public class Guesses
        {
            public object guess1 { get; set; }
            public object guess2 { get; set; }
            public object guess3 { get; set; }
            public object guess4 { get; set; }
            public object guess5 { get; set; }
            public object guess6 { get; set; }
        }

        // How a game json file should be serialized/deserialized.
        public class gameJson
        {
            public string name { get; set; }
            public List<string> guess { get; set; }
            public string wordle { get; set; }
            public int tries { get; set; }
            public Guesses guesses { get; set; }
            public long time { get; set; }
            public int guessindex { get; set; }
        }


        // --------------- Functions ---------------
        // Gets the input from the user and returns it. (Requires user input, untestable)
        public static string GetInput(string word = "", bool isCentered = false, string defaultParam = null, bool isMenu = false, bool askInput = true)
        {
            // Send a message for every split.
            StringToList(word).ForEach(send => SendMessage(send, default, isCentered, default, ConsoleColor.Yellow));
            if (askInput) SendMessage("INPUT: ", null, isCentered, ConsoleColor.Black, ConsoleColor.Cyan);

            // Sends the bottom part of the menu and sets the cursor one line above for the menu to make sense.
            if (isMenu)
            {
                SendMessage("╚════════════════════════════════════════════════════════════════════════════════════════╝");
                Console.SetCursorPosition((Console.WindowWidth + 5) / 2, Console.CursorTop - 1);
            }

            // Get the user input and return it.
            if (!askInput) return defaultParam;
            string userString = Console.ReadLine();
            if (userString == String.Empty) userString = defaultParam;
            return userString;
        }

        // Sends a message with color and optionally centered.
        public static void SendMessage(string message, string escapeChar = null, bool isCentered = true, ConsoleColor bgColor = ConsoleColor.Black, ConsoleColor textColor = ConsoleColor.White, int shiftTop = 1, int shiftX = 0)
        {
            // Checks before sending.
            if (isCentered) Console.SetCursorPosition(((Console.WindowWidth - message.Length) / 2) + shiftX, Console.CursorTop + shiftTop);
            if (bgColor != ConsoleColor.Black) Console.BackgroundColor = bgColor;
            if (textColor != ConsoleColor.White) Console.ForegroundColor = textColor;

            // Send message and reset the colors.
            Console.Write(message + escapeChar);
            Console.ResetColor();
        }

        // Splits a string into a list by using an input as a separator.
        public static List<string> StringToList(string toSplit, string splitBy = ";")
        {
            return toSplit.Split(splitBy).ToList();
        }

        // Changes the user's language. Available languages: "en" and "es".
        public static void ChangeLanguage()
        {
            if (currLang == "en")
            {
                currLang = "es";
                return;
            }
            currLang = "en";
        }

        // Gets the text inside the json file (Used to generate the language text).
        public static string GetText(string option)
        {
            // Don't question what this does. It just works.
            return (string)lang.GetType().GetProperty(currLang).GetValue(lang, null).GetType().GetProperty(option).GetValue(lang.GetType().GetProperty(currLang).GetValue(lang, null), null);
        }

        // Generates a game. You can also load in a game. (Requires user input, untestable)
        public static gameJson GenerateGame(){
            string playerName = new string(GetInput(GetText("gamename"), true, "unnamed").Where(ch => char.IsLetterOrDigit(ch)).ToArray());
            if (playerName == string.Empty || playerName.Length > 15) playerName = "breaker";
            // int tries = int.Parse(GetInput(GetText("gametries"), true)); // (Won't use this, as it breaks the entire game atm.)
            int tries = 6;

            // Check if a game with that name already exists.
            foreach (var file in Directory.GetFiles(@"games"))
            {
                if (playerName == file.Replace(@"games\", "").Replace(".json", ""))
                {
                    if (!GetInput(GetText("gameload"), true, "n").ToLower().Contains("y")) break;
                    string rawJson = File.ReadAllText(file);
                    var deserialized = JsonSerializer.Deserialize<gameJson>(rawJson);
                    deserialized.guess = new List<string>() { "-", "-", "-", "-", "-" };
                    return deserialized;
                }
            }

            // Read the word files and "make" a game.
            string word;
            if (currLang == "en") word = File.ReadAllText($@"config\ENwordList.txt");
            else word = File.ReadAllText($@"config\ESwordList.txt");
            word = word.Split("\n")[new Random().Next(word.Split("\n").Length)];

            // "Save" (more like create) the game.
            var newGame = new gameJson
            {
                name = playerName,
                guess = new List<string> { "-", "-", "-", "-", "-" },
                wordle = word,
                tries = tries,
                guesses = new Guesses {
                    guess1 = "-----",
                    guess2 = "-----",
                    guess3 = "-----",
                    guess4 = "-----",
                    guess5 = "-----",
                    guess6 = "-----"
                },
                time = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                guessindex = 1
            };
            SaveGame(newGame);
            return newGame;
        }

        // Saves the game.
        public static void SaveGame(gameJson game)
        {
            File.WriteAllText($@"games\{game.name}.json", JsonSerializer.Serialize(game));
        }

        // Colors the characters for every guess.
        public static void GetColors(gameJson game, object data)
        {
            Console.WriteLine();
            string guess = data.ToString();
            for (int i = 0; i < guess.Length; i++)
            {
                if (Convert.ToString(game.wordle)[i] == guess[i]) SendMessage(Convert.ToString(guess[i]), default, true, ConsoleColor.Green, ConsoleColor.Black, 0, i - 2);
                else if (Convert.ToString(game.wordle).Contains(guess[i])) SendMessage(Convert.ToString(guess[i]), default, true, ConsoleColor.Yellow, ConsoleColor.Black, 0, i - 2);
                else SendMessage(Convert.ToString(guess[i]), default, true, ConsoleColor.Gray, ConsoleColor.Black, 0, i - 2);
            }
        }

        // Builds the game menu.
        public static void BuildGameMenu(gameJson game)
        {
            // Game menu.
            Console.Clear(); // We clear every time the menu loads.
            Console.CursorVisible = false;
            SendMessage("╔════════════════════════════════════════════════════════════════════════════════════════╗", default, true, default, ConsoleColor.White);
            GetColors(game, game.guesses.guess1);
            GetColors(game, game.guesses.guess2);
            GetColors(game, game.guesses.guess3);
            GetColors(game, game.guesses.guess4);
            GetColors(game, game.guesses.guess5);
            GetColors(game, game.guesses.guess6);
            GetInput(string.Concat(game.guess.ToArray()), true, "0", true, false);
        }

        // Called when ending a game.
        public static (gameJson, bool) EndGame(gameJson game, bool v, bool addExtra = true)
        {
            if (addExtra)
            {
                SaveGame(game);
                Console.WriteLine("\n");
            }
            return (game, v);
        }

        // Called when the player submits a guess.
        public static void SubmitGuess(gameJson game, int guessIndex)
        {
            // Adds the guess to the game json file.
            var gamePointer = game.guesses.GetType().GetProperty("guess" + guessIndex);
            gamePointer.SetValue(game.guesses, string.Concat(game.guess.ToArray()));
            game.tries = Convert.ToInt32(game.tries) - 1;
            SaveGame(game);
        }

        // View the game history. You can use this to cheat if you quit with "escape" and re-load a save, or just check the wordle here.
        public static void GameHistory()
        {
            Console.WriteLine();
            SendMessage(GetText("historytitle"), "\n", true, default, ConsoleColor.Blue);
            foreach (var file in Directory.GetFiles(@"games"))
            {
                string rawJson = File.ReadAllText(file);
                gameJson loaded = JsonSerializer.Deserialize<gameJson>(rawJson);
                Console.WriteLine($"{GetText("historyname")} {loaded.name}\t{GetText("historyattempts")} {loaded.tries}\tWordle: {loaded.wordle}\t{GetText("historytime")} {loaded.time} (UNIX)");
            }
        }


        // --------------- Menus ---------------
        // Code runs from here.
        static void Main(string[] args)
        {
            Environment.CurrentDirectory = @"..\..\..\..\";
            Console.Title = "C#'s Bootleg Wordle - UnderGame, v1.1.0";
            Console.WindowWidth = 120;
            MainMenu();

            // Program ended!
            StringToList(logo).ForEach(send => SendMessage(send, default, false, default, ConsoleColor.Green, default));
            SendMessage("|---------------------------------------------------------------------------------------|");
            SendMessage($"{GetText("thankyou")}\n");
            Console.ReadKey();
        }

        // The game's main menu, you'll be here a while.
        public static void MainMenu()
        {
            Console.CursorVisible = true;
            string action;
            do
            {
                // Get the user's input.
                StringToList(logo).ForEach(send => SendMessage(send, default, false, default, ConsoleColor.Green, default));
                Console.Write($" ({currLang})");
                SendMessage("╔════════════════════════════════════════════════════════════════════════════════════════╗", default, true, default, ConsoleColor.White);
                action = GetInput($"{GetText("menuOption1")};{GetText("menuOption2")};{GetText("menuOption3")};{GetText("menuOption4")}", true, "0", true);
                
                // Do whatever the user wants to do.
                switch (action)
                {
                    // Play the game.
                    case "1":
                        var gameResults = MainGame();
                        // Send the game results.
                        if (gameResults.Item2) SendMessage($"{GetText("gamewin")}");
                        else SendMessage(GetText("gameover"), default, true, default, ConsoleColor.Red);
                        SendMessage(GetText("gameword").Replace("{}", Convert.ToString(gameResults.Item1.wordle)));
                        Console.ReadKey();
                        break;

                    // Change the game language.
                    case "2":
                        ChangeLanguage();
                        break;

                    // View match history.
                    case "3":
                        GameHistory();
                        Console.ReadKey();
                        break;
                }
                Console.Clear();
            } while (!"ENDEXITLEAVE4".Contains(action.ToUpper()));
        }

        // The game. Where the player will be most of the time.
        public static (gameJson, bool) MainGame()
        {
            gameJson game = GenerateGame();
            if (game.guessindex != 1) game.guessindex++; // Needed for some reason...?

            // Lock the player in until they game over or guess the correct word.
            do
            {
                // Guess the word loop
                ConsoleKeyInfo keyPress;
                int wordIndex = 0;
                do
                {
                    // Build the game menu && show the user it's last guess before losing.
                    BuildGameMenu(game);
                    if (Convert.ToInt32(game.tries) == 0) return EndGame(game, false);
                    Console.ForegroundColor = ConsoleColor.Black;
                    keyPress = Console.ReadKey();
                    Console.ResetColor();

                    // Key adding/removing/submitting.
                    switch (keyPress.Key)
                    {
                        // Remove a letter.
                        case ConsoleKey.Backspace:
                            if (wordIndex <= 0) break;
                            wordIndex--;
                            game.guess[wordIndex] = "-";
                            break;

                        // Submit the word.
                        case ConsoleKey.Enter:
                            if (wordIndex < game.guess.Count) break;
                            SubmitGuess(game, game.guessindex);

                            // Resets the variables for another guess.
                            if (string.Concat(game.guess.ToArray()) == game.wordle.ToString()) return EndGame(game, true); // Checks if the user has won.
                            game.guess = new List<string> { "-", "-", "-", "-", "-" };
                            wordIndex = 0;
                            game.guessindex++;
                            break;

                        // Exit the game and save.
                        case ConsoleKey.Escape:
                            if (GetInput($"\n;-{GetText("confirm")}", true, "n").ToLower() == "y") return EndGame(game, false, false);
                            break;

                        // Add the word to the list.
                        default:
                            if (wordIndex >= game.guess.Count || !keyPress.KeyChar.ToString().All(char.IsLetter)) break;
                            game.guess[wordIndex] = keyPress.KeyChar.ToString().ToLower();
                            wordIndex++;
                            break;
                    }
                } while (wordIndex < game.guess.Count || keyPress.Key != ConsoleKey.Enter);
            } while (Convert.ToInt32(game.tries) != 0);
            return EndGame(game, false); // Just in case...
        }
    }
}
