
namespace AdminShell
{
    using Aml.Engine.CAEX;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using static AAS_Repository.Pages.TreePage;

    public class VisualTreeBuilderService
    {
        private readonly UANodesetViewer _viewer;
        private readonly AASXPackageService _packageService;
        private readonly ILogger _logger;
        private readonly CarbonReportingService _carbonReporting;
        private readonly ProductCarbonFootprintService _pcf;
        private readonly OPCUAPubSubService _smec;

        public VisualTreeBuilderService(ILoggerFactory logger, UANodesetViewer viewer, AASXPackageService packages, CarbonReportingService carbonReporting, ProductCarbonFootprintService pcf, OPCUAPubSubService smec)
        {
            _viewer = viewer;
            _packageService = packages;
            _carbonReporting = carbonReporting;
            _pcf = pcf;
            _smec = smec;
            _logger = logger.CreateLogger("VisualTreeBuilderService");
        }

        public static void SignalNewData(TreeUpdateMode mode)
        {
            // intentionally left blank. Users should actively press refresh on their browser, which will cause the visual tree to be rebuilt.
        }

        public List<TreeNode> CreateViewFromPackages()
        {
            List<TreeNode> viewItems = new List<TreeNode>();

            foreach (KeyValuePair<string, AssetAdministrationShellEnvironment> package in _packageService.Packages)
            {
                TreeNode root = new TreeNode();
                root.EnvKey = package.Key;
                root.Text = package.Key;
                root.Tag = package.Value;
                root.Type = "Package";
                viewItems.Add(root);
            }

            return viewItems;
        }

        public List<TreeNode> CreateViewFromAASEnv(string key, AssetAdministrationShellEnvironment aasEnv)
        {
            List<TreeNode> treeNodeDataList = new();

            if (key.Contains(".NodeSet2.xml.aasx"))
            {
                TreeNode uaNodesetTreeNodeData = new()
                {
                    EnvKey = key,
                    Text = key,
                    Type = "UANodeSet"
                };

                treeNodeDataList.Add(uaNodesetTreeNodeData);
            }
            else
            {
                foreach (AssetAdministrationShell aas in aasEnv.AssetAdministrationShells)
                {
                    if (aas != null && aas.IdShort != null)
                    {
                        TreeNode subModelTreeNodeData = new()
                        {
                            EnvKey = key,
                            Text = aas.IdShort,
                            Tag = aas,
                            Type = "AssetAdminShell"
                        };

                        treeNodeDataList.Add(subModelTreeNodeData);
                    }
                }

                foreach (ConceptDescription descr in aasEnv.ConceptDescriptions)
                {
                    if (descr != null && descr.IdShort != null)
                    {
                        TreeNode subModelTreeNodeData = new()
                        {
                            EnvKey = key,
                            Text = descr.IdShort,
                            Tag = descr,
                            Type = "ConceptDescr"
                        };

                        treeNodeDataList.Add(subModelTreeNodeData);
                    }
                }

                foreach (Submodel subModel in aasEnv.Submodels)
                {
                    if (subModel != null && subModel.IdShort != null)
                    {
                        TreeNode subModelTreeNodeData = new()
                        {
                            EnvKey = key,
                            Text = subModel.IdShort,
                            Tag = subModel,
                            Type = "Submodel"
                        };

                        treeNodeDataList.Add(subModelTreeNodeData);
                    }
                }
            }

            return treeNodeDataList;
        }

        public List<TreeNode> CreateViewFromAASSubModel(string key, Submodel subModel)
        {
            List<TreeNode> subModelElementTreeNodeDataList = new();

            foreach (SubmodelElementWrapper smew in subModel.SubmodelElements)
            {
                TreeNode subModelElementTreeNodeData = new()
                {
                    EnvKey = key,
                    Text = smew.SubmodelElement.IdShort,
                    Tag = smew.SubmodelElement,
                    Type = "SubmodelElement"
                };

                subModelElementTreeNodeDataList.Add(subModelElementTreeNodeData);


            }

            return subModelElementTreeNodeDataList;
        }

        public List<TreeNode> CreateViewFromAASSMECollection(string key, SubmodelElementCollection subModelElementCollection)
        {
            List<TreeNode> treeNodeDataList = new();

            foreach (SubmodelElementWrapper smew in subModelElementCollection.Value)
            {
                if (smew != null && smew != null)
                {
                    TreeNode smeItem = new()
                    {
                        EnvKey = key,
                        Text = smew.SubmodelElement.IdShort,
                        Tag = smew.SubmodelElement,
                        Type = "SubmodelElement"
                    };

                    treeNodeDataList.Add(smeItem);
                }
            }

            return treeNodeDataList;
        }

        public List<TreeNode> CreateViewFromAMLCAEXFile(string key, string filename)
        {
            List<TreeNode> treeNodeDataList = new();

            try
            {
                byte[] fileContents = _packageService.GetFileContentsFromPackagePart(key, filename);
                CAEXDocument doc = CAEXDocument.LoadFromBinary(fileContents);

                foreach (var instanceHirarchy in doc.CAEXFile.InstanceHierarchy)
                {
                    TreeNode smeItem = new()
                    {
                        EnvKey = key,
                        Text = instanceHirarchy.ID,
                        Type = "AMLFile",
                        Tag = new SubmodelElement() { IdShort = instanceHirarchy.Name },
                        Children = new List<TreeNode>()
                    };

                    treeNodeDataList.Add(smeItem);

                    foreach (var internalElement in instanceHirarchy.InternalElement)
                    {
                        smeItem.Children = CreateViewFromAMLInternalElement(key, internalElement);
                    }
                }

                foreach (var roleclassLib in doc.CAEXFile.RoleClassLib)
                {
                    TreeNode smeItem = new()
                    {
                        EnvKey = key,
                        Text = roleclassLib.ID,
                        Type = "AMLRole",
                        Tag = new SubmodelElement() { IdShort = roleclassLib.Name },
                        Children = new List<TreeNode>()
                    };

                    treeNodeDataList.Add(smeItem);

                    foreach (RoleFamilyType roleClass in roleclassLib.RoleClass)
                    {
                        smeItem.Children = CreateViewFromAMLRoleClasses(key, roleClass);
                    }
                }

                foreach (var systemUnitClassLib in doc.CAEXFile.SystemUnitClassLib)
                {
                    TreeNode smeItem = new()
                    {
                        EnvKey = key,
                        Text = systemUnitClassLib.ID,
                        Type = "AMLSystemUnit",
                        Tag = new SubmodelElement() { IdShort = systemUnitClassLib.Name },
                        Children = new List<TreeNode>()
                    };

                    treeNodeDataList.Add(smeItem);

                    foreach (SystemUnitFamilyType systemUnitClass in systemUnitClassLib.SystemUnitClass)
                    {
                        smeItem.Children = CreateViewFromAMLSystemUnitClasses(key, systemUnitClass);
                    }
                }

                return treeNodeDataList;
            }
            catch (Exception ex)
            {
                // ignore this node
                _logger.LogError(ex, ex.Message);
                return treeNodeDataList;
            }
        }

        public List<TreeNode> CreateViewFromAMLInternalElement(string key, InternalElementType internalElement)
        {
            List<TreeNode> treeNodeDataList = new();

            TreeNode smeItem = new()
            {
                EnvKey = key,
                Text = internalElement.ID,
                Type = "AMLInternalElement",
                Tag = new SubmodelElement() { IdShort = internalElement.Name },
                Children = new List<TreeNode>()
            };

            treeNodeDataList.Add(smeItem);

            foreach (InternalElementType childInternalElement in internalElement.InternalElement)
            {
                smeItem.Children = CreateViewFromAMLInternalElement(key, childInternalElement);
            }

            return treeNodeDataList;
        }

        public List<TreeNode> CreateViewFromAMLRoleClasses(string key, RoleFamilyType roleClass)
        {
            List<TreeNode> treeNodeDataList = new();

            TreeNode smeItem = new()
            {
                EnvKey = key,
                Text = roleClass.ID,
                Type = "AMLRole",
                Tag = new SubmodelElement() { IdShort = roleClass.Name },
                Children = new List<TreeNode>()
            };

            treeNodeDataList.Add(smeItem);

            foreach (RoleFamilyType childRoleClass in roleClass.RoleClass)
            {
                smeItem.Children = CreateViewFromAMLRoleClasses(key, childRoleClass);
            }

            return treeNodeDataList;
        }

        public List<TreeNode> CreateViewFromAMLSystemUnitClasses(string key, SystemUnitFamilyType systemUnitClass)
        {
            List<TreeNode> treeNodeDataList = new();

            TreeNode smeItem = new()
            {
                EnvKey = key,
                Text = systemUnitClass.ID,
                Type = "AMLSystemUnit",
                Tag = new SubmodelElement() { IdShort = systemUnitClass.Name },
                Children = new List<TreeNode>()
            };

            treeNodeDataList.Add(smeItem);

            foreach (InternalElementType childInternalElement in systemUnitClass.InternalElement)
            {
                smeItem.Children = CreateViewFromAMLInternalElement(key, childInternalElement);
            }

            foreach (SystemUnitFamilyType childSystemUnitClass in systemUnitClass.SystemUnitClass)
            {
                smeItem.Children = CreateViewFromAMLSystemUnitClasses(key, childSystemUnitClass);
            }

            return treeNodeDataList;
        }

        public List<TreeNode> CreateViewFromUANodesetFile(string key)
        {
            try
            {
                _packageService.ReadFile(key, out Uri uri);
                _viewer.LoadLocalNodesetFile(uri.ToString(), key);

                NodesetViewerNode rootNode = _viewer.GetRootNode(key).GetAwaiter().GetResult();
                if (rootNode != null && rootNode.Children)
                {
                    return CreateViewFromUANode(key, rootNode);
                }
                else
                {
                    return new List<TreeNode>();
                }
            }
            catch (Exception ex)
            {
                // ignore this part of the AAS
                _logger.LogError(ex, ex.Message);
                return new List<TreeNode>();
            }
        }

        public List<TreeNode> CreateViewFromUACloudLibNodeset(string key, Uri uri)
        {
            List<TreeNode> treeNodeDataList = new();

            try
            {
                _viewer.Login(uri.AbsoluteUri, Environment.GetEnvironmentVariable("UACLUsername"), Environment.GetEnvironmentVariable("UACLPassword"), key);

                NodesetViewerNode rootNode = _viewer.GetRootNode(key).GetAwaiter().GetResult();
                if (rootNode != null && rootNode.Children)
                {
                    treeNodeDataList = CreateViewFromUANode(key, rootNode);
                }

                return treeNodeDataList;
            }
            catch (Exception ex)
            {
                // ignore this part of the AAS
                _logger.LogError(ex, ex.Message);
                return treeNodeDataList;
            }
        }

        public List<TreeNode> CreateViewFromUANode(string key, NodesetViewerNode rootNode)
        {
            List<TreeNode> treeNodeDataList = new();

            try
            {
                List<NodesetViewerNode> children = _viewer.GetChildren(rootNode.Id, key).GetAwaiter().GetResult();
                foreach (NodesetViewerNode node in children)
                {
                    TreeNode smeItem = new TreeNode
                    {
                        EnvKey = key,
                        Text = node.Text + _viewer.VariableRead(node.Id, key).GetAwaiter().GetResult(),
                        Type = "UANode",
                        Tag = node
                    };

                    treeNodeDataList.Add(smeItem);
                }

                return treeNodeDataList;
            }
            catch (Exception ex)
            {
                // ignore this node
                _logger.LogError(ex, ex.Message);
                return treeNodeDataList;
            }
        }

        public List<TreeNode> CreateViewFromAASOperation(string key, Operation operation)
        {
            List<TreeNode> treeNodeDataList = new();

            foreach (OperationVariable v in operation.InputVariables)
            {
                TreeNode smeItem = new TreeNode
                {
                    EnvKey = key,
                    Text = v.Value.SubmodelElement.IdShort,
                    Type = "In",
                    Tag = v.Value.SubmodelElement
                };

                treeNodeDataList.Add(smeItem);
            }

            foreach (OperationVariable v in operation.OutputVariables)
            {
                TreeNode smeItem = new TreeNode
                {
                    EnvKey = key,
                    Text = v.Value.SubmodelElement.IdShort,
                    Type = "Out",
                    Tag = v.Value.SubmodelElement
                };

                treeNodeDataList.Add(smeItem);
            }

            foreach (OperationVariable v in operation.InoutputVariables)
            {
                TreeNode smeItem = new TreeNode
                {
                    EnvKey = key,
                    Text = v.Value.SubmodelElement.IdShort,
                    Type = "InOut",
                    Tag = v.Value.SubmodelElement
                };

                treeNodeDataList.Add(smeItem);
            }

            return treeNodeDataList;
        }

        public List<TreeNode> CreateViewFromAASEntity(string key, Entity entity)
        {
            List<TreeNode> treeNodeDataList = new();

            foreach (SubmodelElementWrapper statement in entity.Statements)
            {
                if (statement != null && statement != null)
                {
                    TreeNode smeItem = new TreeNode
                    {
                        EnvKey = key,
                        Text = statement.SubmodelElement.IdShort,
                        Type = "In",
                        Tag = statement.SubmodelElement
                    };

                    treeNodeDataList.Add(smeItem);
                }
            }

            return treeNodeDataList;
        }
    }
}
