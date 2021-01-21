using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment2
{
    /// <summary>
    /// Abstract class that sets up the method which will be used by all shapes to set them up and draw them.
    /// </summary>
    public abstract class Shape
    {
        protected TextureBrush brush;
        protected Color colour; //Shape's colour.
        protected int x, y; //X and y coordinate of the shape.

        /// <summary>
        /// Constructor
        /// </summary>
        public Shape()
        {
        
        }

        /// <summary>
        /// Sets the variables up for the different shapes to use.
        /// </summary>
        /// <param name="colour">The colour the shape will be.</param>
        /// <param name="x">The x position of the shape.</param>
        /// <param name="y">The y poisiton of the shape.</param>
        public Shape(Color colour, int x, int y)
        {
            this.brush = brush;
            this.colour = colour; //Shape's colour.
            this.x = x; //Its x position.
            this.y = y; //Its y position.
            //Can't provide anything else as "shape" is too general.
        }

        //The three methods below are from the Shapes interface.
        //Here we are passing on the obligation to implement them to the derived classes by declaring them as abstract.
        public abstract void draw(Graphics g); //Any derrived class must implement this method.

        //Set is declared as virtual so it can be overridden by a more specific child version.
        //But is here so it can be called by that child version to do the generic operations.
        //Note the use of the param keyword to provide a variable parameter list to cope with some shapes having more setup information than others.
        /// <summary>
        /// Will be implemented in the child classes to set the variables.
        /// </summary>
        /// <param name="colour">The colour of the shape.</param>
        /// <param name="list">The list is an array in which the parameters have been saved in for the different shapes.</param>
        public virtual void set(Color colour, params int[] list)
        {
            //Sets the parameters to the variables.
            this.colour = colour;
            this.x = list[0];
            this.y = list[1];
        }

        public virtual void set(TextureBrush brush, params int[] list)
        {
            //Sets the parameters to the variables.
            this.brush = brush;
            this.x = list[0];
            this.y = list[1];
        }

    }
}
