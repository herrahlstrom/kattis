using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace test
{
    [TestClass]
    public class PointInPolygonTest
    {

        [TestMethod]
        public void TestCase1Point1()
        {
            var tc = GetCase1();
            var p = new pointinpolygon.Point { X = 4, Y = 5 };
            var result = tc.Test(p);
            Assert.AreEqual(pointinpolygon.Result.In, result, "Case 1, Point " + p.ToString());
        }
        [TestMethod]
        public void TestCase1Point2()
        {
            var tc = GetCase1();
            var p = new pointinpolygon.Point { X = 5, Y = 5 };
            var result = tc.Test(p);
            Assert.AreEqual(pointinpolygon.Result.On, result, "Case 1, Point " + p.ToString());

        }

        [TestMethod]
        public void TestCase1Point3()
        {
            var tc = GetCase1();
            var p = new pointinpolygon.Point { X = 6, Y = 5 };
            var result = tc.Test(p);
            Assert.AreEqual(pointinpolygon.Result.Out, result, "Case 1, Point " + p.ToString());
        }

        [TestMethod]
        public void TestCase2Point1()
        {
            var tc = GetCase2();
            var p = new pointinpolygon.Point { X = -12, Y = -26 };
            var result = tc.Test(p);
            Assert.AreEqual(pointinpolygon.Result.Out, result, "Case 2, Point " + p.ToString());
        }

        [TestMethod]
        public void TestCase2Point2()
        {
            var tc = GetCase2();
            var p = new pointinpolygon.Point { X = 39, Y = -8 };
            var result = tc.Test(p);
            Assert.AreEqual(pointinpolygon.Result.In, result, "Case 2, Point " + p.ToString());
        }

        private static pointinpolygon.TestCase GetCase1()
        {
            var tc = new pointinpolygon.TestCase();
            tc.AddPolygonPoint(new pointinpolygon.Point { X = 0, Y = 0 });
            tc.AddPolygonPoint(new pointinpolygon.Point { X = 10, Y = 0 });
            tc.AddPolygonPoint(new pointinpolygon.Point { X = 0, Y = 10 });
            return tc;
        }
        private static pointinpolygon.TestCase GetCase2()
        {
            var tc = new pointinpolygon.TestCase();
            tc.AddPolygonPoint(new pointinpolygon.Point { X = 41, Y = -6 });
            tc.AddPolygonPoint(new pointinpolygon.Point { X = -24, Y = -74 });
            tc.AddPolygonPoint(new pointinpolygon.Point { X = -51, Y = -6 });
            tc.AddPolygonPoint(new pointinpolygon.Point { X = 73, Y = 17 });
            tc.AddPolygonPoint(new pointinpolygon.Point { X = -30, Y = -34 });
            return tc;
        }

    }
}
