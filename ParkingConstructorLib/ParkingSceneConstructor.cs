using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParkingConstructorLib.logic;
using ParkingConstructorLib.models;

namespace ParkingConstructorLib
{
    /// <summary>
    /// Конструктор парковки
    /// </summary>
    public class ParkingSceneConstructor
    {
        public ParkingModel ParkingModel { get; private set; }

        public ParkingSceneConstructor(ParkingModel parkingModel = null)
        {
            if (parkingModel != null)
            {
                ParkingModel = parkingModel;
            }
        }

        public void CreateParkingModel(int columns, int rows)
        {
            ParkingModel = new ParkingModel(columns, rows);
        }

        public void SetObjectToModel(int columnIndex, int rowIndex, ParkingModelElement element)
        {
            ParkingModel.SetElement(columnIndex, rowIndex, element);
        }

        public bool IsParkingModelCreate() => ParkingModel != null;
    }
}
