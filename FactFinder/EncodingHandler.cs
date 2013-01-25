using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Omikron.FactFinder
{
    public class EncodingHandler
    {
        protected IConfiguration Configuration;

        public EncodingHandler(IConfiguration configuration)
        {
            Configuration = configuration;
        }
    }
}
