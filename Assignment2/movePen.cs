using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment2
{
    /// <summary>
    /// The move pen class will move the pen around the screen without having to draw anything.
    /// It inherits it's method from the Shape class.
    /// </summary>
    class movePen : Shape
    {
        //Sets up the starting x and y position of the transparent pen.
        int xpos, ypos;

        /// <summary>
        /// Constructor same as the Shape class.
        /// </summary>
        public movePen() : base()
        {

        }

        /// <summary>
        /// The move pen method that sets up the variables.
        /// </summary>
        /// <param name="colour">The colour will be transparent.</param>
        /// <param name="x">The x coordinate it will draw too.</param>
        /// <param name="y">The y coordinate it will draw too.</param>
        public movePen(Color colour, int x, int y) : base(colour, x, y)
        {

            this.x = x; //The only thing that is different from shape
            this.y = y;

        }

        /// <summary>
        /// Sets the variables to the parameters defined by the user.
        /// </summary>
        /// <param name="colour">The colour will be transparent.</param>
        /// <param name="list">The list of variables.</param>
        public override void set(Color colour, params int[] list)
        {
            //list[0] is starting x position, list[1] is startig y position, list[2] is ending x position, list[3] is ending y position.
            base.set(colour, list[0], list[1]);
            this.xpos = list[0];
            this.ypos = list[1];
            this.x = list[2];
            this.y = list[3];

        }

        /// <summary>
        /// The method that will perfrom the move pen fucntion.
        /// </summary>
        /// <param name="g"></param>
        public override void draw(Graphics g)
        {
            // Create pen making the colour transparent.
            Pen pen = new Pen(Color.Transparent, 1);

            // Create points that define line.
            Point point1 = new Point(xpos, ypos);
            Point point2 = new Point(x, y);

            pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Bevel;

            // Draw line to screen.
            g.DrawLine(pen, point1, point2);
        }
    }
}
