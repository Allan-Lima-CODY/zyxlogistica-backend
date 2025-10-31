using ZyxLogistica.Domain.Enums;

namespace ZyxLogistica.Domain.Entities;

public class Driver : EntityBase
{
    public string Name { get; private set; } = String.Empty;
    public string Phone { get; private set; } = String.Empty;
    public string Cnh { get; private init; } = string.Empty;
    public CnhCategory CnhCategory { get; private init; }
    public bool Active { get; private set; }

    public Driver() { }

    private Driver(string name, string phone, string cnh, CnhCategory cnhcategory, bool active)
    {
        Name = name;
        Phone = phone;
        Cnh = cnh;
        CnhCategory = cnhcategory;
        Active = active;
    }

    public static Driver Create(string name, string phone, string cnh, CnhCategory cnhcategory, bool active)
    {
        return new Driver(name, phone, cnh, cnhcategory, active);
    }

    public void Update(string name, string phone)
    {
        Name = name;
        Phone = phone;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        Active = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        Active = false;
        UpdatedAt = DateTime.UtcNow;
    }
}
