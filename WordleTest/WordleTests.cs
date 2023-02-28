using NUnit.Framework;
using System;
using System.Collections.Generic;
using static WordleProgram.Wordle;

namespace WordleTests
{
    public class WordleTests
    {
        [Test]
        // Converts a string into a List<string> by using the specified character as the split.
        public void StringToListTest()
        {
            var expected = new List<string>() { "hello", "world", "programmed", "to", "work" };
            Assert.That(expected, Is.EqualTo(StringToList("hello;world;programmed;to;work", ";")));
        }

        [Test]
        // Gets the entry "x" from "lang.json". The results may vary depending on the selected language.
        public void GetTextTest()
        {
            var expected = "Welcome! Please, insert your player name.";
            Environment.CurrentDirectory = @"..\..\..\..\";
            Assert.That(expected, Is.EqualTo(GetText("gamename")));
        }

        [Test]
        // Does practically nothing but stall the code, adding a Console.WriteLine() and saving the game when needed.
        public void EndGameTest()
        {
            var time = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            Environment.CurrentDirectory = @"..\..\..\..\";
            var game = new gameJson
            {
                name = "pepito",
                guess = new List<string> { "-", "-", "-", "-", "-" },
                wordle = "test",
                tries = 6,
                guesses = new Guesses
                {
                    guess1 = "-----",
                    guess2 = "-----",
                    guess3 = "-----",
                    guess4 = "-----",
                    guess5 = "-----",
                    guess6 = "-----"
                },
                time = time
            };
            Assert.That((game, true), Is.EqualTo(EndGame(game, true, false)));
        }
    }
}