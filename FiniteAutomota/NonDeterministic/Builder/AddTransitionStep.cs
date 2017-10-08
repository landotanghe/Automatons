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
            var source = StatesDefined.FindState(Source);
            foreach (var targetDescriptor in (Targets))
            {
                var target = StatesDefined.FindState(targetDescriptor);
                if (IsEpsilon)
                    source.AddEpsilonTransition(target);
                    else
                        source.AddTransition(Trigger, target);
            }
        }    
    }
}
