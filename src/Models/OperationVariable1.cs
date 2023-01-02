/*
Copyright (c) 2018-2021 Festo AG & Co. KG <https://www.festo.com/net/de_de/Forms/web/contact_international>
Author: Michael Hoffmeister

This source code is licensed under the Apache License 2.0 (see LICENSE.txt).

This source code may use other Open Source software components (see LICENSE.txt).
*/

using Newtonsoft.Json;
using System.Xml.Serialization;

namespace AdminShell
{
    #region AdminShell_V2_0

    public partial class AdminShellV20
    {
        public class OperationVariable : IAasElement
        {
            public enum Direction { In, Out, InOut };

            // Note: for OperationVariable, the values of the SubmodelElement itself ARE NOT TO BE USED!
            // only the SME attributes of "Value" are counting

            // for JSON only
            [XmlIgnore]
            [JsonProperty(PropertyName = "modelType")]
            public JsonModelTypeWrapper JsonModelType { get { return new JsonModelTypeWrapper(GetElementName()); } }

            // members
            public SubmodelElementWrapper value = null;

            // constructors

            public OperationVariable()
            {
            }

            public OperationVariable(OperationVariable src, bool shallowCopy = false)
            {
                this.value = new SubmodelElementWrapper(src?.value?.submodelElement, shallowCopy);
            }

#if !DoNotUseAdminShell
            public OperationVariable(
                AdminShell.AdminShellV10.OperationVariable src, bool shallowCopy = false)
            {
                this.value = new SubmodelElementWrapper(src.value.submodelElement, shallowCopy);
            }
#endif

            public OperationVariable(SubmodelElement elem)
                : base()
            {
                this.value = new SubmodelElementWrapper(elem);
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

    #endregion
}

