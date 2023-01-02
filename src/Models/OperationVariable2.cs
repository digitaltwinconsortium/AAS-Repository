#define UseAdminShell

using System.Xml.Serialization;
using Newtonsoft.Json;

/* Copyright (c) 2018-2019 Festo AG & Co. KG <https://www.festo.com/net/de_de/Forms/web/contact_international>, author: Michael Hoffmeister

This source code is licensed under the Apache License 2.0 (see LICENSE.txt).

This source code may use other Open Source software components (see LICENSE.txt).
*/


#if UseAdminShell

namespace AdminShell
{

    #region Utils

    #endregion


    #region AdminShell_V1_0

    public partial class AdminShellV10
    {
        public class OperationVariable : SubmodelElement
        {
            public enum Direction { In, Out };

            // Note: for OperationVariable, the values of the SubmodelElement itself ARE NOT TO BE USED!
            // only the SME attributes of "Value" are counting

            // for JSON only
            [XmlIgnore]
            [JsonProperty(PropertyName = "modelType")]
            public new JsonModelTypeWrapper JsonModelType { get { return new JsonModelTypeWrapper(GetElementName()); } }

            // members
            public SubmodelElementWrapper value = null;

            // constructors

            public OperationVariable()
            {
                this.kind = new Kind("Type");
            }

            public OperationVariable(OperationVariable src, bool shallowCopy = false)
                : base(src)
            {
                this.value = new SubmodelElementWrapper(src.value.submodelElement, shallowCopy);
            }

            public OperationVariable(SubmodelElement elem)
                : base()
            {
                this.value = new SubmodelElementWrapper(elem);
            }

            public override string GetElementName()
            {
                return "OperationVariable";
            }
        }

    }

    #endregion
}

#endif