
namespace AdminShell
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Dynamic;
    using System.Runtime.Serialization;

    public class RelationshipElement : DataElement
    {
        [Required]
        [DataMember(Name = "first")]
        public ModelReference First { get; set; } = new();

        [Required]
        [DataMember(Name = "second")]
        public ModelReference Second { get; set; } = new();

        public RelationshipElement() { }

        public RelationshipElement(SubmodelElement src)
            : base(src)
        {
            if (!(src is RelationshipElement rel))
                return;

            if (rel.First != null)
                First = new ModelReference(rel.First);

            if (rel.Second != null)
                Second = new ModelReference(rel.Second);
        }

        public static RelationshipElement CreateNew(
            string idShort = null, string category = null,
            Identifier semanticIdKey = null, ModelReference first = null,
            ModelReference second = null)
        {
            var x = new RelationshipElement();
            x.CreateNewLogic(idShort, category, semanticIdKey);
            x.First = first;
            x.Second = second;
            return (x);
        }

        public void Set(ModelReference first = null, ModelReference second = null)
        {
            First = first;
            Second = second;
        }

        public AasElementSelfDescription GetSelfDescription()
        {
            return new AasElementSelfDescription("RelationshipElement", "Rel",
                SubmodelElementWrapper.AdequateElementEnum.RelationshipElement);
        }

        public object ToValueOnlySerialization()
        {
            var output = new Dictionary<string, object>();

            var listFirst = new List<Dictionary<string, string>>();
            foreach (var key in First.Keys)
            {
                var valueDict = new Dictionary<string, string>
                {
                    { "Type", key.Type },
                    { "Value", key.Value }
                };

                listFirst.Add(valueDict);
            }

            var listSecond = new List<Dictionary<string, string>>();
            foreach (var key in Second.Keys)
            {
                var valueDict = new Dictionary<string, string>
                {
                    { "Type", key.Type },
                    { "Value", key.Value }
                };

                listSecond.Add(valueDict);
            }

            dynamic relObj = new ExpandoObject();
            relObj.first = listFirst;
            relObj.second = listSecond;
            output.Add(IdShort, relObj);

            return output;
        }
    }
}
