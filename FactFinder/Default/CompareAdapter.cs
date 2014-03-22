using System;
using System.Collections.Generic;
using log4net;
using Omikron.FactFinder.Adapter;
using Omikron.FactFinder.Core;
using Omikron.FactFinder.Core.Server;
using Omikron.FactFinder.Data;

namespace Omikron.FactFinder.Default
{
    public class CompareAdapter: AbstractAdapter
    {
        private static ILog log;

        static CompareAdapter()
        {
            log = LogManager.GetLogger(typeof(CompareAdapter));
        }

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

        public CompareAdapter(DataProvider dataProvider, ParametersConverter parametersConverter, Omikron.FactFinder.Core.Client.UrlBuilder urlBuilder)
            : base(dataProvider, parametersConverter, urlBuilder)
        {
            log.Debug("Initialize new CompareAdapter.");

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

        protected virtual IDictionary<string, bool> CreateComparableAttributes()
        {
            return new Dictionary<string, bool>();
        }

        protected virtual IList<Record> CreateComparedRecords()
        {
            return new List<Record>();
        }
    }
}
