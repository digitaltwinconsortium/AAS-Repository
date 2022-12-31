/*
Copyright (c) 2018-2021 Festo AG & Co. KG <https://www.festo.com/net/de_de/Forms/web/contact_international>
Author: Michael Hoffmeister

This source code is licensed under the Apache License 2.0 (see LICENSE.txt).

This source code may use other Open Source software components (see LICENSE.txt).
*/

using System.Collections.Generic;

//namespace AdminShell
namespace AdminShell_V30
{

    #region AdminShell_V3_0

    public partial class AdminShellV30
    {
        /// <summary>
        /// This interfaces designates enitities, whích can enumerate their children.
        /// An optional placement can be provided (in/ out/ inout, index, ..)
        /// </summary>
        public interface IEnumerateChildren
        {
            IEnumerable<SubmodelElementWrapper> EnumerateChildren();
            EnumerationPlacmentBase GetChildrenPlacement(SubmodelElement child);
            object AddChild(SubmodelElementWrapper smw, EnumerationPlacmentBase placement = null);
        }

    }

    #endregion
}

