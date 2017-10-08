namespace FiniteAutomota.NonDeterministic.Builder
{
    public interface IAutomatonBuilder<Descriptor, Symbol>
    {
        AddStateStep<Descriptor, Symbol> State(Descriptor description);
        AddTransitionStep<Descriptor, Symbol> Transition();
        Automaton<Descriptor, Symbol> Build(Descriptor description = default(Descriptor));
    }
}