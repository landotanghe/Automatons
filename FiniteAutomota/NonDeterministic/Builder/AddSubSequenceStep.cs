namespace FiniteAutomota.NonDeterministic.Builder
{
    public class AddSubSequenceStep<Descriptor, Symbol>
    {
        internal AutomatonBuilder<Descriptor, Symbol> Builder;
        internal AutomatonBuilder<Descriptor, Symbol> SubSequenceToAdd;
        internal Descriptor Description;
    }
}
