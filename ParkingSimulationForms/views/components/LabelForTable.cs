using System.Windows.Forms;

namespace ParkingSimulationForms.views.components
{
    public class LabelForTable : Label
    {
        private static int realID = 999;
        public LabelForTable()
        {
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.UserPaint |
              ControlStyles.AllPaintingInWmPaint |
              ControlStyles.ResizeRedraw |
              ControlStyles.ContainerControl |
              ControlStyles.OptimizedDoubleBuffer |
              ControlStyles.SupportsTransparentBackColor
              , true);
            this.Text = "LabelForTable";
            continueConstruct(false);
        }
        public LabelForTable(string text, bool isBold, bool isBugged)
        {
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.UserPaint |
              ControlStyles.AllPaintingInWmPaint |
              ControlStyles.ResizeRedraw |
              ControlStyles.ContainerControl |
              ControlStyles.OptimizedDoubleBuffer |
              ControlStyles.SupportsTransparentBackColor
              , true);
            this.Text = text;
            if (isBold)
                this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            else
                this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            continueConstruct(isBugged);
        }
        public LabelForTable(string text, bool isBold)
        {
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.UserPaint |
              ControlStyles.AllPaintingInWmPaint |
              ControlStyles.ResizeRedraw |
              ControlStyles.ContainerControl |
              ControlStyles.OptimizedDoubleBuffer |
              ControlStyles.SupportsTransparentBackColor
              , true);
            this.Text = text;
            if (isBold)
                this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            else
                this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            continueConstruct(false);
        }
        private void continueConstruct(bool isBugged)
        {
            this.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Name = "labelForTable" + realID.ToString();
            realID++;
            this.Size = new System.Drawing.Size(173, 30);
            this.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            if(!isBugged)
                this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        }

        public void enableBorder()
        {
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        }
    }
}
