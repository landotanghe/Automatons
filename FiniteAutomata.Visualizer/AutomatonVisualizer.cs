using FiniteAutomota.NonDeterministic;
using System.Collections.Generic;
using System.Linq;
using System;

namespace FiniteAutomata.Visualizer
{
    public class AutomatonVisualizer
    {
        public void Visualize(Automaton<string,char> automaton)
        {
            var gridBuilder = new GridBuilder();
            var grid = gridBuilder.Build(automaton);
            grid.Draw();
        }
        

        public class GridBuilder
        {
            private NeighboursFinder _neighboursFinder = new NeighboursFinder();
            private HashSet<State<string,char>> _visitedSets = new HashSet<State<string,char>>();

            private StatesGrid Grid = new StatesGrid();

            public StatesGrid Build(Automaton<string,char> automaton)
            {
                foreach (var startingState in automaton.StartStates)
                {
                    Visit(startingState, 0);
                    return Grid;//TODO multiple startingstates, jump to first free line or prepend columns...
                }

                return Grid;
            }

            public void Visit(State<string,char> state, int depth)
            {
                if (HasVisited(state))
                    return;

                _visitedSets.Add(state);

                var neighbours = _neighboursFinder.GetNeighbours(state);
                for(int i=0; i< neighbours.Count; i++)
                {
                    var neighbour = neighbours[i];
                    var column = Grid.Column(state);
                    var target = Grid.Column(neighbour.State);
                    column.Add(target, neighbour);

                    Visit(neighbour.State, depth+1);
                }
            }

            private bool HasVisited(State<string, char> state) => _visitedSets.Contains(state);
        }

        public class StatesGrid{
            private Dictionary<int, StatesColumn> _columns = new Dictionary<int, StatesColumn>();
            private Dictionary<StatesColumn, int> _columnsIndex = new Dictionary<StatesColumn, int>();
            private IPainter _painter = new ConsolePainter();

            private int _currentRow = 3;

            public void StartNextRow()
            {
                _currentRow++;
            }

            public StatesColumn Column(State<string,char> state)
            {
                var column = _columnsIndex.Select(c => c.Key).FirstOrDefault(c => c.State == state);
                if (column == null)
                {
                    var index = _columns.Count;
                    column = new StatesColumn(state, this, _currentRow);
                    _columns.Add(index, column);
                    _columnsIndex.Add(column, index);
                }
                return column;
            }

            public void Draw()
            {
                foreach(var column in _columns.Values)
                {
                    column.Draw(_painter);
                }
            }

            internal int GetX(StatesColumn column)
            {
                var index = _columnsIndex[column];

                var totalWidth = 0;
                for(int i=0; i< index; i++)
                {
                    totalWidth += _columns[i].Width + _columns[i].LongestArrowLength;
                }
                return totalWidth;
            }
        }


        public class Arrow
        {
            public ArrowBase Source { get; }
            public ArrowHead Target { get; }
            public Neighbour Neighbour { get; }
            public int Depth { get; }

            public Arrow(ArrowBase source, ArrowHead target, Neighbour neighbour, int depth)
            {
                Source = source;
                Target = target;
                Neighbour = neighbour;
            }
            
            public int Width => Neighbour.Width + 4;

            public void Draw(IPainter painter)
            {
                if(Source.Row== Target.Row)
                {
                    painter.DrawWarpedArrow(Source.Row, Source.Column.X + Source.Column.Width + 2, Target.Row, Target.Column.X, Depth, Neighbour.Symbols.ToArray(), Neighbour.IsEpsilonIncluded);
                }
                else
                {
                    painter.DrawWarpedArrow(Source.Row, Source.Column.X, Target.Row, Target.Column.X, Depth, Neighbour.Symbols.ToArray(), Neighbour.IsEpsilonIncluded);
                }
            }
        }
        
        public class ArrowBase
        {
            public StatesColumn Column { get; set; }
            public int Row { get; set; }
        }

        public class ArrowHead
        {
            public StatesColumn Column { get; set; }
            public int Row { get; set; }
        }

        public class StatesColumn
        {
            public State<string,char> State;
            private StatesGrid _grid;
            private int _row;
            public StatesColumn(State<string,char> state, StatesGrid grid, int row)
            {
                State = state;
                _grid = grid;
                _row = row;
            }

            private List<Arrow> _arrows = new List<Arrow>();
            private int firstFreeRow = 0;

            public void Add(StatesColumn target, Neighbour neighbour)
            {
                var arrowBase = new ArrowBase
                {
                    Column = this,
                    Row = _row
                };

                var arrowHead = new ArrowHead
                {
                    Column = target,
                    Row = target._row
                };

                var arrow = new Arrow(arrowBase, arrowHead, neighbour, _arrows.Count());

                _arrows.Add(arrow);
            }

            public int X => _grid.GetX(this);

            public int Width => State.Description.Length;

            public int LongestArrowLength => _arrows.Select(a => a.Width).Max();

            public void Draw(IPainter painter)
            {
                painter.DrawNode(_row, X, State.Description);
                foreach(var arrow in _arrows)
                {
                    arrow.Draw(painter);
                }
            }
        }
    }
}
