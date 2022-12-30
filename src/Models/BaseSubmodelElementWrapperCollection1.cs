/*
Copyright (c) 2018-2021 Festo AG & Co. KG <https://www.festo.com/net/de_de/Forms/web/contact_international>
Author: Michael Hoffmeister

This source code is licensed under the Apache License 2.0 (see LICENSE.txt).

This source code may use other Open Source software components (see LICENSE.txt).
*/

using System;
using System.Collections.Generic;
using System.Linq;

namespace AasxCompatibilityModels
{
    #region AdminShell_V2_0

    public partial class AdminShellV20
    {
        /// <summary>
        /// Provides some more functionalities for searching specific elements, e.g. in a SMEC
        /// </summary>
        // OZ
        // Resharper disable UnusedTypeParameter
        public class BaseSubmodelElementWrapperCollection<ELEMT> : List<SubmodelElementWrapper>
            where ELEMT : SubmodelElement
        {
            // Resharper enable UnusedTypeParameter
            // no new members, as due to inheritance

            // constructors

            public BaseSubmodelElementWrapperCollection() : base() { }

            public BaseSubmodelElementWrapperCollection(SubmodelElementWrapperCollection other)
                : base()
            {
                if (other == null)
                    return;

                foreach (var smw in other)
                    this.Add(new SubmodelElementWrapper(smw.submodelElement));
            }

            // better find functions

            public IEnumerable<T> FindDeep<T>(Predicate<T> match = null) where T : SubmodelElement
            {
                foreach (var smw in this)
                {
                    var current = smw.submodelElement;
                    if (current == null)
                        continue;

                    // call lambda for this element
                    if (current is T)
                        if (match == null || match.Invoke(current as T))
                            yield return current as T;

                    // dive into?
                    // TODO (MIHO, 2020-07-31): would be nice to use IEnumerateChildren for this ..
                    if (current is SubmodelElementCollection smc && smc.value != null)
                        foreach (var x in smc.value.FindDeep<T>(match))
                            yield return x;

                    if (current is AnnotatedRelationshipElement are && are.annotations != null)
                        foreach (var x in are.annotations.FindDeep<T>(match))
                            yield return x;

                    if (current is Entity ent && ent.statements != null)
                        foreach (var x in ent.statements.FindDeep<T>(match))
                            yield return x;

                    if (current is Operation op)
                        for (int i = 0; i < 2; i++)
                            if (Operation.GetWrappers(op[i]) != null)
                                foreach (var x in Operation.GetWrappers(op[i]).FindDeep<T>(match))
                                    yield return x;
                }
            }

            public IEnumerable<SubmodelElementWrapper> FindAllIdShort(string idShort)
            {
                foreach (var smw in this)
                    if (smw.submodelElement != null)
                        if (smw.submodelElement.idShort.Trim().ToLower() == idShort.Trim().ToLower())
                            yield return smw;
            }

            public IEnumerable<T> FindAllIdShortAs<T>(string idShort) where T : SubmodelElement
            {
                foreach (var smw in this)
                    if (smw.submodelElement != null && smw.submodelElement is T)
                        if (smw.submodelElement.idShort.Trim().ToLower() == idShort.Trim().ToLower())
                            yield return smw.submodelElement as T;
            }

            public SubmodelElementWrapper FindFirstIdShort(string idShort)
            {
                return FindAllIdShort(idShort)?.FirstOrDefault<SubmodelElementWrapper>();
            }

            public T FindFirstIdShortAs<T>(string idShort) where T : SubmodelElement
            {
                return FindAllIdShortAs<T>(idShort)?.FirstOrDefault<T>();
            }

            public IEnumerable<SubmodelElementWrapper> FindAllSemanticId(Key semId, Type[] allowedTypes = null)
            {
                foreach (var smw in this)
                    if (smw.submodelElement != null && smw.submodelElement.semanticId != null)
                    {
                        if (smw.submodelElement == null)
                            continue;

                        if (allowedTypes != null)
                        {
                            var smwt = smw.submodelElement.GetType();
                            if (!allowedTypes.Contains(smwt))
                                continue;
                        }

                        if (smw.submodelElement.semanticId.MatchesExactlyOneKey(semId))
                            yield return smw;
                    }
            }

            public IEnumerable<T> FindAllSemanticIdAs<T>(Key semId, Key.MatchMode matchMode = Key.MatchMode.Strict)
                where T : SubmodelElement
            {
                foreach (var smw in this)
                    if (smw.submodelElement != null && smw.submodelElement is T
                        && smw.submodelElement.semanticId != null)
                        if (smw.submodelElement.semanticId.MatchesExactlyOneKey(semId, matchMode))
                            yield return smw.submodelElement as T;
            }

            public IEnumerable<T> FindAllSemanticIdAs<T>(Reference semId,
                Key.MatchMode matchMode = Key.MatchMode.Strict)
                where T : SubmodelElement
            {
                foreach (var smw in this)
                    if (smw.submodelElement != null && smw.submodelElement is T
                        && smw.submodelElement.semanticId != null)
                        if (smw.submodelElement.semanticId.Matches(semId, matchMode))
                            yield return smw.submodelElement as T;
            }

            public SubmodelElementWrapper FindFirstSemanticId(Key semId, Type[] allowedTypes = null)
            {
                return FindAllSemanticId(semId, allowedTypes)?.FirstOrDefault<SubmodelElementWrapper>();
            }

            public T FindFirstSemanticIdAs<T>(Key semId, Key.MatchMode matchMode = Key.MatchMode.Strict)
                where T : SubmodelElement
            {
                return FindAllSemanticIdAs<T>(semId, matchMode)?.FirstOrDefault<T>();
            }

            public T FindFirstSemanticIdAs<T>(Reference semId, Key.MatchMode matchMode = Key.MatchMode.Strict)
                where T : SubmodelElement
            {
                return FindAllSemanticIdAs<T>(semId, matchMode)?.FirstOrDefault<T>();
            }

            /* TODO (MIHO, 2021-10-18): there are overlaps of this new function with
             * this old function: FindFirstAnySemanticId(Key[] semId ..
             * clarify/ refactor */
            public IEnumerable<T> FindAllSemanticId<T>(
                Key[] allowedSemId, Key.MatchMode matchMode = Key.MatchMode.Strict,
                bool invertAllowed = false)
                where T : SubmodelElement
            {
                if (allowedSemId == null || allowedSemId.Length < 1)
                    yield break;

                foreach (var smw in this)
                {
                    if (smw.submodelElement == null || !(smw.submodelElement is T))
                        continue;

                    if (smw.submodelElement.semanticId == null || smw.submodelElement.semanticId.Count < 1)
                    {
                        if (invertAllowed)
                            yield return smw.submodelElement as T;
                        continue;
                    }

                    var found = false;
                    foreach (var semId in allowedSemId)
                        if (smw.submodelElement.semanticId.MatchesExactlyOneKey(semId, matchMode))
                        {
                            found = true;
                            break;
                        }

                    if (invertAllowed)
                        found = !found;

                    if (found)
                        yield return smw.submodelElement as T;
                }
            }

            public T FindFirstAnySemanticId<T>(
                Key[] allowedSemId, Key.MatchMode matchMode = Key.MatchMode.Strict,
                bool invertAllowed = false)
                where T : SubmodelElement
            {
                return FindAllSemanticId<T>(allowedSemId, matchMode, invertAllowed)?.FirstOrDefault<T>();
            }

            // recursion

            public void RecurseOnSubmodelElements(
                object state, List<SubmodelElement> parents,
                Action<object, List<SubmodelElement>, SubmodelElement> lambda)
            {
                // trivial
                if (lambda == null)
                    return;
                if (parents == null)
                    parents = new List<SubmodelElement>();

                // over all elements
                foreach (var smw in this)
                {
                    var current = smw.submodelElement;
                    if (current == null)
                        continue;

                    // call lambda for this element
                    lambda(state, parents, current);

                    // add to parents
                    parents.Add(current);

                    // dive into?
                    if (current is SubmodelElementCollection smc)
                        smc.value?.RecurseOnSubmodelElements(state, parents, lambda);

                    if (current is Entity ent)
                        ent.statements?.RecurseOnSubmodelElements(state, parents, lambda);

                    if (current is Operation op)
                        for (int i = 0; i < 2; i++)
                            Operation.GetWrappers(op[i])?.RecurseOnSubmodelElements(state, parents, lambda);

                    if (current is AnnotatedRelationshipElement arel)
                        arel.annotations?.RecurseOnSubmodelElements(state, parents, lambda);

                    // remove from parents
                    parents.RemoveAt(parents.Count - 1);
                }
            }

            // idShort management

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
                this.Add(SubmodelElementWrapper.CreateFor(sme));
            }

            /// <summary>
            /// Finds the first (shall be only 1!) SubmodelElementWrapper with SubmodelElement <c>sme</c>.
            /// </summary>
            public SubmodelElementWrapper FindSubModelElement(SubmodelElement sme)
            {
                if (sme != null)
                    foreach (var smw in this)
                        if (smw?.submodelElement == sme)
                            return smw;
                return null;
            }

            /// <summary>
            /// Removes the first (shall be only 1!) SubmodelElementWrapper with SubmodelElement <c>sme</c>.
            /// </summary>
            public void Remove(SubmodelElement sme)
            {
                if (sme == null)
                    return;
                var found = FindSubModelElement(sme);
                if (found != null)
                    this.Remove(found);
            }

            // a little more business logic

            public T CreateSMEForCD<T>(ConceptDescription cd, string category = null, string idShort = null,
                string idxTemplate = null, int maxNum = 999, bool addSme = false) where T : SubmodelElement, new()
            {
                // access
                if (cd == null)
                    return null;

                // try to potentially figure out idShort
                var ids = cd.idShort;
                if (ids == null && cd.GetIEC61360() != null)
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

            public T CreateSMEForIdShort<T>(string idShort, string category = null,
                string idxTemplate = null, int maxNum = 999, bool addSme = false) where T : SubmodelElement, new()
            {
                // access
                if (idShort == null)
                    return null;

                // try to potentially figure out idShort
                var ids = idShort;

                // unique?
                if (idxTemplate != null)
                    ids = this.IterateIdShortTemplateToBeUnique(idxTemplate, maxNum);

                // make a new instance
                var sme = new T() { idShort = ids };
                if (category != null)
                    sme.category = category;

                // instantanously add it?
                if (addSme)
                    this.Add(sme);

                // give back
                return sme;
            }

            // for conversion

            public T CopyOneSMEbyCopy<T>(Key destSemanticId,
                SubmodelElementWrapperCollection sourceSmc, Key sourceSemanticId,
                ConceptDescription createDefault = null, Action<T> setDefault = null,
                Key.MatchMode matchMode = Key.MatchMode.Relaxed, bool addSme = false) where T : SubmodelElement, new()
            {
                // get source
                var src = sourceSmc?.FindFirstSemanticIdAs<T>(sourceSemanticId, matchMode);
                var aeSrc = SubmodelElementWrapper.GetAdequateEnum(src?.GetElementName());
                if (src == null || aeSrc == SubmodelElementWrapper.AdequateElementEnum.Unknown)
                {
                    // create a default?
                    if (createDefault == null)
                        return null;

                    // ok, default
                    var dflt = this.CreateSMEForCD<T>(createDefault, addSme: addSme);

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
                Key.MatchMode matchMode = Key.MatchMode.Relaxed, bool addSme = false) where T : SubmodelElement, new()
            {
                return this.CopyOneSMEbyCopy<T>(destCD?.GetSingleKey(), sourceSmc, sourceCD?.GetSingleKey(),
                    createDefault ? destCD : null, setDefault, matchMode, addSme);
            }

            public void CopyManySMEbyCopy<T>(Key destSemanticId,
                SubmodelElementWrapperCollection sourceSmc, Key sourceSemanticId,
                ConceptDescription createDefault = null, Action<T> setDefault = null,
                Key.MatchMode matchMode = Key.MatchMode.Relaxed) where T : SubmodelElement, new()
            {
                // bool find possible sources
                bool foundSrc = false;
                if (sourceSmc == null)
                    return;
                foreach (var src in sourceSmc.FindAllSemanticIdAs<T>(sourceSemanticId, matchMode))
                {
                    // type of found src?
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
                CopyManySMEbyCopy(destCD.GetSingleKey(), sourceSmc, sourceCD.GetSingleKey(),
                    createDefault ? destCD : null, setDefault, matchMode);
            }
        }



    }

    #endregion
}

