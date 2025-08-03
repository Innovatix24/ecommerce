

namespace Domain.Entities.Location;

public class Division
{
    public byte Id { get; set; }
    public string Name { get; set; }
    public string NameBn { get; set; }
}

public class District
{
    public byte Id { get; set; }
    public string Name { get; set; }
    public string NameBn { get; set; }
    public byte DivisionId { get; set; }
}

public class Upazila
{
    public short Id { get; set; }
    public string Name { get; set; }
    public string NameBn { get; set; }
    public byte DistrictId { get; set; }
}
