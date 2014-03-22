using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

// Taken from
// http://www.drowningintechnicaldebt.com/ShawnWeisfeld/archive/2010/08/22/using-c-4.0-and-dynamic-to-parse-json.aspx
// and
// http://stackoverflow.com/questions/3142495/deserialize-json-into-c-sharp-dynamic-object
// (with some modifications)

namespace Omikron.FactFinder.Util.Json
{

    public sealed class DynamicJsonObject : DynamicObject
    {
        private readonly IDictionary<string, object> _dictionary;

        public DynamicJsonObject(IDictionary<string, object> dictionary)
        {
            if (dictionary == null)
                throw new ArgumentNullException("dictionary");
            _dictionary = dictionary;
        }

        public IDictionary<string, object> AsDictionary()
        {
            return _dictionary;
        }

        public IDictionary<string, string> AsStringDictionary()
        {
            var stringDictionary = new Dictionary<string, string>(_dictionary.Count);
            foreach (var pair in _dictionary)
            {
                stringDictionary[pair.Key] = (string)pair.Value;
            }
            return stringDictionary;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            return TryGetFieldFromName(binder.Name, out result);
        }

        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            if (indexes[0] is string)
            {
                return TryGetFieldFromName(indexes[0] as string, out result);
            }

            result = null;
            return false;
        }

        private bool TryGetFieldFromName(string name, out object result)
        {
            if (!_dictionary.TryGetValue(name, out result))
            {
                result = null;
                return false;
            }

            var dictionary = result as IDictionary<string, object>;
            if (dictionary != null)
            {
                result = new DynamicJsonObject(dictionary);
                return true;
            }

            var arrayList = result as ArrayList;
            if (arrayList != null)
            {
                if (arrayList.Count > 0 && arrayList[0] is IDictionary<string, object>)
                    result = new List<object>(arrayList.Cast<IDictionary<string, object>>().Select(x => new DynamicJsonObject(x)));
                else
                    result = new List<object>(arrayList.Cast<object>());
                return true;
            }

            return true;
        }

        public bool ContainsKey(string key)
        {
            return _dictionary.ContainsKey(key);
        }
    }
}
