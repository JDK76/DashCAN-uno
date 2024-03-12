using UnitsNet;

namespace DashCAN.Common
{
    internal class UnitHelper
    {
        internal static decimal? ConvertUnit(Unit sourceUnit,  Unit targetUnit, decimal sourceValue)
        {
            if (sourceUnit == targetUnit) return sourceValue;

            switch (sourceUnit)
            {
                case Unit.Kmh:
                    if (targetUnit == Unit.Mph) return (decimal)Speed.FromKilometersPerHour(sourceValue).MilesPerHour;
                    break;
                case Unit.Kelvin:
                    if (targetUnit == Unit.Celcius) return (decimal)Temperature.FromKelvins(sourceValue).DegreesCelsius;
                    else if (targetUnit == Unit.Fahrenheit) return (decimal)Temperature.FromKelvins(sourceValue).DegreesFahrenheit;
                    break;
                case Unit.KpaAbs:
                    return ConvertUnit(Unit.Kpa, targetUnit, sourceValue - 101.3m);
                case Unit.Kpa:
                    if (targetUnit == Unit.KpaAbs) return sourceValue + 101.3m;
                    else if (targetUnit == Unit.Psi) return (decimal)Pressure.FromKilopascals(sourceValue).PoundsForcePerSquareInch;
                    break;
                case Unit.Litre:
                    if (targetUnit == Unit.UsGallon) return (decimal)Volume.FromLiters(sourceValue).UsGallons;
                    break;
            }

            // Unsupported conversion
            return null;
        }
    }
}
