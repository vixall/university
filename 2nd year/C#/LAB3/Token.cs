namespace TextAnalysis.Tokens
{
    public abstract class Token 
        // задает с помощью полиморфизма Word и Punctuation,
        // через него Sentense хранит разнородные токены в одном списке List<Token>
        // под "разнородные" имеется ввиду, что программа не знает что такое Word или Punctuation (определяя их только по Token)
        // так же на нем завязаны сортировки, подсчет, замена и XML

    {
        public abstract string Value { get; }
        public int Length => Value.Length;
    }
}