using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using log4net;
using Omikron.FactFinder.Core;
using Omikron.FactFinder.Core.Server;
using Omikron.FactFinder.Data;
using Omikron.FactFinder.Util.Json;

namespace Omikron.FactFinder.Adapter
{
    public class Compare : AbstractAdapter
    {
        protected IList<string> ProductIDs { get; set; }

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

        private IDictionary<string, bool> _comparableAttributes;
        public IDictionary<string, bool> ComparableAttributes
        {
            get
            {
                if (_comparableAttributes == null || !AttributesUpToDate)
                {
                    _comparableAttributes = CreateComparableAttributes();
                    AttributesUpToDate = true;
                }
                return _comparableAttributes;
            }
        }

        private IList<Record> _comparedRecords;
        public IList<Record> ComparedRecords
        {
            get
            {
                if (_comparedRecords == null || !RecordsUpToDate)
                {
                    _comparedRecords = CreateComparedRecords();
                    RecordsUpToDate = true;
                }
                return _comparedRecords;
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

        static Compare()
        {
            log = LogManager.GetLogger(typeof(Compare));
        }

        public Compare(DataProvider dataProvider, ParametersConverter parametersConverter, Omikron.FactFinder.Core.Client.UrlBuilder urlBuilder)
            : base(dataProvider, parametersConverter, urlBuilder)
        {
            log.Debug("Initialize new CompareAdapter.");

            DataProvider.Type = RequestType.Compare;
            DataProvider.SetParameter("format", "json");

            ProductIDs = new List<string>();
            RecordsUpToDate = false;
            AttributesUpToDate = false;
            IDsOnly = false;
        }

        public virtual void SetProductIDs(string[] productIDs)
        {
            ProductIDs = productIDs;
            DataProvider.SetParameter("ids", String.Join(";", productIDs));
            RecordsUpToDate = false;
            AttributesUpToDate = false;
        }

        protected IDictionary<string, bool> CreateComparableAttributes()
        {
            var attributes = new Dictionary<string, bool>();

            foreach (var attributeData in JsonData.attributes)
                attributes[attributeData.attributeName] = (bool)attributeData.different;

            return attributes;
        }

        protected IList<Record> CreateComparedRecords()
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
