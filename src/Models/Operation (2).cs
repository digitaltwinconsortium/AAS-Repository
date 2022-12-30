/*
Copyright (c) 2018-2021 Festo AG & Co. KG <https://www.festo.com/net/de_de/Forms/web/contact_international>
Author: Michael Hoffmeister

This source code is licensed under the Apache License 2.0 (see LICENSE.txt).

This source code may use other Open Source software components (see LICENSE.txt).
*/

using Newtonsoft.Json;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;


//namespace AdminShellNS
namespace AdminShell_V30
{
    public partial class AdminShellV30
    {
        public class Operation : SubmodelElement, IEnumerateChildren
        {
            // for JSON only
            [XmlIgnore]
            [JsonProperty(PropertyName = "modelType")]
            public new JsonModelTypeWrapper JsonModelType
            {
                get { return new JsonModelTypeWrapper(GetElementName()); }
            }

            // members
            
            [XmlElement(ElementName = "inputVariable")]
             // do NOT count children!
            public List<OperationVariable> inputVariable = new List<OperationVariable>();

            
            [XmlElement(ElementName = "outputVariable")]
             // do NOT count children!
            public List<OperationVariable> outputVariable = new List<OperationVariable>();

            
            [XmlElement(ElementName = "inoutputVariable")]
             // do NOT count children!
            public List<OperationVariable> inoutputVariable = new List<OperationVariable>();

            [XmlIgnore]
            // MICHA 190504: enabled JSON operation variables!
            [JsonProperty(PropertyName = "inputVariable")]
            public OperationVariable[] JsonInputVariable
            {
                get { return inputVariable?.ToArray(); }
                set { inputVariable = (value != null) ? new List<OperationVariable>(value) : null; }
            }

            [XmlIgnore]
            [JsonProperty(PropertyName = "outputVariable")]
            // MICHA 190504: enabled JSON operation variables!
            public OperationVariable[] JsonOutputVariable
            {
                get { return outputVariable?.ToArray(); }
                set { outputVariable = (value != null) ? new List<OperationVariable>(value) : null; }
            }

            [XmlIgnore]
            [JsonProperty(PropertyName = "inoutputVariable")]
            // MICHA 190504: enabled JSON operation variables!
            public OperationVariable[] JsonInOutputVariable
            {
                get { return inoutputVariable?.ToArray(); }
                set { inoutputVariable = (value != null) ? new List<OperationVariable>(value) : null; }
            }

            public List<OperationVariable> this[OperationVariable.Direction dir]
            {
                get
                {
                    if (dir == OperationVariable.Direction.In)
                        return inputVariable;
                    else
                    if (dir == OperationVariable.Direction.Out)
                        return outputVariable;
                    else
                        return inoutputVariable;
                }
                set
                {
                    if (dir == OperationVariable.Direction.In)
                        inputVariable = value;
                    else
                    if (dir == OperationVariable.Direction.Out)
                        outputVariable = value;
                    else
                        inoutputVariable = value;
                }
            }

            public List<OperationVariable> this[int dir]
            {
                get
                {
                    if (dir == 0)
                        return inputVariable;
                    else
                    if (dir == 1)
                        return outputVariable;
                    else
                        return inoutputVariable;
                }
                set
                {
                    if (dir == 0)
                        inputVariable = value;
                    else
                    if (dir == 1)
                        outputVariable = value;
                    else
                        inoutputVariable = value;
                }
            }

            public static SubmodelElementWrapperCollection GetWrappers(List<OperationVariable> ovl)
            {
                var res = new SubmodelElementWrapperCollection();
                foreach (var ov in ovl)
                    if (ov.value != null)
                        res.Add(ov.value);
                return res;
            }

            // enumartes its children
            public IEnumerable<SubmodelElementWrapper> EnumerateChildren()
            {
                if (this.inputVariable != null)
                    foreach (var smw in this.inputVariable)
                        yield return smw?.value;

                if (this.outputVariable != null)
                    foreach (var smw in this.outputVariable)
                        yield return smw?.value;

                if (this.inoutputVariable != null)
                    foreach (var smw in this.inoutputVariable)
                        yield return smw?.value;
            }

            public class EnumerationPlacmentOperationVariable : EnumerationPlacmentBase
            {
                public OperationVariable.Direction Direction;
                public OperationVariable OperationVariable;
            }

            public EnumerationPlacmentBase GetChildrenPlacement(SubmodelElement child)
            {
                // trivial
                if (child == null)
                    return null;

                // search
                OperationVariable.Direction? dir = null;
                OperationVariable opvar = null;
                if (this.inputVariable != null)
                    foreach (var ov in this.inputVariable)
                        if (ov?.value?.submodelElement == child)
                        {
                            dir = OperationVariable.Direction.In;
                            opvar = ov;
                        }

                if (this.outputVariable != null)
                    foreach (var ov in this.outputVariable)
                        if (ov?.value?.submodelElement == child)
                        {
                            dir = OperationVariable.Direction.Out;
                            opvar = ov;
                        }

                if (this.inoutputVariable != null)
                    foreach (var ov in this.inoutputVariable)
                        if (ov?.value?.submodelElement == child)
                        {
                            dir = OperationVariable.Direction.InOut;
                            opvar = ov;
                        }

                // found
                if (!dir.HasValue)
                    return null;
                return new EnumerationPlacmentOperationVariable()
                {
                    Direction = dir.Value,
                    OperationVariable = opvar
                };
            }

            public object AddChild(SubmodelElementWrapper smw, EnumerationPlacmentBase placement = null)
            {
                // not enough information to select list of children?
                var pl = placement as EnumerationPlacmentOperationVariable;
                if (smw == null || pl == null)
                    return null;

                // ok, use information
                var ov = new OperationVariable();
                ov.value = smw;

                if (smw.submodelElement != null)
                    smw.submodelElement.parent = this;

                if (pl.Direction == OperationVariable.Direction.In)
                {
                    if (inputVariable == null)
                        inputVariable = new List<OperationVariable>();
                    inputVariable.Add(ov);
                }

                if (pl.Direction == OperationVariable.Direction.Out)
                {
                    if (outputVariable == null)
                        outputVariable = new List<OperationVariable>();
                    outputVariable.Add(ov);
                }

                if (pl.Direction == OperationVariable.Direction.InOut)
                {
                    if (inoutputVariable == null)
                        inoutputVariable = new List<OperationVariable>();
                    inoutputVariable.Add(ov);
                }

                return ov;
            }

            // constructors

            public Operation() { }

            public Operation(SubmodelElement src)
                : base(src)
            {
                if (!(src is Operation op))
                    return;

                for (int i = 0; i < 2; i++)
                    if (op[i] != null)
                    {
                        if (this[i] == null)
                            this[i] = new List<OperationVariable>();
                        foreach (var ov in op[i])
                            this[i].Add(new OperationVariable(ov));
                    }
            }

#if !DoNotUseAasxCompatibilityModels
            public Operation(AasxCompatibilityModels.AdminShellV10.Operation src)
                : base(src)
            {
                for (int i = 0; i < 2; i++)
                    if (src[i] != null)
                    {
                        if (this[i] == null)
                            this[i] = new List<OperationVariable>();
                        foreach (var ov in src[i])
                            this[i].Add(new OperationVariable(ov));
                    }
            }

            public Operation(AasxCompatibilityModels.AdminShellV20.Operation src)
                : base(src)
            {
                for (int i = 0; i < 2; i++)
                    if (src[i] != null)
                    {
                        if (this[i] == null)
                            this[i] = new List<OperationVariable>();
                        foreach (var ov in src[i])
                            this[i].Add(new OperationVariable(ov));
                    }
            }
#endif

            public override AasElementSelfDescription GetSelfDescription()
            {
                return new AasElementSelfDescription("Operation", "Opr",
                    SubmodelElementWrapper.AdequateElementEnum.Operation);
            }
        }

    }
}
