using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment2
{
    /// <summary>
    /// The class that will clear the textbox back to blank. It inherits its method from the Shape class.
    /// </summary>
    class Clear : Shape
    {
        //Sets the variables used by the draw method.
        int width, height;
        public Clear() : base()
        {

        }
        /// <summary>
        /// Sets the different variables but most are generic due to it just clearing the screen.
        /// </summary>
        /// <param name="colour">The colour it will resst the screen as.</param>
        /// <param name="x">The x coordinate it will start to clear from.</param>
        /// <param name="y">The y coordinate it will start to clear from.</param>
        /// <param name="width">How large the width of the clear area will be.</param>
        /// <param name="height">How large the height of the clear area will be.</param>
        public Clear(Color colour, int x, int y, int width, int height) : base(colour, x, y)
        {
            //Sets the variables to the parameters.
            this.width = width; //The only thing that is different from shape
            this.height = height;
        }

        /// <summary>
        /// Sets up the variables with the parameters.
        /// </summary>
        /// <param name="colour">The colour the background will be.</param>
        /// <param name="list">The list of the variables.</param>
        public override void set(Color colour, params int[] list)
        {
            //list[0] is x, list[1] is y, list[2] is width, list[3] is height
            base.set(colour, list[0], list[1]);
            this.width = list[2];
            this.height = list[3];

        }

        /// <summary>
        /// Draws the new background to the picture box, resetting back to black.
        /// </summary>
        /// <param name="g"></param>
        public override void draw(Graphics g)
        {
            //Sets up the pen and makes the colour black  to reset the background.
            Color colour1 = Color.Black;
            Pen p = new Pen(colour1, 2);
            SolidBrush b = new SolidBrush(colour1);
            //Draws and fills the background to black.
            g.FillRectangle(b, x, y, width, height);
            g.DrawRectangle(p, x, y, width, height);
        }
    }
}
