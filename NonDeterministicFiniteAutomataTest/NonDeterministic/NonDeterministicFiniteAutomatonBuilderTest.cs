using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using FiniteAutomota.NonDeterministic.Builder;
using FiniteAutomota.NonDeterministic.Builder.Exceptions;
using FiniteAutomota.NonDeterministic.Closure;
using FakeItEasy;

namespace FiniteAutomata.NonDeterministic.Test
{
    [TestClass]
    public class NonDeterministicFiniteAutomatonBuilderTest
    {
        private const string Start = "start";
        private const string Source = "source";
        private const string Target = "target";
        private const string Closure = "closure";

        public IClosureCalculator _closureCalculator;
        public AutomatonBuilder _sutWithFakes;
        public AutomatonBuilder _sutWithRealImplementors;

        [TestInitialize]
        public void TestInitialize()
        {
            _closureCalculator = A.Fake<IClosureCalculator>();
            _sutWithFakes = new AutomatonBuilder(_closureCalculator);
            _sutWithRealImplementors = new AutomatonBuilder(new ClosureCalculator());
        } 

        [TestMethod]
        public void CreatedAutomaton_ClosureForStartStateActive()
        {
            var automaton = _sutWithRealImplementors
                .State(Start).ActiveAtStart()
                .Build();

            var currentState = automaton.GetActiveStates();
            Assert.AreEqual(1, currentState.Count());
            Assert.AreEqual(Start, currentState.ElementAt(0).Description);
        }

        [TestMethod]
        public void CreatedAutomaton_StartStateHasNoEpsilonTransition_OnlyStartStateActive()
        {
            var automaton = _sutWithRealImplementors
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
            var automaton = _sutWithRealImplementors
                .State(Start).ActiveAtStart()
                .State(Target)
                .Transition().OnEpsilon().From(Start).To(Target)
                .Build();
            
            var currentState = automaton.GetActiveStates();
            Assert.AreEqual(2, currentState.Count());
            Assert.AreEqual(Start, currentState.ElementAt(0).Description);
            Assert.AreEqual(Target, currentState.ElementAt(1).Description);
        }
        
        [TestMethod]
        [ExpectedException(typeof(UndefinedStateException))]
        public void CreatedAutomaton_AddTransitionForUnknownTarget_UndefinedStateExceptionThrown()
        {
            var automaton = _sutWithFakes
                .State(Start).ActiveAtStart()
                .Transition().OnEpsilon().From(Start).To(Target)
                .Build();
        }

    }
}
