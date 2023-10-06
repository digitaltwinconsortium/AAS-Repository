
namespace AdminShell
{
    using System;
    using System.Collections.Concurrent;
    using System.Diagnostics;

    public class SMIPDataService : IDisposable
    {
        public SMIPDataService()
        {
            // TODO: connect to SMIP
        }

        public void Dispose()
        {
        }

        protected void RunSMIPQuery(string query, ConcurrentDictionary<string, object> values, bool allowMultiRow = false)
        {
            try
            {
               // TODO: Run GraphQL query
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
