using System;
using System.Collections.Generic;
using System.Windows.Documents;

namespace ZbW.Testing.Dms.Client.Model
{
    public class MetadataItem
    {
        public string User { get; set; }
        public string FileName { get; set; }
        public DateTime CreareDate { get; set; }
        public DateTime ValueDate { get; set; }
        public  String Type { get; set; }
        public  List<String> Keywords { get; set; }
    }
}