
namespace AdminShell
{
    using Aml.Engine.CAEX;
    using Kusto.Cloud.Platform.Utils;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using static AAS_Repository.Pages.TreePage;

    public class VisualTreeBuilderService
    {
        private readonly UANodesetViewer _viewer;
        private readonly AASXPackageService _packageService;

        public static event EventHandler NewDataAvailable;

        public class NewDataAvailableArgs : EventArgs
        {
            public TreeUpdateMode signalNewDataMode;

            public NewDataAvailableArgs(TreeUpdateMode mode = TreeUpdateMode.Rebuild)
            {
                signalNewDataMode = mode;
            }
        }

        public enum TreeUpdateMode
        {
            ValuesOnly = 0,     // same tree, only values changed
            Rebuild,            // same tree, structure may change
            RebuildAndCollapse  // build new tree, keep open nodes
        }

        public VisualTreeBuilderService(UANodesetViewer viewer, AASXPackageService packages )
        {
            _viewer = viewer;
            _packageService = packages;
        }

        public static void SignalNewData(TreeUpdateMode mode)
        {
            NewDataAvailable?.Invoke(null, new NewDataAvailableArgs(mode));
        }

        public List<TreeNodeData> BuildTree()
        {
            List<TreeNodeData> viewItems = new List<TreeNodeData>();

            for (int i = 0; i < _packageService.Packages.Values.Count; i++)
            {
                TreeNodeData root = new TreeNodeData();
                root.EnvIndex = i;
                root.Text = _packageService.Packages.Values.ToList()[i].AssetAdministrationShells[0].IdShort;
                root.Tag = _packageService.Packages.Values.ToList()[i].AssetAdministrationShells[0];
                CreateViewFromAASEnv(root, _packageService.Packages.Values.ToList()[i], i);
                viewItems.Add(root);
            }

            return viewItems;
        }

        private void CreateViewFromAASEnv(TreeNodeData root, AssetAdministrationShellEnvironment aasEnv, int i)
        {
            List<TreeNodeData> subModelTreeNodeDataList = new List<TreeNodeData>();
            foreach (Submodel subModel in aasEnv.Submodels)
            {
                if (subModel != null && subModel.IdShort != null)
                {
                    TreeNodeData subModelTreeNodeData = new()
                    {
                        EnvIndex = i,
                        Text = subModel.IdShort,
                        Tag = subModel
                    };

                    subModelTreeNodeDataList.Add(subModelTreeNodeData);
                    CreateViewFromSubModel(subModelTreeNodeData, subModel, i);
                }
            }

            root.Children = subModelTreeNodeDataList;

            foreach (TreeNodeData nodeData in subModelTreeNodeDataList)
            {
                nodeData.Parent = root;
            }
        }

        private void CreateViewFromSubModel(TreeNodeData rootItem, Submodel subModel, int i)
        {
            List<TreeNodeData> subModelElementTreeNodeDataList = new List<TreeNodeData>();
            foreach (SubmodelElementWrapper smew in subModel.SubmodelElements)
            {
                TreeNodeData subModelElementTreeNodeData = new()
                {
                    EnvIndex = i,
                    Text = smew.SubmodelElement.IdShort,
                    Tag = smew.SubmodelElement
                };

                subModelElementTreeNodeDataList.Add(subModelElementTreeNodeData);

                if (smew.SubmodelElement is SubmodelElementCollection)
                {
                    SubmodelElementCollection submodelElementCollection = smew.SubmodelElement as SubmodelElementCollection;
                    CreateViewFromSubModelElementCollection(subModelElementTreeNodeData, submodelElementCollection, i);
                }

                if (smew.SubmodelElement is Operation)
                {
                    Operation operation = smew.SubmodelElement as Operation;
                    CreateViewFromOperation(subModelElementTreeNodeData, operation, i);
                }

                if (smew.SubmodelElement is Entity)
                {
                    Entity entity = smew.SubmodelElement as Entity;
                    CreateViewFromEntity(subModelElementTreeNodeData, entity, i);
                }
            }

            rootItem.Children = subModelElementTreeNodeDataList;

            foreach (TreeNodeData nodeData in subModelElementTreeNodeDataList)
            {
                nodeData.Parent = rootItem;
            }
        }

        private void CreateViewFromSubModelElementCollection(TreeNodeData rootItem, SubmodelElementCollection subModelElementCollection, int i)
        {
            List<TreeNodeData> treeNodeDataList = new List<TreeNodeData>();
            foreach (SubmodelElementWrapper smew in subModelElementCollection.Value)
            {
                if (smew != null && smew != null)
                {
                    TreeNodeData smeItem = new()
                    {
                        EnvIndex = i,
                        Text = smew.SubmodelElement.IdShort,
                        Tag = smew.SubmodelElement
                    };

                    treeNodeDataList.Add(smeItem);

                    if (smew.SubmodelElement is SubmodelElementCollection)
                    {
                        SubmodelElementCollection smecNext = smew.SubmodelElement as SubmodelElementCollection;
                        CreateViewFromSubModelElementCollection(smeItem, smecNext, i);
                    }

                    if (smew.SubmodelElement is Operation)
                    {
                        Operation operation = smew.SubmodelElement as Operation;
                        CreateViewFromOperation(smeItem, operation, i);
                    }

                    if (smew.SubmodelElement is Entity)
                    {
                        Entity entity = smew.SubmodelElement as Entity;
                        CreateViewFromEntity(smeItem, entity, i);
                    }

                    if (smew.SubmodelElement.IdShort == "NODESET2_XML"
                    && Uri.IsWellFormedUriString(((File)smew.SubmodelElement).Value, UriKind.Absolute))
                    {
                        CreateViewFromAdminShellNodeset(smeItem, new Uri(((File)smew.SubmodelElement).Value), i);
                    }

                    if (smew.SubmodelElement.IdShort == "CAEX")
                    {
                        CreateViewFromAMLCAEXFile(smeItem, ((File)smew.SubmodelElement).Value, i);
                    }
                }
            }

            rootItem.Children = treeNodeDataList;

            foreach (TreeNodeData nodeData in treeNodeDataList)
            {
                nodeData.Parent = rootItem;
            }
        }

        private void CreateViewFromAMLCAEXFile(TreeNodeData rootItem, string filename, int i)
        {
            try
            {
                Stream packagePartStream = _packageService.GetStreamFromPackagePart(_packageService.Packages.Keys.ToList()[i], filename);
                CAEXDocument doc = CAEXDocument.LoadFromStream(packagePartStream);
                List<TreeNodeData> treeNodeDataList = new List<TreeNodeData>();

                foreach (var instanceHirarchy in doc.CAEXFile.InstanceHierarchy)
                {
                    TreeNodeData smeItem = new()
                    {
                        EnvIndex = i,
                        Text = instanceHirarchy.ID,
                        Type = "AML",
                        Tag = new SubmodelElement() { IdShort = instanceHirarchy.Name },
                        Children = new List<TreeNodeData>()
                    };

                    treeNodeDataList.Add(smeItem);

                    foreach (var internalElement in instanceHirarchy.InternalElement)
                    {
                        CreateViewFromInternalElement(smeItem, (List<TreeNodeData>)smeItem.Children, internalElement, i);
                    }
                }

                foreach (var roleclassLib in doc.CAEXFile.RoleClassLib)
                {
                    TreeNodeData smeItem = new()
                    {
                        EnvIndex = i,
                        Text = roleclassLib.ID,
                        Type = "AML",
                        Tag = new SubmodelElement() { IdShort = roleclassLib.Name },
                        Children = new List<TreeNodeData>()
                    };

                    treeNodeDataList.Add(smeItem);

                    foreach (RoleFamilyType roleClass in roleclassLib.RoleClass)
                    {
                        CreateViewFromRoleClasses(smeItem, (List<TreeNodeData>)smeItem.Children, roleClass, i);
                    }
                }

                foreach (var systemUnitClassLib in doc.CAEXFile.SystemUnitClassLib)
                {
                    TreeNodeData smeItem = new()
                    {
                        EnvIndex = i,
                        Text = systemUnitClassLib.ID,
                        Type = "AML",
                        Tag = new SubmodelElement() { IdShort = systemUnitClassLib.Name },
                        Children = new List<TreeNodeData>()
                    };

                    treeNodeDataList.Add(smeItem);

                    foreach (SystemUnitFamilyType systemUnitClass in systemUnitClassLib.SystemUnitClass)
                    {
                        CreateViewFromSystemUnitClasses(smeItem, (List<TreeNodeData>)smeItem.Children, systemUnitClass, i);
                    }
                }

                rootItem.Children = treeNodeDataList;

                foreach (TreeNodeData nodeData in treeNodeDataList)
                {
                    nodeData.Parent = rootItem;
                }
            }
            catch (Exception ex)
            {
                // ignore this node
                Console.WriteLine(ex);
            }
        }

        private void CreateViewFromInternalElement(TreeNodeData rootItem, List<TreeNodeData> rootItemChildren, InternalElementType internalElement, int i)
        {
            TreeNodeData smeItem = new()
            {
                EnvIndex = i,
                Text = internalElement.ID,
                Type = "AML",
                Tag = new SubmodelElement() { IdShort = internalElement.Name },
                Parent = rootItem,
                Children = new List<TreeNodeData>()
            };

            rootItemChildren.Add(smeItem);

            foreach (InternalElementType childInternalElement in internalElement.InternalElement)
            {
                CreateViewFromInternalElement(smeItem, (List<TreeNodeData>)smeItem.Children, childInternalElement, i);
            }
        }

        private void CreateViewFromRoleClasses(TreeNodeData rootItem, List<TreeNodeData> rootItemChildren, RoleFamilyType roleClass, int i)
        {
            TreeNodeData smeItem = new()
            {
                EnvIndex = i,
                Text = roleClass.ID,
                Type = "AML",
                Tag = new SubmodelElement() { IdShort = roleClass.Name },
                Parent = rootItem,
                Children = new List<TreeNodeData>()
            };

            rootItemChildren.Add(smeItem);

            foreach (RoleFamilyType childRoleClass in roleClass.RoleClass)
            {
                CreateViewFromRoleClasses(smeItem, (List<TreeNodeData>)smeItem.Children, childRoleClass, i);
            }
        }

        private void CreateViewFromSystemUnitClasses(TreeNodeData rootItem, List<TreeNodeData> rootItemChildren, SystemUnitFamilyType systemUnitClass, int i)
        {
            TreeNodeData smeItem = new()
            {
                EnvIndex = i,
                Text = systemUnitClass.ID,
                Type = "AML",
                Tag = new SubmodelElement() { IdShort = systemUnitClass.Name },
                Parent = rootItem,
                Children = new List<TreeNodeData>()
            };

            rootItemChildren.Add(smeItem);

            foreach (InternalElementType childInternalElement in systemUnitClass.InternalElement)
            {
                CreateViewFromInternalElement(smeItem, (List<TreeNodeData>)smeItem.Children, childInternalElement, i);
            }

            foreach (SystemUnitFamilyType childSystemUnitClass in systemUnitClass.SystemUnitClass)
            {
                CreateViewFromSystemUnitClasses(smeItem, (List<TreeNodeData>)smeItem.Children, childSystemUnitClass, i);
            }
        }

        private void CreateViewFromAdminShellNodeset(TreeNodeData rootItem, Uri uri, int i)
        {
            try
            {
                _viewer.Login(uri.AbsoluteUri, Environment.GetEnvironmentVariable("UACLUsername"), Environment.GetEnvironmentVariable("UACLPassword"));

                NodesetViewerNode rootNode = _viewer.GetRootNode().GetAwaiter().GetResult();
                if (rootNode != null && rootNode.Children)
                {
                    CreateViewFromUANode(rootItem, _viewer, rootNode, i);
                }
            }
            catch (Exception ex)
            {
                // ignore this part of the AAS
                Console.WriteLine(ex);
            }
        }

        private void CreateViewFromUANode(TreeNodeData rootItem, UANodesetViewer viewer, NodesetViewerNode rootNode, int i)
        {
            try
            {
                List<TreeNodeData> treeNodeDataList = new List<TreeNodeData>();
                List<NodesetViewerNode> children = viewer.GetChildren(rootNode.Id).GetAwaiter().GetResult();
                foreach (NodesetViewerNode node in children)
                {
                    TreeNodeData smeItem = new TreeNodeData
                    {
                        EnvIndex = i,
                        Text = node.Text,
                        Type = "UANode",
                        Tag = new SubmodelElement() { IdShort = node.Text }
                    };

                    treeNodeDataList.Add(smeItem);

                    if (node.Children)
                    {
                        CreateViewFromUANode(smeItem, viewer, node, i);
                    }
                }

                rootItem.Children = treeNodeDataList;

                foreach (TreeNodeData nodeData in treeNodeDataList)
                {
                    nodeData.Parent = rootItem;
                }
            }
            catch (Exception ex)
            {
                // ignore this node
                Console.WriteLine(ex);
            }
        }

        private void CreateViewFromOperation(TreeNodeData rootItem, Operation operation, int i)
        {
            List<TreeNodeData> treeNodeDataList = new List<TreeNodeData>();
            foreach (OperationVariable v in operation.InputVariables)
            {
                TreeNodeData smeItem = new TreeNodeData
                {
                    EnvIndex = i,
                    Text = v.Value.SubmodelElement.IdShort,
                    Type = "In",
                    Tag = v.Value.SubmodelElement
                };

                treeNodeDataList.Add(smeItem);
            }

            foreach (OperationVariable v in operation.OutputVariables)
            {
                TreeNodeData smeItem = new TreeNodeData
                {
                    EnvIndex = i,
                    Text = v.Value.SubmodelElement.IdShort,
                    Type = "Out",
                    Tag = v.Value.SubmodelElement
                };

                treeNodeDataList.Add(smeItem);
            }

            foreach (OperationVariable v in operation.InoutputVariables)
            {
                TreeNodeData smeItem = new TreeNodeData
                {
                    EnvIndex = i,
                    Text = v.Value.SubmodelElement.IdShort,
                    Type = "InOut",
                    Tag = v.Value.SubmodelElement
                };

                treeNodeDataList.Add(smeItem);
            }

            rootItem.Children = treeNodeDataList;

            foreach (TreeNodeData nodeData in treeNodeDataList)
            {
                nodeData.Parent = rootItem;
            }
        }

        private void CreateViewFromEntity(TreeNodeData rootItem, Entity entity, int i)
        {
            List<TreeNodeData> treeNodeDataList = new List<TreeNodeData>();
            foreach (SubmodelElementWrapper statement in entity.Statements)
            {
                if (statement != null && statement != null)
                {
                    TreeNodeData smeItem = new TreeNodeData
                    {
                        EnvIndex = i,
                        Text = statement.SubmodelElement.IdShort,
                        Type = "In",
                        Tag = statement.SubmodelElement
                    };

                    treeNodeDataList.Add(smeItem);
                }
            }

            rootItem.Children = treeNodeDataList;

            foreach (TreeNodeData nodeData in treeNodeDataList)
            {
                nodeData.Parent = rootItem;
            }
        }
    }
}
