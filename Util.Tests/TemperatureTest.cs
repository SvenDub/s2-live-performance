using System;
using NUnit.Framework;

namespace Util.Tests
{
    public class TemperatureTest
    {
        [Test]
        public void TestWindChill()
        {
            Assert.AreEqual(27.87, Temperature.WindChill(28, 2));
            Assert.AreEqual(27.38, Temperature.WindChill(28, 3));
            Assert.AreEqual(25.80, Temperature.WindChill(27, 4));
            Assert.AreEqual(17.36, Temperature.WindChill(0, 0));
            Assert.AreEqual(1.78, Temperature.WindChill(10, 7));
            Assert.AreEqual(29.21, Temperature.WindChill(30, 5));
            Assert.AreEqual(-21.34, Temperature.WindChill(-20, 2));
            Assert.AreEqual(4.76, Temperature.WindChill(1, 1));
            Assert.AreEqual(2.99, Temperature.WindChill(-1, 1));
        }

        [Test]
        public void TestWindChillNegative()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Temperature.WindChill(1, -1));
        }
    }
}