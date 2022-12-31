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
        // ReSharper enable ClassNeverInstantiated.Global

        public class DataSpecificationContent
        {

            // members

            public DataSpecificationIEC61360 dataSpecificationIEC61360 = null;
            public DataSpecificationISO99999 dataSpecificationISO99999 = null;

            // constructors

            public DataSpecificationContent() { }

            public DataSpecificationContent(DataSpecificationContent src)
            {
                if (src.dataSpecificationIEC61360 != null)
                    this.dataSpecificationIEC61360 = new DataSpecificationIEC61360(src.dataSpecificationIEC61360);
            }

#if !DoNotUseAasxCompatibilityModels
            public DataSpecificationContent(AasxCompatibilityModels.AdminShellV10.DataSpecificationContent src)
            {
                if (src.dataSpecificationIEC61360 != null)
                    this.dataSpecificationIEC61360 = new DataSpecificationIEC61360(src.dataSpecificationIEC61360);
            }

            public DataSpecificationContent(AasxCompatibilityModels.AdminShellV20.DataSpecificationContent src)
            {
                if (src.dataSpecificationIEC61360 != null)
                    this.dataSpecificationIEC61360 = new DataSpecificationIEC61360(src.dataSpecificationIEC61360);
            }
#endif
        }

    }
}
