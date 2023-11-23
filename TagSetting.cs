// See https://aka.ms/new-console-template for more information

public class TagSetting
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public required string Name { get; set; }

    public required string Value { get; set; }

    public virtual Tag? Tag { get; set; }

    public Guid TagId { get; set; }
}
