using EmailSmsMarketing.BusinessLogic.Services.MailService;
using EmailSmsMarketing.Domain.Entities.Responses;
using EmailSmsMarketing.Domain.Entities.Responses.MailModels.ContactLists.List;
using Newtonsoft.Json;
using EmailSmsMarketing.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;
using EmailSmsMarketing.Domain.Entities.ContactLists;
using EmailSmsMarketing.Domain.Entities.Responses.MailModels.ContactLists;
using System.Runtime.CompilerServices;

namespace EmailSmsMarketing.BusinessLogic.Queries
{
    public class ContactListQuery
    {
        private readonly GlobalQuery globalQuery = new GlobalQuery();
        
        public async Task<ContactListsResponse> GetList(string token)
        {
            try
            {
                var url = MailURLs.GetContactLists(token);
                var Credentials = MailURLs.Credentials();

                QueryDataGet queryDataGet = new QueryDataGet()
                {
                    URL = url,
                    Credentials = Credentials
                };

                var queryResponse = await globalQuery.GetAsync(queryDataGet);

                return JsonConvert.DeserializeObject<ContactListsResponse>(queryResponse);

                
            }
            catch(Exception ex)
            {
                ContactListsResponse contactListsResponse = new ContactListsResponse()
                {
                    ErrorCode = ErrorCode.Internal_error,
                    ErrorMessage = ex.Message,
                };

                return contactListsResponse;
            }
        }

        public async Task<ContactListByIdResponse> GetContactListById(string token, int id)
        {
            try
            {
                var url = MailURLs.GetContactList(token, id);
                var Credentials = MailURLs.Credentials();

                QueryDataGet queryDataGet = new QueryDataGet
                {
                    URL = url,
                    Credentials = Credentials
                };

                var queryResponse = await globalQuery.GetAsync(queryDataGet);

                return JsonConvert.DeserializeObject<ContactListByIdResponse>(queryResponse);
            }
            catch(Exception ex)
            {
                ContactListByIdResponse contactListsResponse = new ContactListByIdResponse()
                {
                    ErrorCode = ErrorCode.Internal_error,
                    ErrorMessage = ex.Message,
                };

                return contactListsResponse;
            }
        }

        public async Task<BaseResponse> DeleteContactListById(string token, int id)
        {
            try
            {
                var url = MailURLs.DeleteContactList(token, id);
                var credentials = MailURLs.Credentials();

                QueryDataGet queryDataGet = new QueryDataGet()
                {
                    URL= url,
                    Credentials = credentials,
                };

                var queryResponse = await globalQuery.GetAsync(queryDataGet);

                return JsonConvert.DeserializeObject<BaseResponse>(queryResponse);
            }
            catch (Exception ex)
            {
                var response = new BaseResponse()
                {
                    ErrorCode = ErrorCode.Internal_error,
                    ErrorMessage = ex.Message
                };

                return response;
            }
        }
        public async Task<BaseResponse> UpdateContactList(ContactListsData data)
        {
            try
            {
                var url = MailURLs.UpdateContactList();
                var credentials = MailURLs.Credentials();
                var json = JsonConvert.SerializeObject(data);

                QueryDataPost queryDataPost = new QueryDataPost()
                {
                    URL = url,
                    Credentials = credentials,
                    JSON = json,
                };
                var queryResponse = await globalQuery.PostAsync(queryDataPost);
                return JsonConvert.DeserializeObject<BaseResponse>(queryResponse);

            }
            catch (Exception ex)
            {
                var response = new BaseResponse()
                {
                    ErrorCode = ErrorCode.Internal_error,
                    ErrorMessage = ex.Message
                };

                return response;
            }
        }

    }
}
