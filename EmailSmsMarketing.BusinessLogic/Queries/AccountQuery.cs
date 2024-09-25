using EmailSmsMarketing.BusinessLogic.Services.AuthService;
using EmailSmsMarketing.Domain.Entities.Authentication;
using EmailSmsMarketing.Domain.Entities.Responses;
using EmailSmsMarketing.Domain.Entities.Settings;
using EmailSmsMarketing.Domain.Entities.UserInfo;
using EmailSmsMarketing.Domain.Enum;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace EmailSmsMarketing.BusinessLogic.Queries
{
    public class AccountQuery
    {
        private readonly GlobalQuery globalQuery = new GlobalQuery();

        public async Task<AuthResponse> Authorize(AuthData data)
        {
            try
            {
                var url = AuthURLs.AuthorizeUser();
                var credentials = AuthURLs.Credentials();
                var json = JsonConvert.SerializeObject(data);

                QueryDataPost queryDataPost = new QueryDataPost()
                {
                    JSON = json,
                    URL = url,
                    Credentials = credentials
                };

                var queryResponse = await globalQuery.PostAsync(queryDataPost);

                return JsonConvert.DeserializeObject<AuthResponse>(queryResponse);
            }
            catch(Exception ex) 
            {
                AuthResponse authResponse = new AuthResponse()
                {
                    ErrorCode = ErrorCode.Internal_error,
                    ErrorMessage = ex.Message
                };

                return authResponse;
            }
        }

        public async Task<BaseResponse> ForgotPassword(ForgotPasswordData forgotPasswordViewModel)
        {
            try
            {
                var url = AuthURLs.ResetPassword();
                var credentials = AuthURLs.Credentials();
                var json = JsonConvert.SerializeObject(forgotPasswordViewModel);

                QueryDataPost queryDataPost = new QueryDataPost()
                {
                    JSON = json,
                    URL = url,
                    Credentials = credentials
                };

                var queryResponse = await globalQuery.PostAsync(queryDataPost);

                return JsonConvert.DeserializeObject<BaseResponse>(queryResponse);
            }
            catch (Exception ex)
            {
                BaseResponse baseResponse = new BaseResponse()
                {
                    ErrorCode = ErrorCode.Internal_error,
                    ErrorMessage = ex.Message,
                };

                return baseResponse;
            }
        }

        public BaseResponseToken RefreshToken(string Token)
        {
            try
            {
                var url = AuthURLs.RefreshToken(Token);
                var credentials = AuthURLs.Credentials();

                QueryDataGet queryDataGet = new QueryDataGet()
                {
                    URL = url,
                    Credentials = credentials
                };

                var queryResponse = globalQuery.Get(queryDataGet);

                return JsonConvert.DeserializeObject<BaseResponseToken>(queryResponse);
            }
            catch (Exception ex)
            {
                BaseResponseToken baseResponse = new BaseResponseToken()
                {
                    ErrorCode = ErrorCode.Internal_error,
                    ErrorMessage = ex.Message,
                };

                return baseResponse;
            }
        }

        public async Task<BaseResponse> ChangePasswordAsync(ChangePasswordData data)
        {
            try
            {
                var url = AuthURLs.ChangePassword();
                var credentials = AuthURLs.Credentials();
                var json = JsonConvert.SerializeObject(data);

                QueryDataPost queryDataPost = new QueryDataPost()
                {
                    URL = url,
                    Credentials = credentials,
                    JSON = json
                };

                var queryResponse = await globalQuery.PostAsync(queryDataPost);
                return JsonConvert.DeserializeObject<BaseResponse>(queryResponse);
            }
            catch(Exception ex)
            {
                BaseResponse baseResponse = new BaseResponse()
                {
                    ErrorCode = ErrorCode.Internal_error,
                    ErrorMessage = ex.Message,
                };

                return baseResponse;
            }
        }


    }
}
