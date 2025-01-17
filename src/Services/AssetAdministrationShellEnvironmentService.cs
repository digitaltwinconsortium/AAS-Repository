
namespace AdminShell
{
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class AssetAdministrationShellEnvironmentService
    {
        private readonly ILogger _logger;

        private UAClient _client = new();

        private const string c_aasRoot = "ns=2;i=1";
        private const string c_submodelRoot = "ns=2;i=2";

        public AssetAdministrationShellEnvironmentService(ILoggerFactory logger, UAClient client)
        {
            _logger = logger.CreateLogger("AssetAdministrationShellEnvironmentService");
            _client = client;
        }

        public List<AssetAdministrationShell> GetAllAssetAdministrationShells(List<string> assetIds = null, string idShort = null)
        {
            var output = new List<AssetAdministrationShell>();

            // get all AASes
            List<NodesetViewerNode> nodeList = _client.GetChildren(c_aasRoot, string.Empty).GetAwaiter().GetResult();
            foreach (NodesetViewerNode a in nodeList)
            {
                AssetAdministrationShell aas = new()
                {
                    Identification = new Identifier() { Id = a.Text, Value = a.Text },
                    IdShort = a.Text
                };

                // get all asset and submodel refs
                List<NodesetViewerNode> assetsAndSubmodelRefs = _client.GetChildren(a.Id, string.Empty).GetAwaiter().GetResult();
                foreach (NodesetViewerNode s in assetsAndSubmodelRefs)
                {
                    if (s.Text.ToLower().Contains("https://admin-shell.io/idta/asset/"))
                    {
                        aas.AssetInformation = new AssetInformation() { AssetKind = AssetKind.Instance, SpecificAssetIds = new List<IdentifierKeyValuePair>() { new IdentifierKeyValuePair() { Key = s.Text } } };
                    }

                    if (s.Text.ToLower().Contains("https://admin-shell.io/idta/submodel"))
                    {
                        aas.Submodels.Add(new SubmodelReference() { Keys = new List<Key>() { new Key() { Value = s.Text } } });
                    }
                }

                output.Add(aas);
            }

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

        public List<Submodel> GetAllSubmodels(Reference reqSemanticId = null, string idShort = null)
        {
            List<Submodel> output = new List<Submodel>();

            // Get All Submodels
            List<NodesetViewerNode> nodeList = _client.GetChildren(c_submodelRoot, string.Empty).GetAwaiter().GetResult();
            foreach (NodesetViewerNode subNode in nodeList)
            {
                Submodel sub = new()
                {
                    Id = subNode.Text,
                    Identification = new Identifier() { Id = subNode.Text, Value = subNode.Text },
                    IdShort = subNode.Text,
                    SemanticId = new Reference() { Keys = new List<Key>() { new Key() { Value = subNode.Text } } },
                    DisplayName = new List<LangString>() { new LangString() { Text = subNode.Text } },
                    Description = new List<LangString>() { new LangString() { Text = _client.VariableRead(subNode.Id, subNode.SessionId).GetAwaiter().GetResult() } }
                };

                // get all submodel elements
                sub.SubmodelElements.AddRange(ReadSubmodelElementNodes(subNode));

                output.Add(sub);
            }

            // Apply filters
            if (output.Any())
            {
                // Filter based on idShort
                if (!string.IsNullOrEmpty(idShort))
                {
                    var submodels = output.Where(s => s.IdShort.Equals(idShort)).ToList();
                    if ((submodels == null) || (submodels?.Count == 0))
                    {
                        _logger.LogInformation($"Submodels with IdShort {idShort} Not Found.");
                    }

                    output = submodels;
                }

                // Filter based on SemanticId
                if ((reqSemanticId != null) && (reqSemanticId.Keys[0].Value != null))
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

        private List<SubmodelElement> ReadSubmodelElementNodes(NodesetViewerNode subNode)
        {
            List<SubmodelElement> output = new();

            List<NodesetViewerNode> submodelElementNodes = _client.GetChildren(subNode.Id, subNode.SessionId).GetAwaiter().GetResult();
            if (submodelElementNodes != null)
            {
                foreach (NodesetViewerNode smeNode in submodelElementNodes)
                {
                    // check for children - if there are, create a smel instead of an sme
                    List<SubmodelElement> children = ReadSubmodelElementNodes(smeNode);
                    if (children.Count > 0)
                    {
                        SubmodelElementList smel = new()
                        {
                            DisplayName = new List<LangString>() { new LangString() { Text = smeNode.Text } },
                            IdShort = smeNode.Text,
                            SemanticId = new SemanticId() { Keys = new List<Key>() { new Key() { Value = smeNode.Text } } }
                        };

                        smel.Value.AddRange(children);

                        output.Add(smel);
                    }
                    else
                    {
                        SubmodelElement sme = new()
                        {
                            DisplayName = new List<LangString>() { new LangString() { Text = smeNode.Text } },
                            IdShort = smeNode.Text,
                            SemanticId = new SemanticId() { Keys = new List<Key>() { new Key() { Value = smeNode.Text } } }
                        };

                        output.Add(sme);
                    }
                }
            }

            return output;
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

        private bool IsAssetAdministrationShellPresent(string aasIdentifier, out AssetAdministrationShell output, out string key)
        {
            var aas = GetAllAssetAdministrationShells().Where(a => a.Identification.Id.Equals(aasIdentifier));
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
            var submodels = GetAllSubmodels().Where(a => a.Identification.Id.Equals(submodelIdentifier));
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
