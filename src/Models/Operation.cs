
namespace AdminShell
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class Operation : SubmodelElement
    {
        [DataMember(Name = "inoutputVariable")]
        public List<OperationVariable> InoutputVariable { get; set; } = new();

        [DataMember(Name = "inputVariable")]
        public List<OperationVariable> InputVariable { get; set; } = new();

        [DataMember(Name = "outputVariable")]
        public List<OperationVariable> OutputVariable { get; set; } = new();

        public List<OperationVariable> this[OperationVariable.Direction dir]
        {
            get
            {
                if (dir == OperationVariable.Direction.In)
                    return InputVariable;
                else
                if (dir == OperationVariable.Direction.Out)
                    return OutputVariable;
                else
                    return InoutputVariable;
            }
            set
            {
                if (dir == OperationVariable.Direction.In)
                    InputVariable = value;
                else
                if (dir == OperationVariable.Direction.Out)
                    OutputVariable = value;
                else
                    InoutputVariable = value;
            }
        }

        public List<OperationVariable> this[int dir]
        {
            get
            {
                if (dir == 0)
                    return InputVariable;
                else
                if (dir == 1)
                    return OutputVariable;
                else
                    return InoutputVariable;
            }
            set
            {
                if (dir == 0)
                    InputVariable = value;
                else
                if (dir == 1)
                    OutputVariable = value;
                else
                    InoutputVariable = value;
            }
        }

        public static List<SubmodelElementWrapper> GetWrappers(List<OperationVariable> ovl)
        {
            var res = new List<SubmodelElementWrapper>();

            foreach (var ov in ovl)
                if (ov.Value != null)
                    res.Add(ov.Value);

            return res;
        }

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
    }
}

