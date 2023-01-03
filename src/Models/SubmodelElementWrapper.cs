
namespace AdminShell
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml;
    using System.Xml.Serialization;

    [XmlType(TypeName = "submodelElement")]
    public class SubmodelElementWrapper
    {
        [XmlElement(ElementName = "property", Type = typeof(Property))]
        [XmlElement(ElementName = "multiLanguageProperty", Type = typeof(MultiLanguageProperty))]
        [XmlElement(ElementName = "range", Type = typeof(Range))]
        [XmlElement(ElementName = "file", Type = typeof(File))]
        [XmlElement(ElementName = "blob", Type = typeof(Blob))]
        [XmlElement(ElementName = "referenceElement", Type = typeof(ReferenceElement))]
        [XmlElement(ElementName = "relationshipElement", Type = typeof(RelationshipElement))]
        [XmlElement(ElementName = "annotatedRelationshipElement", Type = typeof(AnnotatedRelationshipElement))]
        [XmlElement(ElementName = "capability", Type = typeof(Capability))]
        [XmlElement(ElementName = "submodelElementCollection", Type = typeof(SubmodelElementCollection))]
        [XmlElement(ElementName = "operation", Type = typeof(Operation))]
        [XmlElement(ElementName = "basicEvent", Type = typeof(BasicEvent))]
        [XmlElement(ElementName = "entity", Type = typeof(Entity))]
        [XmlElement(ElementName = "submodelElementList", Type = typeof(SubmodelElementList))]
        [XmlElement(ElementName = "submodelElementStruct", Type = typeof(SubmodelElementStruct))]
        [XmlElement(ElementName = "globalReferenceElement", Type = typeof(GlobalReferenceElement))]
        [XmlElement(ElementName = "modelReferenceElement", Type = typeof(ModelReferenceElement))]
        public SubmodelElement submodelElement;

        [XmlIgnore]
        public static string MetaModelVersionCoarse = "AAS3.0";

        [XmlIgnore]
        public static string MetaModelVersionFine = "RC02";

        public enum AdequateElementEnum
        {
            Unknown = 0, SubmodelElementCollection, Property, MultiLanguageProperty, Range, File, Blob,
            ReferenceElement, RelationshipElement, AnnotatedRelationshipElement, Capability, Operation,
            BasicEvent, Entity, SubmodelElementList, SubmodelElementStruct,
            ModelReferenceElement, GlobalReferenceElement
        }

        public static AdequateElementEnum[] AdequateElementsDataElement =
        {
            AdequateElementEnum.SubmodelElementCollection, AdequateElementEnum.RelationshipElement,
            AdequateElementEnum.AnnotatedRelationshipElement, AdequateElementEnum.Capability,
            AdequateElementEnum.Operation, AdequateElementEnum.BasicEvent, AdequateElementEnum.Entity
        };

        // shall be consistent to (int) AdequateElementEnum !
        public static string[] AdequateElementNames = {
            "Unknown", "SubmodelElementCollection", "Property",
            "MultiLanguageProperty", "Range", "File", "Blob", "ReferenceElement", "RelationshipElement",
            "AnnotatedRelationshipElement", "Capability", "Operation", "BasicEvent", "Entity",
            "SubmodelElementList", "SubmodelElementStruct",
            "ModelReferenceElement", "GlobalReferenceElement"
        };

        // shall be consistent to (int) AdequateElementEnum !
        public static string[] AdequateElementShortName = {
            null, "SMC", null,
            "MLP", null, null, null, "Ref", "Rel",
            "ARel", null, null, "Event", "Entity", "SML", "SMS",
            "RefM", "RefG"
        };

        // shall be consistent to (int) AdequateElementEnum !
        public static bool[] AdequateElementDeprecated = {
            false, true, false,
            false, false, false, false, true, false,
            false, false, false, false, false, false, false,
            false, false
        };

        public SubmodelElementWrapper() { }

        public SubmodelElementWrapper(SubmodelElement src, bool shallowCopy = false)
        {
            if (src is SubmodelElementCollection)
                this.submodelElement = new SubmodelElementCollection(
                    src as SubmodelElementCollection, shallowCopy: shallowCopy);
            if (src is Property)
                this.submodelElement = new Property(src as Property);
            if (src is MultiLanguageProperty)
                this.submodelElement = new MultiLanguageProperty(src as MultiLanguageProperty);
            if (src is Range)
                this.submodelElement = new Range(src as Range);
            if (src is File)
                this.submodelElement = new File(src as File);
            if (src is Blob)
                this.submodelElement = new Blob(src as Blob);
            if (src is ReferenceElement)
                this.submodelElement = new ReferenceElement(src as ReferenceElement);
            if (src is RelationshipElement)
                this.submodelElement = new RelationshipElement(src as RelationshipElement);
            if (src is AnnotatedRelationshipElement)
                this.submodelElement = new AnnotatedRelationshipElement(src as AnnotatedRelationshipElement);
            if (src is Capability)
                this.submodelElement = new Capability(src as Capability);
            if (src is Operation)
                this.submodelElement = new Operation(src as Operation);
            if (src is BasicEvent)
                this.submodelElement = new BasicEvent(src as BasicEvent);
            if (src is Entity)
                this.submodelElement = new Entity(src as Entity);

            // care abuot elements, which could be derived from (not so) abstract base types
            if (src is SubmodelElementList)
                this.submodelElement = new SubmodelElementList(src as SubmodelElementList);
            else
            if (src is SubmodelElementStruct)
                this.submodelElement = new SubmodelElementStruct(src as SubmodelElementStruct);
            else
            if (src is SubmodelElementCollection)
                this.submodelElement = new SubmodelElementCollection(
                    src as SubmodelElementCollection, shallowCopy: shallowCopy);

            // again
            if (src is AnnotatedRelationshipElement)
                this.submodelElement = new AnnotatedRelationshipElement(src as AnnotatedRelationshipElement);
            else
            if (src is RelationshipElement)
                this.submodelElement = new RelationshipElement(src as RelationshipElement);

            // again
            if (src is ModelReferenceElement)
                this.submodelElement = new ModelReferenceElement(src as ModelReferenceElement);
            else
                if (src is GlobalReferenceElement)
                this.submodelElement = new GlobalReferenceElement(src as GlobalReferenceElement);
            else
                if (src is ReferenceElement)
                this.submodelElement = new ReferenceElement(src as ReferenceElement);

        }

        // Deprecated. See below.
        public static AdequateElementEnum GetAdequateEnum(string adequateName)
        {
            if (adequateName == null)
                return AdequateElementEnum.Unknown;

            foreach (var en in (AdequateElementEnum[])Enum.GetValues(typeof(AdequateElementEnum)))
                if (Enum.GetName(typeof(AdequateElementEnum), en)?.Trim().ToLower() ==
                    adequateName.Trim().ToLower())
                    return en;

            return AdequateElementEnum.Unknown;
        }

        // Introduced for JSON serialization, can create SubModelElements based on a string name
        public static SubmodelElement CreateAdequateType(AdequateElementEnum ae, SubmodelElement src = null)
        {
            if (ae == AdequateElementEnum.Property)
                return new Property(src);
            if (ae == AdequateElementEnum.MultiLanguageProperty)
                return new MultiLanguageProperty(src);
            if (ae == AdequateElementEnum.Range)
                return new Range(src);
            if (ae == AdequateElementEnum.File)
                return new File(src);
            if (ae == AdequateElementEnum.Blob)
                return new Blob(src);
            if (ae == AdequateElementEnum.ReferenceElement)
                return new ReferenceElement(src);
            if (ae == AdequateElementEnum.RelationshipElement)
                return new RelationshipElement(src);
            if (ae == AdequateElementEnum.AnnotatedRelationshipElement)
                return new AnnotatedRelationshipElement(src);
            if (ae == AdequateElementEnum.Capability)
                return new Capability(src);
            if (ae == AdequateElementEnum.SubmodelElementCollection)
                return new SubmodelElementCollection(src);
            if (ae == AdequateElementEnum.Operation)
                return new Operation(src);
            if (ae == AdequateElementEnum.BasicEvent)
                return new BasicEvent(src);
            if (ae == AdequateElementEnum.Entity)
                return new Entity(src);
            if (ae == AdequateElementEnum.SubmodelElementList)
                return new SubmodelElementList(src);
            if (ae == AdequateElementEnum.SubmodelElementStruct)
                return new SubmodelElementStruct(src);
            if (ae == AdequateElementEnum.ModelReferenceElement)
                return new ModelReferenceElement(src);
            if (ae == AdequateElementEnum.GlobalReferenceElement)
                return new GlobalReferenceElement(src);
            return null;
        }

        // Can create SubmodelElements based on a given Type information
        public static SubmodelElement CreateAdequateType(Type t)
        {
            if (t == null || !t.IsSubclassOf(typeof(AdminShell.SubmodelElement)))
                return null;
            var sme = Activator.CreateInstance(t) as SubmodelElement;
            return sme;
        }

        public static SubmodelElementWrapper CreateFor(SubmodelElement sme)
        {
            var res = new SubmodelElementWrapper() { submodelElement = sme };
            return res;
        }

        public static Referable FindReferableByReference(
            List<SubmodelElementWrapper> wrappers, ModelReference rf, int keyIndex)
        {
            return FindReferableByReference(wrappers, rf?.Keys, keyIndex);
        }

        public static Referable FindReferableByReference(
            List<SubmodelElementWrapper> wrappers, KeyList rf, int keyIndex)
        {
            // first index needs to exist ..
            if (wrappers == null || rf == null || keyIndex >= rf.Count)
                return null;

            // as SubmodelElements are not Identifiables, the actual key shall be IdShort
            if (!rf[keyIndex].IsInSubmodelElements())
                return null;

            // over all wrappers
            foreach (var smw in wrappers)
                if (smw.submodelElement != null &&
                    smw.submodelElement.IdShort.Trim().ToLower() == rf[keyIndex].Value.Trim().ToLower())
                {
                    // match on this level. Did we find a leaf element?
                    if ((keyIndex + 1) >= rf.Count)
                        return smw.submodelElement;

                    // dive into SMC?
                    if (smw.submodelElement is SubmodelElementCollection smc)
                    {
                        var found = FindReferableByReference(smc.Value, rf, keyIndex + 1);
                        if (found != null)
                            return found;
                    }

                    // dive into Entity statements?
                    if (smw.submodelElement is Entity ent)
                    {
                        var found = FindReferableByReference(ent.statements, rf, keyIndex + 1);
                        if (found != null)
                            return found;
                    }

                    // else:
                    return null;
                }

            // no?
            return null;
        }
    }
}
