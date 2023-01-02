#define UseAdminShell

using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;

/* Copyright (c) 2018-2019 Festo AG & Co. KG <https://www.festo.com/net/de_de/Forms/web/contact_international>, author: Michael Hoffmeister

This source code is licensed under the Apache License 2.0 (see LICENSE.txt).

This source code may use other Open Source software components (see LICENSE.txt).
*/


#if UseAdminShell

namespace AdminShell
{

    #region Utils

    #endregion


    #region AdminShell_V1_0

    public partial class AdminShellV10
    {
        public class Operation : SubmodelElement
        {
            // for JSON only
            [XmlIgnore]
            [JsonProperty(PropertyName = "modelType")]
            public new JsonModelTypeWrapper JsonModelType { get { return new JsonModelTypeWrapper(GetElementName()); } }

            // members
            
            [XmlElement(ElementName = "in")]
            public List<OperationVariable> valueIn = new List<OperationVariable>();

            
            [XmlElement(ElementName = "out")]
            public List<OperationVariable> valueOut = new List<OperationVariable>();

            [XmlIgnore]
            // MICHA 190504: enabled JSON operation variables!
            [JsonProperty(PropertyName = "in")]
            // 
            public OperationVariable[] JsonValueIn
            {
                get { return valueIn?.ToArray(); }
                set { valueIn = (value != null) ? new List<OperationVariable>(value) : null; }
            }

            [XmlIgnore]
            [JsonProperty(PropertyName = "out")]
            // MICHA 190504: enabled JSON operation variables!
            // 
            public OperationVariable[] JsonValueOut
            {
                get { return valueOut?.ToArray(); }
                set { valueOut = (value != null) ? new List<OperationVariable>(value) : null; }
            }

            public List<OperationVariable> this[OperationVariable.Direction dir]
            {
                get
                {
                    return (dir == OperationVariable.Direction.In) ? valueIn : valueOut;
                }
                set
                {
                    if (dir == OperationVariable.Direction.In)
                        valueIn = value;
                    else
                        valueOut = value;
                }
            }

            public List<OperationVariable> this[int dir]
            {
                get
                {
                    return (dir == 0) ? valueIn : valueOut;
                }
                set
                {
                    if (dir == 0)
                        valueIn = value;
                    else
                        valueOut = value;
                }
            }

            public static List<SubmodelElementWrapper> GetWrappers(List<OperationVariable> ovl)
            {
                var res = new List<SubmodelElementWrapper>();
                foreach (var ov in ovl)
                    if (ov.value != null)
                        res.Add(ov.value);
                return res;
            }

            // constructors

            public Operation() { }

            public Operation(Operation src)
                : base(src)
            {
                for (int i = 0; i < 2; i++)
                    if (src[i] != null)
                    {
                        if (this[i] == null)
                            this[i] = new List<OperationVariable>();
                        foreach (var ov in src[i])
                            this[i].Add(ov);
                    }
            }


            public override string GetElementName()
            {
                return "Operation";
            }
        }

    }

    #endregion
}

#endif