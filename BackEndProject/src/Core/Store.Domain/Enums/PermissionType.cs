using Store.Domain.Resources;
using System.ComponentModel.DataAnnotations;

namespace Store.Domain.Enums;

public enum PermissionType : int
{
    [Display(Name = "PermissionType_Product", ResourceType = typeof(EnumResources))]
    Product = 1,

    [Display(Name = "PermissionType_ManageUsersGroup", ResourceType = typeof(EnumResources))]
    ManageUsersGroup = 2,

    [Display(Name = "PermissionType_ManageUsers", ResourceType = typeof(EnumResources))]
    ManageUsers = 3,

    [Display(Name = "PermissionType_CreateUser", ResourceType = typeof(EnumResources))]
    CreateUser = 4,

    [Display(Name = "PermissionType_UpdateUser", ResourceType = typeof(EnumResources))]
    UpdateUser = 5,

    [Display(Name = "PermissionType_ListUser", ResourceType = typeof(EnumResources))]
    ListUser = 6,

    [Display(Name = "PermissionType_DeleteUser", ResourceType = typeof(EnumResources))]
    DeleteUser = 7,

    [Display(Name = "PermissionType_GetUserById", ResourceType = typeof(EnumResources))]
    GetUserById = 8,

    [Display(Name = "PermissionType_ChangeUserPassword", ResourceType = typeof(EnumResources))]
    ChangeUserPassword = 36,

    [Display(Name = "PermissionType_ManageCategoryGroup", ResourceType = typeof(EnumResources))]
    ManageCategoryGroup = 9,

    [Display(Name = "PermissionType_ManageCategory", ResourceType = typeof(EnumResources))]
    ManageCategory = 10,

    [Display(Name = "PermissionType_CreateCategory", ResourceType = typeof(EnumResources))]
    CreateCategory = 11,

    [Display(Name = "PermissionType_ManageCommentTopicGroup", ResourceType = typeof(EnumResources))]
    ManageCommentTopicGroup = 12,

    [Display(Name = "PermissionType_ManageCommentTopic", ResourceType = typeof(EnumResources))]
    ManageCommentTopic = 13,

    [Display(Name = "PermissionType_CreateCommentTopic", ResourceType = typeof(EnumResources))]
    CreateCommentTopic = 14,

    [Display(Name = "PermissionType_ManageDeliveryTypeGroup", ResourceType = typeof(EnumResources))]
    ManageDeliveryTypeGroup = 15,

    [Display(Name = "PermissionType_ManageDeliveryType", ResourceType = typeof(EnumResources))]
    ManageDeliveryType = 16,

    [Display(Name = "PermissionType_CreateDeliveryType", ResourceType = typeof(EnumResources))]
    CreateDeliveryType = 17,

    [Display(Name = "PermissionType_ManagePostTypeGroup", ResourceType = typeof(EnumResources))]
    ManagePostTypeGroup = 18,

    [Display(Name = "PermissionType_ManagePostType", ResourceType = typeof(EnumResources))]
    ManagePostType = 19,

    [Display(Name = "PermissionType_CreatePostType", ResourceType = typeof(EnumResources))]
    CreatePostType = 20,

    [Display(Name = "PermissionType_ManageRoleGroup", ResourceType = typeof(EnumResources))]
    ManageRoleGroup = 24,

    [Display(Name = "PermissionType_ManageRole", ResourceType = typeof(EnumResources))]
    ManageRole = 25,

    [Display(Name = "PermissionType_CreateRole", ResourceType = typeof(EnumResources))]
    CreateRole = 26,

    [Display(Name = "PermissionType_ManageStatusGroup", ResourceType = typeof(EnumResources))]
    ManageStatusGroup = 27,

    [Display(Name = "PermissionType_ManageStatus", ResourceType = typeof(EnumResources))]
    ManageStatus = 28,

    [Display(Name = "PermissionType_CreateStatus", ResourceType = typeof(EnumResources))]
    CreateStatus = 29,

    [Display(Name = "PermissionType_ManageSubCategoryGroup", ResourceType = typeof(EnumResources))]
    ManageSubCategoryGroup = 30,

    [Display(Name = "PermissionType_ManageSubCategory", ResourceType = typeof(EnumResources))]
    ManageSubCategory = 31,

    [Display(Name = "PermissionType_CreateSubCategory", ResourceType = typeof(EnumResources))]
    CreateSubCategory = 32,

    [Display(Name = "PermissionType_ManageUserAddressGroup", ResourceType = typeof(EnumResources))]
    ManageUserAddressGroup = 33,

    [Display(Name = "PermissionType_ManageUserAddress", ResourceType = typeof(EnumResources))]
    ManageUserAddress = 34,

    [Display(Name = "PermissionType_CreateUserAddress", ResourceType = typeof(EnumResources))]
    CreateUserAddress = 35,

    [Display(Name = "PermissionType_UpdateRole", ResourceType = typeof(EnumResources))]
    UpdateRole = 37,

    [Display(Name = "PermissionType_DeleteRole", ResourceType = typeof(EnumResources))]
    DeleteRole = 38,

    [Display(Name = "PermissionType_UpdateCategory", ResourceType = typeof(EnumResources))]
    UpdateCategory = 39,

    [Display(Name = "PermissionType_DeleteCategory", ResourceType = typeof(EnumResources))]
    DeleteCategory = 40,

    [Display(Name = "PermissionType_UpdateCommentTopic", ResourceType = typeof(EnumResources))]
    UpdateCommentTopic = 41,

    [Display(Name = "PermissionType_DeleteCommentTopic", ResourceType = typeof(EnumResources))]
    DeleteCommentTopic = 42,

    [Display(Name = "PermissionType_CreateDeliveryOption", ResourceType = typeof(EnumResources))]
    CreateDeliveryOption = 43,

    [Display(Name = "PermissionType_UpdateDeliveryOption", ResourceType = typeof(EnumResources))]
    UpdateDeliveryOption = 44,

    [Display(Name = "PermissionType_UpdateSubCategory", ResourceType = typeof(EnumResources))]
    UpdateSubCategory = 45,

    [Display(Name = "PermissionType_DeleteSubCategory", ResourceType = typeof(EnumResources))]
    DeleteSubCategory = 46,

    [Display(Name = "PermissionType_UpdatePostType", ResourceType = typeof(EnumResources))]
    UpdatePostType = 47,

    [Display(Name = "PermissionType_DeletePostType", ResourceType = typeof(EnumResources))]
    DeletePostType = 48,

    [Display(Name = "PermissionType_UpdateDeliveryType", ResourceType = typeof(EnumResources))]
    UpdateDeliveryType = 49,

    [Display(Name = "PermissionType_DeleteDeliveryType", ResourceType = typeof(EnumResources))]
    DeleteDeliveryType = 50,

    [Display(Name = "PermissionType_ManageLocationGroup", ResourceType = typeof(EnumResources))]
    ManageLocationGroup = 100,

    [Display(Name = "PermissionType_ManageProvince", ResourceType = typeof(EnumResources))]
    ManageProvince = 101,

    [Display(Name = "PermissionType_CreateProvince", ResourceType = typeof(EnumResources))]
    CreateProvince = 102,

    [Display(Name = "PermissionType_UpdateProvince", ResourceType = typeof(EnumResources))]
    UpdateProvince = 103,

    [Display(Name = "PermissionType_DeleteProvince", ResourceType = typeof(EnumResources))]
    DeleteProvince = 104,

    [Display(Name = "PermissionType_ManageCity", ResourceType = typeof(EnumResources))]
    ManageCity = 105,

    [Display(Name = "PermissionType_CreateCity", ResourceType = typeof(EnumResources))]
    CreateCity = 106,

    [Display(Name = "PermissionType_UpdateCity", ResourceType = typeof(EnumResources))]
    UpdateCity = 107,

    [Display(Name = "PermissionType_DeleteCity", ResourceType = typeof(EnumResources))]
    DeleteCity = 108,

    [Display(Name = "PermissionType_ManageProductGroup", ResourceType = typeof(EnumResources))]
    ManageProductGroup = 110,

    [Display(Name = "PermissionType_ManageProduct", ResourceType = typeof(EnumResources))]
    ManageProduct = 111,

    [Display(Name = "PermissionType_CreateProduct", ResourceType = typeof(EnumResources))]
    CreateProduct = 112,

    [Display(Name = "PermissionType_UpdateProduct", ResourceType = typeof(EnumResources))]
    UpdateProduct = 113,

    [Display(Name = "PermissionType_DeleteProduct", ResourceType = typeof(EnumResources))]
    DeleteProduct = 114,

    [Display(Name = "PermissionType_ManageProductPrice", ResourceType = typeof(EnumResources))]
    ManageProductPrice = 115,

    [Display(Name = "PermissionType_CreateProductPrice", ResourceType = typeof(EnumResources))]
    CreateProductPrice = 116,

    [Display(Name = "PermissionType_UpdateProductPrice", ResourceType = typeof(EnumResources))]
    UpdateProductPrice = 117,

    [Display(Name = "PermissionType_ManageProductDescription", ResourceType = typeof(EnumResources))]
    ManageProductDescription = 118,

    [Display(Name = "PermissionType_CreateProductDescription", ResourceType = typeof(EnumResources))]
    CreateProductDescription = 119,

    [Display(Name = "PermissionType_UpdateProductDescription", ResourceType = typeof(EnumResources))]
    UpdateProductDescription = 120,

    [Display(Name = "PermissionType_DeleteProductDescription", ResourceType = typeof(EnumResources))]
    DeleteProductDescription = 121,

    [Display(Name = "PermissionType_ManageProductFile", ResourceType = typeof(EnumResources))]
    ManageProductFile = 122,

    [Display(Name = "PermissionType_CreateProductFileRange", ResourceType = typeof(EnumResources))]
    CreateProductFileRange = 123,

    [Display(Name = "PermissionType_UpdateProductFileStatus", ResourceType = typeof(EnumResources))]
    UpdateProductFileStatus = 124,

    [Display(Name = "PermissionType_DeleteProductFile", ResourceType = typeof(EnumResources))]
    DeleteProductFile = 125,

    [Display(Name = "PermissionType_ManageProductPriceDeliveryOption", ResourceType = typeof(EnumResources))]
    ManageProductPriceDeliveryOption = 129,

    [Display(Name = "PermissionType_CreateProductPriceDeliveryOption", ResourceType = typeof(EnumResources))]
    CreateProductPriceDeliveryOption = 130,

    [Display(Name = "PermissionType_UpdateProductPriceDeliveryOption", ResourceType = typeof(EnumResources))]
    UpdateProductPriceDeliveryOption = 131,

    [Display(Name = "PermissionType_DeleteProductPriceDeliveryOption", ResourceType = typeof(EnumResources))]
    DeleteProductPriceDeliveryOption = 132,

    [Display(Name = "PermissionType_ManageOrderGroup", ResourceType = typeof(EnumResources))]
    ManageOrderGroup = 140,

    [Display(Name = "PermissionType_ManageOrder", ResourceType = typeof(EnumResources))]
    ManageOrder = 141,

    [Display(Name = "PermissionType_ManagePropertyGroup", ResourceType = typeof(EnumResources))]
    ManagePropertyGroup = 150,

    [Display(Name = "PermissionType_ManageProperty", ResourceType = typeof(EnumResources))]
    ManageProperty = 151,

    [Display(Name = "PermissionType_CreateProperty", ResourceType = typeof(EnumResources))]
    CreateProperty = 152,

    [Display(Name = "PermissionType_UpdateProperty", ResourceType = typeof(EnumResources))]
    UpdateProperty = 153,

    [Display(Name = "PermissionType_DeleteProperty", ResourceType = typeof(EnumResources))]
    DeleteProperty = 154,

    [Display(Name = "PermissionType_ManagePropertyCategory", ResourceType = typeof(EnumResources))]
    ManagePropertyCategory = 155,

    [Display(Name = "PermissionType_CreatePropertyCategory", ResourceType = typeof(EnumResources))]
    CreatePropertyCategory = 156,

    [Display(Name = "PermissionType_UpdatePropertyCategory", ResourceType = typeof(EnumResources))]
    UpdatePropertyCategory = 157,

    [Display(Name = "PermissionType_DeletePropertyCategory", ResourceType = typeof(EnumResources))]
    DeletePropertyCategory = 158,

    [Display(Name = "PermissionType_ManagePropertyItem", ResourceType = typeof(EnumResources))]
    ManagePropertyItem = 159,

    [Display(Name = "PermissionType_CreatePropertyItem", ResourceType = typeof(EnumResources))]
    CreatePropertyItem = 160,

    [Display(Name = "PermissionType_UpdatePropertyItem", ResourceType = typeof(EnumResources))]
    UpdatePropertyItem = 161,

    [Display(Name = "PermissionType_DeletePropertyItem", ResourceType = typeof(EnumResources))]
    DeletePropertyItem = 162,

    [Display(Name = "PermissionType_ManageTagGroup", ResourceType = typeof(EnumResources))]
    ManageTagGroup = 170,

    [Display(Name = "PermissionType_ManageTag", ResourceType = typeof(EnumResources))]
    ManageTag = 171,

    [Display(Name = "PermissionType_CreateTag", ResourceType = typeof(EnumResources))]
    CreateTag = 172,

    [Display(Name = "PermissionType_UpdateTag", ResourceType = typeof(EnumResources))]
    UpdateTag = 173,

    [Display(Name = "PermissionType_DeleteTag", ResourceType = typeof(EnumResources))]
    DeleteTag = 174,

    [Display(Name = "PermissionType_ManageBlogPostGroup", ResourceType = typeof(EnumResources))]
    ManageBlogPostGroup = 175,

    [Display(Name = "PermissionType_ManageBlogPost", ResourceType = typeof(EnumResources))]
    ManageBlogPost = 176,

    [Display(Name = "PermissionType_CreateBlogPost", ResourceType = typeof(EnumResources))]
    CreateBlogPost = 177,

    [Display(Name = "PermissionType_UpdateBlogPost", ResourceType = typeof(EnumResources))]
    UpdateBlogPost = 178,

    [Display(Name = "PermissionType_PublishBlogPost", ResourceType = typeof(EnumResources))]
    PublishBlogPost = 179,

    [Display(Name = "PermissionType_ManageBlogPostCategory", ResourceType = typeof(EnumResources))]
    ManageBlogPostCategory = 180,

    [Display(Name = "PermissionType_CreateBlogPostCategory", ResourceType = typeof(EnumResources))]
    CreateBlogPostCategory = 181,

    [Display(Name = "PermissionType_UpdateBlogPostCategory", ResourceType = typeof(EnumResources))]
    UpdateBlogPostCategory = 182,

    [Display(Name = "PermissionType_ManageBlogPostTag", ResourceType = typeof(EnumResources))]
    ManageBlogPostTag = 183,

    [Display(Name = "PermissionType_CreateBlogPostTag", ResourceType = typeof(EnumResources))]
    CreateBlogPostTag = 184,

    [Display(Name = "PermissionType_UpdateBlogPostTag", ResourceType = typeof(EnumResources))]
    UpdateBlogPostTag = 185,

    [Display(Name = "PermissionType_DeleteBlogPostTag", ResourceType = typeof(EnumResources))]
    DeleteBlogPostTag = 186,

    [Display(Name = "PermissionType_ManageBlogPostComment", ResourceType = typeof(EnumResources))]
    ManageBlogPostComment = 187,

    [Display(Name = "PermissionType_ApproveBlogPostComment", ResourceType = typeof(EnumResources))]
    ApproveBlogPostComment = 188,

    [Display(Name = "PermissionType_ManageBlogPostLike", ResourceType = typeof(EnumResources))]
    ManageBlogPostLike = 189,

    [Display(Name = "PermissionType_ManageCompanyGroup", ResourceType = typeof(EnumResources))]
    ManageCompanyGroup = 200,

    [Display(Name = "PermissionType_ManageCompany", ResourceType = typeof(EnumResources))]
    ManageCompany = 201,

    [Display(Name = "PermissionType_ManageCompanyComment", ResourceType = typeof(EnumResources))]
    ManageCompanyComment = 202,

    [Display(Name = "PermissionType_ManageCompanyPosDevice", ResourceType = typeof(EnumResources))]
    ManageCompanyPosDevice = 203,

    [Display(Name = "PermissionType_CreateCompanyPosDevice", ResourceType = typeof(EnumResources))]
    CreateCompanyPosDevice = 204,

    [Display(Name = "PermissionType_UpdateCompanyPosDevice", ResourceType = typeof(EnumResources))]
    UpdateCompanyPosDevice = 205,

    [Display(Name = "PermissionType_DeleteCompanyPosDevice", ResourceType = typeof(EnumResources))]
    DeleteCompanyPosDevice = 206,

    [Display(Name = "PermissionType_ManageFinancialGroup", ResourceType = typeof(EnumResources))]
    ManageFinancialGroup = 220,

    [Display(Name = "PermissionType_ManageFinancialYear", ResourceType = typeof(EnumResources))]
    ManageFinancialYear = 221,

    [Display(Name = "PermissionType_CreateFinancialYear", ResourceType = typeof(EnumResources))]
    CreateFinancialYear = 222,

    [Display(Name = "PermissionType_UpdateFinancialYear", ResourceType = typeof(EnumResources))]
    UpdateFinancialYear = 223,

    [Display(Name = "PermissionType_DeleteFinancialYear", ResourceType = typeof(EnumResources))]
    DeleteFinancialYear = 224,

    [Display(Name = "PermissionType_ManageChartOfAccount", ResourceType = typeof(EnumResources))]
    ManageChartOfAccount = 225,

    [Display(Name = "PermissionType_CreateChartOfAccount", ResourceType = typeof(EnumResources))]
    CreateChartOfAccount = 226,

    [Display(Name = "PermissionType_UpdateChartOfAccount", ResourceType = typeof(EnumResources))]
    UpdateChartOfAccount = 227,

    [Display(Name = "PermissionType_DeleteChartOfAccount", ResourceType = typeof(EnumResources))]
    DeleteChartOfAccount = 228,

    [Display(Name = "PermissionType_ManageProductCommentGroup", ResourceType = typeof(EnumResources))]
    ManageProductCommentGroup = 240,

    [Display(Name = "PermissionType_ManageProductComment", ResourceType = typeof(EnumResources))]
    ManageProductComment = 241,

    [Display(Name = "PermissionType_ChangeProductCommentStatus", ResourceType = typeof(EnumResources))]
    ChangeProductCommentStatus = 242,

    [Display(Name = "PermissionType_ManagePermissionGroup", ResourceType = typeof(EnumResources))]
    ManagePermissionGroup = 250,

    [Display(Name = "PermissionType_ManagePermission", ResourceType = typeof(EnumResources))]
    ManagePermission = 251,

    [Display(Name = "PermissionType_ManageDeliveryOption", ResourceType = typeof(EnumResources))]
    ManageDeliveryOption = 252,

    [Display(Name = "PermissionType_CreateManagedPermission", ResourceType = typeof(EnumResources))]
    CreateManagedPermission = 253,

    [Display(Name = "PermissionType_UpdatePermission", ResourceType = typeof(EnumResources))]
    UpdatePermission = 254,

    [Display(Name = "PermissionType_DeletePermission", ResourceType = typeof(EnumResources))]
    DeletePermission = 255,

    [Display(Name = "PermissionType_ManageRolePermissionGroup", ResourceType = typeof(EnumResources))]
    ManageRolePermissionGroup = 260,

    [Display(Name = "PermissionType_ManageRolePermission", ResourceType = typeof(EnumResources))]
    ManageRolePermission = 261,

    [Display(Name = "PermissionType_AssignRolePermission", ResourceType = typeof(EnumResources))]
    AssignRolePermission = 262,

    [Display(Name = "PermissionType_ManageUserRoleGroup", ResourceType = typeof(EnumResources))]
    ManageUserRoleGroup = 270,

    [Display(Name = "PermissionType_ManageUserRole", ResourceType = typeof(EnumResources))]
    ManageUserRole = 271,

    [Display(Name = "PermissionType_AssignUserRole", ResourceType = typeof(EnumResources))]
    AssignUserRole = 272,

    [Display(Name = "PermissionType_ManageContentPolicyGroup", ResourceType = typeof(EnumResources))]
    ManageContentPolicyGroup = 280,

    [Display(Name = "PermissionType_ManageContentPolicy", ResourceType = typeof(EnumResources))]
    ManageContentPolicy = 281,

    [Display(Name = "PermissionType_CreateContentPolicy", ResourceType = typeof(EnumResources))]
    CreateContentPolicy = 282,

    [Display(Name = "PermissionType_UpdateContentPolicy", ResourceType = typeof(EnumResources))]
    UpdateContentPolicy = 283,

    [Display(Name = "PermissionType_DeleteContentPolicy", ResourceType = typeof(EnumResources))]
    DeleteContentPolicy = 284,

    [Display(Name = "PermissionType_ManageContentPolicyRuleGroup", ResourceType = typeof(EnumResources))]
    ManageContentPolicyRuleGroup = 285,

    [Display(Name = "PermissionType_ManageContentPolicyRule", ResourceType = typeof(EnumResources))]
    ManageContentPolicyRule = 286,

    [Display(Name = "PermissionType_CreateContentPolicyRule", ResourceType = typeof(EnumResources))]
    CreateContentPolicyRule = 287,

    [Display(Name = "PermissionType_UpdateContentPolicyRule", ResourceType = typeof(EnumResources))]
    UpdateContentPolicyRule = 288,

    [Display(Name = "PermissionType_DeleteContentPolicyRule", ResourceType = typeof(EnumResources))]
    DeleteContentPolicyRule = 289,

    [Display(Name = "PermissionType_ListCategory", ResourceType = typeof(EnumResources))]
    ListCategory = 290,

    [Display(Name = "PermissionType_GetCategoryById", ResourceType = typeof(EnumResources))]
    GetCategoryById = 291,

    [Display(Name = "PermissionType_ListCommentTopic", ResourceType = typeof(EnumResources))]
    ListCommentTopic = 292,

    [Display(Name = "PermissionType_GetCommentTopicById", ResourceType = typeof(EnumResources))]
    GetCommentTopicById = 293,

    [Display(Name = "PermissionType_ListDeliveryType", ResourceType = typeof(EnumResources))]
    ListDeliveryType = 294,

    [Display(Name = "PermissionType_GetDeliveryTypeById", ResourceType = typeof(EnumResources))]
    GetDeliveryTypeById = 295,

    [Display(Name = "PermissionType_ListPostType", ResourceType = typeof(EnumResources))]
    ListPostType = 296,

    [Display(Name = "PermissionType_GetPostTypeById", ResourceType = typeof(EnumResources))]
    GetPostTypeById = 297,

    [Display(Name = "PermissionType_ListRole", ResourceType = typeof(EnumResources))]
    ListRole = 298,

    [Display(Name = "PermissionType_GetRoleById", ResourceType = typeof(EnumResources))]
    GetRoleById = 299,

    [Display(Name = "PermissionType_ListSubCategory", ResourceType = typeof(EnumResources))]
    ListSubCategory = 300,

    [Display(Name = "PermissionType_GetSubCategoryById", ResourceType = typeof(EnumResources))]
    GetSubCategoryById = 301,

    [Display(Name = "PermissionType_ListUserAddress", ResourceType = typeof(EnumResources))]
    ListUserAddress = 302,

    [Display(Name = "PermissionType_GetUserAddressById", ResourceType = typeof(EnumResources))]
    GetUserAddressById = 303,

    [Display(Name = "PermissionType_ListProvince", ResourceType = typeof(EnumResources))]
    ListProvince = 304,

    [Display(Name = "PermissionType_GetProvinceById", ResourceType = typeof(EnumResources))]
    GetProvinceById = 305,

    [Display(Name = "PermissionType_ListCity", ResourceType = typeof(EnumResources))]
    ListCity = 306,

    [Display(Name = "PermissionType_GetCityById", ResourceType = typeof(EnumResources))]
    GetCityById = 307,

    [Display(Name = "PermissionType_ListProduct", ResourceType = typeof(EnumResources))]
    ListProduct = 308,

    [Display(Name = "PermissionType_GetProductById", ResourceType = typeof(EnumResources))]
    GetProductById = 309,

    [Display(Name = "PermissionType_ListProductPrice", ResourceType = typeof(EnumResources))]
    ListProductPrice = 310,

    [Display(Name = "PermissionType_GetProductPriceById", ResourceType = typeof(EnumResources))]
    GetProductPriceById = 311,

    [Display(Name = "PermissionType_ListProductDescription", ResourceType = typeof(EnumResources))]
    ListProductDescription = 312,

    [Display(Name = "PermissionType_GetProductDescriptionById", ResourceType = typeof(EnumResources))]
    GetProductDescriptionById = 313,

    [Display(Name = "PermissionType_ListProductFile", ResourceType = typeof(EnumResources))]
    ListProductFile = 314,

    [Display(Name = "PermissionType_GetProductFileById", ResourceType = typeof(EnumResources))]
    GetProductFileById = 315,

    [Display(Name = "PermissionType_ListProductPriceDeliveryOption", ResourceType = typeof(EnumResources))]
    ListProductPriceDeliveryOption = 318,

    [Display(Name = "PermissionType_GetProductPriceDeliveryOptionById", ResourceType = typeof(EnumResources))]
    GetProductPriceDeliveryOptionById = 319,

    [Display(Name = "PermissionType_ListOrder", ResourceType = typeof(EnumResources))]
    ListOrder = 320,

    [Display(Name = "PermissionType_GetOrderById", ResourceType = typeof(EnumResources))]
    GetOrderById = 321,

    [Display(Name = "PermissionType_ListOrderByStatus", ResourceType = typeof(EnumResources))]
    ListOrderByStatus = 322,

    [Display(Name = "PermissionType_GetOrderDetail", ResourceType = typeof(EnumResources))]
    GetOrderDetail = 323,

    [Display(Name = "PermissionType_GetOrderStatusSummary", ResourceType = typeof(EnumResources))]
    GetOrderStatusSummary = 324,

    [Display(Name = "PermissionType_ListProperty", ResourceType = typeof(EnumResources))]
    ListProperty = 325,

    [Display(Name = "PermissionType_GetPropertyById", ResourceType = typeof(EnumResources))]
    GetPropertyById = 326,

    [Display(Name = "PermissionType_ListPropertyCategory", ResourceType = typeof(EnumResources))]
    ListPropertyCategory = 327,

    [Display(Name = "PermissionType_GetPropertyCategoryById", ResourceType = typeof(EnumResources))]
    GetPropertyCategoryById = 328,

    [Display(Name = "PermissionType_ListPropertyItem", ResourceType = typeof(EnumResources))]
    ListPropertyItem = 329,

    [Display(Name = "PermissionType_GetPropertyItemById", ResourceType = typeof(EnumResources))]
    GetPropertyItemById = 330,

    [Display(Name = "PermissionType_ListTag", ResourceType = typeof(EnumResources))]
    ListTag = 333,

    [Display(Name = "PermissionType_GetTagById", ResourceType = typeof(EnumResources))]
    GetTagById = 334,

    [Display(Name = "PermissionType_ListBlogPost", ResourceType = typeof(EnumResources))]
    ListBlogPost = 335,

    [Display(Name = "PermissionType_GetBlogPostById", ResourceType = typeof(EnumResources))]
    GetBlogPostById = 336,

    [Display(Name = "PermissionType_ListBlogPostCategory", ResourceType = typeof(EnumResources))]
    ListBlogPostCategory = 337,

    [Display(Name = "PermissionType_GetBlogPostCategoryById", ResourceType = typeof(EnumResources))]
    GetBlogPostCategoryById = 338,

    [Display(Name = "PermissionType_ListBlogPostTag", ResourceType = typeof(EnumResources))]
    ListBlogPostTag = 339,

    [Display(Name = "PermissionType_GetBlogPostTagById", ResourceType = typeof(EnumResources))]
    GetBlogPostTagById = 340,

    [Display(Name = "PermissionType_ListBlogPostComment", ResourceType = typeof(EnumResources))]
    ListBlogPostComment = 341,

    [Display(Name = "PermissionType_GetBlogPostCommentById", ResourceType = typeof(EnumResources))]
    GetBlogPostCommentById = 342,

    [Display(Name = "PermissionType_ListBlogPostLike", ResourceType = typeof(EnumResources))]
    ListBlogPostLike = 343,

    [Display(Name = "PermissionType_GetBlogPostLikeById", ResourceType = typeof(EnumResources))]
    GetBlogPostLikeById = 344,

    [Display(Name = "PermissionType_ListCompany", ResourceType = typeof(EnumResources))]
    ListCompany = 345,

    [Display(Name = "PermissionType_GetCompanyById", ResourceType = typeof(EnumResources))]
    GetCompanyById = 346,

    [Display(Name = "PermissionType_ListCompanyComment", ResourceType = typeof(EnumResources))]
    ListCompanyComment = 347,

    [Display(Name = "PermissionType_GetCompanyCommentById", ResourceType = typeof(EnumResources))]
    GetCompanyCommentById = 348,

    [Display(Name = "PermissionType_ListCompanyPosDevice", ResourceType = typeof(EnumResources))]
    ListCompanyPosDevice = 349,

    [Display(Name = "PermissionType_GetCompanyPosDeviceById", ResourceType = typeof(EnumResources))]
    GetCompanyPosDeviceById = 350,

    [Display(Name = "PermissionType_ListFinancialYear", ResourceType = typeof(EnumResources))]
    ListFinancialYear = 351,

    [Display(Name = "PermissionType_GetFinancialYearById", ResourceType = typeof(EnumResources))]
    GetFinancialYearById = 352,

    [Display(Name = "PermissionType_ListChartOfAccount", ResourceType = typeof(EnumResources))]
    ListChartOfAccount = 353,

    [Display(Name = "PermissionType_GetChartOfAccountById", ResourceType = typeof(EnumResources))]
    GetChartOfAccountById = 354,

    [Display(Name = "PermissionType_ListProductComment", ResourceType = typeof(EnumResources))]
    ListProductComment = 355,

    [Display(Name = "PermissionType_GetProductCommentById", ResourceType = typeof(EnumResources))]
    GetProductCommentById = 356,

    [Display(Name = "PermissionType_ListPermission", ResourceType = typeof(EnumResources))]
    ListPermission = 357,

    [Display(Name = "PermissionType_GetPermissionById", ResourceType = typeof(EnumResources))]
    GetPermissionById = 358,

    [Display(Name = "PermissionType_CheckPermission", ResourceType = typeof(EnumResources))]
    CheckPermission = 359,

    [Display(Name = "PermissionType_ListDeliveryOption", ResourceType = typeof(EnumResources))]
    ListDeliveryOption = 360,

    [Display(Name = "PermissionType_GetDeliveryOptionById", ResourceType = typeof(EnumResources))]
    GetDeliveryOptionById = 361,

    [Display(Name = "PermissionType_ListRolePermission", ResourceType = typeof(EnumResources))]
    ListRolePermission = 362,

    [Display(Name = "PermissionType_GetRolePermissionById", ResourceType = typeof(EnumResources))]
    GetRolePermissionById = 363,

    [Display(Name = "PermissionType_DeleteRolePermission", ResourceType = typeof(EnumResources))]
    DeleteRolePermission = 364,

    [Display(Name = "PermissionType_ListUserRole", ResourceType = typeof(EnumResources))]
    ListUserRole = 365,

    [Display(Name = "PermissionType_GetUserRoleById", ResourceType = typeof(EnumResources))]
    GetUserRoleById = 366,

    [Display(Name = "PermissionType_DeleteUserRole", ResourceType = typeof(EnumResources))]
    DeleteUserRole = 367,

    [Display(Name = "PermissionType_ListContentPolicy", ResourceType = typeof(EnumResources))]
    ListContentPolicy = 368,

    [Display(Name = "PermissionType_GetContentPolicyById", ResourceType = typeof(EnumResources))]
    GetContentPolicyById = 369,

    [Display(Name = "PermissionType_ListContentPolicyRule", ResourceType = typeof(EnumResources))]
    ListContentPolicyRule = 370,

    [Display(Name = "PermissionType_GetContentPolicyRuleById", ResourceType = typeof(EnumResources))]
    GetContentPolicyRuleById = 371,

    [Display(Name = "PermissionType_ManageBank", ResourceType = typeof(EnumResources))]
    ManageBank = 372,

    [Display(Name = "PermissionType_ListBank", ResourceType = typeof(EnumResources))]
    ListBank = 373,

    [Display(Name = "PermissionType_GetBankById", ResourceType = typeof(EnumResources))]
    GetBankById = 374,

    [Display(Name = "PermissionType_CreateBank", ResourceType = typeof(EnumResources))]
    CreateBank = 375,

    [Display(Name = "PermissionType_UpdateBank", ResourceType = typeof(EnumResources))]
    UpdateBank = 376,

    [Display(Name = "PermissionType_DeleteBank", ResourceType = typeof(EnumResources))]
    DeleteBank = 377,

    [Display(Name = "PermissionType_ManageCmsGroup", ResourceType = typeof(EnumResources))]
    ManageCmsGroup = 378,

    [Display(Name = "PermissionType_ManagePage", ResourceType = typeof(EnumResources))]
    ManagePage = 379,

    [Display(Name = "PermissionType_ListPage", ResourceType = typeof(EnumResources))]
    ListPage = 380,

    [Display(Name = "PermissionType_GetPageById", ResourceType = typeof(EnumResources))]
    GetPageById = 381,

    [Display(Name = "PermissionType_CreatePage", ResourceType = typeof(EnumResources))]
    CreatePage = 382,

    [Display(Name = "PermissionType_UpdatePage", ResourceType = typeof(EnumResources))]
    UpdatePage = 383,

    [Display(Name = "PermissionType_DeletePage", ResourceType = typeof(EnumResources))]
    DeletePage = 384,

    [Display(Name = "PermissionType_ManageSection", ResourceType = typeof(EnumResources))]
    ManageSection = 385,

    [Display(Name = "PermissionType_ListSection", ResourceType = typeof(EnumResources))]
    ListSection = 386,

    [Display(Name = "PermissionType_GetSectionById", ResourceType = typeof(EnumResources))]
    GetSectionById = 387,

    [Display(Name = "PermissionType_CreateSection", ResourceType = typeof(EnumResources))]
    CreateSection = 388,

    [Display(Name = "PermissionType_UpdateSection", ResourceType = typeof(EnumResources))]
    UpdateSection = 389,

    [Display(Name = "PermissionType_DeleteSection", ResourceType = typeof(EnumResources))]
    DeleteSection = 390,

    [Display(Name = "PermissionType_ManageSectionType", ResourceType = typeof(EnumResources))]
    ManageSectionType = 391,

    [Display(Name = "PermissionType_ListSectionType", ResourceType = typeof(EnumResources))]
    ListSectionType = 392,

    [Display(Name = "PermissionType_GetSectionTypeById", ResourceType = typeof(EnumResources))]
    GetSectionTypeById = 393,

    [Display(Name = "PermissionType_CreateSectionType", ResourceType = typeof(EnumResources))]
    CreateSectionType = 394,

    [Display(Name = "PermissionType_UpdateSectionType", ResourceType = typeof(EnumResources))]
    UpdateSectionType = 395,

    [Display(Name = "PermissionType_DeleteSectionType", ResourceType = typeof(EnumResources))]
    DeleteSectionType = 396,

    [Display(Name = "PermissionType_ManageSectionItem", ResourceType = typeof(EnumResources))]
    ManageSectionItem = 397,

    [Display(Name = "PermissionType_ListSectionItem", ResourceType = typeof(EnumResources))]
    ListSectionItem = 398,

    [Display(Name = "PermissionType_GetSectionItemById", ResourceType = typeof(EnumResources))]
    GetSectionItemById = 399,

    [Display(Name = "PermissionType_CreateSectionItem", ResourceType = typeof(EnumResources))]
    CreateSectionItem = 400,

    [Display(Name = "PermissionType_UpdateSectionItem", ResourceType = typeof(EnumResources))]
    UpdateSectionItem = 401,

    [Display(Name = "PermissionType_DeleteSectionItem", ResourceType = typeof(EnumResources))]
    DeleteSectionItem = 402,

    [Display(Name = "PermissionType_ManagePageSection", ResourceType = typeof(EnumResources))]
    ManagePageSection = 403,

    [Display(Name = "PermissionType_ListPageSection", ResourceType = typeof(EnumResources))]
    ListPageSection = 404,

    [Display(Name = "PermissionType_GetPageSectionById", ResourceType = typeof(EnumResources))]
    GetPageSectionById = 405,

    [Display(Name = "PermissionType_CreatePageSection", ResourceType = typeof(EnumResources))]
    CreatePageSection = 406,

    [Display(Name = "PermissionType_UpdatePageSection", ResourceType = typeof(EnumResources))]
    UpdatePageSection = 407,

    [Display(Name = "PermissionType_DeletePageSection", ResourceType = typeof(EnumResources))]
    DeletePageSection = 408,

    [Display(Name = "PermissionType_ManageWebSiteSetting", ResourceType = typeof(EnumResources))]
    ManageWebSiteSetting = 409,

    [Display(Name = "PermissionType_GetWebSiteSettingById", ResourceType = typeof(EnumResources))]
    GetWebSiteSettingById = 410,

    [Display(Name = "PermissionType_UpdateWebSiteSetting", ResourceType = typeof(EnumResources))]
    UpdateWebSiteSetting = 411,

    [Display(Name = "PermissionType_ManageDiscount", ResourceType = typeof(EnumResources))]
    ManageDiscount = 412,

    [Display(Name = "PermissionType_ListDiscount", ResourceType = typeof(EnumResources))]
    ListDiscount = 413,

    [Display(Name = "PermissionType_GetDiscountById", ResourceType = typeof(EnumResources))]
    GetDiscountById = 414,

    [Display(Name = "PermissionType_CreateDiscount", ResourceType = typeof(EnumResources))]
    CreateDiscount = 415,

    [Display(Name = "PermissionType_UpdateDiscount", ResourceType = typeof(EnumResources))]
    UpdateDiscount = 416,

    [Display(Name = "PermissionType_DeleteDiscount", ResourceType = typeof(EnumResources))]
    DeleteDiscount = 417,

    [Display(Name = "PermissionType_ManageProductProperty", ResourceType = typeof(EnumResources))]
    ManageProductProperty = 418,

    [Display(Name = "PermissionType_ListProductProperty", ResourceType = typeof(EnumResources))]
    ListProductProperty = 419,

    [Display(Name = "PermissionType_GetProductPropertyById", ResourceType = typeof(EnumResources))]
    GetProductPropertyById = 420,

    [Display(Name = "PermissionType_CreateProductProperty", ResourceType = typeof(EnumResources))]
    CreateProductProperty = 421,

    [Display(Name = "PermissionType_UpdateProductProperty", ResourceType = typeof(EnumResources))]
    UpdateProductProperty = 422,

    [Display(Name = "PermissionType_DeleteProductProperty", ResourceType = typeof(EnumResources))]
    DeleteProductProperty = 423,

    [Display(Name = "PermissionType_ManageProductFeatureType", ResourceType = typeof(EnumResources))]
    ManageProductFeatureType = 424,

    [Display(Name = "PermissionType_ListProductFeatureType", ResourceType = typeof(EnumResources))]
    ListProductFeatureType = 425,

    [Display(Name = "PermissionType_GetProductFeatureTypeById", ResourceType = typeof(EnumResources))]
    GetProductFeatureTypeById = 426,

    [Display(Name = "PermissionType_CreateProductFeatureType", ResourceType = typeof(EnumResources))]
    CreateProductFeatureType = 427,

    [Display(Name = "PermissionType_UpdateProductFeatureType", ResourceType = typeof(EnumResources))]
    UpdateProductFeatureType = 428,

    [Display(Name = "PermissionType_DeleteProductFeatureType", ResourceType = typeof(EnumResources))]
    DeleteProductFeatureType = 429,

    [Display(Name = "PermissionType_ManageProductOrderItemAttachmentType", ResourceType = typeof(EnumResources))]
    ManageProductOrderItemAttachmentType = 436,

    [Display(Name = "PermissionType_ListProductOrderItemAttachmentType", ResourceType = typeof(EnumResources))]
    ListProductOrderItemAttachmentType = 437,

    [Display(Name = "PermissionType_GetProductOrderItemAttachmentTypeById", ResourceType = typeof(EnumResources))]
    GetProductOrderItemAttachmentTypeById = 438,

    [Display(Name = "PermissionType_CreateProductOrderItemAttachmentType", ResourceType = typeof(EnumResources))]
    CreateProductOrderItemAttachmentType = 439,

    [Display(Name = "PermissionType_UpdateProductOrderItemAttachmentType", ResourceType = typeof(EnumResources))]
    UpdateProductOrderItemAttachmentType = 440,

    [Display(Name = "PermissionType_DeleteProductOrderItemAttachmentType", ResourceType = typeof(EnumResources))]
    DeleteProductOrderItemAttachmentType = 441,

    [Display(Name = "PermissionType_ManageContentPolicyRecordAccessGroup", ResourceType = typeof(EnumResources))]
    ManageContentPolicyRecordAccessGroup = 442,

    [Display(Name = "PermissionType_ManageContentPolicyRecordAccess", ResourceType = typeof(EnumResources))]
    ManageContentPolicyRecordAccess = 443,

    [Display(Name = "PermissionType_CreateContentPolicyRecordAccess", ResourceType = typeof(EnumResources))]
    CreateContentPolicyRecordAccess = 444,

    [Display(Name = "PermissionType_DeleteContentPolicyRecordAccess", ResourceType = typeof(EnumResources))]
    DeleteContentPolicyRecordAccess = 445,

    [Display(Name = "PermissionType_ListContentPolicyRecordAccess", ResourceType = typeof(EnumResources))]
    ListContentPolicyRecordAccess = 446,

    [Display(Name = "PermissionType_GetContentPolicyRecordAccessById", ResourceType = typeof(EnumResources))]
    GetContentPolicyRecordAccessById = 447,

    [Display(Name = "PermissionType_SetContentPolicyRecordAccess", ResourceType = typeof(EnumResources))]
    SetContentPolicyRecordAccess = 448,

    [Display(Name = "PermissionType_ManageWallet", ResourceType = typeof(EnumResources))]
    ManageWallet = 449,

    [Display(Name = "PermissionType_ListWallet", ResourceType = typeof(EnumResources))]
    ListWallet = 450,

    [Display(Name = "PermissionType_GetWalletById", ResourceType = typeof(EnumResources))]
    GetWalletById = 451,

    [Display(Name = "PermissionType_CreateWallet", ResourceType = typeof(EnumResources))]
    CreateWallet = 452,

    [Display(Name = "PermissionType_UpdateWallet", ResourceType = typeof(EnumResources))]
    UpdateWallet = 453,

    [Display(Name = "PermissionType_UpdateWalletStatus", ResourceType = typeof(EnumResources))]
    UpdateWalletStatus = 454,

    [Display(Name = "PermissionType_AdminChargeWallet", ResourceType = typeof(EnumResources))]
    AdminChargeWallet = 455,

    [Display(Name = "PermissionType_ListWalletTransaction", ResourceType = typeof(EnumResources))]
    ListWalletTransaction = 456,

    [Display(Name = "PermissionType_ManageContentPolicyMetadataGroup", ResourceType = typeof(EnumResources))]
    ManageContentPolicyMetadataGroup = 457,

    [Display(Name = "PermissionType_ManageContentPolicyMetadata", ResourceType = typeof(EnumResources))]
    ManageContentPolicyMetadata = 458,

    [Display(Name = "PermissionType_ContentPolicy_GetEntityTypes", ResourceType = typeof(EnumResources))]
    ContentPolicy_GetEntityTypes = 459,

    [Display(Name = "PermissionType_ContentPolicy_GetEntitySchema", ResourceType = typeof(EnumResources))]
    ContentPolicy_GetEntitySchema = 460,

    [Display(Name = "PermissionType_ContentPolicy_GetRuleOptions", ResourceType = typeof(EnumResources))]
    ContentPolicy_GetRuleOptions = 461,

    [Display(Name = "PermissionType_ContentPolicy_GetPropertyOperators", ResourceType = typeof(EnumResources))]
    ContentPolicy_GetPropertyOperators = 462,

    [Display(Name = "PermissionType_ContentPolicy_ValidateRules", ResourceType = typeof(EnumResources))]
    ContentPolicy_ValidateRules = 463,

    [Display(Name = "PermissionType_ContentPolicy_Preview", ResourceType = typeof(EnumResources))]
    ContentPolicy_Preview = 464,

    [Display(Name = "PermissionType_ContentPolicy_CompareMerge", ResourceType = typeof(EnumResources))]
    ContentPolicy_CompareMerge = 465,

    [Display(Name = "PermissionType_ManageCompanyStory", ResourceType = typeof(EnumResources))]
    ManageCompanyStory = 466,

    [Display(Name = "PermissionType_CreateCompanyStory", ResourceType = typeof(EnumResources))]
    CreateCompanyStory = 467,

    [Display(Name = "PermissionType_UpdateCompanyStory", ResourceType = typeof(EnumResources))]
    UpdateCompanyStory = 468,

    [Display(Name = "PermissionType_ActivateCompanyStory", ResourceType = typeof(EnumResources))]
    ActivateCompanyStory = 469,

    [Display(Name = "PermissionType_DeactivateCompanyStory", ResourceType = typeof(EnumResources))]
    DeactivateCompanyStory = 470,

    [Display(Name = "PermissionType_ManageCompanyStoryComment", ResourceType = typeof(EnumResources))]
    ManageCompanyStoryComment = 471,

    [Display(Name = "PermissionType_ApproveCompanyStoryComment", ResourceType = typeof(EnumResources))]
    ApproveCompanyStoryComment = 472,

    [Display(Name = "PermissionType_ManageCompanyStoryLike", ResourceType = typeof(EnumResources))]
    ManageCompanyStoryLike = 473,

    [Display(Name = "PermissionType_ListCompanyStory", ResourceType = typeof(EnumResources))]
    ListCompanyStory = 474,

    [Display(Name = "PermissionType_GetCompanyStoryById", ResourceType = typeof(EnumResources))]
    GetCompanyStoryById = 475,

    [Display(Name = "PermissionType_ListCompanyStoryComment", ResourceType = typeof(EnumResources))]
    ListCompanyStoryComment = 476,

    [Display(Name = "PermissionType_GetCompanyStoryCommentById", ResourceType = typeof(EnumResources))]
    GetCompanyStoryCommentById = 477,

    [Display(Name = "PermissionType_ListCompanyStoryLike", ResourceType = typeof(EnumResources))]
    ListCompanyStoryLike = 478,

    [Display(Name = "PermissionType_GetCompanyStoryLikeById", ResourceType = typeof(EnumResources))]
    GetCompanyStoryLikeById = 479,

    [Display(Name = "PermissionType_ManageLanguageGroup", ResourceType = typeof(EnumResources))]
    ManageLanguageGroup = 480,

    [Display(Name = "PermissionType_ManageLanguage", ResourceType = typeof(EnumResources))]
    ManageLanguage = 481,

    [Display(Name = "PermissionType_ListLanguage", ResourceType = typeof(EnumResources))]
    ListLanguage = 482,

    [Display(Name = "PermissionType_GetLanguageById", ResourceType = typeof(EnumResources))]
    GetLanguageById = 483,

    [Display(Name = "PermissionType_CreateLanguage", ResourceType = typeof(EnumResources))]
    CreateLanguage = 484,

    [Display(Name = "PermissionType_UpdateLanguage", ResourceType = typeof(EnumResources))]
    UpdateLanguage = 485,

    [Display(Name = "PermissionType_DeleteLanguage", ResourceType = typeof(EnumResources))]
    DeleteLanguage = 486,

    [Display(Name = "PermissionType_SetDefaultLanguage", ResourceType = typeof(EnumResources))]
    SetDefaultLanguage = 487,
}