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
using System.Runtime.Serialization;

namespace I4AAS.Submodels
{
    #region SubmodelDataType Class
#if (!OPCUA_EXCLUDE_SubmodelDataType)
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    [DataContract(Namespace = I4AAS.Submodels.Namespaces.I4AASXsd)]
    public partial class SubmodelDataType : IEncodeable, IJsonEncodeable
    {
        #region Constructors
        /// <remarks />
        public SubmodelDataType()
        {
            Initialize();
        }

        [OnDeserializing]
        private void Initialize(StreamingContext context)
        {
            Initialize();
        }

        private void Initialize()
        {
            m_modelId = null;
            m_modelIdShort = null;
            m_modelDescription = null;
        }
        #endregion

        #region Public Properties
        /// <remarks />
        [DataMember(Name = "ModelId", IsRequired = false, Order = 1)]
        public string ModelId
        {
            get { return m_modelId;  }
            set { m_modelId = value; }
        }

        /// <remarks />
        [DataMember(Name = "ModelIdShort", IsRequired = false, Order = 2)]
        public string ModelIdShort
        {
            get { return m_modelIdShort;  }
            set { m_modelIdShort = value; }
        }

        /// <remarks />
        [DataMember(Name = "ModelDescription", IsRequired = false, Order = 3)]
        public LocalizedText ModelDescription
        {
            get { return m_modelDescription;  }
            set { m_modelDescription = value; }
        }
        #endregion

        #region IEncodeable Members
        /// <summary cref="IEncodeable.TypeId" />
        public virtual ExpandedNodeId TypeId => DataTypeIds.SubmodelDataType;

        /// <summary cref="IEncodeable.BinaryEncodingId" />
        public virtual ExpandedNodeId BinaryEncodingId => ObjectIds.SubmodelDataType_Encoding_DefaultBinary;

        /// <summary cref="IEncodeable.XmlEncodingId" />
        public virtual ExpandedNodeId XmlEncodingId => ObjectIds.SubmodelDataType_Encoding_DefaultXml;

        /// <summary cref="IJsonEncodeable.JsonEncodingId" />
        public virtual ExpandedNodeId JsonEncodingId => ObjectIds.SubmodelDataType_Encoding_DefaultJson;

        /// <summary cref="IEncodeable.Encode(IEncoder)" />
        public virtual void Encode(IEncoder encoder)
        {
            encoder.PushNamespace(I4AAS.Submodels.Namespaces.I4AASXsd);

            encoder.WriteString("ModelId", ModelId);
            encoder.WriteString("ModelIdShort", ModelIdShort);
            encoder.WriteLocalizedText("ModelDescription", ModelDescription);

            encoder.PopNamespace();
        }

        /// <summary cref="IEncodeable.Decode(IDecoder)" />
        public virtual void Decode(IDecoder decoder)
        {
            decoder.PushNamespace(I4AAS.Submodels.Namespaces.I4AASXsd);

            ModelId = decoder.ReadString("ModelId");
            ModelIdShort = decoder.ReadString("ModelIdShort");
            ModelDescription = decoder.ReadLocalizedText("ModelDescription");

            decoder.PopNamespace();
        }

        /// <summary cref="IEncodeable.IsEqual(IEncodeable)" />
        public virtual bool IsEqual(IEncodeable encodeable)
        {
            if (Object.ReferenceEquals(this, encodeable))
            {
                return true;
            }

            SubmodelDataType value = encodeable as SubmodelDataType;

            if (value == null)
            {
                return false;
            }

            if (!Utils.IsEqual(m_modelId, value.m_modelId)) return false;
            if (!Utils.IsEqual(m_modelIdShort, value.m_modelIdShort)) return false;
            if (!Utils.IsEqual(m_modelDescription, value.m_modelDescription)) return false;

            return true;
        }

        /// <summary cref="ICloneable.Clone" />
        public virtual object Clone()
        {
            return (SubmodelDataType)this.MemberwiseClone();
        }

        /// <summary cref="Object.MemberwiseClone" />
        public new object MemberwiseClone()
        {
            SubmodelDataType clone = (SubmodelDataType)base.MemberwiseClone();

            clone.m_modelId = (string)Utils.Clone(this.m_modelId);
            clone.m_modelIdShort = (string)Utils.Clone(this.m_modelIdShort);
            clone.m_modelDescription = (LocalizedText)Utils.Clone(this.m_modelDescription);

            return clone;
        }
        #endregion

        #region Private Fields
        private string m_modelId;
        private string m_modelIdShort;
        private LocalizedText m_modelDescription;
        #endregion
    }

    #region SubmodelDataTypeCollection Class
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    [CollectionDataContract(Name = "ListOfSubmodelDataType", Namespace = I4AAS.Submodels.Namespaces.I4AASXsd, ItemName = "SubmodelDataType")]
    public partial class SubmodelDataTypeCollection : List<SubmodelDataType>, ICloneable
    {
        #region Constructors
        /// <remarks />
        public SubmodelDataTypeCollection() {}

        /// <remarks />
        public SubmodelDataTypeCollection(int capacity) : base(capacity) {}

        /// <remarks />
        public SubmodelDataTypeCollection(IEnumerable<SubmodelDataType> collection) : base(collection) {}
        #endregion

        #region Static Operators
        /// <remarks />
        public static implicit operator SubmodelDataTypeCollection(SubmodelDataType[] values)
        {
            if (values != null)
            {
                return new SubmodelDataTypeCollection(values);
            }

            return new SubmodelDataTypeCollection();
        }

        /// <remarks />
        public static explicit operator SubmodelDataType[](SubmodelDataTypeCollection values)
        {
            if (values != null)
            {
                return values.ToArray();
            }

            return null;
        }
        #endregion

        #region ICloneable Methods
        /// <remarks />
        public object Clone()
        {
            return (SubmodelDataTypeCollection)this.MemberwiseClone();
        }
        #endregion

        /// <summary cref="Object.MemberwiseClone" />
        public new object MemberwiseClone()
        {
            SubmodelDataTypeCollection clone = new SubmodelDataTypeCollection(this.Count);

            for (int ii = 0; ii < this.Count; ii++)
            {
                clone.Add((SubmodelDataType)Utils.Clone(this[ii]));
            }

            return clone;
        }
    }
    #endregion
    #endif
    #endregion

    #region AssetDescriptionFileDataType Class
    #if (!OPCUA_EXCLUDE_AssetDescriptionFileDataType)
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    [DataContract(Namespace = I4AAS.Submodels.Namespaces.I4AASXsd)]
    public partial class AssetDescriptionFileDataType : Opc.Ua.DataTypeSchemaHeader
    {
        #region Constructors
        /// <remarks />
        public AssetDescriptionFileDataType()
        {
            Initialize();
        }

        [OnDeserializing]
        private void Initialize(StreamingContext context)
        {
            Initialize();
        }

        private void Initialize()
        {
            m_modelId = null;
            m_modelIdShort = null;
            m_submodels = new ExtensionObjectCollection();
        }
        #endregion

        #region Public Properties
        /// <remarks />
        [DataMember(Name = "ModelId", IsRequired = false, Order = 1)]
        public string ModelId
        {
            get { return m_modelId;  }
            set { m_modelId = value; }
        }

        /// <remarks />
        [DataMember(Name = "ModelIdShort", IsRequired = false, Order = 2)]
        public string ModelIdShort
        {
            get { return m_modelIdShort;  }
            set { m_modelIdShort = value; }
        }

        /// <remarks />
        [DataMember(Name = "Submodels", IsRequired = false, Order = 3)]
        public ExtensionObjectCollection Submodels
        {
            get { return m_submodels;  }
            set { m_submodels = value; }
        }
        #endregion

        #region IEncodeable Members
        /// <summary cref="IEncodeable.TypeId" />
        public override ExpandedNodeId TypeId => DataTypeIds.AssetDescriptionFileDataType;

        /// <summary cref="IEncodeable.BinaryEncodingId" />
        public override ExpandedNodeId BinaryEncodingId => ObjectIds.AssetDescriptionFileDataType_Encoding_DefaultBinary;

        /// <summary cref="IEncodeable.XmlEncodingId" />
        public override ExpandedNodeId XmlEncodingId => ObjectIds.AssetDescriptionFileDataType_Encoding_DefaultXml;

        /// <summary cref="IJsonEncodeable.JsonEncodingId" />
        public override ExpandedNodeId JsonEncodingId => ObjectIds.AssetDescriptionFileDataType_Encoding_DefaultJson;

        /// <summary cref="IEncodeable.Encode(IEncoder)" />
        public override void Encode(IEncoder encoder)
        {
            base.Encode(encoder);

            encoder.PushNamespace(I4AAS.Submodels.Namespaces.I4AASXsd);

            encoder.WriteString("ModelId", ModelId);
            encoder.WriteString("ModelIdShort", ModelIdShort);
            encoder.WriteExtensionObjectArray("Submodels", Submodels);

            encoder.PopNamespace();
        }

        /// <summary cref="IEncodeable.Decode(IDecoder)" />
        public override void Decode(IDecoder decoder)
        {
            base.Decode(decoder);

            decoder.PushNamespace(I4AAS.Submodels.Namespaces.I4AASXsd);

            ModelId = decoder.ReadString("ModelId");
            ModelIdShort = decoder.ReadString("ModelIdShort");
            Submodels = decoder.ReadExtensionObjectArray("Submodels");

            decoder.PopNamespace();
        }

        /// <summary cref="IEncodeable.IsEqual(IEncodeable)" />
        public override bool IsEqual(IEncodeable encodeable)
        {
            if (Object.ReferenceEquals(this, encodeable))
            {
                return true;
            }

            AssetDescriptionFileDataType value = encodeable as AssetDescriptionFileDataType;

            if (value == null)
            {
                return false;
            }

            if (!base.IsEqual(encodeable)) return false;
            if (!Utils.IsEqual(m_modelId, value.m_modelId)) return false;
            if (!Utils.IsEqual(m_modelIdShort, value.m_modelIdShort)) return false;
            if (!Utils.IsEqual(m_submodels, value.m_submodels)) return false;

            return base.IsEqual(encodeable);
        }

        /// <summary cref="ICloneable.Clone" />
        public override object Clone()
        {
            return (AssetDescriptionFileDataType)this.MemberwiseClone();
        }

        /// <summary cref="Object.MemberwiseClone" />
        public new object MemberwiseClone()
        {
            AssetDescriptionFileDataType clone = (AssetDescriptionFileDataType)base.MemberwiseClone();

            clone.m_modelId = (string)Utils.Clone(this.m_modelId);
            clone.m_modelIdShort = (string)Utils.Clone(this.m_modelIdShort);
            clone.m_submodels = (ExtensionObjectCollection)Utils.Clone(this.m_submodels);

            return clone;
        }
        #endregion

        #region Private Fields
        private string m_modelId;
        private string m_modelIdShort;
        private ExtensionObjectCollection m_submodels;
        #endregion
    }

    #region AssetDescriptionFileDataTypeCollection Class
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    [CollectionDataContract(Name = "ListOfAssetDescriptionFileDataType", Namespace = I4AAS.Submodels.Namespaces.I4AASXsd, ItemName = "AssetDescriptionFileDataType")]
    public partial class AssetDescriptionFileDataTypeCollection : List<AssetDescriptionFileDataType>, ICloneable
    {
        #region Constructors
        /// <remarks />
        public AssetDescriptionFileDataTypeCollection() {}

        /// <remarks />
        public AssetDescriptionFileDataTypeCollection(int capacity) : base(capacity) {}

        /// <remarks />
        public AssetDescriptionFileDataTypeCollection(IEnumerable<AssetDescriptionFileDataType> collection) : base(collection) {}
        #endregion

        #region Static Operators
        /// <remarks />
        public static implicit operator AssetDescriptionFileDataTypeCollection(AssetDescriptionFileDataType[] values)
        {
            if (values != null)
            {
                return new AssetDescriptionFileDataTypeCollection(values);
            }

            return new AssetDescriptionFileDataTypeCollection();
        }

        /// <remarks />
        public static explicit operator AssetDescriptionFileDataType[](AssetDescriptionFileDataTypeCollection values)
        {
            if (values != null)
            {
                return values.ToArray();
            }

            return null;
        }
        #endregion

        #region ICloneable Methods
        /// <remarks />
        public object Clone()
        {
            return (AssetDescriptionFileDataTypeCollection)this.MemberwiseClone();
        }
        #endregion

        /// <summary cref="Object.MemberwiseClone" />
        public new object MemberwiseClone()
        {
            AssetDescriptionFileDataTypeCollection clone = new AssetDescriptionFileDataTypeCollection(this.Count);

            for (int ii = 0; ii < this.Count; ii++)
            {
                clone.Add((AssetDescriptionFileDataType)Utils.Clone(this[ii]));
            }

            return clone;
        }
    }
    #endregion
    #endif
    #endregion

    #region NameplateSubmodelDataType Class
    #if (!OPCUA_EXCLUDE_NameplateSubmodelDataType)
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    [DataContract(Namespace = I4AAS.Submodels.Namespaces.I4AASXsd)]
    public partial class NameplateSubmodelDataType : I4AAS.Submodels.SubmodelDataType
    {
        #region Constructors
        /// <remarks />
        public NameplateSubmodelDataType()
        {
            Initialize();
        }

        [OnDeserializing]
        private void Initialize(StreamingContext context)
        {
            Initialize();
        }

        private void Initialize()
        {
            m_uRIOfTheProduct = null;
            m_manufacturerName = null;
            m_manufacturerProductDesignation = null;
            m_contactInformation = new ContactInformationSubmodelDataType();
            m_manufacturerProductRoot = null;
            m_manufacturerProductFamily = null;
            m_manufacturerProductType = null;
            m_orderCodeOfManufacturer = null;
            m_productArticleNumberOfManufacturer = null;
            m_serialNumber = null;
            m_yearOfConstruction = null;
            m_dateOfManufacture = DateTime.MinValue;
            m_hardwareVersion = null;
            m_firmwareVersion = null;
            m_softwareVersion = null;
            m_countryOfOrigin = null;
            m_companyLogo = null;
            m_markings = new MarkingSubmodelDataTypeCollection();
        }
        #endregion

        #region Public Properties
        /// <remarks />
        [DataMember(Name = "URIOfTheProduct", IsRequired = false, Order = 1)]
        public string URIOfTheProduct
        {
            get { return m_uRIOfTheProduct;  }
            set { m_uRIOfTheProduct = value; }
        }

        /// <remarks />
        [DataMember(Name = "ManufacturerName", IsRequired = false, Order = 2)]
        public LocalizedText ManufacturerName
        {
            get { return m_manufacturerName;  }
            set { m_manufacturerName = value; }
        }

        /// <remarks />
        [DataMember(Name = "ManufacturerProductDesignation", IsRequired = false, Order = 3)]
        public LocalizedText ManufacturerProductDesignation
        {
            get { return m_manufacturerProductDesignation;  }
            set { m_manufacturerProductDesignation = value; }
        }

        /// <remarks />
        [DataMember(Name = "ContactInformation", IsRequired = false, Order = 4)]
        public ContactInformationSubmodelDataType ContactInformation
        {
            get
            {
                return m_contactInformation;
            }

            set
            {
                m_contactInformation = value;

                if (value == null)
                {
                    m_contactInformation = new ContactInformationSubmodelDataType();
                }
            }
        }

        /// <remarks />
        [DataMember(Name = "ManufacturerProductRoot", IsRequired = false, Order = 5)]
        public LocalizedText ManufacturerProductRoot
        {
            get { return m_manufacturerProductRoot;  }
            set { m_manufacturerProductRoot = value; }
        }

        /// <remarks />
        [DataMember(Name = "ManufacturerProductFamily", IsRequired = false, Order = 6)]
        public LocalizedText ManufacturerProductFamily
        {
            get { return m_manufacturerProductFamily;  }
            set { m_manufacturerProductFamily = value; }
        }

        /// <remarks />
        [DataMember(Name = "ManufacturerProductType", IsRequired = false, Order = 7)]
        public LocalizedText ManufacturerProductType
        {
            get { return m_manufacturerProductType;  }
            set { m_manufacturerProductType = value; }
        }

        /// <remarks />
        [DataMember(Name = "OrderCodeOfManufacturer", IsRequired = false, Order = 8)]
        public string OrderCodeOfManufacturer
        {
            get { return m_orderCodeOfManufacturer;  }
            set { m_orderCodeOfManufacturer = value; }
        }

        /// <remarks />
        [DataMember(Name = "ProductArticleNumberOfManufacturer", IsRequired = false, Order = 9)]
        public string ProductArticleNumberOfManufacturer
        {
            get { return m_productArticleNumberOfManufacturer;  }
            set { m_productArticleNumberOfManufacturer = value; }
        }

        /// <remarks />
        [DataMember(Name = "SerialNumber", IsRequired = false, Order = 10)]
        public string SerialNumber
        {
            get { return m_serialNumber;  }
            set { m_serialNumber = value; }
        }

        /// <remarks />
        [DataMember(Name = "YearOfConstruction", IsRequired = false, Order = 11)]
        public string YearOfConstruction
        {
            get { return m_yearOfConstruction;  }
            set { m_yearOfConstruction = value; }
        }

        /// <remarks />
        [DataMember(Name = "DateOfManufacture", IsRequired = false, Order = 12)]
        public DateTime DateOfManufacture
        {
            get { return m_dateOfManufacture;  }
            set { m_dateOfManufacture = value; }
        }

        /// <remarks />
        [DataMember(Name = "HardwareVersion", IsRequired = false, Order = 13)]
        public string HardwareVersion
        {
            get { return m_hardwareVersion;  }
            set { m_hardwareVersion = value; }
        }

        /// <remarks />
        [DataMember(Name = "FirmwareVersion", IsRequired = false, Order = 14)]
        public string FirmwareVersion
        {
            get { return m_firmwareVersion;  }
            set { m_firmwareVersion = value; }
        }

        /// <remarks />
        [DataMember(Name = "SoftwareVersion", IsRequired = false, Order = 15)]
        public string SoftwareVersion
        {
            get { return m_softwareVersion;  }
            set { m_softwareVersion = value; }
        }

        /// <remarks />
        [DataMember(Name = "CountryOfOrigin", IsRequired = false, Order = 16)]
        public string CountryOfOrigin
        {
            get { return m_countryOfOrigin;  }
            set { m_countryOfOrigin = value; }
        }

        /// <remarks />
        [DataMember(Name = "CompanyLogo", IsRequired = false, Order = 17)]
        public string CompanyLogo
        {
            get { return m_companyLogo;  }
            set { m_companyLogo = value; }
        }

        /// <remarks />
        [DataMember(Name = "Markings", IsRequired = false, Order = 18)]
        public MarkingSubmodelDataTypeCollection Markings
        {
            get
            {
                return m_markings;
            }

            set
            {
                m_markings = value;

                if (value == null)
                {
                    m_markings = new MarkingSubmodelDataTypeCollection();
                }
            }
        }
        #endregion

        #region IEncodeable Members
        /// <summary cref="IEncodeable.TypeId" />
        public override ExpandedNodeId TypeId => DataTypeIds.NameplateSubmodelDataType;

        /// <summary cref="IEncodeable.BinaryEncodingId" />
        public override ExpandedNodeId BinaryEncodingId => ObjectIds.NameplateSubmodelDataType_Encoding_DefaultBinary;

        /// <summary cref="IEncodeable.XmlEncodingId" />
        public override ExpandedNodeId XmlEncodingId => ObjectIds.NameplateSubmodelDataType_Encoding_DefaultXml;

        /// <summary cref="IJsonEncodeable.JsonEncodingId" />
        public override ExpandedNodeId JsonEncodingId => ObjectIds.NameplateSubmodelDataType_Encoding_DefaultJson;

        /// <summary cref="IEncodeable.Encode(IEncoder)" />
        public override void Encode(IEncoder encoder)
        {
            base.Encode(encoder);

            encoder.PushNamespace(I4AAS.Submodels.Namespaces.I4AASXsd);

            encoder.WriteString("URIOfTheProduct", URIOfTheProduct);
            encoder.WriteLocalizedText("ManufacturerName", ManufacturerName);
            encoder.WriteLocalizedText("ManufacturerProductDesignation", ManufacturerProductDesignation);
            encoder.WriteEncodeable("ContactInformation", ContactInformation, typeof(ContactInformationSubmodelDataType));
            encoder.WriteLocalizedText("ManufacturerProductRoot", ManufacturerProductRoot);
            encoder.WriteLocalizedText("ManufacturerProductFamily", ManufacturerProductFamily);
            encoder.WriteLocalizedText("ManufacturerProductType", ManufacturerProductType);
            encoder.WriteString("OrderCodeOfManufacturer", OrderCodeOfManufacturer);
            encoder.WriteString("ProductArticleNumberOfManufacturer", ProductArticleNumberOfManufacturer);
            encoder.WriteString("SerialNumber", SerialNumber);
            encoder.WriteString("YearOfConstruction", YearOfConstruction);
            encoder.WriteDateTime("DateOfManufacture", DateOfManufacture);
            encoder.WriteString("HardwareVersion", HardwareVersion);
            encoder.WriteString("FirmwareVersion", FirmwareVersion);
            encoder.WriteString("SoftwareVersion", SoftwareVersion);
            encoder.WriteString("CountryOfOrigin", CountryOfOrigin);
            encoder.WriteString("CompanyLogo", CompanyLogo);
            encoder.WriteEncodeableArray("Markings", Markings.ToArray(), typeof(MarkingSubmodelDataType));

            encoder.PopNamespace();
        }

        /// <summary cref="IEncodeable.Decode(IDecoder)" />
        public override void Decode(IDecoder decoder)
        {
            base.Decode(decoder);

            decoder.PushNamespace(I4AAS.Submodels.Namespaces.I4AASXsd);

            URIOfTheProduct = decoder.ReadString("URIOfTheProduct");
            ManufacturerName = decoder.ReadLocalizedText("ManufacturerName");
            ManufacturerProductDesignation = decoder.ReadLocalizedText("ManufacturerProductDesignation");
            ContactInformation = (ContactInformationSubmodelDataType)decoder.ReadEncodeable("ContactInformation", typeof(ContactInformationSubmodelDataType));
            ManufacturerProductRoot = decoder.ReadLocalizedText("ManufacturerProductRoot");
            ManufacturerProductFamily = decoder.ReadLocalizedText("ManufacturerProductFamily");
            ManufacturerProductType = decoder.ReadLocalizedText("ManufacturerProductType");
            OrderCodeOfManufacturer = decoder.ReadString("OrderCodeOfManufacturer");
            ProductArticleNumberOfManufacturer = decoder.ReadString("ProductArticleNumberOfManufacturer");
            SerialNumber = decoder.ReadString("SerialNumber");
            YearOfConstruction = decoder.ReadString("YearOfConstruction");
            DateOfManufacture = decoder.ReadDateTime("DateOfManufacture");
            HardwareVersion = decoder.ReadString("HardwareVersion");
            FirmwareVersion = decoder.ReadString("FirmwareVersion");
            SoftwareVersion = decoder.ReadString("SoftwareVersion");
            CountryOfOrigin = decoder.ReadString("CountryOfOrigin");
            CompanyLogo = decoder.ReadString("CompanyLogo");
            Markings = (MarkingSubmodelDataTypeCollection)decoder.ReadEncodeableArray("Markings", typeof(MarkingSubmodelDataType));

            decoder.PopNamespace();
        }

        /// <summary cref="IEncodeable.IsEqual(IEncodeable)" />
        public override bool IsEqual(IEncodeable encodeable)
        {
            if (Object.ReferenceEquals(this, encodeable))
            {
                return true;
            }

            NameplateSubmodelDataType value = encodeable as NameplateSubmodelDataType;

            if (value == null)
            {
                return false;
            }

            if (!base.IsEqual(encodeable)) return false;
            if (!Utils.IsEqual(m_uRIOfTheProduct, value.m_uRIOfTheProduct)) return false;
            if (!Utils.IsEqual(m_manufacturerName, value.m_manufacturerName)) return false;
            if (!Utils.IsEqual(m_manufacturerProductDesignation, value.m_manufacturerProductDesignation)) return false;
            if (!Utils.IsEqual(m_contactInformation, value.m_contactInformation)) return false;
            if (!Utils.IsEqual(m_manufacturerProductRoot, value.m_manufacturerProductRoot)) return false;
            if (!Utils.IsEqual(m_manufacturerProductFamily, value.m_manufacturerProductFamily)) return false;
            if (!Utils.IsEqual(m_manufacturerProductType, value.m_manufacturerProductType)) return false;
            if (!Utils.IsEqual(m_orderCodeOfManufacturer, value.m_orderCodeOfManufacturer)) return false;
            if (!Utils.IsEqual(m_productArticleNumberOfManufacturer, value.m_productArticleNumberOfManufacturer)) return false;
            if (!Utils.IsEqual(m_serialNumber, value.m_serialNumber)) return false;
            if (!Utils.IsEqual(m_yearOfConstruction, value.m_yearOfConstruction)) return false;
            if (!Utils.IsEqual(m_dateOfManufacture, value.m_dateOfManufacture)) return false;
            if (!Utils.IsEqual(m_hardwareVersion, value.m_hardwareVersion)) return false;
            if (!Utils.IsEqual(m_firmwareVersion, value.m_firmwareVersion)) return false;
            if (!Utils.IsEqual(m_softwareVersion, value.m_softwareVersion)) return false;
            if (!Utils.IsEqual(m_countryOfOrigin, value.m_countryOfOrigin)) return false;
            if (!Utils.IsEqual(m_companyLogo, value.m_companyLogo)) return false;
            if (!Utils.IsEqual(m_markings, value.m_markings)) return false;

            return base.IsEqual(encodeable);
        }

        /// <summary cref="ICloneable.Clone" />
        public override object Clone()
        {
            return (NameplateSubmodelDataType)this.MemberwiseClone();
        }

        /// <summary cref="Object.MemberwiseClone" />
        public new object MemberwiseClone()
        {
            NameplateSubmodelDataType clone = (NameplateSubmodelDataType)base.MemberwiseClone();

            clone.m_uRIOfTheProduct = (string)Utils.Clone(this.m_uRIOfTheProduct);
            clone.m_manufacturerName = (LocalizedText)Utils.Clone(this.m_manufacturerName);
            clone.m_manufacturerProductDesignation = (LocalizedText)Utils.Clone(this.m_manufacturerProductDesignation);
            clone.m_contactInformation = (ContactInformationSubmodelDataType)Utils.Clone(this.m_contactInformation);
            clone.m_manufacturerProductRoot = (LocalizedText)Utils.Clone(this.m_manufacturerProductRoot);
            clone.m_manufacturerProductFamily = (LocalizedText)Utils.Clone(this.m_manufacturerProductFamily);
            clone.m_manufacturerProductType = (LocalizedText)Utils.Clone(this.m_manufacturerProductType);
            clone.m_orderCodeOfManufacturer = (string)Utils.Clone(this.m_orderCodeOfManufacturer);
            clone.m_productArticleNumberOfManufacturer = (string)Utils.Clone(this.m_productArticleNumberOfManufacturer);
            clone.m_serialNumber = (string)Utils.Clone(this.m_serialNumber);
            clone.m_yearOfConstruction = (string)Utils.Clone(this.m_yearOfConstruction);
            clone.m_dateOfManufacture = (DateTime)Utils.Clone(this.m_dateOfManufacture);
            clone.m_hardwareVersion = (string)Utils.Clone(this.m_hardwareVersion);
            clone.m_firmwareVersion = (string)Utils.Clone(this.m_firmwareVersion);
            clone.m_softwareVersion = (string)Utils.Clone(this.m_softwareVersion);
            clone.m_countryOfOrigin = (string)Utils.Clone(this.m_countryOfOrigin);
            clone.m_companyLogo = (string)Utils.Clone(this.m_companyLogo);
            clone.m_markings = (MarkingSubmodelDataTypeCollection)Utils.Clone(this.m_markings);

            return clone;
        }
        #endregion

        #region Private Fields
        private string m_uRIOfTheProduct;
        private LocalizedText m_manufacturerName;
        private LocalizedText m_manufacturerProductDesignation;
        private ContactInformationSubmodelDataType m_contactInformation;
        private LocalizedText m_manufacturerProductRoot;
        private LocalizedText m_manufacturerProductFamily;
        private LocalizedText m_manufacturerProductType;
        private string m_orderCodeOfManufacturer;
        private string m_productArticleNumberOfManufacturer;
        private string m_serialNumber;
        private string m_yearOfConstruction;
        private DateTime m_dateOfManufacture;
        private string m_hardwareVersion;
        private string m_firmwareVersion;
        private string m_softwareVersion;
        private string m_countryOfOrigin;
        private string m_companyLogo;
        private MarkingSubmodelDataTypeCollection m_markings;
        #endregion
    }

    #region NameplateSubmodelDataTypeCollection Class
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    [CollectionDataContract(Name = "ListOfNameplateSubmodelDataType", Namespace = I4AAS.Submodels.Namespaces.I4AASXsd, ItemName = "NameplateSubmodelDataType")]
    public partial class NameplateSubmodelDataTypeCollection : List<NameplateSubmodelDataType>, ICloneable
    {
        #region Constructors
        /// <remarks />
        public NameplateSubmodelDataTypeCollection() {}

        /// <remarks />
        public NameplateSubmodelDataTypeCollection(int capacity) : base(capacity) {}

        /// <remarks />
        public NameplateSubmodelDataTypeCollection(IEnumerable<NameplateSubmodelDataType> collection) : base(collection) {}
        #endregion

        #region Static Operators
        /// <remarks />
        public static implicit operator NameplateSubmodelDataTypeCollection(NameplateSubmodelDataType[] values)
        {
            if (values != null)
            {
                return new NameplateSubmodelDataTypeCollection(values);
            }

            return new NameplateSubmodelDataTypeCollection();
        }

        /// <remarks />
        public static explicit operator NameplateSubmodelDataType[](NameplateSubmodelDataTypeCollection values)
        {
            if (values != null)
            {
                return values.ToArray();
            }

            return null;
        }
        #endregion

        #region ICloneable Methods
        /// <remarks />
        public object Clone()
        {
            return (NameplateSubmodelDataTypeCollection)this.MemberwiseClone();
        }
        #endregion

        /// <summary cref="Object.MemberwiseClone" />
        public new object MemberwiseClone()
        {
            NameplateSubmodelDataTypeCollection clone = new NameplateSubmodelDataTypeCollection(this.Count);

            for (int ii = 0; ii < this.Count; ii++)
            {
                clone.Add((NameplateSubmodelDataType)Utils.Clone(this[ii]));
            }

            return clone;
        }
    }
    #endregion
    #endif
    #endregion

    #region ContactInformationSubmodelDataType Class
    #if (!OPCUA_EXCLUDE_ContactInformationSubmodelDataType)
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    [DataContract(Namespace = I4AAS.Submodels.Namespaces.I4AASXsd)]
    public partial class ContactInformationSubmodelDataType : I4AAS.Submodels.SubmodelDataType
    {
        #region Constructors
        /// <remarks />
        public ContactInformationSubmodelDataType()
        {
            Initialize();
        }

        [OnDeserializing]
        private void Initialize(StreamingContext context)
        {
            Initialize();
        }

        private void Initialize()
        {
            m_street = null;
            m_zipcode = null;
            m_cityTown = null;
            m_nationalCode = null;
        }
        #endregion

        #region Public Properties
        /// <remarks />
        [DataMember(Name = "Street", IsRequired = false, Order = 1)]
        public LocalizedText Street
        {
            get { return m_street;  }
            set { m_street = value; }
        }

        /// <remarks />
        [DataMember(Name = "Zipcode", IsRequired = false, Order = 2)]
        public string Zipcode
        {
            get { return m_zipcode;  }
            set { m_zipcode = value; }
        }

        /// <remarks />
        [DataMember(Name = "CityTown", IsRequired = false, Order = 3)]
        public LocalizedText CityTown
        {
            get { return m_cityTown;  }
            set { m_cityTown = value; }
        }

        /// <remarks />
        [DataMember(Name = "NationalCode", IsRequired = false, Order = 4)]
        public string NationalCode
        {
            get { return m_nationalCode;  }
            set { m_nationalCode = value; }
        }
        #endregion

        #region IEncodeable Members
        /// <summary cref="IEncodeable.TypeId" />
        public override ExpandedNodeId TypeId => DataTypeIds.ContactInformationSubmodelDataType;

        /// <summary cref="IEncodeable.BinaryEncodingId" />
        public override ExpandedNodeId BinaryEncodingId => ObjectIds.ContactInformationSubmodelDataType_Encoding_DefaultBinary;

        /// <summary cref="IEncodeable.XmlEncodingId" />
        public override ExpandedNodeId XmlEncodingId => ObjectIds.ContactInformationSubmodelDataType_Encoding_DefaultXml;

        /// <summary cref="IJsonEncodeable.JsonEncodingId" />
        public override ExpandedNodeId JsonEncodingId => ObjectIds.ContactInformationSubmodelDataType_Encoding_DefaultJson;

        /// <summary cref="IEncodeable.Encode(IEncoder)" />
        public override void Encode(IEncoder encoder)
        {
            base.Encode(encoder);

            encoder.PushNamespace(I4AAS.Submodels.Namespaces.I4AASXsd);

            encoder.WriteLocalizedText("Street", Street);
            encoder.WriteString("Zipcode", Zipcode);
            encoder.WriteLocalizedText("CityTown", CityTown);
            encoder.WriteString("NationalCode", NationalCode);

            encoder.PopNamespace();
        }

        /// <summary cref="IEncodeable.Decode(IDecoder)" />
        public override void Decode(IDecoder decoder)
        {
            base.Decode(decoder);

            decoder.PushNamespace(I4AAS.Submodels.Namespaces.I4AASXsd);

            Street = decoder.ReadLocalizedText("Street");
            Zipcode = decoder.ReadString("Zipcode");
            CityTown = decoder.ReadLocalizedText("CityTown");
            NationalCode = decoder.ReadString("NationalCode");

            decoder.PopNamespace();
        }

        /// <summary cref="IEncodeable.IsEqual(IEncodeable)" />
        public override bool IsEqual(IEncodeable encodeable)
        {
            if (Object.ReferenceEquals(this, encodeable))
            {
                return true;
            }

            ContactInformationSubmodelDataType value = encodeable as ContactInformationSubmodelDataType;

            if (value == null)
            {
                return false;
            }

            if (!base.IsEqual(encodeable)) return false;
            if (!Utils.IsEqual(m_street, value.m_street)) return false;
            if (!Utils.IsEqual(m_zipcode, value.m_zipcode)) return false;
            if (!Utils.IsEqual(m_cityTown, value.m_cityTown)) return false;
            if (!Utils.IsEqual(m_nationalCode, value.m_nationalCode)) return false;

            return base.IsEqual(encodeable);
        }

        /// <summary cref="ICloneable.Clone" />
        public override object Clone()
        {
            return (ContactInformationSubmodelDataType)this.MemberwiseClone();
        }

        /// <summary cref="Object.MemberwiseClone" />
        public new object MemberwiseClone()
        {
            ContactInformationSubmodelDataType clone = (ContactInformationSubmodelDataType)base.MemberwiseClone();

            clone.m_street = (LocalizedText)Utils.Clone(this.m_street);
            clone.m_zipcode = (string)Utils.Clone(this.m_zipcode);
            clone.m_cityTown = (LocalizedText)Utils.Clone(this.m_cityTown);
            clone.m_nationalCode = (string)Utils.Clone(this.m_nationalCode);

            return clone;
        }
        #endregion

        #region Private Fields
        private LocalizedText m_street;
        private string m_zipcode;
        private LocalizedText m_cityTown;
        private string m_nationalCode;
        #endregion
    }

    #region ContactInformationSubmodelDataTypeCollection Class
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    [CollectionDataContract(Name = "ListOfContactInformationSubmodelDataType", Namespace = I4AAS.Submodels.Namespaces.I4AASXsd, ItemName = "ContactInformationSubmodelDataType")]
    public partial class ContactInformationSubmodelDataTypeCollection : List<ContactInformationSubmodelDataType>, ICloneable
    {
        #region Constructors
        /// <remarks />
        public ContactInformationSubmodelDataTypeCollection() {}

        /// <remarks />
        public ContactInformationSubmodelDataTypeCollection(int capacity) : base(capacity) {}

        /// <remarks />
        public ContactInformationSubmodelDataTypeCollection(IEnumerable<ContactInformationSubmodelDataType> collection) : base(collection) {}
        #endregion

        #region Static Operators
        /// <remarks />
        public static implicit operator ContactInformationSubmodelDataTypeCollection(ContactInformationSubmodelDataType[] values)
        {
            if (values != null)
            {
                return new ContactInformationSubmodelDataTypeCollection(values);
            }

            return new ContactInformationSubmodelDataTypeCollection();
        }

        /// <remarks />
        public static explicit operator ContactInformationSubmodelDataType[](ContactInformationSubmodelDataTypeCollection values)
        {
            if (values != null)
            {
                return values.ToArray();
            }

            return null;
        }
        #endregion

        #region ICloneable Methods
        /// <remarks />
        public object Clone()
        {
            return (ContactInformationSubmodelDataTypeCollection)this.MemberwiseClone();
        }
        #endregion

        /// <summary cref="Object.MemberwiseClone" />
        public new object MemberwiseClone()
        {
            ContactInformationSubmodelDataTypeCollection clone = new ContactInformationSubmodelDataTypeCollection(this.Count);

            for (int ii = 0; ii < this.Count; ii++)
            {
                clone.Add((ContactInformationSubmodelDataType)Utils.Clone(this[ii]));
            }

            return clone;
        }
    }
    #endregion
    #endif
    #endregion

    #region MarkingSubmodelDataType Class
    #if (!OPCUA_EXCLUDE_MarkingSubmodelDataType)
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    [DataContract(Namespace = I4AAS.Submodels.Namespaces.I4AASXsd)]
    public partial class MarkingSubmodelDataType : I4AAS.Submodels.SubmodelDataType
    {
        #region Constructors
        /// <remarks />
        public MarkingSubmodelDataType()
        {
            Initialize();
        }

        [OnDeserializing]
        private void Initialize(StreamingContext context)
        {
            Initialize();
        }

        private void Initialize()
        {
            m_markingName = null;
            m_designationOfCertificateOrApproval = null;
            m_issueDate = DateTime.MinValue;
            m_expiryDate = DateTime.MinValue;
            m_markingFile = null;
            m_markingAdditionalText = new StringCollection();
        }
        #endregion

        #region Public Properties
        /// <remarks />
        [DataMember(Name = "MarkingName", IsRequired = false, Order = 1)]
        public string MarkingName
        {
            get { return m_markingName;  }
            set { m_markingName = value; }
        }

        /// <remarks />
        [DataMember(Name = "DesignationOfCertificateOrApproval", IsRequired = false, Order = 2)]
        public string DesignationOfCertificateOrApproval
        {
            get { return m_designationOfCertificateOrApproval;  }
            set { m_designationOfCertificateOrApproval = value; }
        }

        /// <remarks />
        [DataMember(Name = "IssueDate", IsRequired = false, Order = 3)]
        public DateTime IssueDate
        {
            get { return m_issueDate;  }
            set { m_issueDate = value; }
        }

        /// <remarks />
        [DataMember(Name = "ExpiryDate", IsRequired = false, Order = 4)]
        public DateTime ExpiryDate
        {
            get { return m_expiryDate;  }
            set { m_expiryDate = value; }
        }

        /// <remarks />
        [DataMember(Name = "MarkingFile", IsRequired = false, Order = 5)]
        public string MarkingFile
        {
            get { return m_markingFile;  }
            set { m_markingFile = value; }
        }

        /// <remarks />
        [DataMember(Name = "MarkingAdditionalText", IsRequired = false, Order = 6)]
        public StringCollection MarkingAdditionalText
        {
            get
            {
                return m_markingAdditionalText;
            }

            set
            {
                m_markingAdditionalText = value;

                if (value == null)
                {
                    m_markingAdditionalText = new StringCollection();
                }
            }
        }
        #endregion

        #region IEncodeable Members
        /// <summary cref="IEncodeable.TypeId" />
        public override ExpandedNodeId TypeId => DataTypeIds.MarkingSubmodelDataType;

        /// <summary cref="IEncodeable.BinaryEncodingId" />
        public override ExpandedNodeId BinaryEncodingId => ObjectIds.MarkingSubmodelDataType_Encoding_DefaultBinary;

        /// <summary cref="IEncodeable.XmlEncodingId" />
        public override ExpandedNodeId XmlEncodingId => ObjectIds.MarkingSubmodelDataType_Encoding_DefaultXml;

        /// <summary cref="IJsonEncodeable.JsonEncodingId" />
        public override ExpandedNodeId JsonEncodingId => ObjectIds.MarkingSubmodelDataType_Encoding_DefaultJson;

        /// <summary cref="IEncodeable.Encode(IEncoder)" />
        public override void Encode(IEncoder encoder)
        {
            base.Encode(encoder);

            encoder.PushNamespace(I4AAS.Submodels.Namespaces.I4AASXsd);

            encoder.WriteString("MarkingName", MarkingName);
            encoder.WriteString("DesignationOfCertificateOrApproval", DesignationOfCertificateOrApproval);
            encoder.WriteDateTime("IssueDate", IssueDate);
            encoder.WriteDateTime("ExpiryDate", ExpiryDate);
            encoder.WriteString("MarkingFile", MarkingFile);
            encoder.WriteStringArray("MarkingAdditionalText", MarkingAdditionalText);

            encoder.PopNamespace();
        }

        /// <summary cref="IEncodeable.Decode(IDecoder)" />
        public override void Decode(IDecoder decoder)
        {
            base.Decode(decoder);

            decoder.PushNamespace(I4AAS.Submodels.Namespaces.I4AASXsd);

            MarkingName = decoder.ReadString("MarkingName");
            DesignationOfCertificateOrApproval = decoder.ReadString("DesignationOfCertificateOrApproval");
            IssueDate = decoder.ReadDateTime("IssueDate");
            ExpiryDate = decoder.ReadDateTime("ExpiryDate");
            MarkingFile = decoder.ReadString("MarkingFile");
            MarkingAdditionalText = decoder.ReadStringArray("MarkingAdditionalText");

            decoder.PopNamespace();
        }

        /// <summary cref="IEncodeable.IsEqual(IEncodeable)" />
        public override bool IsEqual(IEncodeable encodeable)
        {
            if (Object.ReferenceEquals(this, encodeable))
            {
                return true;
            }

            MarkingSubmodelDataType value = encodeable as MarkingSubmodelDataType;

            if (value == null)
            {
                return false;
            }

            if (!base.IsEqual(encodeable)) return false;
            if (!Utils.IsEqual(m_markingName, value.m_markingName)) return false;
            if (!Utils.IsEqual(m_designationOfCertificateOrApproval, value.m_designationOfCertificateOrApproval)) return false;
            if (!Utils.IsEqual(m_issueDate, value.m_issueDate)) return false;
            if (!Utils.IsEqual(m_expiryDate, value.m_expiryDate)) return false;
            if (!Utils.IsEqual(m_markingFile, value.m_markingFile)) return false;
            if (!Utils.IsEqual(m_markingAdditionalText, value.m_markingAdditionalText)) return false;

            return base.IsEqual(encodeable);
        }

        /// <summary cref="ICloneable.Clone" />
        public override object Clone()
        {
            return (MarkingSubmodelDataType)this.MemberwiseClone();
        }

        /// <summary cref="Object.MemberwiseClone" />
        public new object MemberwiseClone()
        {
            MarkingSubmodelDataType clone = (MarkingSubmodelDataType)base.MemberwiseClone();

            clone.m_markingName = (string)Utils.Clone(this.m_markingName);
            clone.m_designationOfCertificateOrApproval = (string)Utils.Clone(this.m_designationOfCertificateOrApproval);
            clone.m_issueDate = (DateTime)Utils.Clone(this.m_issueDate);
            clone.m_expiryDate = (DateTime)Utils.Clone(this.m_expiryDate);
            clone.m_markingFile = (string)Utils.Clone(this.m_markingFile);
            clone.m_markingAdditionalText = (StringCollection)Utils.Clone(this.m_markingAdditionalText);

            return clone;
        }
        #endregion

        #region Private Fields
        private string m_markingName;
        private string m_designationOfCertificateOrApproval;
        private DateTime m_issueDate;
        private DateTime m_expiryDate;
        private string m_markingFile;
        private StringCollection m_markingAdditionalText;
        #endregion
    }

    #region MarkingSubmodelDataTypeCollection Class
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    [CollectionDataContract(Name = "ListOfMarkingSubmodelDataType", Namespace = I4AAS.Submodels.Namespaces.I4AASXsd, ItemName = "MarkingSubmodelDataType")]
    public partial class MarkingSubmodelDataTypeCollection : List<MarkingSubmodelDataType>, ICloneable
    {
        #region Constructors
        /// <remarks />
        public MarkingSubmodelDataTypeCollection() {}

        /// <remarks />
        public MarkingSubmodelDataTypeCollection(int capacity) : base(capacity) {}

        /// <remarks />
        public MarkingSubmodelDataTypeCollection(IEnumerable<MarkingSubmodelDataType> collection) : base(collection) {}
        #endregion

        #region Static Operators
        /// <remarks />
        public static implicit operator MarkingSubmodelDataTypeCollection(MarkingSubmodelDataType[] values)
        {
            if (values != null)
            {
                return new MarkingSubmodelDataTypeCollection(values);
            }

            return new MarkingSubmodelDataTypeCollection();
        }

        /// <remarks />
        public static explicit operator MarkingSubmodelDataType[](MarkingSubmodelDataTypeCollection values)
        {
            if (values != null)
            {
                return values.ToArray();
            }

            return null;
        }
        #endregion

        #region ICloneable Methods
        /// <remarks />
        public object Clone()
        {
            return (MarkingSubmodelDataTypeCollection)this.MemberwiseClone();
        }
        #endregion

        /// <summary cref="Object.MemberwiseClone" />
        public new object MemberwiseClone()
        {
            MarkingSubmodelDataTypeCollection clone = new MarkingSubmodelDataTypeCollection(this.Count);

            for (int ii = 0; ii < this.Count; ii++)
            {
                clone.Add((MarkingSubmodelDataType)Utils.Clone(this[ii]));
            }

            return clone;
        }
    }
    #endregion
    #endif
    #endregion

    #region ProductCarbonFootprintDataType Class
    #if (!OPCUA_EXCLUDE_ProductCarbonFootprintDataType)
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    [DataContract(Namespace = I4AAS.Submodels.Namespaces.I4AASXsd)]
    public partial class ProductCarbonFootprintDataType : I4AAS.Submodels.SubmodelDataType
    {
        #region Constructors
        /// <remarks />
        public ProductCarbonFootprintDataType()
        {
            Initialize();
        }

        [OnDeserializing]
        private void Initialize(StreamingContext context)
        {
            Initialize();
        }

        private void Initialize()
        {
            m_pCFCalculationMethod = null;
            m_pCFCO2eq = (double)0;
            m_pCFReferenceValueForCalculation = null;
            m_pCFQuantityOfMeasureForCalculation = (double)0;
            m_pCFLiveCyclePhase = null;
            m_pCFGoodsAddressHandover = new AddressSubmodelDataType();
        }
        #endregion

        #region Public Properties
        /// <remarks />
        [DataMember(Name = "PCFCalculationMethod", IsRequired = false, Order = 1)]
        public string PCFCalculationMethod
        {
            get { return m_pCFCalculationMethod;  }
            set { m_pCFCalculationMethod = value; }
        }

        /// <remarks />
        [DataMember(Name = "PCFCO2eq", IsRequired = false, Order = 2)]
        public double PCFCO2eq
        {
            get { return m_pCFCO2eq;  }
            set { m_pCFCO2eq = value; }
        }

        /// <remarks />
        [DataMember(Name = "PCFReferenceValueForCalculation", IsRequired = false, Order = 3)]
        public string PCFReferenceValueForCalculation
        {
            get { return m_pCFReferenceValueForCalculation;  }
            set { m_pCFReferenceValueForCalculation = value; }
        }

        /// <remarks />
        [DataMember(Name = "PCFQuantityOfMeasureForCalculation", IsRequired = false, Order = 4)]
        public double PCFQuantityOfMeasureForCalculation
        {
            get { return m_pCFQuantityOfMeasureForCalculation;  }
            set { m_pCFQuantityOfMeasureForCalculation = value; }
        }

        /// <remarks />
        [DataMember(Name = "PCFLiveCyclePhase", IsRequired = false, Order = 5)]
        public string PCFLiveCyclePhase
        {
            get { return m_pCFLiveCyclePhase;  }
            set { m_pCFLiveCyclePhase = value; }
        }

        /// <remarks />
        [DataMember(Name = "PCFGoodsAddressHandover", IsRequired = false, Order = 6)]
        public AddressSubmodelDataType PCFGoodsAddressHandover
        {
            get
            {
                return m_pCFGoodsAddressHandover;
            }

            set
            {
                m_pCFGoodsAddressHandover = value;

                if (value == null)
                {
                    m_pCFGoodsAddressHandover = new AddressSubmodelDataType();
                }
            }
        }
        #endregion

        #region IEncodeable Members
        /// <summary cref="IEncodeable.TypeId" />
        public override ExpandedNodeId TypeId => DataTypeIds.ProductCarbonFootprintDataType;

        /// <summary cref="IEncodeable.BinaryEncodingId" />
        public override ExpandedNodeId BinaryEncodingId => ObjectIds.ProductCarbonFootprintDataType_Encoding_DefaultBinary;

        /// <summary cref="IEncodeable.XmlEncodingId" />
        public override ExpandedNodeId XmlEncodingId => ObjectIds.ProductCarbonFootprintDataType_Encoding_DefaultXml;

        /// <summary cref="IJsonEncodeable.JsonEncodingId" />
        public override ExpandedNodeId JsonEncodingId => ObjectIds.ProductCarbonFootprintDataType_Encoding_DefaultJson;

        /// <summary cref="IEncodeable.Encode(IEncoder)" />
        public override void Encode(IEncoder encoder)
        {
            base.Encode(encoder);

            encoder.PushNamespace(I4AAS.Submodels.Namespaces.I4AASXsd);

            encoder.WriteString("PCFCalculationMethod", PCFCalculationMethod);
            encoder.WriteDouble("PCFCO2eq", PCFCO2eq);
            encoder.WriteString("PCFReferenceValueForCalculation", PCFReferenceValueForCalculation);
            encoder.WriteDouble("PCFQuantityOfMeasureForCalculation", PCFQuantityOfMeasureForCalculation);
            encoder.WriteString("PCFLiveCyclePhase", PCFLiveCyclePhase);
            encoder.WriteEncodeable("PCFGoodsAddressHandover", PCFGoodsAddressHandover, typeof(AddressSubmodelDataType));

            encoder.PopNamespace();
        }

        /// <summary cref="IEncodeable.Decode(IDecoder)" />
        public override void Decode(IDecoder decoder)
        {
            base.Decode(decoder);

            decoder.PushNamespace(I4AAS.Submodels.Namespaces.I4AASXsd);

            PCFCalculationMethod = decoder.ReadString("PCFCalculationMethod");
            PCFCO2eq = decoder.ReadDouble("PCFCO2eq");
            PCFReferenceValueForCalculation = decoder.ReadString("PCFReferenceValueForCalculation");
            PCFQuantityOfMeasureForCalculation = decoder.ReadDouble("PCFQuantityOfMeasureForCalculation");
            PCFLiveCyclePhase = decoder.ReadString("PCFLiveCyclePhase");
            PCFGoodsAddressHandover = (AddressSubmodelDataType)decoder.ReadEncodeable("PCFGoodsAddressHandover", typeof(AddressSubmodelDataType));

            decoder.PopNamespace();
        }

        /// <summary cref="IEncodeable.IsEqual(IEncodeable)" />
        public override bool IsEqual(IEncodeable encodeable)
        {
            if (Object.ReferenceEquals(this, encodeable))
            {
                return true;
            }

            ProductCarbonFootprintDataType value = encodeable as ProductCarbonFootprintDataType;

            if (value == null)
            {
                return false;
            }

            if (!base.IsEqual(encodeable)) return false;
            if (!Utils.IsEqual(m_pCFCalculationMethod, value.m_pCFCalculationMethod)) return false;
            if (!Utils.IsEqual(m_pCFCO2eq, value.m_pCFCO2eq)) return false;
            if (!Utils.IsEqual(m_pCFReferenceValueForCalculation, value.m_pCFReferenceValueForCalculation)) return false;
            if (!Utils.IsEqual(m_pCFQuantityOfMeasureForCalculation, value.m_pCFQuantityOfMeasureForCalculation)) return false;
            if (!Utils.IsEqual(m_pCFLiveCyclePhase, value.m_pCFLiveCyclePhase)) return false;
            if (!Utils.IsEqual(m_pCFGoodsAddressHandover, value.m_pCFGoodsAddressHandover)) return false;

            return base.IsEqual(encodeable);
        }

        /// <summary cref="ICloneable.Clone" />
        public override object Clone()
        {
            return (ProductCarbonFootprintDataType)this.MemberwiseClone();
        }

        /// <summary cref="Object.MemberwiseClone" />
        public new object MemberwiseClone()
        {
            ProductCarbonFootprintDataType clone = (ProductCarbonFootprintDataType)base.MemberwiseClone();

            clone.m_pCFCalculationMethod = (string)Utils.Clone(this.m_pCFCalculationMethod);
            clone.m_pCFCO2eq = (double)Utils.Clone(this.m_pCFCO2eq);
            clone.m_pCFReferenceValueForCalculation = (string)Utils.Clone(this.m_pCFReferenceValueForCalculation);
            clone.m_pCFQuantityOfMeasureForCalculation = (double)Utils.Clone(this.m_pCFQuantityOfMeasureForCalculation);
            clone.m_pCFLiveCyclePhase = (string)Utils.Clone(this.m_pCFLiveCyclePhase);
            clone.m_pCFGoodsAddressHandover = (AddressSubmodelDataType)Utils.Clone(this.m_pCFGoodsAddressHandover);

            return clone;
        }
        #endregion

        #region Private Fields
        private string m_pCFCalculationMethod;
        private double m_pCFCO2eq;
        private string m_pCFReferenceValueForCalculation;
        private double m_pCFQuantityOfMeasureForCalculation;
        private string m_pCFLiveCyclePhase;
        private AddressSubmodelDataType m_pCFGoodsAddressHandover;
        #endregion
    }

    #region ProductCarbonFootprintDataTypeCollection Class
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    [CollectionDataContract(Name = "ListOfProductCarbonFootprintDataType", Namespace = I4AAS.Submodels.Namespaces.I4AASXsd, ItemName = "ProductCarbonFootprintDataType")]
    public partial class ProductCarbonFootprintDataTypeCollection : List<ProductCarbonFootprintDataType>, ICloneable
    {
        #region Constructors
        /// <remarks />
        public ProductCarbonFootprintDataTypeCollection() {}

        /// <remarks />
        public ProductCarbonFootprintDataTypeCollection(int capacity) : base(capacity) {}

        /// <remarks />
        public ProductCarbonFootprintDataTypeCollection(IEnumerable<ProductCarbonFootprintDataType> collection) : base(collection) {}
        #endregion

        #region Static Operators
        /// <remarks />
        public static implicit operator ProductCarbonFootprintDataTypeCollection(ProductCarbonFootprintDataType[] values)
        {
            if (values != null)
            {
                return new ProductCarbonFootprintDataTypeCollection(values);
            }

            return new ProductCarbonFootprintDataTypeCollection();
        }

        /// <remarks />
        public static explicit operator ProductCarbonFootprintDataType[](ProductCarbonFootprintDataTypeCollection values)
        {
            if (values != null)
            {
                return values.ToArray();
            }

            return null;
        }
        #endregion

        #region ICloneable Methods
        /// <remarks />
        public object Clone()
        {
            return (ProductCarbonFootprintDataTypeCollection)this.MemberwiseClone();
        }
        #endregion

        /// <summary cref="Object.MemberwiseClone" />
        public new object MemberwiseClone()
        {
            ProductCarbonFootprintDataTypeCollection clone = new ProductCarbonFootprintDataTypeCollection(this.Count);

            for (int ii = 0; ii < this.Count; ii++)
            {
                clone.Add((ProductCarbonFootprintDataType)Utils.Clone(this[ii]));
            }

            return clone;
        }
    }
    #endregion
    #endif
    #endregion

    #region AddressSubmodelDataType Class
    #if (!OPCUA_EXCLUDE_AddressSubmodelDataType)
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    [DataContract(Namespace = I4AAS.Submodels.Namespaces.I4AASXsd)]
    public partial class AddressSubmodelDataType : I4AAS.Submodels.SubmodelDataType
    {
        #region Constructors
        /// <remarks />
        public AddressSubmodelDataType()
        {
            Initialize();
        }

        [OnDeserializing]
        private void Initialize(StreamingContext context)
        {
            Initialize();
        }

        private void Initialize()
        {
            m_street = null;
            m_houseNumber = null;
            m_zipCode = null;
            m_cityTown = null;
            m_country = null;
            m_latitude = (double)0;
            m_longitude = (double)0;
        }
        #endregion

        #region Public Properties
        /// <remarks />
        [DataMember(Name = "Street", IsRequired = false, Order = 1)]
        public string Street
        {
            get { return m_street;  }
            set { m_street = value; }
        }

        /// <remarks />
        [DataMember(Name = "HouseNumber", IsRequired = false, Order = 2)]
        public string HouseNumber
        {
            get { return m_houseNumber;  }
            set { m_houseNumber = value; }
        }

        /// <remarks />
        [DataMember(Name = "ZipCode", IsRequired = false, Order = 3)]
        public string ZipCode
        {
            get { return m_zipCode;  }
            set { m_zipCode = value; }
        }

        /// <remarks />
        [DataMember(Name = "CityTown", IsRequired = false, Order = 4)]
        public LocalizedText CityTown
        {
            get { return m_cityTown;  }
            set { m_cityTown = value; }
        }

        /// <remarks />
        [DataMember(Name = "Country", IsRequired = false, Order = 5)]
        public string Country
        {
            get { return m_country;  }
            set { m_country = value; }
        }

        /// <remarks />
        [DataMember(Name = "Latitude", IsRequired = false, Order = 6)]
        public double Latitude
        {
            get { return m_latitude;  }
            set { m_latitude = value; }
        }

        /// <remarks />
        [DataMember(Name = "Longitude", IsRequired = false, Order = 7)]
        public double Longitude
        {
            get { return m_longitude;  }
            set { m_longitude = value; }
        }
        #endregion

        #region IEncodeable Members
        /// <summary cref="IEncodeable.TypeId" />
        public override ExpandedNodeId TypeId => DataTypeIds.AddressSubmodelDataType;

        /// <summary cref="IEncodeable.BinaryEncodingId" />
        public override ExpandedNodeId BinaryEncodingId => ObjectIds.AddressSubmodelDataType_Encoding_DefaultBinary;

        /// <summary cref="IEncodeable.XmlEncodingId" />
        public override ExpandedNodeId XmlEncodingId => ObjectIds.AddressSubmodelDataType_Encoding_DefaultXml;

        /// <summary cref="IJsonEncodeable.JsonEncodingId" />
        public override ExpandedNodeId JsonEncodingId => ObjectIds.AddressSubmodelDataType_Encoding_DefaultJson;

        /// <summary cref="IEncodeable.Encode(IEncoder)" />
        public override void Encode(IEncoder encoder)
        {
            base.Encode(encoder);

            encoder.PushNamespace(I4AAS.Submodels.Namespaces.I4AASXsd);

            encoder.WriteString("Street", Street);
            encoder.WriteString("HouseNumber", HouseNumber);
            encoder.WriteString("ZipCode", ZipCode);
            encoder.WriteLocalizedText("CityTown", CityTown);
            encoder.WriteString("Country", Country);
            encoder.WriteDouble("Latitude", Latitude);
            encoder.WriteDouble("Longitude", Longitude);

            encoder.PopNamespace();
        }

        /// <summary cref="IEncodeable.Decode(IDecoder)" />
        public override void Decode(IDecoder decoder)
        {
            base.Decode(decoder);

            decoder.PushNamespace(I4AAS.Submodels.Namespaces.I4AASXsd);

            Street = decoder.ReadString("Street");
            HouseNumber = decoder.ReadString("HouseNumber");
            ZipCode = decoder.ReadString("ZipCode");
            CityTown = decoder.ReadLocalizedText("CityTown");
            Country = decoder.ReadString("Country");
            Latitude = decoder.ReadDouble("Latitude");
            Longitude = decoder.ReadDouble("Longitude");

            decoder.PopNamespace();
        }

        /// <summary cref="IEncodeable.IsEqual(IEncodeable)" />
        public override bool IsEqual(IEncodeable encodeable)
        {
            if (Object.ReferenceEquals(this, encodeable))
            {
                return true;
            }

            AddressSubmodelDataType value = encodeable as AddressSubmodelDataType;

            if (value == null)
            {
                return false;
            }

            if (!base.IsEqual(encodeable)) return false;
            if (!Utils.IsEqual(m_street, value.m_street)) return false;
            if (!Utils.IsEqual(m_houseNumber, value.m_houseNumber)) return false;
            if (!Utils.IsEqual(m_zipCode, value.m_zipCode)) return false;
            if (!Utils.IsEqual(m_cityTown, value.m_cityTown)) return false;
            if (!Utils.IsEqual(m_country, value.m_country)) return false;
            if (!Utils.IsEqual(m_latitude, value.m_latitude)) return false;
            if (!Utils.IsEqual(m_longitude, value.m_longitude)) return false;

            return base.IsEqual(encodeable);
        }

        /// <summary cref="ICloneable.Clone" />
        public override object Clone()
        {
            return (AddressSubmodelDataType)this.MemberwiseClone();
        }

        /// <summary cref="Object.MemberwiseClone" />
        public new object MemberwiseClone()
        {
            AddressSubmodelDataType clone = (AddressSubmodelDataType)base.MemberwiseClone();

            clone.m_street = (string)Utils.Clone(this.m_street);
            clone.m_houseNumber = (string)Utils.Clone(this.m_houseNumber);
            clone.m_zipCode = (string)Utils.Clone(this.m_zipCode);
            clone.m_cityTown = (LocalizedText)Utils.Clone(this.m_cityTown);
            clone.m_country = (string)Utils.Clone(this.m_country);
            clone.m_latitude = (double)Utils.Clone(this.m_latitude);
            clone.m_longitude = (double)Utils.Clone(this.m_longitude);

            return clone;
        }
        #endregion

        #region Private Fields
        private string m_street;
        private string m_houseNumber;
        private string m_zipCode;
        private LocalizedText m_cityTown;
        private string m_country;
        private double m_latitude;
        private double m_longitude;
        #endregion
    }

    #region AddressSubmodelDataTypeCollection Class
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    [CollectionDataContract(Name = "ListOfAddressSubmodelDataType", Namespace = I4AAS.Submodels.Namespaces.I4AASXsd, ItemName = "AddressSubmodelDataType")]
    public partial class AddressSubmodelDataTypeCollection : List<AddressSubmodelDataType>, ICloneable
    {
        #region Constructors
        /// <remarks />
        public AddressSubmodelDataTypeCollection() {}

        /// <remarks />
        public AddressSubmodelDataTypeCollection(int capacity) : base(capacity) {}

        /// <remarks />
        public AddressSubmodelDataTypeCollection(IEnumerable<AddressSubmodelDataType> collection) : base(collection) {}
        #endregion

        #region Static Operators
        /// <remarks />
        public static implicit operator AddressSubmodelDataTypeCollection(AddressSubmodelDataType[] values)
        {
            if (values != null)
            {
                return new AddressSubmodelDataTypeCollection(values);
            }

            return new AddressSubmodelDataTypeCollection();
        }

        /// <remarks />
        public static explicit operator AddressSubmodelDataType[](AddressSubmodelDataTypeCollection values)
        {
            if (values != null)
            {
                return values.ToArray();
            }

            return null;
        }
        #endregion

        #region ICloneable Methods
        /// <remarks />
        public object Clone()
        {
            return (AddressSubmodelDataTypeCollection)this.MemberwiseClone();
        }
        #endregion

        /// <summary cref="Object.MemberwiseClone" />
        public new object MemberwiseClone()
        {
            AddressSubmodelDataTypeCollection clone = new AddressSubmodelDataTypeCollection(this.Count);

            for (int ii = 0; ii < this.Count; ii++)
            {
                clone.Add((AddressSubmodelDataType)Utils.Clone(this[ii]));
            }

            return clone;
        }
    }
    #endregion
    #endif
    #endregion
}