
using Microsoft.Extensions.Logging;
using Opc.Ua;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdminShell
{
    public class AssetAdministrationShellEnvironmentService
    {
        private readonly UAClient _client;

        public AssetAdministrationShellEnvironmentService(UAClient client)
        {
            _client = client;
        }

        public List<AssetAdministrationShell> GetAllAssetAdministrationShells(List<string> assetIds = null, string idShort = null)
        {
            List<AssetAdministrationShell> output = new();

            // get all AASes
            List<NodesetViewerNode> nodeList = _client.GetChildren(ObjectIds.ObjectsFolder.ToString(), string.Empty).GetAwaiter().GetResult();
            if (nodeList != null)
            {
                foreach (NodesetViewerNode node in nodeList)
                {
                    if (node.Text == "Asset Admin Shells")
                    {
                        List<NodesetViewerNode> aasList = _client.GetChildren(node.Id, node.SessionId).GetAwaiter().GetResult();
                        if (aasList != null)
                        {
                            foreach (NodesetViewerNode a in aasList)
                            {
                                AssetAdministrationShell aas = new()
                                {
                                    ModelType = ModelTypes.AssetAdministrationShell,
                                    Identification = new Identifier() { Id = a.Text, Value = a.Text },
                                    IdShort = a.Text
                                };

                                // get all asset and submodel refs
                                List<NodesetViewerNode> assetsAndSubmodelRefs = _client.GetChildren(a.Id, a.SessionId).GetAwaiter().GetResult();
                                if (assetsAndSubmodelRefs != null)
                                {
                                    foreach (NodesetViewerNode s in assetsAndSubmodelRefs)
                                    {
                                        if (s.Text.ToLower().Contains("https://admin-shell.io/idta/asset/"))
                                        {
                                            aas.AssetInformation = new AssetInformation() { AssetKind = AssetKind.Instance, SpecificAssetIds = new List<IdentifierKeyValuePair>() { new IdentifierKeyValuePair() { Key = s.Text } } };
                                        }

                                        if (s.Text.ToLower().Contains("https://admin-shell.io/idta/submodel"))
                                        {
                                            aas.Submodels.Add(new ModelReference() { Keys = new List<Key>() { new Key() { Value = s.Text, Type = KeyElements.Submodel } } });
                                        }
                                    }
                                }

                                output.Add(aas);
                            }
                        }
                    }
                }
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
                    List<AssetAdministrationShell> filteredAASes = new();
                    foreach (var assetId in assetIds)
                    {
                        filteredAASes.AddRange(output.Where(a => a.AssetInformation.SpecificAssetIds.Contains(new IdentifierKeyValuePair() { Key = assetId })).ToList());
                    }

                    if (filteredAASes.Any())
                    {
                        return filteredAASes;
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
            List<Submodel> output = new();

            // Get All Submodels
            List<NodesetViewerNode> nodeList = _client.GetChildren(ObjectIds.ObjectsFolder.ToString(), string.Empty).GetAwaiter().GetResult();
            if (nodeList != null)
            {
                foreach (NodesetViewerNode node in nodeList)
                {
                    if (node.Text == "Submodels")
                    {
                        List<NodesetViewerNode> submodelList = _client.GetChildren(node.Id, node.SessionId).GetAwaiter().GetResult();
                        if (submodelList != null)
                        {
                            foreach (NodesetViewerNode subNode in submodelList)
                            {
                                Submodel sub = new()
                                {
                                    ModelType = ModelTypes.Submodel,
                                    Id = subNode.Text,
                                    Identification = new Identifier() { Id = subNode.Text, Value = subNode.Text },
                                    IdShort = subNode.Text,
                                    SemanticId = new Reference() { Type = KeyElements.ExternalReference, Keys = new List<Key>() { new Key() { Value = subNode.Text, Type = KeyElements.GlobalReference } } },
                                    DisplayName = new List<LangString>() { new LangString() { Text = subNode.Text } },
                                    Description = new List<LangString>() { new LangString() { Text = _client.VariableRead(subNode.Id, subNode.SessionId).GetAwaiter().GetResult() } }
                                };

                                // get all submodel elements
                                sub.SubmodelElements.AddRange(ReadSubmodelElementNodes(subNode));

                                output.Add(sub);
                            }
                        }
                    }
                }
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
                        Console.WriteLine($"Submodels with IdShort {idShort} Not Found.");
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
                            Console.WriteLine($"Submodels with requested SemnaticId Not Found.");
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
                            ModelType = ModelTypes.SubmodelElementCollection,
                            DisplayName = new List<LangString>() { new LangString() { Text = smeNode.Text } },
                            IdShort = smeNode.Text,
                            SemanticId = new SemanticId() { Type = KeyElements.ExternalReference, Keys = new List<Key>() { new Key() { Value = smeNode.Text, Type = KeyElements.GlobalReference } } }
                        };

                        smel.Value.AddRange(children);

                        output.Add(smel);
                    }
                    else
                    {
                        Property sme = new()
                        {
                            ModelType = ModelTypes.Property,
                            DisplayName = new List<LangString>() { new LangString() { Text = smeNode.Text } },
                            IdShort = smeNode.Text,
                            SemanticId = new SemanticId() {Type = KeyElements.ExternalReference, Keys = new List<Key>() { new Key() { Value = smeNode.Text, Type = KeyElements.GlobalReference } } },
                            Value = _client.VariableRead(smeNode.Id, smeNode.SessionId).GetAwaiter().GetResult()
                        };

                        output.Add(sme);
                    }
                }
            }

            return output;
        }

        internal List<ConceptDescription> GetAllConceptDescriptions(string idShort = null, string reqIsCaseOf = null, string reqDataSpecificationRef = null)
        {
            List<ConceptDescription> output = new();

            // get all concept descriptions
            List<NodesetViewerNode> nodeList = _client.GetChildren(ObjectIds.ObjectsFolder.ToString(), string.Empty).GetAwaiter().GetResult();
            if (nodeList != null)
            {
                foreach (NodesetViewerNode node in nodeList)
                {
                    if (node.Text == "Concept Descriptions")
                    {
                        List<NodesetViewerNode> conceptDescrNodes = _client.GetChildren(node.Id, node.SessionId).GetAwaiter().GetResult();
                        if (conceptDescrNodes != null)
                        {
                            foreach (NodesetViewerNode cdNode in conceptDescrNodes)
                            {
                                ConceptDescription cd = new()
                                {
                                    ModelType = ModelTypes.ConceptDescription,
                                    Identification = new Identifier() { Id = cdNode.Text, Value = cdNode.Text },
                                    IdShort = cdNode.Text
                                };

                                output.Add(cd);
                            }
                        }
                    }
                }
            }

            if (output.Any())
            {
                // Filter AASs based on IdShort
                if (!string.IsNullOrEmpty(idShort))
                {
                    output = output.Where(a => a.IdShort.Equals(idShort)).ToList();
                    if ((output == null) || output?.Count == 0)
                    {
                        throw new Exception($"Concept Description with IdShort {idShort} Not Found.");
                    }
                }
            }

            return output;
        }

        internal ConceptDescription GetConceptDescriptionById(string cdIdentifier)
        {
            IEnumerable<ConceptDescription> cd = GetAllConceptDescriptions().Where(a => a.Identification.Id.Equals(cdIdentifier));
            if (cd.Any())
            {
                return cd.First();
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

        public List<Reference> GetAllSubmodelReferences(string decodedAasId)
        {
            var aas = GetAssetAdministrationShellById(decodedAasId, out _);

            if (aas != null)
            {
                List<Reference> references = new();
                foreach(ModelReference smr in aas.Submodels)
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
            {
                return null;
            }

            return submodel.SubmodelElements;
        }

        public SubmodelElement GetSubmodelElementByPath(string submodelIdentifier, string idShortPath, out object smeParent)
        {
            if (IsSubmodelElementPresent(submodelIdentifier, idShortPath, out SubmodelElement output, out smeParent))
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

            Submodel submodel = GetSubmodelById(submodelIdentifier, out _);
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

            if (parent is Submodel submodel)
            {
                return FindSubmodelElementByIdShort(submodel, idShortPath);
            }
            else if (parent is SubmodelElementList list)
            {
                return FindSubmodelElementByIdShort(list, idShortPath);
            }
            else if (parent is Entity entity)
            {
                return FindSubmodelElementByIdShort(entity, idShortPath);
            }
            else if (parent is DataElement dataElement)
            {
                return FindSubmodelElementByIdShort(dataElement, idShortPath);
            }
            else
            {
                throw new Exception($"Parent of Type {parent.GetType()} not supported.");
            }
        }

        public string GetFileByPath(string submodelIdentifier, string idShortPath, out byte[] byteArray, out long fileSize)
        {
            byteArray = null;
            string fileName = null;
            fileSize = 0;

            SubmodelElement sme = GetSubmodelElementByPath(submodelIdentifier, idShortPath, out _);
            if (sme != null)
            {
                if (sme is File file)
                {
                    fileName = file.Value;
                    byteArray = System.IO.File.ReadAllBytes(System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "wwwroot", fileName));
                    fileSize = byteArray.Length;
                }
                else
                {
                    throw new Exception($"Submodel element {sme.IdShort} is not of Type File.");
                }
            }

            return fileName;
        }

        public string GetThumbnail(string decodedAasIdentifier, out byte[] content, out long fileSize)
        {
            return GetFileByPath(decodedAasIdentifier, "https://admin-shell.io/idta/asset/thumbnail", out content, out fileSize);
        }
    }
}
