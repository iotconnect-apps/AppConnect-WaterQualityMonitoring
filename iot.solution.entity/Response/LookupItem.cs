using System;

namespace iot.solution.entity
{
    public class LookupItem
    {
        private string _Value;
        public string Value { get { return _Value; } set { _Value = value; } }
        public string Text { get; set; }
    }
    public class LookupItemWithStatus : LookupItem
    {
        public bool? IsActive { get; set; }
    }

    public class LookupItemWithImage 
    {
        private string _Value;
        public string Guid { get { return _Value; } set { _Value = value; } }
        public string Name { get; set; }
        public string Image { get; set; }
    }
    public class TagLookup
    {
        public string tag { get; set; }
        public bool templateTag { get; set; }
    }
}
