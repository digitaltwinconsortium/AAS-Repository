/*
Copyright (c) 2018-2021 Festo AG & Co. KG <https://www.festo.com/net/de_de/Forms/web/contact_international>
Author: Michael Hoffmeister

This source code is licensed under the Apache License 2.0 (see LICENSE.txt).

This source code may use other Open Source software components (see LICENSE.txt).
*/

using AdminShell;
using Newtonsoft.Json;
using System.Xml.Serialization;

//namespace AdminShell
namespace AdminShell_V30
{

    #region AdminShell_V3_0

    public partial class AdminShellV30
    {
        public class ModelingKind
        {
            // constants
            public static string Template = "Template";
            public static string Instance = "Instance";

            [MetaModelName("ModelingKind.kind")]
            
            [XmlText]
            
            public string kind = Instance;

            // getters / setters

            [XmlIgnore]
            
            public bool IsInstance { get { return kind == null || kind.Trim().ToLower() == Instance.ToLower(); } }

            [XmlIgnore]
            
            public bool IsTemplate { get { return kind != null && kind.Trim().ToLower() == Template.ToLower(); } }

            // constructors / creators

            public ModelingKind() { }

            public ModelingKind(ModelingKind src)
            {
                kind = src.kind;
            }

#if !DoNotUseAasxCompatibilityModels
            public ModelingKind(AasxCompatibilityModels.AdminShellV10.Kind src)
            {
                kind = src.kind;
            }

            public ModelingKind(AasxCompatibilityModels.AdminShellV20.ModelingKind src)
            {
                kind = src.kind;
            }
#endif

            public ModelingKind(string kind)
            {
                this.kind = kind;
            }

            public static ModelingKind CreateAsTemplate()
            {
                var res = new ModelingKind() { kind = Template };
                return res;
            }

            public static ModelingKind CreateAsInstance()
            {
                var res = new ModelingKind() { kind = Instance };
                return res;
            }

            // validation

            public static void Validate(AasValidationRecordList results, ModelingKind mk, Referable container)
            {
                // access
                if (results == null || container == null)
                    return;

                // check
                if (mk == null || mk.kind == null)
                {
                    // warning
                    results.Add(new AasValidationRecord(
                        AasValidationSeverity.Warning, container,
                        "ModelingKind: is null",
                        () =>
                        {
                        }));
                }
                else
                {
                    var k = mk.kind.Trim();
                    var kl = k.ToLower();
                    if (kl != Template.ToLower() && kl != Instance.ToLower())
                    {
                        // violation case
                        results.Add(new AasValidationRecord(
                            AasValidationSeverity.SchemaViolation, container,
                            $"ModelingKind: enumeration value neither {Template} nor {Instance}",
                            () =>
                            {
                                mk.kind = Instance;
                            }));
                    }
                    else if (k != Template && k != Instance)
                    {
                        // warning
                        results.Add(new AasValidationRecord(
                            AasValidationSeverity.Warning, container,
                            "ModelingKind: enumeration value in wrong casing",
                            () =>
                            {
                                if (kl == Template.ToLower())
                                    mk.kind = Template;
                                else
                                    mk.kind = Instance;
                            }));
                    }
                }
            }
        }

    }

    #endregion
}

