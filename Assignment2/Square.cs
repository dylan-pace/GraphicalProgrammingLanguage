using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment2
{
    /// <summary>
    /// Class that sets up and draws a square. The methods have been inherited by the Rectangle class.
    /// </summary>
    class Square : Rectangle
    {
        //Sets up the variable for the size of the side.
        private int size;

        /// <summary>
        /// Constructor.
        /// </summary>
        public Square():base()
        {

        }

        /// <summary>
        /// Sets up the variables for the square from the user's parameters.
        /// </summary>
        /// <param name="colour">The colour of the square.</param>
        /// <param name="x">The starting x position of the square.</param>
        /// <param name="y">The starting y position of the square.</param>
        /// <param name="size">The size of the square.</param>
        public Square(Color colour, int x, int y, int size) : base(colour, x, y, size, size)
        {
            this.size = size;
        }

        //No draw method here because it is provided by the parent class Rectangle.
        public override void draw(Graphics g)
        {
            //Draws the square.
            base.draw(g);
        }

    }
}
