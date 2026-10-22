using System;
using System.Collections.Generic;
using System.Threading;

namespace SnakeGame
{
    class Program
    {
        static int width = 30;
        static int height = 20;
        static int score = 0;
        static int speed = 150; // Скорость игры (меньше = быстрее)

        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            Console.WindowWidth = width + 2;
            Console.WindowHeight = height + 2;

            List<Point> snake = new List<Point> { new Point(width / 2, height / 2) };
            Point food = GenerateFood(snake);
            Direction dir = Direction.Right;

            while (true)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKey key = Console.ReadKey(true).Key;
                    switch (key)
                    {
                        case ConsoleKey.UpArrow: if (dir != Direction.Down) dir = Direction.Up; break;
                        case ConsoleKey.DownArrow: if (dir != Direction.Up) dir = Direction.Down; break;
                        case ConsoleKey.LeftArrow: if (dir != Direction.Right) dir = Direction.Left; break;
                        case ConsoleKey.RightArrow: if (dir != Direction.Left) dir = Direction.Right; break;
                    }
                }

                Point head = snake[0];
                Point newHead = new Point(head.X, head.Y);

                switch (dir)
                {
                    case Direction.Up: newHead.Y--; break;
                    case Direction.Down: newHead.Y++; break;
                    case Direction.Left: newHead.X--; break;
                    case Direction.Right: newHead.X++; break;
                }

                // Проверка на столкновение со стенами или собой
                if (newHead.X < 0 || newHead.X >= width || newHead.Y < 0 || newHead.Y >= height || snake.Contains(newHead))
                {
                    GameOver();
                    return;
                }

                snake.Insert(0, newHead);

                if (newHead.Equals(food))
                {
                    score++;
                    food = GenerateFood(snake);
                }
                else
                {
                    snake.RemoveAt(snake.Count - 1);
                }

                Console.Clear();
                DrawBorders();
                Console.SetCursorPosition(food.X, food.Y);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("@"); // Яблоко

                Console.ForegroundColor = ConsoleColor.Green;
                foreach (var p in snake)
                {
                    Console.SetCursorPosition(p.X, p.Y);
                    Console.Write("O"); // Тело змейки
                }

                Console.ForegroundColor = ConsoleColor.White;
                Thread.Sleep(speed);
            }
        }

        static void DrawBorders()
        {
            for (int i = 0; i <= width; i++)
            {
                Console.SetCursorPosition(i, height); Console.Write("#");
                Console.SetCursorPosition(i, 0); Console.Write("#");
            }
            for (int i = 0; i <= height; i++)
            {
                Console.SetCursorPosition(0, i); Console.Write("#");
                Console.SetCursorPosition(width, i); Console.Write("#");
            }
        }

        static Point GenerateFood(List<Point> snake)
        {
            Random rnd = new Random();
            while (true)
            {
                Point p = new Point(rnd.Next(1, width - 1), rnd.Next(1, height - 1));
                if (!snake.Contains(p)) return p;
            }
        }

        static void GameOver()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n\n  ИГРА ОКОНЧЕНА!");
            Console.ResetColor();
            Console.WriteLine($"  Ваш счет: {score}");
            Console.WriteLine("  Нажмите любую клавишу для выхода...");
            Console.ReadKey();
        }
    }

    enum Direction { Up, Down, Left, Right }

    class Point
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Point(int x, int y) { X = x; Y = y; }

        public override bool Equals(object obj) => obj is Point p && X == p.X && Y == p.Y;
        public override int GetHashCode() => HashCode.Combine(X, Y);
    }
}