using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingSimulationForms
{
    public class LabelForTable : System.Windows.Forms.Label
    {
        private static int realID = 999;
        public LabelForTable()
        {
            this.Text = "LabelForTable";
            continueConstruct();
        }
        public LabelForTable(string text, bool isBold)
        {
            this.Text = text;
            if (isBold)
                this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            else
                this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            continueConstruct();
        }
        private void continueConstruct()
        {
            this.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Name = "labelForTable" + realID.ToString();
            realID++;
            this.Size = new System.Drawing.Size(173, 79);
            this.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        }
    }
}
