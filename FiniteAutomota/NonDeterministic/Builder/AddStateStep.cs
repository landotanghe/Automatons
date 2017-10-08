namespace FiniteAutomota.NonDeterministic.Builder
{
    public class AddStateStep<Descriptor, Symbol> : IAutomatonBuilder<Descriptor, Symbol>
    {
        internal AutomatonBuilder<Descriptor, Symbol> Builder;
        internal State<Descriptor, Symbol> StateToBuild;
        internal bool IsActiveAtStart = false;
        internal bool IsFinal = false;

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

        public AddStateStep<Descriptor, Symbol> Final()
        {
            IsFinal = true;
            return this;
        }

        public AddStateStep<Descriptor, Symbol> State(Descriptor description)
        {
            return Builder.State(description);
        }

        public AddTransitionStep<Descriptor, Symbol> Transition()
        {
            return Builder.Transition();
        }

        public Automaton<Descriptor, Symbol> Build()
        {
            return Builder.Build();
        }

        public IAutomatonBuilder<Descriptor, Symbol> SubSequence(IAutomatonBuilder<Descriptor, Symbol> subSequence, Descriptor description)
        {
            return Builder.SubSequence(subSequence, description);
        }
    }
}
