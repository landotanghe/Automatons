using System;

namespace FiniteAutomata.Visualizer
{
    public class ConsolePainter : IPainter
    {
        public void DrawWarpedArrow(int row1, int col1, int row2, int col2, int depth, char[] symbols, bool epsilon)
        {
            if (!CanBeDrawn(row1, row2, depth))
                throw new ArgumentException("can not draw this arrow");

            DrawWarpedArrow(row1, col1, row2, col2, depth);

            if (depth >= 0)
            {
                DrawSymbols(row1 + depth, col1 + 1, symbols, epsilon);
            }else
            {
                DrawSymbols(row2 - 1, col2 + 2, symbols, epsilon);
            }
        }

        private bool CanBeDrawn(int row1, int row2, int depth)
        {
            var maxDepth = Absolute(depth);
            var actualDepth = Absolute(row1 - row2);
            return actualDepth <= maxDepth;
        }

        private static void DrawSymbols(int row, int col, char[] symbols, bool epsilon)
        {
            for (int i = 0; i < symbols.Length; i++)
            {
                Draw(row, col + i, symbols[i], ConsoleColor.Red, ConsoleColor.DarkGray);
            }
            if (epsilon)
            {
                Draw(row, col + symbols.Length, '€', ConsoleColor.Green, ConsoleColor.DarkGray);
            }
        }
        
        private int Absolute(int val)
        {
            return val > 0 ? val : -val;
        }

        private void DrawWarpedArrow(int row1, int col1, int row2, int col2, int depth)
        {
            var deepestRow = row1 + depth;

            if(depth == 0)
            {
                DrawHorizontalLine(deepestRow, col1, col2);
                Draw(row2, col2 - 1, '>');
            }
            else if (depth < 0)
            {
                DrawVerticalLine(row1, deepestRow, col1);
                DrawVerticalLine(row2, deepestRow + 1, col2);
                DrawHorizontalLine(deepestRow, col1, col2 + 1);
                Draw(row2, col2, '/');
            }else
            {
                DrawVerticalLine(row1, deepestRow, col1);
                DrawVerticalLine(row2, deepestRow - 1, col2);
                DrawHorizontalLine(deepestRow, col1, col2 - 1);
                Draw(row2, col2, '/');
            }
        }

        private void DrawHorizontalLine(int row, int col1, int col2)
        {
            if (col1 > col2)
            {
                DrawHorizontalLine(row, col2, col1);
            }
            else
            {
                for (int col = col1; col <= col2; col++)
                {
                    Draw(row, col, '-');
                }
            }
        }

        private void DrawVerticalLine(int row1, int row2, int col)
        {
            if (row1 > row2) { 
                DrawVerticalLine(row2, row1, col);
            }
            else
            {
                for (int row = row1; row <= row2; row++)
                {
                    Draw(row, col, '|');
                }
            }
        }

        private static void Draw(int row, int col, char character)
        {
            Draw(row, col, character, ConsoleColor.White, ConsoleColor.Black);
        }

        private static void Draw(int row, int col, char character, ConsoleColor fore, ConsoleColor back)
        {
            Console.CursorTop = row;
            Console.CursorLeft = col;
            Console.ForegroundColor = fore;
            Console.BackgroundColor = back;
            Console.Write(character);
        }

        public void DrawNode(int row, int col, string description)
        {
            Console.CursorTop = row;
            Console.CursorLeft = col;
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write($"({description})");
        }
    }
}
