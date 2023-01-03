
namespace AdminShell
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Runtime.Serialization;

    [DataContract]
    public class Property : DataElement
    {
        [DataMember(Name = "value")]
        [MetaModelName("Property.value")]
        public string Value { get; set; }

        [DataMember(Name = "valueId")]
        public GlobalReference ValueId { get; set; }

        [DataMember(Name = "valueType")]
        [MetaModelName("Property.valueType")]
        public ValueTypeEnum ValueType { get; set; }

        public Property() { }

        public Property(SubmodelElement src)
            : base(src)
        {
            if (!(src is Property p))
                return;

            this.valueType = p.valueType;

            this.value = p.value;

            if (p.valueId != null)
                valueId = new GlobalReference(p.valueId);
        }

        public static Property CreateNew(
            string idShort = null, string category = null, Identifier semanticIdKey = null)
        {
            var x = new Property();

            x.CreateNewLogic(idShort, category, semanticIdKey);

            return x;
        }

        public Property Set(string valueType = "", string value = "")
        {
            this.valueType = valueType;
            this.value = value;

            return this;
        }

        public Property SetValueId(string valueId)
        {
            this.valueId = GlobalReference.CreateNew(value);

            return this;
        }

        public override AasElementSelfDescription GetSelfDescription()
        {
            return new AasElementSelfDescription("Property", "Prop", SubmodelElementWrapper.AdequateElementEnum.Property);
        }

        public string ValueAsText(string defaultLang = null)
        {
            return "" + value;
        }

        public void ValueFromText(string text, string defaultLang = null)
        {
            value = "" + text;
        }

        public bool IsTrue()
        {
            if (this.valueType?.Trim().ToLower() == "boolean")
            {
                var v = "" + this.value?.Trim().ToLower();
                if (v == "true" || v == "1")
                    return true;
            }
            return false;
        }

        public double? ValueAsDouble()
        {
            // pointless
            if (this.value == null || this.value.Trim() == "" || this.valueType == null)
                return null;

            // Type?
            var vt = this.valueType.Trim().ToLower();
            if (!DataElement.ValueTypes_Number.Contains(vt))
                return null;

            // try convert
            if (double.TryParse(this.value, NumberStyles.Any, CultureInfo.InvariantCulture, out double dbl))
                return dbl;

            // no
            return null;
        }

        public object ToValueOnlySerialization()
        {
            var valueObject = new Dictionary<string, string>
            {
                { idShort, value }
            };
            return valueObject;
        }
    }
}

