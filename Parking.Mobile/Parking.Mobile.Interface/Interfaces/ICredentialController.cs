using System;
using Parking.Mobile.Interface.Message.Request;
using Parking.Mobile.Interface.Message.Response;

namespace Parking.Mobile.Interface.Interfaces
{
    public interface ICredentialController
    {
        ServiceResponseDefault<GetCredentialInfoResponse> GetCredentialInfo(GetCredentialInfoRequest request);

        ServiceResponseDefault<GetCredentialByDocumentResponse> GetCredentialByDocument(string parkingCode, string document, bool activeOnly);

        ServiceResponseDefault<ListClientResponse> ListClient(ListClientRequest request);

        ServiceResponseDefault<GetClientResponse> GetClientInfo(string parkingCode, int idClient);
    }
}

