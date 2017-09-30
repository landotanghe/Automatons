using System;

namespace FiniteAutomota.NonDeterministic.Builder
{
    public partial class AutomatonBuilder<Descriptor, Symbol>
    {
        public partial class AddStateStep<Descriptor, Symbol>
        {
            public AutomatonBuilder<Descriptor, Symbol> Builder;
            public State<Descriptor, Symbol> StateToBuild;
            public bool IsActiveAtStart = false;

            public AddStateStep(Descriptor description, AutomatonBuilder<Descriptor, Symbol> builder)
            {
                StateToBuild = new State<Descriptor, Symbol>(description);
                Builder = builder;
            }

            public AddStateStep<Descriptor, Symbol> ActiveAtStart()
            {
                IsActiveAtStart = true;
                return this;
            }
        }

        public partial class AddStateStep<Descriptor, Symbol> : IAutomatonBuilder<Descriptor, Symbol>
        {
            public AutomatonBuilder<Descriptor, Symbol>.AddStateStep<Descriptor, Symbol> State(Descriptor description)
            {
                return Builder.State(description);
            }

            public AutomatonBuilder<Descriptor, Symbol>.AddTransitionStep<Descriptor, Symbol> Transition()
            {
                return Builder.Transition();
            }

            public Automaton<Descriptor, Symbol> Build()
            {
                return Builder.Build();
            }
        }
    }
}
