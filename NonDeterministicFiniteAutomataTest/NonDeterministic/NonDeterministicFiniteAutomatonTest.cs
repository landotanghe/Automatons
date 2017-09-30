using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using FiniteAutomota.NonDeterministic.Builder;
using FiniteAutomota.NonDeterministic.Builder.Exceptions;

namespace FiniteAutomata.NonDeterministic.Test
{
    [TestClass]
    public class NonDeterministicFiniteAutomatonTest
    {
        private const string Start = "start";
        private const string Source = "source";
        private const string Target = "target";    

        [TestMethod]
        public void CreatedAutomaton_StartStateActive()
        {
            var automaton = new AutomatonBuilder()
                .State(Start).ActiveAtStart()
                .Build();

            var currentState = automaton.GetActiveStates();
            Assert.AreEqual(1, currentState.Count());
            Assert.AreEqual(Start, currentState.ElementAt(0).Description);
        }
        
        [TestMethod]
        [ExpectedException(typeof(UndefinedStateException))]
        public void CreatedAutomaton_AddTransitionForUnknownTarget_UndefinedStateExceptionThrown()
        {
            var automaton = new AutomatonBuilder()
                .State(Start).ActiveAtStart()
                .Transition().OnEpsilon().From(Start).To(Target)
                .Build();
        }

        [TestMethod]
        public void CreatedAutomaton_StartStateHasNoEpsilonTransition_OnlyStartStateActive()
        {
            var automaton = new AutomatonBuilder()
                .State(Start).ActiveAtStart()
                .State(Source)
                .State(Target)
                .Transition().OnEpsilon().From(Source).To(Target)
                .Build();

            var currentState = automaton.GetActiveStates();
            Assert.AreEqual(1, currentState.Count());
            Assert.AreEqual(Start, currentState.ElementAt(0).Description);
        }

        [TestMethod]
        public void CreatedAutomaton_StartStateHasEpsilonTransition_BothStatesActive()
        {
            var automaton = new AutomatonBuilder()
                .State(Start).ActiveAtStart()
                .State(Target)
                .Transition().OnEpsilon().From(Start).To(Target)
                .Build();
            
            var currentState = automaton.GetActiveStates();
            Assert.AreEqual(2, currentState.Count());
            Assert.AreEqual(Start, currentState.ElementAt(0).Description);
            Assert.AreEqual(Target, currentState.ElementAt(1).Description);
        }
    }
}
