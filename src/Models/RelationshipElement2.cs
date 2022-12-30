#define UseAasxCompatibilityModels

using System.Xml.Serialization;
using Newtonsoft.Json;

/* Copyright (c) 2018-2019 Festo AG & Co. KG <https://www.festo.com/net/de_de/Forms/web/contact_international>, author: Michael Hoffmeister

This source code is licensed under the Apache License 2.0 (see LICENSE.txt).

This source code may use other Open Source software components (see LICENSE.txt).
*/


#if UseAasxCompatibilityModels

namespace AasxCompatibilityModels
{

    #region Utils

    #endregion


    #region AdminShell_V1_0

    public partial class AdminShellV10
    {
        public class RelationshipElement : DataElement
        {
            // for JSON only
            [XmlIgnore]
            [JsonProperty(PropertyName = "modelType")]
            public new JsonModelTypeWrapper JsonModelType { get { return new JsonModelTypeWrapper(GetElementName()); } }

            // members

            public Reference first = new Reference();
            public Reference second = new Reference();

            // constructors

            public RelationshipElement() { }

            public RelationshipElement(RelationshipElement src)
                : base(src)
            {
                if (src.first != null)
                    this.first = new Reference(src.first);
                if (src.second != null)
                    this.second = new Reference(src.second);
            }

            public static RelationshipElement CreateNew(string idShort = null, string category = null, Key semanticIdKey = null)
            {
                var x = new RelationshipElement();
                x.CreateNewLogic(idShort, category, semanticIdKey);
                return (x);
            }

            public void Set(Reference first = null, Reference second = null)
            {
                this.first = first;
                this.second = second;
            }

            public override string GetElementName()
            {
                return "RelationshipElement";
            }
        }

    }

    #endregion
}

#endif