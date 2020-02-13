using NewWq.DAL;
using NewWq.Dto;
using NewWq.IBLL;
using NewWq.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewWq.BLL
{
    public class CommodityManager : ICommodityManager
    {

        //分类操作
        public async Task CreateCategory(string name, Guid userId)
        {
            using (IDAL.ICategoryService categorySvc = new CategoryService())
            {
                await categorySvc.CreateAsync(new Models.Category()
                {
                    CategoryName = name,
                    UserId = userId
                });
            }
        }
        public async Task EditCategory(Guid categoryId, string newCategoryName)
        {
            using (IDAL.ICategoryService categorySvc = new CategoryService())
            {
                var category = await categorySvc.GetOneByIdAsync(categoryId);
                category.CategoryName = newCategoryName;
                await categorySvc.EditAsync(category);

            }
        }
        public async Task RemoveCategory(Guid categoryId)
        {
            using (IDAL.ICategoryService categorySvc = new CategoryService())
            {
                var category = await categorySvc.GetOneByIdAsync(categoryId);
                await categorySvc.RemoveAsync(category);
            }
        }
        public async Task<List<CategoryDto>> GetAllCategories(Guid userId)
        {
            using (IDAL.ICategoryService categorySvc = new DAL.CategoryService())
            {
                return await categorySvc.GetAll(m => m.UserId == userId).Select(m => new CategoryDto()
                {
                    UserId = m.UserId,
                    CategoryName = m.CategoryName,
                    CategoryId = m.Id
                }).ToListAsync();
            }
        }
        public async Task<List<CategoryDto>> GetAllCategories(int top)
        {
            using (IDAL.ICategoryService categorySvc = new DAL.CategoryService())
            {
                return await categorySvc.GetAll(m => true).Take(top).Select(m => new CategoryDto()
                {
                    UserId = m.UserId,
                    CategoryName = m.CategoryName,
                    CategoryId = m.Id
                }).ToListAsync();
            }
        }
        public async Task<CategoryDto> GetCategoryById(Guid id)
        {
            using (IDAL.ICategoryService categoryService = new DAL.CategoryService())
            {
                var category = await categoryService.GetOneByIdAsync(id);
                return new CategoryDto()
                {
                    CategoryId = category.Id,
                    CategoryName = category.CategoryName,
                    UserId = category.UserId
                };

            }
        }


        //商品操作
        public async Task CreateCommodity(string title, string content, Guid[] categoryIds, Guid userId, string mainImg, string tbUrl)
        {
            using (IDAL.ICommodityService comSvc = new DAL.CommodityService())
            {
                Commodity commodity = new Commodity()
                {
                    Title = title,
                    Content = content,
                    UserId = userId,
                    MainImage = mainImg,
                    TaobaoUrl = tbUrl
                };
                await comSvc.CreateAsync(commodity);
                Guid commodityId = commodity.Id;

                using (IDAL.IComtoCategoryService comtoSvc = new DAL.ComtoCategoryService())
                {
                    foreach (var categoryId in categoryIds)
                    {
                        await comtoSvc.CreateAsync(new ComtoCategory()
                        {
                            CommodityId = commodityId,
                            CategoryId = categoryId
                        }, false);
                    }

                    await comtoSvc.SaveAsync();
                }
            }
        }
        public async Task EditCommodity(Guid commodityId, string title, string content, Guid[] categoryIds, string mainImg, string tbUrl)
        {
            using (IDAL.ICommodityService commodityService = new DAL.CommodityService())
            {
                var commodity = await commodityService.GetOneByIdAsync(commodityId);
                commodity.Title = title;
                commodity.Content = content;
                commodity.MainImage = mainImg;
                commodity.TaobaoUrl = tbUrl;
                await commodityService.EditAsync(commodity);

                using (IDAL.IComtoCategoryService comtoCategoryService = new DAL.ComtoCategoryService())
                {
                    //删除原有类别
                    foreach (var comtocateId in comtoCategoryService.GetAll(m => m.CommodityId == commodityId))
                    {
                        await comtoCategoryService.RemoveAsync(comtocateId, false);

                    }

                    //保存现有类别
                    foreach (var categoryId in categoryIds)
                    {
                        await comtoCategoryService.CreateAsync(new ComtoCategory()
                        {
                            CategoryId = categoryId,
                            CommodityId = commodityId
                        });
                    }

                    await comtoCategoryService.SaveAsync();

                }
            }
        }
        public async Task<bool> ExistsCommodity(Guid commodityId)
        {
            using (IDAL.ICommodityService commodityService = new DAL.CommodityService())
            {
                return await commodityService.GetAll(m => m.Id == commodityId).AnyAsync();
            }
        }
        public async Task<List<CommodityDto>> GetAllCommodities(Guid userId)
        {
            using (IDAL.ICommodityService commodityService = new DAL.CommodityService())
            {
                var list = await commodityService.GetAll().Include(m => m.User).Where(m => m.UserId == userId).Select(m => new Dto.CommodityDto()
                {
                    Title = m.Title,
                    Content = m.Content,
                    GoodCount = m.GoodCount,
                    BadCount = m.BadCount,
                    CreateTime = m.CreateTime,
                    Id = m.Id,
                    MainImage = m.MainImage,
                    TaobaoUrl = m.TaobaoUrl,
                    Email = m.User.Email,
                    ImagePath = m.User.ImagePath
                }).ToListAsync();

                using (IDAL.IComtoCategoryService comtoCategoryService = new DAL.ComtoCategoryService())
                {
                    foreach (var item in list)
                    {
                        var cates = await comtoCategoryService.GetAll(m => m.CommodityId == item.Id).Include(m => m.Category).ToListAsync();
                        item.CategoryIds = cates.Select(m => m.CategoryId).ToArray();
                        item.CategoryNames = cates.Select(m => m.Category.CategoryName).ToArray();
                    }

                }

                return list;
            }
        }
        public async Task<List<CommodityDto>> GetAllCommodities(Guid userId, int pageIndex = 1, int pageSize = 3, bool asc = true)
        {
            using (IDAL.ICommodityService commodityService = new DAL.CommodityService())
            {
                var list = await commodityService.GetAll(m => m.UserId == userId, asc, pageSize, pageIndex).Include(m => m.User).Select(m => new Dto.CommodityDto()
                {
                    Title = m.Title,
                    Content = m.Content,
                    GoodCount = m.GoodCount,
                    BadCount = m.BadCount,
                    CreateTime = m.CreateTime,
                    Id = m.Id,
                    MainImage = m.MainImage,
                    TaobaoUrl = m.TaobaoUrl,
                    Email = m.User.Email,
                    ImagePath = m.User.ImagePath
                }).ToListAsync();

                using (IDAL.IComtoCategoryService comtoCategoryService = new DAL.ComtoCategoryService())
                {
                    foreach (var item in list)
                    {
                        var cates = await comtoCategoryService.GetAll(m => m.CommodityId == item.Id).Include(m => m.Category).ToListAsync();
                        item.CategoryIds = cates.Select(m => m.CategoryId).ToArray();
                        item.CategoryNames = cates.Select(m => m.Category.CategoryName).ToArray();
                    }

                }

                return list;
            }
        }

        public async Task<List<CommodityDto>> GetAllCommoditiesByCateId(Guid? categoryId, int pageIndex = 1, int pageSize = 3, bool asc = true)
        {
            List<CommodityDto> list = new List<CommodityDto>();
            using (IDAL.ICommodityService commodityService = new DAL.CommodityService())
            {
                using (IDAL.IComtoCategoryService comtoCategoryService = new DAL.ComtoCategoryService())
                {

                    if (categoryId != null)
                    {
                        List<Guid> comtocateList = await comtoCategoryService.GetAll(m => m.CategoryId == categoryId.Value).Select(m => m.CommodityId).ToListAsync();


                        list = await commodityService.GetAll(m => comtocateList.Contains(m.Id), asc, pageSize, pageIndex).Include(m => m.User).Select(m => new Dto.CommodityDto()
                        {
                            Title = m.Title,
                            Content = m.Content,
                            GoodCount = m.GoodCount,
                            BadCount = m.BadCount,
                            CreateTime = m.CreateTime,
                            Id = m.Id,
                            MainImage = m.MainImage,
                            TaobaoUrl = m.TaobaoUrl,
                            Email = m.User.Email,
                            ImagePath = m.User.ImagePath
                        }).ToListAsync();
                    }
                    else
                    {
                        list = await commodityService.GetAll(m => true, asc, pageSize, pageIndex).Include(m => m.User).Select(m => new Dto.CommodityDto()
                        {
                            Title = m.Title,
                            Content = m.Content,
                            GoodCount = m.GoodCount,
                            BadCount = m.BadCount,
                            CreateTime = m.CreateTime,
                            Id = m.Id,
                            MainImage = m.MainImage,
                            TaobaoUrl = m.TaobaoUrl,
                            Email = m.User.Email,
                            ImagePath = m.User.ImagePath
                        }).ToListAsync();

                    }

                    foreach (var item in list)
                    {
                        var cates = await comtoCategoryService.GetAll(m => m.CommodityId == item.Id).Include(m => m.Category).ToListAsync();
                        item.CategoryIds = cates.Select(m => m.CategoryId).ToArray();
                        item.CategoryNames = cates.Select(m => m.Category.CategoryName).ToArray();
                    }

                }

                return list;
            }
        }

        public async Task<List<CommodityDto>> GetAllCommodities(int count)
        {
            using (IDAL.ICommodityService commodityService = new DAL.CommodityService())
            {
                var list = await commodityService.GetAll(m => true, false).Take(count).Include(m => m.User).Select(m => new CommodityDto()
                {
                    Id = m.Id,
                    Title = m.Title,
                    Content = m.Content,
                    CreateTime = m.CreateTime,
                    Email = m.User.Email,
                    GoodCount = m.GoodCount,
                    BadCount = m.BadCount,
                    ImagePath = m.User.ImagePath,
                    MainImage = m.MainImage,
                    TaobaoUrl = m.TaobaoUrl
                }).ToListAsync();

                using (IDAL.IComtoCategoryService comtoCategoryService = new DAL.ComtoCategoryService())
                {
                    foreach (var item in list)
                    {
                        var contocates = await comtoCategoryService.GetAll(m => m.CommodityId == item.Id).Include(m => m.Category).ToListAsync();
                        item.CategoryIds = contocates.Select(m => m.CategoryId).ToArray();
                        item.CategoryNames = contocates.Select(m => m.Category.CategoryName).ToArray();
                    }
                }

                return list;
            }
        }

        public Task<List<CommodityDto>> GetAllCommoditiesByEmail(string email)
        {
            throw new NotImplementedException();
        }
        public async Task<CommodityDto> GetOneCommodityById(Guid commodityId)
        {
            using (IDAL.ICommodityService commodityService = new DAL.CommodityService())
            {
                var data = await commodityService.GetAll(m => m.Id == commodityId, false).Include(m => m.User).Select(m => new Dto.CommodityDto()
                {
                    Id = m.Id,
                    Title = m.Title,
                    Content = m.Content,
                    CreateTime = m.CreateTime,
                    Email = m.User.Email,
                    GoodCount = m.GoodCount,
                    BadCount = m.BadCount,
                    ImagePath = m.User.ImagePath,
                    MainImage = m.MainImage,
                    TaobaoUrl = m.TaobaoUrl,

                }).FirstAsync();



                using (IDAL.IComtoCategoryService comtoCategoryService = new DAL.ComtoCategoryService())
                {
                    var cates = await comtoCategoryService.GetAll(m => m.CommodityId == data.Id).Include(m => m.Category).ToListAsync();
                    data.CategoryIds = cates.Select(m => m.CategoryId).ToArray();
                    data.CategoryNames = cates.Select(m => m.Category.CategoryName).ToArray();

                    return data;
                }


            }
        }
        public async Task RemoveCommodity(Guid commodityId)
        {
            using (IDAL.ICommodityService commodityService = new DAL.CommodityService())
            {
                var commodity = await commodityService.GetOneByIdAsync(commodityId);
                await commodityService.RemoveAsync(commodity);
            }
        }


        public async Task<NextPrevDto> NextOne(Guid comid)
        {
            var commodity = await GetOneCommodityById(comid);
            using (IDAL.ICommodityService commodityService = new DAL.CommodityService())
            {
                var comNext = await commodityService.GetAll(m => m.CreateTime > commodity.CreateTime).FirstOrDefaultAsync();
                if (comNext == null)
                {
                    return null;
                }
                else
                {
                    return new NextPrevDto()
                    {
                        Id = comNext.Id,
                        Title = comNext.Title
                    };
                }
            }

        }

        public async Task<NextPrevDto> BeforeOne(Guid comid)
        {
            var commodity = await GetOneCommodityById(comid);
            using (IDAL.ICommodityService commodityService = new DAL.CommodityService())
            {
                var comNext = await commodityService.GetAll(m => m.CreateTime < commodity.CreateTime).FirstOrDefaultAsync();
                if (comNext == null)
                {
                    return null;
                }
                else
                {
                    return new NextPrevDto()
                    {
                        Id = comNext.Id,
                        Title = comNext.Title
                    };
                }
            }

        }


        //分页 获取数据条数
        public async Task<int> GetDataCount(Guid userId)
        {
            using (IDAL.ICommodityService commodityService = new DAL.CommodityService())
            {
                return await commodityService.GetAll().CountAsync(m => m.UserId == userId);
            }
        }

        public async Task<int> GetComCount(Guid? cateId)
        {
            using (IDAL.IComtoCategoryService comtoCategoryService = new DAL.ComtoCategoryService())
            {
                if (cateId == null)
                {
                    return await comtoCategoryService.GetAll().Select(m => m.CommodityId).Distinct().CountAsync();
                }
                else
                {
                    return await comtoCategoryService.GetAll(m => m.CategoryId == cateId.Value).CountAsync();
                }

            }
        }


        //点赞反对
        public async Task GoodCount(Guid commodityId)
        {
            using (IDAL.ICommodityService commodityService = new DAL.CommodityService())
            {
                var commodity = await commodityService.GetOneByIdAsync(commodityId);
                commodity.GoodCount++;
                await commodityService.EditAsync(commodity);
            }
        }

        public async Task BadCount(Guid commodityId)
        {
            using (IDAL.ICommodityService commodityService = new DAL.CommodityService())
            {
                var commodity = await commodityService.GetOneByIdAsync(commodityId);
                commodity.BadCount++;
                await commodityService.EditAsync(commodity);
            }
        }


        //评论操作
        public async Task CreateComment(Guid userId, Guid commodityId, string content)
        {
            using (IDAL.ICommentService commentService = new DAL.CommentService())
            {
                await commentService.CreateAsync(new Comment()
                {
                    UserId = userId,
                    CommodityId = commodityId,
                    Content = content
                });

            }
        }
        public async Task<List<CommentDto>> GetCommentByCommodityId(Guid commodityId)
        {
            using (IDAL.ICommentService commentService = new DAL.CommentService())
            {
                return await commentService.GetAll(m => m.CommodityId == commodityId, false).Include(m => m.User).Select(m => new Dto.CommentDto()
                {
                    Id = m.Id,
                    CommodityId = m.CommodityId,
                    UserId = m.UserId,
                    Email = m.User.Email,
                    Content = m.Content,
                    CreateTime = m.CreateTime
                }).ToListAsync();
            }
        }

        public async Task<List<ComtoCategoryDto>> GetAllCategoryIdsByCommodityId(Guid commodityId)
        {
            using (IDAL.IComtoCategoryService comtoCategoryService = new DAL.ComtoCategoryService())
            {
                return await comtoCategoryService.GetAll(m => m.CommodityId == commodityId).Select(m => new ComtoCategoryDto()
                {
                    CategoryId = m.CategoryId,
                    CommodityId = m.CommodityId
                }).ToListAsync();
            }
        }

        public async Task DeleteAllCategoryIdsByCommodityId(Guid commodityId)
        {
            using (IDAL.IComtoCategoryService comtoCategoryService = new DAL.ComtoCategoryService())
            {
                var comList = comtoCategoryService.GetAll(m => m.CommodityId == commodityId);
                foreach (var item in comList)
                {
                    await comtoCategoryService.RemoveAsync(item, false);
                }

                await comtoCategoryService.SaveAsync();

            }
        }

    }
}
