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
    public partial class AdminShellV30
    {
        //
        // Reference elements
        //

        /// <summary>
        /// This class was the old V2.0 ReferenceElement, which now turned into 
        /// more a abstract class.
        /// </summary>
        public class ReferenceElement : DataElement
        {
            // for JSON only
            [XmlIgnore]
            [JsonProperty(PropertyName = "modelType")]
            public new JsonModelTypeWrapper JsonModelType
            {
                get { return new JsonModelTypeWrapper(GetElementName()); }
            }

            // members

            // constructors

            public ReferenceElement() { }

            public ReferenceElement(SubmodelElement src)
                : base(src)
            {
            }

#if !DoNotUseAdminShell
            public ReferenceElement(AdminShell.AdminShellV10.ReferenceElement src)
                : base(src)
            {
            }

            public ReferenceElement(AdminShell.AdminShellV20.ReferenceElement src)
                : base(src)
            {
            }
#endif

            // self description

            public override AasElementSelfDescription GetSelfDescription()
            {
                return new AasElementSelfDescription("ReferenceElement", "Ref",
                    SubmodelElementWrapper.AdequateElementEnum.ReferenceElement);
            }

        }

    }
}
