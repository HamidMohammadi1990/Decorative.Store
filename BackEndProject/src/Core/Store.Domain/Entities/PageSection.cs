using Store.Domain.Common;

namespace Store.Domain.Entities;

public class PageSection : BaseEntity
{
    public int PageId { get; private set; }
    public int SectionId { get; private set; }
    public int Priority { get; private set; }


    public Page Page { get; private set; } = default!;
    public Section Section { get; private set; } = default!;


    public static PageSection Create(int pageId, int sectionId, int priority)
        => new()
        {
            PageId = pageId,
            SectionId = sectionId,
            Priority = priority
        };

    public void Update(int pageId, int sectionId, int priority)
    {
        PageId = pageId;
        SectionId = sectionId;
        Priority = priority;
    }
}