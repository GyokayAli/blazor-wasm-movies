using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorMovies.Components.Helpers
{
    public struct MultipleSelectorModel
    {
        public string Key { get; set; }
        public string Value { get; set; }

        public MultipleSelectorModel(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
}
