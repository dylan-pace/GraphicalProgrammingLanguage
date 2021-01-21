using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment2
{
    /// <summary>
    /// The factory class gets the shape defined by the user and poits the program to run the code for those particular shapes.
    /// </summary>
    class ShapeFactory
    {
        /// <summary>
        /// Gets the shapes that are sent from the user's code.
        /// </summary>
        /// <param name="shapeType"></param>
        /// <returns></returns>
        public Shape getShape(String shapeType)
        {
            //Trims the shapes so they are consistant. 
            shapeType = shapeType.ToUpper().Trim(); //you could argue that you want a specific word string to create an object but I'm allowing any case combination

            //If the circle is defined run the circle class.
            if (shapeType.Equals("CIRCLE"))
            {
                return new Circle();

            }
            //If the rectangle is defined then run the rectangle class.
            if (shapeType.Equals("RECTANGLE"))
            {
                return new Rectangle();

            }
            //If the square is defined then run the square class.
            if (shapeType.Equals("SQUARE"))
            {
                return new Square();

            }
            //If the triangle is defined then run the triangle class.
            if (shapeType.Equals("TRIANGLE"))
            {
                return new Triangle();

            }
            //If the pentagon is defined then run the pentagon class.
            if (shapeType.Equals("PENTAGON"))
            {
                return new Pentagon();

            }
            //If drawline is defined then run the drawline class.
            if (shapeType.Equals("DRAWLINE"))
            {
                return new drawLine();

            }
            //If movepen is defined then run the movepen class.
            if (shapeType.Equals("MOVEPEN"))
            {
                return new movePen();
            }
            //If clear is defined then run the clear class.
            if (shapeType.Equals("CLEAR"))
            {
                return new Clear();
            }
            else
            {
                //if we get here then what has been passed in is inkown so throw an appropriate exception
                ArgumentException argEx = new ArgumentException("Factory error: " + shapeType + " does not exist");
                throw argEx;
            }
        }
    }
}
