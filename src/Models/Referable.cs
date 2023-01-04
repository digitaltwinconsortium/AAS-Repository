
namespace AdminShell
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    public class Referable : IAasElement
    {
        [DataMember(Name = "category")]
        [MetaModelName("Referable.category")]
        public string Category { get; set; }

        [DataMember(Name = "description")]
        public Description Description { get; set; }

        [DataMember(Name = "displayName")]
        public string DisplayName { get; set; }

        [Required]
        [MetaModelName("Referable.idShort")]
        [DataMember(Name = "idShort")]
        public string IdShort { get; set; }

        [Required]
        [DataMember(Name = "modelType")]
        public ModelType ModelType { get; set; }

        [DataMember(Name = "checksum")]
        [MetaModelName("Referable.checksum")]
        public string Checksum { get; set; } = string.Empty;

        [XmlIgnore]
        public IAasElement Parent { get; set; }

        public static string CONSTANT = "CONSTANT";
        public static string Category_PARAMETER = "PARAMETER";
        public static string VARIABLE = "VARIABLE";

        public static string[] ReferableCategoryNames = new string[] { CONSTANT, Category_PARAMETER, VARIABLE };

        public List<Extension> extension = null;

        [XmlIgnore]
        public DateTime TimeStampCreate;

        [XmlIgnore]
        public DateTime TimeStamp;

        public void setTimeStamp(DateTime timeStamp)
        {
            Referable r = this;

            do
            {
                r.TimeStamp = timeStamp;
                if (r != r.Parent)
                {
                    r = (Referable)r.Parent;
                }
                else
                    r = null;
            }
            while (r != null);
        }

        public Referable() { }

        public Referable(Referable src)
        {
            if (src == null)
                return;

            IdShort = src.IdShort;

            Category = src.Category;

            if (src.Description != null)
                Description = new Description(src.Description);
        }

        public string CollectIdShortByParent()
        {
            // recurse first
            var head = "";
            if (!(this is Identifiable) && Parent is Referable prf)
                // can go up
                head = prf.CollectIdShortByParent() + "/";
            // add own
            var myid = "<no Id-Short!>";
            if (IdShort != null && IdShort.Trim() != "")
                myid = IdShort.Trim();
            // together
            return head + myid;
        }
    }
}
