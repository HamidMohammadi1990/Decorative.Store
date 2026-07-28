using Store.Domain.Common;

namespace Store.Domain.Entities;

public class Company : BaseEntity
{
    public int UserId { get; private set; }
    public int CityId { get; private set; }
    public string Name { get; private set; } = default!;
    public string Code { get; private set; } = default!;
    public string PhoneNumber { get; private set; } = default!;
    public string? Email { get; private set; }
    public string PostalCode { get; private set; } = default!;
    public string Address { get; private set; } = default!;
    public string? Description { get; private set; }
    public DateTime CreatedOnUtc { get; private set; } = DateTime.UtcNow;
    public bool IsActive { get; private set; }
    public float Latitude { get; private set; }
    public float Longitude { get; private set; }


    public User User { get; private set; } = default!;
    public City City { get; private set; } = default!;
    public ICollection<Wallet> Wallets { get; set; } = default!;
    public ICollection<OrderItem> OrderItems { get; private set; } = default!;
    public ICollection<CompanyComment> CompanyComments { get; set; } = default!;
    public ICollection<ProductPrice> ProductPrices { get; private set; } = default!;
    public ICollection<FinancialYear> FinancialYears { get; set; } = default!;
    public ICollection<ProductComment> ProductComments { get; set; } = default!;
    public ICollection<CompanyProduct> CompanyProducts { get; private set; } = default!;
    public ICollection<OrderCommission> OrderCommissions { get; private set; } = default!;
    public ICollection<BankTransaction> BankTransactions { get; private set; } = default!;
    public ICollection<CompanyPosDevice> CompanyPosDevices { get; set; } = default!;
    public ICollection<ChequeTransaction> ChequeTransactions { get; set; } = default!;
    public ICollection<PropertyItemPrice> PropertyItemPrices { get; private set; } = default!;
    public ICollection<WalletTransaction> WalletTransactions { get; set; } = default!;
    public ICollection<ProductPropertyPrice> ProductPropertyPrices { get; private set; } = default!;
    public ICollection<CompanyStory> CompanyStories { get; private set; } = default!;


    public static Company Create(int userId, int cityid, string name, string code, string phoneNumber, string? email,
                                 string postalCode, string address, string? description, float latitude, float longitude)
    {
        return new()
        {
            Name = name,
            Code = code,
            Email = email,
            UserId = userId,
            CityId = cityid,
            Address = address,
            Latitude = latitude,
            Longitude = longitude,
            PostalCode = postalCode,
            PhoneNumber = phoneNumber,
            Description = description
        };
    }
    public void Update(int cityid, string name, string code, string phoneNumber, string email, string postalCode,
                                 string address, string description, float latitude, float longitude)
    {
        Name = name;
        Code = code;
        Email = email;
        CityId = cityid;
        Address = address;
        Latitude = latitude;
        Longitude = longitude;
        PostalCode = postalCode;
        PhoneNumber = phoneNumber;
        Description = description;
    }
    public void Active()
    {
        IsActive = true;
    }
    public void InActive()
    {
        IsActive = false;
    }
}