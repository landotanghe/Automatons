using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using FiniteAutomota.NonDeterministic.Builder;
using FiniteAutomota.NonDeterministic.Builder.Exceptions;
using FiniteAutomota.NonDeterministic.Closure;
using FakeItEasy;
using FiniteAutomota.NonDeterministic;
using System.Collections.Generic;

namespace FiniteAutomata.Test.NonDeterministic
{
    [TestClass]
    public class NonDeterministicFiniteAutomatonBuilderTest
    {
        private const string Start = "start";
        private const string Source = "source";
        private const string Target = "target";

        private const string Node1 = "node1";
        private const string Node2 = "node2";
        private const string Node3 = "node3";
        private const string Node4 = "node4";

        private const string Closure = "closure";

        public IClosureCalculator _closureCalculator;
        public AutomatonBuilder _sutWithFakes;
        public AutomatonBuilder _sutWithRealImplementors;

        [TestInitialize]
        public void TestInitialize()
        {
            _closureCalculator = A.Fake<IClosureCalculator>();
            var fakeClosure = new List<State<string, char>> {
                new State<string, char>(Closure)
            };
            var call = A.CallTo(() => _closureCalculator.GetClosureFor<string, char>(A<IEnumerable<State>>._))
                        .Returns(fakeClosure);

            _sutWithFakes = new AutomatonBuilder(_closureCalculator);
            _sutWithRealImplementors = new AutomatonBuilder(new ClosureCalculator());
        }

        [TestMethod]
        public void CreatedAutomaton_WithMultipleStates_StartStateActive()
        {
            var automaton = _sutWithRealImplementors
                .State(Start).ActiveAtStart()
                .State(Source)
                .Build();

            var currentState = automaton.GetActiveStates();
            Assert.AreEqual(1, currentState.Count());
            Assert.AreEqual(Start, currentState.ElementAt(0).Description);
        }

        //[TestMethod]
        //public void CreatedAutomaton_ClosureForStartStateActive()
        //{
        //    var automaton = _sutWithFakes
        //        .State(Start).ActiveAtStart()
        //        .Build();

        //    var closrureWtf = _closureCalculator.GetClosureFor(new List<State> { new State("aaa") });

        //    var currentState = automaton.GetActiveStates();
        //    Assert.AreEqual(1, currentState.Count());
        //    Assert.AreEqual(Closure, currentState.ElementAt(0).Description);
        //}

        [TestMethod]
        [ExpectedException(typeof(AtLeastOneStartStateRequiredException))]
        public void CreatedAutomaton_NoStartStateSet_ThrowsNoStartStateException_OnBuild()
        {
            var automaton = _sutWithRealImplementors
                .State(Source)
                .State(Target)
                .Transition().OnEpsilon().From(Source).To(Target)
                .Build();
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

        [TestMethod]
        public void AutomatonNotInFinalState_IsNotAccepted()
        {
            var builder = new AutomatonBuilder()
                .State(Start).ActiveAtStart()
                .Transition().On('x').From(Start).To(Target)
                .State(Target);

            var automaton1 = builder.Build();

            automaton1.Process('x');

            Assert.AreEqual(false, automaton1.IsAccepted());
        }

        [TestMethod]
        public void AutomatonInFinalState_IsAccepted()
        {
            var builder = new AutomatonBuilder()
                .State(Start).ActiveAtStart()
                .Transition().On('x').From(Start).To(Target)
                .State(Target).Final();

            var automaton1 = builder.Build();

            automaton1.Process('x');

            Assert.AreEqual(true, automaton1.IsAccepted());
        }

        [TestMethod]
        public void BuildMultiple_DifferentStates()
        {
            var builder = new AutomatonBuilder()
                .State(Start).ActiveAtStart()
                .Transition().On('x').From(Start).To(Target)
                .State(Target).Final();

            var automaton1 = builder.Build();
            var automaton2 = builder.Build();

            automaton1.Process('x');

            Assert.AreEqual(true, automaton1.IsAccepted());
            Assert.AreEqual(false, automaton2.IsAccepted());
        }



        //[TestMethod]
        //public void Build_Subsequence_SimplifiedToHaveNoEpsilons()
        //{
        //    var subsequence = new AutomatonBuilder()
        //        .State(Start).ActiveAtStart()
        //        .Transition().On('x').From(Start).To(Target)
        //        .State(Target).Final();

        //    var automaton = new AutomatonBuilder()
        //        .State(Start).ActiveAtStart()
        //        .SubSequence(subsequence, "subseq")
        //        .Transition().On('x').From(Start).To("subseq")
        //        .Transition().On('x').From("subseq").To(Target)
        //        .State(Target).Build();

        //    Assert.AreEqual(1, automaton.StartStates);
        //    Assert.AreEqual(1, automaton.GetActiveStates().Count());
        //    Assert.AreEqual(1, automaton.FinalStates);
            
        //    automaton.Process('x');
        //    Assert.AreEqual(1, automaton.GetActiveStates().Count());
        //}
        
        //[TestMethod]
        //public void Build_MultipleEpsilons_SimplifiedToHaveNoEpsilons()
        //{
        //    var automaton = new AutomatonBuilder()
        //        .State(Start).ActiveAtStart()
        //        .State(Node1)
        //        .State(Node2)
        //        .State(Node3)
        //        .State(Node4)
        //        .Transition().OnEpsilon().From(Start).To(Node1)
        //        .Transition().OnEpsilon().From(Node1).To(Node2)
        //        .Transition().On('x').From(Node2).To(Node3)
        //        .Transition().OnEpsilon().From(Node3).To(Node4)
        //        .Transition().OnEpsilon().From(Node4).To(Target)
        //        .State(Target).Build();

        //    Assert.AreEqual(1, automaton.StartStates);
        //    Assert.AreEqual(1, automaton.GetActiveStates().Count());
        //    Assert.AreEqual(1, automaton.FinalStates);

        //    automaton.Process('x');
        //    Assert.AreEqual(1, automaton.GetActiveStates().Count());
        //}
    }
}
