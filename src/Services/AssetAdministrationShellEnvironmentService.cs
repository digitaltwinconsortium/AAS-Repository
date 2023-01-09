
namespace AdminShell
{
    using Microsoft.Extensions.Logging;
    using Opc.Ua;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class AssetAdministrationShellEnvironmentService
    {
        private readonly ILogger _logger;
        private readonly AASXPackageService _packageService;

        public AssetAdministrationShellEnvironmentService(ILoggerFactory logger, AASXPackageService packageService)
        {
            _logger = logger.CreateLogger("AssetAdministrationShellEnvironmentService");
            _packageService = packageService;
        }

        public void UpdateFileByPath(string aasIdentifier, string submodelIdentifier, string idShortPath, string fileName, string contentType, Stream fileContent)
        {
            var aas = GetAssetAdministrationShellById(aasIdentifier, out _);
            if (aas != null)
            {
                if (IsSubmodelPresentInAAS(aas, submodelIdentifier))
                {
                    UpdateFileByPathSubmodelRepo(submodelIdentifier, idShortPath, fileName, contentType, fileContent);
                }
            }
        }

        public void UpdateSubmodelElementByPath(SubmodelElement body, string aasIdentifier, string submodelIdentifier, string idShortPath)
        {
            if (string.IsNullOrEmpty(body.IdShort))
            {
                throw new Exception("SubmodelElement");
            }

            var aas = GetAssetAdministrationShellById(aasIdentifier, out _);
            if (aas != null)
            {
                if (IsSubmodelPresentInAAS(aas, submodelIdentifier))
                {
                    UpdateSubmodelElementByPathSubmodelRepo(body, submodelIdentifier, idShortPath);
                }
            }
        }

        public void UpdateSubmodel(Submodel body, string aasIdentifier, string submodelIdentifier)
        {
            if (string.IsNullOrEmpty(body.IdShort))
            {
                throw new Exception("Submodel");
            }

            var aas = GetAssetAdministrationShellById(aasIdentifier, out _);
            if (aas != null)
            {
                if (IsSubmodelPresentInAAS(aas, submodelIdentifier))
                {
                    UpdateSubmodelById(body, submodelIdentifier);
                }
            }
        }

        private bool IsSubmodelPresentInAAS(AssetAdministrationShell aas, string submodelIdentifier)
        {
            if (aas.Submodels.Any(s => s.ToString() == submodelIdentifier))
            {
                return true;
            }
            else
            {
                throw new Exception($"SubmodelReference with Id {submodelIdentifier} not found in AAS with Id {aas.Identification}");
            }
        }

        public void UpdateAssetInformation(AssetInformation body, string aasIdentifier)
        {
            var aas = GetAssetAdministrationShellById(aasIdentifier, out _);
            if (aas != null)
            {
                aas.AssetInformation = body;
                VisualTreeBuilderService.SignalNewData(TreeUpdateMode.Rebuild);
            }
        }

        public void UpdateAssetAdministrationShellById(AssetAdministrationShell body, string aasIdentifier)
        {
            if (string.IsNullOrEmpty(body.Identification))
            {
                throw new Exception("Provided Asset Administration Shell is missing its ID!");
            }

            if (_packageService.Packages.ContainsKey(aasIdentifier))
            {
                _packageService.Packages[aasIdentifier].AssetAdministrationShells.Remove(body);
                _packageService.Packages[aasIdentifier].AssetAdministrationShells.Add(body);

                VisualTreeBuilderService.SignalNewData(TreeUpdateMode.Rebuild);
            }
            else
            {
                throw new Exception($"Asset Admin Shell with ID {aasIdentifier} not found!");
            }
        }

        public Reference CreateSubmodelReference(Reference body, string aasIdentifier)
        {
            var aas = GetAssetAdministrationShellById(aasIdentifier, out _);

            if (aas != null)
            {
                var found = aas.Submodels.Any(s => s.Matches(body));
                if (found)
                {
                    throw new Exception($"Requested submodel reference already exists in AAS with Id {aasIdentifier}");
                }
                else
                {
                    aas.Submodels.Add((SubmodelReference)body);
                    return body;
                }
            }

            return null;
        }

        public SubmodelElement CreateSubmodelElementByPath(SubmodelElement body, string aasIdentifier, string submodelIdentifier, string idShortPath)
        {
            if (string.IsNullOrEmpty(body.IdShort))
            {
                throw new Exception("SubmodelElement");
            }

            var aas = GetAssetAdministrationShellById(aasIdentifier, out _);
            if (aas != null)
            {
                if (IsSubmodelPresentInAAS(aas, submodelIdentifier))
                {
                    return CreateSubmodelElementByPathSubmodelRepo(body, submodelIdentifier, idShortPath);
                }
            }

            return null;
        }

        public SubmodelElement CreateSubmodelElement(SubmodelElement body, string aasIdentifier, string submodelIdentifier)
        {
            if (string.IsNullOrEmpty(body.IdShort))
            {
                throw new Exception("SubmodelElement");
            }

            var aas = GetAssetAdministrationShellById(aasIdentifier, out _);
            if (aas != null)
            {
                if (IsSubmodelPresentInAAS(aas, submodelIdentifier))
                {
                    return CreateSubmodelElementSubmodelRepo(body, submodelIdentifier);
                }
            }

            return null;
        }

        public AssetAdministrationShell CreateAssetAdministrationShell(AssetAdministrationShell body)
        {
            if (string.IsNullOrEmpty(body.Identification))
            {
                throw new Exception("AssetAdministrationShell");
            }

            //Check if AAS exists
            var found = IsAssetAdministrationShellPresent(body.Identification, out _, out _);
            if (found)
            {
                throw new Exception($"AssetAdministrationShell with Id {body.Identification} already exists.");
            }

            AssetAdministrationShellEnvironment env = new();
            env.AssetAdministrationShells.Add(body);
            _packageService.SaveAs(body.Identification, env);

            VisualTreeBuilderService.SignalNewData(TreeUpdateMode.RebuildAndCollapse);

            return body;
        }



        public OperationResult GetOperationAsyncResult(string aasIdentifier, string submodelIdentifier, string idShortPath, string handleId)
        {
            var aas = GetAssetAdministrationShellById(aasIdentifier, out _);
            if (aas != null)
            {
                if (IsSubmodelPresentInAAS(aas, submodelIdentifier))
                {
                    return GetOperationAsyncResultSubmodelRepo(submodelIdentifier, idShortPath, handleId);
                }
            }

            return null;
        }

        public string GetFileByPath(string aasIdentifier, string submodelIdentifier, string idShortPath, out byte[] content, out long fileSize)
        {
            content = null;
            fileSize = 0;
            var aas = GetAssetAdministrationShellById(aasIdentifier, out _);
            if (aas != null)
            {
                if (IsSubmodelPresentInAAS(aas, submodelIdentifier))
                {
                    return GetFileByPathSubmodelRepo(submodelIdentifier, idShortPath, out content, out fileSize);
                }
            }

            return null;
        }

        public void DeleteSubmodelElementByPath(string aasIdentifier, string submodelIdentifier, string idShortPath)
        {
            var aas = GetAssetAdministrationShellById(aasIdentifier, out _);
            if (aas != null)
            {
                if (IsSubmodelPresentInAAS(aas, submodelIdentifier))
                {
                    DeleteSubmodelElementByPathSubmodelRepo(submodelIdentifier, idShortPath);
                }
            }
        }

        public SubmodelElement GetSubmodelElementByPath(string aasIdentifier, string submodelIdentifier, string idShortPath)
        {
            var aas = GetAssetAdministrationShellById(aasIdentifier, out _);
            if (aas != null)
            {
                if (IsSubmodelPresentInAAS(aas, submodelIdentifier))
                {
                    var output = GetSubmodelElementByPathSubmodelRepo(submodelIdentifier, idShortPath, out _);
                    return output;
                }
            }

            return null;
        }

        public Submodel GetSubmodel(string aasIdentifier, string submodelIdentifier)
        {
            var aas = GetAssetAdministrationShellById(aasIdentifier, out _);
            if (aas != null)
            {
                var submodelRefs = aas.Submodels.Where(s => s.Matches(submodelIdentifier));
                if (submodelRefs.Any())
                {
                    return GetSubmodelById(submodelIdentifier, out _);
                }
                else
                {
                    throw new Exception($"SubmodelReference with Id {submodelIdentifier} not found in AAS with Id {aasIdentifier}");
                }
            }

            return null;
        }

        public void DeleteSubmodelReferenceById(string aasIdentifier, string submodelIdentifier)
        {
            var aas = GetAssetAdministrationShellById(aasIdentifier, out _);
            if (aas != null)
            {
                var submodelRefs = aas.Submodels.Where(s => s.Matches(submodelIdentifier));
                if (submodelRefs.Any())
                {
                    aas.Submodels.Remove(submodelRefs.First());
                    _packageService.Save(aasIdentifier);

                    VisualTreeBuilderService.SignalNewData(TreeUpdateMode.Rebuild);
                }
                else
                {
                    throw new Exception($"SubmodelReference with Id {submodelIdentifier} not found in AAS with Id {aasIdentifier}");
                }
            }
        }

        public void DeleteAssetAdministrationShellById(string aasIdentifier)
        {
            var aas = GetAssetAdministrationShellById(aasIdentifier, out string key);
            if ((aas != null) && !string.IsNullOrEmpty(key))
            {
                _packageService.Packages[key].AssetAdministrationShells.Remove(aas);
                if (_packageService.Packages[key].AssetAdministrationShells.Count == 0)
                {
                    _packageService.Delete(key); // TODO: what about Submodels?
                }
                else
                {
                    _packageService.Save(key);
                }

                VisualTreeBuilderService.SignalNewData(TreeUpdateMode.RebuildAndCollapse);
            }
            else
            {
                throw new Exception("Unexpected error occurred.");
            }
        }

        public AssetInformation GetAssetInformationFromAas(string aasIdentifier)
        {
            var aas = GetAssetAdministrationShellById(aasIdentifier, out _);
            if (aas != null)
            {
                return aas.AssetInformation;
            }

            return null;
        }

        public List<Reference> GetAllSubmodelReferences(string decodedAasId)
        {
            var aas = GetAssetAdministrationShellById(decodedAasId, out _);

            if (aas != null)
            {
                List<Reference> references = new();
                foreach(SubmodelReference smr in aas.Submodels)
                {
                    references.Add(smr);
                }

                return references;
            }

            return null;
        }

        public List<AssetAdministrationShell> GetAllAssetAdministrationShells(List<string> assetIds = null, string idShort = null)
        {
            var output = new List<AssetAdministrationShell>();

            //Get All AASs
            foreach (var env in _packageService.Packages)
            {
                output.AddRange(env.Value.AssetAdministrationShells);
            }

            if (output.Any())
            {
                //Filter AASs based on IdShort
                if (!string.IsNullOrEmpty(idShort))
                {
                    output = output.Where(a => a.IdShort.Equals(idShort)).ToList();
                    if ((output == null) || output?.Count == 0)
                    {
                        throw new Exception($"AssetAdministrationShells with IdShort {idShort} Not Found.");
                    }
                }

                //Filter based on AssetId
                if (assetIds != null && assetIds.Count != 0)
                {
                    var aasList = new List<AssetAdministrationShell>();
                    foreach (var assetId in assetIds)
                    {
                        aasList.AddRange(output.Where(a => a.AssetInformation.SpecificAssetIds.Contains(new IdentifierKeyValuePair() { Key = assetId })).ToList());
                    }

                    if (aasList.Any())
                    {
                        return aasList;
                    }
                    else
                    {
                        throw new Exception($"AssetAdministrationShells with requested SpecificAssetIds Not Found.");
                    }
                }
            }

            return output;
        }

        public object GetAllSubmodelElements(string aasIdentifier, string submodelIdentifier)
        {
            object output = null;
            //Find AAS
            var aas = GetAssetAdministrationShellById(aasIdentifier, out _);
            if (aas != null)
            {
                //Check if AAS consist the requested submodel
                IEnumerable<Reference> references = aas.Submodels.Where(s => s.Matches(submodelIdentifier));
                if ((references == null) || (references?.Count() == 0))
                {
                    throw new Exception($"Requested submodel: {submodelIdentifier} not found in AAS: {aasIdentifier}");
                }

                output = GetAllSubmodelElementsFromSubmodel(submodelIdentifier);
            }

            return output;
        }

        public AssetAdministrationShell GetAssetAdministrationShellById(string aasIdentifier, out string key)
        {
            bool found = IsAssetAdministrationShellPresent(aasIdentifier, out AssetAdministrationShell output, out key);

            if (found)
            {
                return output;
            }
            else
            {
                throw new Exception($"AssetAdministrationShell with Id {aasIdentifier} not found.");
            }
        }

        private bool IsAssetAdministrationShellPresent(string aasIdentifier, out AssetAdministrationShell output, out string key)
        {
            foreach (KeyValuePair<string, AssetAdministrationShellEnvironment> package in _packageService.Packages)
            {
                var aas = package.Value.AssetAdministrationShells.Where(a => a.Id.Equals(aasIdentifier));
                if (aas.Any())
                {
                    output = aas.First();
                    key = package.Key;
                    return true;
                }
            }

            output = null;
            key = null;
            return false;
        }

        public void UpdateConceptDescriptionById(ConceptDescription body, string cdIdentifier)
        {
            if (string.IsNullOrEmpty(body.Id))
            {
                throw new Exception("ConceptDescription");
            }

            var conceptDescription = GetConceptDescriptionById(cdIdentifier, out string key);
            if (conceptDescription != null)
            {
                int cdIndex = _packageService.Packages[key].ConceptDescriptions.IndexOf(conceptDescription);
                _packageService.Packages[key].ConceptDescriptions.Remove(conceptDescription);
                _packageService.Packages[key].ConceptDescriptions.Insert(cdIndex, body);
                _packageService.Save(key);

                VisualTreeBuilderService.SignalNewData(TreeUpdateMode.ValuesOnly);
            }
        }

        public ConceptDescription CreateConceptDescription(ConceptDescription body)
        {
            if (string.IsNullOrEmpty(body.Id))
            {
                throw new Exception("ConceptDescription");
            }

            //Check if AAS exists
            var found = IsConceptDescriptionPresent(body.Id, out _, out _);
            if (found)
            {
                throw new Exception($"ConceptDescription with Id {body.Id} already exists.");
            }

            AssetAdministrationShellEnvironment env = new();
            env.ConceptDescriptions.Add(body);
            _packageService.SaveAs(body.Id, env);

            VisualTreeBuilderService.SignalNewData(TreeUpdateMode.Rebuild);

            return body;
        }

        public void DeleteConceptDescriptionById(string cdIdentifier)
        {
            var conceptDescription = GetConceptDescriptionById(cdIdentifier, out string key);
            if ((conceptDescription != null) && !string.IsNullOrEmpty(key))
            {
                _packageService.Packages[key].ConceptDescriptions.Remove(conceptDescription);
                _packageService.Save(key);

                VisualTreeBuilderService.SignalNewData(TreeUpdateMode.Rebuild);
            }
            else
            {
                throw new Exception("Unexpected error occurred.");
            }
        }

        public ConceptDescription GetConceptDescriptionById(string cdIdentifier, out string key)
        {
            bool found = IsConceptDescriptionPresent(cdIdentifier, out ConceptDescription output, out key);
            if (found)
            {
                return output;
            }
            else
            {
                throw new Exception($"ConceptDescription with Id {cdIdentifier} not found.");
            }
        }

        private bool IsConceptDescriptionPresent(string cdIdentifier, out ConceptDescription output, out string key)
        {
            foreach (KeyValuePair<string, AssetAdministrationShellEnvironment> package in _packageService.Packages)
            {
                var conceptDescriptions = package.Value.ConceptDescriptions.Where(c => c.Id.Equals(cdIdentifier));
                if (conceptDescriptions.Any())
                {
                    output = conceptDescriptions.First();
                    key = package.Key;
                    return true;
                }
            }

            output = null;
            key = null;
            return false;
        }

        public List<ConceptDescription> GetAllConceptDescriptions(string idShort = null, Reference reqIsCaseOf = null, Reference reqDataSpecificationRef = null)
        {
            var output = new List<ConceptDescription>();

            //Get All Concept descriptions
            foreach (KeyValuePair<string, AssetAdministrationShellEnvironment> package in _packageService.Packages)
            {
                    output.AddRange(package.Value.ConceptDescriptions);
            }

            if (output.Any())
            {
                //Filter AASs based on IdShort
                if (!string.IsNullOrEmpty(idShort))
                {
                    var cdList = output.Where(cd => cd.IdShort.Equals(idShort)).ToList();
                    if ((cdList == null) || cdList?.Count == 0)
                    {
                        throw new Exception($"Concept Description with IdShort {idShort} Not Found.");
                    }
                    else
                    {
                        output = cdList;
                    }
                }

                //Filter based on IsCaseOf
                if (reqIsCaseOf != null)
                {
                    var cdList = new List<ConceptDescription>();
                    foreach (var conceptDescription in output)
                    {
                        if (conceptDescription.IsCaseOf?.Count > 0)
                        {
                            foreach (var reference in conceptDescription.IsCaseOf)
                            {
                                if (reference != null && reference.Matches(reqIsCaseOf))
                                {
                                    cdList.Add(conceptDescription);
                                    break;
                                }
                            }
                        }
                    }
                    if ((cdList == null) || cdList?.Count == 0)
                    {
                        throw new Exception($"Concept Description with requested IsCaseOf Not Found.");
                    }
                    else
                    {
                        output = cdList;
                    }

                }

                //Filter based on DataSpecificationRef
                if (reqDataSpecificationRef != null)
                {
                    var cdList = new List<ConceptDescription>();
                    foreach (var conceptDescription in output)
                    {
                        if (conceptDescription.DataSpecifications?.Count > 0)
                        {
                            foreach (var reference in conceptDescription.DataSpecifications)
                            {
                                if (reference != null && reference.Matches(reqDataSpecificationRef))
                                {
                                    cdList.Add(conceptDescription);
                                    break;
                                }
                            }
                        }
                    }
                    if ((cdList == null) || cdList?.Count == 0)
                    {
                        throw new Exception($"Concept Description with requested DataSpecificationReference Not Found.");
                    }
                    else
                    {
                        output = cdList;
                    }
                }
            }

            return output;
        }

        public void UpdateSubmodelElementByPathSubmodelRepo(SubmodelElement body, string submodelIdentifier, string idShortPath = null)
        {
            if (string.IsNullOrEmpty(body.IdShort))
            {
                throw new Exception("SubmodelElement");
            }

            var submodelElement = GetSubmodelElementByPathSubmodelRepo(submodelIdentifier, idShortPath, out object smeParent);
            if (submodelElement != null && smeParent != null)
            {
                {
                    if (smeParent is SubmodelElementCollection collection)
                    {
                        var smeIndex = collection.Value.IndexOf(new SubmodelElementWrapper(submodelElement));
                        collection.Value.Remove(new SubmodelElementWrapper(submodelElement));
                        collection.Value.Insert(smeIndex, new SubmodelElementWrapper(body));
                    }
                    else if (smeParent is SubmodelElementList list)
                    {
                        var smeIndex = list.Value.IndexOf(new SubmodelElementWrapper(submodelElement));
                        list.Value.Remove(new SubmodelElementWrapper(submodelElement));
                        list.Value.Insert(smeIndex, new SubmodelElementWrapper(body));
                    }
                    //Added support for submodel here, as no other api found for this functionality
                    else if (smeParent is Submodel submodel)
                    {
                        var smeIndex = submodel.SubmodelElements.IndexOf(new SubmodelElementWrapper(submodelElement));
                        submodel.SubmodelElements.Remove(new SubmodelElementWrapper(submodelElement));
                        submodel.SubmodelElements.Insert(smeIndex, new SubmodelElementWrapper(body));
                    }
                }

                VisualTreeBuilderService.SignalNewData(TreeUpdateMode.Rebuild);
            }
        }

        public void UpdateSubmodelById(Submodel body, string submodelIdentifier = null)
        {
            if (string.IsNullOrEmpty(body.Identification))
            {
               throw new Exception("Submodel");
            }

            var submodel = GetSubmodelById(submodelIdentifier, out string key);
            if (submodel != null)
            {
                _packageService.Packages[key].Submodels.Remove(submodel);
                _packageService.Packages[key].Submodels.Add(body);
                _packageService.Save(key);

                VisualTreeBuilderService.SignalNewData(TreeUpdateMode.Rebuild);
            }
        }

        public SubmodelElement CreateSubmodelElementByPathSubmodelRepo(SubmodelElement body, string submodelIdentifier, string idShortPath)
        {
            if (string.IsNullOrEmpty(body.IdShort))
            {
                throw new Exception("SubmodelElement");
            }

            var newIdShortPath = idShortPath + "." + body.IdShort;
            var found = IsSubmodelElementPresent(submodelIdentifier, newIdShortPath, out _, out object smeParent);
            if (found)
            {
                throw new Exception($"SubmodelElement with IdShort {body.IdShort} already exists.");
            }
            else
            {
                if (smeParent != null && smeParent is Submodel submodel)
                {
                    submodel.SubmodelElements ??= new List<SubmodelElementWrapper>();

                    submodel.SubmodelElements.Add(new SubmodelElementWrapper(body));

                    body.Parent = submodel;
                }
                else if (smeParent != null && smeParent is SubmodelElementCollection collection)
                {
                    collection.Value ??= new List<SubmodelElementWrapper>();

                    collection.Value.Add(new SubmodelElementWrapper(body));

                    body.Parent = collection;
                }
                else if (smeParent != null && smeParent is SubmodelElementList list)
                {
                    list.Value ??= new List<SubmodelElementWrapper>();

                    list.Value.Add(new SubmodelElementWrapper(body));

                    body.Parent = list;
                }
                else if (smeParent != null && smeParent is Entity entity)
                {
                    entity.Statements = new List<SubmodelElementWrapper>();

                    entity.Statements.Add(new SubmodelElementWrapper(body));
                    body.Parent = entity;
                }
                else if (smeParent != null && smeParent is AnnotatedRelationshipElement annotatedRelationshipElement)
                {
                    annotatedRelationshipElement.Annotations ??= new List<DataElement>();

                    annotatedRelationshipElement.Annotations.Add((DataElement)body);
                    body.Parent = annotatedRelationshipElement;
                }

                VisualTreeBuilderService.SignalNewData(TreeUpdateMode.Rebuild);

                return body;
            }
        }

        public SubmodelElement CreateSubmodelElementSubmodelRepo(SubmodelElement body, string submodelIdentifier)
        {
            if (string.IsNullOrEmpty(body.IdShort))
            {
                throw new Exception("SubmodelElement");
            }

            var found = IsSubmodelElementPresent(submodelIdentifier, body.IdShort, out _, out object smeParent);
            if (found)
            {
                throw new Exception($"SubmodelElement with IdShort {body.IdShort} already exists.");
            }
            else
            {
                if (smeParent != null && smeParent is Submodel submodel)
                {
                    submodel.SubmodelElements ??= new List<SubmodelElementWrapper>();

                    submodel.SubmodelElements.Add(new SubmodelElementWrapper(body));

                    body.Parent = submodel;

                    VisualTreeBuilderService.SignalNewData(TreeUpdateMode.Rebuild);

                    return body;
                }
            }

            return null;

        }

        public Submodel CreateSubmodel(Submodel body)
        {
            if (string.IsNullOrEmpty(body.Id))
            {
                throw new Exception("Submodel");
            }

            //Check if AAS exists
            var found = IsSubmodelPresent(body.Id, out _, out _);
            if (found)
            {
                throw new Exception($"Submodel with Id {body.Id} already exists.");
            }

            AssetAdministrationShellEnvironment env = new();
            env.Submodels.Add(body);
            _packageService.Packages.Add(body.Id, env);

            VisualTreeBuilderService.SignalNewData(TreeUpdateMode.RebuildAndCollapse);

            return body;
        }

        public Submodel GetSubmodelById(string submodelIdentifier, out string key)
        {
            bool found = IsSubmodelPresent(submodelIdentifier, out Submodel output, out key);
            if (found)
            {
                return output;
            }
            else
            {
                throw new Exception($"Submodel with Id {submodelIdentifier} not found.");
            }
        }

        private bool IsSubmodelPresent(string submodelIdentifier, out Submodel output, out string key)
        {
            foreach (KeyValuePair<string, AssetAdministrationShellEnvironment> package in _packageService.Packages)
            {
                var submodels = package.Value.Submodels.Where(a => a.Id.Equals(submodelIdentifier));
                if (submodels.Any())
                {
                    output = submodels.First();
                    key = package.Key;
                    return true;
                }
            }

            output = null;
            key = null;
            return false;
        }

        public object GetAllSubmodelElementsFromSubmodel(string submodelIdentifier = null)
        {
            var submodel = GetSubmodelById(submodelIdentifier, out _);
            if (submodel == null)
                return null;

            return submodel.SubmodelElements;
        }

        public List<Submodel> GetAllSubmodels(Reference reqSemanticId = null, string idShort = null)
        {
            List<Submodel> output = new List<Submodel>();

            //Get All Submodels
            foreach (KeyValuePair<string, AssetAdministrationShellEnvironment> package in _packageService.Packages)
            {
                foreach (var s in package.Value.Submodels)
                {
                    output.Add(s);
                }
            }

            //Apply filters
            if (output.Any())
            {
                //Filter w.r.t idShort
                if (!string.IsNullOrEmpty(idShort))
                {
                    var submodels = output.Where(s => s.IdShort.Equals(idShort)).ToList();
                    if ((submodels == null) || (submodels?.Count == 0))
                    {
                        _logger.LogInformation($"Submodels with IdShort {idShort} Not Found.");
                    }

                    output = submodels;
                }

                //Filter w.r.t. SemanticId
                if (reqSemanticId != null)
                {
                    if (output.Any())
                    {
                        var submodels = output.Where(s => s.SemanticId.Matches(reqSemanticId)).ToList();
                        if ((submodels == null) || submodels?.Count == 0)
                        {
                            _logger.LogInformation($"Submodels with requested SemnaticId Not Found.");
                        }

                        output = submodels;
                    }

                }
            }

            return output;
        }

        public void DeleteSubmodelById(string submodelIdentifier)
        {
            var submodel = GetSubmodelById(submodelIdentifier, out string key);
            if ((submodel != null) && !string.IsNullOrEmpty(key))
            {
                _packageService.Packages[key].Submodels.Remove(submodel);

                //Delete submodel reference from AAS
                foreach (var aas in _packageService.Packages[key].AssetAdministrationShells)
                {
                    DeleteSubmodelReferenceById(aas.Id, submodelIdentifier);
                }

                _packageService.Save(key);

                VisualTreeBuilderService.SignalNewData(TreeUpdateMode.Rebuild);
            }
            else
            {
                throw new Exception("Unexpected error occurred.");
            }
        }

        public SubmodelElement GetSubmodelElementByPathSubmodelRepo(string submodelIdentifier, string idShortPath, out object smeParent)
        {
            bool found = IsSubmodelElementPresent(submodelIdentifier, idShortPath, out SubmodelElement output, out smeParent);

            if (found)
            {
                return output;
            }
            else
            {
                throw new Exception($"Requested submodel element {idShortPath} NOT found.");
            }
        }

        private bool IsSubmodelElementPresent(string submodelIdentifier, string idShortPath, out SubmodelElement output, out object smeParent)
        {
            output = null;
            smeParent = null;
            var submodel = GetSubmodelById(submodelIdentifier, out _);

            if (submodel != null)
            {
                output = GetSubmodelElementByPath(submodel, idShortPath, out object parent);
                smeParent = parent;
                if (output != null)
                {
                    return true;
                }

            }

            return false;
        }

        private SubmodelElement FindSubmodelElementByIdShort(Submodel sm, string idShort)
        {
                foreach (SubmodelElementWrapper smew in sm.SubmodelElements)
                    if (smew.SubmodelElement.SemanticId != null)
                        if (smew.SubmodelElement.SemanticId.Matches(new Identifier(idShort)))
                            return smew.SubmodelElement;

            return null;
        }

        private SubmodelElement FindSubmodelElementByIdShort(SubmodelElementCollection smec, string idShort)
        {
            foreach (SubmodelElementWrapper smew in smec.Value)
                if (smew.SubmodelElement.SemanticId != null)
                    if (smew.SubmodelElement.SemanticId.Matches(new Identifier(idShort)))
                        return smew.SubmodelElement;

            return null;
        }

        private SubmodelElement FindSubmodelElementByIdShort(SubmodelElement sme, string idShort)
        {
            if (sme.SemanticId != null)
                if (sme.SemanticId.Matches(new Identifier(idShort)))
                    return sme;

            return null;
        }

        private SubmodelElement GetSubmodelElementByPath(object parent, string idShortPath, out object outParent)
        {
            outParent = parent;
            if (idShortPath.Contains('.'))
            {
                string[] idShorts = idShortPath.Split('.', 2);
                if (parent is Submodel submodel)
                {
                    var submodelElement = FindSubmodelElementByIdShort(submodel, idShorts[0]);
                    if (submodelElement != null)
                    {
                        return GetSubmodelElementByPath(submodelElement, idShorts[1], out outParent);
                    }
                }
                else if (parent is SubmodelElementCollection collection)
                {
                    var submodelElement = FindSubmodelElementByIdShort(collection, idShorts[0]);
                    if (submodelElement != null)
                    {
                        return GetSubmodelElementByPath(submodelElement, idShorts[1], out outParent);
                    }
                }
                else if (parent is SubmodelElementList list)
                {
                    var submodelElement = FindSubmodelElementByIdShort(list, idShorts[0]);
                    if (submodelElement != null)
                    {
                        return GetSubmodelElementByPath(submodelElement, idShorts[1], out outParent);
                    }
                }
                else if (parent is Entity entity)
                {
                    var submodelElement = FindSubmodelElementByIdShort(entity, idShortPath);
                    if (submodelElement != null)
                    {
                        return GetSubmodelElementByPath(submodelElement, idShorts[1], out outParent);
                    }
                }
                else if (parent is AnnotatedRelationshipElement annotatedRelationshipElement)
                {
                    var submodelElement = FindSubmodelElementByIdShort(annotatedRelationshipElement, idShortPath);
                    if (submodelElement != null)
                    {
                        return GetSubmodelElementByPath(submodelElement, idShorts[1], out outParent);
                    }
                }
                else
                {
                    throw new Exception($"Parent of Type {parent.GetType()} not supported.");
                }
            }
            else
            {
                if (parent is Submodel submodel)
                {
                    var submodelElement = FindSubmodelElementByIdShort(submodel, idShortPath);
                    if (submodelElement != null)
                    {
                        return submodelElement;
                    }
                }
                else if (parent is SubmodelElementCollection collection)
                {
                    var submodelElement = FindSubmodelElementByIdShort(collection, idShortPath);
                    if (submodelElement != null)
                    {
                        return submodelElement;
                    }
                }
                else if (parent is SubmodelElementList list)
                {
                    var submodelElement = FindSubmodelElementByIdShort(list, idShortPath);
                    if (submodelElement != null)
                    {
                        return submodelElement;
                    }
                }
                else if (parent is Entity entity)
                {
                    var submodelElement = FindSubmodelElementByIdShort(entity, idShortPath);
                    if (submodelElement != null)
                    {
                        return submodelElement;
                    }
                }
                else if (parent is AnnotatedRelationshipElement annotatedRelationshipElement)
                {
                    var submodelElement = FindSubmodelElementByIdShort(annotatedRelationshipElement, idShortPath);
                    if (submodelElement != null)
                    {
                        return submodelElement;
                    }
                }
                else
                {
                    throw new Exception($"Parent of Type {parent.GetType()} not supported.");
                }
            }

            return null;
        }

        public void DeleteSubmodelElementByPathSubmodelRepo(string submodelIdentifier, string idShortPath)
        {
            var submodelElement = GetSubmodelElementByPathSubmodelRepo(submodelIdentifier, idShortPath, out object smeParent);
            if (submodelElement != null)
            {
                if (smeParent is SubmodelElementCollection parentCollection)
                {
                    parentCollection.Value.Remove(new SubmodelElementWrapper(submodelElement));
                }
                else if (smeParent is SubmodelElementList parentList)
                {
                    parentList.Value.Remove(new SubmodelElementWrapper(submodelElement));
                }
                else if (smeParent is AnnotatedRelationshipElement annotatedRelationshipElement)
                {
                    annotatedRelationshipElement.Annotations.Remove((DataElement)submodelElement);
                }
                else if (smeParent is Entity entity)
                {
                    entity.Statements.Remove(new SubmodelElementWrapper(submodelElement));
                }
                else if (smeParent is Submodel parentSubmodel)
                {
                    parentSubmodel.SubmodelElements.Remove(new SubmodelElementWrapper(submodelElement));
                }

                VisualTreeBuilderService.SignalNewData(TreeUpdateMode.Rebuild);
            }
        }

        public string GetFileByPathSubmodelRepo(string submodelIdentifier, string idShortPath, out byte[] byteArray, out long fileSize)
        {
            byteArray = null;
            string fileName = null;
            fileSize = 0;

            var submodel = GetSubmodelById(submodelIdentifier, out string key);

            var fileElement = GetSubmodelElementByPathSubmodelRepo(submodelIdentifier, idShortPath, out _);

            if (fileElement != null)
            {
                if (fileElement is File file)
                {
                    fileName = file.Value;
                    fileSize = _packageService.GetPackageStream(fileName).Length;
                }
                else
                {
                    throw new Exception($"Submodel element {fileElement.IdShort} is not of Type File.");
                }
            }

            return fileName;
        }

        public void UpdateFileByPathSubmodelRepo(string submodelIdentifier, string idShortPath, string fileName, string contentType, Stream fileContent)
        {
            _ = GetSubmodelById(submodelIdentifier, out string key);

            var fileElement = GetSubmodelElementByPathSubmodelRepo(submodelIdentifier, idShortPath, out _);
            if (fileElement != null)
            {
                // update
                if (fileElement is File file)
                {
                    var sourcePath = Path.GetDirectoryName(file.Value);
                    var targetFile = Path.Combine(sourcePath, fileName);
                    file.Value = FormatFilePath(targetFile);
                    _packageService.ReplaceSupplementaryFileInPackage(key, targetFile, contentType, fileContent);
                    _packageService.Save(key);

                    VisualTreeBuilderService.SignalNewData(TreeUpdateMode.RebuildAndCollapse);
                }
                else
                {
                    throw new Exception($"Submodel element {fileElement.IdShort} is not of Type File.");
                }
            }
            else
            {
                // add
                File newFile = new();
                var sourcePath = Path.GetDirectoryName(newFile.Value);
                var targetFile = Path.Combine(sourcePath, fileName);
                newFile.Value = FormatFilePath(targetFile);
                _packageService.AddSupplementaryFileToPackage(key, newFile.Value, contentType, fileContent);
                _packageService.Save(key);

                VisualTreeBuilderService.SignalNewData(TreeUpdateMode.RebuildAndCollapse);
            }
        }

        private string FormatFilePath(string filePath)
        {
            return Regex.Replace(filePath, @"\\", "/");
        }

        public OperationResult GetOperationAsyncResultSubmodelRepo(string decodedSubmodelId, string idShortPath, string handleId)
        {
            var operationElement = GetSubmodelElementByPathSubmodelRepo(decodedSubmodelId, idShortPath, out _);

            if (operationElement != null)
            {
                if (operationElement is Operation)
                {
                    return new OperationResult();
                }
                else
                {
                    throw new Exception($"Submodel element {operationElement.IdShort} is not of Type Operation.");
                }
            }

            return null;
        }

        public OperationResult InvokeOperationSubmodelRepo(string submodelIdentifier, string idShortPath, OperationRequest operationRequest)
        {
            var operationElement = GetSubmodelElementByPathSubmodelRepo(submodelIdentifier, idShortPath, out _);

            if (operationElement != null)
            {
                if (operationElement is Operation operation)
                {
                    CheckOperationVariables(operation, operationRequest);
                    OperationResult operationResult = new OperationResult();

                    return operationResult;
                }
                else
                {
                    throw new Exception($"Submodel element {operationElement.IdShort} is not of Type Operation.");
                }
            }

            return null;
        }

        private void CheckOperationVariables(Operation operation, OperationRequest operationRequest)
        {
            if (operation.InputVariables.Count != operationRequest.InputArguments.Count)
            {
                throw new Exception($"Incorrect number of InputVariables in OperationRequest.");
            }
            else if (operation.InoutputVariables.Count != operationRequest.InoutputArguments.Count)
            {
                throw new Exception($"Incorrect number of InOutputVariables in OperationRequest.");
            }
        }

        public OperationResult InvokeOperationAsyncSubmodelRepo(string submodelIdentifier, string idShortPath, OperationRequest operationRequest)
        {
            var operationElement = GetSubmodelElementByPathSubmodelRepo(submodelIdentifier, idShortPath, out _);

            if (operationElement != null)
            {
                if (operationElement is Operation operation)
                {
                    CheckOperationVariables(operation, operationRequest);
                    OperationResult operationHandle = new OperationResult();

                    return operationHandle;
                }
                else
                {
                    throw new Exception($"Submodel element {operationElement.IdShort} is not of Type Operation.");
                }
            }

            return null;
        }
    }
}
