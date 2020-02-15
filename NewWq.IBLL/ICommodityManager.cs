using NewWq.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NewWq.IBLL
{
    public interface ICommodityManager
    {
        Task CreateCommodity(string title, string content, Guid[] categoryIds, Guid userId, string mainImg, string tbUrl);

        //分类操作
        Task CreateCategory(string name, Guid userId);
        Task<List<Dto.CategoryDto>> GetAllCategories(Guid userId);
        Task<Dto.CategoryDto> GetCategoryById(Guid id);
        Task<List<CategoryDto>> GetAllCategories(int top);
        List<CategoryDto> GetAllCategoriesSync();
        Task<List<CategoryDto>> GetAllCategories();
        Task EditCategory(Guid categoryId, string newCategoryName);
        Task RemoveCategory(Guid categoryId);


        Task<List<Dto.CommodityDto>> GetAllCommodities(Guid userId);
        Task<List<Dto.CommodityDto>> GetAllCommodities(Guid userId, int pageIndex = 1, int pageSize = 3, bool asc = true);
        Task<List<Dto.CommodityDto>> GetAllCommoditiesByCateId(Guid? cateId, int pageIndex = 1, int pageSize = 3, bool asc = true);
        Task<List<Dto.CommodityDto>> GetAllCommoditiesByCateId(string cateId);
        Task<List<Dto.CommodityDto>> GetAllCommodities(int count);

        Task<int> GetDataCount(Guid userId);
        Task<int> GetComCount(Guid? cateId);
        Task<List<Dto.CommodityDto>> GetAllCommoditiesByEmail(string email);
        Task<List<Dto.ComtoCategoryDto>> GetAllCategoryIdsByCommodityId(Guid commodityId);

        Task DeleteAllCategoryIdsByCommodityId(Guid commodityId);


        Task RemoveCommodity(Guid commodityId);
        Task EditCommodity(Guid commodityId, string title, string content, Guid[] categoryIds, string mainImg, string tbUrl);

        Task<bool> ExistsCommodity(Guid commodityId);
        Task<Dto.CommodityDto> GetOneCommodityById(Guid commodityId);

        //点赞
        Task GoodCount(Guid commodityId);
        Task BadCount(Guid commodityId);

        //评论
        Task CreateComment(Guid userId, Guid commodityId, string content);
        Task<List<Dto.CommentDto>> GetCommentByCommodityId(Guid commodityId);


        Task<NextPrevDto> NextOne(Guid comid);

        Task<NextPrevDto> BeforeOne(Guid comid);
    }
}
