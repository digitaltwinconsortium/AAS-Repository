
namespace AdminShell
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class Operation : SubmodelElement
    {
        [DataMember(Name = "inoutputVariables")]
        public List<OperationVariable> InoutputVariables { get; set; } = new();

        [DataMember(Name = "inputVariables")]
        public List<OperationVariable> InputVariables { get; set; } = new();

        [DataMember(Name = "outputVariables")]
        public List<OperationVariable> OutputVariables { get; set; } = new();

        public List<OperationVariable> this[OperationVariable.Direction dir]
        {
            get
            {
                if (dir == OperationVariable.Direction.In)
                    return InputVariables;
                else
                if (dir == OperationVariable.Direction.Out)
                    return OutputVariables;
                else
                    return InoutputVariables;
            }
            set
            {
                if (dir == OperationVariable.Direction.In)
                    InputVariables = value;
                else
                if (dir == OperationVariable.Direction.Out)
                    OutputVariables = value;
                else
                    InoutputVariables = value;
            }
        }

        public List<OperationVariable> this[int dir]
        {
            get
            {
                if (dir == 0)
                    return InputVariables;
                else
                if (dir == 1)
                    return OutputVariables;
                else
                    return InoutputVariables;
            }
            set
            {
                if (dir == 0)
                    InputVariables = value;
                else
                if (dir == 1)
                    OutputVariables = value;
                else
                    InoutputVariables = value;
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

