using System.Collections.Generic;

namespace Omikron.FactFinder.Data
{
    public class AdvisorAnswer
    {
        public string Text { get; private set; }
        public string Parameters { get; private set; }
        public List<AdvisorQuestion> SubQuestions { get; private set; }

        public AdvisorAnswer(
            string text = "",
            string parameters = "",
            IEnumerable<AdvisorQuestion> subQuestions = null
            )
        {
            Text = text.Trim();
            Parameters = parameters;
            SubQuestions = new List<AdvisorQuestion>();
            if (subQuestions != null)
                AddSubQuestions(subQuestions);
        }

        public void AddSubQuestions(IEnumerable<AdvisorQuestion> subQuestions)
        {
            SubQuestions.AddRange(subQuestions);
        }

        public bool HasSubQuestions()
        {
            return SubQuestions.Count > 0;
        }
    }
}
