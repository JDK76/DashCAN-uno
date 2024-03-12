namespace DashCAN.Common
{
    public enum DataSource
    {
        Demo,
        CanBus
    }

    public enum Unit
    {
        Boolean,
        Gear,
        Kmh,
        Mph,
        Kelvin,
        Celcius,
        Fahrenheit,
        Kpa,
        KpaAbs,
        Psi,
        Lambda,
        Litre,
        UsGallon,
        Percent,
        RPM,
        Volts
    }

    public enum Gear
    {
        Unknown = -2,
        Reverse = -1,
        Neutral = 0,
        First = 1,
        Second = 2,
        Third = 3,
        Fourth = 4,
        Fifth = 5,
        Sixth = 6
    }

    public enum IndicatorType
    {
        None,
        Left,
        Right,
        HighBeam
    }

    public enum WarningType
    {
        None,
        DoorOpen,
        ParkBrake,
        Oil,
        Battery,
        Fuel,
        Brake,
        Engine
    }
}
