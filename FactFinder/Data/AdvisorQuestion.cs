using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Omikron.FactFinder.Data
{
    public class AdvisorQuestion
    {
        public string Text { get; private set; }
        public List<AdvisorAnswer> Answers { get; private set; }

        public AdvisorQuestion(string text = "", IEnumerable<AdvisorAnswer> answers = null)
        {
            Text = text.Trim();
            Answers = new List<AdvisorAnswer>();
            if (answers != null)
                AddAnswers(answers);
        }

        public void AddAnswers(IEnumerable<AdvisorAnswer> answers)
        {
            Answers.AddRange(answers);
        }

        public bool HasAnswers()
        {
            return Answers.Count > 0;
        }
    }
}
