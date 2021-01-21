using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment2
{
    /// <summary>
    /// Class that sets up and draws a circle. The method have been inherited by the Shape class.
    /// </summary>
    public class Circle : Shape
    {
        //Sets up the radius variable to be set.
        int radius;

        /// <summary>
        /// Constructor that has the same fucntion as it's base class.
        /// </summary>
        public Circle() : base()
        {

        }

        /// <summary>
        /// This method which sets up the different parameters of the circle.
        /// </summary>
        /// <param name="colour">The colour of the circle.</param>
        /// <param name="x">The x coordinate where the circle will draw from.</param>
        /// <param name="y">The y coordinate where the circle will draw from.</param>
        /// <param name="radius">The radius which will define how big the circle will be.</param>
        public Circle(Color colour, int x, int y, int radius) : base(colour, x, y)
        {
            //The only thing that is different from the Shape class.
            this.radius = radius; 
        }

        /// <summary>
        /// This sets the variables to the parameters defined by the user in the program.
        /// </summary>
        /// <param name="colour">The colour of the shape.</param>
        /// <param name="list">The list array saves the information from the user's command from input.</param>
        public override void set(Color colour, params int[] list)
        {
            //list[0] is x, list[1] is y, list[2] is radius
            base.set(colour, list[0], list[1]);
            this.radius = list[2];
        }

        /// <summary>
        /// This sets the variables to the parameters defined by the user in the program.
        /// </summary>
        /// <param name="colour">The colour of the shape.</param>
        /// <param name="list">The list array saves the information from the user's command from input.</param>
        public override void set(TextureBrush brush, params int[] list)
        {
            //list[0] is x, list[1] is y, list[2] is radius
            base.set(brush, list[0], list[1]);
            this.radius = list[2];
        }

        /// <summary>
        /// The method which draws the circle.
        /// </summary>
        /// <param name="g"></param>
        public override void draw(Graphics g)
        {
            //Sets up the pen to draw the circle with the colour defined by the uer.
            Pen p = new Pen(colour, 2);
            SolidBrush b = new SolidBrush(colour);
            //Draw the circle.
            g.DrawEllipse(p, x, y, radius, radius);

        }

    }
}
