using System.Text;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Edition.Application.Common.Extensions;
using Store.Infrastructure.Persistence.Models;
using Store.Infrastructure.Persistence.Contracts;
using Store.Common.Utilities;
using Store.Domain.Enums;
using Store.Domain.Dtos.Others;
using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.SeedData;

public class SeedService(EditionDbContext context) : ISeedService
{
    public async Task SeedDataAsync(List<DynamicPermission> dynamicPermissions)
    {
        //var meshkinDashtCity = await SeedProvincesAndRelatedCitiesAsync();
        //var adminRoleId = await SeedRolesAsync();
        //await SeedUsersAsync(adminRoleId, meshkinDashtCity!.Id);

        if (dynamicPermissions.Count > 0)
            await SeedPermissionsAsync(dynamicPermissions);

        //if (permissionIds.Count != 0)
        //    await SeedRolePermissionsAsync(permissionIds, adminRoleId);

        //await SeedChartOfAccountsAsync();
        var productFeatureTypes = await SeedProductFeatureTypesAsync();
        await SeedCategoriesAsync(productFeatureTypes);
        await SeedContentPoliciesAsync();
    }

    private async Task SeedContentPoliciesAsync()
    {
        var adminRoles = await context.Role
            .Where(x => x.Title == "مدیر" || x.Title.Contains("مدیر"))
            .ToListAsync();

        foreach (var adminRole in adminRoles)
            adminRole.SetBypassContentPolicy(true);

        var bloggerRole = await context.Role.FirstOrDefaultAsync(x => x.Title == "بلاگر");
        if (bloggerRole is null)
        {
            bloggerRole = Role.Create("بلاگر");
            bloggerRole.SetRequireContentPolicy(true);
            context.Role.Add(bloggerRole);
        }
        else
        {
            bloggerRole.SetRequireContentPolicy(true);
        }

        await context.SaveChangesAsync();
    }

    private async Task<City?> SeedProvincesAndRelatedCitiesAsync()
    {
        if (await context.City.AnyAsync())
            return await context.City
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Name.Contains("مشکین دشت"));

        var provinces = await GetProvincesAsync();
        if (provinces is null || provinces.Count == 0) return null;

        var cities = await GetCitiesAsync();

        var provinceModels =
            provinces.Select(province =>
            {
                var provinceModel = Province.Create(province.Name, province.Slug, province.TelPrefix, null, 0, null, null);

                if (cities.Count != 0)
                {
                    var relatedCities = cities
                        .Where(x => x.ProvinceId == province.Id)
                        .Select(x => City.Create(0, x.Name, x.Slug, null, 0, null, null))
                        .ToList();

                    provinceModel.AddCities(relatedCities);
                }

                return provinceModel;
            })
            .ToList();

        context.Province.AddRange(provinceModels);
        await context.SaveChangesAsync();

        return provinceModels
            .SelectMany(x => x.Cities)
            .FirstOrDefault(x => x.Name.Contains("مشکین دشت"));
    }

    private static async Task<List<ProvinceSeedDataDto>> GetProvincesAsync()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "SeedData", "Data", "Provinces.json");
        var jsonData = await File.ReadAllTextAsync(path, Encoding.UTF8);
        var provinces = JsonConvert.DeserializeObject<List<ProvinceSeedDataDto>?>(jsonData);
        return provinces ?? [];
    }

    private static async Task<List<CitySeedDataDto>> GetCitiesAsync()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "SeedData", "Data", "Cities.json");
        var jsonData = await File.ReadAllTextAsync(path, Encoding.UTF8);
        var cities = JsonConvert.DeserializeObject<List<CitySeedDataDto>>(jsonData);
        return cities ?? [];
    }

    private static async Task<List<CategoriesSeedDataDto>> GetCategoriesAsync()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "SeedData", "Data", "Categories.json");
        var jsonData = await File.ReadAllTextAsync(path, Encoding.UTF8);
        var categories = JsonConvert.DeserializeObject<List<CategoriesSeedDataDto>>(jsonData);
        return categories ?? [];
    }

    private static async Task<List<AllProductsSeedDataDto>> GetAllProductsAsync()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "SeedData", "Data", "AllProducts.json");
        var jsonData = await File.ReadAllTextAsync(path, Encoding.UTF8);
        var products = JsonConvert.DeserializeObject<List<AllProductsSeedDataDto>>(jsonData);
        return products ?? [];
    }

    private async Task<List<ProductFeatureType>> SeedProductFeatureTypesAsync()
    {
        var featureTypes = new List<ProductFeatureType> {
            ProductFeatureType.Create("نمایش در منو", ProductFeatureTypeCode.DisplayOnMenu, ProductFeatureDataType.Boolean, "", true)
        };

        foreach (var featureType in featureTypes)
        {
            if (!await context.ProductFeatureType.AnyAsync(x => x.Type == featureType.Type))
                context.ProductFeatureType.Add(featureType);
        }

        await context.SaveChangesAsync();

        return await context.ProductFeatureType.ToListAsync();
    }

    private async Task SeedCategoriesAsync(List<ProductFeatureType> productFeatureTypes)
    {
        var categories = await GetCategoriesAsync();
        var allSingleProducts = (await GetAllProductsAsync()).SelectMany(x => x.Products).ToList();

        var allProducts = new List<Product>();

        foreach (var categoryData in categories)
        {
            var category = Category.Create(categoryData.Title, categoryData.Slug, categoryData.Code);
            var subCategories = categoryData
                .SubCategories
                .Select(x =>
                {
                    var subCategory = SubCategory.Create(x.Title, x.Slug, x.Code, 0);
                    var products = x
                        .Products
                        .Select(product => Product.Create(product.Title, product.Slug, "", product.Code, 0))
                        .ToList();

                    var displayOnMenuFeatureType = productFeatureTypes.FirstOrDefault(x => x.Type == ProductFeatureTypeCode.DisplayOnMenu);
                    if (displayOnMenuFeatureType is not null)
                    {
                        foreach (var product in products)
                        {
                            if (allSingleProducts.Any(p => p.Title == product.Slug))
                                product.AddFeature(ProductFeature.Create(0, displayOnMenuFeatureType.Id, displayOnMenuFeatureType.Type.ToValue()!));
                        }
                    }

                    subCategory.AddProducts(products);
                    allProducts.AddRange(products);

                    return subCategory;
                })
                .ToList();

            category.AddSubCategories(subCategories);

            context.Category.Add(category);
        }

        await context.SaveChangesAsync();
        await AddProductsPropertiesAsync(allProducts);
    }

    private async Task AddProductsPropertiesAsync(List<Product> products)
    {
        //var properties = new List<Property>();
        //var propertyCategories = new List<PropertyCategory>();       

        foreach (var product in products)
        {
            var productProperties = GetProductProperties(product.Slug);
            if (productProperties is null)
                continue;

            foreach (var productProperty in productProperties)
            {
                var category = PropertyCategory.Create(productProperty.CategoryTitle);

                var properties = productProperty
                    .Properties
                    .Select((x, i) =>
                    {
                        var type = getType(x.Type);
                        var property = Property.Create(type, null, x.Title, 0, ++i, x.Description);
                        return property;
                    })
                    .ToList();
            }
        }

        static PropertyType getType(string type)
        {
            return type switch
            {
                "number" => PropertyType.Numeric,
                "text" => PropertyType.Text,
                "checkbox" => PropertyType.Boolean,
                "select" => PropertyType.Select,
                _ => throw new NotImplementedException()
            };
        }

        await context.SaveChangesAsync();
    }

    private async Task<int> SeedRolesAsync()
    {
        var roles = new List<Role>
        {
            Role.Create("مدیر"),
            Role.Create("فروشنده")
        };
        var existsRoles = await context.Role.AsNoTracking().ToListAsync();
        roles = roles.Where(role => !existsRoles.Any(x => x.Title == role.Title)).ToList();

        if (roles.Count != 0)
        {
            context.Role.AddRange(roles);
            await context.SaveChangesAsync();
        }

        var adminRoleId =
            roles.Count != 0
            ? roles.First().Id
            : existsRoles.Single(x => x.Title == "مدیر" || x.Title.Contains("مدیر")).Id;

        return adminRoleId;
    }

    private async Task<List<int>> SeedPermissionsAsync(List<DynamicPermission> dynamicPermissions)
    {
        var priority = 1;
        foreach (var dynamicPermission in dynamicPermissions)
        {
            var existsTabPermission = await context
                             .Permission
                             .FirstOrDefaultAsync(x => x.Title == dynamicPermission.Name && x.ParentId == PermissionType.Product);

            var tabPermission = existsTabPermission ??
                                Permission.Create(dynamicPermission.Controllers[0].GroupType, "", dynamicPermission.Name, "",
                                                  priority, PermissionLevelType.Tab, PermissionType.Product);
            tabPermission.SetParents([]);

            var versionOfControllers = dynamicPermission.Controllers.GroupBy(x => GetControllerName(x.FullName)).ToList();
            foreach (var controller in versionOfControllers)
            {
                var actions = controller.ToList().SelectMany(x => x.Actions).DistinctBy(x => new { x.Name, x.Type }).ToList();
                var firstController = controller.First();
                var existsPagePermission =
                        await context.Permission
                       .SingleOrDefaultAsync(x => x.Title == firstController.Name && x.NameSpace == firstController.FullName);

                var pagePermission = existsPagePermission ??
                                     Permission.Create(firstController.Type, firstController.Url, firstController.Name,
                                     firstController.FullName, ++priority, PermissionLevelType.Page, tabPermission.Id);
                pagePermission.SetParents([]);

                foreach (var action in actions)
                {
                    if (action.Type == pagePermission.Id)
                        continue;

                    var isExistsAction = await context.Permission.AnyAsync(x => x.Title == action.Name && x.NameSpace == action.FullNames.FirstOrDefault());
                    if (isExistsAction)
                        continue;

                    var pagePermissionAction =
                            Permission.Create(action.Type, action.Url, action.Name, action.FullNames.FirstOrDefault() ?? "",
                                              ++priority, PermissionLevelType.Action, pagePermission.Id);

                    if (!pagePermission.Children.Any(x => x.Title == pagePermissionAction.Title && x.NameSpace == pagePermissionAction.NameSpace))
                        pagePermission.Children.Add(pagePermissionAction);
                }

                if (!tabPermission.Children.Any(x => x.Title == pagePermission.Title && x.NameSpace == pagePermission.NameSpace))
                    tabPermission.Children.Add(pagePermission);
            }
            if (existsTabPermission is null)
                context.Permission.Add(tabPermission);
            else
                context.Permission.Update(tabPermission);

            priority++;
        }
        await context.SaveChangesAsync();

        var permissionIds =
                context.ChangeTracker
               .Entries<Permission>()
               .SelectMany(x => x.Properties)
               .Where(x => x.Metadata.Name == "Id")
               .Select(x => Convert.ToInt32(x.CurrentValue))
               .ToList();

        return permissionIds;

        static string GetControllerName(string fullName)
           => fullName.Split(".").Last();
    }

    private async Task SeedUsersAsync(int adminRoleId, int meshkinDashtCityId)
    {
        var adminUserRole = UserRole.Create(adminRoleId);

        List<User> users =
            [
                User.Create("hamidmohammadnian@gmail.com", meshkinDashtCityId, GenderType.Male, "09307653782", "Hamid", "Mohammadnian", "09307653782",
                            SecurityUtility.GetSha256Hash("admin"), Guid.NewGuid().ToString())
                    .SetUserRoles([ adminUserRole ]),

                User.Create("hosseinojaq@gmail.com", meshkinDashtCityId, GenderType.Male, "09383109379", "Hossein", "Ojaq", "09383109379",
                            SecurityUtility.GetSha256Hash("admin"), Guid.NewGuid().ToString())
                    .SetUserRoles([ adminUserRole ])
            ];

        foreach (var user in users)
        {
            if (!await context.User.AnyAsync(x => x.UserName == user.UserName))
                context.User.Add(user);
        }
        await context.SaveChangesAsync();
    }

    private async Task SeedRolePermissionsAsync(List<int> permissionIds, int adminRoleId)
    {
        var rolePermissionIds =
             context.RolePermission
            .Where(x => permissionIds.Contains(x.Id))
            .AsNoTracking()
            .Select(x => x.Id)
            .ToList();

        permissionIds =
            permissionIds
            .Where(x => !rolePermissionIds
            .Contains(x))
            .ToList();

        var rolePermissionsForAdmin = permissionIds.Select(x => RolePermission.Create(adminRoleId, (PermissionType)x!)).ToList();
        context.RolePermission.AddRange(rolePermissionsForAdmin);
        await context.SaveChangesAsync();
    }

    private async Task SeedChartOfAccountsAsync()
    {
        if (await context.ChartOfAccount.AnyAsync())
            return;

        var path = Path.Combine(AppContext.BaseDirectory, "SeedData", "Data", "ChartOfAccounts.json");
        var jsonData = await File.ReadAllTextAsync(path, Encoding.UTF8);
        var chartOfAccounts = JsonConvert.DeserializeObject<List<ChartOfAccountSeedDataDto>>(jsonData);
        if (chartOfAccounts is null || chartOfAccounts.Count == 0) return;

        var accounts =
            chartOfAccounts
            .Where(x => x.Level == 1)
            .Select(account =>
            {
                var levelOfOneAccount = ChartOfAccount.Create(
                    account.Level,
                    "",
                    account.Name,
                    (ChartOfAccountType)account.AccountType,
                    (ChartOfAccountDetailType)account.DetailType);

                var levelOfTwoAccounts = chartOfAccounts
                        .Where(x => x.ParentId == account.Id)
                        .Select(levelTwoAccountItem =>
                        {
                            var levelTwoAccount = ChartOfAccount.Create(
                                levelTwoAccountItem.Level,
                                "",
                                levelTwoAccountItem.Name,
                                (ChartOfAccountType)levelTwoAccountItem.AccountType,
                                (ChartOfAccountDetailType)levelTwoAccountItem.DetailType);

                            var levelOfThreeAccounts = chartOfAccounts
                                    .Where(x => x.ParentId == levelTwoAccountItem.Id)
                                    .Select(levelOfThreeAccountItem =>
                                    {
                                        var levelThreeAccount = ChartOfAccount.Create(
                                            levelOfThreeAccountItem.Level,
                                            "",
                                            levelOfThreeAccountItem.Name,
                                            (ChartOfAccountType)levelOfThreeAccountItem.AccountType,
                                            (ChartOfAccountDetailType)levelOfThreeAccountItem.DetailType);

                                        var levelOfFourAccounts = chartOfAccounts
                                                .Where(x => x.ParentId == levelOfThreeAccountItem.Id)
                                                .Select(levelOfFourAccountItem =>
                                                {
                                                    var levelFourAccount = ChartOfAccount.Create(
                                                        levelOfFourAccountItem.Level,
                                                        "",
                                                        levelOfFourAccountItem.Name,
                                                        (ChartOfAccountType)levelOfFourAccountItem.AccountType,
                                                        (ChartOfAccountDetailType)levelOfFourAccountItem.DetailType);

                                                    return levelFourAccount;
                                                })
                                                .ToList();

                                        levelThreeAccount.AddChildren(levelOfFourAccounts);

                                        return levelThreeAccount;
                                    })
                                    .ToList();

                            levelTwoAccount.AddChildren(levelOfThreeAccounts);

                            return levelTwoAccount;
                        })
                        .ToList();

                levelOfOneAccount.AddChildren(levelOfTwoAccounts);

                return levelOfOneAccount;
            })
            .ToList();

        context.ChartOfAccount.AddRange(accounts);
        await context.SaveChangesAsync();
    }

    private static List<ProductPropertySeedDto>? GetProductProperties(string slug)
    {
        try
        {
            var path = Path.Combine(AppContext.BaseDirectory, "SeedData", "Data", "Extract Code Files", "Multi Files", "Properties", "Fixes", $"{slug}.json");
            if (!File.Exists(path))
                return default;

            var jsonData = File.ReadAllText(path, Encoding.UTF8);
            var properties = JsonConvert.DeserializeObject<List<ProductPropertySeedDto>>(jsonData);
            return properties;
        }
        catch (Exception ex)
        {
            return default!;
        }
    }
}