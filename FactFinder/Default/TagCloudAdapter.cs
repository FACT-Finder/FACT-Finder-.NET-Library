using System.Collections.Generic;
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

        public TagCloudAdapter(DataProvider dataProvider, ParametersHandler parametersHandler)
            : base(dataProvider, parametersHandler)
        { }
        
        protected virtual IList<TagQuery> CreateTagCloud()
        {
            return new List<TagQuery>();
        }
    }
}
