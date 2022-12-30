/*
Copyright (c) 2018-2021 Festo AG & Co. KG <https://www.festo.com/net/de_de/Forms/web/contact_international>
Author: Michael Hoffmeister

This source code is licensed under the Apache License 2.0 (see LICENSE.txt).

This source code may use other Open Source software components (see LICENSE.txt).
*/

using Newtonsoft.Json;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace AasxCompatibilityModels
{
    #region AdminShell_V2_0

    public partial class AdminShellV20
    {
#if __not_valid_anymore
        [XmlType(TypeName = "hasDataSpecification")]
        public class HasDataSpecification
        {
            [XmlElement(ElementName = "reference")] // make "reference" go away by magic?!
            public List<Reference> reference = new List<Reference>();

            public HasDataSpecification() { }

            public HasDataSpecification(HasDataSpecification src)
            {
                foreach (var r in src.reference)
                    reference.Add(new Reference(r));
            }

#if !DoNotUseAasxCompatibilityModels
            public HasDataSpecification(AasxCompatibilityModels.AdminShellV10.HasDataSpecification src)
            {
                foreach (var r in src.reference)
                    reference.Add(new Reference(r));
            }
#endif
        }
#else
        // Note: In versions prior to V2.0.1, the SDK has "HasDataSpecification" containing only a Reference.
        // Iv 2.0.1, theoretically each entity with HasDataSpecification could also conatin a 
        // EmbeddedDataSpecification. 

        [XmlType(TypeName = "hasDataSpecification")]
        public class HasDataSpecification : List<EmbeddedDataSpecification>
        {
            public HasDataSpecification() { }

            public HasDataSpecification(HasDataSpecification src)
            {
                foreach (var r in src)
                    this.Add(new EmbeddedDataSpecification(r));
            }

            public HasDataSpecification(IEnumerable<EmbeddedDataSpecification> src)
            {
                foreach (var r in src)
                    this.Add(new EmbeddedDataSpecification(r));
            }

#if !DoNotUseAasxCompatibilityModels
            public HasDataSpecification(AasxCompatibilityModels.AdminShellV10.HasDataSpecification src)
            {
                foreach (var r in src.reference)
                    this.Add(new EmbeddedDataSpecification(r));
            }
#endif

            // make some explicit and easy to use getter, setters            

            [XmlIgnore]
            
            public EmbeddedDataSpecification IEC61360
            {
                get
                {
                    foreach (var eds in this)
                        if (eds?.dataSpecificationContent?.dataSpecificationIEC61360 != null
                            || eds?.dataSpecification?.MatchesExactlyOneKey(
                                DataSpecificationIEC61360.GetKey(), Key.MatchMode.Identification) == true)
                            return eds;
                    return null;
                }
                set
                {
                    // search existing first?
                    var eds = this.IEC61360;
                    if (eds != null)
                    {
                        // replace this
                        /* TODO (MIHO, 2020-08-30): this does not prevent the corner case, that we could have
                            * multiple dataSpecificationIEC61360 in this list, which would be an error */
                        this.Remove(eds);
                        this.Add(value);
                        return;
                    }

                    // no? .. add!
                    this.Add(value);
                }
            }

            [XmlIgnore]
            
            public DataSpecificationIEC61360 IEC61360Content
            {
                get
                {
                    return this.IEC61360?.dataSpecificationContent?.dataSpecificationIEC61360;
                }
                set
                {
                    // search existing first?
                    var eds = this.IEC61360;
                    if (eds != null)
                    {
                        // replace this
                        eds.dataSpecificationContent.dataSpecificationIEC61360 = value;
                        return;
                    }
                    // no? .. add!
                    var edsnew = new EmbeddedDataSpecification();
                    edsnew.dataSpecificationContent.dataSpecificationIEC61360 = value;
                    this.Add(edsnew);
                }
            }

        }



    }

    #endif
}

#endregion