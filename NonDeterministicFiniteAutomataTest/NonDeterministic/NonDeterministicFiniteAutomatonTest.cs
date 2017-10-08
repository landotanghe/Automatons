using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using FiniteAutomota.NonDeterministic.Builder;
using FiniteAutomota.NonDeterministic.Builder.Exceptions;
using FiniteAutomota.NonDeterministic.Closure;

namespace FiniteAutomata.Test.NonDeterministic
{
    [TestClass]
    public class NonDeterministicFiniteAutomatonTest
    {
        private const string Subsequence = "Subsequence";
        private const string Start = "start";
        private const string Source1 = "source1";
        private const string Source2 = "source2";
        private const string Target1 = "target1";
        private const string Target2 = "target2";
        private const string Closure = "closure";
        private const char SymbolA = 'a';
        private const char SymbolB = 'b';
        
        [TestInitialize]
        public void TestInitialize()
        {
            CreateBuilder();
        }

        private AutomatonBuilder CreateBuilder()
        {
            return new AutomatonBuilder(new ClosureCalculator());
        }

        [TestMethod]
        public void CreatedAutomaton_WithMultipleStates_StartStateActive()
        {
            var automaton = CreateBuilder()
                .State(Start).ActiveAtStart()
                .State(Source1)
                .Build();

            var currentState = automaton.GetActiveStates();
            Assert.AreEqual(1, currentState.Count());
            Assert.AreEqual(Start, currentState.ElementAt(0).Description);
        }
        
        [TestMethod]
        [ExpectedException(typeof(AtLeastOneStartStateRequiredException))]
        public void CreatedAutomaton_NoStartStateSet_ThrowsNoStartStateException_OnBuild()
        {
            var automaton = CreateBuilder()
                .State(Source1)
                .State(Target1)
                .Transition().OnEpsilon().From(Source1).To(Target1)
                .Build();
        }

        [TestMethod]
        public void CreatedAutomaton_StartStateHasNoEpsilonTransition_OnlyStartStateActive()
        {
            var automaton = CreateBuilder()
                .State(Start).ActiveAtStart()
                .State(Source1)
                .State(Target1)
                .Transition().OnEpsilon().From(Source1).To(Target1)
                .Build();

            var currentState = automaton.GetActiveStates();
            Assert.AreEqual(1, currentState.Count());
            Assert.AreEqual(Start, currentState.ElementAt(0).Description);
        }

        [TestMethod]
        public void CreatedAutomaton_StartStateHasEpsilonTransition_BothStatesActive()
        {
            var automaton = CreateBuilder()
                .State(Start).ActiveAtStart()
                .State(Target1)
                .Transition().OnEpsilon().From(Start).To(Target1)
                .Build();
            
            var currentState = automaton.GetActiveStates();
            Assert.AreEqual(2, currentState.Count());
            Assert.AreEqual(Start, currentState.ElementAt(0).Description);
            Assert.AreEqual(Target1, currentState.ElementAt(1).Description);
        }
        
        [TestMethod]
        [ExpectedException(typeof(UndefinedStateException))]
        public void CreatedAutomaton_AddEpsilonTransitionForUnknownTarget_UndefinedStateExceptionThrown()
        {
            var automaton = CreateBuilder()
                .State(Start).ActiveAtStart()
                .Transition().OnEpsilon().From(Start).To(Target1)
                .Build();
        }

        [TestMethod]
        [ExpectedException(typeof(UndefinedStateException))]
        public void CreatedAutomaton_AddSymbolTransitionForUnknownTarget_UndefinedStateExceptionThrown()
        {
            var automaton = CreateBuilder()
                .State(Start).ActiveAtStart()
                .Transition().On(SymbolA).From(Start).To(Target1)
                .Build();
        }

        [TestMethod]
        public void CreatedAutomaton_AddSymbolTransition_StartStateDoesNotIncludeTarget()
        {
            var automaton = CreateBuilder()
                .State(Start).ActiveAtStart()
                .State(Target1)
                .Transition().On(SymbolA).From(Start).To(Target1)
                .Build();

            var currentState = automaton.GetActiveStates();
            Assert.AreEqual(1, currentState.Count());
            Assert.AreEqual(Start, currentState.ElementAt(0).Description);
        }
        
        [TestMethod]
        public void Automaton_WithSymbolTransition_OnProcessingThatSymbol_StateTransitionsToTargets()
        {
            var automaton = CreateBuilder()
                .State(Start).ActiveAtStart()
                .State(Target1)
                .State(Target2)
                .Transition().On(SymbolA).From(Start).To(Target1, Target2)
                .Build();

            automaton.Process(SymbolA);

            var currentState = automaton.GetActiveStates().Select(state => state.Description);
            Assert.AreEqual(2, currentState.Count());
            Assert.IsTrue(currentState.Contains(Target1));
            Assert.IsTrue(currentState.Contains(Target2));
        }


        [TestMethod]
        public void Automaton_WithSymbolTransition_TargetHasEpsilonTransition_OnProcessingThatSymbol_StateTransitionsToBothTargets()
        {
            var automaton = CreateBuilder()
                .State(Start).ActiveAtStart()
                .State(Target1)
                .State(Target2)
                .Transition().On(SymbolA).From(Start).To(Target1)
                .Transition().OnEpsilon().From(Target1).To(Target2)
                .Build();

            automaton.Process(SymbolA);

            var currentState = automaton.GetActiveStates().Select(state => state.Description);
            Assert.AreEqual(2, currentState.Count());
            Assert.IsTrue(currentState.Contains(Target1));
            Assert.IsTrue(currentState.Contains(Target2));
        }

        [TestMethod]
        public void Automaton_IsAcceptedWhenReached_AtleastOneFinalState()
        {
            var automaton = CreateBuilder()
                .State(Start).ActiveAtStart()
                .State(Target1).Final()
                .State(Target2).Final()
                .Transition().On('a').From(Start).To(Target1)
                .Build();
            
            Assert.IsFalse(automaton.IsAccepted());
            automaton.Process('a');
            Assert.IsTrue(automaton.IsAccepted());
        }


        [TestMethod]
        public void CreatedAutomaton_AddSubsequences()
        {
            var subSequence = CreateBuilder()
                .State(Start).ActiveAtStart()
                .State(Target1).Final()
                .Transition().On('b').From(Start).To(Target1);

            var automaton = CreateBuilder()
                .State(Start).ActiveAtStart()
                .State(Target1).Final()
                .SubSequence(subSequence, Subsequence)
                .Transition().On('a').From(Start).To(Subsequence)
                .Transition().On('c').From(Subsequence).To(Target1)
                .Build();

            Assert.IsFalse(automaton.IsAccepted());
            automaton.Process('a');
            Assert.IsFalse(automaton.IsAccepted());
            automaton.Process('b');
            Assert.IsFalse(automaton.IsAccepted());
            automaton.Process('c');
            Assert.IsTrue(automaton.IsAccepted());
        }
    }
}
