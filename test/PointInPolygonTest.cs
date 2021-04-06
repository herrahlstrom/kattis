using Microsoft.VisualStudio.TestTools.UnitTesting;
using pointinpolygon;

namespace test
{
    [TestClass]
    public class PointInPolygonTest
    {
        private static TestCase GetCase1()
        {
            var tc = new TestCase();
            tc.AddPolygonPoint(new Point {X = 0, Y = 0});
            tc.AddPolygonPoint(new Point {X = 10, Y = 0});
            tc.AddPolygonPoint(new Point {X = 0, Y = 10});
            return tc;
        }

        private static TestCase GetCase2()
        {
            var tc = new TestCase();
            tc.AddPolygonPoint(new Point {X = 41, Y = -6});
            tc.AddPolygonPoint(new Point {X = -24, Y = -74});
            tc.AddPolygonPoint(new Point {X = -51, Y = -6});
            tc.AddPolygonPoint(new Point {X = 73, Y = 17});
            tc.AddPolygonPoint(new Point {X = -30, Y = -34});
            return tc;
        }

        [TestMethod]
        public void TestCase1Point1()
        {
            TestCase tc = GetCase1();
            var p = new Point {X = 4, Y = 5};
            Result result = tc.Test(p);
            Assert.AreEqual(Result.In, result, "Case 1, Point " + p);
        }

        [TestMethod]
        public void TestCase1Point2()
        {
            TestCase tc = GetCase1();
            var p = new Point {X = 5, Y = 5};
            Result result = tc.Test(p);
            Assert.AreEqual(Result.On, result, "Case 1, Point " + p);
        }

        [TestMethod]
        public void TestCase1Point3()
        {
            TestCase tc = GetCase1();
            var p = new Point {X = 6, Y = 5};
            Result result = tc.Test(p);
            Assert.AreEqual(Result.Out, result, "Case 1, Point " + p);
        }

        [TestMethod]
        public void TestCase2Point1()
        {
            TestCase tc = GetCase2();
            var p = new Point {X = -12, Y = -26};
            Result result = tc.Test(p);
            Assert.AreEqual(Result.Out, result, "Case 2, Point " + p);
        }

        [TestMethod]
        public void TestCase2Point2()
        {
            TestCase tc = GetCase2();
            var p = new Point {X = 39, Y = -8};
            Result result = tc.Test(p);
            Assert.AreEqual(Result.In, result, "Case 2, Point " + p);
        }

        [TestMethod]
        public void TestMShapedPolygon()
        {
            var tc = new TestCase();
            tc.AddPolygonPoint(new Point {X = 0, Y = 0});
            tc.AddPolygonPoint(new Point {X = 0, Y = 10});
            tc.AddPolygonPoint(new Point {X = 4, Y = 5});
            tc.AddPolygonPoint(new Point {X = 8, Y = 10});
            tc.AddPolygonPoint(new Point {X = 8, Y = 0});

            var p1 = new Point {X = 0, Y = 6};
            Assert.AreEqual(Result.On, tc.Test(p1), "M Shape Case, Point " + p1);

            var p2 = new Point {X = 1, Y = 6};
            Assert.AreEqual(Result.In, tc.Test(p2), "M Shape Case, Point " + p2);

            var p3 = new Point {X = 4, Y = 6};
            Assert.AreEqual(Result.Out, tc.Test(p3), "M Shape Case, Point " + p3);

            var p4 = new Point {X = 7, Y = 6};
            Assert.AreEqual(Result.In, tc.Test(p4), "M Shape Case, Point " + p4);

            var p5 = new Point {X = 8, Y = 6};
            Assert.AreEqual(Result.On, tc.Test(p5), "M Shape Case, Point " + p5);

            var p6 = new Point {X = 9, Y = 6};
            Assert.AreEqual(Result.Out, tc.Test(p6), "M Shape Case, Point " + p6);
        }

        [TestMethod]
        public void TestTangentRayLinePolygon1()
        {
            var tc = new TestCase();
            tc.AddPolygonPoint(new Point {X = 0, Y = 0});
            tc.AddPolygonPoint(new Point {X = 0, Y = 4});
            tc.AddPolygonPoint(new Point {X = 6, Y = 4});
            tc.AddPolygonPoint(new Point {X = 6, Y = 0});
            tc.AddPolygonPoint(new Point {X = 4, Y = 0});
            tc.AddPolygonPoint(new Point {X = 4, Y = 2});
            tc.AddPolygonPoint(new Point {X = 2, Y = 2});
            tc.AddPolygonPoint(new Point {X = 2, Y = 0});

            // Y=0
            Assert.AreEqual(Result.Out, tc.Test(new Point {X = -1, Y = 0}));
            Assert.AreEqual(Result.On, tc.Test(new Point {X = 0, Y = 0}));
            Assert.AreEqual(Result.On, tc.Test(new Point {X = 1, Y = 0}));
            Assert.AreEqual(Result.On, tc.Test(new Point {X = 2, Y = 0}));
            Assert.AreEqual(Result.Out, tc.Test(new Point {X = 3, Y = 0}));
            Assert.AreEqual(Result.On, tc.Test(new Point {X = 4, Y = 0}));
            Assert.AreEqual(Result.On, tc.Test(new Point {X = 5, Y = 0}));
            Assert.AreEqual(Result.On, tc.Test(new Point {X = 6, Y = 0}));
            Assert.AreEqual(Result.Out, tc.Test(new Point {X = 7, Y = 0}));

            // Y=1
            Assert.AreEqual(Result.Out, tc.Test(new Point {X = -1, Y = 1}));
            Assert.AreEqual(Result.On, tc.Test(new Point {X = 0, Y = 1}));
            Assert.AreEqual(Result.In, tc.Test(new Point {X = 1, Y = 1}));
            Assert.AreEqual(Result.On, tc.Test(new Point {X = 2, Y = 1}));
            Assert.AreEqual(Result.Out, tc.Test(new Point {X = 3, Y = 1}));
            Assert.AreEqual(Result.On, tc.Test(new Point {X = 4, Y = 1}));
            Assert.AreEqual(Result.In, tc.Test(new Point {X = 5, Y = 1}));
            Assert.AreEqual(Result.On, tc.Test(new Point {X = 6, Y = 1}));
            Assert.AreEqual(Result.Out, tc.Test(new Point {X = 7, Y = 1}));

            // Y=2
            Assert.AreEqual(Result.Out, tc.Test(new Point {X = -1, Y = 2}));
            Assert.AreEqual(Result.On, tc.Test(new Point {X = 0, Y = 2}));
            Assert.AreEqual(Result.In, tc.Test(new Point {X = 1, Y = 2}));
            Assert.AreEqual(Result.On, tc.Test(new Point {X = 2, Y = 2}));
            Assert.AreEqual(Result.On, tc.Test(new Point {X = 3, Y = 2}));
            Assert.AreEqual(Result.On, tc.Test(new Point {X = 4, Y = 2}));
            Assert.AreEqual(Result.In, tc.Test(new Point {X = 5, Y = 2}));
            Assert.AreEqual(Result.On, tc.Test(new Point {X = 6, Y = 2}));
            Assert.AreEqual(Result.Out, tc.Test(new Point {X = 7, Y = 2}));
        }

        [TestMethod]
        public void TestTangentRayLinePolygon2()
        {
            var tc = new TestCase();
            tc.AddPolygonPoint(new Point {X = 0, Y = 0});
            tc.AddPolygonPoint(new Point {X = 0, Y = 3});
            tc.AddPolygonPoint(new Point {X = 6, Y = 3});
            tc.AddPolygonPoint(new Point {X = 6, Y = 2});
            tc.AddPolygonPoint(new Point {X = 4, Y = 2});
            tc.AddPolygonPoint(new Point {X = 4, Y = 1});
            tc.AddPolygonPoint(new Point {X = 2, Y = 1});
            tc.AddPolygonPoint(new Point {X = 2, Y = 0});

            // Y=0
            Assert.AreEqual(Result.Out, tc.Test(new Point {X = -1, Y = 0}), "X:-1 Y:0");
            Assert.AreEqual(Result.On, tc.Test(new Point {X = 0, Y = 0}), "X:0 Y:0");
            Assert.AreEqual(Result.On, tc.Test(new Point {X = 1, Y = 0}), "X:1 Y:0");
            Assert.AreEqual(Result.On, tc.Test(new Point {X = 2, Y = 0}), "X:2 Y:0");
            Assert.AreEqual(Result.Out, tc.Test(new Point {X = 3, Y = 0}), "X:3 Y:0");
            Assert.AreEqual(Result.Out, tc.Test(new Point {X = 4, Y = 0}), "X:4 Y:0");
            Assert.AreEqual(Result.Out, tc.Test(new Point {X = 5, Y = 0}), "X:5 Y:0");
            Assert.AreEqual(Result.Out, tc.Test(new Point {X = 6, Y = 0}), "X:6 Y:0");
            Assert.AreEqual(Result.Out, tc.Test(new Point {X = 7, Y = 0}), "X:7 Y:0");

            // Y=1
            Assert.AreEqual(Result.Out, tc.Test(new Point {X = -1, Y = 1}), "X:-1 Y:1");
            Assert.AreEqual(Result.On, tc.Test(new Point {X = 0, Y = 1}), "X:0 Y:1");
            Assert.AreEqual(Result.In, tc.Test(new Point {X = 1, Y = 1}), "X:1 Y:1");
            Assert.AreEqual(Result.On, tc.Test(new Point {X = 2, Y = 1}), "X:2 Y:1");
            Assert.AreEqual(Result.On, tc.Test(new Point {X = 3, Y = 1}), "X:3 Y:1");
            Assert.AreEqual(Result.On, tc.Test(new Point {X = 4, Y = 1}), "X:4 Y:1");
            Assert.AreEqual(Result.Out, tc.Test(new Point {X = 5, Y = 1}), "X:5 Y:1");
            Assert.AreEqual(Result.Out, tc.Test(new Point {X = 6, Y = 0}), "X:6 Y:1");
            Assert.AreEqual(Result.Out, tc.Test(new Point {X = 7, Y = 0}), "X:7 Y:1");

            // Y=2
            Assert.AreEqual(Result.Out, tc.Test(new Point {X = -1, Y = 2}), "X:-1 Y:2");
            Assert.AreEqual(Result.On, tc.Test(new Point {X = 0, Y = 2}), "X:0 Y:2");
            Assert.AreEqual(Result.In, tc.Test(new Point {X = 1, Y = 2}), "X:1 Y:2");
            Assert.AreEqual(Result.In, tc.Test(new Point {X = 2, Y = 2}), "X:2 Y:2");
            Assert.AreEqual(Result.In, tc.Test(new Point {X = 3, Y = 2}), "X:3 Y:2");
            Assert.AreEqual(Result.On, tc.Test(new Point {X = 4, Y = 2}), "X:4 Y:2");
            Assert.AreEqual(Result.On, tc.Test(new Point {X = 5, Y = 2}), "X:5 Y:2");
            Assert.AreEqual(Result.On, tc.Test(new Point {X = 6, Y = 0}), "X:6 Y:2");
            Assert.AreEqual(Result.Out, tc.Test(new Point {X = 7, Y = 0}), "X:7 Y:2");
        }
    }
}