
namespace AdminShell
{
    using AdminShell_V30;
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
        /// De-serialize an open stream into AdministrationShellEnv. Does version/ compatibility management.
        /// </summary>
        /// <param name="s">Open for read stream</param>
        /// <returns></returns>
        public static AdminShell.AdministrationShellEnv DeserializeXmlFromStreamWithCompat(Stream s)
        {
            // not sure
            AdminShell.AdministrationShellEnv res = null;

            // try get first element
            var nsuri = TryReadXmlFirstElementNamespaceURI(s);

            // read V1.0?
            if (nsuri != null && nsuri.Trim() == "http://www.admin-shell.io/aas/1/0")
            {
                XmlSerializer serializer = new XmlSerializer(typeof(AasxCompatibilityModels.AdminShellV10.AdministrationShellEnv), "http://www.admin-shell.io/aas/1/0");
                var v10 = serializer.Deserialize(s) as AasxCompatibilityModels.AdminShellV10.AdministrationShellEnv;
                res = new AdminShellV30.AdministrationShellEnv(v10);
                return res;
            }

            // read V2.0?
            if (nsuri != null && nsuri.Trim() == "http://www.admin-shell.io/aas/2/0")
            {
                XmlSerializer serializer = new XmlSerializer(typeof(AdminShell.AdministrationShellEnv), "http://www.admin-shell.io/aas/2/0");
                res = serializer.Deserialize(s) as AdminShell.AdministrationShellEnv;
                return res;
            }

            // nope!
            return null;
        }

    }

}
