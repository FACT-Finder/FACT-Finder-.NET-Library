using System.Collections.Generic;
using log4net;
using Omikron.FactFinder.Core.Server;
using Omikron.FactFinder.Data;
namespace Omikron.FactFinder.Adapter
{
    public class SimilarRecords : AbstractAdapter
    {
        protected string ProductID { get; set; }

        protected bool AttributesUpToDate { get; set; }
        protected bool RecordsUpToDate { get; set; }

        private bool _idsOnly;
        public bool IDsOnly
        {
            protected get
            {
                return _idsOnly;
            }
            set
            {
                if (_idsOnly && !value)
                    RecordsUpToDate = false;
                _idsOnly = value;
                Parameters["idsOnly"] = value ? "true" : "false";
            }
        }

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
                    RecordsUpToDate = false;
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
                if (_similarAttributes == null || !AttributesUpToDate)
                {
                    _similarAttributes = CreateSimilarAttributes();
                    AttributesUpToDate = true;
                }
                return _similarAttributes;
            }
        }

        private IList<Record> _records;
        public IList<Record> Records
        {
            get
            {
                if (_records == null || !RecordsUpToDate)
                {
                    _records = CreateRecords();
                    RecordsUpToDate = true;
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
            RecordsUpToDate = false;
            AttributesUpToDate = false;
            IDsOnly = false;
            MaxRecordCount = 0;
        }

        public virtual void SetProductID(string productID)
        {
            if (productID != ProductID)
            {
                ProductID = productID;
                Parameters["id"] = productID;
                RecordsUpToDate = false;
                AttributesUpToDate = false;
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
                if (IDsOnly)
                {
                    records.Add(new Record((string)recordData.id));
                    continue;
                }

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
