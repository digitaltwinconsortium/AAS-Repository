
using System;
using System.Collections.Concurrent;
using System.Data;
using System.Diagnostics;
using System.Net;
using System.Net.Http;

namespace AdminShell
{
    public class DynamicsDataService : IDisposable
    {
        private HttpClient _client = null;

        public DynamicsDataService()
        {
            _client = new();
        }

        public void Dispose()
        {
            if (_client != null)
            {
                _client.Dispose();
                _client = null;
            }
        }

        public void RunDynamicsQuery(string query, ConcurrentDictionary<string, object> values)
        {
            try
            {
                var response = _client.GetAsync(query).Result;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var content = response.Content.ReadAsStringAsync().Result;
                    var dataSet = new DataSet();
                    dataSet.ReadXml(new System.IO.StringReader(content));
                    if (dataSet.Tables.Count > 0)
                    {
                        var table = dataSet.Tables[0];
                        foreach (DataRow row in table.Rows)
                        {
                            foreach (DataColumn column in table.Columns)
                            {
                                values.TryAdd(column.ColumnName, row[column]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
