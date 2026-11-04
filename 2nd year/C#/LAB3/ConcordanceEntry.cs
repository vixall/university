using System.Collections.Generic;

namespace TextAnalysis.Model
{
    //LAB_4
    public class ConcordanceEntry
    {
        public int Count { get; set; }
        public SortedSet<int> LineNumbers { get; } = new SortedSet<int>();
    }
    // end
}