using FiniteAutomota.NonDeterministic;
using System.Collections.Generic;
using System.Linq;

namespace FiniteAutomata.Visualizer
{
    public class AutomatonVisualizer
    {
        public Automaton Automaton { get; set; }
        

        public void Visualize()
        {
            

            List<State<string, char>> VisualizedStates = new List<State<string, char>>();
            

            foreach(var neighbour in Automaton.StartStates)
            {

            }
        }

        public void Traverse(State state, GridBuilder traversal)
        {

            traversal.Visit(state);

            

        }

        public class GridBuilder
        {
            private NeighboursFinder<string, char> _neighboursFinder = new NeighboursFinder<string, char>();
            private HashSet<State<string,char>> _visitedSets = new HashSet<State<string,char>>();

            private StatesGrid Grid = new StatesGrid();

            public void Build(Automaton automaton)
            {
                foreach (var startingState in automaton.StartStates)
                {
                    Visit(startingState);
                    return;//TODO multiple startingstates, jump to first free line
                }
            }

            public void Visit(State<string,char> state)
            {
                if (HasVisited(state))
                    return;

                _visitedSets.Add(state);

                var neighbours = _neighboursFinder.GetNeighbours(state);
                for(int i=0; i< neighbours.Count; i++)
                {
                    var neighbour = neighbours[i];
                    Grid.Line(i).Add(neighbour);
                }
            }

            public bool HasVisited(State<string, char> state) => _visitedSets.Contains(state);
        }

        public class StatesGrid{
            private Dictionary<int, StatesColumn> _columns = new Dictionary<int, StatesColumn>();
            private IPainter _painter = new ConsolePainter();

            public StatesColumn Line(int i)
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
            public Neighbour<string, char> Neighbour { get; set; }
            public bool IsFirstOccurence { get; set; } //to determine if this cell is visualized or only its incoming edge is shown, still need to find where firstOccurence is as the incoming edge will point there
            public int Width => Neighbour.Description.Length;
        }

        public class StatesColumn
        {
            public List<Cell> States = new List<Cell>();

            public void Add(Neighbour<string, char> neighbour)
            {
                States.Add(new Cell {
                   Neighbour = neighbour
                });
            }

            public int Width => States.Select(cell => cell.Width).Max();
        }
    }
}
