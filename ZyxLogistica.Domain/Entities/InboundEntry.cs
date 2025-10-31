namespace ZyxLogistica.Domain.Entities;

public class InboundEntry : EntityBase
{
    public Inventory Inventory { get; private set; } = null!;
    public string Reference { get; private set; } = string.Empty;
    public string SupplierName { get; private set; } = string.Empty;
    public string Observation { get; private set; } = string.Empty;

    private InboundEntry() { }

    private InboundEntry(Inventory inventory, string reference, string supplierName, string observation)
    {
        Inventory = inventory;
        Reference = reference;
        SupplierName = supplierName;
        Observation = observation;
    }

    public static InboundEntry Create(Inventory inventory, string reference, string supplierName, string observation)
    {
        return new InboundEntry(inventory, reference, supplierName, observation);
    }

    public void Update(string observation, string supplierName, string reference)
    {
        Observation = observation;
        SupplierName = supplierName;
        Reference = reference;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetInventory(Inventory inv) => Inventory = inv;
}
