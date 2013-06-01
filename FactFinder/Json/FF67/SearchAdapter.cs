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

        protected override Data.CampaignList CreateCampaigns()
        {
            var campaigns = new List<Campaign>();

            foreach (var campaignData in JsonData.campaigns)
            {
                var campaign = new Campaign(
                    (string)campaignData.name,
                    (string)campaignData.category,
                    (string)campaignData.target.destination
                );

                string flavor = (string)campaignData.flavour;

                if (flavor == "FEEDBACK")
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

                    if (campaignData.pushedProductsRecords.Count > 0)
                    {
                        var pushedProducts = new List<Record>();

                        foreach (var recordData in campaignData.pushedProductsRecords)
                        {
                            var record = new Record((string)recordData.id);
                            record.SetFieldValues(recordData.record.AsDictionary());
                            pushedProducts.Add(record);
                        }

                        campaign.AddPushedProducts(pushedProducts);
                    }
                }

                if (flavor == "ADVISOR")
                {
                    var activeQuestions = new List<AdvisorQuestion>();

                    // The active questions can still be empty if we have already moved down the whole question tree (while the search query still fulfills the campaign condition)
					foreach(var questionData in campaignData.activeQuestions)
                    {
                        activeQuestions.Add(LoadAdvisorQuestion(questionData));
                    }

                    campaign.AddActiveQuestions(activeQuestions);
					
					// Fetch advisor tree if it exists
					var advisorTree = new List<AdvisorQuestion>();
					
					foreach(var questionData in campaignData.activeQuestions)
                    {
                        activeQuestions.Add(LoadAdvisorQuestion(questionData, true));
                    }

					campaign.AddToAdvisorTree(advisorTree);
                }

                campaigns.Add(campaign);
            }

            return new CampaignList(campaigns);
        }

        protected AdvisorQuestion LoadAdvisorQuestion(dynamic questionData, bool recursive = false)
        {
            var answers = new List<AdvisorAnswer>();

            foreach (var answerData in questionData.answers)
            {
                string text = (string)answerData.text;
                string parameters = ParametersHandler.GeneratePageLink(
                    ParametersHandler.ParseParametersFromString((string)answerData.@params)
                );
                
                var subquestions = new List<AdvisorQuestion>();
                if (recursive)
                    foreach(var subquestionData in answerData.questions)
                        subquestions.Add(LoadAdvisorQuestion(subquestionData, true));
                
                answers.Add(new AdvisorAnswer(
                    text,
                    parameters,
                    subquestions
                ));
            }

            return new AdvisorQuestion((string)questionData.text, answers);
        }
    }
}
