
namespace AdminShell
{
    using AdminShell.Data;
    using Aml.Engine.CAEX;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using static AAS_Repository.Pages.TreePage;

    public class TreeBuilder
    {
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

        public static void SignalNewData(TreeUpdateMode mode)
        {
            NewDataAvailable?.Invoke(null, new NewDataAvailableArgs(mode));
        }

        public List<TreeNodeData> BuildTree()
        {
            List<TreeNodeData> viewItems = new List<TreeNodeData>();

            for (int i = 0; i < Program.env.Count; i++)
            {
                TreeNodeData root = new TreeNodeData();
                root.EnvIndex = i;
                if (Program.env[i] != null)
                {
                    root.Text = Program.env[i].AasEnv.AssetAdministrationShells[0].IdShort;
                    root.Tag = Program.env[i].AasEnv.AssetAdministrationShells[0];
                    CreateViewFromAASEnv(root, Program.env[i].AasEnv, i);
                    viewItems.Add(root);
                }
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
                    TreeNodeData subModelTreeNodeData = new TreeNodeData();
                    subModelTreeNodeData.EnvIndex = i;
                    subModelTreeNodeData.Text = subModel.IdShort;
                    subModelTreeNodeData.Tag = subModel;
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
                TreeNodeData subModelElementTreeNodeData = new TreeNodeData();
                subModelElementTreeNodeData.EnvIndex = i;
                subModelElementTreeNodeData.Text = smew.SubmodelElement.IdShort;
                subModelElementTreeNodeData.Tag = smew;
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
            foreach (SubmodelElement subModelElement in subModelElementCollection.Value)
            {
                if (subModelElement != null && subModelElement != null)
                {
                    TreeNodeData smeItem = new TreeNodeData();
                    smeItem.EnvIndex = i;
                    smeItem.Text = subModelElement.IdShort;
                    smeItem.Tag = subModelElement;
                    treeNodeDataList.Add(smeItem);

                    if (subModelElement is SubmodelElementCollection)
                    {
                        SubmodelElementCollection smecNext = subModelElement as SubmodelElementCollection;
                        CreateViewFromSubModelElementCollection(smeItem, smecNext, i);
                    }

                    if (subModelElement is Operation)
                    {
                        Operation operation = subModelElement as Operation;
                        CreateViewFromOperation(smeItem, operation, i);
                    }

                    if (subModelElement is Entity)
                    {
                        Entity entity = subModelElement as Entity;
                        CreateViewFromEntity(smeItem, entity, i);
                    }

                    if ((subModelElement.IdShort == "NODESET2_XML")
                    && Uri.IsWellFormedUriString(subModelElement.ValueAsText(), UriKind.Absolute))
                    {
                        CreateViewFromAdminShellNodeset(smeItem, new Uri(subModelElement.ValueAsText()), i);
                    }

                    if (subModelElement.IdShort == "CAEX")

                    {
                        CreateViewFromAMLCAEXFile(smeItem, subModelElement.ValueAsText(), i);
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
                Stream packagePartStream = Program.env[i].GetLocalStreamFromPackage(filename);
                CAEXDocument doc = CAEXDocument.LoadFromStream(packagePartStream);
                List<TreeNodeData> treeNodeDataList = new List<TreeNodeData>();

                foreach (var instanceHirarchy in doc.CAEXFile.InstanceHierarchy)
                {
                    TreeNodeData smeItem = new TreeNodeData();
                    smeItem.EnvIndex = i;
                    smeItem.Text = instanceHirarchy.ID;
                    smeItem.Type = "AML";
                    smeItem.Tag = new SubmodelElement() { IdShort = instanceHirarchy.Name };
                    smeItem.Children = new List<TreeNodeData>();
                    treeNodeDataList.Add(smeItem);

                    foreach (var internalElement in instanceHirarchy.InternalElement)
                    {
                        CreateViewFromInternalElement(smeItem, (List<TreeNodeData>)smeItem.Children, internalElement, i);
                    }
                }

                foreach (var roleclassLib in doc.CAEXFile.RoleClassLib)
                {
                    TreeNodeData smeItem = new TreeNodeData();
                    smeItem.EnvIndex = i;
                    smeItem.Text = roleclassLib.ID;
                    smeItem.Type = "AML";
                    smeItem.Tag = new SubmodelElement() { IdShort = roleclassLib.Name };
                    smeItem.Children = new List<TreeNodeData>();
                    treeNodeDataList.Add(smeItem);

                    foreach (RoleFamilyType roleClass in roleclassLib.RoleClass)
                    {
                        CreateViewFromRoleClasses(smeItem, (List<TreeNodeData>)smeItem.Children, roleClass, i);
                    }
                }

                foreach (var systemUnitClassLib in doc.CAEXFile.SystemUnitClassLib)
                {
                    TreeNodeData smeItem = new TreeNodeData();
                    smeItem.EnvIndex = i;
                    smeItem.Text = systemUnitClassLib.ID;
                    smeItem.Type = "AML";
                    smeItem.Tag = new SubmodelElement() { IdShort = systemUnitClassLib.Name };
                    smeItem.Children = new List<TreeNodeData>();
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
            TreeNodeData smeItem = new TreeNodeData();
            smeItem.EnvIndex = i;
            smeItem.Text = internalElement.ID;
            smeItem.Type = "AML";
            smeItem.Tag = new SubmodelElement() { IdShort = internalElement.Name };
            smeItem.Parent = rootItem;
            smeItem.Children = new List<TreeNodeData>();
            rootItemChildren.Add(smeItem);

            foreach (InternalElementType childInternalElement in internalElement.InternalElement)
            {
                CreateViewFromInternalElement(smeItem, (List<TreeNodeData>)smeItem.Children, childInternalElement, i);
            }
        }

        private void CreateViewFromRoleClasses(TreeNodeData rootItem, List<TreeNodeData> rootItemChildren, RoleFamilyType roleClass, int i)
        {
            TreeNodeData smeItem = new TreeNodeData();
            smeItem.EnvIndex = i;
            smeItem.Text = roleClass.ID;
            smeItem.Type = "AML";
            smeItem.Tag = new SubmodelElement() { IdShort = roleClass.Name };
            smeItem.Parent = rootItem;
            smeItem.Children = new List<TreeNodeData>();
            rootItemChildren.Add(smeItem);

            foreach (RoleFamilyType childRoleClass in roleClass.RoleClass)
            {
                CreateViewFromRoleClasses(smeItem, (List<TreeNodeData>)smeItem.Children, childRoleClass, i);
            }
        }

        private void CreateViewFromSystemUnitClasses(TreeNodeData rootItem, List<TreeNodeData> rootItemChildren, SystemUnitFamilyType systemUnitClass, int i)
        {
            TreeNodeData smeItem = new TreeNodeData();
            smeItem.EnvIndex = i;
            smeItem.Text = systemUnitClass.ID;
            smeItem.Type = "AML";
            smeItem.Tag = new SubmodelElement() { IdShort = systemUnitClass.Name };
            smeItem.Parent = rootItem;
            smeItem.Children = new List<TreeNodeData>();
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
                UANodesetViewer viewer = new UANodesetViewer();
                viewer.Login(uri.AbsoluteUri, Environment.GetEnvironmentVariable("UACLUsername"), Environment.GetEnvironmentVariable("UACLPassword"));

                NodesetViewerNode rootNode = viewer.GetRootNode().GetAwaiter().GetResult();
                if ((rootNode != null) && rootNode.Children)
                {
                    CreateViewFromUANode(rootItem, viewer, rootNode, i);
                }

                viewer.Disconnect();
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
                    TreeNodeData smeItem = new TreeNodeData();
                    smeItem.EnvIndex = i;
                    smeItem.Text = node.Text;
                    smeItem.Type = "UANode";
                    smeItem.Tag = new SubmodelElement() { IdShort = node.Text };
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
                TreeNodeData smeItem = new TreeNodeData();
                smeItem.EnvIndex = i;
                smeItem.Text = v.Value.SubmodelElement.IdShort;
                smeItem.Type = "In";
                smeItem.Tag = v.Value.SubmodelElement;
                treeNodeDataList.Add(smeItem);
            }

            foreach (OperationVariable v in operation.OutputVariables)
            {
                TreeNodeData smeItem = new TreeNodeData();
                smeItem.EnvIndex = i;
                smeItem.Text = v.Value.SubmodelElement.IdShort;
                smeItem.Type = "Out";
                smeItem.Tag = v.Value.SubmodelElement;
                treeNodeDataList.Add(smeItem);
            }

            foreach (OperationVariable v in operation.InoutputVariables)
            {
                TreeNodeData smeItem = new TreeNodeData();
                smeItem.EnvIndex = i;
                smeItem.Text = v.Value.SubmodelElement.IdShort;
                smeItem.Type = "InOut";
                smeItem.Tag = v.Value.SubmodelElement;
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
            foreach (SubmodelElement statement in entity.Statements)
            {
                if (statement != null && statement != null)
                {
                    TreeNodeData smeItem = new TreeNodeData();
                    smeItem.EnvIndex = i;
                    smeItem.Text = statement.IdShort;
                    smeItem.Type = "In";
                    smeItem.Tag = statement;
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
