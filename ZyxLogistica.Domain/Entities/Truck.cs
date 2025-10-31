namespace ZyxLogistica.Domain.Entities;

public class Truck : EntityBase
{
    public string LicensePlate { get; private set; } = String.Empty;
    public string Model { get; private set; } = String.Empty;
    public int Year { get; private set; } = 0;
    public decimal CapacityKg { get; private set; } = 0;
    public bool Available { get; private set; }

    private Truck() { }

    private Truck(string licensePlate, string model, int year, decimal capacityKg, bool available)
    {
        LicensePlate = licensePlate;
        Model = model;
        Year = year;
        CapacityKg = capacityKg;
        Available = available;
    }

    public static Truck Create(string licensePlate, string model, int year, decimal capacityKg, bool available)
    {
        return new Truck(licensePlate, model, year, capacityKg, available);
    }

    public void Update(string model, int year, decimal capacityKg)
    {
        Model = model;
        Year = year;
        CapacityKg = capacityKg;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ToMakeAvailable()
    {
        Available = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Occupy()
    {
        Available = false;
        UpdatedAt = DateTime.UtcNow;
    }
}