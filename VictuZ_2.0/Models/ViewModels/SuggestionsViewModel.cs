using VictuZ_2._0.Models.Suggestions;

namespace VictuZ_2._0.ViewModels
{
    public class SuggestionsViewModel
    {
        public List<Suggestion> TopSuggestions { get; set; }
        public List<Suggestion> TrendingSuggestions { get; set; }
        public List<Suggestion> AllSuggestions { get; set; }

        public SuggestionsViewModel()
        {
            TopSuggestions = new List<Suggestion>();
            TrendingSuggestions = new List<Suggestion>();
            AllSuggestions = new List<Suggestion>();
        }
    }
}

