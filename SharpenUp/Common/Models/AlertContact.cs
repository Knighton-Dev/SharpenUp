namespace SharpenUp.Common.Models
{
    // TODO: JsonProperties
    public class AlertContact
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public int Type { get; set; } // TODO: Map out this type
        public int Threshold { get; set; }
        public int Recurrence { get; set; }
    }
}
