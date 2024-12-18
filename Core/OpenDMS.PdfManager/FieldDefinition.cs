namespace OpenDMS.PdfManager
{
    public class FieldDefinition
    {
        public IList<iText.Kernel.Geom.Rectangle> Locations { get; set; }
        public Int32 FieldType { get; set; }
        public String[] FieldValues { get; set; }
        public String[] FieldStates { get; set; }
        public String[] FieldCaptions { get; set; }
        public String FieldName { get; set; }
        public int Page { get; set; }
        public float Left { get { return Locations[0].GetLeft(); } }
        public float Top { get { return Locations[0].GetTop(); } }
        public float Width { get { return Locations[0].GetWidth(); } }
        public float Height { get { return Locations[0].GetHeight(); } }

    }
}
