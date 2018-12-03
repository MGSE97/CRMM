using System.Collections.Generic;
using System.Linq;
using Data.Mapping;
using Data.Mapping.Attributes;

namespace CRMM.Models
{
    public class TestModel
    {
        private string _a;

        [Key]
        public string a
        {
            get => _a;
            private set
            {
                _a = value;
                HistoryList.Add($"a({_a})");
            }
        }

        private int _b;

        public int b
        {
            get => _b;
            set
            {
                _b = value;
                HistoryList.Add($"b({_b})");
            }
        }

        public string History => string.Join(", ", HistoryList);
        public readonly List<string> HistoryList = new List<string>();

        public TestModel()
        {
            HistoryList.Add("Init");
            a = "0";
            b = 0;
            HistoryList.Add("_Init");
        }
        public TestModel(string __a, int __b)
        {
            HistoryList.Add("Init");
            a = __a;
            b = __b;
            HistoryList.Add("_Init");
        }
    }
}