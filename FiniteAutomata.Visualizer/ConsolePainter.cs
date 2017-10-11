using System;

namespace FiniteAutomata.Visualizer
{
    public class ConsolePainter : IPainter
    {
        public void DrawWarpedArrow(int row1, int col1, int row2, int col2, int depth, char[] symbols)
        {
            if (!CanBeDrawn(row1, row2, depth))
                throw new ArgumentException("can not draw this arrow");

            DrawWarpedArrow(row1, col1, row2, col2, depth);
            Console.CursorTop = row1 + depth;
            Console.CursorLeft = col1 + 1;
            for(int i=0; i < symbols.Length; i++)
            {
                Console.Write(symbols[i]);
            }
        }

        private bool CanBeDrawn(int row1, int row2, int depth)
        {
            var maxDepth = Absolute(depth);
            var actualDepth = Absolute(row1 - row2);
            return actualDepth <= maxDepth;
        }

        private int Absolute(int val)
        {
            return val > 0 ? val : -val;
        }

        public void DrawWarpedArrow(int row1, int col1, int row2, int col2, int depth)
        {
            var deepestRow = row1 + depth;
            DrawVerticalLine(row1, deepestRow, col1);
            DrawVerticalLine(row2, deepestRow, col2);
            DrawHorizontalLine(deepestRow, col1, col2);
            DrawUpwardArrow(row2, col2);
        }

        private void DrawHorizontalLine(int row, int col1, int col2)
        {
            if (col1 > col2)
            {
                DrawHorizontalLine(row, col2, col1);
            }
            else
            {
                Console.CursorTop = row;
                Console.CursorLeft = col1;
                for (int col = col1; col <= col2; col++)
                {
                    Console.Write('-');
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
                    Console.CursorTop = row;
                    Console.CursorLeft = col;
                    Console.Write('|');
                }
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
