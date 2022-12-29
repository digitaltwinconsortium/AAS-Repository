#define UseAasxCompatibilityModels

using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

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
        [XmlType(TypeName = "submodelElement")]
        public class SubmodelElementWrapper
        {

            // members

            [XmlElement(ElementName = "property", Type = typeof(Property))]
            [XmlElement(ElementName = "file", Type = typeof(File))]
            [XmlElement(ElementName = "blob", Type = typeof(Blob))]
            [XmlElement(ElementName = "referenceElement", Type = typeof(ReferenceElement))]
            [XmlElement(ElementName = "relationshipElement", Type = typeof(RelationshipElement))]
            [XmlElement(ElementName = "submodelElementCollection", Type = typeof(SubmodelElementCollection))]
            [XmlElement(ElementName = "operation", Type = typeof(Operation))]
            public SubmodelElement submodelElement;

            // element names
            public static string[] AdequateElementNames = { "SubmodelElementCollection", "Property", "File", "Blob", "ReferenceElement",
                "RelationshipElement", "Operation", "OperationVariable" };

            // constructors

            public SubmodelElementWrapper() { }

            // for cloning
            public SubmodelElementWrapper(SubmodelElement src, bool shallowCopy = false)
            {
                if (src is Property)
                    this.submodelElement = new Property(src as Property);
                if (src is File)
                    this.submodelElement = new File(src as File);
                if (src is Blob)
                    this.submodelElement = new Blob(src as Blob);
                if (src is ReferenceElement)
                    this.submodelElement = new ReferenceElement(src as ReferenceElement);
                if (src is RelationshipElement)
                    this.submodelElement = new RelationshipElement(src as RelationshipElement);
                if (src is SubmodelElementCollection)
                    this.submodelElement = new SubmodelElementCollection(src as SubmodelElementCollection, shallowCopy: shallowCopy);
                if (src is Operation)
                    this.submodelElement = new Operation(src as Operation);
            }

            /// <summary>
            /// Introduced for JSON serialization, can create SubModelElements based on a string name
            /// </summary>
            /// <param name="elementName">string name (standard PascalCased)</param>
            /// <returns>SubmodelElement</returns>
            public static SubmodelElement CreateAdequateType(string elementName)
            {
                if (elementName == "Property")
                    return new Property();
                if (elementName == "File")
                    return new File();
                if (elementName == "Blob")
                    return new Blob();
                if (elementName == "ReferenceElement")
                    return new ReferenceElement();
                if (elementName == "RelationshipElement")
                    return new RelationshipElement();
                if (elementName == "SubmodelElementCollection")
                    return new SubmodelElementCollection();
                if (elementName == "Operation")
                    return new Operation();
                if (elementName == "OperationVariable")
                    return new OperationVariable();
                return null;
            }

            /// <summary>
            /// Can create SubmodelElements based on a numerical index
            /// </summary>
            /// <param name="index">Index 0..7 (6+7 are Operation..!)</param>
            /// <returns>SubmodelElement</returns>
            public static SubmodelElement CreateAdequateType(int index)
            {
                AdminShellV10.SubmodelElement sme = null;
                switch (index)
                {
                    case 0:
                        sme = new AdminShellV10.Property();
                        break;
                    case 1:
                        sme = new AdminShellV10.File();
                        break;
                    case 2:
                        sme = new AdminShellV10.Blob();
                        break;
                    case 3:
                        sme = new AdminShellV10.ReferenceElement();
                        break;
                    case 4:
                        sme = new AdminShellV10.SubmodelElementCollection();
                        break;
                    case 5:
                        sme = new AdminShellV10.RelationshipElement();
                        break;
                    case 6:
                        sme = new AdminShellV10.Operation();
                        break;
                    case 7:
                        sme = new AdminShellV10.OperationVariable();
                        break;
                }
                return sme;
            }

            public string GetFourDigitCode()
            {
                if (submodelElement == null)
                    return ("Null");
                if (submodelElement is AdminShellV10.Property) return ("Prop");
                if (submodelElement is AdminShellV10.File) return ("File");
                if (submodelElement is AdminShellV10.Blob) return ("Blob");
                if (submodelElement is AdminShellV10.ReferenceElement) return ("Ref");
                if (submodelElement is AdminShellV10.RelationshipElement) return ("Rel");
                if (submodelElement is AdminShellV10.SubmodelElementCollection) return ("Coll");
                if (submodelElement is AdminShellV10.Operation) return ("Opr");
                return ("Elem");
            }

            public static List<SubmodelElement> ListOfWrappersToListOfElems(List<SubmodelElementWrapper> wrappers)
            {
                var res = new List<SubmodelElement>();
                if (wrappers == null)
                    return res;
                foreach (var w in wrappers)
                    if (w.submodelElement != null)
                        res.Add(w.submodelElement);
                return res;
            }

            public static SubmodelElementWrapper CreateFor(SubmodelElement sme)
            {
                var res = new SubmodelElementWrapper();
                res.submodelElement = sme;
                return res;
            }

            public static Referable FindReferableByReference(List<SubmodelElementWrapper> wrappers, Reference rf, int keyIndex)
            {
                // first index needs to exist ..
                if (wrappers == null || rf == null || keyIndex >= rf.Count)
                    return null;

                // as SubmodelElements are not Identifiables, the actual key shall be IdSHort
                if (rf[keyIndex].idType.Trim().ToLower() != Key.GetIdentifierTypeName(Key.IdentifierType.IdShort).Trim().ToLower())
                    return null;

                // over all wrappers
                if (wrappers != null)
                    foreach (var smw in wrappers)
                        if (smw.submodelElement != null && smw.submodelElement.idShort.Trim().ToLower() == rf[keyIndex].value.Trim().ToLower())
                        {
                            // match on this level. Did we find a leaf element?
                            if ((keyIndex + 1) >= rf.Count)
                                return smw.submodelElement;

                            // ok, not a leaf, must be a recursion
                            // int SMEC
                            if (smw.submodelElement is SubmodelElementCollection)
                                return FindReferableByReference((smw.submodelElement as SubmodelElementCollection).value, rf, keyIndex + 1);

                            // TODO: Operation

                            // else: :-(
                            return null;
                        }

                // no?
                return null;
            }
        }

    }

    #endregion
}

#endif