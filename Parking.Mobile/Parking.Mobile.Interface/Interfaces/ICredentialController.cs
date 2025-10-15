using System;
using Parking.Mobile.Interface.Message.Request;
using Parking.Mobile.Interface.Message.Response;

namespace Parking.Mobile.Interface.Interfaces
{
    public interface ICredentialController
    {
        ResponseDefault<GetCredentialInfoResponse> GetCredentialInfo(GetCredentialInfoRequest request);

        ResponseDefault<GetCredentialByDocumentResponse> GetCredentialByDocument(string parkingCode, string document, bool activeOnly);

        ResponseDefault<ListClientResponse> ListClient(ListClientRequest request);

        ResponseDefault<GetClientResponse> GetClientInfo(string parkingCode, int idClient);
    }
}

