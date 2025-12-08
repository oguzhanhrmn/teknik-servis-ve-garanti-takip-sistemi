namespace TSGTS.Core.Entities;

public class Category
{
    public int Id { get; set; }
    public string CategoryName { get; set; } = string.Empty;

    public ICollection<Model> Models { get; set; } = new List<Model>();
}
