using Store.Domain.Common;

namespace Store.Domain.Entities;

public class AssistantFaq : BaseEntity
{
    public int LanguageId { get; private set; }
    public string Question { get; private set; } = default!;
    public string Answer { get; private set; } = default!;
    public int Priority { get; private set; }
    public bool IsActive { get; private set; } = true;

    public Language Language { get; private set; } = default!;

    public static AssistantFaq Create(int languageId, string question, string answer, int priority)
        => new()
        {
            LanguageId = languageId,
            Question = question,
            Answer = answer,
            Priority = priority,
        };

    public void Update(int languageId, string question, string answer, int priority, bool isActive)
    {
        LanguageId = languageId;
        Question = question;
        Answer = answer;
        Priority = priority;
        IsActive = isActive;
    }
}
