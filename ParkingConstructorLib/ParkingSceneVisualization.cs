using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParkingConstructorLib.logic;

namespace ParkingConstructorLib
{
    /// <summary>
    /// Визуализатор парковки
    /// </summary>
    public class ParkingSceneVisualization
    {
        private ParkingModel parkingModel;

        public ParkingSceneVisualization()
        {
        }

        public void SetParkingModel(ParkingModel parkingModel)
        {
            this.parkingModel = parkingModel;
        }
    }
}
