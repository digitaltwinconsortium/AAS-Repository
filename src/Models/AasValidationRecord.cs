/*
Copyright (c) 2018-2019 Festo AG & Co. KG <https://www.festo.com/net/de_de/Forms/web/contact_international>
Author: Michael Hoffmeister

This source code is licensed under the Apache License 2.0 (see LICENSE.txt).

This source code may use other Open Source software components (see LICENSE.txt).
*/

using AdminShell_V30;
using System;

namespace AdminShellNS
{
    public class AasValidationRecord
    {
        public AasValidationSeverity Severity = AasValidationSeverity.Hint;
        public AdminShell.Referable Source = null;
        public string Message = "";

        public Action Fix = null;

        public AasValidationRecord(AasValidationSeverity Severity, AdminShell.Referable Source,
            string Message, Action Fix = null)
        {
            this.Severity = Severity;
            this.Source = Source;
            this.Message = Message;
            this.Fix = Fix;
        }

        public override string ToString()
        {
            return $"[{Severity.ToString()}] in {"" + Source?.ToString()}: {"" + Message}";
        }

        public string DisplaySeverity { get { return "" + Severity.ToString(); } }
        public string DisplaySource
        {
            get
            {
                return "" + ((Source != null) ? Source.ToString() : "(whole content)");
            }
        }
        public string DisplayMessage { get { return "" + Message?.ToString(); } }
    }
}
