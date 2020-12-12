using System;

namespace ParkingSimulationForms.views.components
{
    public class TextBoxWithPlaceholder : System.Windows.Forms.TextBox
    {

        System.Drawing.Color DefaultColor;
        private string _placeHolderText;
        public string PlaceHolderText {
            get
            {
                return _placeHolderText;
            }
            set {
                setNewPlaceholder(value);
                _placeHolderText = value;
            }
        }

        private void setNewPlaceholder(string placeholder)
        {
            if (this.ForeColor == System.Drawing.Color.Gray || String.IsNullOrEmpty(this.Text))
            {
                this.ForeColor = System.Drawing.Color.Gray;
                this.Text = placeholder;
            }
        }

        public TextBoxWithPlaceholder()
        {
            // get default color of text
            DefaultColor = this.ForeColor;
            // Add event handler for when the control gets focus

            this.GotFocus += (object sender, EventArgs e) =>
            {
                if(this.Text == PlaceHolderText)
                {
                    this.Text = String.Empty;
                    this.ForeColor = DefaultColor;
                }
            };

            // add event handling when focus is lost
            this.LostFocus += (Object sender, EventArgs e) => {
                if (String.IsNullOrEmpty(this.Text) || this.Text == PlaceHolderText)
                {
                    this.ForeColor = System.Drawing.Color.Gray;
                    this.Text = PlaceHolderText;
                }
                else
                {
                    this.ForeColor = DefaultColor;
                }
            };
        }
        public TextBoxWithPlaceholder(string placeholdertext)
        {
            // get default color of text
            DefaultColor = this.ForeColor;
            // Add event handler for when the control gets focus
            this.GotFocus += (object sender, EventArgs e) =>
            {
                if (this.Text == PlaceHolderText)
                {
                    this.Text = String.Empty;
                    this.ForeColor = DefaultColor;
                }
            };

            // add event handling when focus is lost
            this.LostFocus += (Object sender, EventArgs e) => {
                if (String.IsNullOrEmpty(this.Text) || this.Text == PlaceHolderText)
                {
                    this.ForeColor = System.Drawing.Color.Gray;
                    this.Text = PlaceHolderText;
                }
                else
                {
                    this.ForeColor = DefaultColor;
                }
            };

            if (!string.IsNullOrEmpty(placeholdertext))
            {
                // change style   
                this.ForeColor = System.Drawing.Color.Gray;
                // Add text
                PlaceHolderText = placeholdertext;
                this.Text = placeholdertext;
            }
        }
    }
}
