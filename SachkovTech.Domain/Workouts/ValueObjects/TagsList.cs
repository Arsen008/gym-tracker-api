namespace SachkovTech.Domain.Workouts.ValueObjects;

public record TagsList
{
    public IReadOnlyList<Tag> Items { get; } = new List<Tag>();

    private TagsList() { }

    public TagsList(IReadOnlyList<Tag> items)
    {
        Items = items;
    }
    public TagsList(IEnumerable<string>? tags)
    {
        Items = (tags ?? Array.Empty<string>())
            .Select(t => Tag.Create(t))
            .Where(r => r.IsSuccess)
            .Select(r => r.Value)
            .Distinct()
            .ToList();
    }
}