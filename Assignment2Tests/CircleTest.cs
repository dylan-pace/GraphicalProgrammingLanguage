using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assignment2;
using System.Drawing;
using System.Windows.Forms;
using System.Security.Cryptography;

namespace Assignment2Tests
{
    /// <summary>
    /// A test class that tests the functionality of creating and drawing a circle.
    /// </summary>
    [TestClass]
    public class CircleTest
    {
        //Instantiates Form1 to gets its variables.
        Form1 form1 = new Form1();

        /// <summary>
        /// Test method to test an invalid radius conversion from string to integar.
        /// </summary>
        [TestMethod]
        public void convertRadius_Invalid()
        {
            //Sets up the expected and actual variables.
            int expected = 10;
            int rep = 0;
            string radius = "incorrectDatatype";
            try
            {
                //Should fail because incorrect data type has been entered.
                rep = Convert.ToInt32(radius);
            }
            //Catches the exception as the format was wrong.
            catch (FormatException)
            {
                //Asserts that the expected and actual results are not the same.
                Assert.AreNotEqual(expected, rep, 00.1, "Numbers Don't match.");
            }
        }

        /// <summary>
        /// Test method that tests for whether the radius input was valid.
        /// </summary>
        [TestMethod]
        public void convertRadius_Valid()
        {
            //Sets up the expected and actual variables.
            int expected = 10;
            int actual = 10;
            string radius = "10";
            //Converts the integar.
            actual = Convert.ToInt32(radius);
            //Makes sure the two values are equal.
            Assert.AreEqual(expected, actual, 00.1, "Numbers match");
        }

        /// <summary>
        /// Test method that tests for whether the circle parameters are in the correct format.
        /// </summary>
        [TestMethod]
        public void drawCircle_CorrectFormat()
        {
            try
            {
                //Sets up the variables.
                int x = 0;
                int y = 0;
                int radius = 50;
                Color newColor = Color.Blue;
                Pen pen = new Pen(newColor, 2);
                //Passes the variables through the Circle method.
                Circle circle = new Circle(newColor, x, y, radius);
            }
            catch (Exception)
            {
                //Catches the exception if the variables couldn't pass.
                Assert.Fail("Cannot pass variable");
            }
        }

        /// <summary>
        /// Test method that tests whether the circle variables are in the incorrect format.
        /// </summary>
        [TestMethod]
        public void drawCircle_IncorrectFormat()
        {
            try
            {
                //Sets up the variables.
                int x = 0;
                int y = 0;
                int radius = -50;
                Color newColor = Color.Blue;
                Pen pen = new Pen(newColor, 2);
                if (radius < 0)
                {
                    //Passes through the Circle method.
                    Circle circle = new Circle(newColor, x, y, radius);
                }
            }catch (Exception)
            {
                //Catches the excpetion if something goes wrong.
                Assert.Fail("Integer too small.");
            }
        }

        /// <summary>
        /// Test method that tests to see if the user has entered too many parameters.
        /// </summary>
        [TestMethod]
        public void commandLine_TooManyParams()
        {
            try
            {
                //Sets up the parameters.
                string[] command = { "circle", "4" };
                string comm = command[0];
                string rad1 = command[1];
                string rad2 = command[2];
                //Checks to see if theres too many.
                Assert.Fail("too many params");
            }
            //Catch the exception if there are too many parameters.
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("too many params");
            }
        }
    }
}
