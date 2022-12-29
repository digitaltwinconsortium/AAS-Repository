using IO.Swagger.Models;
using System.Collections.Generic;
using System.IO;

namespace IO.Swagger.V1RC03.Services
{
    public interface IAssetAdministrationShellEnvironmentService
    {
        AssetAdministrationShell GetAssetAdministrationShellById(string aasIdentifier, out int packageIndex);
  
        Submodel GetSubmodelById(string submodelIdentifier, out int packageIndex);
        
        List<AssetAdministrationShell> GetAllAssetAdministrationShells(List<string> assetIds = null, string idShort = null);
        
        List<ConceptDescription> GetAllConceptDescriptions(string idShort = null, Reference reqIsCaseOf = null, Reference reqDataSpecificationRef = null);
        
        object GetAllSubmodelElements(string aasIdentifier, string submodelIdentifier);
        
        object GetAllSubmodelElementsFromSubmodel(string submodelIdentifier);
        
        List<Reference> GetAllSubmodelReferences(string aasIdentifier);
        
        List<Submodel> GetAllSubmodels(Reference reqSemanticId = null, string idShort = null);
        
        AssetInformation GetAssetInformationFromAas(string aasIdentifier);
        
        ConceptDescription GetConceptDescriptionById(string cdIdentifier, out int packageIndex);
        
        void DeleteAssetAdministrationShellById(string aasIdentifier);
        
        void DeleteConceptDescriptionById(string cdIdentifier);
        
        void DeleteSubmodelById(string submodelIdentifier);
        
        void DeleteSubmodelReferenceById(string aasIdentifier, string submodelIdentifier);
        
        Submodel GetSubmodel(string aasIdentifier, string submodelIdentifier);
        
        SubmodelElement GetSubmodelElementByPathSubmodelRepo(string submodelIdentifier, string idShortPath, out object smeParent);
        
        SubmodelElement GetSubmodelElementByPath(string aasIdentifier, string submodelIdentifier, string idShortPath);
        
        void DeleteSubmodelElementByPathSubmodelRepo(string submodelIdentifier, string idShortPath);
        
        void DeleteSubmodelElementByPath(string aasIdentifier, string submodelIdentifier, string idShortPath);
        
        string GetFileByPathSubmodelRepo(string submodelIdentifier, string idShortPath, out byte[] byteArray, out long fileSize);
        
        string GetFileByPath(string aasIdentifier, string submodelIdentifier, string idShortPath, out byte[] content, out long fileSize);
        
        OperationResult GetOperationAsyncResultSubmodelRepo(string submodelIdentifier, string idShortPath, string handleId);
        
        OperationResult GetOperationAsyncResult(string aasIdentifier, string submodelIdentifier, string idShortPath, string handleId);
        
        OperationResult InvokeOperationSubmodelRepo(string submodelIdentifier, string idShortPath, OperationRequest operationRequest);
        
        OperationResult InvokeOperationAsyncSubmodelRepo(string submodelIdentifier, string idShortPath, OperationRequest operationRequest);
        
        AssetAdministrationShell CreateAssetAdministrationShell(AssetAdministrationShell body);
        
        ConceptDescription CreateConceptDescription(ConceptDescription body);
        
        Submodel CreateSubmodel(Submodel body);
        
        SubmodelElement CreateSubmodelElementSubmodelRepo(SubmodelElement body, string submodelIdentifier);
        
        SubmodelElement CreateSubmodelElement(SubmodelElement body, string aasIdentifier, string submodelIdentifier);
        
        SubmodelElement CreateSubmodelElementByPathSubmodelRepo(SubmodelElement body, string submodelIdentifier, string idShortPath);
        
        SubmodelElement CreateSubmodelElementByPath(SubmodelElement body, string aasIdentifier, string submodelIdentifier, string idShortPath);
        
        Reference CreateSubmodelReference(Reference body, string aasIdentifier);
        
        void UpdateAssetAdministrationShellById(AssetAdministrationShell body, string aasIdentifier);
        
        void UpdateAssetInformation(AssetInformation body, string aasIdentifier);
        
        void UpdateConceptDescriptionById(ConceptDescription body, string cdIdentifier);
        
        void UpdateSubmodelById(Submodel body, string submodelIdentifier);
        
        void UpdateSubmodel(Submodel body, string aasIdentifier, string submodelIdentifier);
        
        void UpdateSubmodelElementByPathSubmodelRepo(SubmodelElement body, string submodelIdentifier, string idShortPath);
        
        void UpdateSubmodelElementByPath(SubmodelElement body, string aasIdentifier, string submodelIdentifier, string idShortPath);
        
        void UpdateFileByPathSubmodelRepo(string decodedSubmodelId, string idShortPath, string fileName, string contentType, Stream fileContent);
        
        void UpdateFileByPath(string aasIdentifier, string submodelIdentifier, string idShortPath, string fileName, string contentType, Stream stream);
        
        void SecurityCheck(string objPath = "", string aasOrSubmodel = null, object objectAasOrSubmodel = null);
    }

}