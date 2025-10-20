using System;
using System.Collections.Generic;
using Parking.Mobile.Interface.Interfaces;
using Parking.Mobile.Interface.Message.Request;
using Parking.Mobile.Interface.Message.Response;

namespace Parking.Mobile.ApplicationCore
{
    public class AppCredential : ICredentialController
    {
        public ResponseDefault<GetClientResponse> GetClientInfo(string parkingCode, int idClient)
        {
            List<CredentialItemInfo> lst = new List<CredentialItemInfo>();

            lst.Add(new CredentialItemInfo()
            {
                Active = true,
                Code = "1223344",
                DateStart = new DateTime(2025, 1, 1),
                DateEnd = new DateTime(2026, 1, 1),
                IDType = 1,
                Name = "Cred",
                TypeDescription = "Credenciado"
            });

            return new ResponseDefault<GetClientResponse>()
            {
                Success = true,
                Data = new GetClientResponse()
                {
                    Active = true,
                    Code = "999",
                    Name = "Cliente teste",
                    DateStart = new DateTime(2025, 1, 1),
                    DateEnd = new DateTime(2026, 1, 1),
                    Document = "999.999.999-99",
                    IDClient = 1,
                    GroupName = "Geral",
                    Credentials = lst,
                    Plates = new List<PlateInfo>(),
                    VacancyLimit = 1
                }
                };
        }

        public ResponseDefault<GetCredentialByDocumentResponse> GetCredentialByDocument(string parkingCode, string document, bool activeOnly)
        {
            List<CredentialItemInfo> lst = new List<CredentialItemInfo>();

            lst.Add(new CredentialItemInfo()
            {
                Active = true,
                Code = "1223344",
                DateStart = new DateTime(2025, 1, 1),
                DateEnd = new DateTime(2026, 1, 1),
                IDType = 1,
                Name = "Cred",
                TypeDescription = "Credenciado"
            });

            return new ResponseDefault<GetCredentialByDocumentResponse>()
            {
                Success = true,
                Data = new GetCredentialByDocumentResponse()
                {
                    Credentials = lst
                }
            };
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
            List<ClientInfo> lst = new List<ClientInfo>();

            lst.Add(new ClientInfo() { IDClient = 1, Name = "Cliente teste", Code = "999" });

            return new ResponseDefault<ListClientResponse>()
            {
                Success = true,
                Data = new ListClientResponse()
                {
                    Clients = lst
                }
            };
        }
    }
}

