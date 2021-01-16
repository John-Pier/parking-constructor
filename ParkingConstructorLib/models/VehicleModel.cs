using System.Collections.Generic;

namespace ParkingConstructorLib.models
{
    public enum CarType{
        Car,
        Truck
    };
    
    //Цель, к которой пытается следовать транспорт
    public enum TargetType
    {
        Parking,
        Cashier,
        Exit,
    }

    //Последнее перемещение было вертикальным или горизонтальным (для выбора нужной текстуры)
    public enum LastDirection
    {
        Horizontal,
        Vertical,
    }
    
    //Получить доступные для проезда элементы
    public interface IVehicleModel
    {
        List<ParkingModelElementType> GetAvailableElementTypesForMovement();
    }
}
