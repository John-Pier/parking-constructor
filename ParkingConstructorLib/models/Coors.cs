using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingConstructorLib.models
{
    public class Coors : IComparable
    {
        public int ColumnIndex;
        public int RowIndex;
        
        public Coors(int colIndex, int rowIndex)
        {
            this.ColumnIndex = colIndex;
            this.RowIndex = rowIndex;
        }

        public override bool Equals(object obj)
        {
            var coors = (Coors) obj;
            return coors != null && ColumnIndex == coors.ColumnIndex && RowIndex == coors.RowIndex;
        }

        public int CompareTo(object obj)
        {
            throw new NotImplementedException();
        }
    }
}
