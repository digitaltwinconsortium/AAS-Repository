
namespace AdminShell
{
    using System;

    public class CarbonIntensityQueryResult
    {
        public CarbonData[] data { get; set; }
    }

    public class CarbonData
    {
        public DateTime from { get; set; }

        public DateTime to { get; set; }

        public CarbonIntensity intensity { get; set; }
    }

    public class CarbonIntensity
    {
        public int forecast { get; set; }

        public int actual { get; set; }

        public string index { get; set; }
    }

    public class RegionQueryResult
    {
        public string abbrev { get; set; }

        public int id { get; set; }

        public string name { get; set; }
    }

    public class WattTimeQueryResult
    {
        public string freq { get; set; }

        public string ba { get; set; }

        public int percent { get; set; }

        public float moer { get; set; }

        public DateTime point_time { get; set; }
    }
}