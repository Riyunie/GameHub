using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;

namespace Speedrun
{
    class Program
    {

        static void Main(string[] args)
        {
            Welcome("#     #  #####  #      ####   ######    #   #   #####");
            Welcome("#     #  #      #     #      #      #   ## ##   #    ");
            Welcome("#  #  #  ####   #     #      #      #   # # #   #####");
            Welcome("#  #  #  #      #     #      #      #   #   #   #    ");
            Welcome(" #   #   #####  ####   ####   ######    #   #   ##### \n");

            Welcome("#####   ##### ");
            Welcome("  #    #     #");
            Welcome("  #    #     #");
            Welcome("  #    #     #");
            Welcome("  #     ##### \n");

            Welcome(" ####    #    #   #  #####  #   #  #   # ##### ");
            Welcome("#       # #   ## ##  #      #   #  #   #  #   # ");
            Welcome("#  ##  #####  # # #  ####   #####  #   #  ##### ");
            Welcome("#   #  #   #  #   #  #      #   #  #   #  #    #");
            Welcome(" ###   #   #  #   #  #####  #   #   ###  ######");

            Console.WriteLine("\nEnter the number of the game you want to play :D\n \n1.Snake Game \n2.Hangman \n\nPress \"0\" to exit the game");

            string UserInput = Console.ReadLine();

            switch (UserInput)
            {
                case "0":
                    Console.WriteLine("Thank you for playing :D");
                    Environment.Exit(0);
                    break;
                case "1":
                    Console.Clear();
                    Console.WriteLine("Starting Snake Game...");
                    Console.Clear();
                    SnakeGame();
                    break;

                case "2":
                    Console.WriteLine("Starting Hangman Game...");
                    Console.Clear();
                    Hangman();
                    break;

            }

        }

        static void Welcome(string text)
        {
            Console.WriteLine(text);
            Thread.Sleep(100);
        }

        static void SnakeGame()
        {
            int width = 30;
            int height = 20;
            int snakeX = 0;
            int snakeY = 0;
            int foodX = 0;
            int foodY = 0;
            int score = 0;
            bool gameOver = false;
            char direction = 'R';
            List<int> tailX = new List<int>();
            List<int> tailY = new List<int>();
            Random random = new Random();

            Console.CursorVisible = false;
            Console.SetWindowSize(width + 1, height + 1);

            snakeX = width / 2;
            snakeY = height / 2;
            foodX = random.Next(1, width);
            foodY = random.Next(1, height);

            void DrawBorders()
            {
                Console.SetCursorPosition(0, 0);
                for (int i = 0; i <= width; i++)
                    Console.Write("#");

                for (int i = 1; i < height; i++)
                {
                    Console.SetCursorPosition(0, i);
                    Console.Write("#");
                    Console.SetCursorPosition(width, i);
                    Console.Write("#");
                }

                Console.SetCursorPosition(0, height);
                for (int i = 0; i <= width; i++)
                    Console.Write("#");
            }

            void DrawFood()
            {
                Console.SetCursorPosition(foodX, foodY);
                Console.Write("X");
            }

            void DrawSnake()
            {
                Console.SetCursorPosition(snakeX, snakeY);
                Console.Write("O");

                for (int tailIndex = 0; tailIndex < tailX.Count; tailIndex++)
                {
                    Console.SetCursorPosition(tailX[tailIndex], tailY[tailIndex]);
                    Console.Write("o");
                }

            }

            void UpdateDirection(ConsoleKey key)
            {
                switch (key)
                {
                    case ConsoleKey.W:
                        if (direction != 'D')
                            direction = 'U';
                        break;
                    case ConsoleKey.S:
                        if (direction != 'U')
                            direction = 'D';
                        break;
                    case ConsoleKey.A:
                        if (direction != 'R')
                            direction = 'L';
                        break;
                    case ConsoleKey.D:
                        if (direction != 'L')
                            direction = 'R';
                        break;
                }
            }

            void MoveSnake()
            {
                int prevX = snakeX;
                int prevY = snakeY;

                switch (direction)
                {
                    case 'R':
                        snakeX++;
                        break;
                    case 'L':
                        snakeX--;
                        break;
                    case 'U':
                        snakeY--;
                        break;
                    case 'D':
                        snakeY++;
                        break;
                }

                if (tailX.Count > 0)
                {
                    tailX.Insert(0, prevX);
                    tailY.Insert(0, prevY);
                    tailX.RemoveAt(tailX.Count - 1);
                    tailY.RemoveAt(tailY.Count - 1);
                }
            }

            void CheckCollision()
            {
                if (snakeX <= 0 || snakeX >= width || snakeY <= 0 || snakeY >= height)
                    gameOver = true;

                if (snakeX == foodX && snakeY == foodY)
                {
                    score += 10;

                    tailX.Add(foodX);
                    tailY.Add(foodY);

                    foodX = random.Next(1, width);
                    foodY = random.Next(1, height);
                }
                else
                {

                    for (int i = 0; i < tailX.Count; i++)
                    {
                        if (snakeX == tailX[i] && snakeY == tailY[i])
                            gameOver = true;
                    }
                }
            }

            void DrawGame()
            {
                Console.Clear();
                DrawBorders();
                DrawFood();
                DrawSnake();
            }

            while (!gameOver)
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true).Key;
                    UpdateDirection(key);
                }

                MoveSnake();
                CheckCollision();
                DrawGame();
                Thread.Sleep(100);
            }

            Console.SetCursorPosition(width / 2 - 8, height / 2);
            Console.WriteLine("Game Over! Score: " + score);


            bool playAgain = AskPlayAgain();
            if (playAgain)
            {
                Console.Clear();
                SnakeGame();
            }
            else
            {
                Console.WriteLine("Thank you for playing the Snake Game!");
                Environment.Exit(0);
            }
        }

        static void Hangman()
        {
            string[] words = { "apple", "banana", "orange", "grape", "strawberry" };
            string wordToGuess;
            char[] guessedLetters;
            int attemptsLeft = 6;

            void InitializeGame()
            {
                Random random = new Random();
                wordToGuess = words[random.Next(0, words.Length)];
                guessedLetters = new char[wordToGuess.Length];

                for (int i = 0; i < guessedLetters.Length; i++)
                {
                    guessedLetters[i] = '_';
                }
            }

            void PlayGame()
            {
                bool wordGuessed = false;

                while (!wordGuessed && attemptsLeft > 0)
                {
                    Console.WriteLine($"\nAttempts left: {attemptsLeft}");
                    Console.WriteLine($"Word to guess (Fruit edition): {string.Join(" ", guessedLetters)}");

                    Console.Write("\nEnter a letter: ");
                    char input = Console.ReadLine().ToLower()[0];

                    if (wordToGuess.Contains(input))
                    {
                        UpdateGuessedLetters(input);
                    }
                    else
                    {
                        Console.WriteLine($"The word doesn't contain '{input}'.");
                        attemptsLeft--;
                    }

                    wordGuessed = CheckIfWordGuessed();
                }

                if (wordGuessed)
                {
                    Console.WriteLine($"\nCongratulations! You guessed the word: {wordToGuess}");
                }
                else
                {
                    Console.WriteLine("\nGame Over! You ran out of attempts.");
                }

                Console.ReadKey();
            }

            void UpdateGuessedLetters(char letter)
            {
                for (int i = 0; i < wordToGuess.Length; i++)
                {
                    if (wordToGuess[i] == letter)
                    {
                        guessedLetters[i] = letter;
                    }
                }
            }

            bool CheckIfWordGuessed()
            {
                return !guessedLetters.Contains('_');
            }

            InitializeGame();
            PlayGame();

            bool playAgain = AskPlayAgain();
            if (playAgain)
            {
                Console.Clear();
                Hangman();
            }
            else
            {
                Console.WriteLine("Thank you for playing Hangman!");
                Environment.Exit(0);
            }
        }

        static bool AskPlayAgain()
        {
            Console.WriteLine("\n         Try again? (Y/N)");
            string playAgainInput = Console.ReadLine().Trim().ToLower();

            while (playAgainInput != "y" && playAgainInput != "n")
            {
                Console.WriteLine("         Y or N.");
                playAgainInput = Console.ReadLine();
            }

            return playAgainInput == "y";
        }
    }
}