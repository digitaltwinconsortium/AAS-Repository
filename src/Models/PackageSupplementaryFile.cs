
namespace AdminShell
{
    using System;

    public class PackageSupplementaryFile : Referable
    {
        public Uri Uri { get; set; }

        public string Key { get; set; }

        public byte[] SourceBytes { get; set; }

        public LocationType Location { get; set; }

        public SpecialHandlingType SpecialHandling { get; set; }
    }
}
