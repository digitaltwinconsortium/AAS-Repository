#define UseAasxCompatibilityModels


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
        public class Identifiable : Referable
        {

            // members

            public Identification identification = new Identification();
            public Administration administration = null;

            // constructors

            public Identifiable() : base() { }

            public Identifiable(Identifiable src)
                : base(src)
            {
                if (src.identification != null)
                    this.identification = new Identification(src.identification);
                if (src.administration != null)
                    this.administration = new Administration(src.administration);
            }

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
                    return AdminShellUtil.FilterFriendlyName(this.identification.id);
                return AdminShellUtil.FilterFriendlyName(this.idShort);
            }

        }

    }

    #endregion
}

#endif