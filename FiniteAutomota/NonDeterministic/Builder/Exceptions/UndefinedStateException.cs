using System;

namespace FiniteAutomota.NonDeterministic.Builder.Exceptions
{
    public class UndefinedStateException : Exception
    {
        public UndefinedStateException(object definition) : base($"State '{definition}' not known, add a state with this definition before adding a transition for it")
        {
        }
    }
}
