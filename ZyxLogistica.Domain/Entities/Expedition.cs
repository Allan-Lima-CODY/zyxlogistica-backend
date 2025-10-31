namespace ZyxLogistica.Domain.Entities;

public class Expedition : EntityBase
{
    public Order Order { get; private set; } = null!;
    public Driver Driver { get; private set; } = null!;
    public Truck Truck { get; private set; } = null!;
    public DateTime DeliveryForecast { get; private set; }
    public string Observation { get; private set; } = string.Empty;

    private Expedition() { }

    private Expedition(Order order, Driver driver, Truck truck, DateTime deliveryForecast, string observation)
    {
        Order = order;
        Driver = driver;
        Truck = truck;
        DeliveryForecast = deliveryForecast;
        Observation = observation;
    }

    public static Expedition Create(Order order, Driver driver, Truck truck, DateTime deliveryForecast, string observation)
    {
        return new Expedition(order, driver, truck, deliveryForecast, observation);
    }

    public void Update(DateTime newForecast, string observation, Truck truck, Driver driver)
    {
        Observation = observation;
        DeliveryForecast = newForecast;
        Truck = truck;
        Driver = driver;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ChangeTruck(Truck newTruck)
    {
        Truck.ToMakeAvailable();
        newTruck.Occupy();
        Truck = newTruck;
    }
}
