#define UseAdminShell

using System.Collections.Generic;
using System.Xml;
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
        public class Referable
        {

            // members

            public string idShort = null;
            public string category = null;

            [XmlElement(ElementName = "description")]
            
            public Description description = null;
            [XmlIgnore]
            [JsonProperty(PropertyName = "descriptions")]
            public List<LangStr> JsonDescription
            {
                get
                {
                    if (description == null)
                        return null;
                    return description.langString;
                }
                set
                {
                    if (description == null)
                        description = new Description();
                    description.langString = value;
                }
            }

            [XmlIgnore]
            
            public Referable parent = null;

            public static string[] ReferableCategoryNames = new string[] { "CONSTANT", "PARAMETER", "VARIABLE" };

            // constructors

            public Referable() { }

            public Referable(Referable src)
            {
                this.idShort = src.idShort;
                this.category = src.category;
                if (src.description != null)
                    this.description = new Description(src.description);
            }

            public void AddDescription(string lang, string str)
            {
                if (description == null)
                    description = new Description();
                description.langString.Add(LangStr.CreateNew(lang, str));
            }

            public virtual string GetElementName()
            {
                return "GlobalReference"; // not correct, but this method wasn't overridden correctly
            }

            public string GetFriendlyName()
            {
                return AdminShellUtil.FilterFriendlyName(this.idShort);
            }

            public void CollectReferencesByParent(List<Key> refs)
            {
                // check, if this is identifiable
                if (this is Identifiable)
                {
                    var idf = this as Identifiable;
                    var k = Key.CreateNew(idf.GetElementName(), true, idf.identification.idType, idf.identification.id);
                    refs.Insert(0, k);
                }
                else
                {
                    var k = Key.CreateNew(this.GetElementName(), true, "idShort", this.idShort);
                    refs.Insert(0, k);
                    // recurse upwards!
                    if (parent != null && parent is Referable)
                        (this.parent).CollectReferencesByParent(refs);
                }
            }

            public string CollectIdShortByParent()
            {
                // recurse first
                var head = "";
                if (!(this is Identifiable) && this.parent != null && this.parent is Referable)
                    // can go up
                    head = this.parent.CollectIdShortByParent() + "/";
                // add own
                var myid = "<no id-Short!>";
                if (this.idShort != null && this.idShort.Trim() != "")
                    myid = this.idShort.Trim();
                // together
                return head + myid;
            }
        }

    }

    #endregion
}

#endif