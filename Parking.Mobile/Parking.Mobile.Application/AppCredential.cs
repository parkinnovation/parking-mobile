using System;
using Parking.Mobile.Interface.Interfaces;
using Parking.Mobile.Interface.Message.Request;
using Parking.Mobile.Interface.Message.Response;

namespace Parking.Mobile.ApplicationCore
{
    public class AppCredential : ICredentialController
    {
        public ResponseDefault<GetClientResponse> GetClientInfo(string parkingCode, int idClient)
        {
            throw new NotImplementedException();
        }

        public ResponseDefault<GetCredentialByDocumentResponse> GetCredentialByDocument(string parkingCode, string document, bool activeOnly)
        {
            throw new NotImplementedException();
        }

        public ResponseDefault<GetCredentialInfoResponse> GetCredentialInfo(GetCredentialInfoRequest request)
        {
            throw new NotImplementedException();
        }

        public ResponseDefault<ListClientResponse> ListClient(ListClientRequest request)
        {
            throw new NotImplementedException();
        }
    }
}

