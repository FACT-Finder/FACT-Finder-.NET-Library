using System.Collections.Generic;
using log4net;
using Omikron.FactFinder.Data;

namespace Omikron.FactFinder.Default
{
    public class TagCloudAdapter : Adapter
    {
        private IList<TagQuery> _tagCloud;
        public IList<TagQuery> TagCloud
        {
            get
            {
                if (_tagCloud == null)
                    _tagCloud = CreateTagCloud();
                return _tagCloud; 
            }
        }

        private static ILog log;

        static TagCloudAdapter()
        {
            log = LogManager.GetLogger(typeof(TagCloudAdapter));
        }

        public TagCloudAdapter(DataProvider dataProvider, ParametersHandler parametersHandler)
            : base(dataProvider, parametersHandler)
        {
            log.Debug("Initialize new RecommendationAdapter.");
        }
        
        protected virtual IList<TagQuery> CreateTagCloud()
        {
            return new List<TagQuery>();
        }
    }
}
