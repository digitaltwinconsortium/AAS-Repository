
namespace AdminShell
{
    using Newtonsoft.Json;
    using System.Runtime.Serialization;

    [DataContract]
    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public enum ModelTypes
    {
        [EnumMember(Value = "Asset")]
        Asset = 0,

        [EnumMember(Value = "AssetAdministrationShell")]
        AssetAdministrationShell = 1,

        [EnumMember(Value = "ConceptDescription")]
        ConceptDescription = 2,

        [EnumMember(Value = "Submodel")]
        Submodel = 3,

        [EnumMember(Value = "AccessPermissionRule")]
        AccessPermissionRule = 4,

        [EnumMember(Value = "AnnotatedRelationshipElement")]
        AnnotatedRelationshipElement = 5,

        [EnumMember(Value = "BasicEvent")]
        BasicEvent = 6,

        [EnumMember(Value = "Blob")]
        Blob = 7,

        [EnumMember(Value = "Capability")]
        Capability = 8,

        [EnumMember(Value = "DataElement")]
        DataElement = 9,

        [EnumMember(Value = "File")]
        File = 10,

        [EnumMember(Value = "Entity")]
        Entity = 11,

        [EnumMember(Value = "Event")]
        Event = 12,

        [EnumMember(Value = "MultiLanguageProperty")]
        MultiLanguageProperty = 13,

        [EnumMember(Value = "Operation")]
        Operation = 14,

        [EnumMember(Value = "Property")]
        Property = 15,

        [EnumMember(Value = "Range")]
        Range = 16,

        [EnumMember(Value = "ReferenceElement")]
        ReferenceElement = 17,

        [EnumMember(Value = "RelationshipElement")]
        RelationshipElement = 18,

        [EnumMember(Value = "SubmodelElement")]
        SubmodelElement = 19,

        [EnumMember(Value = "SubmodelElementList")]
        SubmodelElementList = 20,

        [EnumMember(Value = "SubmodelElementStruct")]
        SubmodelElementStruct = 21,

        [EnumMember(Value = "View")]
        View = 22,

        [EnumMember(Value = "GlobalReference")]
        GlobalReference = 23,

        [EnumMember(Value = "FragmentReference")]
        FragmentReference = 24,

        [EnumMember(Value = "Constraint")]
        Constraint = 25,

        [EnumMember(Value = "Formula")]
        Formula = 26,

        [EnumMember(Value = "Qualifier")]
        Qualifier = 27
    }
}
