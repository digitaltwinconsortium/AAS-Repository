
namespace AdminShell
{
    public class DataSpecificationContent
    {
        public DataSpecificationIEC61360 dataSpecificationIEC61360 = new DataSpecificationIEC61360();

        public DataSpecificationContent() { }

        public DataSpecificationContent(DataSpecificationContent src)
        {
            if (src.dataSpecificationIEC61360 != null)
                dataSpecificationIEC61360 = new DataSpecificationIEC61360(src.dataSpecificationIEC61360);
        }
    }
}
