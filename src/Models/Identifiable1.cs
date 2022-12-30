/*
Copyright (c) 2018-2021 Festo AG & Co. KG <https://www.festo.com/net/de_de/Forms/web/contact_international>
Author: Michael Hoffmeister

This source code is licensed under the Apache License 2.0 (see LICENSE.txt).

This source code may use other Open Source software components (see LICENSE.txt).
*/

using System;
using System.Collections.Generic;
using System.Globalization;

namespace AasxCompatibilityModels
{
    #region AdminShell_V2_0

    public partial class AdminShellV20
    {
        public class Identifiable : Referable
        {

            // members

            public Identification identification = new Identification();
            public Administration administration = null;

            // constructors

            public Identifiable() : base() { }

            public Identifiable(string idShort) : base(idShort) { }

            public Identifiable(Identifiable src)
                : base(src)
            {
                if (src == null)
                    return;
                if (src.identification != null)
                    this.identification = new Identification(src.identification);
                if (src.administration != null)
                    this.administration = new Administration(src.administration);
            }

#if !DoNotUseAasxCompatibilityModels
            public Identifiable(AasxCompatibilityModels.AdminShellV10.Identifiable src)
                : base(src)
            {
                if (src.identification != null)
                    this.identification = new Identification(src.identification);
                if (src.administration != null)
                    this.administration = new Administration(src.administration);
            }
#endif

            public void SetIdentification(string idType, string id, string idShort = null)
            {
                identification.idType = idType;
                identification.id = id;
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
                if (identification != null && identification.id != "")
                    return AdminShellNS.AdminShellUtil.FilterFriendlyName(this.identification.id);
                return AdminShellNS.AdminShellUtil.FilterFriendlyName(this.idShort);
            }

            public override string ToString()
            {
                return ("" + identification?.ToString() + " " + administration?.ToString()).Trim();
            }

            // sorting

            public class ComparerIdentification : IComparer<Identifiable>
            {
                public int Compare(Identifiable a, Identifiable b)
                {
                    if (a?.identification == null && b?.identification == null)
                        return 0;
                    if (a?.identification == null)
                        return +1;
                    if (b?.identification == null)
                        return -1;

                    var vc = String.Compare(a.identification.idType, b.identification.idType,
                        CultureInfo.InvariantCulture, CompareOptions.IgnoreCase);
                    if (vc != 0)
                        return vc;

                    return String.Compare(a.identification.id, b.identification.id,
                        CultureInfo.InvariantCulture, CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace);
                }
            }

        }



    }

    #endregion
}

