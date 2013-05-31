using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security;
using System.Text;

namespace Omikron.FactFinder.Data
{
    public class Campaign
    {
        public string Name { get; private set; }
        public string Category { get; private set; }
        public string RedirectUrl { get; private set; }
        public List<Record> PushedProducts { get; private set; }
        public Dictionary<string, string> Feedback { get; private set; }
        public List<AdvisorQuestion> ActiveQuestions { get; private set; }
        public List<AdvisorQuestion> AdvisorTree { get; private set; }

        public Campaign(
            string name = "",
            string category = "",
            string redirectUrl = "",
            IList<Record> pushedProducts = null,
            IDictionary<string, string> feedback = null,
            IList<AdvisorQuestion> activeQuestions = null,
            IList<AdvisorQuestion> advisorTree = null
        )
        {
            Name = name;
            Category = category;
            RedirectUrl = redirectUrl;
            PushedProducts = new List<Record>();
            Feedback = new Dictionary<string, string>();
            ActiveQuestions = new List<AdvisorQuestion>();
            AdvisorTree = new List<AdvisorQuestion>();
            if (pushedProducts != null)
                AddPushedProducts(pushedProducts);
            if (feedback != null)
                AddFeedback(feedback);
            if (activeQuestions != null)
                AddActiveQuestions(activeQuestions);
            if (advisorTree != null)
                AddToAdvisorTree(advisorTree);
        }

        public void AddPushedProducts(IList<Record> pushedProducts)
        {
            PushedProducts.AddRange(pushedProducts);
        }

        public void AddFeedback(IDictionary<string, string> feedback)
        {
            foreach (var item in feedback)
            {
                if (item.Value != "")
                    Feedback.Add(item.Key, item.Value);
            }
        }

        public void AddActiveQuestions(IList<AdvisorQuestion> activeQuestions)
        {
            ActiveQuestions.AddRange(activeQuestions);
        }

        public void AddToAdvisorTree(IList<AdvisorQuestion> advisorTree)
        {
            AdvisorTree.AddRange(advisorTree);
        }

        public bool HasRedirect()
        {
            return RedirectUrl != "";
        }

        public bool HasPushedProducts()
        {
            return PushedProducts.Count > 0;
        }

        public bool HasFeedback(string label = null)
        {
            if (label == null)
            {
                return Feedback.Count > 0;
            }
            else
            {
                return Feedback.ContainsKey(label);
            }
        }

        public string GetFeedbackFor(string label)
        {
            if (Feedback.ContainsKey(label))
                return Feedback[label];
            else
                return "";
        }

        public bool HasActiveQuestions()
        {
            return ActiveQuestions.Count > 0;
        }

        public bool HasAdvisorTree()
        {
            return AdvisorTree.Count > 0;
        }

        /// <summary>
        /// Use this exception class to signal that a redirect 
        /// </summary>
        public class RedirectException : Exception
        {
            public RedirectException() : base() { }
        }
    }
}
