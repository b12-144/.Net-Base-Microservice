using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Entities {
    public class EUserAdvancedSearchRequest {
        public string name { get; set; }
        public string mobile { get; set; }
        public string postalCode { get; set; }
        public string city { get; set; }
        public string state { get; set; }
    }
}
