namespace FiniteAutomota.NonDeterministic.Builder
{
    public class AddSubSequenceStep<Descriptor, Symbol>
    {
        internal IAutomatonBuilder<Descriptor, Symbol> SubSequenceBuilder;
        internal Descriptor Description;

        internal Automaton<Descriptor, Symbol> SubSequence => SubSequenceBuilder.Build();
    }
}
