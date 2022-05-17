using DTO.Integration.Wix.Post.Output;
using System.Threading.Tasks;

namespace Services.Integration.Wix.Post
{
    public class WixPostService
    {
        internal WixApiService WixApiService;

        public WixPostService() => WixApiService = new();

        public async Task<GetWixPostsListOutput> GetPosts(int? offset) => await WixApiService.GetPosts(offset).ConfigureAwait(false);

        public async Task<WixPostDataOutput> GetPostBySlug(string input) => (await WixApiService.GetPostBySlug(input).ConfigureAwait(false))?.Post;

        public async Task<WixPostDataOutput> GetPostById(string input, bool richContent) => (await WixApiService.GetPostById(input, richContent).ConfigureAwait(false))?.Post;

    }
}
