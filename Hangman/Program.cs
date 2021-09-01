using System;
using System.Collections.Generic;

namespace Hangman
{
    class Program
    {
        static void Main(string[] args)
        {
            int health = 10;
            string menuSelection;
            string charInput;
            int points;
            char[] wordLetters;
            //int correctGuesses;
            List<char> correctGuesses = new List<char>();
            List<char> guessedChars = new List<char>();
            char currentGuess;
            string playedWord;
            bool alreadyGuessed = false;
            bool correctGuess = false;
            int counter = 0;
            playedWord = "pablo"; // temporary, hopefully

            Console.WriteLine("Hello World!");
            Console.WriteLine("Type 'play' to start a game with a random word in the library.");
            Console.WriteLine("Type 'manage' to add or remove words from the library.");
            menuSelection = Console.ReadLine();
            switch (menuSelection)
            {
                case "play":
                    wordLetters = playedWord.ToCharArray();
                    while (correctGuesses.Count < playedWord.Length)
                    {
                        while (counter < playedWord.Length)
                        {
                            Console.Write(" _");
                            counter++;
                            //gör om gör rätt
                        }
                        counter = 0;


                        charInput = Console.ReadLine(); //läs bokstaven
                        if (charInput.Length == 1) //om input är exakt en bokstav
                        {
                            currentGuess = char.Parse(charInput); //konvertera till en variabel för den nuvarande gissningen
                            while (counter < guessedChars.Count) //kollar om bokstaven gissats på tidigare
                            {
                                if (currentGuess == guessedChars[counter]) //om bokstaven gissats på tidigare så säger den till och man får inte poängavdrag eller bonus
                                {
                                    Console.WriteLine("You have already guessed this letter!");
                                    alreadyGuessed = true; //håller koll på om den nuvarande gissningen redan gjorts
                                }
                                counter++;
                            }
                            counter = 0;
                            

                            if (alreadyGuessed == false) //körs bara om bokstaven inte gissats tidigare
                            {
                                guessedChars.Add(currentGuess); //lägger till gissningen till listan av gissningar
                                while (counter < playedWord.Length)
                                {
                                    if (currentGuess == wordLetters[counter]) //om gissningen innehåller en av ordets bokstäver så körs denna
                                    {
                                        correctGuesses.Add(currentGuess); //lägger till denna gissning i listan av korrekta gissningar (todo: se till att den hamnar på en motsvarande plats mot positionen av bokstaven den kollar))
                                        Console.WriteLine("Correct guess!");
                                        correctGuess = true;
                                    }
                                    counter++;
                                }
                                if (correctGuess == false)
                                {
                                    Console.WriteLine("Sorry, that's not right!"); //skadar spelaren om gissningen är fel
                                    health--;
                                }
                            }


                            //återställer till nästa loop
                            counter = 0;
                            alreadyGuessed = false; 
                            correctGuess = false;
                        }
                        else { Console.WriteLine("That's not a single letter! Please type only one character."); }
                    }


                    if (correctGuesses.Count == playedWord.Length) //when the amount of correct guesses are the same as the length of the played word the player wins and the game ends
                    {
                        Console.WriteLine("Congratulations! You win :)");
                    }


                    break;
                case "manage":

                break;
                default:
                    Console.WriteLine("I didn't understand that. Type 'play' to start a game or 'manage' to add or remove words from the library (both in lowercase)");
                break;
            }

        }
    }
}
