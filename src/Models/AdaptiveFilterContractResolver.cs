
namespace AdminShell
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using System.Reflection;

    public class AdminShellConverters
    {
        /// <summary>
        /// This converter / contract resolver for Newtonsoft Json adaptively filters different levels of depth
        /// of nested AASX structures.
        /// </summary>
        public class AdaptiveFilterContractResolver : DefaultContractResolver
        {
            public bool AasHasViews = true;
            public bool BlobHasValue = true;
            public bool SubmodelHasElements = true;
            public bool SmcHasValue = true;
            public bool OpHasVariables = true;

            public AdaptiveFilterContractResolver() { }

            public AdaptiveFilterContractResolver(bool deep = true, bool complete = true)
            {
                if (!deep)
                {
                    SubmodelHasElements = false;
                    SmcHasValue = false;
                    OpHasVariables = false;
                }
                if (!complete)
                {
                    AasHasViews = false;
                    BlobHasValue = false;
                }

            }

            protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
            {
                JsonProperty property = base.CreateProperty(member, memberSerialization);

                if (!BlobHasValue && property.DeclaringType == typeof(AdminShell.Blob) && property.PropertyName == "Value")
                    property.ShouldSerialize = instance => { return false; };

                if (!SubmodelHasElements && property.DeclaringType == typeof(AdminShell.Submodel) && property.PropertyName == "submodelElements")
                    property.ShouldSerialize = instance => { return false; };

                if (!SmcHasValue && property.DeclaringType == typeof(AdminShell.SubmodelElementCollection) && property.PropertyName == "Value")
                    property.ShouldSerialize = instance => { return false; };

                if (!OpHasVariables && property.DeclaringType == typeof(AdminShell.Operation) && (property.PropertyName == "in" || property.PropertyName == "out"))
                    property.ShouldSerialize = instance => { return false; };

                if (!AasHasViews && property.DeclaringType == typeof(AdminShell.AdministrationShell) && property.PropertyName == "views")
                    property.ShouldSerialize = instance => { return false; };

                return property;
            }
        }

    }
}
