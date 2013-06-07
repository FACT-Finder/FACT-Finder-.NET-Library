using System;
using System.Collections.Generic;

namespace Omikron.FactFinder.Data
{
    public class CampaignList : List<Campaign>
    {
        public CampaignList()
            : base()
        { }

        public CampaignList(int capacity)
            : base(capacity)
        { }

        public CampaignList(IEnumerable<Campaign> collection)
            : base(collection)
        { }

        public bool HasRedirect()
        {
            foreach (var campaign in this)
                if (campaign.HasRedirect())
                    return true;

            return false;
        }

        public Uri GetRedirectUrl()
        {
            foreach (var campaign in this)
                if (campaign.HasRedirect())
                    return campaign.RedirectUrl;

            return null;
        }

        public bool HasPushedProducts()
        {
            foreach (var campaign in this)
                if (campaign.HasPushedProducts())
                    return true;

            return false;
        }

        public IList<Record> GetPushedProducts()
        {
            var pushedProducts = new List<Record>();
            foreach (var campaign in this)
                if (campaign.HasPushedProducts())
                    pushedProducts.AddRange(campaign.PushedProducts);

            return pushedProducts;
        }

        public bool HasFeedback()
        {
            foreach (var campaign in this)
                if (campaign.HasFeedback())
                    return true;

            return false;
        }

        public string GetFeedbackFor(string label)
        {
            string feedback = "";
            foreach (var campaign in this)
                if (campaign.HasFeedback(label))
                    feedback += campaign.GetFeedbackFor(label) + System.Environment.NewLine;

            return feedback;
        }

        public bool HasActiveQuestions()
        {
            foreach (var campaign in this)
                if (campaign.HasActiveQuestions())
                    return true;

            return false;
        }

        public IList<AdvisorQuestion> GetActiveQuestions()
        {
            var activeQuestions = new List<AdvisorQuestion>();
            foreach (var campaign in this)
                if (campaign.HasActiveQuestions())
                    activeQuestions.AddRange(campaign.ActiveQuestions);

            return activeQuestions;
        }

        public bool HasAdvisorTree()
        {
            foreach (var campaign in this)
                if (campaign.HasAdvisorTree())
                    return true;

            return false;
        }

        public IList<AdvisorQuestion> GetAdvisorTree()
        {
            var advisorTree = new List<AdvisorQuestion>();
            foreach (var campaign in this)
                if (campaign.HasAdvisorTree())
                    advisorTree.AddRange(campaign.AdvisorTree);

            return advisorTree;
        }
    }
}
