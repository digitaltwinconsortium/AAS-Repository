using AdminShellNS;

namespace AasxServerBlazor.Models
{
    public class GetSubmodelsItem
    {
        public AdminShell.Identification id = new AdminShell.Identification();
        public string idShort = "";
        public string kind = "";

        public GetSubmodelsItem() { }

        public GetSubmodelsItem(AdminShell.Identification id, string idShort, string kind)
        {
            this.id = id;
            this.idShort = idShort;
            this.kind = kind;
        }

        public GetSubmodelsItem(AdminShell.Identifiable idi, string kind)
        {
            this.id = idi.identification;
            this.idShort = idi.idShort;
            this.kind = kind;
        }
    }
}
