/*
Copyright (c) 2018-2021 Festo AG & Co. KG <https://www.festo.com/net/de_de/Forms/web/contact_international>
Author: Michael Hoffmeister

This source code is licensed under the Apache License 2.0 (see LICENSE.txt).

This source code may use other Open Source software components (see LICENSE.txt).
*/



//namespace AdminShell
namespace AdminShell_V30
{
    public partial class AdminShellV30
    {
        public class DataElementWrapperCollection : BaseSubmodelElementWrapperCollection<DataElement>
        {
            public DataElementWrapperCollection() : base() { }

            public DataElementWrapperCollection(SubmodelElementWrapperCollection other)
                : base(other)
            {
            }

            public DataElementWrapperCollection(DataElementWrapperCollection other)
                : base()
            {
                foreach (var wo in other)
                    this.Add(wo);
            }

#if !DoNotUseAasxCompatibilityModels
            public DataElementWrapperCollection(AasxCompatibilityModels.AdminShellV20.DataElementWrapperCollection src)
                : base()
            {
                foreach (var wo in src)
                    this.Add(new SubmodelElementWrapper(wo?.submodelElement));
            }
#endif

        }

    }
}
