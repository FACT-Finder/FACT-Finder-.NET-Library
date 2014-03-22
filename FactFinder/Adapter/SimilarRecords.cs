using System.Collections.Generic;
using System.Web.Script.Serialization;
using log4net;
using Omikron.FactFinder.Core;
using Omikron.FactFinder.Core.Server;
using Omikron.FactFinder.Data;
using Omikron.FactFinder.Util.Json;
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
                DataProvider.SetParameter("idsOnly", value ? "true" : "false");
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
                    DataProvider.UnsetParameter("maxRecordCount");
                else
                    DataProvider.SetParameter("maxRecordCount", value.ToString());
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

        protected dynamic JsonData
        {
            get
            {
                var jsonSerializer = new JavaScriptSerializer();
                jsonSerializer.RegisterConverters(new[] { new DynamicJsonConverter() });
                return jsonSerializer.Deserialize(base.Data, typeof(object));
            }
        }

        private static ILog log;
        
        static SimilarRecords()
        {
            log = LogManager.GetLogger(typeof(SimilarRecords));
        }

        public SimilarRecords(DataProvider dataProvider, ParametersConverter parametersConverter, Omikron.FactFinder.Core.Client.UrlBuilder urlBuilder)
            : base(dataProvider, parametersConverter, urlBuilder)
        {
            log.Debug("Initialize new SimilarRecordsAdapter.");
            DataProvider.Type = RequestType.SimilarRecords;

            ProductID = "";
            RecordsUpToDate = false;
            AttributesUpToDate = false;
            IDsOnly = false;
            MaxRecordCount = 0;

            DataProvider.SetParameter("format", "json");
        }

        public virtual void SetProductID(string productID)
        {
            if (productID != ProductID)
            {
                ProductID = productID;
                DataProvider.SetParameter("id", productID);
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
