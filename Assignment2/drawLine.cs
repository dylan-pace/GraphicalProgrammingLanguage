using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment2
{
    /// <summary>
    /// Class that sets up and draws a line on the screen. Inherits its methods from the Shape class.
    /// </summary>
    class drawLine : Shape
    {
        //Sets up the x and y variables.
        int xpos, ypos;

        /// <summary>
        /// Constructor.
        /// </summary>
        public drawLine() : base()
        {

        }

        /// <summary>
        /// Sets up the draw line variables.
        /// </summary>
        /// <param name="colour">The colour of the line.</param>
        /// <param name="x">The x position of the line.</param>
        /// <param name="y">The y position of the line.</param>
        public drawLine(Color colour, int x, int y) : base(colour, x, y)
        {

            this.x = x; //The only thing that is different from shape
            this.y = y;

        }

        /// <summary>
        /// Sets the variables to the parameters.
        /// </summary>
        /// <param name="colour">The colour of the line.</param>
        /// <param name="list">The list of variables.</param>
        public override void set(Color colour, params int[] list)
        {
            //List[0] is x, list[1] is y, list[2] is width, list[3] is height.
            base.set(colour, list[0], list[1]);
            this.xpos = list[0];
            this.ypos = list[1];
            this.x = list[2];
            this.y = list[3];

        }

        /// <summary>
        /// Draws the line to the screen.
        /// </summary>
        /// <param name="g"></param>
        public override void draw(Graphics g)
        {
            //Create pen and set the colour.
            Pen pen = new Pen(colour, 1);

            //Create points that define line.
            Point point1 = new Point(xpos, ypos);
            Point point2 = new Point(x, y);

            pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Bevel;

            //Draw line to screen.
            g.DrawLine(pen, point1, point2);
        }
    }
}
