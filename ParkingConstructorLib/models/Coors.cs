using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingConstructorLib.models
{
    public class Coors
    {
        public int columnIndex;
        public int rowIndex;
        public Coors(int colIndex, int rowIndex)
        {
            this.columnIndex = colIndex;
            this.rowIndex = rowIndex;
        }
    }
}
