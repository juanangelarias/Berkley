namespace James.Shared.Imaging
{
    internal class MimeTypeValue
    {
        internal string MimeType { get; set; } = string.Empty;
        internal string FileExtension { get; set; } = string.Empty;
        internal bool IsMaster { get; set; }
        public override string ToString()
        {
            return $"{MimeType} {FileExtension}{(IsMaster ? " MASTER" : "")}";
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return GetHashCode()==obj.GetHashCode();
        }
    }
}