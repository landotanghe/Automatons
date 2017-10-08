using FiniteAutomota.NonDeterministic.Builder.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FiniteAutomota.NonDeterministic.Builder
{
    public class AddTransitionStep<Descriptor, Symbol>
    {
        public AutomatonBuilder<Descriptor, Symbol> Builder;
        public Descriptor Source { get; private set; }
        public Descriptor[] Targets { get; private set; }

        public Symbol Trigger { get; set; }
        public bool IsEpsilon { get; set; }

        public AddTransitionStep(AutomatonBuilder<Descriptor, Symbol> builder){
            Builder = builder;
        }

        public TransitionSourceStep<Descriptor, Symbol> OnEpsilon()
        {
            IsEpsilon = true;
            return new TransitionSourceStep<Descriptor, Symbol>
            {
                TransitionStep = this
            };
        }

        public TransitionSourceStep<Descriptor, Symbol> On(Symbol symbol)
        { 
            IsEpsilon = false;
            Trigger = symbol;
            return new TransitionSourceStep<Descriptor, Symbol>
            {
                TransitionStep = this
            };
        }

        public class TransitionSourceStep<Descriptor, Symbol>
        {
            public AddTransitionStep<Descriptor, Symbol> TransitionStep { get; internal set; }

            public TransitionTargetStep<Descriptor, Symbol> From(Descriptor source)
            {
                TransitionStep.Source = source;

                return new TransitionTargetStep<Descriptor, Symbol>
                {
                    TransitionStep = TransitionStep
                };
            }
        }

        public class TransitionTargetStep<Descriptor, Symbol>
        {
            public AddTransitionStep<Descriptor, Symbol> TransitionStep { get; internal set; }

            public IAutomatonBuilder<Descriptor, Symbol> To(params Descriptor[] target)
            {
                TransitionStep.Targets = target;
                return TransitionStep.Builder;
            }                
        }

        public void AddToSource(StateDefintionsManager<Descriptor, Symbol> StatesDefined)
        {
            var sources = CollectSourceStates(StatesDefined);

            foreach(var source in sources)
            {
                foreach (var targetDescriptor in (Targets))
                {
                    var targets = CollecTargetStates(StatesDefined, targetDescriptor);
                    foreach(var target in targets)
                    {
                        if (IsEpsilon)
                            source.AddEpsilonTransition(target);
                        else
                            source.AddTransition(Trigger, target);
                    }
                }
            }
        }

        private List<State<Descriptor, Symbol>> CollectSourceStates(StateDefintionsManager<Descriptor, Symbol> StatesDefined)
        {
            var sources = new List<State<Descriptor, Symbol>>();

            var source = StatesDefined.FindStateOrDefault(Source);
            if (source == null)
            {
                var subsequence = StatesDefined.FindSubSequenceOrDefault(Source);
                if (subsequence == null)
                    throw new UndefinedStateException(Source);
                sources = subsequence.FinalStates;
            }
            else
            {
                sources.Add(source);
            }

            return sources;
        }

        private List<State<Descriptor, Symbol>> CollecTargetStates(StateDefintionsManager<Descriptor, Symbol> StatesDefined, Descriptor targetDescriptor)
        {
            var targets = new List<State<Descriptor, Symbol>>();

            var target = StatesDefined.FindStateOrDefault(targetDescriptor);
            if (target == null)
            {
                var subsequence = StatesDefined.FindSubSequenceOrDefault(targetDescriptor);
                if (subsequence == null)
                    throw new UndefinedStateException(targetDescriptor);
                targets = subsequence.StartStates;
            }
            else
            {
                targets.Add(target);
            }

            return targets;
        }
    }
}
