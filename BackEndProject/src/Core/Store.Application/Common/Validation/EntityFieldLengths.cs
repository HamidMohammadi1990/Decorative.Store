namespace Edition.Application.Common.Validation;

/// <summary>
/// String length limits aligned with Edition.Infrastructure.Persistence.Configuration entity configs.
/// </summary>
public static class EntityFieldLengths
{
    public static class Bank
    {
        public const int Name = 30;
        public const int Code = 50;
    }

    public static class BlogPost
    {
        public const int Title = 70;
        public const int Slug = 150;
        public const int Summary = 200;
        public const int MetaTitle = 150;
        public const int Content = 2500;
    }

    public static class BlogPostCategory
    {
        public const int Title = 70;
        public const int Slug = 150;
    }

    public static class BlogPostComment
    {
        public const int Comment = 2500;
    }

    public static class Category
    {
        public const int Title = 60;
        public const int Slug = 150;
        public const int Code = 12;
    }

    public static class ChartOfAccount
    {
        public const int Code = 20;
        public const int Title = 50;
    }

    public static class City
    {
        public const int Name = 25;
        public const int Slug = 30;
        public const int Description = 200;
    }

    public static class CommentTopic
    {
        public const int Title = 35;
    }

    public static class Company
    {
        public const int Name = 30;
        public const int Description = 300;
        public const int Code = 12;
        public const int PhoneNumber = 11;
        public const int Email = 35;
        public const int PostalCode = 10;
        public const int Address = 120;
    }

    public static class CompanyPosDevice
    {
        public const int Title = 50;
        public const int Description = 200;
        public const int SerialNumber = 16;
    }

    public static class ContentPolicy
    {
        public const int Name = 100;
        public const int EntityType = 100;
        public const int QueryAction = 100;
    }

    public static class ContentPolicyRule
    {
        public const int FieldPath = 150;
        public const int Value = 200;
    }

    public static class DeliveryOption
    {
        public const int Title = 30;
    }

    public static class DeliveryType
    {
        public const int Title = 30;
    }

    public static class Discount
    {
        public const int Code = 20;
    }

    public static class FinancialYear
    {
        public const int Title = 50;
    }

    public static class Order
    {
        public const int Title = 50;
    }

    public static class Page
    {
        public const int Title = 350;
        public const int Slug = 60;
        public const int MetaTitle = 120;
        public const int MetaDescription = 300;
    }

    public static class Permission
    {
        public const int Title = 40;
        public const int Slug = 150;
        public const int GroupName = 100;
    }

    public static class PostType
    {
        public const int Title = 35;
        public const int Slug = 150;
        public const int Description = 150;
    }

    public static class Product
    {
        public const int Title = 150;
        public const int Slug = 150;
        public const int Description = 400;
        public const int ProductCode = 10;
    }

    public static class ProductComment
    {
        public const int Comment = 250;
    }

    public static class ProductDescription
    {
        public const int Description = 2500;
    }

    public static class ProductFeatureType
    {
        public const int Name = 50;
        public const int Description = 100;
    }

    public static class ProductFile
    {
        public const int Title = 30;
        public const int FileName = 35;
    }

    public static class ProductOrderItemAttachmentType
    {
        public const int Description = 200;
    }

    public static class ProductPropertyRule
    {
        public const int Description = 250;
    }

    public static class Property
    {
        public const int Title = 30;
    }

    public static class PropertyCategory
    {
        public const int Title = 30;
    }

    public static class PropertyItem
    {
        public const int Title = 30;
    }

    public static class Province
    {
        public const int Name = 25;
        public const int Slug = 30;
        public const int TelPrefix = 6;
        public const int Description = 200;
    }

    public static class Role
    {
        public const int Name = 20;
    }

    public static class Section
    {
        public const int Title = 80;
        public const int Description = 160;
        public const int MetaTitle = 150;
        public const int MetaDescription = 80;
    }

    public static class SectionItem
    {
        public const int Title = 80;
        public const int Link = 24;
        public const int Icon = 36;
        public const int Description = 150;
        public const int MetaDescription = 250;
    }

    public static class SectionType
    {
        public const int Title = 80;
    }

    public static class SubCategory
    {
        public const int Title = 60;
        public const int Slug = 150;
        public const int Code = 12;
    }

    public static class Tag
    {
        public const int Title = 50;
    }

    public static class User
    {
        public const int FirstName = 20;
        public const int LastName = 20;
        public const int UserName = 50;
        public const int Email = 30;
        public const int PasswordHash = 256;
        public const int Mobile = 11;
        public const int NationalCode = 20;
        public const int Address = 40;
    }
}
