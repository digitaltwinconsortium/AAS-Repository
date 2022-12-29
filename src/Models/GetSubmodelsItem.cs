using AdminShell_V30;
using AdminShellNS;

namespace AasxServerBlazor.Models
{
    public class GetSubmodelsItem
    {
        public AdminShell.Identifier id = new AdminShell.Identifier();
        public string idShort = "";
        public string kind = "";

        public GetSubmodelsItem() { }

        public GetSubmodelsItem(AdminShell.Identifier id, string idShort, string kind)
        {
            this.id = id;
            this.idShort = idShort;
            this.kind = kind;
        }

        public GetSubmodelsItem(AdminShell.Identifiable idi, string kind)
        {
            this.id = idi.id;
            this.idShort = idi.idShort;
            this.kind = kind;
        }
    }
}
