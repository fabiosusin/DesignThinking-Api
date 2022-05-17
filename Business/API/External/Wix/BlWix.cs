using DAO.DBConnection;
using DAO.External.Wix;
using DAO.General.Log;
using DAO.Hub.Application.Wix;
using DTO.External.Wix.Database;
using DTO.External.Wix.Enum;
using DTO.External.Wix.Input;
using DTO.External.Wix.Output;
using DTO.General.Base.Api.Output;
using DTO.General.Log.Database;
using DTO.General.Log.Enum;
using DTO.Integration.Wix.Category.Output;
using DTO.Integration.Wix.Post.Output;
using Services.Integration.Wix.Category;
using Services.Integration.Wix.Post;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.API.External.Wix
{
    public class BlWix
    {
        protected WixPostService WixPostService;
        protected WixAllyTagDAO WixAllyCategoryDAO;
        protected LogHistoryDAO LogHistoryDAO;
        protected WixCategoryDAO WixCategoryDAO;
        protected WixTagDAO WixTagDAO;
        protected WixPostDAO WixPostDAO;
        protected WixCategoryService WixCategoryService;
        public BlWix(XDataDatabaseSettings settings)
        {
            WixAllyCategoryDAO = new(settings);
            WixCategoryDAO = new(settings);
            WixPostDAO = new(settings);
            LogHistoryDAO = new(settings);
            WixTagDAO = new(settings);
            WixCategoryService = new();
            WixPostService = new();
        }

        public BaseApiOutput RegisterTags(List<WixTagRegisterInput> tags)
        {
            if (!(tags?.Any() ?? false))
                return new("Nenhuma Tag informada!");

            foreach (var tag in tags)
                WixTagDAO.Insert(new(tag));

            return new(true);
        }

        public async Task<BaseApiOutput> RegisterPosts(DateTime? date)
        {
            var hasMore = false;
            var offset = 0;
            try
            {
                do
                {
                    var postList = await GetPosts(offset).ConfigureAwait(false);
                    if (!(postList?.Posts?.Any() ?? false))
                        return new("Nenhum post encontrado");

                    offset += 50;
                    hasMore = postList.MetaData.Total > offset;

                    var posts = date.HasValue && date.Value > DateTime.MinValue ? postList.Posts.Where(x => x.LastPublishedDate.Date == date.Value.Date) : postList.Posts;
                    if (!(postList?.Posts?.Any() ?? false))
                        continue;

                    var postsIds = posts.Select(x => x.Id);
                    var existingPosts = WixPostDAO.Find(x => postsIds.Contains(x.WixPostId));

                    foreach (var postId in postsIds)
                    {

                        var postRichContent = await GetPostById(postId, true).ConfigureAwait(false);
                        var paragraphs = GetPostParagraphs(postRichContent?.RichContent);

                        var post = posts.First(x => x.Id == postId);
                        var existing = existingPosts.FirstOrDefault(x => x.WixPostId == postId);

                        if (existing != null)
                        {
                            existing = new(existing.Id, post, paragraphs);
                            WixPostDAO.Update(existing);
                        }
                        else
                            WixPostDAO.Insert(new WixPost(post, paragraphs));
                    }
                }
                while (hasMore);
            }
            catch { return new($"Ocorreu um erro ao cadastrar os posts {(date.HasValue ? "do dia: " + date?.ToString("dd/MM/yyyy") : "")}"); }

            return new(true);
        }

        public async Task<BaseApiOutput> RegisterCategories()
        {
            var hasMore = false;
            var offset = 0;
            try
            {
                do
                {
                    var categoriesList = await GetCategories(null).ConfigureAwait(false);
                    if (!(categoriesList?.Categories?.Any() ?? false))
                        return new("Nenhuma categoria encontrada");

                    offset += 50;
                    hasMore = categoriesList.MetaData.Total > offset;

                    var categories = categoriesList.Categories;
                    if (!(categoriesList?.Categories?.Any() ?? false))
                        continue;

                    var categoriesIds = categories.Select(x => x.Id);
                    var existingCategories = WixCategoryDAO.Find(x => categoriesIds.Contains(x.WixCategoryId));

                    foreach (var categoryId in categoriesIds)
                    {
                        var wixCategory = categories.First(x => x.Id == categoryId);
                        var existing = existingCategories.FirstOrDefault(x => x.WixCategoryId == categoryId);

                        if (existing != null)
                        {
                            existing = new(wixCategory, existing.Id);
                            WixCategoryDAO.Update(existing);
                        }
                        else
                        {
                            var insertResult = WixCategoryDAO.Insert(new WixCategory(wixCategory));
                            if (insertResult?.Success ?? false)
                            {
                                var category = (WixCategory)insertResult.Data;
                                WixAllyCategoryDAO.InsertCategoryMasterAlly(category.Id);
                            }
                        }
                    }
                }
                while (hasMore);
            }
            catch { return new($"Ocorreu um erro ao cadastrar as categorias"); }

            return new(true);
        }

        public async Task<GetWixPostsListOutput> GetPosts(int? offset)
        {
            try
            {
                return await WixPostService.GetPosts(offset).ConfigureAwait(false);
            }
            catch { return null; }
        }

        public async Task<WixPostDataOutput> GetPostBySlug(string input)
        {
            try
            {
                return await WixPostService.GetPostBySlug(input).ConfigureAwait(false);
            }
            catch { return null; }
        }

        public async Task<WixPostDataOutput> GetPostById(string input, bool richContent)
        {
            try
            {
                return await WixPostService.GetPostById(input, richContent).ConfigureAwait(false);
            }
            catch { return null; }
        }

        public async Task<GetWixCategoryListOutput> GetCategories(int? offset)
        {
            try
            {
                return await WixCategoryService.GetCategories(offset).ConfigureAwait(false);
            }
            catch { return null; }
        }

        public async Task<WixCategoryDataOutput> GetCategoryBySlug(string input)
        {
            try
            {
                return await WixCategoryService.GetCategoryBySlug(input).ConfigureAwait(false);
            }
            catch { return null; }
        }

        public async Task<WixCategoryDataOutput> GetCategoryById(string input)
        {
            try
            {
                return await WixCategoryService.GetCategoryById(input).ConfigureAwait(false);
            }
            catch { return null; }
        }

        private static List<WixPostParagraphData> GetPostParagraphs(WixRichContent input)
        {
            if (input == null)
                return null;

            var result = new List<WixPostParagraphData>();

            foreach (var node in input.Nodes)
            {
                var data = node.Type.ToUpper() switch
                {
                    WixPostParagraphDataType.Heading => new WixPostParagraphData(WixPostParagraphType.Text, GetNodesParagraph(node)),
                    WixPostParagraphDataType.Paragraph => new WixPostParagraphData(WixPostParagraphType.Text, GetNodesParagraph(node)),
                    WixPostParagraphDataType.Image => new WixPostParagraphData(WixPostParagraphType.Image, node.ImageData?.Image?.Src?.Id),
                    _ => null
                };

                if (string.IsNullOrEmpty(data?.Data))
                    continue;

                result.Add(data);
            }

            return result;
        }

        private static string GetNodesParagraph(WixRichContentNode nodeInput)
        {
            if (nodeInput == null)
                return null;

            var result = string.Empty;
            foreach (var node in nodeInput.Nodes.Where(x => !string.IsNullOrEmpty(x?.TextData?.Text)))
                result += node.TextData?.Text;

            return result.Trim();
        }

    }
}
