
namespace AdminShell
{
    using System.Collections.Generic;

    public class QualifierCollection : List<Qualifier>
    {
        public QualifierCollection()
        {

        }

        public new void Add(Qualifier q)
        {
            if (q == null)
                return;
            base.Add(q);
        }

        public Qualifier FindType(string type)
        {
            if (type == null)
                return null;
            foreach (var q in this)
                if (q != null && q.type != null && q.type.Trim() == type.Trim())
                    return q;
            return null;
        }

        public Qualifier FindSemanticId(SemanticId semId)
        {
            if (semId == null)
                return null;
            foreach (var q in this)
                if (q != null && q.semanticId != null && q.semanticId.Matches(semId))
                    return q;
            return null;
        }

        public string ToString(int format = 0, string delimiter = ";", string referencesDelimiter = ",")
        {
            var res = "";
            foreach (var q in this)
            {
                if (res != "")
                    res += delimiter;
                res += q.ToString(format, referencesDelimiter);
            }
            return res;
        }

        public override string ToString()
        {
            return this.ToString(0);
        }

        public static void AddQualifier(
            ref QualifierCollection qualifiers,
            string qualifierType = null, string qualifierValue = null, KeyList semanticKeys = null,
            GlobalReference qualifierValueId = null)
        {
            if (qualifiers == null)
                qualifiers = new QualifierCollection();
            var q = new Qualifier()
            {
                Type = qualifierType,
                Value = qualifierValue,
                ValueId = qualifierValueId,
            };
            if (semanticKeys != null)
                q.semanticId = SemanticId.CreateFromKeys(semanticKeys);
            qualifiers.Add(q);
        }

        public static Qualifier HasQualifierOfType(
            QualifierCollection qualifiers,
            string qualifierType)
        {
            if (qualifiers == null || qualifierType == null)
                return null;
            foreach (var q in qualifiers)
                if (q.type.Trim().ToLower() == qualifierType.Trim().ToLower())
                    return q;
            return null;
        }

        public IEnumerable<Qualifier> FindAllQualifierType(string qualifierType)
        {
            if (qualifierType == null)
                yield break;
            foreach (var q in this)
                if (q.Type.Trim().ToLower() == qualifierType.Trim().ToLower())
                    yield return q;
        }
    }
}
