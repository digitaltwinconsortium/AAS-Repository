
namespace AdminShell
{
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class AssetAdministrationShellEnvironmentService
    {
        private readonly ILogger _logger;

        private AssetAdministrationShellEnvironment _env = new();

        public AssetAdministrationShellEnvironmentService(ILoggerFactory logger)
        {
            _logger = logger.CreateLogger("AssetAdministrationShellEnvironmentService");
        }

        public AssetAdministrationShellEnvironment GetEnv()
        {
            return _env;
        }

        private bool IsSubmodelPresentInAAS(AssetAdministrationShell aas, string submodelIdentifier)
        {
            if (aas.Submodels.Any(s => s.Keys[0].Value == submodelIdentifier))
            {
                return true;
            }
            else
            {
                throw new Exception($"SubmodelReference with Id {submodelIdentifier} not found in AAS with Id {aas.Identification}");
            }
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
                    return GetFileByPath(submodelIdentifier, idShortPath, out content, out fileSize);
                }
            }

            return null;
        }

        public SubmodelElement GetSubmodelElementByPath(string aasIdentifier, string submodelIdentifier, string idShortPath)
        {
            var aas = GetAssetAdministrationShellById(aasIdentifier, out _);
            if (aas != null)
            {
                if (IsSubmodelPresentInAAS(aas, submodelIdentifier))
                {
                    var output = GetSubmodelElementByPath(submodelIdentifier, idShortPath, out _);
                    return output;
                }
            }

            return null;
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

            // Get All AASs
            output.AddRange(_env.AssetAdministrationShells);

            if (output.Any())
            {
                // Filter AASs based on IdShort
                if (!string.IsNullOrEmpty(idShort))
                {
                    output = output.Where(a => a.IdShort.Equals(idShort)).ToList();
                    if ((output == null) || output?.Count == 0)
                    {
                        throw new Exception($"AssetAdministrationShells with IdShort {idShort} Not Found.");
                    }
                }

                // Filter based on AssetId
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
            var aas = _env.AssetAdministrationShells.Where(a => a.Identification.Id.Equals(aasIdentifier));
            if (aas.Any())
            {
                output = aas.First();
                key = aasIdentifier;
                return true;
            }

            output = null;
            key = null;
            return false;
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
            var submodels = _env.Submodels.Where(a => a.Identification.Id.Equals(submodelIdentifier));
            if (submodels.Any())
            {
                output = submodels.First();
                key = submodelIdentifier;
                return true;
            }

            output = null;
            key = null;
            return false;
        }

        public List<SubmodelElement> GetAllSubmodelElementsFromSubmodel(string submodelIdentifier = null)
        {
            var submodel = GetSubmodelById(submodelIdentifier, out _);
            if (submodel == null)
                return null;

            return submodel.SubmodelElements;
        }

        public List<Submodel> GetAllSubmodels(Reference reqSemanticId = null, string idShort = null)
        {
            List<Submodel> output = new List<Submodel>();

            // Get All Submodels
            foreach (var s in _env.Submodels)
            {
                output.Add(s);
            }

            // Apply filters
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

                // Filter w.r.t. SemanticId
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

        public SubmodelElement GetSubmodelElementByPath(string submodelIdentifier, string idShortPath, out object smeParent)
        {
            bool found = IsSubmodelElementPresent(submodelIdentifier, idShortPath, out SubmodelElement output, out smeParent);

            if (found)
            {
                return output;
            }
            else
            {
                return null;
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
            foreach (SubmodelElement sme in sm.SubmodelElements)
            {
                if (sme.IdShort == idShort)
                {
                    return sme;
                }
            }

            return null;
        }

        private SubmodelElement FindSubmodelElementByIdShort(SubmodelElementList smec, string idShort)
        {
            foreach (SubmodelElement sme in smec.Value)
            {
                if (sme.IdShort == idShort)
                {
                    return sme;
                }
            }

            return null;
        }

        private SubmodelElement FindSubmodelElementByIdShort(SubmodelElement sme, string idShort)
        {
            if (sme.IdShort == idShort)
            {
                return sme;
            }

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
                else if (parent is SubmodelElementList collection)
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
                else if (parent is SubmodelElementList collection)
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

        public string GetFileByPath(string submodelIdentifier, string idShortPath, out byte[] byteArray, out long fileSize)
        {
            byteArray = null;
            string fileName = null;
            fileSize = 0;

            var submodel = GetSubmodelById(submodelIdentifier, out string key);

            var fileElement = GetSubmodelElementByPath(submodelIdentifier, idShortPath, out _);

            if (fileElement != null)
            {
                if (fileElement is File file)
                {
                    fileName = file.Value;
                    byteArray = new byte[0]; // TODO: Get file content
                    fileSize = byteArray.Length;
                }
                else
                {
                    throw new Exception($"Submodel element {fileElement.IdShort} is not of Type File.");
                }
            }

            return fileName;
        }

        public string GetThumbnail(string decodedAasIdentifier, out byte[] content, out long fileSize)
        {
            throw new NotImplementedException();
        }
    }
}
