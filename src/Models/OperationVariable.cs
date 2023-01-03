
namespace AdminShell
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    [DataContract]
    public class OperationVariable : IAasElement
    {
        public enum Direction { In, Out, InOut };

        [Required]
        [DataMember(Name = "Value")]
        public SubmodelElementWrapper Value { get; set; }

        public OperationVariable()
        {
        }

        public OperationVariable(OperationVariable src, bool shallowCopy = false)
        {
            Value = new SubmodelElementWrapper(src?.Value?.submodelElement, shallowCopy);
        }

        public OperationVariable(SubmodelElement elem)
            : base()
        {
            Value = new SubmodelElementWrapper(elem);
        }

        public AasElementSelfDescription GetSelfDescription()
        {
            return new AasElementSelfDescription("OperationVariable", "OprVar");
        }

        public string GetElementName()
        {
            return this.GetSelfDescription()?.ElementName;
        }
    }
}
