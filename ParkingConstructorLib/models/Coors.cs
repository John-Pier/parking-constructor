using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingConstructorLib.models
{
    public class Coors : IComparable
    {
        public int columnIndex;
        public int rowIndex;
        public Coors(int colIndex, int rowIndex)
        {
            this.columnIndex = colIndex;
            this.rowIndex = rowIndex;
        }
        private static int[,,] localMap;

        public static void setLocalMap(int[,,] map)
        {
            localMap = map;
        }

        public int CompareTo(object o)
        {
            Coors c = o as Coors;
            if (localMap[columnIndex, rowIndex, 2] < localMap[c.columnIndex, c.rowIndex, 2])
                return -1;
            if (localMap[columnIndex, rowIndex, 2] > localMap[c.columnIndex, c.rowIndex, 2])
                return 1;
            return 0;
        }
    }
}
