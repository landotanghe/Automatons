using System;
using System.Collections.Generic;
using System.Linq;

namespace FiniteAutomota.NonDeterministic.Builder
{
    public class StateDefintionsManager<Descriptor, Symbol>
    {
        private List<AddStateStep<Descriptor, Symbol>> StatesToAdd = new List<AddStateStep<Descriptor, Symbol>>();
        private List<AddSubSequenceStep<Descriptor, Symbol>> SubsequencesToAdd = new List<AddSubSequenceStep<Descriptor, Symbol>>();

        public void AddState(AddStateStep<Descriptor, Symbol> state)
        {
            StatesToAdd.Add(state);
        }

        public void AddSubsequence(AddSubSequenceStep<Descriptor,Symbol> seq)
        {
            SubsequencesToAdd.Add(seq);
        }

        private bool Equals(Descriptor description1, Descriptor description2)
        {
            return EqualityComparer<Descriptor>.Default.Equals(description1, description2);
        }

        public State<Descriptor, Symbol> FindStateOrDefault(Descriptor description)
        {
            return StatesToAdd
                .Select(state => state.StateToBuild)
                .Where(state => Equals(state.Description, description))
                .SingleOrDefault();
        }

        public Automaton<Descriptor, Symbol> FindSubSequenceOrDefault(Descriptor description)
        {
            return SubsequencesToAdd
                .Where(seq => Equals(seq.Description, description))
                .Select(seq => seq.SubSequence)
                .SingleOrDefault();
        }

        public List<State<Descriptor, Symbol>> UserDefinedStartStates()
        {
            return StatesToAdd
                .Where(state => state.IsActiveAtStart)
                .Select(state => state.StateToBuild)
                .ToList();
        }

        public List<State<Descriptor, Symbol>> FinalStates()
        {
            return StatesToAdd
                .Where(state => state.IsFinal)
                .Select(state => state.StateToBuild)
                .ToList();
        }       
    }
}