using System;

namespace FiniteAutomata.Visualizer
{
    public class ConsolePainter : IPainter
    {
        public void DrawRightWardArrow(int row, int col1, int col2, int depth, char[] symbols)
        {
            DrawRightWardArrow(row, col1, col2, depth);
            Console.CursorTop = row;
            Console.CursorLeft = col1 + 1;
            for(int i=0; i < symbols.Length; i++)
            {
                Console.Write(symbols[i]);
            }
        }

        public void DrawRightWardArrow(int row, int col1, int col2, int depth)
        {
            var deepestRow = row + depth;
            DrawVerticalLine(row, deepestRow, col1);
            DrawVerticalLine(row, deepestRow, col2);
            DrawHorizontalLine(deepestRow, col1, col2);
            DrawUpwardArrow(row, col2);
        }

        private void DrawHorizontalLine(int row, int col1, int col2)
        {
            Console.CursorTop = row;
            Console.CursorLeft = col1;
            for(int col = col1; col <= col2; col++)
            {
                Console.Write('-');
            }
        }

        private void DrawVerticalLine(int row1, int row2, int col)
        {
            for (int row = row1; row <= row2; row++)
            {
                Console.CursorTop = row;
                Console.CursorLeft = col;
                Console.Write('|');
            }
        }

        private void DrawUpwardArrow(int row, int col)
        {
            Console.CursorTop = row;
            Console.CursorLeft = col;
            Console.Write('^');
        }

    }
}
