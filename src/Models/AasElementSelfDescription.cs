
namespace AdminShell
{
    public class AasElementSelfDescription
    {
        public string ElementName = "";

        public string ElementAbbreviation = "";

        public SubmodelElementWrapper.AdequateElementEnum ElementEnum = SubmodelElementWrapper.AdequateElementEnum.Unknown;

        public AasElementSelfDescription() { }

        public AasElementSelfDescription(
            string ElementName, string ElementAbbreviation,
            SubmodelElementWrapper.AdequateElementEnum elementEnum = SubmodelElementWrapper.AdequateElementEnum.Unknown)
        {
            this.ElementName = ElementName;
            this.ElementAbbreviation = ElementAbbreviation;
            this.ElementEnum = elementEnum;
        }
    }
}

