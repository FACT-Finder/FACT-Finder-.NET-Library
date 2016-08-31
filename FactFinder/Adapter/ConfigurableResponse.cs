using Omikron.FactFinder.Core.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Omikron.FactFinder.Adapter
{
    public class ConfigurableResponse : AbstractAdapter
    {
        private bool _idsOnly;
        public bool IDsOnly
        {
            get
            {
                return _idsOnly;
            }
            set
            {
                if (!_idsOnly && value)
                {
                    UpToDate = false;
                    Parameters["idsOnly"] = "true";
                }
                else
                {
                    UpToDate = false;
                    Parameters.Remove("idsOnly");
                };
            }
        }

        protected ConfigurableResponse(Request request, Core.Client.UrlBuilder urlBuilder)
            : base(request, urlBuilder)
        {
        }

    }
}
