using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParkingConstructorLib.models;

namespace ParkingConstructorLib.services
{
    public class ParkingModelService<T> where T: class
    {

        public ParkingModelService(ParkingModelElement<T>[,] parkingLot)
        {

        }

        public bool CheckCorrectParkingModelElementsArray()
        {
            return true;
        }
    }
}
