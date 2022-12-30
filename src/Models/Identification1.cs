/*
Copyright (c) 2018-2021 Festo AG & Co. KG <https://www.festo.com/net/de_de/Forms/web/contact_international>
Author: Michael Hoffmeister

This source code is licensed under the Apache License 2.0 (see LICENSE.txt).

This source code may use other Open Source software components (see LICENSE.txt).
*/

using System.Xml;
using System.Xml.Serialization;

namespace AasxCompatibilityModels
{
    #region AdminShell_V2_0

    public partial class AdminShellV20
    {
        public class Identification
        {

            // members

            [XmlAttribute]
            
            public string idType = "";

            [XmlText]
            
            public string id = "";

            // some constants

            public static string IRDI = "IRDI";
            public static string IRI = "IRI";
            public static string IdShort = "IdShort";

            // constructors

            public Identification() { }

            public Identification(Identification src)
            {
                this.idType = src.idType;
                this.id = src.id;
            }

#if !DoNotUseAasxCompatibilityModels
            public Identification(AasxCompatibilityModels.AdminShellV10.Identification src)
            {
                this.idType = src.idType;
                if (this.idType.Trim().ToLower() == "uri")
                    this.idType = Identification.IRI;
                this.id = src.id;
            }
#endif

            public Identification(string idType, string id)
            {
                this.idType = idType;
                this.id = id;
            }

            public Identification(Key key)
            {
                this.idType = key.idType;
                this.id = key.value;
            }

            // Creator with validation

          

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

