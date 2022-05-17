using DTO.General.Api.Enum;
using DTO.Integration.Wix.Category.Output;
using DTO.Integration.Wix.Post.Output;
using DTO.Integration.Wix.Token.Input;
using DTO.Integration.Wix.Token.Output;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Services.Integration.Wix
{
    internal class WixApiService
    {
        private readonly ApiDispatcher _apiDispatcher;
        private static readonly string BlogBaseUrl = "http://www.wixapis.com/blog/v3";
        private static readonly string OAuthUrl = "https://www.wix.com/oauth/access";
        private static readonly string ClientId = "6ddea4eb-2b2d-4666-80a8-e21208fe9004";
        private static readonly string ClientSecret = "4f43f6db-0050-4471-a147-1d6fb1ac35a6";
        private static readonly string RefreshToken = "OAUTH2.eyJraWQiOiJkZ0x3cjNRMCIsImFsZyI6IkhTMjU2In0.eyJkYXRhIjoie1wiaWRcIjpcImVjMzQ3ZjA3LTBkOWYtNDg2ZC05NjA0LTg1YmJiOGExYzA3ZlwifSIsImlhdCI6MTY0Mzk4MDM4NSwiZXhwIjoxNzA3MDUyMzg1fQ.AIW2Y6EClHqTqLK_6QidnVfG7CdCl4IKQWQbBhMK5ZY";
        private static string Token;

        // O Code é utilizado para gerar o Token e RefreshToken
        //private static readonly string Code = "OAUTH2.eyJraWQiOiJWUTQwMVRlWiIsImFsZyI6IkhTMjU2In0.eyJkYXRhIjoie1wiYXBwSWRcIjpcIjZkZGVhNGViLTJiMmQtNDY2Ni04MGE4LWUyMTIwOGZlOTAwNFwiLFwiaW5zdGFuY2VJZFwiOlwiZTQzZjc5NjUtY2MyYS00Yzc5LThlMzgtZDlkMTU3MTM2MTZjXCIsXCJzY29wZVwiOltcIlNDT1BFLkRDLUJPT0tJTkdTLlJFQUQtQk9PS0lOR1MtUFVCTElDXCIsXCJTQ09QRS5EQy1CTE9HLlJFQUQtQkxPR1NcIixcIlNDT1BFLkRDLk1BTkFHRS1ZT1VSLUFQUFwiLFwiU0NPUEUuREMtQk9PS0lOR1MuTUFOQUdFLUJPT0tJTkdTXCIsXCJTQ09QRS5EQy1CT09LSU5HUy5SRUFELUJPT0tJTkdTLVNFTlNJVElWRVwiLFwiU0NPUEUuREMtQk9PS0lOR1MtTUVHQS5SRUFELUJPT0tJTkdTXCIsXCJTQ09QRS5EQy1BUFBTLk1BTkFHRS1FTUJFRERFRC1TQ1JJUFRTXCIsXCJTQ09QRS5EQy1CT09LSU5HUy5SRUFELUNBTEVOREFSXCIsXCJTQ09QRS5EQy1CT09LSU5HUy1NRUdBLk1BTkFHRS1CT09LSU5HU1wiLFwiU0NPUEUuREMtQk9PS0lOR1MuU0VTU0lPTlMtUkVTT1VSQ0VTXCIsXCJTQ09QRS5EQy1BUFAtTUFSS0VULkdFVC1PV05FUlwiXX0iLCJpYXQiOjE2NDM5ODAzMzUsImV4cCI6MTY0Mzk4MDkzNX0.pEpi6FkgMKHaLvpYgc5Ug02iz6G7liJH_y2KUpJS-V4";

        public WixApiService()
        {
            _apiDispatcher = new ApiDispatcher();
        }

        public async Task<GetWixPostsListOutput> GetPosts(int? offset)
        {
            Token = (await GetToken().ConfigureAwait(false))?.AccessToken;
            return await SendRequest<GetWixPostsListOutput>(RequestMethodEnum.GET, $"{ BlogBaseUrl }/posts?featured=false&fieldsToInclude=CONTENT_TEXT&paging.offset={offset ?? 0}&paging.limit=50");
        }

        public async Task<GetWixPostDetailsOutput> GetPostBySlug(string slug)
        {
            Token = (await GetToken().ConfigureAwait(false))?.AccessToken;
            return await SendRequest<GetWixPostDetailsOutput>(RequestMethodEnum.GET, $"{ BlogBaseUrl }/posts/slugs/{slug}?fieldsToInclude=CONTENT_TEXT");
        }

        public async Task<GetWixPostDetailsOutput> GetPostById(string id, bool richContent)
        {
            Token = (await GetToken().ConfigureAwait(false))?.AccessToken;
            return await SendRequest<GetWixPostDetailsOutput>(RequestMethodEnum.GET, $"{ BlogBaseUrl }/posts/{id}?fieldsToInclude={ (richContent ? "RICH_CONTENT" : "CONTENT_TEXT") }");
        }

        public async Task<GetWixCategoryListOutput> GetCategories(int? offset)
        {
            Token = (await GetToken().ConfigureAwait(false))?.AccessToken;
            return await SendRequest<GetWixCategoryListOutput>(RequestMethodEnum.GET, $"{ BlogBaseUrl }/categories?paging.offset={offset ?? 0}&paging.limit=50");
        }

        public async Task<WixCategoryDetailsOutput> GetCategoryBySlug(string slug)
        {
            Token = (await GetToken().ConfigureAwait(false))?.AccessToken;
            return await SendRequest<WixCategoryDetailsOutput>(RequestMethodEnum.GET, $"{ BlogBaseUrl }/categories/slugs/{slug}");
        }

        public async Task<WixCategoryDetailsOutput> GetCategoryById(string id)
        {
            Token = (await GetToken().ConfigureAwait(false))?.AccessToken;
            return await SendRequest<WixCategoryDetailsOutput>(RequestMethodEnum.GET, $"{ BlogBaseUrl }/categories/{id}");
        }

        private async Task<WixTokenOutput> GetToken() => await SendRequest<WixTokenOutput>(RequestMethodEnum.POST, OAuthUrl, new RefreshWixTokenInput(ClientId, ClientSecret, RefreshToken));

        private static Tuple<HttpRequestHeader, string>[] DefaultHeaders() => new Tuple<HttpRequestHeader, string>[]
            {
                new Tuple<HttpRequestHeader, string>(HttpRequestHeader.ContentType, "application/json")
            };

        private static Tuple<string, string>[] WixHeaders() => string.IsNullOrEmpty(Token) ? null : new Tuple<string, string>[] { new Tuple<string, string>("Authorization", Token) };

        private async Task<T> SendRequest<T>(RequestMethodEnum method, string url, object body = null)
        {
            try
            {
                return await _apiDispatcher.DispatchWithResponseAsync<T>(url, method, body, DefaultHeaders(), WixHeaders());
            }
            catch
            {
                throw;
            }
        }
    }
}
