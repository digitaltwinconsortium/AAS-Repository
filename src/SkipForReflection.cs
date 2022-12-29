/*
Copyright (c) 2018-2021 Festo AG & Co. KG <https://www.festo.com/net/de_de/Forms/web/contact_international>
Author: Michael Hoffmeister

This source code is licensed under the Apache License 2.0 (see LICENSE.txt).

This source code may use other Open Source software components (see LICENSE.txt).
*/

namespace AasxCompatibilityModels
{
    #region AdminShell_V2_0

    public partial class AdminShellV20
    {
        /// <summary>
        /// This attribute indicates, that the field / property shall be skipped for reflection
        /// in order to avoid cycles
        /// </summary>
        [System.AttributeUsage(System.AttributeTargets.Field | System.AttributeTargets.Property, AllowMultiple = true)]
        public class SkipForReflection : System.Attribute
        {
        }



    }

    #endregion
}

