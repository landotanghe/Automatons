namespace FiniteAutomota.NonDeterministic.Builder
{
    public interface IAutomatonBuilder<Descriptor, Symbol>
    {
        AddStateStep<Descriptor, Symbol> State(Descriptor description);
        IAutomatonBuilder<Descriptor, Symbol> SubSequence(IAutomatonBuilder<Descriptor, Symbol> subSequence, Descriptor description);
        AddTransitionStep<Descriptor, Symbol> Transition();
        Automaton<Descriptor, Symbol> Build();
    }
}