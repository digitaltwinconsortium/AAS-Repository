
namespace AdminShell
{
    using AdminShell;
    using System.IO;
    using System.Xml.Serialization;

    /// <summary>
    /// Provides (static?) helpers for serializing AAS..
    /// </summary>
    public static class AdminShellSerializationHelper
    {

        public static string TryReadXmlFirstElementNamespaceURI(Stream s)
        {
            string res = null;

            try
            {
                var xr = System.Xml.XmlReader.Create(s);
                int i = 0;
                while (xr.Read())
                {
                    // limit amount of read
                    i++;
                    if (i > 99)
                        // obviously not found
                        break;

                    // find element
                    if (xr.NodeType == System.Xml.XmlNodeType.Element)
                    {
                        res = xr.NamespaceURI;
                        break;
                    }
                }
                xr.Close();
            }
            catch { }

            // give back
            return res;
        }

        /// <summary>
        /// De-serialize an open stream into AssetAdministrationShellEnvironment. Does Version/ compatibility management.
        /// </summary>
        /// <param name="s">Open for read stream</param>
        /// <returns></returns>
        public static AssetAdministrationShellEnvironment DeserializeXmlFromStreamWithCompat(Stream s)
        {
            // try get first element
            var nsuri = TryReadXmlFirstElementNamespaceURI(s);

            // read V1.0
            if (nsuri != null && nsuri.Trim() == "http://www.admin-shell.io/aas/1/0")
            {
                XmlSerializer serializer = new XmlSerializer(typeof(AssetAdministrationShellEnvironment), "http://www.admin-shell.io/aas/1/0");
                return serializer.Deserialize(s) as AssetAdministrationShellEnvironment;
            }

            // read V2.0
            if (nsuri != null && nsuri.Trim() == "http://www.admin-shell.io/aas/2/0")
            {
                XmlSerializer serializer = new XmlSerializer(typeof(AssetAdministrationShellEnvironment), "http://www.admin-shell.io/aas/2/0");
                return serializer.Deserialize(s) as AssetAdministrationShellEnvironment;
            }

            // read V3.0
            if (nsuri != null && nsuri.Trim() == "http://www.admin-shell.io/aas/3/0")
            {
                XmlSerializer serializer = new XmlSerializer(typeof(AssetAdministrationShellEnvironment), "http://www.admin-shell.io/aas/3/0");
                return serializer.Deserialize(s) as AssetAdministrationShellEnvironment;
            }

            return null;
        }

    }

}
