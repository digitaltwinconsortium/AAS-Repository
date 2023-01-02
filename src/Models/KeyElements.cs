
namespace AdminShell
{
    using Newtonsoft.Json;
    using System.Runtime.Serialization;

    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public enum KeyElements
    {
        [EnumMember(Value = "Asset")]
        AssetEnum = 0,

        [EnumMember(Value = "AssetAdministrationShell")]
        AssetAdministrationShellEnum = 1,

        [EnumMember(Value = "ConceptDescription")]
        ConceptDescriptionEnum = 2,

        [EnumMember(Value = "Submodel")]
        SubmodelEnum = 3,

        [EnumMember(Value = "AccessPermissionRule")]
        AccessPermissionRuleEnum = 4,

        [EnumMember(Value = "AnnotatedRelationshipElement")]
        AnnotatedRelationshipElementEnum = 5,

        [EnumMember(Value = "BasicEvent")]
        BasicEventEnum = 6,

        [EnumMember(Value = "Blob")]
        BlobEnum = 7,

        [EnumMember(Value = "Capability")]
        CapabilityEnum = 8,

        [EnumMember(Value = "DataElement")]
        DataElementEnum = 9,

        [EnumMember(Value = "File")]
        FileEnum = 10,

        [EnumMember(Value = "Entity")]
        EntityEnum = 11,

        [EnumMember(Value = "Event")]
        EventEnum = 12,

        [EnumMember(Value = "MultiLanguageProperty")]
        MultiLanguagePropertyEnum = 13,

        [EnumMember(Value = "Operation")]
        OperationEnum = 14,

        [EnumMember(Value = "Property")]
        PropertyEnum = 15,

        [EnumMember(Value = "Range")]
        RangeEnum = 16,

        [EnumMember(Value = "ReferenceElement")]
        ReferenceElementEnum = 17,

        [EnumMember(Value = "RelationshipElement")]
        RelationshipElementEnum = 18,

        [EnumMember(Value = "SubmodelElement")]
        SubmodelElementEnum = 19,

        [EnumMember(Value = "SubmodelElementList")]
        SubmodelElementListEnum = 20,

        [EnumMember(Value = "SubmodelElementStruct")]
        SubmodelElementStructEnum = 21,

        [EnumMember(Value = "View")]
        ViewEnum = 22,

        [EnumMember(Value = "GlobalReference")]
        GlobalReferenceEnum = 23,

        [EnumMember(Value = "FragmentReference")]
        FragmentReferenceEnum = 24
    }
}
