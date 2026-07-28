namespace Edition.Application.Configurations.Email;

public class EmailTokenProviderConfiguration : IEmailTokenProviderConfiguration
{
    public int MinNumber { get; set; }
    public int MaxNumber { get; set; }
    public int Duration { get; set; }
    public int DigitCount { get; set; }
}