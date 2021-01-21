using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment2Tests
{
    [TestClass]
    class StatementTests
    {

        [TestMethod]
        public void testIfStatements()
        {
            try
            {
                int num = 10;
                int num2 = 20;
                string symbol = "=";
                if (symbol == "=")
                {
                    if (num == num2)
                    {
                        Assert.AreEqual(num, num2, 00.1, "Numbers match.");
                    }
                    else
                    {
                        Assert.AreNotEqual(num, num2, 00.1, "Numbers don't match.");
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Symbol isn't right.");
            }
        }
    }
}
