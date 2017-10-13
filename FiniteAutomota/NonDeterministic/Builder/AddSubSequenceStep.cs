namespace FiniteAutomota.NonDeterministic.Builder
{
    public class AddSubSequenceStep<Descriptor, Symbol>
    {
        internal Descriptor Description;
        internal Automaton<Descriptor, Symbol> SubSequence { get; private set; }
        internal IAutomatonBuilder<Descriptor, Symbol> SubSequenceBuilder;

        internal void Reset()
        {
            SubSequence = SubSequenceBuilder.Build();
        }
    }
}
