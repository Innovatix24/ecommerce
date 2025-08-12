
namespace Domain.Entities.Products;
public class Manufacturer 
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string BottomDescription { get; set; }
    public int ManufacturerTemplateId { get; set; }
    public string MetaKeywords { get; set; }
    public string MetaDescription { get; set; }
    public string MetaTitle { get; set; }
    public int MediaFileId { get; set; }
    public bool Published { get; set; }
    public bool Deleted { get; set; }
    public int DisplayOrder { get; set; }
}
