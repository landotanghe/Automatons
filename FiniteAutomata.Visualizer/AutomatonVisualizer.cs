using FiniteAutomota.NonDeterministic;
using System.Collections.Generic;
using System.Linq;

namespace FiniteAutomata.Visualizer
{
    public class AutomatonVisualizer
    {
        public void Visualize(Automaton Automaton)
        {
            var gridBuilder = new GridBuilder();
            var grid = gridBuilder.Build(Automaton);
            grid.Draw();
        }
        

        public class GridBuilder
        {
            private NeighboursFinder _neighboursFinder = new NeighboursFinder();
            private HashSet<State<string,char>> _visitedSets = new HashSet<State<string,char>>();

            private StatesGrid Grid = new StatesGrid();

            public StatesGrid Build(Automaton automaton)
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
                    Grid.Column(depth).Add(new Cell
                    {
                        Neighbour = neighbour,
                        IsFirstOccurence = HasVisited(state)
                    });
                    Visit(neighbour.State, depth+1);
                }
            }

            private bool HasVisited(State<string, char> state) => _visitedSets.Contains(state);
        }

        public class StatesGrid{
            private Dictionary<int, StatesColumn> _columns = new Dictionary<int, StatesColumn>();
            private IPainter _painter = new ConsolePainter();

            public StatesColumn Column(int i)
            {
                if (!_columns.ContainsKey(i))
                {
                    _columns.Add(i, new StatesColumn());
                }
                return _columns[i];
            }

            public void Draw()
            {
                foreach(var column in _columns)
                {

                }
            }
        }

        public class Cell
        {
            public Neighbour Neighbour { get; set; }
            public bool IsFirstOccurence { get; set; } //to determine if this cell is visualized or only its incoming edge is shown, still need to find where firstOccurence is as the incoming edge will point there
            public int Width => Neighbour.Width;
        }

        public class StatesColumn
        {
            public List<Cell> Cells = new List<Cell>();

            public void Add(Cell cell)
            {
                Cells.Add(cell);
            }

            public int Width => Cells.Select(cell => cell.Width).Max();
        }
    }
}
