
namespace AdminShell
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Provides some more functionalities for searching specific elements, e.g. in a SMEC
    /// </summary>
    public class BaseSubmodelElementWrapperCollection<Element> : List<SubmodelElementWrapper> where Element : SubmodelElement
    {
        public Referable Parent = null;

        /// <summary>
        /// Checks, if given <c>idShort</c> is already existing in the collection of SubmodelElements.
        /// Trims the string, but does not ignore upper/ lowercase. An empty <c>idShort</c> returns <c>false</c>.
        /// </summary>
        public bool CheckIdShortIsUnique(string idShort)
        {
            idShort = idShort?.Trim();
            if (idShort == null || idShort.Length < 1)
                return false;

            var res = true;
            foreach (var smw in this)
                if (smw.submodelElement != null && smw.submodelElement.idShort != null &&
                    smw.submodelElement.idShort == idShort)
                {
                    res = false;
                    break;
                }

            return res;
        }

        /// <summary>
        /// The string <c>idShortTemplate</c> shall contain <c>Format.String</c> partt such as <c>{0}</c>.
        /// A <c>int</c>-Parameter is as long incremented, until the resulting <c>idShort</c> proves
        /// to be unique in the collection of SubmodelElements or <c>maxNum</c> is reached.
        /// Returns <c>null</c> in case of any error.
        /// </summary>
        public string IterateIdShortTemplateToBeUnique(string idShortTemplate, int maxNum)
        {
            if (idShortTemplate == null || maxNum < 1 || !idShortTemplate.Contains("{0"))
                return null;

            int i = 1;
            while (i < maxNum)
            {
                var ids = String.Format(idShortTemplate, i);
                if (this.CheckIdShortIsUnique(ids))
                    return ids;
                i++;
            }

            return null;
        }

        // give more direct access to SMEs

        /// <summary>
        /// Add <c>sme</c> by creating a SubmodelElementWrapper for it and adding to this collection.
        /// </summary>
        public void Add(SubmodelElement sme)
        {
            if (sme == null)
                return;
            sme.parent = this.Parent;
            this.Add(SubmodelElementWrapper.CreateFor(sme));
        }

        /// <summary>
        /// Add <c>sme</c> by creating a SubmodelElementWrapper for it and adding to this collection.
        /// </summary>
        public void Insert(int index, SubmodelElement sme)
        {
            if (sme == null || index < 0 || index >= this.Count)
                return;
            sme.parent = this.Parent;
            this.Insert(index, SubmodelElementWrapper.CreateFor(sme));
        }

        public T CreateSMEForCD<T>(ConceptDescription cd, string category = null, string idShort = null,
            string idxTemplate = null, int maxNum = 999, bool addSme = false) where T : SubmodelElement, new()
        {
            // access
            if (cd == null)
                return null;

            // try to potentially figure out idShort
            var ids = cd.idShort;
            if ((ids == null || ids.Trim() == "") && cd.GetIEC61360() != null)
                ids = cd.GetIEC61360().shortName?
                    .GetDefaultStr();
            if (idShort != null)
                ids = idShort;
            if (ids == null)
                return null;

            // unique?
            if (idxTemplate != null)
                ids = this.IterateIdShortTemplateToBeUnique(idxTemplate, maxNum);

            // make a new instance
            var sme = new T()
            {
                idShort = ids,
                semanticId = new SemanticId(cd.GetCdReference())
            };
            if (category != null)
                sme.category = category;

            // if its a SMC, make sure its accessible
            if (sme is SubmodelElementCollection smc)
                smc.value = new SubmodelElementWrapperCollection();

            // instantanously add it?
            if (addSme)
                this.Add(sme);

            // give back
            return sme;
        }

        public T AdaptiveConvertTo<T>(
            SubmodelElement anySrc,
            ConceptDescription createDefault = null,
            string idShort = null, bool addSme = false) where T : SubmodelElement, new()
        {
            if (typeof(T) == typeof(MultiLanguageProperty)
                    && anySrc is Property srcProp)
            {
                var res = this.CreateSMEForCD<T>(createDefault, idShort: idShort, addSme: addSme);
                if (res is MultiLanguageProperty mlp)
                {
                    mlp.Value = new LangStringSet("EN?", srcProp.value);
                    mlp.ValueId = srcProp.valueId;
                    return res;
                }
            }

            if (typeof(T) == typeof(Property)
                    && anySrc is MultiLanguageProperty srcMlp)
            {
                var res = this.CreateSMEForCD<T>(createDefault, idShort: idShort, addSme: addSme);
                if (res is Property prp)
                {
                    prp.value = "" + srcMlp.Value?.GetDefaultStr();
                    prp.valueId = srcMlp.ValueId;
                    return res;
                }
            }

            return null;
        }

        public T CopyOneSMEbyCopy<T>(Identifier destSemanticId,
            SubmodelElementWrapperCollection sourceSmc, Identifier[] sourceSemanticId,
            ConceptDescription createDefault = null, Action<T> setDefault = null,
            Key.MatchMode matchMode = Key.MatchMode.Relaxed,
            string idShort = null, bool addSme = false) where T : SubmodelElement, new()
        {
            // get source
            var src = sourceSmc?.FindFirstAnySemanticIdAs<T>(sourceSemanticId, matchMode);

            // may be make an adaptive conversion
            if (src == null)
            {
                var anySrc = sourceSmc?.FindFirstAnySemanticId(sourceSemanticId, matchMode: matchMode);
                src = AdaptiveConvertTo<T>(anySrc?.submodelElement, createDefault,
                            idShort: idShort, addSme: false);
            }

            // proceed
            var aeSrc = SubmodelElementWrapper.GetAdequateEnum(src?.GetElementName());
            if (src == null || aeSrc == SubmodelElementWrapper.AdequateElementEnum.Unknown)
            {
                // create a default?
                if (createDefault == null)
                    return null;

                // ok, default
                var dflt = this.CreateSMEForCD<T>(createDefault, idShort: idShort, addSme: addSme);

                // set default?
                setDefault?.Invoke(dflt);

                // return
                return dflt;
            }

            // ok, create new one
            var dst = SubmodelElementWrapper.CreateAdequateType(aeSrc, src) as T;
            if (dst == null)
                return null;

            // make same things sure
            dst.idShort = src.idShort;
            dst.category = src.category;
            dst.semanticId = new SemanticId(destSemanticId);

            // instantanously add it?
            if (addSme)
                this.Add(dst);

            // give back
            return dst;
        }

        public T CopyOneSMEbyCopy<T>(ConceptDescription destCD,
            SubmodelElementWrapperCollection sourceSmc, ConceptDescription sourceCD,
            bool createDefault = false, Action<T> setDefault = null,
            Key.MatchMode matchMode = Key.MatchMode.Relaxed,
            string idShort = null, bool addSme = false) where T : SubmodelElement, new()
        {
            return this.CopyOneSMEbyCopy<T>(destCD?.GetSingleId(), sourceSmc, new[] { sourceCD?.GetSingleId() },
                createDefault ? destCD : null, setDefault, matchMode, idShort, addSme);
        }

        public T CopyOneSMEbyCopy<T>(ConceptDescription destCD,
            SubmodelElementWrapperCollection sourceSmc, Identifier[] sourceIds,
            bool createDefault = false, Action<T> setDefault = null,
            Key.MatchMode matchMode = Key.MatchMode.Relaxed,
            string idShort = null, bool addSme = false) where T : SubmodelElement, new()
        {
            return this.CopyOneSMEbyCopy<T>(destCD?.GetSingleId(), sourceSmc, sourceIds,
                createDefault ? destCD : null, setDefault, matchMode, idShort, addSme);
        }

        public void CopyManySMEbyCopy<T>(Identifier destSemanticId,
            SubmodelElementWrapperCollection sourceSmc, Identifier sourceSemanticId,
            ConceptDescription createDefault = null, Action<T> setDefault = null,
            Key.MatchMode matchMode = Key.MatchMode.Relaxed) where T : SubmodelElement, new()
        {
            // bool find possible sources
            bool foundSrc = false;
            if (sourceSmc == null)
                return;
            foreach (var src in sourceSmc.FindAllSemanticIdAs<T>(sourceSemanticId, matchMode))
            {
                // Type of found src?
                var aeSrc = SubmodelElementWrapper.GetAdequateEnum(src?.GetElementName());

                // ok?
                if (src == null || aeSrc == SubmodelElementWrapper.AdequateElementEnum.Unknown)
                    continue;
                foundSrc = true;

                // ok, create new one
                var dst = SubmodelElementWrapper.CreateAdequateType(aeSrc, src) as T;
                if (dst != null)
                {
                    // make same things sure
                    dst.idShort = src.idShort;
                    dst.category = src.category;
                    dst.semanticId = new SemanticId(destSemanticId);

                    // instantanously add it?
                    this.Add(dst);
                }
            }

            // default?
            if (createDefault != null && !foundSrc)
            {
                // ok, default
                var dflt = this.CreateSMEForCD<T>(createDefault, addSme: true);

                // set default?
                setDefault?.Invoke(dflt);
            }
        }

        public void CopyManySMEbyCopy<T>(ConceptDescription destCD,
            SubmodelElementWrapperCollection sourceSmc, ConceptDescription sourceCD,
            bool createDefault = false, Action<T> setDefault = null,
            Key.MatchMode matchMode = Key.MatchMode.Relaxed) where T : SubmodelElement, new()
        {
            CopyManySMEbyCopy(destCD.GetSingleId(), sourceSmc, sourceCD.GetSingleId(),
                createDefault ? destCD : null, setDefault, matchMode);
        }
    }
}
