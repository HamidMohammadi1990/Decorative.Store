namespace Edition.Application.Configurations.Email;

public interface IEmailTokenProviderConfiguration
{
    int MinNumber { get; set; }
    int MaxNumber { get; set; }
    int Duration { get; set; }
    int DigitCount { get; set; }
}