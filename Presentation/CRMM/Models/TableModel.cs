using System;
using System.Collections;
using System.Collections.Generic;

namespace CRMM.Models
{
    public class TableModel
    {
        public string Title { get; set; }
        public Type DataType { get; set; }
        public IEnumerable Data { get; set; }

        public TableMode Mode { get; set; }
        public TableOptions Options { get; set; }

        public TableModel(string title, Type dataType, IEnumerable data, TableOptions options = null, TableMode mode = TableMode.Full)
        {
            DataType = dataType;
            Data = data;
            Mode = mode;
            Title = title;
            Options = options;
        }

        public TableModel()
        {
        }
    }

    [Flags]
    public enum TableMode
    {
        None = 0,
        Create = 1,
        Edit = 2,
        Delete = 4,
        Export = 8,
        Full = Create|Edit|Delete|Export
    }

    public class TableOptions
    {
        public string CreateUrl { get; set; }
        public string EditUrl { get; set; }
        public string DeleteUrl { get; set; }
        public string ExportUrl { get; set; }

        public TableOptions()
        {
            
        }

        public TableOptions(string createUrl, string editUrl, string deleteUrl, string exportUrl)
        {
            CreateUrl = createUrl;
            EditUrl = editUrl;
            DeleteUrl = deleteUrl;
            ExportUrl = exportUrl;
        }
    }
}