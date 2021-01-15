using System.Windows.Forms;

namespace ParkingSimulationForms.views.components
{
    public class DoubleBufferedTable : TableLayoutPanel
    {
        public DoubleBufferedTable()
        {
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.UserPaint |
              ControlStyles.AllPaintingInWmPaint |
              ControlStyles.ResizeRedraw |
              ControlStyles.ContainerControl |
              ControlStyles.OptimizedDoubleBuffer |
              ControlStyles.SupportsTransparentBackColor
              , true);
        }
    }
}
