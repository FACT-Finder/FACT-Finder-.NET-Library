using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Omikron.FactFinder
{
    public abstract class Adapter
    {
        protected DataProvider DataProvider;

        public Adapter(DataProvider dataProvider)
        {
            DataProvider = dataProvider;
        }

        protected string Data
        {
            get
            {
                return DataProvider.Data;
            }
        }
    }
}
