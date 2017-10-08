using FiniteAutomota.NonDeterministic;
using FiniteAutomota.NonDeterministic.Closure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace FiniteAutomata.Test.NonDeterministic
{
    [TestClass]
    public class ClosureCalculatorTest
    {
        private ClosureCalculator _sut;
        // add more tests and fix implementation using TDD

        [TestInitialize]
        public void TestInitialize()
        {
            _sut = new ClosureCalculator();
        }

        [TestMethod]
        public void NoEpsilonTransitions_Closure_EqualsInput()
        {
            var a = new State("a");
            var b = new State("b");
            var c = new State("c");
            b.AddTransition('x', c);

            var closure = _sut.GetClosureFor(new List<State> { a, b });

            Assert.AreEqual(2, closure.Count);
            Assert.IsTrue(closure.Contains(a));
            Assert.IsTrue(closure.Contains(b));
        }


        [TestMethod]
        public void EpsilonTransitionsPointTowardsStartState_Closure_EqualsInput()
        {
            var a = new State("a");
            var b = new State("b");
            var c = new State("c");
            c.AddEpsilonTransition(a);
            c.AddEpsilonTransition(b);

            var closure = _sut.GetClosureFor(new List<State> { a, b });

            Assert.AreEqual(2, closure.Count);
            Assert.IsTrue(closure.Contains(a));
            Assert.IsTrue(closure.Contains(b));
        }


        [TestMethod]
        public void EpsilonTransitionFromStartState_Closure_ContainsTargetAndOriginalStartState()
        {
            var a = new State("a");
            var b = new State("b");
            var c = new State("c");
            a.AddEpsilonTransition(b);

            var closure = _sut.GetClosureFor(new List<State> { a });

            Assert.AreEqual(2, closure.Count);
            Assert.IsTrue(closure.Contains(a));
            Assert.IsTrue(closure.Contains(b));
        }

        [TestMethod]
        public void EpsilonTransitionFromStartState_Indirectly_ToOtherStartState_Closure_StatesAddedOnlyOnce()
        {
            var a = new State("a");
            var b = new State("b");
            var c = new State("c");
            a.AddEpsilonTransition(b);
            b.AddEpsilonTransition(c);

            var closure = _sut.GetClosureFor(new List<State> { a, c });

            Assert.AreEqual(3, closure.Count);
            Assert.IsTrue(closure.Contains(a));
            Assert.IsTrue(closure.Contains(b));
            Assert.IsTrue(closure.Contains(c));
        }

        [TestMethod]
        public void MultipleEpsilonPathsToSameState_Closure_StatesAddedOnlyOnce()
        {
            var a = new State("a");
            var b = new State("b");
            var c = new State("c");
            a.AddEpsilonTransition(c);
            a.AddEpsilonTransition(b);
            b.AddEpsilonTransition(c);

            var closure = _sut.GetClosureFor(new List<State> { a });

            Assert.AreEqual(3, closure.Count);
            Assert.IsTrue(closure.Contains(a));
            Assert.IsTrue(closure.Contains(b));
            Assert.IsTrue(closure.Contains(c));
        }

        [TestMethod]
        public void EpsilonTransitionChain_Closure_ContainsAllStatesInEpsilonChain()
        {
            var a = new State("a");
            var b = new State("b");
            var c = new State("c");
            var d = new State("d");
            var e = new State("e");
            a.AddEpsilonTransition(b);
            b.AddEpsilonTransition(c);
            c.AddEpsilonTransition(d);
            d.AddEpsilonTransition(e);

            var closure = _sut.GetClosureFor(new List<State> { a });

            Assert.AreEqual(5, closure.Count);
            Assert.IsTrue(closure.Contains(a));
            Assert.IsTrue(closure.Contains(b));
            Assert.IsTrue(closure.Contains(c));
            Assert.IsTrue(closure.Contains(d));
            Assert.IsTrue(closure.Contains(e));
        }
    }
}