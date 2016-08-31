using Omikron.FactFinder.Core.Server;
using Omikron.FactFinder.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Omikron.FactFinder.Adapter
{
    public class PersonalisedResponse : ConfigurableResponse
    {
        private string _sid;
        public string Sid
        {
            get
            {
                return _sid;
            }
            set
            {
                if (string.IsNullOrEmpty(_sid) || !_sid.Equals(value) && !string.IsNullOrEmpty(value))
                {
                    UpToDate = false;
                    _sid = value;
                    Parameters["sid"] = _sid;
                }
                else
                {
                    UpToDate = false;
                    Parameters.Remove("sid");
                };
            }
        }

        protected PersonalisedResponse(Request request, Core.Client.UrlBuilder urlBuilder)
            : base(request, urlBuilder)
        {
        }
    }
}
