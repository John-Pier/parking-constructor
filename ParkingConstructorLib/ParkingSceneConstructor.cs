using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParkingConstructorLib.logic;

namespace ParkingConstructorLib
{
    /// <summary>
    /// Конструктор парковки
    /// </summary>
    public class ParkingSceneConstructor
    {
        private ParkingModel parkingModel;

        public ParkingSceneConstructor(ParkingModel parkingModel)
        {
            this.parkingModel = parkingModel;
        }
    }
}
