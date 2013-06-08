using System;
using System.Collections.Generic;
using log4net;
using Omikron.FactFinder.Data;

namespace Omikron.FactFinder.Json.FF67
{
    public class JsonSearchAdapter : Omikron.FactFinder.Json.FF66.JsonSearchAdapter
    {
        private static ILog log;

        static JsonSearchAdapter()
        {
            log = LogManager.GetLogger(typeof(JsonSearchAdapter));
        }

        public JsonSearchAdapter(DataProvider dataProvider, ParametersHandler parametersHandler)
            : base(dataProvider, parametersHandler)
        { }

        protected override CampaignList CreateCampaigns()
        {
            var campaigns = new List<Campaign>();

            if (JsonData.searchResult.ContainsKey("campaignsList"))
            {
                foreach (var campaignData in JsonData.searchResult.campaignsList)
                {
                    var campaign = CreateEmptyCampaignObject(campaignData);

                    FillCampaignObject(campaign, campaignData);

                    campaigns.Add(campaign);
                }
            }

            return new CampaignList(campaigns);
        }

        protected override Campaign CreateEmptyCampaignObject(dynamic campaignData)
        {
            return new Campaign(
                (string)campaignData.name,
                "",
                new Uri((string)campaignData.target.destination, UriKind.RelativeOrAbsolute)
            );
        }

        protected override void FillCampaignObject(Campaign campaign, dynamic campaignData)
        {
            switch((string)campaignData.flavour)
            {
            case "FEEDBACK":
                FillCampaignWithFeedback(campaign, campaignData);
                FillCampaignWithPushedProducts(campaign, campaignData);
                break;

            case "ADVISOR":
                FillCampaignWithAdvisorData(campaign, campaignData);
                break;
            }
        }

        protected override void FillCampaignWithFeedback(Campaign campaign, dynamic campaignData)
        {
            if (campaignData.feedbackTexts.Count > 0)
            {
                var feedback = new Dictionary<string, string>();

                foreach (var feedbackData in campaignData.feedbackTexts)
                {
                    string label = feedbackData.label.ToString();
                    if (label != "")
                        feedback[label] = (string)feedbackData.text;

                    string id = feedbackData.id.ToString();
                    if (id != "")
                        feedback[id] = (string)feedbackData.text;
                }

                campaign.AddFeedback(feedback);
            }
        }

        protected override void FillCampaignWithPushedProducts(Campaign campaign, dynamic campaignData)
        {
            if (campaignData.pushedProductsRecordList.Count > 0)
            {
                var pushedProducts = new List<Record>();

                foreach (var recordData in campaignData.pushedProductsRecordList)
                {
                    var record = new Record((string)recordData.id);
                    record.SetFieldValues(recordData.record.AsDictionary());
                    pushedProducts.Add(record);
                }

                campaign.AddPushedProducts(pushedProducts);
            }
        }

        protected virtual void FillCampaignWithAdvisorData(Campaign campaign, dynamic campaignData)
        {
            var activeQuestions = new List<AdvisorQuestion>();

            // The active questions can still be empty if we have already moved down the whole question tree (while the search query still fulfills the campaign condition)
            foreach (var questionData in campaignData.activeQuestions)
            {
                activeQuestions.Add(LoadAdvisorQuestion(questionData));
            }

            campaign.AddActiveQuestions(activeQuestions);

            // Fetch advisor tree if it exists
            var advisorTree = new List<AdvisorQuestion>();

            foreach (var questionData in campaignData.activeQuestions)
            {
                activeQuestions.Add(LoadAdvisorQuestion(questionData, true));
            }

            campaign.AddToAdvisorTree(advisorTree);
        }

        protected AdvisorQuestion LoadAdvisorQuestion(dynamic questionData, bool recursive = false)
        {
            var answers = new List<AdvisorAnswer>();

            foreach (var answerData in questionData.answers)
            {
                string text = (string)answerData.text;
                Uri parameters = ParametersHandler.GeneratePageLink(
                    ParametersHandler.ParseParametersFromString((string)answerData.@params)
                );
                
                var subquestions = new List<AdvisorQuestion>();
                if (recursive)
                    foreach(var subquestionData in answerData.questions)
                        subquestions.Add(LoadAdvisorQuestion(subquestionData, true));
                
                answers.Add(new AdvisorAnswer(
                    text,
                    parameters.Query,
                    subquestions
                ));
            }

            return new AdvisorQuestion((string)questionData.text, answers);
        }
    }
}
