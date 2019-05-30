using System;
using System.Collections.Generic;

namespace ConnectApp.Models.Model {
    [Serializable]
    public class Place {
        public string id;
        public string type;
        public string name;
        public List<string> components;
    }
}