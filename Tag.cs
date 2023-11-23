// See https://aka.ms/new-console-template for more information

public class Tag
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public required string Label { get; set; }

    public virtual ICollection<TagSetting>? Settings { get; set; }
}
