/*
Copyright (c) 2018-2021 Festo AG & Co. KG <https://www.festo.com/net/de_de/Forms/web/contact_international>
Author: Michael Hoffmeister

This source code is licensed under the Apache License 2.0 (see LICENSE.txt).

This source code may use other Open Source software components (see LICENSE.txt).
*/

using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace AasxCompatibilityModels
{
    #region AdminShell_V2_0

    public partial class AdminShellV20
    {
#if __not_valid_anymore
#else
#endif

        [XmlType(TypeName = "ContainedElements")]
        public class ContainedElements
        {

            // members

            [XmlElement(ElementName = "containedElementRef")] // make "reference" go away by magic?!
            public List<ContainedElementRef> reference = new List<ContainedElementRef>();

            // getter / setter

            public bool IsEmpty { get { return reference == null || reference.Count < 1; } }
            public int Count { get { if (reference == null) return 0; return reference.Count; } }
            public ContainedElementRef this[int index] { get { return reference[index]; } }

            // Creators

            public ContainedElements() { }

            public ContainedElements(ContainedElements src)
            {
                if (src.reference != null)
                    foreach (var r in src.reference)
                        this.reference.Add(new ContainedElementRef(r));
            }

#if !DoNotUseAasxCompatibilityModels
            public ContainedElements(AasxCompatibilityModels.AdminShellV10.ContainedElements src)
            {
                if (src.reference != null)
                    foreach (var r in src.reference)
                        this.reference.Add(new ContainedElementRef(r));
            }
#endif

            public static ContainedElements CreateOrSetInner(ContainedElements outer, ContainedElementRef[] inner)
            {
                var res = outer;
                if (res == null)
                    res = new ContainedElements();
                if (inner == null)
                {
                    res.reference = null;
                    return res;
                }
                res.reference = new List<ContainedElementRef>(inner);
                return res;
            }

        }



    }

    #endregion
}

