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
            return new ResponseDefault<GetCredentialInfoResponse>()
            {
                Success = request.Plate == "AAA-0002",
                Data = new GetCredentialInfoResponse()
                {
                    IDClient = 1,
                    Name = "Cliente Teste",
                    ClientActive = true,
                    DateStart = new DateTime(2000, 1, 1),
                    DateEnd = new DateTime(2049, 1, 1),
                    Credential = "111",
                    CredentialActive = true
                }
            };
        }

        public ResponseDefault<ListClientResponse> ListClient(ListClientRequest request)
        {
            throw new NotImplementedException();
        }
    }
}

