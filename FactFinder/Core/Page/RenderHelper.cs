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

            if (useLegacyTracking)
                return CreateLegacyJavaScriptClickCode(record, title, sid);
            else
            {
                string channel = FFParameters.Channel;
                var parameters = new NameValueCollection();
                parameters["id"] = record.ID;
                return CreateJavaScriptTrackingCode(EventType.Inspect, SearchAdapter.Result.RefKey, sid, channel, parameters);
            }
        }

        private string CreateJavaScriptTrackingCode(EventType type, string sourceRefKey, string sid, string channel, NameValueCollection additionalParameters = null)
        {
            if (String.IsNullOrEmpty(sid))
                sid = HttpContextFactory.Current.Session.SessionID;
            if (String.IsNullOrEmpty(channel))
                channel = FFParameters.Channel;

            var sb = new StringBuilder("{");
            foreach (string key in additionalParameters)
            {
                sb.Append(key);
                sb.Append(": '");
                sb.Append(additionalParameters[key]);
                sb.Append("'");
            }
            sb.Append("}");

            // Escape single quotes that are not yet escaped
            sid = Regex.Replace(sid, @"
                    (?<!\\)     # Make sure the match is not preceded by further backslashes
                    ((?:\\\\)*) # match and capture an even number of backslashes in group 1 (to write back)
                    '           # match a single quote
                ", @"$1\'", RegexOptions.IgnorePatternWhitespace);

            return String.Format("trackEvent('{0}', '{1}', '{2}', '{3}', '{4}');",
                type.ToString(),
                sourceRefKey,
                sid,
                channel,
                sb.ToString()
            );
        }

        public string CreateJavaScriptEventCode(IDictionary<EventType, NameValueCollection> events, string sid, string channel)
        {
            string sourceRefKey = SearchAdapter.Result.RefKey;
            
            var sb = new StringBuilder("$(document).ready( function (){ ");
            foreach (var kvp in events)
                sb.Append(CreateJavaScriptTrackingCode(kvp.Key, sourceRefKey, sid, channel, kvp.Value));
            sb.Append(" });");

            return sb.ToString();
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
                string originalPageSize  = SearchAdapter.ProductsPerPageOptions.DefaultOption.Label;
                string pageSize          = SearchAdapter.ProductsPerPageOptions.SelectedOption.Label;

                int originalPosition = record.OriginalPosition;
                if (originalPosition == 0) originalPosition = position;

                string similarity = record.Similarity.ToString("F2"); // two decimal places, no thousand separator
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
