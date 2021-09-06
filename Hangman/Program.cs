using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

namespace Hangman
{
    class Program
    {
        static void Main(string[] args)
        {
            //för att slumpa ett ord från en lista kanske lite för satsigt gör färdigt en annan gång

            string path = "WordList.txt";

            //List<string> wordList = new List<string>();

            //input stuff
            string menuSelection;
            string charInput;

            //gameplay grejer
            int health = 8;
            string playedWord;
            char[] wordLetters;
            List<char> correctGuesses = new List<char>();
            List<char> guessedChars = new List<char>();
            bool guessedWordCompletely = false;
            int correctStreak = 0;
                       
            //cycle grejer
            char currentGuess;
            bool alreadyGuessed = false;
            bool correctGuess = false;
            bool currentCycleReturnedValue = false;
            int counter = 0; //jag hade kanske kunnat skippa den här men jag är lat och dum
            bool exitGame = false;

            //manage
            int numInput;
            List<string> wordList = new List<string>();

            //display 
            StringBuilder progressDisplay = new StringBuilder(50);
            var rand = new Random();
            //här börjar grejer


            if (File.Exists(path) == false) {
                File.Create(path);
            }

            wordList = File.ReadAllLines("WordList.txt").ToList();
            wordList.Add("assurance");
            wordList.Add("capybara");
            wordList.Add("raspberry");
            Console.WriteLine("Hello World!");
            Console.WriteLine("Type 'play' to start a game with a random word in the library.");
            Console.WriteLine("Type 'manage' to add or (in the future!) remove words from the library.");
            menuSelection = Console.ReadLine();

            switch (menuSelection)
            {
                case "play":
                    
                    playedWord = wordList[rand.Next(wordList.Count)].ToLower(); // temporary, hopefully UPDATE: it was not temporary
                    wordLetters = playedWord.ToCharArray();
                    correctGuesses.ToArray();
                    while (correctGuesses.Count < playedWord.Length && health > 0)
                    {
                        if (guessedChars.Count == 0)
                        {
                            Console.WriteLine("The word is ", playedWord.Length, " letters long!");
                            foreach (char c in wordLetters) {
                                Console.Write("[_]");
                                }
                            Console.WriteLine("");
                            Console.WriteLine("You may enter either a single character or the entire word, once you have figured out what it is.");
                            Console.WriteLine("You have " + health + " tries. One will be removed upon an incorrect guess.");
                            Console.WriteLine("Repeating a previous guess or guessing correctly will not remove a try.");
                            Console.WriteLine("To guess the entire word, type 'guess' instead of a letter.");
                        }

                        while (counter < playedWord.Length && guessedChars.Count > 0)
                        {
                           foreach(char T in guessedChars)  
                           {
                                if (T == wordLetters[counter]) //ifall bokstaven i gissade bokstäver stämmer överens med en av ordets bokstäver så skrivs bokstaven ut på den platsen av den bokstäven
                                {
                                    progressDisplay.Append("[");
                                    progressDisplay.Append(wordLetters[counter]);
                                    progressDisplay.Append("]");
                                    currentCycleReturnedValue = true;
                                }
                           }                          
                           if(currentCycleReturnedValue == false)
                           {
                                progressDisplay.Append("[_]");                          
                           }
                           currentCycleReturnedValue = false;
                           counter++;
                        }

                        Console.WriteLine(progressDisplay);
                        if (health < 8) { Console.WriteLine("_____"); }
                        if (health < 7) { Console.WriteLine("  |  "); }
                        if (health < 6) { Console.WriteLine("  O  "); } 
                        if (health < 5) { Console.WriteLine(" /X\\ "); }
                        if (health < 4) { Console.WriteLine("/ X \\ "); }
                        if (health < 3) { Console.WriteLine(" / \\ "); }
                        if (health < 2) { Console.WriteLine("/   \\"); }
                        //jätteklumpigt men det fungerar och det är allt jag bryr mig om just nu 
                        if (correctStreak > 2)
                        {
                            Console.WriteLine("Guessed " + correctStreak + " letters correctly in a row!");
                        }
                        progressDisplay.Clear();
                        counter = 0;

                        charInput = Console.ReadLine().ToLower(); 
                        if (charInput.Length == 1) 
                        {
                            currentGuess = char.Parse(charInput); 
                            while (counter < guessedChars.Count) 
                            {
                                if (currentGuess == guessedChars[counter]) 
                                {
                                    Console.WriteLine("You have already guessed this letter!");
                                    alreadyGuessed = true;
                                }
                                counter++;
                            }
                            counter = 0;

                            if (alreadyGuessed == false)
                            {
                                guessedChars.Add(currentGuess); 
                                while (counter < playedWord.Length)
                                {
                                    if (currentGuess == wordLetters[counter]) 
                                    {
                                        correctGuesses.Add(currentGuess); 
                                        Console.WriteLine("Correct guess!");
                                        correctGuess = true; //funkar vid flera av samma bokstav pga lägger till bokstaven lika många gånger som den finns i ordet
                                        correctStreak++;
                                    }
                                    counter++;
                                }
                                if (correctGuess == false)
                                {
                                    health--;
                                    correctStreak = 0;
                                    if (health > 0) //så den inte säger att du har 0 försök kvar
                                    {
                                        Console.WriteLine("Sorry, that's not right! You have " + health + " tries remaining."); 
                                    }
                                }
                            }


                            //återställer till nästa loop
                            counter = 0;
                            alreadyGuessed = false;
                            correctGuess = false;
                        }
                        else if (charInput == "guess")
                        {
                            Console.WriteLine("You think you know the entire word? Type it down below if you're so sure!");
                            charInput = Console.ReadLine().ToLower();
                            if (charInput == playedWord) { 
                                foreach (char c in wordLetters)
                                {
                                    correctGuesses.Add(c);
                                    guessedWordCompletely = true;
                                }
                            }
                            else {
                                health--;
                                correctStreak = 0;
                                if (health > 0) //så den inte säger att du har 0 försök kvar
                                {
                                    Console.WriteLine("Sorry, that's not right! You have " + health + " tries remaining.");
                                }
                            }
                        }
                        else if (guessedWordCompletely == false && charInput.Length == 0) { Console.WriteLine("You didn't write anything!"); }
                        else
                        {
                            Console.WriteLine("If you wish to guess what the word is, type 'guess'. Otherwise, please only enter one character.");
                            Console.WriteLine("");
                        }

                        /* Gör färdigt detta en vacker dag när du inte är lika trött
                        Console.WriteLine("Press the any key to proceed.");
                        Console.ReadKey();
                        Console.Clear();
                        */ 

                    }

                    if (health < 1) //när du misslyckas :(
                    {
                        Console.WriteLine("I'm sorry. You lost the game. The word was " + playedWord + ".");
                    }
                    
                    if (correctGuesses.Count == playedWord.Length || guessedWordCompletely == true) //when the amount of correct guesses are the same as the length of the played word the player wins and the game ends
                    {
                        Console.WriteLine("Congratulations! You win :)");
                        Console.WriteLine("The word is " + playedWord + "!");
                    }
                    break;

                case "manage":
                    while(exitGame == false) 
                    { 
                        Console.WriteLine("To view the existing words, type 'view'.");
                        Console.WriteLine("To add a word, type 'add'.");
                        menuSelection = Console.ReadLine();
                        switch (menuSelection)
                        {
                            case "add":
                                Console.WriteLine("Type the word you want to add.");
                                charInput = Console.ReadLine();
                                wordList.Add(charInput);
                                File.WriteAllLines(path, wordList, Encoding.UTF8);

                            break;
                            /* case "view":
                                 foreach (string str in wordList) { 
                                 Console.WriteLine("[" + counter + "]" + wordList[counter]);
                                     counter++;
                                 }
                                 Console.WriteLine("To remove an item, type the number corresponding to the word you wish to delete.");
                                 Console.WriteLine("If you don't wish to make any changes, type skip.");
                                 counter = 0;
                                 charInput = Console.ReadLine();
                                 if (charInput.All(char.IsDigit) == true)
                                 {
                                     numInput =  parse
                                 }
                                 break; */
                            default:
                                Console.WriteLine("pls do as told");
                                exitGame = true;
                                break;
                        }

                    }
                break;
                default:
                    Console.WriteLine("I didn't understand that. Type 'play' to start a game or 'manage' to add or remove words from the library (both in lowercase)");
                break;
            }

        }
    }
}
