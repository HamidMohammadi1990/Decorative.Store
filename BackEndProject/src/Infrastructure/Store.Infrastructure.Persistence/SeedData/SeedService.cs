using Store.Domain.Enums;
using Store.Domain.Entities;
using Store.Common.Utilities;
using Store.Domain.Dtos.Others;
using Microsoft.EntityFrameworkCore;
using Store.Infrastructure.Persistence.Contracts;

namespace Store.Infrastructure.Persistence.SeedData;

public class SeedService(EditionDbContext context) : ISeedService
{
    public Task SeedCatalogAsync(CancellationToken cancellationToken = default)
        => SeedCatalogInternalAsync(cancellationToken);

    private async Task SeedCatalogInternalAsync(CancellationToken cancellationToken = default)
    {
        await SeedCategoriesAsync(cancellationToken);
        await SeedPropertiesAsync(cancellationToken);
        await SeedProductsAsync(cancellationToken);
        await SeedProductPricesAsync(cancellationToken);
        await SeedProductStockAsync(cancellationToken);
        await SeedProductFilesAsync(cancellationToken);
        await SeedProductPropertiesAsync(cancellationToken);
        await SeedCommentTopicsAsync(cancellationToken);
        await SeedBlogPostCategoriesAsync(cancellationToken);
        await SeedBlogPostsAsync(cancellationToken);
        await CmsSeedService.SeedShopPagesAsync(context, cancellationToken);
    }

    public async Task SeedDataAsync(List<DynamicPermission> dynamicPermissions, CancellationToken cancellationToken = default)
    {
        await SeedCatalogAsync(cancellationToken);

        if (dynamicPermissions.Count > 0)
        {
            var permissionIds = await SeedPermissionsAsync(dynamicPermissions);

            var adminRoleId = await SeedRolesAsync();
            await SeedContentPoliciesAsync();

            if (permissionIds.Count != 0)
                await SeedRolePermissionsAsync(permissionIds, adminRoleId);


            await SeedUsersAsync(adminRoleId);
            await SeedBlogPostsAsync(cancellationToken);
        }
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

    private async Task SeedUsersAsync(int adminRoleId)
    {
        var adminUserRole = UserRole.Create(adminRoleId);

        List<User> users =
            [
                User.Create("hamidmohammadnian@gmail.com", GenderType.Male, "09307653782", "Hamid", "Mohammadnian", "09307653782",
                            SecurityUtility.GetSha256Hash("admin"), Guid.NewGuid().ToString())
                    .SetUserRoles([ adminUserRole ])
            ];

        foreach (var user in users)
        {
            if (!await context.User.AnyAsync(x => x.UserName == user.UserName))
            {
                context.User.Add(user);
                continue;
            }

            var existingUser = await context.User.FirstOrDefaultAsync(x => x.UserName == user.UserName);
            if (existingUser is not null && existingUser.EnsureSecurityStamp())
                context.User.Update(existingUser);
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

    private async Task SeedCategoriesAsync(CancellationToken cancellationToken = default)
    {
        if (await context.Category.AnyAsync(cancellationToken))
            return;

        var faLanguage = await context.Language
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Code == "fa-IR", cancellationToken);

        var enLanguage = await context.Language
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Code == "en-US", cancellationToken);

        if (faLanguage is null || enLanguage is null)
            throw new InvalidOperationException("Category seed requires fa-IR and en-US languages to be seeded first.");

        foreach (var item in CategorySeedData.Items)
        {
            var category = Category.Create(item.Code);
            category.UpsertTranslation(faLanguage.Id, item.FaTitle, item.FaSlug);
            category.UpsertTranslation(enLanguage.Id, item.EnTitle, item.EnSlug);

            var subCategories = item.SubCategories
                .Select(sub =>
                {
                    var subCategory = SubCategory.Create(sub.Code, categoryId: 0);
                    subCategory.UpsertTranslation(faLanguage.Id, sub.FaTitle, sub.FaSlug);
                    subCategory.UpsertTranslation(enLanguage.Id, sub.EnTitle, sub.EnSlug);
                    return subCategory;
                })
                .ToList();

            category.AddSubCategories(subCategories);
            context.Category.Add(category);
        }

        await context.SaveChangesAsync(cancellationToken);
    }

    private async Task SeedProductsAsync(CancellationToken cancellationToken = default)
    {
        if (await context.Product.AnyAsync(cancellationToken))
            return;

        if (!await context.Category.AnyAsync(cancellationToken))
            throw new InvalidOperationException("Product seed requires categories to be seeded first.");

        var faLanguage = await context.Language
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Code == "fa-IR", cancellationToken);

        var enLanguage = await context.Language
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Code == "en-US", cancellationToken);

        if (faLanguage is null || enLanguage is null)
            throw new InvalidOperationException("Product seed requires fa-IR and en-US languages to be seeded first.");

        var subCategorySlugMap = await context.SubCategoryTranslation
            .AsNoTracking()
            .Where(x => x.LanguageId == faLanguage.Id)
            .ToDictionaryAsync(x => x.Slug, x => x.SubCategoryId, cancellationToken);

        foreach (var item in ProductSeedData.Items)
        {
            if (!subCategorySlugMap.TryGetValue(item.SubCategorySlug, out var subCategoryId))
                throw new InvalidOperationException(
                    $"SubCategory slug '{item.SubCategorySlug}' was not found for product '{item.ProductCode}'.");

            var product = Product.Create(item.ProductCode, subCategoryId, item.Price, item.CompareAtPrice);
            product.SetInStock(item.InStock);
            product.UpsertTranslation(faLanguage.Id, item.FaTitle, item.FaSlug, item.FaDescription);
            product.UpsertTranslation(enLanguage.Id, item.EnTitle, item.EnSlug, item.EnDescription);
            context.Product.Add(product);
        }

        await context.SaveChangesAsync(cancellationToken);
    }

    private async Task SeedProductPricesAsync(CancellationToken cancellationToken = default)
    {
        var products = await context.Product.ToListAsync(cancellationToken);
        if (products.Count == 0)
            return;

        var priceByCode = ProductSeedData.Items.ToDictionary(
            item => item.ProductCode,
            StringComparer.OrdinalIgnoreCase);
        var changed = false;

        foreach (var product in products)
        {
            if (!priceByCode.TryGetValue(product.ProductCode, out var seedItem))
                continue;

            if (product.Price == seedItem.Price && product.CompareAtPrice == seedItem.CompareAtPrice)
                continue;

            product.SetPricing(seedItem.Price, seedItem.CompareAtPrice);
            changed = true;
        }

        if (changed)
            await context.SaveChangesAsync(cancellationToken);
    }

    private async Task SeedProductStockAsync(CancellationToken cancellationToken = default)
    {
        var products = await context.Product.ToListAsync(cancellationToken);
        if (products.Count == 0)
            return;

        var stockByCode = ProductSeedData.Items.ToDictionary(
            item => item.ProductCode,
            item => item.InStock,
            StringComparer.OrdinalIgnoreCase);
        var changed = false;

        foreach (var product in products)
        {
            if (!stockByCode.TryGetValue(product.ProductCode, out var inStock))
                continue;

            if (product.InStock == inStock)
                continue;

            product.SetInStock(inStock);
            changed = true;
        }

        if (changed)
            await context.SaveChangesAsync(cancellationToken);
    }

    private async Task SeedProductFilesAsync(CancellationToken cancellationToken = default)
    {
        if (await context.ProductFile.AnyAsync(cancellationToken))
            return;

        if (!await context.Product.AnyAsync(cancellationToken))
            throw new InvalidOperationException("Product file seed requires products to be seeded first.");

        var faLanguage = await context.Language
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Code == "fa-IR", cancellationToken);

        var enLanguage = await context.Language
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Code == "en-US", cancellationToken);

        if (faLanguage is null || enLanguage is null)
            throw new InvalidOperationException("Product file seed requires fa-IR and en-US languages to be seeded first.");

        var productMap = await context.Product
            .AsNoTracking()
            .ToDictionaryAsync(x => x.ProductCode, x => x.Id, StringComparer.OrdinalIgnoreCase, cancellationToken);

        var copiedImages = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        foreach (var item in ProductSeedData.Items)
        {
            if (!productMap.TryGetValue(item.ProductCode, out var productId))
                throw new InvalidOperationException($"Product '{item.ProductCode}' was not found for image seed.");

            if (!copiedImages.TryGetValue(item.ImageBaseName, out var fileName))
            {
                fileName = ProductImageSeedAssets.EnsureCopiedToUploads(item.ImageBaseName);
                copiedImages[item.ImageBaseName] = fileName;
            }

            var productFile = ProductFile.Create(productId, fileName, isMain: true);
            productFile.UpsertTranslation(faLanguage.Id, item.FaImageTitle);
            productFile.UpsertTranslation(enLanguage.Id, item.EnImageTitle);
            context.ProductFile.Add(productFile);
        }

        await context.SaveChangesAsync(cancellationToken);
    }

    private async Task SeedPropertiesAsync(CancellationToken cancellationToken = default)
    {
        if (await context.PropertyCategory.AnyAsync(cancellationToken))
            return;

        var faLanguage = await context.Language
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Code == "fa-IR", cancellationToken);

        var enLanguage = await context.Language
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Code == "en-US", cancellationToken);

        if (faLanguage is null || enLanguage is null)
            throw new InvalidOperationException("Property seed requires fa-IR and en-US languages to be seeded first.");

        var catalog = PropertySeedData.Catalog;
        var category = PropertyCategory.Create(catalog.CategoryCode);
        category.UpsertTranslation(faLanguage.Id, catalog.FaCategoryTitle);
        category.UpsertTranslation(enLanguage.Id, catalog.EnCategoryTitle);
        context.PropertyCategory.Add(category);
        await context.SaveChangesAsync(cancellationToken);

        foreach (var propertySeed in catalog.Properties)
        {
            var property = Property.Create(
                PropertyType.Select,
                parentId: null,
                propertySeed.Code,
                category.Id,
                propertySeed.Priority);
            property.UpsertTranslation(faLanguage.Id, propertySeed.FaTitle, description: null);
            property.UpsertTranslation(enLanguage.Id, propertySeed.EnTitle, description: null);
            context.Property.Add(property);
            await context.SaveChangesAsync(cancellationToken);

            foreach (var valueSeed in propertySeed.Values)
            {
                var propertyItem = PropertyItem.Create(valueSeed.Code, property.Id, valueSeed.Priority);
                propertyItem.UpsertTranslation(faLanguage.Id, valueSeed.FaTitle);
                propertyItem.UpsertTranslation(enLanguage.Id, valueSeed.EnTitle);
                context.PropertyItem.Add(propertyItem);
            }
        }

        await context.SaveChangesAsync(cancellationToken);
    }

    private async Task SeedProductPropertiesAsync(CancellationToken cancellationToken = default)
    {
        if (await context.ProductProperty.AnyAsync(cancellationToken))
            return;

        if (!await context.Product.AnyAsync(cancellationToken))
            throw new InvalidOperationException("Product property seed requires products to be seeded first.");

        if (!await context.Property.AnyAsync(cancellationToken))
            throw new InvalidOperationException("Product property seed requires properties to be seeded first.");

        var propertyMap = await context.Property
            .AsNoTracking()
            .ToDictionaryAsync(x => x.Code, x => x.Id, StringComparer.OrdinalIgnoreCase, cancellationToken);

        var propertyItemMap = await context.PropertyItem
            .AsNoTracking()
            .Join(
                context.Property.AsNoTracking(),
                item => item.PropertyId,
                property => property.Id,
                (item, property) => new { PropertyCode = property.Code, ValueCode = item.Code, item.Id })
            .ToDictionaryAsync(
                x => $"{x.PropertyCode}:{x.ValueCode}",
                x => x.Id,
                StringComparer.OrdinalIgnoreCase,
                cancellationToken);

        var productMap = await context.Product
            .AsNoTracking()
            .ToDictionaryAsync(x => x.ProductCode, x => x.Id, StringComparer.OrdinalIgnoreCase, cancellationToken);

        foreach (var assignment in PropertySeedData.ProductAssignments)
        {
            if (!productMap.TryGetValue(assignment.ProductCode, out var productId))
                throw new InvalidOperationException($"Product '{assignment.ProductCode}' was not found for facet seed.");

            if (!propertyMap.TryGetValue(assignment.PropertyCode, out var propertyId))
                throw new InvalidOperationException($"Property '{assignment.PropertyCode}' was not found for facet seed.");

            if (!propertyItemMap.TryGetValue($"{assignment.PropertyCode}:{assignment.ValueCode}", out var propertyItemId))
                throw new InvalidOperationException(
                    $"Property item '{assignment.PropertyCode}:{assignment.ValueCode}' was not found for facet seed.");

            var productProperty = ProductProperty.Create(productId, propertyId, isActive: true, propertyItemId);
            context.ProductProperty.Add(productProperty);
        }

        await context.SaveChangesAsync(cancellationToken);
    }

    private async Task SeedCommentTopicsAsync(CancellationToken cancellationToken = default)
    {
        if (await context.CommentTopic.AnyAsync(cancellationToken))
            return;

        context.CommentTopic.Add(CommentTopic.Create("General review", priority: 1));
        await context.SaveChangesAsync(cancellationToken);
    }

    private async Task SeedBlogPostCategoriesAsync(CancellationToken cancellationToken = default)
    {
        if (await context.BlogPostCategory.AnyAsync(cancellationToken))
            return;

        var faLanguage = await context.Language
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Code == "fa-IR", cancellationToken);

        var enLanguage = await context.Language
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Code == "en-US", cancellationToken);

        if (faLanguage is null || enLanguage is null)
            throw new InvalidOperationException("Blog post category seed requires fa-IR and en-US languages to be seeded first.");

        foreach (var item in BlogPostCategorySeedData.Items)
        {
            var category = BlogPostCategory.Create(item.Code);
            category.UpsertTranslation(faLanguage.Id, item.FaTitle, item.FaSlug);
            category.UpsertTranslation(enLanguage.Id, item.EnTitle, item.EnSlug);
            context.BlogPostCategory.Add(category);
        }

        await context.SaveChangesAsync(cancellationToken);
    }

    private async Task SeedBlogPostsAsync(CancellationToken cancellationToken = default)
    {
        if (await context.BlogPost.AnyAsync(cancellationToken))
            return;

        if (!await context.User.AnyAsync(cancellationToken))
            return;

        var faLanguage = await context.Language
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Code == "fa-IR", cancellationToken);

        var enLanguage = await context.Language
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Code == "en-US", cancellationToken);

        if (faLanguage is null || enLanguage is null)
            throw new InvalidOperationException("Blog post seed requires fa-IR and en-US languages to be seeded first.");

        var author = await context.User.AsNoTracking().FirstOrDefaultAsync(cancellationToken)
            ?? throw new InvalidOperationException("Blog post seed requires at least one user.");

        var categories = await context.BlogPostCategory
            .AsNoTracking()
            .Include(x => x.Translations)
            .ToListAsync(cancellationToken);

        var categoryBySlug = categories
            .SelectMany(category => category.Translations.Select(translation => new { translation.Slug, CategoryId = category.Id }))
            .GroupBy(x => x.Slug, StringComparer.OrdinalIgnoreCase)
            .ToDictionary(x => x.Key, x => x.First().CategoryId, StringComparer.OrdinalIgnoreCase);

        foreach (var item in BlogPostSeedData.Items)
        {
            if (!categoryBySlug.TryGetValue(item.CategorySlug, out var categoryId))
                throw new InvalidOperationException($"Blog category '{item.CategorySlug}' was not found for post '{item.Code}'.");

            var blogPost = BlogPost.Create(
                item.Code,
                categoryId,
                author.Id,
                item.ReadingTimeMinutes,
                item.IsFeatured);

            blogPost.UpsertTranslation(
                faLanguage.Id,
                item.FaTitle,
                item.FaSlug,
                item.FaMetaDescription,
                item.FaSeoKeywords,
                item.FaContent);

            blogPost.UpsertTranslation(
                enLanguage.Id,
                item.EnTitle,
                item.EnSlug,
                item.EnMetaDescription,
                item.EnSeoKeywords,
                item.EnContent);

            blogPost.Publish(item.PublishedOnUtc);
            context.BlogPost.Add(blogPost);
        }

        await context.SaveChangesAsync(cancellationToken);
    }
}