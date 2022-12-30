#define UseAasxCompatibilityModels

using System.Xml;
using System.Xml.Serialization;

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
        public class Identification
        {

            // members

            [XmlAttribute]
            public string idType = "";
            [XmlText]
            public string id = "";

            // constructors

            public Identification() { }

            public Identification(string idType, string id)
            {
                this.idType = idType;
                this.id = id;
            }

            public Identification(Identification src)
            {
                this.idType = src.idType;
                this.id = src.id;
            }

            // Creator with validation

            public static Identification CreateNew(string idType, string id)
            {
                if (idType == null || id == null)
                    return null;
                var found = false;
                foreach (var x in Key.IdentifierTypeNames)
                    found = found || idType.ToLower().Trim() == x.ToLower().Trim();
                if (!found)
                    return null;
                return new Identification(idType, id);
            }

            // further

            public bool IsEqual(Identification other)
            {
                return (this.idType.Trim().ToLower() == other.idType.Trim().ToLower()
                    && this.id.Trim().ToLower() == other.id.Trim().ToLower());
            }

            public override string ToString()
            {
                return $"[{this.idType}] {this.id}";
            }
        }

    }

    #endregion
}

#endif