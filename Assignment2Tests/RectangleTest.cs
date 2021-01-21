using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment2Tests
{
    /// <summary>
    /// Test class to test the functions of setting up and drawing a rectangle.
    /// </summary>
    [TestClass]
    public class RectangleTest
    {
        /// <summary>
        /// Test method to test the conversion of rectangle parameters.
        /// </summary>
        [TestMethod]
        public void convertRectVariables_Invalid()
        {
            //Sets up the variables.
            int expected = 10;
            int hei = 0;
            int wid = 0;
            string height = "incorrectDatatype_height";
            string width = "incorrectDatatype_width";
            try
            {
                //Tries to convert with an incorrect format.
                hei = Convert.ToInt32(height);
                wid = Convert.ToInt32(width);
            }
            //Catches the exception as the format is wrong.
            catch (FormatException)
            {
                //Makes sure the expected and actual outcomes are different.
                Assert.AreNotEqual(expected, hei, 00.1, "Height Numbers Don't match.");
                Assert.AreNotEqual(expected, wid, 00.1, "Width Numbers Don't match.");
            }
        }
    }
}
