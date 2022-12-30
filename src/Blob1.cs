/*
Copyright (c) 2018-2021 Festo AG & Co. KG <https://www.festo.com/net/de_de/Forms/web/contact_international>
Author: Michael Hoffmeister

This source code is licensed under the Apache License 2.0 (see LICENSE.txt).

This source code may use other Open Source software components (see LICENSE.txt).
*/

using System.Collections.Generic;

namespace AasxCompatibilityModels
{
    #region AdminShell_V2_0

    public partial class AdminShellV20
    {
        public class Blob : DataElement
        {
            // members

            [MetaModelName("Blob.mimeType")]
            
            
            public string mimeType = "";

            [MetaModelName("Blob.value")]
            
            
            public string value = "";

            // constructors

            public Blob(SubmodelElement src)
                : base(src)
            {
                if (!(src is Blob blb))
                    return;

                this.mimeType = blb.mimeType;
                this.value = blb.value;
            }

#if !DoNotUseAasxCompatibilityModels
            public Blob(AasxCompatibilityModels.AdminShellV10.Blob src)
                : base(src)
            {
                if (src == null)
                    return;

                this.mimeType = src.mimeType;
                this.value = src.value;
            }
#endif

            public override AasElementSelfDescription GetSelfDescription()
            {
                return new AasElementSelfDescription("Blob", "Blob");
            }

            public override object ToValueOnlySerialization()
            {
                var output = new Dictionary<string, Dictionary<string, string>>();
                var valueDict = new Dictionary<string, string>
                {
                    { "mimeType", mimeType },
                };

                output.Add(idShort, valueDict);
                return output;
            }
        }



    }

    #endregion
}

