using Store.Domain.Common;

namespace Store.Domain.Entities;

public class WebSiteSetting : BaseEntity
{
    public string? Email { get; private set; }
    public string? PhoneNumber { get; private set; }
    public string? Telephone { get; private set; }
    public string? Address { get; private set; }
    public string? CartNumber { get; private set; }    
    public string? EmailUserName { get; private set; }
    public string? EmailPassword { get; private set; }
    public string? SmsAccountUrl { get; private set; }
    public string? SmsAccountUserName { get; private set; }
    public string? SmsAccountPassword { get; private set; }


    public void Update(
        string? email,
        string? phoneNumber,
        string? telephone,
        string? address,
        string? cartNumber,
        string? emailUserName,
        string? emailPassword,
        string? smsAccountUrl,
        string? smsAccountUserName,
        string? smsAccountPassword)
    {
        Email = email;
        PhoneNumber = phoneNumber;
        Telephone = telephone;
        Address = address;
        CartNumber = cartNumber;
        EmailUserName = emailUserName;
        if (emailPassword is not null)
            EmailPassword = emailPassword;
        SmsAccountUrl = smsAccountUrl;
        SmsAccountUserName = smsAccountUserName;
        if (smsAccountPassword is not null)
            SmsAccountPassword = smsAccountPassword;
    }
}