namespace TSGTS.Core.Entities;

public class Model
{
    public int Id { get; set; }
    public int BrandId { get; set; }
    public int CategoryId { get; set; }
    public string ModelName { get; set; } = string.Empty;

    public Brand? Brand { get; set; }
    public Category? Category { get; set; }
    public ICollection<Device> Devices { get; set; } = new List<Device>();
}
