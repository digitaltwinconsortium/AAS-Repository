
namespace AdminShell
{
    using System;

    public class AasValidationRecord
    {
        public AasValidationSeverity Severity = AasValidationSeverity.Hint;

        public Referable Source = null;

        public string Message = string.Empty;

        public Action Fix = null;

        public AasValidationRecord(AasValidationSeverity Severity, AdminShell.Referable Source,
            string Message, Action Fix = null)
        {
            this.Severity = Severity;
            this.Source = Source;
            this.Message = Message;
            this.Fix = Fix;
        }
    }
}
