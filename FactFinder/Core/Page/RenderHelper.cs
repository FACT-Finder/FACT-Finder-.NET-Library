using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Text.RegularExpressions;
using Omikron.FactFinder.Adapter;
using Omikron.FactFinder.Data;
using Omikron.FactFinder.Util;

namespace Omikron.FactFinder.Core.Page
{
    public class RenderHelper
    {
        protected Search SearchAdapter { get; set; }
        protected SearchParameters FFParameters { get; set; }

        public RenderHelper(SearchParameters ffParameters, Search searchAdapter)
        {
            SearchAdapter = searchAdapter;
            FFParameters = ffParameters;
        }

        public string CreateJavaScriptClickCode(Record record, string title, string sid, bool useLegacyTracking = true)
        {
            if (String.IsNullOrEmpty(sid))
                sid = HttpContextFactory.Current.Session.SessionID;

            return CreateLegacyJavaScriptClickCode(record, title, sid);
        }

        private string CreateLegacyJavaScriptClickCode(Record record, string title, string sid)
        {
            string query = FFParameters.Query.Replace(@"'", @"\'");

            int position = record.Position;

            string clickCode = "";

            if (position != 0 && query != "")
            {
                string channel = FFParameters.Channel;

                int currentPageNumber = SearchAdapter.Paging.CurrentPage;
                string originalPageSize = SearchAdapter.ProductsPerPageOptions.DefaultOption.Label;
                string pageSize = SearchAdapter.ProductsPerPageOptions.SelectedOption.Label;

                int originalPosition = record.OriginalPosition;
                if (originalPosition == 0) originalPosition = position;

                string similarity = record.Similarity.ToString(System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")); // two decimal places, no thousand separator
                string id = record.ID;

                title = Regex.Replace(title, "['\"\\\0]", @"\$&");
                sid = Regex.Replace(sid, "['\"\\\0]", @"\$&");

                clickCode = String.Format("clickProduct('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', 'click');",
                    query, id, position, originalPosition, currentPageNumber, similarity, sid, title, pageSize, originalPageSize, channel);
            }

            return clickCode;
        }
    }
}
