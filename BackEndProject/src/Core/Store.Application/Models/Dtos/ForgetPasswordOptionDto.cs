using Store.Domain.Enums;

namespace Edition.Application.Models.Dtos;

public record ForgetPasswordOptionDto
(
    string Title, 
    ForgetPasswordOptionType OptionType
);