
namespace AdminShell
{
    public enum TreeUpdateMode
    {
        ValuesOnly = 0,     // same tree, only values changed
        Rebuild,            // same tree, structure may change
        RebuildAndCollapse  // build new tree, keep open nodes
    }
}
