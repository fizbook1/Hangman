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
            int health = 10;
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

            //display 
            StringBuilder progressDisplay = new StringBuilder(50);
            var rand = new Random();
            //här börjar grejer



            List<string> wordList = File.ReadAllLines("WordList.txt").ToList();
            Console.WriteLine("Hello World!");
            Console.WriteLine("Type 'play' to start a game with a random word in the library.");
            Console.WriteLine("Type 'manage' to add or remove words from the library.");
            menuSelection = Console.ReadLine();

            switch (menuSelection)
            {
                case "play":
                    
                    playedWord = wordList[rand.Next(wordList.Count)].ToLower(); // temporary, hopefully
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
                                if (T == wordLetters[counter]) //ifall bokstaven i gissade bokstäver stämmer överens med en av ordets bokstäver så skrivs bokstaven ut på den platsen.
                                {
                                    progressDisplay.Append("[");
                                    progressDisplay.Append(wordLetters[counter]);
                                    progressDisplay.Append("]");
                                    currentCycleReturnedValue = true;
                                }
                           }
                             
                           if(currentCycleReturnedValue == false) //ifall ingen bokstav stämmer överens skriver den ett tomrum
                           {
                                progressDisplay.Append("[_]");                          
                           }
                           currentCycleReturnedValue = false;
                           counter++;
                        }
                        Console.WriteLine(progressDisplay);
                        if (correctStreak > 2)
                        {
                            Console.WriteLine("Guessed " + correctStreak + " letters correctly in a row!");
                        }
                        progressDisplay.Clear();
                        counter = 0;


                        charInput = Console.ReadLine().ToLower(); //läs bokstaven
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
                                        correctGuesses.Add(currentGuess); //lägger till denna gissning i listan av korrekta gissningar)
                                        Console.WriteLine("Correct guess!");
                                        correctGuess = true; //genom något mirakel funkar detta även när flera av samma bokstav förekommer
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
