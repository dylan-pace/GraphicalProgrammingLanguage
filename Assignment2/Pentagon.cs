using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment2
{
    /// <summary>
    /// Sets up and draws a triangle. The methods are inherited by the Shape class.
    /// </summary>
    class Pentagon : Shape
    {
        //Sets up the points and the position of the triangle.
        Point points;
        private int xpos, ypos, z, a;

        /// <summary>
        /// Constructor.
        /// </summary>
        public Pentagon() : base()
        {
        }

        /// <summary>
        /// Sets up the variables for the triangle.
        /// </summary>
        /// <param name="colour">The colour of the pentagon.</param>
        /// <param name="x">The x position of the pentagon.</param>
        /// <param name="y">The y position of the pentagon.</param>
        /// <param name="z">The z position of the pentagon.</param>
        /// <param name="points">The points of the pentagon.</param>
        public Pentagon(Color colour, int x, int y, int z, int a, Point points)
        {

            this.points = points; //The only thing that is different from the Shape class.
            this.x = x;
            this.y = y;
            this.z = z;
            this.a = a;
        }

        /// <summary>
        /// Sets the variables to the parameters defined by the user.
        /// </summary>
        /// <param name="colour">The colour of the pentagon.</param>
        /// <param name="list">The list of variables.</param>
        public override void set(Color colour, params int[] list)
        {
            //list[0] is x, list[1] is y, list[2] onwards are the points.
            base.set(colour, list[0], list[1]);
            this.xpos = list[0];
            this.ypos = list[1];
            this.x = list[2];
            this.y = list[3];
            this.z = list[4];
            this.a = list[5];

        }

        /// <summary>
        /// Draws the pentagon.
        /// </summary>
        /// <param name="g"></param>
        public override void draw(Graphics g)
        {
            //Sets up the pen with the desired colour.
            Pen p = new Pen(colour, 2);
            SolidBrush b = new SolidBrush(colour);
            //Create points that define polygon.
            Point point1 = new Point(xpos, ypos);
            Point point2 = new Point(x, y);
            Point point3 = new Point(y, z);
            Point point4 = new Point(z, a);
            Point point5 = new Point(xpos, ypos);
            Point[] curvePoints =
                     {
                 point1,
                 point2,
                 point3,
                 point4,
                 point5
             };
            //Draw and fill the triangle.
            g.FillPolygon(b, curvePoints);
            g.DrawPolygon(p, curvePoints);
        }
    }
}
