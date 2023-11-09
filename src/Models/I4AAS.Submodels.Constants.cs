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

namespace I4AAS.Submodels
{
    #region DataType Identifiers
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public static partial class DataTypes
    {
        /// <remarks />
        public const uint SubmodelDataType = 2;

        /// <remarks />
        public const uint AssetDescriptionFileDataType = 3;

        /// <remarks />
        public const uint NameplateSubmodelDataType = 77;

        /// <remarks />
        public const uint ContactInformationSubmodelDataType = 83;

        /// <remarks />
        public const uint MarkingSubmodelDataType = 113;

        /// <remarks />
        public const uint ProductCarbonFootprintDataType = 128;

        /// <remarks />
        public const uint AddressSubmodelDataType = 137;
    }
    #endregion

    #region Method Identifiers
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public static partial class Methods
    {
        /// <remarks />
        public const uint AssetModelType_Nameplate_CompanyLogo_Open = 263;

        /// <remarks />
        public const uint AssetModelType_Nameplate_CompanyLogo_Close = 266;

        /// <remarks />
        public const uint AssetModelType_Nameplate_CompanyLogo_Read = 268;

        /// <remarks />
        public const uint AssetModelType_Nameplate_CompanyLogo_Write = 271;

        /// <remarks />
        public const uint AssetModelType_Nameplate_CompanyLogo_GetPosition = 273;

        /// <remarks />
        public const uint AssetModelType_Nameplate_CompanyLogo_SetPosition = 276;

        /// <remarks />
        public const uint AssetModelType_AssetFile_Open = 219;

        /// <remarks />
        public const uint AssetModelType_AssetFile_Close = 222;

        /// <remarks />
        public const uint AssetModelType_AssetFile_Read = 224;

        /// <remarks />
        public const uint AssetModelType_AssetFile_Write = 227;

        /// <remarks />
        public const uint AssetModelType_AssetFile_GetPosition = 229;

        /// <remarks />
        public const uint AssetModelType_AssetFile_SetPosition = 232;

        /// <remarks />
        public const uint NameplateSubmodelType_CompanyLogo_Open = 33;

        /// <remarks />
        public const uint NameplateSubmodelType_CompanyLogo_Close = 36;

        /// <remarks />
        public const uint NameplateSubmodelType_CompanyLogo_Read = 38;

        /// <remarks />
        public const uint NameplateSubmodelType_CompanyLogo_Write = 41;

        /// <remarks />
        public const uint NameplateSubmodelType_CompanyLogo_GetPosition = 43;

        /// <remarks />
        public const uint NameplateSubmodelType_CompanyLogo_SetPosition = 46;

        /// <remarks />
        public const uint NameplateSubmodelType_Markings_Placeholder_MarkingFile_Open = 61;

        /// <remarks />
        public const uint NameplateSubmodelType_Markings_Placeholder_MarkingFile_Close = 64;

        /// <remarks />
        public const uint NameplateSubmodelType_Markings_Placeholder_MarkingFile_Read = 66;

        /// <remarks />
        public const uint NameplateSubmodelType_Markings_Placeholder_MarkingFile_Write = 69;

        /// <remarks />
        public const uint NameplateSubmodelType_Markings_Placeholder_MarkingFile_GetPosition = 71;

        /// <remarks />
        public const uint NameplateSubmodelType_Markings_Placeholder_MarkingFile_SetPosition = 74;

        /// <remarks />
        public const uint MarkingSubmodelType_MarkingFile_Open = 97;

        /// <remarks />
        public const uint MarkingSubmodelType_MarkingFile_Close = 100;

        /// <remarks />
        public const uint MarkingSubmodelType_MarkingFile_Read = 102;

        /// <remarks />
        public const uint MarkingSubmodelType_MarkingFile_Write = 105;

        /// <remarks />
        public const uint MarkingSubmodelType_MarkingFile_GetPosition = 107;

        /// <remarks />
        public const uint MarkingSubmodelType_MarkingFile_SetPosition = 110;
    }
    #endregion

    #region Object Identifiers
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public static partial class Objects
    {
        /// <remarks />
        public const uint AssetModelType_Nameplate = 234;

        /// <remarks />
        public const uint AssetModelType_Nameplate_ContactInformation = 238;

        /// <remarks />
        public const uint AssetModelType_Submodels_Placeholder = 210;

        /// <remarks />
        public const uint AssetModelType_AssetFile = 211;

        /// <remarks />
        public const uint NameplateSubmodelType_ContactInformation = 8;

        /// <remarks />
        public const uint NameplateSubmodelType_CompanyLogo = 25;

        /// <remarks />
        public const uint NameplateSubmodelType_Markings_Placeholder = 48;

        /// <remarks />
        public const uint NameplateSubmodelType_Markings_Placeholder_MarkingFile = 53;

        /// <remarks />
        public const uint MarkingSubmodelType_MarkingFile = 89;

        /// <remarks />
        public const uint ProductCarbonFootprintSubmodelType_PCFGoodsAddressHandover = 120;

        /// <remarks />
        public const uint SubmodelDataType_Encoding_DefaultBinary = 138;

        /// <remarks />
        public const uint AssetDescriptionFileDataType_Encoding_DefaultBinary = 139;

        /// <remarks />
        public const uint NameplateSubmodelDataType_Encoding_DefaultBinary = 140;

        /// <remarks />
        public const uint ContactInformationSubmodelDataType_Encoding_DefaultBinary = 141;

        /// <remarks />
        public const uint MarkingSubmodelDataType_Encoding_DefaultBinary = 142;

        /// <remarks />
        public const uint ProductCarbonFootprintDataType_Encoding_DefaultBinary = 143;

        /// <remarks />
        public const uint AddressSubmodelDataType_Encoding_DefaultBinary = 144;

        /// <remarks />
        public const uint SubmodelDataType_Encoding_DefaultXml = 170;

        /// <remarks />
        public const uint AssetDescriptionFileDataType_Encoding_DefaultXml = 171;

        /// <remarks />
        public const uint NameplateSubmodelDataType_Encoding_DefaultXml = 172;

        /// <remarks />
        public const uint ContactInformationSubmodelDataType_Encoding_DefaultXml = 173;

        /// <remarks />
        public const uint MarkingSubmodelDataType_Encoding_DefaultXml = 174;

        /// <remarks />
        public const uint ProductCarbonFootprintDataType_Encoding_DefaultXml = 175;

        /// <remarks />
        public const uint AddressSubmodelDataType_Encoding_DefaultXml = 176;

        /// <remarks />
        public const uint SubmodelDataType_Encoding_DefaultJson = 202;

        /// <remarks />
        public const uint AssetDescriptionFileDataType_Encoding_DefaultJson = 203;

        /// <remarks />
        public const uint NameplateSubmodelDataType_Encoding_DefaultJson = 204;

        /// <remarks />
        public const uint ContactInformationSubmodelDataType_Encoding_DefaultJson = 205;

        /// <remarks />
        public const uint MarkingSubmodelDataType_Encoding_DefaultJson = 206;

        /// <remarks />
        public const uint ProductCarbonFootprintDataType_Encoding_DefaultJson = 207;

        /// <remarks />
        public const uint AddressSubmodelDataType_Encoding_DefaultJson = 208;
    }
    #endregion

    #region ObjectType Identifiers
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public static partial class ObjectTypes
    {
        /// <remarks />
        public const uint SubmodelType = 1;

        /// <remarks />
        public const uint AssetModelType = 209;

        /// <remarks />
        public const uint NameplateSubmodelType = 4;

        /// <remarks />
        public const uint ContactInformationSubmodelType = 78;

        /// <remarks />
        public const uint MarkingSubmodelType = 84;

        /// <remarks />
        public const uint ProductCarbonFootprintSubmodelType = 114;

        /// <remarks />
        public const uint AddressSubmodelType = 129;
    }
    #endregion

    #region Variable Identifiers
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public static partial class Variables
    {
        /// <remarks />
        public const uint SubmodelType_ModelId = 278;

        /// <remarks />
        public const uint SubmodelType_ModelIdShort = 279;

        /// <remarks />
        public const uint SubmodelType_ModelDescription = 280;

        /// <remarks />
        public const uint AssetModelType_Nameplate_ModelId = 281;

        /// <remarks />
        public const uint AssetModelType_Nameplate_ModelIdShort = 282;

        /// <remarks />
        public const uint AssetModelType_Nameplate_URIOfTheProduct = 235;

        /// <remarks />
        public const uint AssetModelType_Nameplate_ManufacturerName = 236;

        /// <remarks />
        public const uint AssetModelType_Nameplate_ManufacturerProductDesignation = 237;

        /// <remarks />
        public const uint AssetModelType_Nameplate_ContactInformation_ModelId = 284;

        /// <remarks />
        public const uint AssetModelType_Nameplate_ContactInformation_ModelIdShort = 285;

        /// <remarks />
        public const uint AssetModelType_Nameplate_ContactInformation_Street = 239;

        /// <remarks />
        public const uint AssetModelType_Nameplate_ContactInformation_Zipcode = 240;

        /// <remarks />
        public const uint AssetModelType_Nameplate_ContactInformation_CityTown = 241;

        /// <remarks />
        public const uint AssetModelType_Nameplate_ContactInformation_NationalCode = 242;

        /// <remarks />
        public const uint AssetModelType_Nameplate_YearOfConstruction = 249;

        /// <remarks />
        public const uint AssetModelType_Nameplate_CompanyLogo_Size = 256;

        /// <remarks />
        public const uint AssetModelType_Nameplate_CompanyLogo_Writable = 257;

        /// <remarks />
        public const uint AssetModelType_Nameplate_CompanyLogo_UserWritable = 258;

        /// <remarks />
        public const uint AssetModelType_Nameplate_CompanyLogo_OpenCount = 259;

        /// <remarks />
        public const uint AssetModelType_Nameplate_CompanyLogo_Open_InputArguments = 264;

        /// <remarks />
        public const uint AssetModelType_Nameplate_CompanyLogo_Open_OutputArguments = 265;

        /// <remarks />
        public const uint AssetModelType_Nameplate_CompanyLogo_Close_InputArguments = 267;

        /// <remarks />
        public const uint AssetModelType_Nameplate_CompanyLogo_Read_InputArguments = 269;

        /// <remarks />
        public const uint AssetModelType_Nameplate_CompanyLogo_Read_OutputArguments = 270;

        /// <remarks />
        public const uint AssetModelType_Nameplate_CompanyLogo_Write_InputArguments = 272;

        /// <remarks />
        public const uint AssetModelType_Nameplate_CompanyLogo_GetPosition_InputArguments = 274;

        /// <remarks />
        public const uint AssetModelType_Nameplate_CompanyLogo_GetPosition_OutputArguments = 275;

        /// <remarks />
        public const uint AssetModelType_Nameplate_CompanyLogo_SetPosition_InputArguments = 277;

        /// <remarks />
        public const uint AssetModelType_Submodels_Placeholder_ModelId = 287;

        /// <remarks />
        public const uint AssetModelType_Submodels_Placeholder_ModelIdShort = 288;

        /// <remarks />
        public const uint AssetModelType_AssetFile_Size = 212;

        /// <remarks />
        public const uint AssetModelType_AssetFile_Writable = 213;

        /// <remarks />
        public const uint AssetModelType_AssetFile_UserWritable = 214;

        /// <remarks />
        public const uint AssetModelType_AssetFile_OpenCount = 215;

        /// <remarks />
        public const uint AssetModelType_AssetFile_Open_InputArguments = 220;

        /// <remarks />
        public const uint AssetModelType_AssetFile_Open_OutputArguments = 221;

        /// <remarks />
        public const uint AssetModelType_AssetFile_Close_InputArguments = 223;

        /// <remarks />
        public const uint AssetModelType_AssetFile_Read_InputArguments = 225;

        /// <remarks />
        public const uint AssetModelType_AssetFile_Read_OutputArguments = 226;

        /// <remarks />
        public const uint AssetModelType_AssetFile_Write_InputArguments = 228;

        /// <remarks />
        public const uint AssetModelType_AssetFile_GetPosition_InputArguments = 230;

        /// <remarks />
        public const uint AssetModelType_AssetFile_GetPosition_OutputArguments = 231;

        /// <remarks />
        public const uint AssetModelType_AssetFile_SetPosition_InputArguments = 233;

        /// <remarks />
        public const uint NameplateSubmodelType_URIOfTheProduct = 5;

        /// <remarks />
        public const uint NameplateSubmodelType_ManufacturerName = 6;

        /// <remarks />
        public const uint NameplateSubmodelType_ManufacturerProductDesignation = 7;

        /// <remarks />
        public const uint NameplateSubmodelType_ContactInformation_ModelId = 293;

        /// <remarks />
        public const uint NameplateSubmodelType_ContactInformation_ModelIdShort = 294;

        /// <remarks />
        public const uint NameplateSubmodelType_ContactInformation_Street = 9;

        /// <remarks />
        public const uint NameplateSubmodelType_ContactInformation_Zipcode = 10;

        /// <remarks />
        public const uint NameplateSubmodelType_ContactInformation_CityTown = 11;

        /// <remarks />
        public const uint NameplateSubmodelType_ContactInformation_NationalCode = 12;

        /// <remarks />
        public const uint NameplateSubmodelType_ManufacturerProductRoot = 13;

        /// <remarks />
        public const uint NameplateSubmodelType_ManufacturerProductFamily = 14;

        /// <remarks />
        public const uint NameplateSubmodelType_ManufacturerProductType = 15;

        /// <remarks />
        public const uint NameplateSubmodelType_OrderCodeOfManufacturer = 16;

        /// <remarks />
        public const uint NameplateSubmodelType_ProductArticleNumberOfManufacturer = 17;

        /// <remarks />
        public const uint NameplateSubmodelType_SerialNumber = 18;

        /// <remarks />
        public const uint NameplateSubmodelType_YearOfConstruction = 19;

        /// <remarks />
        public const uint NameplateSubmodelType_DateOfManufacture = 20;

        /// <remarks />
        public const uint NameplateSubmodelType_HardwareVersion = 21;

        /// <remarks />
        public const uint NameplateSubmodelType_FirmwareVersion = 22;

        /// <remarks />
        public const uint NameplateSubmodelType_SoftwareVersion = 23;

        /// <remarks />
        public const uint NameplateSubmodelType_CountryOfOrigin = 24;

        /// <remarks />
        public const uint NameplateSubmodelType_CompanyLogo_Size = 26;

        /// <remarks />
        public const uint NameplateSubmodelType_CompanyLogo_Writable = 27;

        /// <remarks />
        public const uint NameplateSubmodelType_CompanyLogo_UserWritable = 28;

        /// <remarks />
        public const uint NameplateSubmodelType_CompanyLogo_OpenCount = 29;

        /// <remarks />
        public const uint NameplateSubmodelType_CompanyLogo_Open_InputArguments = 34;

        /// <remarks />
        public const uint NameplateSubmodelType_CompanyLogo_Open_OutputArguments = 35;

        /// <remarks />
        public const uint NameplateSubmodelType_CompanyLogo_Close_InputArguments = 37;

        /// <remarks />
        public const uint NameplateSubmodelType_CompanyLogo_Read_InputArguments = 39;

        /// <remarks />
        public const uint NameplateSubmodelType_CompanyLogo_Read_OutputArguments = 40;

        /// <remarks />
        public const uint NameplateSubmodelType_CompanyLogo_Write_InputArguments = 42;

        /// <remarks />
        public const uint NameplateSubmodelType_CompanyLogo_GetPosition_InputArguments = 44;

        /// <remarks />
        public const uint NameplateSubmodelType_CompanyLogo_GetPosition_OutputArguments = 45;

        /// <remarks />
        public const uint NameplateSubmodelType_CompanyLogo_SetPosition_InputArguments = 47;

        /// <remarks />
        public const uint NameplateSubmodelType_Markings_Placeholder_ModelId = 296;

        /// <remarks />
        public const uint NameplateSubmodelType_Markings_Placeholder_ModelIdShort = 297;

        /// <remarks />
        public const uint NameplateSubmodelType_Markings_Placeholder_MarkingName = 49;

        /// <remarks />
        public const uint NameplateSubmodelType_Markings_Placeholder_DesignationOfCertificateOrApproval = 50;

        /// <remarks />
        public const uint NameplateSubmodelType_Markings_Placeholder_IssueDate = 51;

        /// <remarks />
        public const uint NameplateSubmodelType_Markings_Placeholder_ExpiryDate = 52;

        /// <remarks />
        public const uint NameplateSubmodelType_Markings_Placeholder_MarkingFile_Size = 54;

        /// <remarks />
        public const uint NameplateSubmodelType_Markings_Placeholder_MarkingFile_Writable = 55;

        /// <remarks />
        public const uint NameplateSubmodelType_Markings_Placeholder_MarkingFile_UserWritable = 56;

        /// <remarks />
        public const uint NameplateSubmodelType_Markings_Placeholder_MarkingFile_OpenCount = 57;

        /// <remarks />
        public const uint NameplateSubmodelType_Markings_Placeholder_MarkingFile_Open_InputArguments = 62;

        /// <remarks />
        public const uint NameplateSubmodelType_Markings_Placeholder_MarkingFile_Open_OutputArguments = 63;

        /// <remarks />
        public const uint NameplateSubmodelType_Markings_Placeholder_MarkingFile_Close_InputArguments = 65;

        /// <remarks />
        public const uint NameplateSubmodelType_Markings_Placeholder_MarkingFile_Read_InputArguments = 67;

        /// <remarks />
        public const uint NameplateSubmodelType_Markings_Placeholder_MarkingFile_Read_OutputArguments = 68;

        /// <remarks />
        public const uint NameplateSubmodelType_Markings_Placeholder_MarkingFile_Write_InputArguments = 70;

        /// <remarks />
        public const uint NameplateSubmodelType_Markings_Placeholder_MarkingFile_GetPosition_InputArguments = 72;

        /// <remarks />
        public const uint NameplateSubmodelType_Markings_Placeholder_MarkingFile_GetPosition_OutputArguments = 73;

        /// <remarks />
        public const uint NameplateSubmodelType_Markings_Placeholder_MarkingFile_SetPosition_InputArguments = 75;

        /// <remarks />
        public const uint NameplateSubmodelType_Markings_Placeholder_MarkingAdditionalText = 76;

        /// <remarks />
        public const uint ContactInformationSubmodelType_Street = 79;

        /// <remarks />
        public const uint ContactInformationSubmodelType_Zipcode = 80;

        /// <remarks />
        public const uint ContactInformationSubmodelType_CityTown = 81;

        /// <remarks />
        public const uint ContactInformationSubmodelType_NationalCode = 82;

        /// <remarks />
        public const uint MarkingSubmodelType_MarkingName = 85;

        /// <remarks />
        public const uint MarkingSubmodelType_DesignationOfCertificateOrApproval = 86;

        /// <remarks />
        public const uint MarkingSubmodelType_IssueDate = 87;

        /// <remarks />
        public const uint MarkingSubmodelType_ExpiryDate = 88;

        /// <remarks />
        public const uint MarkingSubmodelType_MarkingFile_Size = 90;

        /// <remarks />
        public const uint MarkingSubmodelType_MarkingFile_Writable = 91;

        /// <remarks />
        public const uint MarkingSubmodelType_MarkingFile_UserWritable = 92;

        /// <remarks />
        public const uint MarkingSubmodelType_MarkingFile_OpenCount = 93;

        /// <remarks />
        public const uint MarkingSubmodelType_MarkingFile_Open_InputArguments = 98;

        /// <remarks />
        public const uint MarkingSubmodelType_MarkingFile_Open_OutputArguments = 99;

        /// <remarks />
        public const uint MarkingSubmodelType_MarkingFile_Close_InputArguments = 101;

        /// <remarks />
        public const uint MarkingSubmodelType_MarkingFile_Read_InputArguments = 103;

        /// <remarks />
        public const uint MarkingSubmodelType_MarkingFile_Read_OutputArguments = 104;

        /// <remarks />
        public const uint MarkingSubmodelType_MarkingFile_Write_InputArguments = 106;

        /// <remarks />
        public const uint MarkingSubmodelType_MarkingFile_GetPosition_InputArguments = 108;

        /// <remarks />
        public const uint MarkingSubmodelType_MarkingFile_GetPosition_OutputArguments = 109;

        /// <remarks />
        public const uint MarkingSubmodelType_MarkingFile_SetPosition_InputArguments = 111;

        /// <remarks />
        public const uint MarkingSubmodelType_MarkingAdditionalText = 112;

        /// <remarks />
        public const uint ProductCarbonFootprintSubmodelType_PCFCalculationMethod = 115;

        /// <remarks />
        public const uint ProductCarbonFootprintSubmodelType_PCFCO2eq = 116;

        /// <remarks />
        public const uint ProductCarbonFootprintSubmodelType_PCFReferenceValueForCalculation = 117;

        /// <remarks />
        public const uint ProductCarbonFootprintSubmodelType_PCFQuantityOfMeasureForCalculation = 118;

        /// <remarks />
        public const uint ProductCarbonFootprintSubmodelType_PCFLiveCyclePhase = 119;

        /// <remarks />
        public const uint ProductCarbonFootprintSubmodelType_PCFGoodsAddressHandover_ModelId = 308;

        /// <remarks />
        public const uint ProductCarbonFootprintSubmodelType_PCFGoodsAddressHandover_ModelIdShort = 309;

        /// <remarks />
        public const uint AddressSubmodelType_Street = 130;

        /// <remarks />
        public const uint AddressSubmodelType_HouseNumber = 131;

        /// <remarks />
        public const uint AddressSubmodelType_ZipCode = 132;

        /// <remarks />
        public const uint AddressSubmodelType_CityTown = 133;

        /// <remarks />
        public const uint AddressSubmodelType_Country = 134;

        /// <remarks />
        public const uint AddressSubmodelType_Latitude = 135;

        /// <remarks />
        public const uint AddressSubmodelType_Longitude = 136;

        /// <remarks />
        public const uint I4AAS_BinarySchema = 145;

        /// <remarks />
        public const uint I4AAS_BinarySchema_NamespaceUri = 147;

        /// <remarks />
        public const uint I4AAS_BinarySchema_Deprecated = 148;

        /// <remarks />
        public const uint I4AAS_BinarySchema_SubmodelDataType = 149;

        /// <remarks />
        public const uint I4AAS_BinarySchema_AssetDescriptionFileDataType = 152;

        /// <remarks />
        public const uint I4AAS_BinarySchema_NameplateSubmodelDataType = 155;

        /// <remarks />
        public const uint I4AAS_BinarySchema_ContactInformationSubmodelDataType = 158;

        /// <remarks />
        public const uint I4AAS_BinarySchema_MarkingSubmodelDataType = 161;

        /// <remarks />
        public const uint I4AAS_BinarySchema_ProductCarbonFootprintDataType = 164;

        /// <remarks />
        public const uint I4AAS_BinarySchema_AddressSubmodelDataType = 167;

        /// <remarks />
        public const uint I4AAS_XmlSchema = 177;

        /// <remarks />
        public const uint I4AAS_XmlSchema_NamespaceUri = 179;

        /// <remarks />
        public const uint I4AAS_XmlSchema_Deprecated = 180;

        /// <remarks />
        public const uint I4AAS_XmlSchema_SubmodelDataType = 181;

        /// <remarks />
        public const uint I4AAS_XmlSchema_AssetDescriptionFileDataType = 184;

        /// <remarks />
        public const uint I4AAS_XmlSchema_NameplateSubmodelDataType = 187;

        /// <remarks />
        public const uint I4AAS_XmlSchema_ContactInformationSubmodelDataType = 190;

        /// <remarks />
        public const uint I4AAS_XmlSchema_MarkingSubmodelDataType = 193;

        /// <remarks />
        public const uint I4AAS_XmlSchema_ProductCarbonFootprintDataType = 196;

        /// <remarks />
        public const uint I4AAS_XmlSchema_AddressSubmodelDataType = 199;
    }
    #endregion

    #region DataType Node Identifiers
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public static partial class DataTypeIds
    {
        /// <remarks />
        public static readonly ExpandedNodeId SubmodelDataType = new ExpandedNodeId(I4AAS.Submodels.DataTypes.SubmodelDataType, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId AssetDescriptionFileDataType = new ExpandedNodeId(I4AAS.Submodels.DataTypes.AssetDescriptionFileDataType, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId NameplateSubmodelDataType = new ExpandedNodeId(I4AAS.Submodels.DataTypes.NameplateSubmodelDataType, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId ContactInformationSubmodelDataType = new ExpandedNodeId(I4AAS.Submodels.DataTypes.ContactInformationSubmodelDataType, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId MarkingSubmodelDataType = new ExpandedNodeId(I4AAS.Submodels.DataTypes.MarkingSubmodelDataType, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId ProductCarbonFootprintDataType = new ExpandedNodeId(I4AAS.Submodels.DataTypes.ProductCarbonFootprintDataType, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId AddressSubmodelDataType = new ExpandedNodeId(I4AAS.Submodels.DataTypes.AddressSubmodelDataType, I4AAS.Submodels.Namespaces.I4AAS);
    }
    #endregion

    #region Method Node Identifiers
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public static partial class MethodIds
    {
        /// <remarks />
        public static readonly ExpandedNodeId AssetModelType_Nameplate_CompanyLogo_Open = new ExpandedNodeId(I4AAS.Submodels.Methods.AssetModelType_Nameplate_CompanyLogo_Open, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId AssetModelType_Nameplate_CompanyLogo_Close = new ExpandedNodeId(I4AAS.Submodels.Methods.AssetModelType_Nameplate_CompanyLogo_Close, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId AssetModelType_Nameplate_CompanyLogo_Read = new ExpandedNodeId(I4AAS.Submodels.Methods.AssetModelType_Nameplate_CompanyLogo_Read, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId AssetModelType_Nameplate_CompanyLogo_Write = new ExpandedNodeId(I4AAS.Submodels.Methods.AssetModelType_Nameplate_CompanyLogo_Write, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId AssetModelType_Nameplate_CompanyLogo_GetPosition = new ExpandedNodeId(I4AAS.Submodels.Methods.AssetModelType_Nameplate_CompanyLogo_GetPosition, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId AssetModelType_Nameplate_CompanyLogo_SetPosition = new ExpandedNodeId(I4AAS.Submodels.Methods.AssetModelType_Nameplate_CompanyLogo_SetPosition, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId AssetModelType_AssetFile_Open = new ExpandedNodeId(I4AAS.Submodels.Methods.AssetModelType_AssetFile_Open, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId AssetModelType_AssetFile_Close = new ExpandedNodeId(I4AAS.Submodels.Methods.AssetModelType_AssetFile_Close, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId AssetModelType_AssetFile_Read = new ExpandedNodeId(I4AAS.Submodels.Methods.AssetModelType_AssetFile_Read, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId AssetModelType_AssetFile_Write = new ExpandedNodeId(I4AAS.Submodels.Methods.AssetModelType_AssetFile_Write, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId AssetModelType_AssetFile_GetPosition = new ExpandedNodeId(I4AAS.Submodels.Methods.AssetModelType_AssetFile_GetPosition, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId AssetModelType_AssetFile_SetPosition = new ExpandedNodeId(I4AAS.Submodels.Methods.AssetModelType_AssetFile_SetPosition, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId NameplateSubmodelType_CompanyLogo_Open = new ExpandedNodeId(I4AAS.Submodels.Methods.NameplateSubmodelType_CompanyLogo_Open, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId NameplateSubmodelType_CompanyLogo_Close = new ExpandedNodeId(I4AAS.Submodels.Methods.NameplateSubmodelType_CompanyLogo_Close, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId NameplateSubmodelType_CompanyLogo_Read = new ExpandedNodeId(I4AAS.Submodels.Methods.NameplateSubmodelType_CompanyLogo_Read, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId NameplateSubmodelType_CompanyLogo_Write = new ExpandedNodeId(I4AAS.Submodels.Methods.NameplateSubmodelType_CompanyLogo_Write, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId NameplateSubmodelType_CompanyLogo_GetPosition = new ExpandedNodeId(I4AAS.Submodels.Methods.NameplateSubmodelType_CompanyLogo_GetPosition, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId NameplateSubmodelType_CompanyLogo_SetPosition = new ExpandedNodeId(I4AAS.Submodels.Methods.NameplateSubmodelType_CompanyLogo_SetPosition, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId NameplateSubmodelType_Markings_Placeholder_MarkingFile_Open = new ExpandedNodeId(I4AAS.Submodels.Methods.NameplateSubmodelType_Markings_Placeholder_MarkingFile_Open, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId NameplateSubmodelType_Markings_Placeholder_MarkingFile_Close = new ExpandedNodeId(I4AAS.Submodels.Methods.NameplateSubmodelType_Markings_Placeholder_MarkingFile_Close, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId NameplateSubmodelType_Markings_Placeholder_MarkingFile_Read = new ExpandedNodeId(I4AAS.Submodels.Methods.NameplateSubmodelType_Markings_Placeholder_MarkingFile_Read, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId NameplateSubmodelType_Markings_Placeholder_MarkingFile_Write = new ExpandedNodeId(I4AAS.Submodels.Methods.NameplateSubmodelType_Markings_Placeholder_MarkingFile_Write, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId NameplateSubmodelType_Markings_Placeholder_MarkingFile_GetPosition = new ExpandedNodeId(I4AAS.Submodels.Methods.NameplateSubmodelType_Markings_Placeholder_MarkingFile_GetPosition, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId NameplateSubmodelType_Markings_Placeholder_MarkingFile_SetPosition = new ExpandedNodeId(I4AAS.Submodels.Methods.NameplateSubmodelType_Markings_Placeholder_MarkingFile_SetPosition, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId MarkingSubmodelType_MarkingFile_Open = new ExpandedNodeId(I4AAS.Submodels.Methods.MarkingSubmodelType_MarkingFile_Open, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId MarkingSubmodelType_MarkingFile_Close = new ExpandedNodeId(I4AAS.Submodels.Methods.MarkingSubmodelType_MarkingFile_Close, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId MarkingSubmodelType_MarkingFile_Read = new ExpandedNodeId(I4AAS.Submodels.Methods.MarkingSubmodelType_MarkingFile_Read, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId MarkingSubmodelType_MarkingFile_Write = new ExpandedNodeId(I4AAS.Submodels.Methods.MarkingSubmodelType_MarkingFile_Write, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId MarkingSubmodelType_MarkingFile_GetPosition = new ExpandedNodeId(I4AAS.Submodels.Methods.MarkingSubmodelType_MarkingFile_GetPosition, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId MarkingSubmodelType_MarkingFile_SetPosition = new ExpandedNodeId(I4AAS.Submodels.Methods.MarkingSubmodelType_MarkingFile_SetPosition, I4AAS.Submodels.Namespaces.I4AAS);
    }
    #endregion

    #region Object Node Identifiers
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public static partial class ObjectIds
    {
        /// <remarks />
        public static readonly ExpandedNodeId AssetModelType_Nameplate = new ExpandedNodeId(I4AAS.Submodels.Objects.AssetModelType_Nameplate, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId AssetModelType_Nameplate_ContactInformation = new ExpandedNodeId(I4AAS.Submodels.Objects.AssetModelType_Nameplate_ContactInformation, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId AssetModelType_Submodels_Placeholder = new ExpandedNodeId(I4AAS.Submodels.Objects.AssetModelType_Submodels_Placeholder, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId AssetModelType_AssetFile = new ExpandedNodeId(I4AAS.Submodels.Objects.AssetModelType_AssetFile, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId NameplateSubmodelType_ContactInformation = new ExpandedNodeId(I4AAS.Submodels.Objects.NameplateSubmodelType_ContactInformation, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId NameplateSubmodelType_CompanyLogo = new ExpandedNodeId(I4AAS.Submodels.Objects.NameplateSubmodelType_CompanyLogo, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId NameplateSubmodelType_Markings_Placeholder = new ExpandedNodeId(I4AAS.Submodels.Objects.NameplateSubmodelType_Markings_Placeholder, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId NameplateSubmodelType_Markings_Placeholder_MarkingFile = new ExpandedNodeId(I4AAS.Submodels.Objects.NameplateSubmodelType_Markings_Placeholder_MarkingFile, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId MarkingSubmodelType_MarkingFile = new ExpandedNodeId(I4AAS.Submodels.Objects.MarkingSubmodelType_MarkingFile, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId ProductCarbonFootprintSubmodelType_PCFGoodsAddressHandover = new ExpandedNodeId(I4AAS.Submodels.Objects.ProductCarbonFootprintSubmodelType_PCFGoodsAddressHandover, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId SubmodelDataType_Encoding_DefaultBinary = new ExpandedNodeId(I4AAS.Submodels.Objects.SubmodelDataType_Encoding_DefaultBinary, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId AssetDescriptionFileDataType_Encoding_DefaultBinary = new ExpandedNodeId(I4AAS.Submodels.Objects.AssetDescriptionFileDataType_Encoding_DefaultBinary, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId NameplateSubmodelDataType_Encoding_DefaultBinary = new ExpandedNodeId(I4AAS.Submodels.Objects.NameplateSubmodelDataType_Encoding_DefaultBinary, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId ContactInformationSubmodelDataType_Encoding_DefaultBinary = new ExpandedNodeId(I4AAS.Submodels.Objects.ContactInformationSubmodelDataType_Encoding_DefaultBinary, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId MarkingSubmodelDataType_Encoding_DefaultBinary = new ExpandedNodeId(I4AAS.Submodels.Objects.MarkingSubmodelDataType_Encoding_DefaultBinary, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId ProductCarbonFootprintDataType_Encoding_DefaultBinary = new ExpandedNodeId(I4AAS.Submodels.Objects.ProductCarbonFootprintDataType_Encoding_DefaultBinary, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId AddressSubmodelDataType_Encoding_DefaultBinary = new ExpandedNodeId(I4AAS.Submodels.Objects.AddressSubmodelDataType_Encoding_DefaultBinary, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId SubmodelDataType_Encoding_DefaultXml = new ExpandedNodeId(I4AAS.Submodels.Objects.SubmodelDataType_Encoding_DefaultXml, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId AssetDescriptionFileDataType_Encoding_DefaultXml = new ExpandedNodeId(I4AAS.Submodels.Objects.AssetDescriptionFileDataType_Encoding_DefaultXml, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId NameplateSubmodelDataType_Encoding_DefaultXml = new ExpandedNodeId(I4AAS.Submodels.Objects.NameplateSubmodelDataType_Encoding_DefaultXml, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId ContactInformationSubmodelDataType_Encoding_DefaultXml = new ExpandedNodeId(I4AAS.Submodels.Objects.ContactInformationSubmodelDataType_Encoding_DefaultXml, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId MarkingSubmodelDataType_Encoding_DefaultXml = new ExpandedNodeId(I4AAS.Submodels.Objects.MarkingSubmodelDataType_Encoding_DefaultXml, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId ProductCarbonFootprintDataType_Encoding_DefaultXml = new ExpandedNodeId(I4AAS.Submodels.Objects.ProductCarbonFootprintDataType_Encoding_DefaultXml, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId AddressSubmodelDataType_Encoding_DefaultXml = new ExpandedNodeId(I4AAS.Submodels.Objects.AddressSubmodelDataType_Encoding_DefaultXml, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId SubmodelDataType_Encoding_DefaultJson = new ExpandedNodeId(I4AAS.Submodels.Objects.SubmodelDataType_Encoding_DefaultJson, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId AssetDescriptionFileDataType_Encoding_DefaultJson = new ExpandedNodeId(I4AAS.Submodels.Objects.AssetDescriptionFileDataType_Encoding_DefaultJson, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId NameplateSubmodelDataType_Encoding_DefaultJson = new ExpandedNodeId(I4AAS.Submodels.Objects.NameplateSubmodelDataType_Encoding_DefaultJson, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId ContactInformationSubmodelDataType_Encoding_DefaultJson = new ExpandedNodeId(I4AAS.Submodels.Objects.ContactInformationSubmodelDataType_Encoding_DefaultJson, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId MarkingSubmodelDataType_Encoding_DefaultJson = new ExpandedNodeId(I4AAS.Submodels.Objects.MarkingSubmodelDataType_Encoding_DefaultJson, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId ProductCarbonFootprintDataType_Encoding_DefaultJson = new ExpandedNodeId(I4AAS.Submodels.Objects.ProductCarbonFootprintDataType_Encoding_DefaultJson, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId AddressSubmodelDataType_Encoding_DefaultJson = new ExpandedNodeId(I4AAS.Submodels.Objects.AddressSubmodelDataType_Encoding_DefaultJson, I4AAS.Submodels.Namespaces.I4AAS);
    }
    #endregion

    #region ObjectType Node Identifiers
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public static partial class ObjectTypeIds
    {
        /// <remarks />
        public static readonly ExpandedNodeId SubmodelType = new ExpandedNodeId(I4AAS.Submodels.ObjectTypes.SubmodelType, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId AssetModelType = new ExpandedNodeId(I4AAS.Submodels.ObjectTypes.AssetModelType, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId NameplateSubmodelType = new ExpandedNodeId(I4AAS.Submodels.ObjectTypes.NameplateSubmodelType, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId ContactInformationSubmodelType = new ExpandedNodeId(I4AAS.Submodels.ObjectTypes.ContactInformationSubmodelType, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId MarkingSubmodelType = new ExpandedNodeId(I4AAS.Submodels.ObjectTypes.MarkingSubmodelType, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId ProductCarbonFootprintSubmodelType = new ExpandedNodeId(I4AAS.Submodels.ObjectTypes.ProductCarbonFootprintSubmodelType, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId AddressSubmodelType = new ExpandedNodeId(I4AAS.Submodels.ObjectTypes.AddressSubmodelType, I4AAS.Submodels.Namespaces.I4AAS);
    }
    #endregion

    #region Variable Node Identifiers
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public static partial class VariableIds
    {
        /// <remarks />
        public static readonly ExpandedNodeId SubmodelType_ModelId = new ExpandedNodeId(I4AAS.Submodels.Variables.SubmodelType_ModelId, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId SubmodelType_ModelIdShort = new ExpandedNodeId(I4AAS.Submodels.Variables.SubmodelType_ModelIdShort, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId SubmodelType_ModelDescription = new ExpandedNodeId(I4AAS.Submodels.Variables.SubmodelType_ModelDescription, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId AssetModelType_Nameplate_ModelId = new ExpandedNodeId(I4AAS.Submodels.Variables.AssetModelType_Nameplate_ModelId, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId AssetModelType_Nameplate_ModelIdShort = new ExpandedNodeId(I4AAS.Submodels.Variables.AssetModelType_Nameplate_ModelIdShort, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId AssetModelType_Nameplate_URIOfTheProduct = new ExpandedNodeId(I4AAS.Submodels.Variables.AssetModelType_Nameplate_URIOfTheProduct, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId AssetModelType_Nameplate_ManufacturerName = new ExpandedNodeId(I4AAS.Submodels.Variables.AssetModelType_Nameplate_ManufacturerName, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId AssetModelType_Nameplate_ManufacturerProductDesignation = new ExpandedNodeId(I4AAS.Submodels.Variables.AssetModelType_Nameplate_ManufacturerProductDesignation, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId AssetModelType_Nameplate_ContactInformation_ModelId = new ExpandedNodeId(I4AAS.Submodels.Variables.AssetModelType_Nameplate_ContactInformation_ModelId, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId AssetModelType_Nameplate_ContactInformation_ModelIdShort = new ExpandedNodeId(I4AAS.Submodels.Variables.AssetModelType_Nameplate_ContactInformation_ModelIdShort, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId AssetModelType_Nameplate_ContactInformation_Street = new ExpandedNodeId(I4AAS.Submodels.Variables.AssetModelType_Nameplate_ContactInformation_Street, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId AssetModelType_Nameplate_ContactInformation_Zipcode = new ExpandedNodeId(I4AAS.Submodels.Variables.AssetModelType_Nameplate_ContactInformation_Zipcode, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId AssetModelType_Nameplate_ContactInformation_CityTown = new ExpandedNodeId(I4AAS.Submodels.Variables.AssetModelType_Nameplate_ContactInformation_CityTown, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId AssetModelType_Nameplate_ContactInformation_NationalCode = new ExpandedNodeId(I4AAS.Submodels.Variables.AssetModelType_Nameplate_ContactInformation_NationalCode, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId AssetModelType_Nameplate_YearOfConstruction = new ExpandedNodeId(I4AAS.Submodels.Variables.AssetModelType_Nameplate_YearOfConstruction, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId AssetModelType_Nameplate_CompanyLogo_Size = new ExpandedNodeId(I4AAS.Submodels.Variables.AssetModelType_Nameplate_CompanyLogo_Size, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId AssetModelType_Nameplate_CompanyLogo_Writable = new ExpandedNodeId(I4AAS.Submodels.Variables.AssetModelType_Nameplate_CompanyLogo_Writable, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId AssetModelType_Nameplate_CompanyLogo_UserWritable = new ExpandedNodeId(I4AAS.Submodels.Variables.AssetModelType_Nameplate_CompanyLogo_UserWritable, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId AssetModelType_Nameplate_CompanyLogo_OpenCount = new ExpandedNodeId(I4AAS.Submodels.Variables.AssetModelType_Nameplate_CompanyLogo_OpenCount, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId AssetModelType_Nameplate_CompanyLogo_Open_InputArguments = new ExpandedNodeId(I4AAS.Submodels.Variables.AssetModelType_Nameplate_CompanyLogo_Open_InputArguments, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId AssetModelType_Nameplate_CompanyLogo_Open_OutputArguments = new ExpandedNodeId(I4AAS.Submodels.Variables.AssetModelType_Nameplate_CompanyLogo_Open_OutputArguments, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId AssetModelType_Nameplate_CompanyLogo_Close_InputArguments = new ExpandedNodeId(I4AAS.Submodels.Variables.AssetModelType_Nameplate_CompanyLogo_Close_InputArguments, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId AssetModelType_Nameplate_CompanyLogo_Read_InputArguments = new ExpandedNodeId(I4AAS.Submodels.Variables.AssetModelType_Nameplate_CompanyLogo_Read_InputArguments, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId AssetModelType_Nameplate_CompanyLogo_Read_OutputArguments = new ExpandedNodeId(I4AAS.Submodels.Variables.AssetModelType_Nameplate_CompanyLogo_Read_OutputArguments, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId AssetModelType_Nameplate_CompanyLogo_Write_InputArguments = new ExpandedNodeId(I4AAS.Submodels.Variables.AssetModelType_Nameplate_CompanyLogo_Write_InputArguments, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId AssetModelType_Nameplate_CompanyLogo_GetPosition_InputArguments = new ExpandedNodeId(I4AAS.Submodels.Variables.AssetModelType_Nameplate_CompanyLogo_GetPosition_InputArguments, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId AssetModelType_Nameplate_CompanyLogo_GetPosition_OutputArguments = new ExpandedNodeId(I4AAS.Submodels.Variables.AssetModelType_Nameplate_CompanyLogo_GetPosition_OutputArguments, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId AssetModelType_Nameplate_CompanyLogo_SetPosition_InputArguments = new ExpandedNodeId(I4AAS.Submodels.Variables.AssetModelType_Nameplate_CompanyLogo_SetPosition_InputArguments, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId AssetModelType_Submodels_Placeholder_ModelId = new ExpandedNodeId(I4AAS.Submodels.Variables.AssetModelType_Submodels_Placeholder_ModelId, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId AssetModelType_Submodels_Placeholder_ModelIdShort = new ExpandedNodeId(I4AAS.Submodels.Variables.AssetModelType_Submodels_Placeholder_ModelIdShort, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId AssetModelType_AssetFile_Size = new ExpandedNodeId(I4AAS.Submodels.Variables.AssetModelType_AssetFile_Size, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId AssetModelType_AssetFile_Writable = new ExpandedNodeId(I4AAS.Submodels.Variables.AssetModelType_AssetFile_Writable, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId AssetModelType_AssetFile_UserWritable = new ExpandedNodeId(I4AAS.Submodels.Variables.AssetModelType_AssetFile_UserWritable, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId AssetModelType_AssetFile_OpenCount = new ExpandedNodeId(I4AAS.Submodels.Variables.AssetModelType_AssetFile_OpenCount, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId AssetModelType_AssetFile_Open_InputArguments = new ExpandedNodeId(I4AAS.Submodels.Variables.AssetModelType_AssetFile_Open_InputArguments, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId AssetModelType_AssetFile_Open_OutputArguments = new ExpandedNodeId(I4AAS.Submodels.Variables.AssetModelType_AssetFile_Open_OutputArguments, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId AssetModelType_AssetFile_Close_InputArguments = new ExpandedNodeId(I4AAS.Submodels.Variables.AssetModelType_AssetFile_Close_InputArguments, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId AssetModelType_AssetFile_Read_InputArguments = new ExpandedNodeId(I4AAS.Submodels.Variables.AssetModelType_AssetFile_Read_InputArguments, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId AssetModelType_AssetFile_Read_OutputArguments = new ExpandedNodeId(I4AAS.Submodels.Variables.AssetModelType_AssetFile_Read_OutputArguments, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId AssetModelType_AssetFile_Write_InputArguments = new ExpandedNodeId(I4AAS.Submodels.Variables.AssetModelType_AssetFile_Write_InputArguments, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId AssetModelType_AssetFile_GetPosition_InputArguments = new ExpandedNodeId(I4AAS.Submodels.Variables.AssetModelType_AssetFile_GetPosition_InputArguments, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId AssetModelType_AssetFile_GetPosition_OutputArguments = new ExpandedNodeId(I4AAS.Submodels.Variables.AssetModelType_AssetFile_GetPosition_OutputArguments, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId AssetModelType_AssetFile_SetPosition_InputArguments = new ExpandedNodeId(I4AAS.Submodels.Variables.AssetModelType_AssetFile_SetPosition_InputArguments, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId NameplateSubmodelType_URIOfTheProduct = new ExpandedNodeId(I4AAS.Submodels.Variables.NameplateSubmodelType_URIOfTheProduct, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId NameplateSubmodelType_ManufacturerName = new ExpandedNodeId(I4AAS.Submodels.Variables.NameplateSubmodelType_ManufacturerName, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId NameplateSubmodelType_ManufacturerProductDesignation = new ExpandedNodeId(I4AAS.Submodels.Variables.NameplateSubmodelType_ManufacturerProductDesignation, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId NameplateSubmodelType_ContactInformation_ModelId = new ExpandedNodeId(I4AAS.Submodels.Variables.NameplateSubmodelType_ContactInformation_ModelId, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId NameplateSubmodelType_ContactInformation_ModelIdShort = new ExpandedNodeId(I4AAS.Submodels.Variables.NameplateSubmodelType_ContactInformation_ModelIdShort, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId NameplateSubmodelType_ContactInformation_Street = new ExpandedNodeId(I4AAS.Submodels.Variables.NameplateSubmodelType_ContactInformation_Street, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId NameplateSubmodelType_ContactInformation_Zipcode = new ExpandedNodeId(I4AAS.Submodels.Variables.NameplateSubmodelType_ContactInformation_Zipcode, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId NameplateSubmodelType_ContactInformation_CityTown = new ExpandedNodeId(I4AAS.Submodels.Variables.NameplateSubmodelType_ContactInformation_CityTown, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId NameplateSubmodelType_ContactInformation_NationalCode = new ExpandedNodeId(I4AAS.Submodels.Variables.NameplateSubmodelType_ContactInformation_NationalCode, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId NameplateSubmodelType_ManufacturerProductRoot = new ExpandedNodeId(I4AAS.Submodels.Variables.NameplateSubmodelType_ManufacturerProductRoot, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId NameplateSubmodelType_ManufacturerProductFamily = new ExpandedNodeId(I4AAS.Submodels.Variables.NameplateSubmodelType_ManufacturerProductFamily, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId NameplateSubmodelType_ManufacturerProductType = new ExpandedNodeId(I4AAS.Submodels.Variables.NameplateSubmodelType_ManufacturerProductType, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId NameplateSubmodelType_OrderCodeOfManufacturer = new ExpandedNodeId(I4AAS.Submodels.Variables.NameplateSubmodelType_OrderCodeOfManufacturer, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId NameplateSubmodelType_ProductArticleNumberOfManufacturer = new ExpandedNodeId(I4AAS.Submodels.Variables.NameplateSubmodelType_ProductArticleNumberOfManufacturer, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId NameplateSubmodelType_SerialNumber = new ExpandedNodeId(I4AAS.Submodels.Variables.NameplateSubmodelType_SerialNumber, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId NameplateSubmodelType_YearOfConstruction = new ExpandedNodeId(I4AAS.Submodels.Variables.NameplateSubmodelType_YearOfConstruction, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId NameplateSubmodelType_DateOfManufacture = new ExpandedNodeId(I4AAS.Submodels.Variables.NameplateSubmodelType_DateOfManufacture, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId NameplateSubmodelType_HardwareVersion = new ExpandedNodeId(I4AAS.Submodels.Variables.NameplateSubmodelType_HardwareVersion, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId NameplateSubmodelType_FirmwareVersion = new ExpandedNodeId(I4AAS.Submodels.Variables.NameplateSubmodelType_FirmwareVersion, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId NameplateSubmodelType_SoftwareVersion = new ExpandedNodeId(I4AAS.Submodels.Variables.NameplateSubmodelType_SoftwareVersion, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId NameplateSubmodelType_CountryOfOrigin = new ExpandedNodeId(I4AAS.Submodels.Variables.NameplateSubmodelType_CountryOfOrigin, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId NameplateSubmodelType_CompanyLogo_Size = new ExpandedNodeId(I4AAS.Submodels.Variables.NameplateSubmodelType_CompanyLogo_Size, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId NameplateSubmodelType_CompanyLogo_Writable = new ExpandedNodeId(I4AAS.Submodels.Variables.NameplateSubmodelType_CompanyLogo_Writable, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId NameplateSubmodelType_CompanyLogo_UserWritable = new ExpandedNodeId(I4AAS.Submodels.Variables.NameplateSubmodelType_CompanyLogo_UserWritable, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId NameplateSubmodelType_CompanyLogo_OpenCount = new ExpandedNodeId(I4AAS.Submodels.Variables.NameplateSubmodelType_CompanyLogo_OpenCount, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId NameplateSubmodelType_CompanyLogo_Open_InputArguments = new ExpandedNodeId(I4AAS.Submodels.Variables.NameplateSubmodelType_CompanyLogo_Open_InputArguments, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId NameplateSubmodelType_CompanyLogo_Open_OutputArguments = new ExpandedNodeId(I4AAS.Submodels.Variables.NameplateSubmodelType_CompanyLogo_Open_OutputArguments, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId NameplateSubmodelType_CompanyLogo_Close_InputArguments = new ExpandedNodeId(I4AAS.Submodels.Variables.NameplateSubmodelType_CompanyLogo_Close_InputArguments, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId NameplateSubmodelType_CompanyLogo_Read_InputArguments = new ExpandedNodeId(I4AAS.Submodels.Variables.NameplateSubmodelType_CompanyLogo_Read_InputArguments, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId NameplateSubmodelType_CompanyLogo_Read_OutputArguments = new ExpandedNodeId(I4AAS.Submodels.Variables.NameplateSubmodelType_CompanyLogo_Read_OutputArguments, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId NameplateSubmodelType_CompanyLogo_Write_InputArguments = new ExpandedNodeId(I4AAS.Submodels.Variables.NameplateSubmodelType_CompanyLogo_Write_InputArguments, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId NameplateSubmodelType_CompanyLogo_GetPosition_InputArguments = new ExpandedNodeId(I4AAS.Submodels.Variables.NameplateSubmodelType_CompanyLogo_GetPosition_InputArguments, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId NameplateSubmodelType_CompanyLogo_GetPosition_OutputArguments = new ExpandedNodeId(I4AAS.Submodels.Variables.NameplateSubmodelType_CompanyLogo_GetPosition_OutputArguments, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId NameplateSubmodelType_CompanyLogo_SetPosition_InputArguments = new ExpandedNodeId(I4AAS.Submodels.Variables.NameplateSubmodelType_CompanyLogo_SetPosition_InputArguments, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId NameplateSubmodelType_Markings_Placeholder_ModelId = new ExpandedNodeId(I4AAS.Submodels.Variables.NameplateSubmodelType_Markings_Placeholder_ModelId, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId NameplateSubmodelType_Markings_Placeholder_ModelIdShort = new ExpandedNodeId(I4AAS.Submodels.Variables.NameplateSubmodelType_Markings_Placeholder_ModelIdShort, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId NameplateSubmodelType_Markings_Placeholder_MarkingName = new ExpandedNodeId(I4AAS.Submodels.Variables.NameplateSubmodelType_Markings_Placeholder_MarkingName, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId NameplateSubmodelType_Markings_Placeholder_DesignationOfCertificateOrApproval = new ExpandedNodeId(I4AAS.Submodels.Variables.NameplateSubmodelType_Markings_Placeholder_DesignationOfCertificateOrApproval, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId NameplateSubmodelType_Markings_Placeholder_IssueDate = new ExpandedNodeId(I4AAS.Submodels.Variables.NameplateSubmodelType_Markings_Placeholder_IssueDate, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId NameplateSubmodelType_Markings_Placeholder_ExpiryDate = new ExpandedNodeId(I4AAS.Submodels.Variables.NameplateSubmodelType_Markings_Placeholder_ExpiryDate, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId NameplateSubmodelType_Markings_Placeholder_MarkingFile_Size = new ExpandedNodeId(I4AAS.Submodels.Variables.NameplateSubmodelType_Markings_Placeholder_MarkingFile_Size, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId NameplateSubmodelType_Markings_Placeholder_MarkingFile_Writable = new ExpandedNodeId(I4AAS.Submodels.Variables.NameplateSubmodelType_Markings_Placeholder_MarkingFile_Writable, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId NameplateSubmodelType_Markings_Placeholder_MarkingFile_UserWritable = new ExpandedNodeId(I4AAS.Submodels.Variables.NameplateSubmodelType_Markings_Placeholder_MarkingFile_UserWritable, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId NameplateSubmodelType_Markings_Placeholder_MarkingFile_OpenCount = new ExpandedNodeId(I4AAS.Submodels.Variables.NameplateSubmodelType_Markings_Placeholder_MarkingFile_OpenCount, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId NameplateSubmodelType_Markings_Placeholder_MarkingFile_Open_InputArguments = new ExpandedNodeId(I4AAS.Submodels.Variables.NameplateSubmodelType_Markings_Placeholder_MarkingFile_Open_InputArguments, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId NameplateSubmodelType_Markings_Placeholder_MarkingFile_Open_OutputArguments = new ExpandedNodeId(I4AAS.Submodels.Variables.NameplateSubmodelType_Markings_Placeholder_MarkingFile_Open_OutputArguments, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId NameplateSubmodelType_Markings_Placeholder_MarkingFile_Close_InputArguments = new ExpandedNodeId(I4AAS.Submodels.Variables.NameplateSubmodelType_Markings_Placeholder_MarkingFile_Close_InputArguments, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId NameplateSubmodelType_Markings_Placeholder_MarkingFile_Read_InputArguments = new ExpandedNodeId(I4AAS.Submodels.Variables.NameplateSubmodelType_Markings_Placeholder_MarkingFile_Read_InputArguments, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId NameplateSubmodelType_Markings_Placeholder_MarkingFile_Read_OutputArguments = new ExpandedNodeId(I4AAS.Submodels.Variables.NameplateSubmodelType_Markings_Placeholder_MarkingFile_Read_OutputArguments, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId NameplateSubmodelType_Markings_Placeholder_MarkingFile_Write_InputArguments = new ExpandedNodeId(I4AAS.Submodels.Variables.NameplateSubmodelType_Markings_Placeholder_MarkingFile_Write_InputArguments, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId NameplateSubmodelType_Markings_Placeholder_MarkingFile_GetPosition_InputArguments = new ExpandedNodeId(I4AAS.Submodels.Variables.NameplateSubmodelType_Markings_Placeholder_MarkingFile_GetPosition_InputArguments, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId NameplateSubmodelType_Markings_Placeholder_MarkingFile_GetPosition_OutputArguments = new ExpandedNodeId(I4AAS.Submodels.Variables.NameplateSubmodelType_Markings_Placeholder_MarkingFile_GetPosition_OutputArguments, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId NameplateSubmodelType_Markings_Placeholder_MarkingFile_SetPosition_InputArguments = new ExpandedNodeId(I4AAS.Submodels.Variables.NameplateSubmodelType_Markings_Placeholder_MarkingFile_SetPosition_InputArguments, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId NameplateSubmodelType_Markings_Placeholder_MarkingAdditionalText = new ExpandedNodeId(I4AAS.Submodels.Variables.NameplateSubmodelType_Markings_Placeholder_MarkingAdditionalText, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId ContactInformationSubmodelType_Street = new ExpandedNodeId(I4AAS.Submodels.Variables.ContactInformationSubmodelType_Street, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId ContactInformationSubmodelType_Zipcode = new ExpandedNodeId(I4AAS.Submodels.Variables.ContactInformationSubmodelType_Zipcode, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId ContactInformationSubmodelType_CityTown = new ExpandedNodeId(I4AAS.Submodels.Variables.ContactInformationSubmodelType_CityTown, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId ContactInformationSubmodelType_NationalCode = new ExpandedNodeId(I4AAS.Submodels.Variables.ContactInformationSubmodelType_NationalCode, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId MarkingSubmodelType_MarkingName = new ExpandedNodeId(I4AAS.Submodels.Variables.MarkingSubmodelType_MarkingName, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId MarkingSubmodelType_DesignationOfCertificateOrApproval = new ExpandedNodeId(I4AAS.Submodels.Variables.MarkingSubmodelType_DesignationOfCertificateOrApproval, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId MarkingSubmodelType_IssueDate = new ExpandedNodeId(I4AAS.Submodels.Variables.MarkingSubmodelType_IssueDate, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId MarkingSubmodelType_ExpiryDate = new ExpandedNodeId(I4AAS.Submodels.Variables.MarkingSubmodelType_ExpiryDate, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId MarkingSubmodelType_MarkingFile_Size = new ExpandedNodeId(I4AAS.Submodels.Variables.MarkingSubmodelType_MarkingFile_Size, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId MarkingSubmodelType_MarkingFile_Writable = new ExpandedNodeId(I4AAS.Submodels.Variables.MarkingSubmodelType_MarkingFile_Writable, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId MarkingSubmodelType_MarkingFile_UserWritable = new ExpandedNodeId(I4AAS.Submodels.Variables.MarkingSubmodelType_MarkingFile_UserWritable, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId MarkingSubmodelType_MarkingFile_OpenCount = new ExpandedNodeId(I4AAS.Submodels.Variables.MarkingSubmodelType_MarkingFile_OpenCount, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId MarkingSubmodelType_MarkingFile_Open_InputArguments = new ExpandedNodeId(I4AAS.Submodels.Variables.MarkingSubmodelType_MarkingFile_Open_InputArguments, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId MarkingSubmodelType_MarkingFile_Open_OutputArguments = new ExpandedNodeId(I4AAS.Submodels.Variables.MarkingSubmodelType_MarkingFile_Open_OutputArguments, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId MarkingSubmodelType_MarkingFile_Close_InputArguments = new ExpandedNodeId(I4AAS.Submodels.Variables.MarkingSubmodelType_MarkingFile_Close_InputArguments, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId MarkingSubmodelType_MarkingFile_Read_InputArguments = new ExpandedNodeId(I4AAS.Submodels.Variables.MarkingSubmodelType_MarkingFile_Read_InputArguments, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId MarkingSubmodelType_MarkingFile_Read_OutputArguments = new ExpandedNodeId(I4AAS.Submodels.Variables.MarkingSubmodelType_MarkingFile_Read_OutputArguments, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId MarkingSubmodelType_MarkingFile_Write_InputArguments = new ExpandedNodeId(I4AAS.Submodels.Variables.MarkingSubmodelType_MarkingFile_Write_InputArguments, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId MarkingSubmodelType_MarkingFile_GetPosition_InputArguments = new ExpandedNodeId(I4AAS.Submodels.Variables.MarkingSubmodelType_MarkingFile_GetPosition_InputArguments, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId MarkingSubmodelType_MarkingFile_GetPosition_OutputArguments = new ExpandedNodeId(I4AAS.Submodels.Variables.MarkingSubmodelType_MarkingFile_GetPosition_OutputArguments, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId MarkingSubmodelType_MarkingFile_SetPosition_InputArguments = new ExpandedNodeId(I4AAS.Submodels.Variables.MarkingSubmodelType_MarkingFile_SetPosition_InputArguments, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId MarkingSubmodelType_MarkingAdditionalText = new ExpandedNodeId(I4AAS.Submodels.Variables.MarkingSubmodelType_MarkingAdditionalText, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId ProductCarbonFootprintSubmodelType_PCFCalculationMethod = new ExpandedNodeId(I4AAS.Submodels.Variables.ProductCarbonFootprintSubmodelType_PCFCalculationMethod, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId ProductCarbonFootprintSubmodelType_PCFCO2eq = new ExpandedNodeId(I4AAS.Submodels.Variables.ProductCarbonFootprintSubmodelType_PCFCO2eq, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId ProductCarbonFootprintSubmodelType_PCFReferenceValueForCalculation = new ExpandedNodeId(I4AAS.Submodels.Variables.ProductCarbonFootprintSubmodelType_PCFReferenceValueForCalculation, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId ProductCarbonFootprintSubmodelType_PCFQuantityOfMeasureForCalculation = new ExpandedNodeId(I4AAS.Submodels.Variables.ProductCarbonFootprintSubmodelType_PCFQuantityOfMeasureForCalculation, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId ProductCarbonFootprintSubmodelType_PCFLiveCyclePhase = new ExpandedNodeId(I4AAS.Submodels.Variables.ProductCarbonFootprintSubmodelType_PCFLiveCyclePhase, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId ProductCarbonFootprintSubmodelType_PCFGoodsAddressHandover_ModelId = new ExpandedNodeId(I4AAS.Submodels.Variables.ProductCarbonFootprintSubmodelType_PCFGoodsAddressHandover_ModelId, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId ProductCarbonFootprintSubmodelType_PCFGoodsAddressHandover_ModelIdShort = new ExpandedNodeId(I4AAS.Submodels.Variables.ProductCarbonFootprintSubmodelType_PCFGoodsAddressHandover_ModelIdShort, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId AddressSubmodelType_Street = new ExpandedNodeId(I4AAS.Submodels.Variables.AddressSubmodelType_Street, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId AddressSubmodelType_HouseNumber = new ExpandedNodeId(I4AAS.Submodels.Variables.AddressSubmodelType_HouseNumber, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId AddressSubmodelType_ZipCode = new ExpandedNodeId(I4AAS.Submodels.Variables.AddressSubmodelType_ZipCode, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId AddressSubmodelType_CityTown = new ExpandedNodeId(I4AAS.Submodels.Variables.AddressSubmodelType_CityTown, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId AddressSubmodelType_Country = new ExpandedNodeId(I4AAS.Submodels.Variables.AddressSubmodelType_Country, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId AddressSubmodelType_Latitude = new ExpandedNodeId(I4AAS.Submodels.Variables.AddressSubmodelType_Latitude, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId AddressSubmodelType_Longitude = new ExpandedNodeId(I4AAS.Submodels.Variables.AddressSubmodelType_Longitude, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId I4AAS_BinarySchema = new ExpandedNodeId(I4AAS.Submodels.Variables.I4AAS_BinarySchema, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId I4AAS_BinarySchema_NamespaceUri = new ExpandedNodeId(I4AAS.Submodels.Variables.I4AAS_BinarySchema_NamespaceUri, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId I4AAS_BinarySchema_Deprecated = new ExpandedNodeId(I4AAS.Submodels.Variables.I4AAS_BinarySchema_Deprecated, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId I4AAS_BinarySchema_SubmodelDataType = new ExpandedNodeId(I4AAS.Submodels.Variables.I4AAS_BinarySchema_SubmodelDataType, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId I4AAS_BinarySchema_AssetDescriptionFileDataType = new ExpandedNodeId(I4AAS.Submodels.Variables.I4AAS_BinarySchema_AssetDescriptionFileDataType, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId I4AAS_BinarySchema_NameplateSubmodelDataType = new ExpandedNodeId(I4AAS.Submodels.Variables.I4AAS_BinarySchema_NameplateSubmodelDataType, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId I4AAS_BinarySchema_ContactInformationSubmodelDataType = new ExpandedNodeId(I4AAS.Submodels.Variables.I4AAS_BinarySchema_ContactInformationSubmodelDataType, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId I4AAS_BinarySchema_MarkingSubmodelDataType = new ExpandedNodeId(I4AAS.Submodels.Variables.I4AAS_BinarySchema_MarkingSubmodelDataType, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId I4AAS_BinarySchema_ProductCarbonFootprintDataType = new ExpandedNodeId(I4AAS.Submodels.Variables.I4AAS_BinarySchema_ProductCarbonFootprintDataType, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId I4AAS_BinarySchema_AddressSubmodelDataType = new ExpandedNodeId(I4AAS.Submodels.Variables.I4AAS_BinarySchema_AddressSubmodelDataType, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId I4AAS_XmlSchema = new ExpandedNodeId(I4AAS.Submodels.Variables.I4AAS_XmlSchema, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId I4AAS_XmlSchema_NamespaceUri = new ExpandedNodeId(I4AAS.Submodels.Variables.I4AAS_XmlSchema_NamespaceUri, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId I4AAS_XmlSchema_Deprecated = new ExpandedNodeId(I4AAS.Submodels.Variables.I4AAS_XmlSchema_Deprecated, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId I4AAS_XmlSchema_SubmodelDataType = new ExpandedNodeId(I4AAS.Submodels.Variables.I4AAS_XmlSchema_SubmodelDataType, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId I4AAS_XmlSchema_AssetDescriptionFileDataType = new ExpandedNodeId(I4AAS.Submodels.Variables.I4AAS_XmlSchema_AssetDescriptionFileDataType, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId I4AAS_XmlSchema_NameplateSubmodelDataType = new ExpandedNodeId(I4AAS.Submodels.Variables.I4AAS_XmlSchema_NameplateSubmodelDataType, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId I4AAS_XmlSchema_ContactInformationSubmodelDataType = new ExpandedNodeId(I4AAS.Submodels.Variables.I4AAS_XmlSchema_ContactInformationSubmodelDataType, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId I4AAS_XmlSchema_MarkingSubmodelDataType = new ExpandedNodeId(I4AAS.Submodels.Variables.I4AAS_XmlSchema_MarkingSubmodelDataType, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId I4AAS_XmlSchema_ProductCarbonFootprintDataType = new ExpandedNodeId(I4AAS.Submodels.Variables.I4AAS_XmlSchema_ProductCarbonFootprintDataType, I4AAS.Submodels.Namespaces.I4AAS);

        /// <remarks />
        public static readonly ExpandedNodeId I4AAS_XmlSchema_AddressSubmodelDataType = new ExpandedNodeId(I4AAS.Submodels.Variables.I4AAS_XmlSchema_AddressSubmodelDataType, I4AAS.Submodels.Namespaces.I4AAS);
    }
    #endregion

    #region BrowseName Declarations
    /// <remarks />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public static partial class BrowseNames
    {
        /// <remarks />
        public const string AddressSubmodelDataType = "AddressSubmodelDataType";

        /// <remarks />
        public const string AddressSubmodelType = "AddressSubmodelType";

        /// <remarks />
        public const string AssetDescriptionFileDataType = "AssetDescriptionFileDataType";

        /// <remarks />
        public const string AssetFile = "AssetFile";

        /// <remarks />
        public const string AssetModelType = "AssetModelType";

        /// <remarks />
        public const string CityTown = "CityTown";

        /// <remarks />
        public const string CompanyLogo = "CompanyLogo";

        /// <remarks />
        public const string ContactInformation = "ContactInformation";

        /// <remarks />
        public const string ContactInformationSubmodelDataType = "ContactInformationSubmodelDataType";

        /// <remarks />
        public const string ContactInformationSubmodelType = "ContactInformationSubmodelType";

        /// <remarks />
        public const string Country = "Country";

        /// <remarks />
        public const string CountryOfOrigin = "CountryOfOrigin";

        /// <remarks />
        public const string DateOfManufacture = "DateOfManufacture";

        /// <remarks />
        public const string DesignationOfCertificateOrApproval = "DesignationOfCertificateOrApproval";

        /// <remarks />
        public const string ExpiryDate = "ExpiryDate";

        /// <remarks />
        public const string FirmwareVersion = "FirmwareVersion";

        /// <remarks />
        public const string HardwareVersion = "HardwareVersion";

        /// <remarks />
        public const string HouseNumber = "HouseNumber";

        /// <remarks />
        public const string I4AAS_BinarySchema = "I4AAS.Submodels";

        /// <remarks />
        public const string I4AAS_XmlSchema = "I4AAS.Submodels";

        /// <remarks />
        public const string IssueDate = "IssueDate";

        /// <remarks />
        public const string Latitude = "Latitude";

        /// <remarks />
        public const string Longitude = "Longitude";

        /// <remarks />
        public const string ManufacturerName = "ManufacturerName";

        /// <remarks />
        public const string ManufacturerProductDesignation = "ManufacturerProductDesignation";

        /// <remarks />
        public const string ManufacturerProductFamily = "ManufacturerProductFamily";

        /// <remarks />
        public const string ManufacturerProductRoot = "ManufacturerProductRoot";

        /// <remarks />
        public const string ManufacturerProductType = "ManufacturerProductType";

        /// <remarks />
        public const string MarkingAdditionalText = "MarkingAdditionalText";

        /// <remarks />
        public const string MarkingFile = "MarkingFile";

        /// <remarks />
        public const string MarkingName = "MarkingName";

        /// <remarks />
        public const string Markings_Placeholder = "<MarkingName>";

        /// <remarks />
        public const string MarkingSubmodelDataType = "MarkingSubmodelDataType";

        /// <remarks />
        public const string MarkingSubmodelType = "MarkingSubmodelType";

        /// <remarks />
        public const string ModelDescription = "ModelDescription";

        /// <remarks />
        public const string ModelId = "ModelId";

        /// <remarks />
        public const string ModelIdShort = "ModelIdShort";

        /// <remarks />
        public const string Nameplate = "Nameplate";

        /// <remarks />
        public const string NameplateSubmodelDataType = "NameplateSubmodelDataType";

        /// <remarks />
        public const string NameplateSubmodelType = "NameplateSubmodelType";

        /// <remarks />
        public const string NationalCode = "NationalCode";

        /// <remarks />
        public const string OrderCodeOfManufacturer = "OrderCodeOfManufacturer";

        /// <remarks />
        public const string PCFCalculationMethod = "PCFCalculationMethod";

        /// <remarks />
        public const string PCFCO2eq = "PCFCO2eq";

        /// <remarks />
        public const string PCFGoodsAddressHandover = "PCFGoodsAddressHandover";

        /// <remarks />
        public const string PCFLiveCyclePhase = "PCFLiveCyclePhase";

        /// <remarks />
        public const string PCFQuantityOfMeasureForCalculation = "PCFQuantityOfMeasureForCalculation";

        /// <remarks />
        public const string PCFReferenceValueForCalculation = "PCFReferenceValueForCalculation";

        /// <remarks />
        public const string ProductArticleNumberOfManufacturer = "ProductArticleNumberOfManufacturer";

        /// <remarks />
        public const string ProductCarbonFootprintDataType = "ProductCarbonFootprintDataType";

        /// <remarks />
        public const string ProductCarbonFootprintSubmodelType = "ProductCarbonFootprintSubmodelType";

        /// <remarks />
        public const string SerialNumber = "SerialNumber";

        /// <remarks />
        public const string SoftwareVersion = "SoftwareVersion";

        /// <remarks />
        public const string Street = "Street";

        /// <remarks />
        public const string SubmodelDataType = "SubmodelDataType";

        /// <remarks />
        public const string Submodels_Placeholder = "<Submodels>";

        /// <remarks />
        public const string SubmodelType = "SubmodelType";

        /// <remarks />
        public const string URIOfTheProduct = "URIOfTheProduct";

        /// <remarks />
        public const string YearOfConstruction = "YearOfConstruction";

        /// <remarks />
        public const string Zipcode = "Zipcode";

        /// <remarks />
        public const string ZipCode = "ZipCode";
    }
    #endregion

    #region Namespace Declarations
    /// <remarks />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public static partial class Namespaces
    {
        /// <summary>
        /// The URI for the I4AAS namespace (.NET code namespace is 'I4AAS.Submodels').
        /// </summary>
        public const string I4AAS = "http://opcfoundation.org/UA/I4AAS/";

        /// <summary>
        /// The URI for the I4AASXsd namespace (.NET code namespace is 'I4AAS.Submodels').
        /// </summary>
        public const string I4AASXsd = "http://opcfoundation.org/UA/I4AAS/Types.xsd";

        /// <summary>
        /// The URI for the OpcUa namespace (.NET code namespace is 'Opc.Ua').
        /// </summary>
        public const string OpcUa = "http://opcfoundation.org/UA/";

        /// <summary>
        /// The URI for the OpcUaXsd namespace (.NET code namespace is 'Opc.Ua').
        /// </summary>
        public const string OpcUaXsd = "http://opcfoundation.org/UA/2008/02/Types.xsd";

        /// <summary>
        /// The URI for the IRDI namespace (.NET code namespace is 'I4AAS.IRDI').
        /// </summary>
        public const string IRDI = "http://opcfoundation.org/UA/Dictionary/IRDI";

        /// <summary>
        /// The URI for the IRDIXsd namespace (.NET code namespace is 'I4AAS.IRDI').
        /// </summary>
        public const string IRDIXsd = "http://opcfoundation.org/UA/Dictionary/IRDI/Types.xsd";
    }
    #endregion
}