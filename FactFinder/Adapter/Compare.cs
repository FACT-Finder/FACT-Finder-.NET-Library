using System;
using System.Collections.Generic;
using log4net;
using Omikron.FactFinder.Core.Server;
using Omikron.FactFinder.Data;

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
                Parameters["idsOnly"] = value ? "true" : "false";
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

        protected dynamic JsonData { get { return ResponseContent; } }

        private static ILog log;

        static Compare()
        {
            log = LogManager.GetLogger(typeof(Compare));
        }

        public Compare(Request request, Core.Client.UrlBuilder urlBuilder)
            : base(request, urlBuilder)
        {
            log.Debug("Initialize new Compare adapter.");

            Request.Action = RequestType.Compare;
            Parameters["format"] = "json";

            UseJsonResponseContentProcessor();

            ProductIDs = new List<string>();
            RecordsUpToDate = false;
            AttributesUpToDate = false;
            IDsOnly = false;
        }

        public virtual void SetProductIDs(string[] productIDs)
        {
            ProductIDs = productIDs;
            Parameters["ids"] = String.Join(";", productIDs);
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
