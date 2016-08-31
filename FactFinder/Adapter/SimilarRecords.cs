using System.Collections.Generic;
using log4net;
using Omikron.FactFinder.Core.Server;
using Omikron.FactFinder.Data;
namespace Omikron.FactFinder.Adapter
{
    public class SimilarRecords : ConfigurableResponse
    {
        protected string ProductID { get; set; }
        
        private int _maxRecordCount;
        public int MaxRecordCount
        {
            protected get
            {
                return _maxRecordCount;
            }
            set
            {
                value = value < 0 ? 0 : value;
                if (_maxRecordCount != value)
                    UpToDate = false;
                _maxRecordCount = value;
                if (value == 0)
                    Parameters.Remove("maxRecordCount");
                else
                    Parameters["maxRecordCount"] = value.ToString();
            }
        }

        private IDictionary<string, string> _similarAttributes;
        public IDictionary<string, string> SimilarAttributes
        {
            get
            {
                if (_similarAttributes == null || !UpToDate)
                {
                    _similarAttributes = CreateSimilarAttributes();
                    UpToDate = true;
                }
                return _similarAttributes;
            }
        }

        private IList<Record> _records;
        public IList<Record> Records
        {
            get
            {
                if (_records == null || !UpToDate)
                {
                    _records = CreateRecords();
                    UpToDate = true;
                }
                return _records;
            }
        }

        protected dynamic JsonData { get { return ResponseContent; } }

        private static ILog log;
        
        static SimilarRecords()
        {
            log = LogManager.GetLogger(typeof(SimilarRecords));
        }

        public SimilarRecords(Request request, Core.Client.UrlBuilder urlBuilder)
            : base(request, urlBuilder)
        {
            log.Debug("Initialize new SimilarRecords adapter.");

            Request.Action = RequestType.SimilarRecords;
            Parameters["format"] = "json";

            UseJsonResponseContentProcessor();

            ProductID = "";
            UpToDate = false;
            MaxRecordCount = 0;
        }

        public virtual void SetProductID(string productID)
        {
            if (productID != ProductID)
            {
                ProductID = productID;
                Parameters["id"] = productID;
                UpToDate = false;
            }
        }

        protected IDictionary<string, string> CreateSimilarAttributes()
        {
            var attributes = new Dictionary<string, string>();

            foreach (var attributeData in JsonData.attributes)
                attributes[attributeData.name] = (string)attributeData.value;

            return attributes;
        }

        protected IList<Record> CreateRecords()
        {
            var records = new List<Record>();

            int position = 0;

            foreach (var recordData in JsonData.records)
            {
                records.Add(new Record(
                    (string)recordData.id,
                    100,
                    position,
                    position,
                    recordData.record.AsDictionary()
                ));

                ++position;
            }

            return records;
        }
    }
}
