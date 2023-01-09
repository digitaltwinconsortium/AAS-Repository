
namespace AdminShell
{
    public class AasElementSelfDescription
    {
        public string ElementName = "";

        public string ElementAbbreviation = "";

        public AdequateElementEnum ElementEnum = AdequateElementEnum.Unknown;

        public AasElementSelfDescription() { }

        public AasElementSelfDescription(
            string ElementName, string ElementAbbreviation,
            AdequateElementEnum elementEnum = AdequateElementEnum.Unknown)
        {
            this.ElementName = ElementName;
            this.ElementAbbreviation = ElementAbbreviation;
            this.ElementEnum = elementEnum;
        }
    }
}

