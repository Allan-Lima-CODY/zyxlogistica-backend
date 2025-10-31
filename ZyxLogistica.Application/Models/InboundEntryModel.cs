namespace ZyxLogistica.Application.Models;

public record InboundEntryModel
{
    public record InboundEntryInput(InventoryModel.InventoryInput InventoryInput, string Reference, string SupplierName, string Observation);
    public record InboundEntryUpdate(string Reference, string SupplierName, string Observation);
}