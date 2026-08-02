using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Store.Infrastructure.Persistence.Interceptors;
using Store.Domain.Entities;
using Store.Domain.Enums;

namespace Store.Infrastructure.Persistence;

public sealed class EditionDbContext
    (DbContextOptions<EditionDbContext> options)
    : DbContext(options)
{
    public DbSet<Language> Language { get; set; }
    public DbSet<Category> Category { get; set; }
    public DbSet<CategoryTranslation> CategoryTranslation { get; set; }
    public DbSet<Company> Company { get; set; }
    public DbSet<CompanyComment> CompanyComment { get; set; }
    public DbSet<CompanyStory> CompanyStory { get; set; }
    public DbSet<CompanyStoryItem> CompanyStoryItem { get; set; }
    public DbSet<CompanyStoryComment> CompanyStoryComment { get; set; }
    public DbSet<CompanyStoryLike> CompanyStoryLike { get; set; }
    public DbSet<DeliveryOption> DeliveryOption { get; set; }
    public DbSet<CompanyProduct> CompanyProduct { get; set; }
    public DbSet<CommentTopic> CommentTopic { get; set; }
    public DbSet<DeliveryType> DeliveryType { get; set; }
    public DbSet<Order> Order { get; set; }
    public DbSet<OrderNote> OrderNote { get; set; }
    public DbSet<OrderItem> OrderItem { get; set; }
    public DbSet<OrderItemAttachment> OrderItemAttachment { get; set; }
    public DbSet<OrderItemProperty> OrderItemProperty { get; set; }
    public DbSet<OrderItemAttachmentType> OrderItemAttachmentType { get; set; }
    public DbSet<OrderItemAttachmentTypeRestriction> OrderItemAttachmentTypeRestriction { get; set; }
    public DbSet<Permission> Permission { get; set; }
    public DbSet<PostType> PostType { get; set; }
    public DbSet<Product> Product { get; set; }
    public DbSet<ProductTranslation> ProductTranslation { get; set; }
    public DbSet<ProductComment> ProductComment { get; set; }
    public DbSet<ProductDescription> ProductDescription { get; set; }
    public DbSet<ProductOrderItemAttachmentType> ProductOrderItemAttachmentType { get; set; }
    public DbSet<ProductFeature> ProductFeature { get; set; }
    public DbSet<ProductFeatureType> ProductFeatureType { get; set; }
    public DbSet<Discount> Discount { get; set; }
    public DbSet<ProductFile> ProductFile { get; set; }
    public DbSet<ProductFileTranslation> ProductFileTranslation { get; set; }
    public DbSet<ProductPrice> ProductPrice { get; set; }
    public DbSet<ProductProperty> ProductProperty { get; set; }
    public DbSet<ProductPropertyPrice> ProductPropertyPrice { get; set; }
    public DbSet<ProductPriceDeliveryOption> ProductPriceDeliveryOption { get; set; }
    public DbSet<Property> Property { get; set; }
    public DbSet<PropertyTranslation> PropertyTranslation { get; set; }
    public DbSet<PropertyCategory> PropertyCategory { get; set; }
    public DbSet<PropertyCategoryTranslation> PropertyCategoryTranslation { get; set; }
    public DbSet<PropertyItem> PropertyItem { get; set; }
    public DbSet<PropertyItemTranslation> PropertyItemTranslation { get; set; }
    public DbSet<PropertyItemPrice> PropertyItemPrice { get; set; }
    public DbSet<PropertyItemDependency> PropertyItemDependency { get; set; }
    public DbSet<RefreshToken> RefreshToken { get; set; }
    public DbSet<UserSession> UserSession { get; set; }
    public DbSet<Role> Role { get; set; }
    public DbSet<RolePermission> RolePermission { get; set; }
    public DbSet<SubCategory> SubCategory { get; set; }
    public DbSet<SubCategoryTranslation> SubCategoryTranslation { get; set; }
    public DbSet<User> User { get; set; }
    public DbSet<UserAddress> UserAddress { get; set; }
    public DbSet<UserRole> UserRole { get; set; }
    public DbSet<WebSiteSetting> WebSiteSetting { get; set; }
    public DbSet<BankAccount> BankAccount { get; set; }
    public DbSet<Currency> Currency { get; set; }
    public DbSet<Wallet> Wallet { get; set; }
    public DbSet<BankTransaction> BankTransaction { get; set; }
    public DbSet<WalletTransaction> WalletTransaction { get; set; }
    public DbSet<Province> Province { get; set; }
    public DbSet<City> City { get; set; }
    public DbSet<ProductPropertyRule> ProductPropertyRule { get; set; }
    public DbSet<ProductPropertyRuleTranslation> ProductPropertyRuleTranslation { get; set; }
    public DbSet<Tag> Tag { get; set; }
    public DbSet<BlogPost> BlogPost { get; set; }
    public DbSet<BlogPostTag> BlogPostTag { get; set; }
    public DbSet<BlogPostLike> BlogPostLike { get; set; }
    public DbSet<BlogPostComment> BlogPostComment { get; set; }
    public DbSet<BlogPostCategory> BlogPostCategory { get; set; }
    public DbSet<ContentPolicy> ContentPolicy { get; set; }
    public DbSet<ContentPolicyRule> ContentPolicyRule { get; set; }
    public DbSet<ContentPolicyRecordAccess> ContentPolicyRecordAccess { get; set; }
    public DbSet<Bank> Bank { get; set; }
    public DbSet<ChartOfAccount> ChartOfAccount { get; set; }
    public DbSet<ChequeTransaction> ChequeTransaction { get; set; }
    public DbSet<CompanyPosDevice> CompanyPosDevice { get; set; }
    public DbSet<Expense> Expense { get; set; }
    public DbSet<ExpenseType> ExpenseType { get; set; }
    public DbSet<FinancialDocument> FinancialDocument { get; set; }
    public DbSet<FinancialDocumentDetail> FinancialDocumentDetail { get; set; }
    public DbSet<FinancialYear> FinancialYear { get; set; }
    public DbSet<OrderCommission> OrderCommission { get; set; }
    public DbSet<OrderVat> OrderVat { get; set; }
    public DbSet<PosTransaction> PosTransaction { get; set; }
    public DbSet<Page> Page { get; set; }
    public DbSet<Section> Section { get; set; }
    public DbSet<SectionItem> SectionItem { get; set; }
    public DbSet<SectionType> SectionType { get; set; }
    public DbSet<PageSection> PageSection { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(new CleanStringPropertyInterceptor());
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}