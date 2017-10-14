using FiniteAutomota.NonDeterministic;
using System.Collections.Generic;
using System.Linq;
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
            private NeighboursFinder<string, char> _neighboursFinder = new NeighboursFinder<string, char>();
            private HashSet<State<string,char>> _visitedSets = new HashSet<State<string,char>>();

            private StatesGrid Grid = new StatesGrid();

            public StatesGrid Build(Automaton<string,char> automaton)
            {
                foreach (var startingState in automaton.StartStates)
                {
                    Visit(startingState);
                    return Grid;//TODO multiple startingstates, jump to first free line or prepend columns...
                }

                return Grid;
            }

            private void Visit(State<string,char> state)
            {
                if (HasVisited(state))
                    return;

                _visitedSets.Add(state);

                var neighbours = _neighboursFinder.GetNeighbours(state);
                for(int i=0; i < neighbours.Count; i++)
                {
                    var neighbour = neighbours[i];
                    var column = Grid.Column(state);
                    var target = Grid.Column(neighbour.State);
                    column.Add(target, neighbour);

                    Visit(neighbour.State);
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

            internal List<StatesColumn> ColumnsInRange(StatesColumn column1, StatesColumn column2)
            {
                int index1 = _columnsIndex[column1];
                int index2 = _columnsIndex[column2];

                return index1 < index2
                    ? ColumnsInRange(index1, index2)
                    : ColumnsInRange(index2, index1);
            }

            private List<StatesColumn> ColumnsInRange(int index1, int index2)
            {
                var keys = _columns.Keys.Where(col => col >= index1 && col <= index2);
                return keys.Select(col => _columns[col])
                    .ToList();
            }
        }

        


        public class Arrow
        {
            public ArrowBase Source { get; }
            public ArrowHead Target { get; }
            public Neighbour<string, char> Neighbour { get; }
            public int Depth { get; }

            public Arrow(ArrowBase source, ArrowHead target, Neighbour<string, char> neighbour, int depth)
            {
                Source = source;
                Target = target;
                Neighbour = neighbour;
                Depth = depth;
            }
            
            public int Width => GetNeighbourWidth(Neighbour) + 4;

            private int GetNeighbourWidth(Neighbour<string, char> neighbour)
            {
                return neighbour.Description.Length + neighbour.Symbols.Count + (neighbour.IsEpsilonIncluded ? 1 : 0);
            }

            public void Draw(IPainter painter)
            {
                if(Source.Row == Target.Row && Depth == 0)
                {
                    painter.DrawWarpedArrow(Source.Row, Source.Column.X + Source.Column.Width + 2, Target.Row, Target.Column.X, Depth, Neighbour.Symbols.ToArray(), Neighbour.IsEpsilonIncluded);
                }
                else if(Depth >0)
                {
                    painter.DrawWarpedArrow(Source.Row + 1, Source.Column.X, Target.Row + 1, Target.Column.X - 2, Depth, Neighbour.Symbols.ToArray(), Neighbour.IsEpsilonIncluded);
                }else
                {
                    painter.DrawWarpedArrow(Source.Row - 1, Source.Column.X, Target.Row - 1, Target.Column.X+ 2, Depth, Neighbour.Symbols.ToArray(), Neighbour.IsEpsilonIncluded);
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
            private List<int> _depthsUsed;

            private StatesGrid _grid;
            private int _row;
            public StatesColumn(State<string,char> state, StatesGrid grid, int row)
            {
                State = state;
                _grid = grid;
                _row = row;
                _depthsUsed = new List<int>();
            }

            public List<Arrow> _arrows = new List<Arrow>();
            public List<Arrow> _backArrows = new List<Arrow>();//TODO use this

            public void Add(StatesColumn target, Neighbour<string, char> neighbour)
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

                if(arrowBase.Column.X < arrowHead.Column.X)
                {
                    AddForwardArrow(arrowBase, arrowHead, neighbour);
                }else
                {
                    AddBackwardArrow(arrowBase, arrowHead, neighbour);
                }
            }
            

            private void AddForwardArrow(ArrowBase arrowBase, ArrowHead arrowHead, Neighbour<string, char> neighbour)
            {
                var columns = _grid.ColumnsInRange(arrowBase.Column, arrowHead.Column);
                int depth = 1;
                if(columns.Count == 2 && arrowHead.Column._depthsUsed.Count() == 0)
                {
                    depth = 0;
                }
                else
                {
                    while (columns.Any(c => c._depthsUsed.Contains(depth)))
                    {
                        depth++;
                    }
                    foreach (var column in columns)
                    {
                        column._depthsUsed.Add(depth);
                    }
                }

                var arrow = new Arrow(arrowBase, arrowHead, neighbour, depth);

                _arrows.Add(arrow);
            }

            private void AddBackwardArrow(ArrowBase arrowBase, ArrowHead arrowHead, Neighbour<string, char> neighbour)
            {
                var columns = _grid.ColumnsInRange(arrowBase.Column, arrowHead.Column);
                int depth = 1;
                while (columns.Any(c => c._depthsUsed.Contains(depth)))
                {
                    depth--;
                }
                foreach (var column in columns)
                {
                    column._depthsUsed.Add(depth);
                }

                var arrow = new Arrow(arrowBase, arrowHead, neighbour, depth);
                _backArrows.Add(arrow);
            }
            
            public int X => _grid.GetX(this);

            public int Width => State.Description.Length;

            public int LongestArrowLength => _arrows.Union(_backArrows).Select(a => a.Width).Union(new List<int> { 5}).Max();

            public void Draw(IPainter painter)
            {
                painter.DrawNode(_row, X, State.Description);
                foreach(var arrow in _arrows)
                {
                    arrow.Draw(painter);
                }
                foreach(var arrow in _backArrows)
                {
                    arrow.Draw(painter);
                }
            }
        }
    }
}
