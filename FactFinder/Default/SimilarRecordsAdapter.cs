using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using Omikron.FactFinder.Data;

namespace Omikron.FactFinder.Default
{
    public class SimilarRecordsAdapter : Adapter
    {
        private static ILog log;

        static SimilarRecordsAdapter()
        {
            log = LogManager.GetLogger(typeof(SimilarRecordsAdapter));
        }

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

        public SimilarRecordsAdapter(DataProvider dataProvider, ParametersHandler parametersHandler)
            : base(dataProvider, parametersHandler)
        {
            log.Debug("Initialize new SimilarRecordsAdapter.");

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
                DataProvider.SetParameter("id", productID);
                RecordsUpToDate = false;
                AttributesUpToDate = false;
            }
        }

        protected virtual IDictionary<string, string> CreateSimilarAttributes()
        {
            return new Dictionary<string, string>();
        }

        protected virtual IList<Record> CreateRecords()
        {
            return new List<Record>();
        }
    }
}
