namespace FiniteAutomota.NonDeterministic.Builder
{
    public partial class AutomatonBuilder<Descriptor, Symbol>
    {
        public class AddTransitionStep<Descriptor, Symbol>
        {
            public AutomatonBuilder<Descriptor, Symbol> Builder;
            public Descriptor Source { get; private set; }
            public Descriptor Target { get; private set; }

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

                public IAutomatonBuilder<Descriptor, Symbol> To(Descriptor target)
                {
                    TransitionStep.Target = target;

                    return TransitionStep.Builder;
                }                
            }
        }        
    }
}
