namespace TSGTS.Core.Entities;

public class Brand
{
    public int Id { get; set; }
    public string BrandName { get; set; } = string.Empty;

    public ICollection<Model> Models { get; set; } = new List<Model>();
    public ICollection<Device> Devices { get; set; } = new List<Device>();
}
