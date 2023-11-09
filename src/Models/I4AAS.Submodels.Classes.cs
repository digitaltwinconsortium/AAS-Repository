/* ========================================================================
 * Copyright (c) 2005-2021 The OPC Foundation, Inc. All rights reserved.
 *
 * OPC Foundation MIT License 1.00
 *
 * Permission is hereby granted, free of charge, to any person
 * obtaining a copy of this software and associated documentation
 * files (the "Software"), to deal in the Software without
 * restriction, including without limitation the rights to use,
 * copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the
 * Software is furnished to do so, subject to the following
 * conditions:
 *
 * The above copyright notice and this permission notice shall be
 * included in all copies or substantial portions of the Software.
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
 * OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 * NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
 * HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
 * WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
 * OTHER DEALINGS IN THE SOFTWARE.
 *
 * The complete license agreement can be found here:
 * http://opcfoundation.org/License/MIT/1.00/
 * ======================================================================*/

using Opc.Ua;
using System;
using System.Collections.Generic;

namespace I4AAS.Submodels
{
    #region SubmodelState Class
#if (!OPCUA_EXCLUDE_SubmodelState)
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public partial class SubmodelState : BaseObjectState
    {
        #region Constructors
        /// <remarks />
        public SubmodelState(NodeState parent) : base(parent)
        {
        }

        /// <remarks />
        protected override NodeId GetDefaultTypeDefinitionId(NamespaceTable namespaceUris)
        {
            return Opc.Ua.NodeId.Create(I4AAS.Submodels.ObjectTypes.SubmodelType, I4AAS.Submodels.Namespaces.I4AAS, namespaceUris);
        }

        #if (!OPCUA_EXCLUDE_InitializationStrings)
        /// <remarks />
        protected override void Initialize(ISystemContext context)
        {
            base.Initialize(context);
            Initialize(context, InitializationString);
            InitializeOptionalChildren(context);
        }

        /// <remarks />
        protected override void Initialize(ISystemContext context, NodeState source)
        {
            InitializeOptionalChildren(context);
            base.Initialize(context, source);
        }

        /// <remarks />
        protected override void InitializeOptionalChildren(ISystemContext context)
        {
            base.InitializeOptionalChildren(context);

            if (ModelDescription != null)
            {
                ModelDescription.Initialize(context, ModelDescription_InitializationString);
            }
        }

        #region Initialization String
        private const string ModelDescription_InitializationString =
           "AgAAACIAAABodHRwOi8vb3BjZm91bmRhdGlvbi5vcmcvVUEvSTRBQVMvKwAAAGh0dHA6Ly9vcGNmb3Vu" +
           "ZGF0aW9uLm9yZy9VQS9EaWN0aW9uYXJ5L0lSREn/////FWCJCgIAAAABABAAAABNb2RlbERlc2NyaXB0" +
           "aW9uAQEYAQAuAEQYAQAAABX/////AQH/////AAAAAA==";

        private const string InitializationString =
           "AgAAACIAAABodHRwOi8vb3BjZm91bmRhdGlvbi5vcmcvVUEvSTRBQVMvKwAAAGh0dHA6Ly9vcGNmb3Vu" +
           "ZGF0aW9uLm9yZy9VQS9EaWN0aW9uYXJ5L0lSREn/////BGCAAgEAAAABABQAAABTdWJtb2RlbFR5cGVJ" +
           "bnN0YW5jZQEBAQABAQEAAQAAAP////8DAAAAFWCJCgIAAAABAAcAAABNb2RlbElkAQEWAQAuAEQWAQAA" +
           "AAz/////AQH/////AAAAABVgiQoCAAAAAQAMAAAATW9kZWxJZFNob3J0AQEXAQAuAEQXAQAAAAz/////" +
           "AQH/////AAAAABVgiQoCAAAAAQAQAAAATW9kZWxEZXNjcmlwdGlvbgEBGAEALgBEGAEAAAAV/////wEB" +
           "/////wAAAAA=";
        #endregion
        #endif
        #endregion

        #region Public Properties
        /// <remarks />
        public PropertyState<string> ModelId
        {
            get
            {
                return m_modelId;
            }

            set
            {
                if (!Object.ReferenceEquals(m_modelId, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_modelId = value;
            }
        }

        /// <remarks />
        public PropertyState<string> ModelIdShort
        {
            get
            {
                return m_modelIdShort;
            }

            set
            {
                if (!Object.ReferenceEquals(m_modelIdShort, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_modelIdShort = value;
            }
        }

        /// <remarks />
        public PropertyState<LocalizedText> ModelDescription
        {
            get
            {
                return m_modelDescription;
            }

            set
            {
                if (!Object.ReferenceEquals(m_modelDescription, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_modelDescription = value;
            }
        }
        #endregion

        #region Overridden Methods
        /// <remarks />
        public override void GetChildren(
            ISystemContext context,
            IList<BaseInstanceState> children)
        {
            if (m_modelId != null)
            {
                children.Add(m_modelId);
            }

            if (m_modelIdShort != null)
            {
                children.Add(m_modelIdShort);
            }

            if (m_modelDescription != null)
            {
                children.Add(m_modelDescription);
            }

            base.GetChildren(context, children);
        }

        /// <remarks />
        protected override BaseInstanceState FindChild(
            ISystemContext context,
            QualifiedName browseName,
            bool createOrReplace,
            BaseInstanceState replacement)
        {
            if (QualifiedName.IsNull(browseName))
            {
                return null;
            }

            BaseInstanceState instance = null;

            switch (browseName.Name)
            {
                case I4AAS.Submodels.BrowseNames.ModelId:
                {
                    if (createOrReplace)
                    {
                        if (ModelId == null)
                        {
                            if (replacement == null)
                            {
                                ModelId = new PropertyState<string>(this);
                            }
                            else
                            {
                                ModelId = (PropertyState<string>)replacement;
                            }
                        }
                    }

                    instance = ModelId;
                    break;
                }

                case I4AAS.Submodels.BrowseNames.ModelIdShort:
                {
                    if (createOrReplace)
                    {
                        if (ModelIdShort == null)
                        {
                            if (replacement == null)
                            {
                                ModelIdShort = new PropertyState<string>(this);
                            }
                            else
                            {
                                ModelIdShort = (PropertyState<string>)replacement;
                            }
                        }
                    }

                    instance = ModelIdShort;
                    break;
                }

                case I4AAS.Submodels.BrowseNames.ModelDescription:
                {
                    if (createOrReplace)
                    {
                        if (ModelDescription == null)
                        {
                            if (replacement == null)
                            {
                                ModelDescription = new PropertyState<LocalizedText>(this);
                            }
                            else
                            {
                                ModelDescription = (PropertyState<LocalizedText>)replacement;
                            }
                        }
                    }

                    instance = ModelDescription;
                    break;
                }
            }

            if (instance != null)
            {
                return instance;
            }

            return base.FindChild(context, browseName, createOrReplace, replacement);
        }
        #endregion

        #region Private Fields
        private PropertyState<string> m_modelId;
        private PropertyState<string> m_modelIdShort;
        private PropertyState<LocalizedText> m_modelDescription;
        #endregion
    }
    #endif
    #endregion

    #region AssetModelState Class
    #if (!OPCUA_EXCLUDE_AssetModelState)
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public partial class AssetModelState : BaseObjectState
    {
        #region Constructors
        /// <remarks />
        public AssetModelState(NodeState parent) : base(parent)
        {
        }

        /// <remarks />
        protected override NodeId GetDefaultTypeDefinitionId(NamespaceTable namespaceUris)
        {
            return Opc.Ua.NodeId.Create(I4AAS.Submodels.ObjectTypes.AssetModelType, I4AAS.Submodels.Namespaces.I4AAS, namespaceUris);
        }

        #if (!OPCUA_EXCLUDE_InitializationStrings)
        /// <remarks />
        protected override void Initialize(ISystemContext context)
        {
            base.Initialize(context);
            Initialize(context, InitializationString);
            InitializeOptionalChildren(context);
        }

        /// <remarks />
        protected override void Initialize(ISystemContext context, NodeState source)
        {
            InitializeOptionalChildren(context);
            base.Initialize(context, source);
        }

        /// <remarks />
        protected override void InitializeOptionalChildren(ISystemContext context)
        {
            base.InitializeOptionalChildren(context);

            if (AssetFile != null)
            {
                AssetFile.Initialize(context, AssetFile_InitializationString);
            }
        }

        #region Initialization String
        private const string AssetFile_InitializationString =
           "AgAAACIAAABodHRwOi8vb3BjZm91bmRhdGlvbi5vcmcvVUEvSTRBQVMvKwAAAGh0dHA6Ly9vcGNmb3Vu" +
           "ZGF0aW9uLm9yZy9VQS9EaWN0aW9uYXJ5L0lSREn/////BGCACgEAAAABAAkAAABBc3NldEZpbGUBAdMA" +
           "AC8BADct0wAAAP////8KAAAAFWCJCgIAAAAAAAQAAABTaXplAQHUAAAuAETUAAAAAAn/////AQH/////" +
           "AAAAABVgiQoCAAAAAAAIAAAAV3JpdGFibGUBAdUAAC4ARNUAAAAAAf////8BAf////8AAAAAFWCJCgIA" +
           "AAAAAAwAAABVc2VyV3JpdGFibGUBAdYAAC4ARNYAAAAAAf////8BAf////8AAAAAFWCJCgIAAAAAAAkA" +
           "AABPcGVuQ291bnQBAdcAAC4ARNcAAAAABf////8BAf////8AAAAABGGCCgQAAAAAAAQAAABPcGVuAQHb" +
           "AAAvAQA8LdsAAAABAf////8CAAAAF2CpCgIAAAAAAA4AAABJbnB1dEFyZ3VtZW50cwEB3AAALgBE3AAA" +
           "AJYBAAAAAQAqAQETAAAABAAAAE1vZGUAA/////8AAAAAAAEAKAEBAAAAAQAAAAEAAAABAf////8AAAAA" +
           "F2CpCgIAAAAAAA8AAABPdXRwdXRBcmd1bWVudHMBAd0AAC4ARN0AAACWAQAAAAEAKgEBGQAAAAoAAABG" +
           "aWxlSGFuZGxlAAf/////AAAAAAABACgBAQAAAAEAAAABAAAAAQH/////AAAAAARhggoEAAAAAAAFAAAA" +
           "Q2xvc2UBAd4AAC8BAD8t3gAAAAEB/////wEAAAAXYKkKAgAAAAAADgAAAElucHV0QXJndW1lbnRzAQHf" +
           "AAAuAETfAAAAlgEAAAABACoBARkAAAAKAAAARmlsZUhhbmRsZQAH/////wAAAAAAAQAoAQEAAAABAAAA" +
           "AQAAAAEB/////wAAAAAEYYIKBAAAAAAABAAAAFJlYWQBAeAAAC8BAEEt4AAAAAEB/////wIAAAAXYKkK" +
           "AgAAAAAADgAAAElucHV0QXJndW1lbnRzAQHhAAAuAEThAAAAlgIAAAABACoBARkAAAAKAAAARmlsZUhh" +
           "bmRsZQAH/////wAAAAAAAQAqAQEVAAAABgAAAExlbmd0aAAG/////wAAAAAAAQAoAQEAAAABAAAAAgAA" +
           "AAEB/////wAAAAAXYKkKAgAAAAAADwAAAE91dHB1dEFyZ3VtZW50cwEB4gAALgBE4gAAAJYBAAAAAQAq" +
           "AQETAAAABAAAAERhdGEAD/////8AAAAAAAEAKAEBAAAAAQAAAAEAAAABAf////8AAAAABGGCCgQAAAAA" +
           "AAUAAABXcml0ZQEB4wAALwEARC3jAAAAAQH/////AQAAABdgqQoCAAAAAAAOAAAASW5wdXRBcmd1bWVu" +
           "dHMBAeQAAC4AROQAAACWAgAAAAEAKgEBGQAAAAoAAABGaWxlSGFuZGxlAAf/////AAAAAAABACoBARMA" +
           "AAAEAAAARGF0YQAP/////wAAAAAAAQAoAQEAAAABAAAAAgAAAAEB/////wAAAAAEYYIKBAAAAAAACwAA" +
           "AEdldFBvc2l0aW9uAQHlAAAvAQBGLeUAAAABAf////8CAAAAF2CpCgIAAAAAAA4AAABJbnB1dEFyZ3Vt" +
           "ZW50cwEB5gAALgBE5gAAAJYBAAAAAQAqAQEZAAAACgAAAEZpbGVIYW5kbGUAB/////8AAAAAAAEAKAEB" +
           "AAAAAQAAAAEAAAABAf////8AAAAAF2CpCgIAAAAAAA8AAABPdXRwdXRBcmd1bWVudHMBAecAAC4AROcA" +
           "AACWAQAAAAEAKgEBFwAAAAgAAABQb3NpdGlvbgAJ/////wAAAAAAAQAoAQEAAAABAAAAAQAAAAEB////" +
           "/wAAAAAEYYIKBAAAAAAACwAAAFNldFBvc2l0aW9uAQHoAAAvAQBJLegAAAABAf////8BAAAAF2CpCgIA" +
           "AAAAAA4AAABJbnB1dEFyZ3VtZW50cwEB6QAALgBE6QAAAJYCAAAAAQAqAQEZAAAACgAAAEZpbGVIYW5k" +
           "bGUAB/////8AAAAAAAEAKgEBFwAAAAgAAABQb3NpdGlvbgAJ/////wAAAAAAAQAoAQEAAAABAAAAAgAA" +
           "AAEB/////wAAAAA=";

        private const string InitializationString =
           "AgAAACIAAABodHRwOi8vb3BjZm91bmRhdGlvbi5vcmcvVUEvSTRBQVMvKwAAAGh0dHA6Ly9vcGNmb3Vu" +
           "ZGF0aW9uLm9yZy9VQS9EaWN0aW9uYXJ5L0lSREn/////BGCAAgEAAAABABYAAABBc3NldE1vZGVsVHlw" +
           "ZUluc3RhbmNlAQHRAAEB0QDRAAAA/////wIAAAAEYIAKAQAAAAEACQAAAE5hbWVwbGF0ZQEB6gAALwEB" +
           "BADqAAAA/////wcAAAAVYIkKAgAAAAEABwAAAE1vZGVsSWQBARkBAC4ARBkBAAAADP////8BAf////8A" +
           "AAAAFWCJCgIAAAABAAwAAABNb2RlbElkU2hvcnQBARoBAC4ARBoBAAAADP////8BAf////8AAAAAFWCJ" +
           "CgIAAAABAA8AAABVUklPZlRoZVByb2R1Y3QBAesAAC4AROsAAAABAMdc/////wEBAQAAAAEAvUQAAwIA" +
           "FAAAADAxNzMtMSMwMi1BQVk4MTEjMDAxAAAAABVgiQoCAAAAAQAQAAAATWFudWZhY3R1cmVyTmFtZQEB" +
           "7AAALgBE7AAAAAAV/////wEBAQAAAAEAvUQAAwIAFAAAADAxNzMtMSMwMi1BQU82NzcjMDAyAAAAABVg" +
           "iQoCAAAAAQAeAAAATWFudWZhY3R1cmVyUHJvZHVjdERlc2lnbmF0aW9uAQHtAAAuAETtAAAAABX/////" +
           "AQEBAAAAAQC9RAADAgAUAAAAMDE3My0xIzAyLUFBVzMzOCMwMDEAAAAABGCACgEAAAABABIAAABDb250" +
           "YWN0SW5mb3JtYXRpb24BAe4AAC8BAU4A7gAAAAEAAAABAL1EAAMCAFAAAABodHRwczovL2FkbWluLXNo" +
           "ZWxsLmlvL3p2ZWkvbmFtZXBsYXRlLzEvMC9Db250YWN0SW5mb3JtYXRpb25zL0NvbnRhY3RJbmZvcm1h" +
           "dGlvbgYAAAAVYIkKAgAAAAEABwAAAE1vZGVsSWQBARwBAC4ARBwBAAAADP////8BAf////8AAAAAFWCJ" +
           "CgIAAAABAAwAAABNb2RlbElkU2hvcnQBAR0BAC4ARB0BAAAADP////8BAf////8AAAAAFWCJCgIAAAAB" +
           "AAYAAABTdHJlZXQBAe8AAC4ARO8AAAAAFf////8BAQEAAAABAL1EAAMCABQAAAAwMTczLTEjMDItQUFP" +
           "MTI4IzAwMgAAAAAVYIkKAgAAAAEABwAAAFppcGNvZGUBAfAAAC4ARPAAAAAADP////8BAQEAAAABAL1E" +
           "AAMCABQAAAAwMTczLTEjMDItQUFPMTI5IzAwMgAAAAAVYIkKAgAAAAEACAAAAENpdHlUb3duAQHxAAAu" +
           "AETxAAAAABX/////AQEBAAAAAQC9RAADAgAUAAAAMDE3My0xIzAyLUFBTzEzMiMwMDIAAAAAFWCJCgIA" +
           "AAABAAwAAABOYXRpb25hbENvZGUBAfIAAC4ARPIAAAAADP////8BAQEAAAABAL1EAAMCABQAAAAwMTcz" +
           "LTEjMDItQUFPMTM0IzAwMgAAAAAVYIkKAgAAAAEAEgAAAFllYXJPZkNvbnN0cnVjdGlvbgEB+QAALgBE" +
           "+QAAAAAM/////wEBAQAAAAEAvUQAAwIAFAAAADAxNzMtMSMwMi1BQVA5MDYjMDAxAAAAAARggAoBAAAA" +
           "AQAJAAAAQXNzZXRGaWxlAQHTAAAvAQA3LdMAAAD/////CgAAABVgiQoCAAAAAAAEAAAAU2l6ZQEB1AAA" +
           "LgBE1AAAAAAJ/////wEB/////wAAAAAVYIkKAgAAAAAACAAAAFdyaXRhYmxlAQHVAAAuAETVAAAAAAH/" +
           "////AQH/////AAAAABVgiQoCAAAAAAAMAAAAVXNlcldyaXRhYmxlAQHWAAAuAETWAAAAAAH/////AQH/" +
           "////AAAAABVgiQoCAAAAAAAJAAAAT3BlbkNvdW50AQHXAAAuAETXAAAAAAX/////AQH/////AAAAAARh" +
           "ggoEAAAAAAAEAAAAT3BlbgEB2wAALwEAPC3bAAAAAQH/////AgAAABdgqQoCAAAAAAAOAAAASW5wdXRB" +
           "cmd1bWVudHMBAdwAAC4ARNwAAACWAQAAAAEAKgEBEwAAAAQAAABNb2RlAAP/////AAAAAAABACgBAQAA" +
           "AAEAAAABAAAAAQH/////AAAAABdgqQoCAAAAAAAPAAAAT3V0cHV0QXJndW1lbnRzAQHdAAAuAETdAAAA" +
           "lgEAAAABACoBARkAAAAKAAAARmlsZUhhbmRsZQAH/////wAAAAAAAQAoAQEAAAABAAAAAQAAAAEB////" +
           "/wAAAAAEYYIKBAAAAAAABQAAAENsb3NlAQHeAAAvAQA/Ld4AAAABAf////8BAAAAF2CpCgIAAAAAAA4A" +
           "AABJbnB1dEFyZ3VtZW50cwEB3wAALgBE3wAAAJYBAAAAAQAqAQEZAAAACgAAAEZpbGVIYW5kbGUAB///" +
           "//8AAAAAAAEAKAEBAAAAAQAAAAEAAAABAf////8AAAAABGGCCgQAAAAAAAQAAABSZWFkAQHgAAAvAQBB" +
           "LeAAAAABAf////8CAAAAF2CpCgIAAAAAAA4AAABJbnB1dEFyZ3VtZW50cwEB4QAALgBE4QAAAJYCAAAA" +
           "AQAqAQEZAAAACgAAAEZpbGVIYW5kbGUAB/////8AAAAAAAEAKgEBFQAAAAYAAABMZW5ndGgABv////8A" +
           "AAAAAAEAKAEBAAAAAQAAAAIAAAABAf////8AAAAAF2CpCgIAAAAAAA8AAABPdXRwdXRBcmd1bWVudHMB" +
           "AeIAAC4AROIAAACWAQAAAAEAKgEBEwAAAAQAAABEYXRhAA//////AAAAAAABACgBAQAAAAEAAAABAAAA" +
           "AQH/////AAAAAARhggoEAAAAAAAFAAAAV3JpdGUBAeMAAC8BAEQt4wAAAAEB/////wEAAAAXYKkKAgAA" +
           "AAAADgAAAElucHV0QXJndW1lbnRzAQHkAAAuAETkAAAAlgIAAAABACoBARkAAAAKAAAARmlsZUhhbmRs" +
           "ZQAH/////wAAAAAAAQAqAQETAAAABAAAAERhdGEAD/////8AAAAAAAEAKAEBAAAAAQAAAAIAAAABAf//" +
           "//8AAAAABGGCCgQAAAAAAAsAAABHZXRQb3NpdGlvbgEB5QAALwEARi3lAAAAAQH/////AgAAABdgqQoC" +
           "AAAAAAAOAAAASW5wdXRBcmd1bWVudHMBAeYAAC4AROYAAACWAQAAAAEAKgEBGQAAAAoAAABGaWxlSGFu" +
           "ZGxlAAf/////AAAAAAABACgBAQAAAAEAAAABAAAAAQH/////AAAAABdgqQoCAAAAAAAPAAAAT3V0cHV0" +
           "QXJndW1lbnRzAQHnAAAuAETnAAAAlgEAAAABACoBARcAAAAIAAAAUG9zaXRpb24ACf////8AAAAAAAEA" +
           "KAEBAAAAAQAAAAEAAAABAf////8AAAAABGGCCgQAAAAAAAsAAABTZXRQb3NpdGlvbgEB6AAALwEASS3o" +
           "AAAAAQH/////AQAAABdgqQoCAAAAAAAOAAAASW5wdXRBcmd1bWVudHMBAekAAC4AROkAAACWAgAAAAEA" +
           "KgEBGQAAAAoAAABGaWxlSGFuZGxlAAf/////AAAAAAABACoBARcAAAAIAAAAUG9zaXRpb24ACf////8A" +
           "AAAAAAEAKAEBAAAAAQAAAAIAAAABAf////8AAAAA";
        #endregion
        #endif
        #endregion

        #region Public Properties
        /// <remarks />
        public NameplateSubmodelState Nameplate
        {
            get
            {
                return m_nameplate;
            }

            set
            {
                if (!Object.ReferenceEquals(m_nameplate, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_nameplate = value;
            }
        }

        /// <remarks />
        public FileState AssetFile
        {
            get
            {
                return m_assetFile;
            }

            set
            {
                if (!Object.ReferenceEquals(m_assetFile, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_assetFile = value;
            }
        }
        #endregion

        #region Overridden Methods
        /// <remarks />
        public override void GetChildren(
            ISystemContext context,
            IList<BaseInstanceState> children)
        {
            if (m_nameplate != null)
            {
                children.Add(m_nameplate);
            }

            if (m_assetFile != null)
            {
                children.Add(m_assetFile);
            }

            base.GetChildren(context, children);
        }

        /// <remarks />
        protected override BaseInstanceState FindChild(
            ISystemContext context,
            QualifiedName browseName,
            bool createOrReplace,
            BaseInstanceState replacement)
        {
            if (QualifiedName.IsNull(browseName))
            {
                return null;
            }

            BaseInstanceState instance = null;

            switch (browseName.Name)
            {
                case I4AAS.Submodels.BrowseNames.Nameplate:
                {
                    if (createOrReplace)
                    {
                        if (Nameplate == null)
                        {
                            if (replacement == null)
                            {
                                Nameplate = new NameplateSubmodelState(this);
                            }
                            else
                            {
                                Nameplate = (NameplateSubmodelState)replacement;
                            }
                        }
                    }

                    instance = Nameplate;
                    break;
                }

                case I4AAS.Submodels.BrowseNames.AssetFile:
                {
                    if (createOrReplace)
                    {
                        if (AssetFile == null)
                        {
                            if (replacement == null)
                            {
                                AssetFile = new FileState(this);
                            }
                            else
                            {
                                AssetFile = (FileState)replacement;
                            }
                        }
                    }

                    instance = AssetFile;
                    break;
                }
            }

            if (instance != null)
            {
                return instance;
            }

            return base.FindChild(context, browseName, createOrReplace, replacement);
        }
        #endregion

        #region Private Fields
        private NameplateSubmodelState m_nameplate;
        private FileState m_assetFile;
        #endregion
    }
    #endif
    #endregion

    #region NameplateSubmodelState Class
    #if (!OPCUA_EXCLUDE_NameplateSubmodelState)
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public partial class NameplateSubmodelState : SubmodelState
    {
        #region Constructors
        /// <remarks />
        public NameplateSubmodelState(NodeState parent) : base(parent)
        {
        }

        /// <remarks />
        protected override NodeId GetDefaultTypeDefinitionId(NamespaceTable namespaceUris)
        {
            return Opc.Ua.NodeId.Create(I4AAS.Submodels.ObjectTypes.NameplateSubmodelType, I4AAS.Submodels.Namespaces.I4AAS, namespaceUris);
        }

        #if (!OPCUA_EXCLUDE_InitializationStrings)
        /// <remarks />
        protected override void Initialize(ISystemContext context)
        {
            base.Initialize(context);
            Initialize(context, InitializationString);
            InitializeOptionalChildren(context);
        }

        /// <remarks />
        protected override void Initialize(ISystemContext context, NodeState source)
        {
            InitializeOptionalChildren(context);
            base.Initialize(context, source);
        }

        /// <remarks />
        protected override void InitializeOptionalChildren(ISystemContext context)
        {
            base.InitializeOptionalChildren(context);

            if (ManufacturerProductRoot != null)
            {
                ManufacturerProductRoot.Initialize(context, ManufacturerProductRoot_InitializationString);
            }

            if (ManufacturerProductFamily != null)
            {
                ManufacturerProductFamily.Initialize(context, ManufacturerProductFamily_InitializationString);
            }

            if (ManufacturerProductType != null)
            {
                ManufacturerProductType.Initialize(context, ManufacturerProductType_InitializationString);
            }

            if (OrderCodeOfManufacturer != null)
            {
                OrderCodeOfManufacturer.Initialize(context, OrderCodeOfManufacturer_InitializationString);
            }

            if (ProductArticleNumberOfManufacturer != null)
            {
                ProductArticleNumberOfManufacturer.Initialize(context, ProductArticleNumberOfManufacturer_InitializationString);
            }

            if (SerialNumber != null)
            {
                SerialNumber.Initialize(context, SerialNumber_InitializationString);
            }

            if (DateOfManufacture != null)
            {
                DateOfManufacture.Initialize(context, DateOfManufacture_InitializationString);
            }

            if (HardwareVersion != null)
            {
                HardwareVersion.Initialize(context, HardwareVersion_InitializationString);
            }

            if (FirmwareVersion != null)
            {
                FirmwareVersion.Initialize(context, FirmwareVersion_InitializationString);
            }

            if (SoftwareVersion != null)
            {
                SoftwareVersion.Initialize(context, SoftwareVersion_InitializationString);
            }

            if (CountryOfOrigin != null)
            {
                CountryOfOrigin.Initialize(context, CountryOfOrigin_InitializationString);
            }

            if (CompanyLogo != null)
            {
                CompanyLogo.Initialize(context, CompanyLogo_InitializationString);
            }
        }

        #region Initialization String
        private const string ManufacturerProductRoot_InitializationString =
           "AgAAACIAAABodHRwOi8vb3BjZm91bmRhdGlvbi5vcmcvVUEvSTRBQVMvKwAAAGh0dHA6Ly9vcGNmb3Vu" +
           "ZGF0aW9uLm9yZy9VQS9EaWN0aW9uYXJ5L0lSREn/////FWCJCgIAAAABABcAAABNYW51ZmFjdHVyZXJQ" +
           "cm9kdWN0Um9vdAEBDQAALgBEDQAAAAAV/////wEBAQAAAAEAvUQAAwIAFAAAADAxNzMtMSMwMi1BQVU3" +
           "MzIjMDAxAAAAAA==";

        private const string ManufacturerProductFamily_InitializationString =
           "AgAAACIAAABodHRwOi8vb3BjZm91bmRhdGlvbi5vcmcvVUEvSTRBQVMvKwAAAGh0dHA6Ly9vcGNmb3Vu" +
           "ZGF0aW9uLm9yZy9VQS9EaWN0aW9uYXJ5L0lSREn/////FWCJCgIAAAABABkAAABNYW51ZmFjdHVyZXJQ" +
           "cm9kdWN0RmFtaWx5AQEOAAAuAEQOAAAAABX/////AQEBAAAAAQC9RAADAgAUAAAAMDE3My0xIzAyLUFB" +
           "VTczMSMwMDEAAAAA";

        private const string ManufacturerProductType_InitializationString =
           "AgAAACIAAABodHRwOi8vb3BjZm91bmRhdGlvbi5vcmcvVUEvSTRBQVMvKwAAAGh0dHA6Ly9vcGNmb3Vu" +
           "ZGF0aW9uLm9yZy9VQS9EaWN0aW9uYXJ5L0lSREn/////FWCJCgIAAAABABcAAABNYW51ZmFjdHVyZXJQ" +
           "cm9kdWN0VHlwZQEBDwAALgBEDwAAAAAV/////wEBAQAAAAEAvUQAAwIAFAAAADAxNzMtMSMwMi1BQU8w" +
           "NTcjMDAyAAAAAA==";

        private const string OrderCodeOfManufacturer_InitializationString =
           "AgAAACIAAABodHRwOi8vb3BjZm91bmRhdGlvbi5vcmcvVUEvSTRBQVMvKwAAAGh0dHA6Ly9vcGNmb3Vu" +
           "ZGF0aW9uLm9yZy9VQS9EaWN0aW9uYXJ5L0lSREn/////FWCJCgIAAAABABcAAABPcmRlckNvZGVPZk1h" +
           "bnVmYWN0dXJlcgEBEAAALgBEEAAAAAAM/////wEBAQAAAAEAvUQAAwIAFAAAADAxNzMtMSMwMi1BQU8y" +
           "MjcjMDAyAAAAAA==";

        private const string ProductArticleNumberOfManufacturer_InitializationString =
           "AgAAACIAAABodHRwOi8vb3BjZm91bmRhdGlvbi5vcmcvVUEvSTRBQVMvKwAAAGh0dHA6Ly9vcGNmb3Vu" +
           "ZGF0aW9uLm9yZy9VQS9EaWN0aW9uYXJ5L0lSREn/////FWCJCgIAAAABACIAAABQcm9kdWN0QXJ0aWNs" +
           "ZU51bWJlck9mTWFudWZhY3R1cmVyAQERAAAuAEQRAAAAAAz/////AQEBAAAAAQC9RAADAgAUAAAAMDE3" +
           "My0xIzAyLUFBTzY3NiMwMDMAAAAA";

        private const string SerialNumber_InitializationString =
           "AgAAACIAAABodHRwOi8vb3BjZm91bmRhdGlvbi5vcmcvVUEvSTRBQVMvKwAAAGh0dHA6Ly9vcGNmb3Vu" +
           "ZGF0aW9uLm9yZy9VQS9EaWN0aW9uYXJ5L0lSREn/////FWCJCgIAAAABAAwAAABTZXJpYWxOdW1iZXIB" +
           "ARIAAC4ARBIAAAAADP////8BAQEAAAABAL1EAAMCABQAAAAwMTczLTEjMDItQUFNNTU2IzAwMgAAAAA=";

        private const string DateOfManufacture_InitializationString =
           "AgAAACIAAABodHRwOi8vb3BjZm91bmRhdGlvbi5vcmcvVUEvSTRBQVMvKwAAAGh0dHA6Ly9vcGNmb3Vu" +
           "ZGF0aW9uLm9yZy9VQS9EaWN0aW9uYXJ5L0lSREn/////FWCJCgIAAAABABEAAABEYXRlT2ZNYW51ZmFj" +
           "dHVyZQEBFAAALgBEFAAAAAAN/////wEBAQAAAAEAvUQAAwIAFAAAADAxNzMtMSMwMi1BQVI5NzIjMDAy" +
           "AAAAAA==";

        private const string HardwareVersion_InitializationString =
           "AgAAACIAAABodHRwOi8vb3BjZm91bmRhdGlvbi5vcmcvVUEvSTRBQVMvKwAAAGh0dHA6Ly9vcGNmb3Vu" +
           "ZGF0aW9uLm9yZy9VQS9EaWN0aW9uYXJ5L0lSREn/////FWCJCgIAAAABAA8AAABIYXJkd2FyZVZlcnNp" +
           "b24BARUAAC4ARBUAAAABAMde/////wEBAQAAAAEAvUQAAwIAFAAAADAxNzMtMSMwMi1BQU4yNzAjMDAy" +
           "AAAAAA==";

        private const string FirmwareVersion_InitializationString =
           "AgAAACIAAABodHRwOi8vb3BjZm91bmRhdGlvbi5vcmcvVUEvSTRBQVMvKwAAAGh0dHA6Ly9vcGNmb3Vu" +
           "ZGF0aW9uLm9yZy9VQS9EaWN0aW9uYXJ5L0lSREn/////FWCJCgIAAAABAA8AAABGaXJtd2FyZVZlcnNp" +
           "b24BARYAAC4ARBYAAAABAMde/////wEBAQAAAAEAvUQAAwIAFAAAADAxNzMtMSMwMi1BQU05ODUjMDAy" +
           "AAAAAA==";

        private const string SoftwareVersion_InitializationString =
           "AgAAACIAAABodHRwOi8vb3BjZm91bmRhdGlvbi5vcmcvVUEvSTRBQVMvKwAAAGh0dHA6Ly9vcGNmb3Vu" +
           "ZGF0aW9uLm9yZy9VQS9EaWN0aW9uYXJ5L0lSREn/////FWCJCgIAAAABAA8AAABTb2Z0d2FyZVZlcnNp" +
           "b24BARcAAC4ARBcAAAABAMde/////wEBAQAAAAEAvUQAAwIAFAAAADAxNzMtMSMwMi1BQU03MzcjMDAy" +
           "AAAAAA==";

        private const string CountryOfOrigin_InitializationString =
           "AgAAACIAAABodHRwOi8vb3BjZm91bmRhdGlvbi5vcmcvVUEvSTRBQVMvKwAAAGh0dHA6Ly9vcGNmb3Vu" +
           "ZGF0aW9uLm9yZy9VQS9EaWN0aW9uYXJ5L0lSREn/////FWCJCgIAAAABAA8AAABDb3VudHJ5T2ZPcmln" +
           "aW4BARgAAC4ARBgAAAAADP////8BAQEAAAABAL1EAAMCABQAAAAwMTczLTEjMDItQUFPMjU5IzAwNAAA" +
           "AAA=";

        private const string CompanyLogo_InitializationString =
           "AgAAACIAAABodHRwOi8vb3BjZm91bmRhdGlvbi5vcmcvVUEvSTRBQVMvKwAAAGh0dHA6Ly9vcGNmb3Vu" +
           "ZGF0aW9uLm9yZy9VQS9EaWN0aW9uYXJ5L0lSREn/////BGCACgEAAAABAAsAAABDb21wYW55TG9nbwEB" +
           "GQAALwEANy0ZAAAAAQAAAAEAvUQAAwIAPwAAAGh0dHBzOi8vYWRtaW4tc2hlbGwuaW8venZlaS9uYW1l" +
           "cGxhdGUvMi8wL05hbWVwbGF0ZS9Db21wYW55TG9nbwoAAAAVYIkKAgAAAAAABAAAAFNpemUBARoAAC4A" +
           "RBoAAAAACf////8BAf////8AAAAAFWCJCgIAAAAAAAgAAABXcml0YWJsZQEBGwAALgBEGwAAAAAB////" +
           "/wEB/////wAAAAAVYIkKAgAAAAAADAAAAFVzZXJXcml0YWJsZQEBHAAALgBEHAAAAAAB/////wEB////" +
           "/wAAAAAVYIkKAgAAAAAACQAAAE9wZW5Db3VudAEBHQAALgBEHQAAAAAF/////wEB/////wAAAAAEYYIK" +
           "BAAAAAAABAAAAE9wZW4BASEAAC8BADwtIQAAAAEB/////wIAAAAXYKkKAgAAAAAADgAAAElucHV0QXJn" +
           "dW1lbnRzAQEiAAAuAEQiAAAAlgEAAAABACoBARMAAAAEAAAATW9kZQAD/////wAAAAAAAQAoAQEAAAAB" +
           "AAAAAQAAAAEB/////wAAAAAXYKkKAgAAAAAADwAAAE91dHB1dEFyZ3VtZW50cwEBIwAALgBEIwAAAJYB" +
           "AAAAAQAqAQEZAAAACgAAAEZpbGVIYW5kbGUAB/////8AAAAAAAEAKAEBAAAAAQAAAAEAAAABAf////8A" +
           "AAAABGGCCgQAAAAAAAUAAABDbG9zZQEBJAAALwEAPy0kAAAAAQH/////AQAAABdgqQoCAAAAAAAOAAAA" +
           "SW5wdXRBcmd1bWVudHMBASUAAC4ARCUAAACWAQAAAAEAKgEBGQAAAAoAAABGaWxlSGFuZGxlAAf/////" +
           "AAAAAAABACgBAQAAAAEAAAABAAAAAQH/////AAAAAARhggoEAAAAAAAEAAAAUmVhZAEBJgAALwEAQS0m" +
           "AAAAAQH/////AgAAABdgqQoCAAAAAAAOAAAASW5wdXRBcmd1bWVudHMBAScAAC4ARCcAAACWAgAAAAEA" +
           "KgEBGQAAAAoAAABGaWxlSGFuZGxlAAf/////AAAAAAABACoBARUAAAAGAAAATGVuZ3RoAAb/////AAAA" +
           "AAABACgBAQAAAAEAAAACAAAAAQH/////AAAAABdgqQoCAAAAAAAPAAAAT3V0cHV0QXJndW1lbnRzAQEo" +
           "AAAuAEQoAAAAlgEAAAABACoBARMAAAAEAAAARGF0YQAP/////wAAAAAAAQAoAQEAAAABAAAAAQAAAAEB" +
           "/////wAAAAAEYYIKBAAAAAAABQAAAFdyaXRlAQEpAAAvAQBELSkAAAABAf////8BAAAAF2CpCgIAAAAA" +
           "AA4AAABJbnB1dEFyZ3VtZW50cwEBKgAALgBEKgAAAJYCAAAAAQAqAQEZAAAACgAAAEZpbGVIYW5kbGUA" +
           "B/////8AAAAAAAEAKgEBEwAAAAQAAABEYXRhAA//////AAAAAAABACgBAQAAAAEAAAACAAAAAQH/////" +
           "AAAAAARhggoEAAAAAAALAAAAR2V0UG9zaXRpb24BASsAAC8BAEYtKwAAAAEB/////wIAAAAXYKkKAgAA" +
           "AAAADgAAAElucHV0QXJndW1lbnRzAQEsAAAuAEQsAAAAlgEAAAABACoBARkAAAAKAAAARmlsZUhhbmRs" +
           "ZQAH/////wAAAAAAAQAoAQEAAAABAAAAAQAAAAEB/////wAAAAAXYKkKAgAAAAAADwAAAE91dHB1dEFy" +
           "Z3VtZW50cwEBLQAALgBELQAAAJYBAAAAAQAqAQEXAAAACAAAAFBvc2l0aW9uAAn/////AAAAAAABACgB" +
           "AQAAAAEAAAABAAAAAQH/////AAAAAARhggoEAAAAAAALAAAAU2V0UG9zaXRpb24BAS4AAC8BAEktLgAA" +
           "AAEB/////wEAAAAXYKkKAgAAAAAADgAAAElucHV0QXJndW1lbnRzAQEvAAAuAEQvAAAAlgIAAAABACoB" +
           "ARkAAAAKAAAARmlsZUhhbmRsZQAH/////wAAAAAAAQAqAQEXAAAACAAAAFBvc2l0aW9uAAn/////AAAA" +
           "AAABACgBAQAAAAEAAAACAAAAAQH/////AAAAAA==";

        private const string InitializationString =
           "AgAAACIAAABodHRwOi8vb3BjZm91bmRhdGlvbi5vcmcvVUEvSTRBQVMvKwAAAGh0dHA6Ly9vcGNmb3Vu" +
           "ZGF0aW9uLm9yZy9VQS9EaWN0aW9uYXJ5L0lSREn/////BGCAAgEAAAABAB0AAABOYW1lcGxhdGVTdWJt" +
           "b2RlbFR5cGVJbnN0YW5jZQEBBAABAQQABAAAAP////8TAAAAFWCJCgIAAAABAAcAAABNb2RlbElkAQEi" +
           "AQAuAEQiAQAAAAz/////AQH/////AAAAABVgiQoCAAAAAQAMAAAATW9kZWxJZFNob3J0AQEjAQAuAEQj" +
           "AQAAAAz/////AQH/////AAAAABVgiQoCAAAAAQAPAAAAVVJJT2ZUaGVQcm9kdWN0AQEFAAAuAEQFAAAA" +
           "AQDHXP////8BAQEAAAABAL1EAAMCABQAAAAwMTczLTEjMDItQUFZODExIzAwMQAAAAAVYIkKAgAAAAEA" +
           "EAAAAE1hbnVmYWN0dXJlck5hbWUBAQYAAC4ARAYAAAAAFf////8BAQEAAAABAL1EAAMCABQAAAAwMTcz" +
           "LTEjMDItQUFPNjc3IzAwMgAAAAAVYIkKAgAAAAEAHgAAAE1hbnVmYWN0dXJlclByb2R1Y3REZXNpZ25h" +
           "dGlvbgEBBwAALgBEBwAAAAAV/////wEBAQAAAAEAvUQAAwIAFAAAADAxNzMtMSMwMi1BQVczMzgjMDAx" +
           "AAAAAARggAoBAAAAAQASAAAAQ29udGFjdEluZm9ybWF0aW9uAQEIAAAvAQFOAAgAAAABAAAAAQC9RAAD" +
           "AgBQAAAAaHR0cHM6Ly9hZG1pbi1zaGVsbC5pby96dmVpL25hbWVwbGF0ZS8xLzAvQ29udGFjdEluZm9y" +
           "bWF0aW9ucy9Db250YWN0SW5mb3JtYXRpb24GAAAAFWCJCgIAAAABAAcAAABNb2RlbElkAQElAQAuAEQl" +
           "AQAAAAz/////AQH/////AAAAABVgiQoCAAAAAQAMAAAATW9kZWxJZFNob3J0AQEmAQAuAEQmAQAAAAz/" +
           "////AQH/////AAAAABVgiQoCAAAAAQAGAAAAU3RyZWV0AQEJAAAuAEQJAAAAABX/////AQEBAAAAAQC9" +
           "RAADAgAUAAAAMDE3My0xIzAyLUFBTzEyOCMwMDIAAAAAFWCJCgIAAAABAAcAAABaaXBjb2RlAQEKAAAu" +
           "AEQKAAAAAAz/////AQEBAAAAAQC9RAADAgAUAAAAMDE3My0xIzAyLUFBTzEyOSMwMDIAAAAAFWCJCgIA" +
           "AAABAAgAAABDaXR5VG93bgEBCwAALgBECwAAAAAV/////wEBAQAAAAEAvUQAAwIAFAAAADAxNzMtMSMw" +
           "Mi1BQU8xMzIjMDAyAAAAABVgiQoCAAAAAQAMAAAATmF0aW9uYWxDb2RlAQEMAAAuAEQMAAAAAAz/////" +
           "AQEBAAAAAQC9RAADAgAUAAAAMDE3My0xIzAyLUFBTzEzNCMwMDIAAAAAFWCJCgIAAAABABcAAABNYW51" +
           "ZmFjdHVyZXJQcm9kdWN0Um9vdAEBDQAALgBEDQAAAAAV/////wEBAQAAAAEAvUQAAwIAFAAAADAxNzMt" +
           "MSMwMi1BQVU3MzIjMDAxAAAAABVgiQoCAAAAAQAZAAAATWFudWZhY3R1cmVyUHJvZHVjdEZhbWlseQEB" +
           "DgAALgBEDgAAAAAV/////wEBAQAAAAEAvUQAAwIAFAAAADAxNzMtMSMwMi1BQVU3MzEjMDAxAAAAABVg" +
           "iQoCAAAAAQAXAAAATWFudWZhY3R1cmVyUHJvZHVjdFR5cGUBAQ8AAC4ARA8AAAAAFf////8BAQEAAAAB" +
           "AL1EAAMCABQAAAAwMTczLTEjMDItQUFPMDU3IzAwMgAAAAAVYIkKAgAAAAEAFwAAAE9yZGVyQ29kZU9m" +
           "TWFudWZhY3R1cmVyAQEQAAAuAEQQAAAAAAz/////AQEBAAAAAQC9RAADAgAUAAAAMDE3My0xIzAyLUFB" +
           "TzIyNyMwMDIAAAAAFWCJCgIAAAABACIAAABQcm9kdWN0QXJ0aWNsZU51bWJlck9mTWFudWZhY3R1cmVy" +
           "AQERAAAuAEQRAAAAAAz/////AQEBAAAAAQC9RAADAgAUAAAAMDE3My0xIzAyLUFBTzY3NiMwMDMAAAAA" +
           "FWCJCgIAAAABAAwAAABTZXJpYWxOdW1iZXIBARIAAC4ARBIAAAAADP////8BAQEAAAABAL1EAAMCABQA" +
           "AAAwMTczLTEjMDItQUFNNTU2IzAwMgAAAAAVYIkKAgAAAAEAEgAAAFllYXJPZkNvbnN0cnVjdGlvbgEB" +
           "EwAALgBEEwAAAAAM/////wEBAQAAAAEAvUQAAwIAFAAAADAxNzMtMSMwMi1BQVA5MDYjMDAxAAAAABVg" +
           "iQoCAAAAAQARAAAARGF0ZU9mTWFudWZhY3R1cmUBARQAAC4ARBQAAAAADf////8BAQEAAAABAL1EAAMC" +
           "ABQAAAAwMTczLTEjMDItQUFSOTcyIzAwMgAAAAAVYIkKAgAAAAEADwAAAEhhcmR3YXJlVmVyc2lvbgEB" +
           "FQAALgBEFQAAAAEAx17/////AQEBAAAAAQC9RAADAgAUAAAAMDE3My0xIzAyLUFBTjI3MCMwMDIAAAAA" +
           "FWCJCgIAAAABAA8AAABGaXJtd2FyZVZlcnNpb24BARYAAC4ARBYAAAABAMde/////wEBAQAAAAEAvUQA" +
           "AwIAFAAAADAxNzMtMSMwMi1BQU05ODUjMDAyAAAAABVgiQoCAAAAAQAPAAAAU29mdHdhcmVWZXJzaW9u" +
           "AQEXAAAuAEQXAAAAAQDHXv////8BAQEAAAABAL1EAAMCABQAAAAwMTczLTEjMDItQUFNNzM3IzAwMgAA" +
           "AAAVYIkKAgAAAAEADwAAAENvdW50cnlPZk9yaWdpbgEBGAAALgBEGAAAAAAM/////wEBAQAAAAEAvUQA" +
           "AwIAFAAAADAxNzMtMSMwMi1BQU8yNTkjMDA0AAAAAARggAoBAAAAAQALAAAAQ29tcGFueUxvZ28BARkA" +
           "AC8BADctGQAAAAEAAAABAL1EAAMCAD8AAABodHRwczovL2FkbWluLXNoZWxsLmlvL3p2ZWkvbmFtZXBs" +
           "YXRlLzIvMC9OYW1lcGxhdGUvQ29tcGFueUxvZ28KAAAAFWCJCgIAAAAAAAQAAABTaXplAQEaAAAuAEQa" +
           "AAAAAAn/////AQH/////AAAAABVgiQoCAAAAAAAIAAAAV3JpdGFibGUBARsAAC4ARBsAAAAAAf////8B" +
           "Af////8AAAAAFWCJCgIAAAAAAAwAAABVc2VyV3JpdGFibGUBARwAAC4ARBwAAAAAAf////8BAf////8A" +
           "AAAAFWCJCgIAAAAAAAkAAABPcGVuQ291bnQBAR0AAC4ARB0AAAAABf////8BAf////8AAAAABGGCCgQA" +
           "AAAAAAQAAABPcGVuAQEhAAAvAQA8LSEAAAABAf////8CAAAAF2CpCgIAAAAAAA4AAABJbnB1dEFyZ3Vt" +
           "ZW50cwEBIgAALgBEIgAAAJYBAAAAAQAqAQETAAAABAAAAE1vZGUAA/////8AAAAAAAEAKAEBAAAAAQAA" +
           "AAEAAAABAf////8AAAAAF2CpCgIAAAAAAA8AAABPdXRwdXRBcmd1bWVudHMBASMAAC4ARCMAAACWAQAA" +
           "AAEAKgEBGQAAAAoAAABGaWxlSGFuZGxlAAf/////AAAAAAABACgBAQAAAAEAAAABAAAAAQH/////AAAA" +
           "AARhggoEAAAAAAAFAAAAQ2xvc2UBASQAAC8BAD8tJAAAAAEB/////wEAAAAXYKkKAgAAAAAADgAAAElu" +
           "cHV0QXJndW1lbnRzAQElAAAuAEQlAAAAlgEAAAABACoBARkAAAAKAAAARmlsZUhhbmRsZQAH/////wAA" +
           "AAAAAQAoAQEAAAABAAAAAQAAAAEB/////wAAAAAEYYIKBAAAAAAABAAAAFJlYWQBASYAAC8BAEEtJgAA" +
           "AAEB/////wIAAAAXYKkKAgAAAAAADgAAAElucHV0QXJndW1lbnRzAQEnAAAuAEQnAAAAlgIAAAABACoB" +
           "ARkAAAAKAAAARmlsZUhhbmRsZQAH/////wAAAAAAAQAqAQEVAAAABgAAAExlbmd0aAAG/////wAAAAAA" +
           "AQAoAQEAAAABAAAAAgAAAAEB/////wAAAAAXYKkKAgAAAAAADwAAAE91dHB1dEFyZ3VtZW50cwEBKAAA" +
           "LgBEKAAAAJYBAAAAAQAqAQETAAAABAAAAERhdGEAD/////8AAAAAAAEAKAEBAAAAAQAAAAEAAAABAf//" +
           "//8AAAAABGGCCgQAAAAAAAUAAABXcml0ZQEBKQAALwEARC0pAAAAAQH/////AQAAABdgqQoCAAAAAAAO" +
           "AAAASW5wdXRBcmd1bWVudHMBASoAAC4ARCoAAACWAgAAAAEAKgEBGQAAAAoAAABGaWxlSGFuZGxlAAf/" +
           "////AAAAAAABACoBARMAAAAEAAAARGF0YQAP/////wAAAAAAAQAoAQEAAAABAAAAAgAAAAEB/////wAA" +
           "AAAEYYIKBAAAAAAACwAAAEdldFBvc2l0aW9uAQErAAAvAQBGLSsAAAABAf////8CAAAAF2CpCgIAAAAA" +
           "AA4AAABJbnB1dEFyZ3VtZW50cwEBLAAALgBELAAAAJYBAAAAAQAqAQEZAAAACgAAAEZpbGVIYW5kbGUA" +
           "B/////8AAAAAAAEAKAEBAAAAAQAAAAEAAAABAf////8AAAAAF2CpCgIAAAAAAA8AAABPdXRwdXRBcmd1" +
           "bWVudHMBAS0AAC4ARC0AAACWAQAAAAEAKgEBFwAAAAgAAABQb3NpdGlvbgAJ/////wAAAAAAAQAoAQEA" +
           "AAABAAAAAQAAAAEB/////wAAAAAEYYIKBAAAAAAACwAAAFNldFBvc2l0aW9uAQEuAAAvAQBJLS4AAAAB" +
           "Af////8BAAAAF2CpCgIAAAAAAA4AAABJbnB1dEFyZ3VtZW50cwEBLwAALgBELwAAAJYCAAAAAQAqAQEZ" +
           "AAAACgAAAEZpbGVIYW5kbGUAB/////8AAAAAAAEAKgEBFwAAAAgAAABQb3NpdGlvbgAJ/////wAAAAAA" +
           "AQAoAQEAAAABAAAAAgAAAAEB/////wAAAAA=";
        #endregion
        #endif
        #endregion

        #region Public Properties
        /// <remarks />
        public PropertyState<string> URIOfTheProduct
        {
            get
            {
                return m_uRIOfTheProduct;
            }

            set
            {
                if (!Object.ReferenceEquals(m_uRIOfTheProduct, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_uRIOfTheProduct = value;
            }
        }

        /// <remarks />
        public PropertyState<LocalizedText> ManufacturerName
        {
            get
            {
                return m_manufacturerName;
            }

            set
            {
                if (!Object.ReferenceEquals(m_manufacturerName, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_manufacturerName = value;
            }
        }

        /// <remarks />
        public PropertyState<LocalizedText> ManufacturerProductDesignation
        {
            get
            {
                return m_manufacturerProductDesignation;
            }

            set
            {
                if (!Object.ReferenceEquals(m_manufacturerProductDesignation, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_manufacturerProductDesignation = value;
            }
        }

        /// <remarks />
        public ContactInformationSubmodelState ContactInformation
        {
            get
            {
                return m_contactInformation;
            }

            set
            {
                if (!Object.ReferenceEquals(m_contactInformation, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_contactInformation = value;
            }
        }

        /// <remarks />
        public PropertyState<LocalizedText> ManufacturerProductRoot
        {
            get
            {
                return m_manufacturerProductRoot;
            }

            set
            {
                if (!Object.ReferenceEquals(m_manufacturerProductRoot, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_manufacturerProductRoot = value;
            }
        }

        /// <remarks />
        public PropertyState<LocalizedText> ManufacturerProductFamily
        {
            get
            {
                return m_manufacturerProductFamily;
            }

            set
            {
                if (!Object.ReferenceEquals(m_manufacturerProductFamily, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_manufacturerProductFamily = value;
            }
        }

        /// <remarks />
        public PropertyState<LocalizedText> ManufacturerProductType
        {
            get
            {
                return m_manufacturerProductType;
            }

            set
            {
                if (!Object.ReferenceEquals(m_manufacturerProductType, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_manufacturerProductType = value;
            }
        }

        /// <remarks />
        public PropertyState<string> OrderCodeOfManufacturer
        {
            get
            {
                return m_orderCodeOfManufacturer;
            }

            set
            {
                if (!Object.ReferenceEquals(m_orderCodeOfManufacturer, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_orderCodeOfManufacturer = value;
            }
        }

        /// <remarks />
        public PropertyState<string> ProductArticleNumberOfManufacturer
        {
            get
            {
                return m_productArticleNumberOfManufacturer;
            }

            set
            {
                if (!Object.ReferenceEquals(m_productArticleNumberOfManufacturer, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_productArticleNumberOfManufacturer = value;
            }
        }

        /// <remarks />
        public PropertyState<string> SerialNumber
        {
            get
            {
                return m_serialNumber;
            }

            set
            {
                if (!Object.ReferenceEquals(m_serialNumber, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_serialNumber = value;
            }
        }

        /// <remarks />
        public PropertyState<string> YearOfConstruction
        {
            get
            {
                return m_yearOfConstruction;
            }

            set
            {
                if (!Object.ReferenceEquals(m_yearOfConstruction, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_yearOfConstruction = value;
            }
        }

        /// <remarks />
        public PropertyState<DateTime> DateOfManufacture
        {
            get
            {
                return m_dateOfManufacture;
            }

            set
            {
                if (!Object.ReferenceEquals(m_dateOfManufacture, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_dateOfManufacture = value;
            }
        }

        /// <remarks />
        public PropertyState<string> HardwareVersion
        {
            get
            {
                return m_hardwareVersion;
            }

            set
            {
                if (!Object.ReferenceEquals(m_hardwareVersion, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_hardwareVersion = value;
            }
        }

        /// <remarks />
        public PropertyState<string> FirmwareVersion
        {
            get
            {
                return m_firmwareVersion;
            }

            set
            {
                if (!Object.ReferenceEquals(m_firmwareVersion, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_firmwareVersion = value;
            }
        }

        /// <remarks />
        public PropertyState<string> SoftwareVersion
        {
            get
            {
                return m_softwareVersion;
            }

            set
            {
                if (!Object.ReferenceEquals(m_softwareVersion, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_softwareVersion = value;
            }
        }

        /// <remarks />
        public PropertyState<string> CountryOfOrigin
        {
            get
            {
                return m_countryOfOrigin;
            }

            set
            {
                if (!Object.ReferenceEquals(m_countryOfOrigin, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_countryOfOrigin = value;
            }
        }

        /// <remarks />
        public FileState CompanyLogo
        {
            get
            {
                return m_companyLogo;
            }

            set
            {
                if (!Object.ReferenceEquals(m_companyLogo, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_companyLogo = value;
            }
        }
        #endregion

        #region Overridden Methods
        /// <remarks />
        public override void GetChildren(
            ISystemContext context,
            IList<BaseInstanceState> children)
        {
            if (m_uRIOfTheProduct != null)
            {
                children.Add(m_uRIOfTheProduct);
            }

            if (m_manufacturerName != null)
            {
                children.Add(m_manufacturerName);
            }

            if (m_manufacturerProductDesignation != null)
            {
                children.Add(m_manufacturerProductDesignation);
            }

            if (m_contactInformation != null)
            {
                children.Add(m_contactInformation);
            }

            if (m_manufacturerProductRoot != null)
            {
                children.Add(m_manufacturerProductRoot);
            }

            if (m_manufacturerProductFamily != null)
            {
                children.Add(m_manufacturerProductFamily);
            }

            if (m_manufacturerProductType != null)
            {
                children.Add(m_manufacturerProductType);
            }

            if (m_orderCodeOfManufacturer != null)
            {
                children.Add(m_orderCodeOfManufacturer);
            }

            if (m_productArticleNumberOfManufacturer != null)
            {
                children.Add(m_productArticleNumberOfManufacturer);
            }

            if (m_serialNumber != null)
            {
                children.Add(m_serialNumber);
            }

            if (m_yearOfConstruction != null)
            {
                children.Add(m_yearOfConstruction);
            }

            if (m_dateOfManufacture != null)
            {
                children.Add(m_dateOfManufacture);
            }

            if (m_hardwareVersion != null)
            {
                children.Add(m_hardwareVersion);
            }

            if (m_firmwareVersion != null)
            {
                children.Add(m_firmwareVersion);
            }

            if (m_softwareVersion != null)
            {
                children.Add(m_softwareVersion);
            }

            if (m_countryOfOrigin != null)
            {
                children.Add(m_countryOfOrigin);
            }

            if (m_companyLogo != null)
            {
                children.Add(m_companyLogo);
            }

            base.GetChildren(context, children);
        }

        /// <remarks />
        protected override BaseInstanceState FindChild(
            ISystemContext context,
            QualifiedName browseName,
            bool createOrReplace,
            BaseInstanceState replacement)
        {
            if (QualifiedName.IsNull(browseName))
            {
                return null;
            }

            BaseInstanceState instance = null;

            switch (browseName.Name)
            {
                case I4AAS.Submodels.BrowseNames.URIOfTheProduct:
                {
                    if (createOrReplace)
                    {
                        if (URIOfTheProduct == null)
                        {
                            if (replacement == null)
                            {
                                URIOfTheProduct = new PropertyState<string>(this);
                            }
                            else
                            {
                                URIOfTheProduct = (PropertyState<string>)replacement;
                            }
                        }
                    }

                    instance = URIOfTheProduct;
                    break;
                }

                case I4AAS.Submodels.BrowseNames.ManufacturerName:
                {
                    if (createOrReplace)
                    {
                        if (ManufacturerName == null)
                        {
                            if (replacement == null)
                            {
                                ManufacturerName = new PropertyState<LocalizedText>(this);
                            }
                            else
                            {
                                ManufacturerName = (PropertyState<LocalizedText>)replacement;
                            }
                        }
                    }

                    instance = ManufacturerName;
                    break;
                }

                case I4AAS.Submodels.BrowseNames.ManufacturerProductDesignation:
                {
                    if (createOrReplace)
                    {
                        if (ManufacturerProductDesignation == null)
                        {
                            if (replacement == null)
                            {
                                ManufacturerProductDesignation = new PropertyState<LocalizedText>(this);
                            }
                            else
                            {
                                ManufacturerProductDesignation = (PropertyState<LocalizedText>)replacement;
                            }
                        }
                    }

                    instance = ManufacturerProductDesignation;
                    break;
                }

                case I4AAS.Submodels.BrowseNames.ContactInformation:
                {
                    if (createOrReplace)
                    {
                        if (ContactInformation == null)
                        {
                            if (replacement == null)
                            {
                                ContactInformation = new ContactInformationSubmodelState(this);
                            }
                            else
                            {
                                ContactInformation = (ContactInformationSubmodelState)replacement;
                            }
                        }
                    }

                    instance = ContactInformation;
                    break;
                }

                case I4AAS.Submodels.BrowseNames.ManufacturerProductRoot:
                {
                    if (createOrReplace)
                    {
                        if (ManufacturerProductRoot == null)
                        {
                            if (replacement == null)
                            {
                                ManufacturerProductRoot = new PropertyState<LocalizedText>(this);
                            }
                            else
                            {
                                ManufacturerProductRoot = (PropertyState<LocalizedText>)replacement;
                            }
                        }
                    }

                    instance = ManufacturerProductRoot;
                    break;
                }

                case I4AAS.Submodels.BrowseNames.ManufacturerProductFamily:
                {
                    if (createOrReplace)
                    {
                        if (ManufacturerProductFamily == null)
                        {
                            if (replacement == null)
                            {
                                ManufacturerProductFamily = new PropertyState<LocalizedText>(this);
                            }
                            else
                            {
                                ManufacturerProductFamily = (PropertyState<LocalizedText>)replacement;
                            }
                        }
                    }

                    instance = ManufacturerProductFamily;
                    break;
                }

                case I4AAS.Submodels.BrowseNames.ManufacturerProductType:
                {
                    if (createOrReplace)
                    {
                        if (ManufacturerProductType == null)
                        {
                            if (replacement == null)
                            {
                                ManufacturerProductType = new PropertyState<LocalizedText>(this);
                            }
                            else
                            {
                                ManufacturerProductType = (PropertyState<LocalizedText>)replacement;
                            }
                        }
                    }

                    instance = ManufacturerProductType;
                    break;
                }

                case I4AAS.Submodels.BrowseNames.OrderCodeOfManufacturer:
                {
                    if (createOrReplace)
                    {
                        if (OrderCodeOfManufacturer == null)
                        {
                            if (replacement == null)
                            {
                                OrderCodeOfManufacturer = new PropertyState<string>(this);
                            }
                            else
                            {
                                OrderCodeOfManufacturer = (PropertyState<string>)replacement;
                            }
                        }
                    }

                    instance = OrderCodeOfManufacturer;
                    break;
                }

                case I4AAS.Submodels.BrowseNames.ProductArticleNumberOfManufacturer:
                {
                    if (createOrReplace)
                    {
                        if (ProductArticleNumberOfManufacturer == null)
                        {
                            if (replacement == null)
                            {
                                ProductArticleNumberOfManufacturer = new PropertyState<string>(this);
                            }
                            else
                            {
                                ProductArticleNumberOfManufacturer = (PropertyState<string>)replacement;
                            }
                        }
                    }

                    instance = ProductArticleNumberOfManufacturer;
                    break;
                }

                case I4AAS.Submodels.BrowseNames.SerialNumber:
                {
                    if (createOrReplace)
                    {
                        if (SerialNumber == null)
                        {
                            if (replacement == null)
                            {
                                SerialNumber = new PropertyState<string>(this);
                            }
                            else
                            {
                                SerialNumber = (PropertyState<string>)replacement;
                            }
                        }
                    }

                    instance = SerialNumber;
                    break;
                }

                case I4AAS.Submodels.BrowseNames.YearOfConstruction:
                {
                    if (createOrReplace)
                    {
                        if (YearOfConstruction == null)
                        {
                            if (replacement == null)
                            {
                                YearOfConstruction = new PropertyState<string>(this);
                            }
                            else
                            {
                                YearOfConstruction = (PropertyState<string>)replacement;
                            }
                        }
                    }

                    instance = YearOfConstruction;
                    break;
                }

                case I4AAS.Submodels.BrowseNames.DateOfManufacture:
                {
                    if (createOrReplace)
                    {
                        if (DateOfManufacture == null)
                        {
                            if (replacement == null)
                            {
                                DateOfManufacture = new PropertyState<DateTime>(this);
                            }
                            else
                            {
                                DateOfManufacture = (PropertyState<DateTime>)replacement;
                            }
                        }
                    }

                    instance = DateOfManufacture;
                    break;
                }

                case I4AAS.Submodels.BrowseNames.HardwareVersion:
                {
                    if (createOrReplace)
                    {
                        if (HardwareVersion == null)
                        {
                            if (replacement == null)
                            {
                                HardwareVersion = new PropertyState<string>(this);
                            }
                            else
                            {
                                HardwareVersion = (PropertyState<string>)replacement;
                            }
                        }
                    }

                    instance = HardwareVersion;
                    break;
                }

                case I4AAS.Submodels.BrowseNames.FirmwareVersion:
                {
                    if (createOrReplace)
                    {
                        if (FirmwareVersion == null)
                        {
                            if (replacement == null)
                            {
                                FirmwareVersion = new PropertyState<string>(this);
                            }
                            else
                            {
                                FirmwareVersion = (PropertyState<string>)replacement;
                            }
                        }
                    }

                    instance = FirmwareVersion;
                    break;
                }

                case I4AAS.Submodels.BrowseNames.SoftwareVersion:
                {
                    if (createOrReplace)
                    {
                        if (SoftwareVersion == null)
                        {
                            if (replacement == null)
                            {
                                SoftwareVersion = new PropertyState<string>(this);
                            }
                            else
                            {
                                SoftwareVersion = (PropertyState<string>)replacement;
                            }
                        }
                    }

                    instance = SoftwareVersion;
                    break;
                }

                case I4AAS.Submodels.BrowseNames.CountryOfOrigin:
                {
                    if (createOrReplace)
                    {
                        if (CountryOfOrigin == null)
                        {
                            if (replacement == null)
                            {
                                CountryOfOrigin = new PropertyState<string>(this);
                            }
                            else
                            {
                                CountryOfOrigin = (PropertyState<string>)replacement;
                            }
                        }
                    }

                    instance = CountryOfOrigin;
                    break;
                }

                case I4AAS.Submodels.BrowseNames.CompanyLogo:
                {
                    if (createOrReplace)
                    {
                        if (CompanyLogo == null)
                        {
                            if (replacement == null)
                            {
                                CompanyLogo = new FileState(this);
                            }
                            else
                            {
                                CompanyLogo = (FileState)replacement;
                            }
                        }
                    }

                    instance = CompanyLogo;
                    break;
                }
            }

            if (instance != null)
            {
                return instance;
            }

            return base.FindChild(context, browseName, createOrReplace, replacement);
        }
        #endregion

        #region Private Fields
        private PropertyState<string> m_uRIOfTheProduct;
        private PropertyState<LocalizedText> m_manufacturerName;
        private PropertyState<LocalizedText> m_manufacturerProductDesignation;
        private ContactInformationSubmodelState m_contactInformation;
        private PropertyState<LocalizedText> m_manufacturerProductRoot;
        private PropertyState<LocalizedText> m_manufacturerProductFamily;
        private PropertyState<LocalizedText> m_manufacturerProductType;
        private PropertyState<string> m_orderCodeOfManufacturer;
        private PropertyState<string> m_productArticleNumberOfManufacturer;
        private PropertyState<string> m_serialNumber;
        private PropertyState<string> m_yearOfConstruction;
        private PropertyState<DateTime> m_dateOfManufacture;
        private PropertyState<string> m_hardwareVersion;
        private PropertyState<string> m_firmwareVersion;
        private PropertyState<string> m_softwareVersion;
        private PropertyState<string> m_countryOfOrigin;
        private FileState m_companyLogo;
        #endregion
    }
    #endif
    #endregion

    #region ContactInformationSubmodelState Class
    #if (!OPCUA_EXCLUDE_ContactInformationSubmodelState)
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public partial class ContactInformationSubmodelState : SubmodelState
    {
        #region Constructors
        /// <remarks />
        public ContactInformationSubmodelState(NodeState parent) : base(parent)
        {
        }

        /// <remarks />
        protected override NodeId GetDefaultTypeDefinitionId(NamespaceTable namespaceUris)
        {
            return Opc.Ua.NodeId.Create(I4AAS.Submodels.ObjectTypes.ContactInformationSubmodelType, I4AAS.Submodels.Namespaces.I4AAS, namespaceUris);
        }

        #if (!OPCUA_EXCLUDE_InitializationStrings)
        /// <remarks />
        protected override void Initialize(ISystemContext context)
        {
            base.Initialize(context);
            Initialize(context, InitializationString);
            InitializeOptionalChildren(context);
        }

        /// <remarks />
        protected override void Initialize(ISystemContext context, NodeState source)
        {
            InitializeOptionalChildren(context);
            base.Initialize(context, source);
        }

        /// <remarks />
        protected override void InitializeOptionalChildren(ISystemContext context)
        {
            base.InitializeOptionalChildren(context);
        }

        #region Initialization String
        private const string InitializationString =
           "AgAAACIAAABodHRwOi8vb3BjZm91bmRhdGlvbi5vcmcvVUEvSTRBQVMvKwAAAGh0dHA6Ly9vcGNmb3Vu" +
           "ZGF0aW9uLm9yZy9VQS9EaWN0aW9uYXJ5L0lSREn/////BGCAAgEAAAABACYAAABDb250YWN0SW5mb3Jt" +
           "YXRpb25TdWJtb2RlbFR5cGVJbnN0YW5jZQEBTgABAU4ATgAAAP////8GAAAAFWCJCgIAAAABAAcAAABN" +
           "b2RlbElkAQErAQAuAEQrAQAAAAz/////AQH/////AAAAABVgiQoCAAAAAQAMAAAATW9kZWxJZFNob3J0" +
           "AQEsAQAuAEQsAQAAAAz/////AQH/////AAAAABVgiQoCAAAAAQAGAAAAU3RyZWV0AQFPAAAuAERPAAAA" +
           "ABX/////AQEBAAAAAQC9RAADAgAUAAAAMDE3My0xIzAyLUFBTzEyOCMwMDIAAAAAFWCJCgIAAAABAAcA" +
           "AABaaXBjb2RlAQFQAAAuAERQAAAAAAz/////AQEBAAAAAQC9RAADAgAUAAAAMDE3My0xIzAyLUFBTzEy" +
           "OSMwMDIAAAAAFWCJCgIAAAABAAgAAABDaXR5VG93bgEBUQAALgBEUQAAAAAV/////wEBAQAAAAEAvUQA" +
           "AwIAFAAAADAxNzMtMSMwMi1BQU8xMzIjMDAyAAAAABVgiQoCAAAAAQAMAAAATmF0aW9uYWxDb2RlAQFS" +
           "AAAuAERSAAAAAAz/////AQEBAAAAAQC9RAADAgAUAAAAMDE3My0xIzAyLUFBTzEzNCMwMDIAAAAA";
        #endregion
        #endif
        #endregion

        #region Public Properties
        /// <remarks />
        public PropertyState<LocalizedText> Street
        {
            get
            {
                return m_street;
            }

            set
            {
                if (!Object.ReferenceEquals(m_street, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_street = value;
            }
        }

        /// <remarks />
        public PropertyState<string> Zipcode
        {
            get
            {
                return m_zipcode;
            }

            set
            {
                if (!Object.ReferenceEquals(m_zipcode, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_zipcode = value;
            }
        }

        /// <remarks />
        public PropertyState<LocalizedText> CityTown
        {
            get
            {
                return m_cityTown;
            }

            set
            {
                if (!Object.ReferenceEquals(m_cityTown, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_cityTown = value;
            }
        }

        /// <remarks />
        public PropertyState<string> NationalCode
        {
            get
            {
                return m_nationalCode;
            }

            set
            {
                if (!Object.ReferenceEquals(m_nationalCode, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_nationalCode = value;
            }
        }
        #endregion

        #region Overridden Methods
        /// <remarks />
        public override void GetChildren(
            ISystemContext context,
            IList<BaseInstanceState> children)
        {
            if (m_street != null)
            {
                children.Add(m_street);
            }

            if (m_zipcode != null)
            {
                children.Add(m_zipcode);
            }

            if (m_cityTown != null)
            {
                children.Add(m_cityTown);
            }

            if (m_nationalCode != null)
            {
                children.Add(m_nationalCode);
            }

            base.GetChildren(context, children);
        }

        /// <remarks />
        protected override BaseInstanceState FindChild(
            ISystemContext context,
            QualifiedName browseName,
            bool createOrReplace,
            BaseInstanceState replacement)
        {
            if (QualifiedName.IsNull(browseName))
            {
                return null;
            }

            BaseInstanceState instance = null;

            switch (browseName.Name)
            {
                case I4AAS.Submodels.BrowseNames.Street:
                {
                    if (createOrReplace)
                    {
                        if (Street == null)
                        {
                            if (replacement == null)
                            {
                                Street = new PropertyState<LocalizedText>(this);
                            }
                            else
                            {
                                Street = (PropertyState<LocalizedText>)replacement;
                            }
                        }
                    }

                    instance = Street;
                    break;
                }

                case I4AAS.Submodels.BrowseNames.Zipcode:
                {
                    if (createOrReplace)
                    {
                        if (Zipcode == null)
                        {
                            if (replacement == null)
                            {
                                Zipcode = new PropertyState<string>(this);
                            }
                            else
                            {
                                Zipcode = (PropertyState<string>)replacement;
                            }
                        }
                    }

                    instance = Zipcode;
                    break;
                }

                case I4AAS.Submodels.BrowseNames.CityTown:
                {
                    if (createOrReplace)
                    {
                        if (CityTown == null)
                        {
                            if (replacement == null)
                            {
                                CityTown = new PropertyState<LocalizedText>(this);
                            }
                            else
                            {
                                CityTown = (PropertyState<LocalizedText>)replacement;
                            }
                        }
                    }

                    instance = CityTown;
                    break;
                }

                case I4AAS.Submodels.BrowseNames.NationalCode:
                {
                    if (createOrReplace)
                    {
                        if (NationalCode == null)
                        {
                            if (replacement == null)
                            {
                                NationalCode = new PropertyState<string>(this);
                            }
                            else
                            {
                                NationalCode = (PropertyState<string>)replacement;
                            }
                        }
                    }

                    instance = NationalCode;
                    break;
                }
            }

            if (instance != null)
            {
                return instance;
            }

            return base.FindChild(context, browseName, createOrReplace, replacement);
        }
        #endregion

        #region Private Fields
        private PropertyState<LocalizedText> m_street;
        private PropertyState<string> m_zipcode;
        private PropertyState<LocalizedText> m_cityTown;
        private PropertyState<string> m_nationalCode;
        #endregion
    }
    #endif
    #endregion

    #region MarkingSubmodelState Class
    #if (!OPCUA_EXCLUDE_MarkingSubmodelState)
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public partial class MarkingSubmodelState : SubmodelState
    {
        #region Constructors
        /// <remarks />
        public MarkingSubmodelState(NodeState parent) : base(parent)
        {
        }

        /// <remarks />
        protected override NodeId GetDefaultTypeDefinitionId(NamespaceTable namespaceUris)
        {
            return Opc.Ua.NodeId.Create(I4AAS.Submodels.ObjectTypes.MarkingSubmodelType, I4AAS.Submodels.Namespaces.I4AAS, namespaceUris);
        }

        #if (!OPCUA_EXCLUDE_InitializationStrings)
        /// <remarks />
        protected override void Initialize(ISystemContext context)
        {
            base.Initialize(context);
            Initialize(context, InitializationString);
            InitializeOptionalChildren(context);
        }

        /// <remarks />
        protected override void Initialize(ISystemContext context, NodeState source)
        {
            InitializeOptionalChildren(context);
            base.Initialize(context, source);
        }

        /// <remarks />
        protected override void InitializeOptionalChildren(ISystemContext context)
        {
            base.InitializeOptionalChildren(context);
        }

        #region Initialization String
        private const string InitializationString =
           "AgAAACIAAABodHRwOi8vb3BjZm91bmRhdGlvbi5vcmcvVUEvSTRBQVMvKwAAAGh0dHA6Ly9vcGNmb3Vu" +
           "ZGF0aW9uLm9yZy9VQS9EaWN0aW9uYXJ5L0lSREn/////BGCAAgEAAAABABsAAABNYXJraW5nU3VibW9k" +
           "ZWxUeXBlSW5zdGFuY2UBAVQAAQFUAFQAAAD/////CAAAABVgiQoCAAAAAQAHAAAATW9kZWxJZAEBLgEA" +
           "LgBELgEAAAAM/////wEB/////wAAAAAVYIkKAgAAAAEADAAAAE1vZGVsSWRTaG9ydAEBLwEALgBELwEA" +
           "AAAM/////wEB/////wAAAAAVYIkKAgAAAAEACwAAAE1hcmtpbmdOYW1lAQFVAAAuAERVAAAAAAz/////" +
           "AQEBAAAAAQC9RAADAgBQAAAAaHR0cHM6Ly9hZG1pbi1zaGVsbC5pby96dmVpL25hbWVwbGF0ZS8yLzAv" +
           "TmFtZXBsYXRlL01hcmtpbmdzL01hcmtpbmcvTWFya2luZ05hbWUAAAAAFWCJCgIAAAABACIAAABEZXNp" +
           "Z25hdGlvbk9mQ2VydGlmaWNhdGVPckFwcHJvdmFsAQFWAAAuAERWAAAAAAz/////AQEBAAAAAQC9RAAD" +
           "AgAZAAAAMDExMi8yLy8vNjE5ODcjQUJINzgzIzAwMQAAAAAVYIkKAgAAAAEACQAAAElzc3VlRGF0ZQEB" +
           "VwAALgBEVwAAAAAN/////wEBAQAAAAEAvUQAAwIATgAAAGh0dHBzOi8vYWRtaW4tc2hlbGwuaW8venZl" +
           "aS9uYW1lcGxhdGUvMi8wL05hbWVwbGF0ZS9NYXJraW5ncy9NYXJraW5nL0lzc3VlRGF0ZQAAAAAVYIkK" +
           "AgAAAAEACgAAAEV4cGlyeURhdGUBAVgAAC4ARFgAAAAADf////8BAQEAAAABAL1EAAMCAE8AAABodHRw" +
           "czovL2FkbWluLXNoZWxsLmlvL3p2ZWkvbmFtZXBsYXRlLzIvMC9OYW1lcGxhdGUvTWFya2luZ3MvTWFy" +
           "a2luZy9FeHBpcnlEYXRlAAAAAARggAoBAAAAAQALAAAATWFya2luZ0ZpbGUBAVkAAC8BADctWQAAAAEA" +
           "AAABAL1EAAMCAFAAAABodHRwczovL2FkbWluLXNoZWxsLmlvL3p2ZWkvbmFtZXBsYXRlLzIvMC9OYW1l" +
           "cGxhdGUvTWFya2luZ3MvTWFya2luZy9NYXJraW5nRmlsZQoAAAAVYIkKAgAAAAAABAAAAFNpemUBAVoA" +
           "AC4ARFoAAAAACf////8BAf////8AAAAAFWCJCgIAAAAAAAgAAABXcml0YWJsZQEBWwAALgBEWwAAAAAB" +
           "/////wEB/////wAAAAAVYIkKAgAAAAAADAAAAFVzZXJXcml0YWJsZQEBXAAALgBEXAAAAAAB/////wEB" +
           "/////wAAAAAVYIkKAgAAAAAACQAAAE9wZW5Db3VudAEBXQAALgBEXQAAAAAF/////wEB/////wAAAAAE" +
           "YYIKBAAAAAAABAAAAE9wZW4BAWEAAC8BADwtYQAAAAEB/////wIAAAAXYKkKAgAAAAAADgAAAElucHV0" +
           "QXJndW1lbnRzAQFiAAAuAERiAAAAlgEAAAABACoBARMAAAAEAAAATW9kZQAD/////wAAAAAAAQAoAQEA" +
           "AAABAAAAAQAAAAEB/////wAAAAAXYKkKAgAAAAAADwAAAE91dHB1dEFyZ3VtZW50cwEBYwAALgBEYwAA" +
           "AJYBAAAAAQAqAQEZAAAACgAAAEZpbGVIYW5kbGUAB/////8AAAAAAAEAKAEBAAAAAQAAAAEAAAABAf//" +
           "//8AAAAABGGCCgQAAAAAAAUAAABDbG9zZQEBZAAALwEAPy1kAAAAAQH/////AQAAABdgqQoCAAAAAAAO" +
           "AAAASW5wdXRBcmd1bWVudHMBAWUAAC4ARGUAAACWAQAAAAEAKgEBGQAAAAoAAABGaWxlSGFuZGxlAAf/" +
           "////AAAAAAABACgBAQAAAAEAAAABAAAAAQH/////AAAAAARhggoEAAAAAAAEAAAAUmVhZAEBZgAALwEA" +
           "QS1mAAAAAQH/////AgAAABdgqQoCAAAAAAAOAAAASW5wdXRBcmd1bWVudHMBAWcAAC4ARGcAAACWAgAA" +
           "AAEAKgEBGQAAAAoAAABGaWxlSGFuZGxlAAf/////AAAAAAABACoBARUAAAAGAAAATGVuZ3RoAAb/////" +
           "AAAAAAABACgBAQAAAAEAAAACAAAAAQH/////AAAAABdgqQoCAAAAAAAPAAAAT3V0cHV0QXJndW1lbnRz" +
           "AQFoAAAuAERoAAAAlgEAAAABACoBARMAAAAEAAAARGF0YQAP/////wAAAAAAAQAoAQEAAAABAAAAAQAA" +
           "AAEB/////wAAAAAEYYIKBAAAAAAABQAAAFdyaXRlAQFpAAAvAQBELWkAAAABAf////8BAAAAF2CpCgIA" +
           "AAAAAA4AAABJbnB1dEFyZ3VtZW50cwEBagAALgBEagAAAJYCAAAAAQAqAQEZAAAACgAAAEZpbGVIYW5k" +
           "bGUAB/////8AAAAAAAEAKgEBEwAAAAQAAABEYXRhAA//////AAAAAAABACgBAQAAAAEAAAACAAAAAQH/" +
           "////AAAAAARhggoEAAAAAAALAAAAR2V0UG9zaXRpb24BAWsAAC8BAEYtawAAAAEB/////wIAAAAXYKkK" +
           "AgAAAAAADgAAAElucHV0QXJndW1lbnRzAQFsAAAuAERsAAAAlgEAAAABACoBARkAAAAKAAAARmlsZUhh" +
           "bmRsZQAH/////wAAAAAAAQAoAQEAAAABAAAAAQAAAAEB/////wAAAAAXYKkKAgAAAAAADwAAAE91dHB1" +
           "dEFyZ3VtZW50cwEBbQAALgBEbQAAAJYBAAAAAQAqAQEXAAAACAAAAFBvc2l0aW9uAAn/////AAAAAAAB" +
           "ACgBAQAAAAEAAAABAAAAAQH/////AAAAAARhggoEAAAAAAALAAAAU2V0UG9zaXRpb24BAW4AAC8BAEkt" +
           "bgAAAAEB/////wEAAAAXYKkKAgAAAAAADgAAAElucHV0QXJndW1lbnRzAQFvAAAuAERvAAAAlgIAAAAB" +
           "ACoBARkAAAAKAAAARmlsZUhhbmRsZQAH/////wAAAAAAAQAqAQEXAAAACAAAAFBvc2l0aW9uAAn/////" +
           "AAAAAAABACgBAQAAAAEAAAACAAAAAQH/////AAAAABdgiQoCAAAAAQAVAAAATWFya2luZ0FkZGl0aW9u" +
           "YWxUZXh0AQFwAAAuAERwAAAAAAwBAAAAAQAAAAAAAAABAQEAAAABAL1EAAMCAFoAAABodHRwczovL2Fk" +
           "bWluLXNoZWxsLmlvL3p2ZWkvbmFtZXBsYXRlLzIvMC9OYW1lcGxhdGUvTWFya2luZ3MvTWFya2luZy9N" +
           "YXJraW5nQWRkaXRpb25hbFRleHQAAAAA";
        #endregion
        #endif
        #endregion

        #region Public Properties
        /// <remarks />
        public PropertyState<string> MarkingName
        {
            get
            {
                return m_markingName;
            }

            set
            {
                if (!Object.ReferenceEquals(m_markingName, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_markingName = value;
            }
        }

        /// <remarks />
        public PropertyState<string> DesignationOfCertificateOrApproval
        {
            get
            {
                return m_designationOfCertificateOrApproval;
            }

            set
            {
                if (!Object.ReferenceEquals(m_designationOfCertificateOrApproval, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_designationOfCertificateOrApproval = value;
            }
        }

        /// <remarks />
        public PropertyState<DateTime> IssueDate
        {
            get
            {
                return m_issueDate;
            }

            set
            {
                if (!Object.ReferenceEquals(m_issueDate, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_issueDate = value;
            }
        }

        /// <remarks />
        public PropertyState<DateTime> ExpiryDate
        {
            get
            {
                return m_expiryDate;
            }

            set
            {
                if (!Object.ReferenceEquals(m_expiryDate, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_expiryDate = value;
            }
        }

        /// <remarks />
        public FileState MarkingFile
        {
            get
            {
                return m_markingFile;
            }

            set
            {
                if (!Object.ReferenceEquals(m_markingFile, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_markingFile = value;
            }
        }

        /// <remarks />
        public PropertyState<string[]> MarkingAdditionalText
        {
            get
            {
                return m_markingAdditionalText;
            }

            set
            {
                if (!Object.ReferenceEquals(m_markingAdditionalText, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_markingAdditionalText = value;
            }
        }
        #endregion

        #region Overridden Methods
        /// <remarks />
        public override void GetChildren(
            ISystemContext context,
            IList<BaseInstanceState> children)
        {
            if (m_markingName != null)
            {
                children.Add(m_markingName);
            }

            if (m_designationOfCertificateOrApproval != null)
            {
                children.Add(m_designationOfCertificateOrApproval);
            }

            if (m_issueDate != null)
            {
                children.Add(m_issueDate);
            }

            if (m_expiryDate != null)
            {
                children.Add(m_expiryDate);
            }

            if (m_markingFile != null)
            {
                children.Add(m_markingFile);
            }

            if (m_markingAdditionalText != null)
            {
                children.Add(m_markingAdditionalText);
            }

            base.GetChildren(context, children);
        }

        /// <remarks />
        protected override BaseInstanceState FindChild(
            ISystemContext context,
            QualifiedName browseName,
            bool createOrReplace,
            BaseInstanceState replacement)
        {
            if (QualifiedName.IsNull(browseName))
            {
                return null;
            }

            BaseInstanceState instance = null;

            switch (browseName.Name)
            {
                case I4AAS.Submodels.BrowseNames.MarkingName:
                {
                    if (createOrReplace)
                    {
                        if (MarkingName == null)
                        {
                            if (replacement == null)
                            {
                                MarkingName = new PropertyState<string>(this);
                            }
                            else
                            {
                                MarkingName = (PropertyState<string>)replacement;
                            }
                        }
                    }

                    instance = MarkingName;
                    break;
                }

                case I4AAS.Submodels.BrowseNames.DesignationOfCertificateOrApproval:
                {
                    if (createOrReplace)
                    {
                        if (DesignationOfCertificateOrApproval == null)
                        {
                            if (replacement == null)
                            {
                                DesignationOfCertificateOrApproval = new PropertyState<string>(this);
                            }
                            else
                            {
                                DesignationOfCertificateOrApproval = (PropertyState<string>)replacement;
                            }
                        }
                    }

                    instance = DesignationOfCertificateOrApproval;
                    break;
                }

                case I4AAS.Submodels.BrowseNames.IssueDate:
                {
                    if (createOrReplace)
                    {
                        if (IssueDate == null)
                        {
                            if (replacement == null)
                            {
                                IssueDate = new PropertyState<DateTime>(this);
                            }
                            else
                            {
                                IssueDate = (PropertyState<DateTime>)replacement;
                            }
                        }
                    }

                    instance = IssueDate;
                    break;
                }

                case I4AAS.Submodels.BrowseNames.ExpiryDate:
                {
                    if (createOrReplace)
                    {
                        if (ExpiryDate == null)
                        {
                            if (replacement == null)
                            {
                                ExpiryDate = new PropertyState<DateTime>(this);
                            }
                            else
                            {
                                ExpiryDate = (PropertyState<DateTime>)replacement;
                            }
                        }
                    }

                    instance = ExpiryDate;
                    break;
                }

                case I4AAS.Submodels.BrowseNames.MarkingFile:
                {
                    if (createOrReplace)
                    {
                        if (MarkingFile == null)
                        {
                            if (replacement == null)
                            {
                                MarkingFile = new FileState(this);
                            }
                            else
                            {
                                MarkingFile = (FileState)replacement;
                            }
                        }
                    }

                    instance = MarkingFile;
                    break;
                }

                case I4AAS.Submodels.BrowseNames.MarkingAdditionalText:
                {
                    if (createOrReplace)
                    {
                        if (MarkingAdditionalText == null)
                        {
                            if (replacement == null)
                            {
                                MarkingAdditionalText = new PropertyState<string[]>(this);
                            }
                            else
                            {
                                MarkingAdditionalText = (PropertyState<string[]>)replacement;
                            }
                        }
                    }

                    instance = MarkingAdditionalText;
                    break;
                }
            }

            if (instance != null)
            {
                return instance;
            }

            return base.FindChild(context, browseName, createOrReplace, replacement);
        }
        #endregion

        #region Private Fields
        private PropertyState<string> m_markingName;
        private PropertyState<string> m_designationOfCertificateOrApproval;
        private PropertyState<DateTime> m_issueDate;
        private PropertyState<DateTime> m_expiryDate;
        private FileState m_markingFile;
        private PropertyState<string[]> m_markingAdditionalText;
        #endregion
    }
    #endif
    #endregion

    #region ProductCarbonFootprintSubmodelState Class
    #if (!OPCUA_EXCLUDE_ProductCarbonFootprintSubmodelState)
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public partial class ProductCarbonFootprintSubmodelState : SubmodelState
    {
        #region Constructors
        /// <remarks />
        public ProductCarbonFootprintSubmodelState(NodeState parent) : base(parent)
        {
        }

        /// <remarks />
        protected override NodeId GetDefaultTypeDefinitionId(NamespaceTable namespaceUris)
        {
            return Opc.Ua.NodeId.Create(I4AAS.Submodels.ObjectTypes.ProductCarbonFootprintSubmodelType, I4AAS.Submodels.Namespaces.I4AAS, namespaceUris);
        }

        #if (!OPCUA_EXCLUDE_InitializationStrings)
        /// <remarks />
        protected override void Initialize(ISystemContext context)
        {
            base.Initialize(context);
            Initialize(context, InitializationString);
            InitializeOptionalChildren(context);
        }

        /// <remarks />
        protected override void Initialize(ISystemContext context, NodeState source)
        {
            InitializeOptionalChildren(context);
            base.Initialize(context, source);
        }

        /// <remarks />
        protected override void InitializeOptionalChildren(ISystemContext context)
        {
            base.InitializeOptionalChildren(context);
        }

        #region Initialization String
        private const string InitializationString =
           "AgAAACIAAABodHRwOi8vb3BjZm91bmRhdGlvbi5vcmcvVUEvSTRBQVMvKwAAAGh0dHA6Ly9vcGNmb3Vu" +
           "ZGF0aW9uLm9yZy9VQS9EaWN0aW9uYXJ5L0lSREn/////BGCAAgEAAAABACoAAABQcm9kdWN0Q2FyYm9u" +
           "Rm9vdHByaW50U3VibW9kZWxUeXBlSW5zdGFuY2UBAXIAAQFyAHIAAAD/////CAAAABVgiQoCAAAAAQAH" +
           "AAAATW9kZWxJZAEBMQEALgBEMQEAAAAM/////wEB/////wAAAAAVYIkKAgAAAAEADAAAAE1vZGVsSWRT" +
           "aG9ydAEBMgEALgBEMgEAAAAM/////wEB/////wAAAAAVYIkKAgAAAAEAFAAAAFBDRkNhbGN1bGF0aW9u" +
           "TWV0aG9kAQFzAAAuAERzAAAAAAz/////AQEBAAAAAQC9RAADAgAUAAAAMDE3My0xIzAyLUFCRzg1NCMw" +
           "MDEAAAAAFWCJCgIAAAABAAgAAABQQ0ZDTzJlcQEBdAAALgBEdAAAAAAL/////wEBAQAAAAEAvUQAAwIA" +
           "FAAAADAxNzMtMSMwMi1BQkc4NTUjMDAxAAAAABVgiQoCAAAAAQAfAAAAUENGUmVmZXJlbmNlVmFsdWVG" +
           "b3JDYWxjdWxhdGlvbgEBdQAALgBEdQAAAAAM/////wEBAQAAAAEAvUQAAwIAFAAAADAxNzMtMSMwMi1B" +
           "Qkc4NTYjMDAxAAAAABVgiQoCAAAAAQAiAAAAUENGUXVhbnRpdHlPZk1lYXN1cmVGb3JDYWxjdWxhdGlv" +
           "bgEBdgAALgBEdgAAAAAL/////wEBAQAAAAEAvUQAAwIAFAAAADAxNzMtMSMwMi1BQkc4NTcjMDAxAAAA" +
           "ABdgiQoCAAAAAQARAAAAUENGTGl2ZUN5Y2xlUGhhc2UBAXcAAC4ARHcAAAAADAEAAAABAAAAAAAAAAEB" +
           "AQAAAAEAvUQAAwIAFAAAADAxNzMtMSMwMi1BQkc4NTgjMDAxAAAAAARggAoBAAAAAQAXAAAAUENGR29v" +
           "ZHNBZGRyZXNzSGFuZG92ZXIBAXgAAC8BAYEAeAAAAAEAAAABAL1EAAMCABQAAAAwMTczLTEjMDItQUJJ" +
           "NDk3IzAwMQIAAAAVYIkKAgAAAAEABwAAAE1vZGVsSWQBATQBAC4ARDQBAAAADP////8BAf////8AAAAA" +
           "FWCJCgIAAAABAAwAAABNb2RlbElkU2hvcnQBATUBAC4ARDUBAAAADP////8BAf////8AAAAA";
        #endregion
        #endif
        #endregion

        #region Public Properties
        /// <remarks />
        public PropertyState<string> PCFCalculationMethod
        {
            get
            {
                return m_pCFCalculationMethod;
            }

            set
            {
                if (!Object.ReferenceEquals(m_pCFCalculationMethod, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_pCFCalculationMethod = value;
            }
        }

        /// <remarks />
        public PropertyState<double> PCFCO2eq
        {
            get
            {
                return m_pCFCO2eq;
            }

            set
            {
                if (!Object.ReferenceEquals(m_pCFCO2eq, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_pCFCO2eq = value;
            }
        }

        /// <remarks />
        public PropertyState<string> PCFReferenceValueForCalculation
        {
            get
            {
                return m_pCFReferenceValueForCalculation;
            }

            set
            {
                if (!Object.ReferenceEquals(m_pCFReferenceValueForCalculation, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_pCFReferenceValueForCalculation = value;
            }
        }

        /// <remarks />
        public PropertyState<double> PCFQuantityOfMeasureForCalculation
        {
            get
            {
                return m_pCFQuantityOfMeasureForCalculation;
            }

            set
            {
                if (!Object.ReferenceEquals(m_pCFQuantityOfMeasureForCalculation, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_pCFQuantityOfMeasureForCalculation = value;
            }
        }

        /// <remarks />
        public PropertyState<string[]> PCFLiveCyclePhase
        {
            get
            {
                return m_pCFLiveCyclePhase;
            }

            set
            {
                if (!Object.ReferenceEquals(m_pCFLiveCyclePhase, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_pCFLiveCyclePhase = value;
            }
        }

        /// <remarks />
        public AddressSubmodelState PCFGoodsAddressHandover
        {
            get
            {
                return m_pCFGoodsAddressHandover;
            }

            set
            {
                if (!Object.ReferenceEquals(m_pCFGoodsAddressHandover, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_pCFGoodsAddressHandover = value;
            }
        }
        #endregion

        #region Overridden Methods
        /// <remarks />
        public override void GetChildren(
            ISystemContext context,
            IList<BaseInstanceState> children)
        {
            if (m_pCFCalculationMethod != null)
            {
                children.Add(m_pCFCalculationMethod);
            }

            if (m_pCFCO2eq != null)
            {
                children.Add(m_pCFCO2eq);
            }

            if (m_pCFReferenceValueForCalculation != null)
            {
                children.Add(m_pCFReferenceValueForCalculation);
            }

            if (m_pCFQuantityOfMeasureForCalculation != null)
            {
                children.Add(m_pCFQuantityOfMeasureForCalculation);
            }

            if (m_pCFLiveCyclePhase != null)
            {
                children.Add(m_pCFLiveCyclePhase);
            }

            if (m_pCFGoodsAddressHandover != null)
            {
                children.Add(m_pCFGoodsAddressHandover);
            }

            base.GetChildren(context, children);
        }

        /// <remarks />
        protected override BaseInstanceState FindChild(
            ISystemContext context,
            QualifiedName browseName,
            bool createOrReplace,
            BaseInstanceState replacement)
        {
            if (QualifiedName.IsNull(browseName))
            {
                return null;
            }

            BaseInstanceState instance = null;

            switch (browseName.Name)
            {
                case I4AAS.Submodels.BrowseNames.PCFCalculationMethod:
                {
                    if (createOrReplace)
                    {
                        if (PCFCalculationMethod == null)
                        {
                            if (replacement == null)
                            {
                                PCFCalculationMethod = new PropertyState<string>(this);
                            }
                            else
                            {
                                PCFCalculationMethod = (PropertyState<string>)replacement;
                            }
                        }
                    }

                    instance = PCFCalculationMethod;
                    break;
                }

                case I4AAS.Submodels.BrowseNames.PCFCO2eq:
                {
                    if (createOrReplace)
                    {
                        if (PCFCO2eq == null)
                        {
                            if (replacement == null)
                            {
                                PCFCO2eq = new PropertyState<double>(this);
                            }
                            else
                            {
                                PCFCO2eq = (PropertyState<double>)replacement;
                            }
                        }
                    }

                    instance = PCFCO2eq;
                    break;
                }

                case I4AAS.Submodels.BrowseNames.PCFReferenceValueForCalculation:
                {
                    if (createOrReplace)
                    {
                        if (PCFReferenceValueForCalculation == null)
                        {
                            if (replacement == null)
                            {
                                PCFReferenceValueForCalculation = new PropertyState<string>(this);
                            }
                            else
                            {
                                PCFReferenceValueForCalculation = (PropertyState<string>)replacement;
                            }
                        }
                    }

                    instance = PCFReferenceValueForCalculation;
                    break;
                }

                case I4AAS.Submodels.BrowseNames.PCFQuantityOfMeasureForCalculation:
                {
                    if (createOrReplace)
                    {
                        if (PCFQuantityOfMeasureForCalculation == null)
                        {
                            if (replacement == null)
                            {
                                PCFQuantityOfMeasureForCalculation = new PropertyState<double>(this);
                            }
                            else
                            {
                                PCFQuantityOfMeasureForCalculation = (PropertyState<double>)replacement;
                            }
                        }
                    }

                    instance = PCFQuantityOfMeasureForCalculation;
                    break;
                }

                case I4AAS.Submodels.BrowseNames.PCFLiveCyclePhase:
                {
                    if (createOrReplace)
                    {
                        if (PCFLiveCyclePhase == null)
                        {
                            if (replacement == null)
                            {
                                PCFLiveCyclePhase = new PropertyState<string[]>(this);
                            }
                            else
                            {
                                PCFLiveCyclePhase = (PropertyState<string[]>)replacement;
                            }
                        }
                    }

                    instance = PCFLiveCyclePhase;
                    break;
                }

                case I4AAS.Submodels.BrowseNames.PCFGoodsAddressHandover:
                {
                    if (createOrReplace)
                    {
                        if (PCFGoodsAddressHandover == null)
                        {
                            if (replacement == null)
                            {
                                PCFGoodsAddressHandover = new AddressSubmodelState(this);
                            }
                            else
                            {
                                PCFGoodsAddressHandover = (AddressSubmodelState)replacement;
                            }
                        }
                    }

                    instance = PCFGoodsAddressHandover;
                    break;
                }
            }

            if (instance != null)
            {
                return instance;
            }

            return base.FindChild(context, browseName, createOrReplace, replacement);
        }
        #endregion

        #region Private Fields
        private PropertyState<string> m_pCFCalculationMethod;
        private PropertyState<double> m_pCFCO2eq;
        private PropertyState<string> m_pCFReferenceValueForCalculation;
        private PropertyState<double> m_pCFQuantityOfMeasureForCalculation;
        private PropertyState<string[]> m_pCFLiveCyclePhase;
        private AddressSubmodelState m_pCFGoodsAddressHandover;
        #endregion
    }
    #endif
    #endregion

    #region AddressSubmodelState Class
    #if (!OPCUA_EXCLUDE_AddressSubmodelState)
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public partial class AddressSubmodelState : SubmodelState
    {
        #region Constructors
        /// <remarks />
        public AddressSubmodelState(NodeState parent) : base(parent)
        {
        }

        /// <remarks />
        protected override NodeId GetDefaultTypeDefinitionId(NamespaceTable namespaceUris)
        {
            return Opc.Ua.NodeId.Create(I4AAS.Submodels.ObjectTypes.AddressSubmodelType, I4AAS.Submodels.Namespaces.I4AAS, namespaceUris);
        }

        #if (!OPCUA_EXCLUDE_InitializationStrings)
        /// <remarks />
        protected override void Initialize(ISystemContext context)
        {
            base.Initialize(context);
            Initialize(context, InitializationString);
            InitializeOptionalChildren(context);
        }

        /// <remarks />
        protected override void Initialize(ISystemContext context, NodeState source)
        {
            InitializeOptionalChildren(context);
            base.Initialize(context, source);
        }

        /// <remarks />
        protected override void InitializeOptionalChildren(ISystemContext context)
        {
            base.InitializeOptionalChildren(context);

            if (Street != null)
            {
                Street.Initialize(context, Street_InitializationString);
            }

            if (HouseNumber != null)
            {
                HouseNumber.Initialize(context, HouseNumber_InitializationString);
            }

            if (ZipCode != null)
            {
                ZipCode.Initialize(context, ZipCode_InitializationString);
            }

            if (CityTown != null)
            {
                CityTown.Initialize(context, CityTown_InitializationString);
            }

            if (Country != null)
            {
                Country.Initialize(context, Country_InitializationString);
            }

            if (Latitude != null)
            {
                Latitude.Initialize(context, Latitude_InitializationString);
            }

            if (Longitude != null)
            {
                Longitude.Initialize(context, Longitude_InitializationString);
            }
        }

        #region Initialization String
        private const string Street_InitializationString =
           "AgAAACIAAABodHRwOi8vb3BjZm91bmRhdGlvbi5vcmcvVUEvSTRBQVMvKwAAAGh0dHA6Ly9vcGNmb3Vu" +
           "ZGF0aW9uLm9yZy9VQS9EaWN0aW9uYXJ5L0lSREn/////FWCJCgIAAAABAAYAAABTdHJlZXQBAYIAAC4A" +
           "RIIAAAAADP////8BAQEAAAABAL1EAAMCABQAAAAwMTczLTEjMDItQUJIOTU2IzAwMQAAAAA=";

        private const string HouseNumber_InitializationString =
           "AgAAACIAAABodHRwOi8vb3BjZm91bmRhdGlvbi5vcmcvVUEvSTRBQVMvKwAAAGh0dHA6Ly9vcGNmb3Vu" +
           "ZGF0aW9uLm9yZy9VQS9EaWN0aW9uYXJ5L0lSREn/////FWCJCgIAAAABAAsAAABIb3VzZU51bWJlcgEB" +
           "gwAALgBEgwAAAAAM/////wEBAQAAAAEAvUQAAwIAFAAAADAxNzMtMSMwMi1BQkg5NTcjMDAxAAAAAA==";

        private const string ZipCode_InitializationString =
           "AgAAACIAAABodHRwOi8vb3BjZm91bmRhdGlvbi5vcmcvVUEvSTRBQVMvKwAAAGh0dHA6Ly9vcGNmb3Vu" +
           "ZGF0aW9uLm9yZy9VQS9EaWN0aW9uYXJ5L0lSREn/////FWCJCgIAAAABAAcAAABaaXBDb2RlAQGEAAAu" +
           "AESEAAAAAAz/////AQEBAAAAAQC9RAADAgAUAAAAMDE3My0xIzAyLUFCSDk1OCMwMDEAAAAA";

        private const string CityTown_InitializationString =
           "AgAAACIAAABodHRwOi8vb3BjZm91bmRhdGlvbi5vcmcvVUEvSTRBQVMvKwAAAGh0dHA6Ly9vcGNmb3Vu" +
           "ZGF0aW9uLm9yZy9VQS9EaWN0aW9uYXJ5L0lSREn/////FWCJCgIAAAABAAgAAABDaXR5VG93bgEBhQAA" +
           "LgBEhQAAAAAM/////wEBAQAAAAEAvUQAAwIAFAAAADAxNzMtMSMwMi1BQkg5NTkjMDAxAAAAAA==";

        private const string Country_InitializationString =
           "AgAAACIAAABodHRwOi8vb3BjZm91bmRhdGlvbi5vcmcvVUEvSTRBQVMvKwAAAGh0dHA6Ly9vcGNmb3Vu" +
           "ZGF0aW9uLm9yZy9VQS9EaWN0aW9uYXJ5L0lSREn/////FWCJCgIAAAABAAcAAABDb3VudHJ5AQGGAAAu" +
           "AESGAAAAAAz/////AQEBAAAAAQC9RAADAgAUAAAAMDE3My0xIzAyLUFBTzI1OSMwMDUAAAAA";

        private const string Latitude_InitializationString =
           "AgAAACIAAABodHRwOi8vb3BjZm91bmRhdGlvbi5vcmcvVUEvSTRBQVMvKwAAAGh0dHA6Ly9vcGNmb3Vu" +
           "ZGF0aW9uLm9yZy9VQS9EaWN0aW9uYXJ5L0lSREn/////FWCJCgIAAAABAAgAAABMYXRpdHVkZQEBhwAA" +
           "LgBEhwAAAAAL/////wEBAQAAAAEAvUQAAwIAFAAAADAxNzMtMSMwMi1BQkg5NjAjMDAxAAAAAA==";

        private const string Longitude_InitializationString =
           "AgAAACIAAABodHRwOi8vb3BjZm91bmRhdGlvbi5vcmcvVUEvSTRBQVMvKwAAAGh0dHA6Ly9vcGNmb3Vu" +
           "ZGF0aW9uLm9yZy9VQS9EaWN0aW9uYXJ5L0lSREn/////FWCJCgIAAAABAAkAAABMb25naXR1ZGUBAYgA" +
           "AC4ARIgAAAAAC/////8BAQEAAAABAL1EAAMCABQAAAAwMTczLTEjMDItQUJIOTYxIzAwMQAAAAA=";

        private const string InitializationString =
           "AgAAACIAAABodHRwOi8vb3BjZm91bmRhdGlvbi5vcmcvVUEvSTRBQVMvKwAAAGh0dHA6Ly9vcGNmb3Vu" +
           "ZGF0aW9uLm9yZy9VQS9EaWN0aW9uYXJ5L0lSREn/////BGCAAgEAAAABABsAAABBZGRyZXNzU3VibW9k" +
           "ZWxUeXBlSW5zdGFuY2UBAYEAAQGBAIEAAAD/////CQAAABVgiQoCAAAAAQAHAAAATW9kZWxJZAEBNwEA" +
           "LgBENwEAAAAM/////wEB/////wAAAAAVYIkKAgAAAAEADAAAAE1vZGVsSWRTaG9ydAEBOAEALgBEOAEA" +
           "AAAM/////wEB/////wAAAAAVYIkKAgAAAAEABgAAAFN0cmVldAEBggAALgBEggAAAAAM/////wEBAQAA" +
           "AAEAvUQAAwIAFAAAADAxNzMtMSMwMi1BQkg5NTYjMDAxAAAAABVgiQoCAAAAAQALAAAASG91c2VOdW1i" +
           "ZXIBAYMAAC4ARIMAAAAADP////8BAQEAAAABAL1EAAMCABQAAAAwMTczLTEjMDItQUJIOTU3IzAwMQAA" +
           "AAAVYIkKAgAAAAEABwAAAFppcENvZGUBAYQAAC4ARIQAAAAADP////8BAQEAAAABAL1EAAMCABQAAAAw" +
           "MTczLTEjMDItQUJIOTU4IzAwMQAAAAAVYIkKAgAAAAEACAAAAENpdHlUb3duAQGFAAAuAESFAAAAAAz/" +
           "////AQEBAAAAAQC9RAADAgAUAAAAMDE3My0xIzAyLUFCSDk1OSMwMDEAAAAAFWCJCgIAAAABAAcAAABD" +
           "b3VudHJ5AQGGAAAuAESGAAAAAAz/////AQEBAAAAAQC9RAADAgAUAAAAMDE3My0xIzAyLUFBTzI1OSMw" +
           "MDUAAAAAFWCJCgIAAAABAAgAAABMYXRpdHVkZQEBhwAALgBEhwAAAAAL/////wEBAQAAAAEAvUQAAwIA" +
           "FAAAADAxNzMtMSMwMi1BQkg5NjAjMDAxAAAAABVgiQoCAAAAAQAJAAAATG9uZ2l0dWRlAQGIAAAuAESI" +
           "AAAAAAv/////AQEBAAAAAQC9RAADAgAUAAAAMDE3My0xIzAyLUFCSDk2MSMwMDEAAAAA";
        #endregion
        #endif
        #endregion

        #region Public Properties
        /// <remarks />
        public PropertyState<string> Street
        {
            get
            {
                return m_street;
            }

            set
            {
                if (!Object.ReferenceEquals(m_street, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_street = value;
            }
        }

        /// <remarks />
        public PropertyState<string> HouseNumber
        {
            get
            {
                return m_houseNumber;
            }

            set
            {
                if (!Object.ReferenceEquals(m_houseNumber, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_houseNumber = value;
            }
        }

        /// <remarks />
        public PropertyState<string> ZipCode
        {
            get
            {
                return m_zipCode;
            }

            set
            {
                if (!Object.ReferenceEquals(m_zipCode, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_zipCode = value;
            }
        }

        /// <remarks />
        public PropertyState<string> CityTown
        {
            get
            {
                return m_cityTown;
            }

            set
            {
                if (!Object.ReferenceEquals(m_cityTown, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_cityTown = value;
            }
        }

        /// <remarks />
        public PropertyState<string> Country
        {
            get
            {
                return m_country;
            }

            set
            {
                if (!Object.ReferenceEquals(m_country, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_country = value;
            }
        }

        /// <remarks />
        public PropertyState<double> Latitude
        {
            get
            {
                return m_latitude;
            }

            set
            {
                if (!Object.ReferenceEquals(m_latitude, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_latitude = value;
            }
        }

        /// <remarks />
        public PropertyState<double> Longitude
        {
            get
            {
                return m_longitude;
            }

            set
            {
                if (!Object.ReferenceEquals(m_longitude, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_longitude = value;
            }
        }
        #endregion

        #region Overridden Methods
        /// <remarks />
        public override void GetChildren(
            ISystemContext context,
            IList<BaseInstanceState> children)
        {
            if (m_street != null)
            {
                children.Add(m_street);
            }

            if (m_houseNumber != null)
            {
                children.Add(m_houseNumber);
            }

            if (m_zipCode != null)
            {
                children.Add(m_zipCode);
            }

            if (m_cityTown != null)
            {
                children.Add(m_cityTown);
            }

            if (m_country != null)
            {
                children.Add(m_country);
            }

            if (m_latitude != null)
            {
                children.Add(m_latitude);
            }

            if (m_longitude != null)
            {
                children.Add(m_longitude);
            }

            base.GetChildren(context, children);
        }

        /// <remarks />
        protected override BaseInstanceState FindChild(
            ISystemContext context,
            QualifiedName browseName,
            bool createOrReplace,
            BaseInstanceState replacement)
        {
            if (QualifiedName.IsNull(browseName))
            {
                return null;
            }

            BaseInstanceState instance = null;

            switch (browseName.Name)
            {
                case I4AAS.Submodels.BrowseNames.Street:
                {
                    if (createOrReplace)
                    {
                        if (Street == null)
                        {
                            if (replacement == null)
                            {
                                Street = new PropertyState<string>(this);
                            }
                            else
                            {
                                Street = (PropertyState<string>)replacement;
                            }
                        }
                    }

                    instance = Street;
                    break;
                }

                case I4AAS.Submodels.BrowseNames.HouseNumber:
                {
                    if (createOrReplace)
                    {
                        if (HouseNumber == null)
                        {
                            if (replacement == null)
                            {
                                HouseNumber = new PropertyState<string>(this);
                            }
                            else
                            {
                                HouseNumber = (PropertyState<string>)replacement;
                            }
                        }
                    }

                    instance = HouseNumber;
                    break;
                }

                case I4AAS.Submodels.BrowseNames.ZipCode:
                {
                    if (createOrReplace)
                    {
                        if (ZipCode == null)
                        {
                            if (replacement == null)
                            {
                                ZipCode = new PropertyState<string>(this);
                            }
                            else
                            {
                                ZipCode = (PropertyState<string>)replacement;
                            }
                        }
                    }

                    instance = ZipCode;
                    break;
                }

                case I4AAS.Submodels.BrowseNames.CityTown:
                {
                    if (createOrReplace)
                    {
                        if (CityTown == null)
                        {
                            if (replacement == null)
                            {
                                CityTown = new PropertyState<string>(this);
                            }
                            else
                            {
                                CityTown = (PropertyState<string>)replacement;
                            }
                        }
                    }

                    instance = CityTown;
                    break;
                }

                case I4AAS.Submodels.BrowseNames.Country:
                {
                    if (createOrReplace)
                    {
                        if (Country == null)
                        {
                            if (replacement == null)
                            {
                                Country = new PropertyState<string>(this);
                            }
                            else
                            {
                                Country = (PropertyState<string>)replacement;
                            }
                        }
                    }

                    instance = Country;
                    break;
                }

                case I4AAS.Submodels.BrowseNames.Latitude:
                {
                    if (createOrReplace)
                    {
                        if (Latitude == null)
                        {
                            if (replacement == null)
                            {
                                Latitude = new PropertyState<double>(this);
                            }
                            else
                            {
                                Latitude = (PropertyState<double>)replacement;
                            }
                        }
                    }

                    instance = Latitude;
                    break;
                }

                case I4AAS.Submodels.BrowseNames.Longitude:
                {
                    if (createOrReplace)
                    {
                        if (Longitude == null)
                        {
                            if (replacement == null)
                            {
                                Longitude = new PropertyState<double>(this);
                            }
                            else
                            {
                                Longitude = (PropertyState<double>)replacement;
                            }
                        }
                    }

                    instance = Longitude;
                    break;
                }
            }

            if (instance != null)
            {
                return instance;
            }

            return base.FindChild(context, browseName, createOrReplace, replacement);
        }
        #endregion

        #region Private Fields
        private PropertyState<string> m_street;
        private PropertyState<string> m_houseNumber;
        private PropertyState<string> m_zipCode;
        private PropertyState<string> m_cityTown;
        private PropertyState<string> m_country;
        private PropertyState<double> m_latitude;
        private PropertyState<double> m_longitude;
        #endregion
    }
    #endif
    #endregion
}