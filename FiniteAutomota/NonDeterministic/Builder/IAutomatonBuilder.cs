namespace FiniteAutomota.NonDeterministic.Builder
{
    public interface IAutomatonBuilder<Descriptor, Symbol>
    {
        AutomatonBuilder<Descriptor, Symbol>.AddStateStep<Descriptor, Symbol> State(Descriptor description);
        AutomatonBuilder<Descriptor, Symbol>.AddTransitionStep<Descriptor, Symbol> Transition();
        Automaton<Descriptor, Symbol> Build(Descriptor description = default(Descriptor));
    }
}