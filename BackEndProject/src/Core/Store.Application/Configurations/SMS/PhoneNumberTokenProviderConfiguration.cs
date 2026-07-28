namespace Edition.Application.Configurations.SMS;

public class PhoneNumberTokenProviderConfiguration : IPhoneNumberTokenProviderConfiguration
{
    public int MinNumber { get; set; }
    public int MaxNumber { get; set; }
    public int Duration { get; set; }
    public int DigitCount { get; set; }
}