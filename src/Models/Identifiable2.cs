/*
Copyright (c) 2018-2021 Festo AG & Co. KG <https://www.festo.com/net/de_de/Forms/web/contact_international>
Author: Michael Hoffmeister

This source code is licensed under the Apache License 2.0 (see LICENSE.txt).

This source code may use other Open Source software components (see LICENSE.txt).
*/

using AdminShell;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Serialization;

//namespace AdminShell
namespace AdminShell_V30
{

    #region AdminShell_V3_0

    public partial class AdminShellV30
    {
        public class Identifiable : Referable, IGetModelReference
        {

            // members

            // this is complex, because V3.0 made id a simple string
            // and this should be translated to the outside.
            // TODO (MIHO, 2021-12-30): consider a converter for this
            // https://stackoverflow.com/questions/24472404/json-net-how-to-serialize-object-as-value

            
            public Identifier id = new Identifier();
            [XmlIgnore]
            [JsonProperty(PropertyName = "id")]
            public string JsonId
            {
                get { return id?.value; }
                set
                {
                    if (id == null)
                        id = new Identifier(value);
                    else
                        id.value = value;
                }
            }

            // rest of members

            public Administration administration = null;

            // constructors

            public Identifiable() : base() { }

            public Identifiable(string idShort) : base(idShort) { }

            public Identifiable(Identifiable src)
                : base(src)
            {
                if (src == null)
                    return;
                if (src.id != null)
                    this.id = new Identifier(src.id);
                if (src.administration != null)
                    this.administration = new Administration(src.administration);
            }

#if !DoNotUseAasxCompatibilityModels
            public Identifiable(AasxCompatibilityModels.AdminShellV10.Identifiable src)
                : base(src)
            {
                if (src.identification != null)
                    this.id = new Identifier(src.identification);
                if (src.administration != null)
                    this.administration = new Administration(src.administration);
            }

            public Identifiable(AasxCompatibilityModels.AdminShellV20.Identifiable src)
                : base(src)
            {
                if (src.identification != null)
                    this.id = new Identifier(src.identification);
                if (src.administration != null)
                    this.administration = new Administration(src.administration);
            }
#endif

            public void SetIdentification(string id, string idShort = null)
            {
                this.id.value = id;
                if (idShort != null)
                    this.idShort = idShort;
            }

            public void SetAdminstration(string version, string revision)
            {
                if (administration == null)
                    administration = new Administration();
                administration.version = version;
                administration.revision = revision;
            }

            public new string GetFriendlyName()
            {
                if (id != null && id.value != "")
                    return AdminShellUtil.FilterFriendlyName(this.id.value);
                return AdminShellUtil.FilterFriendlyName(this.idShort);
            }

            public override string ToString()
            {
                return ("" + id?.ToString() + " " + administration?.ToString()).Trim();
            }

            public override Key ToKey()
            {
                return new Key(GetElementName(), "" + id?.value);
            }

            // self description

            public override ModelReference GetModelReference(bool includeParents = true)
            {
                var r = new ModelReference();

                if (this is IGetSemanticId igs)
                    r.referredSemanticId = igs.GetSemanticId();

                r.Keys.Add(
                    Key.CreateNew(this.GetElementName(), this.id.value));
                return r;
            }

            // sorting

            public class ComparerIdentification : IComparer<Identifiable>
            {
                public int Compare(Identifiable a, Identifiable b)
                {
                    if (a?.id == null && b?.id == null)
                        return 0;
                    if (a?.id == null)
                        return +1;
                    if (b?.id == null)
                        return -1;

                    return String.Compare(a.id.value, b.id.value,
                        CultureInfo.InvariantCulture, CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace);
                }
            }

        }

    }

    #endregion
}

