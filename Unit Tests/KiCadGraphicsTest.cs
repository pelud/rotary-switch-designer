using Rotary_Switch_Designer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Drawing;

namespace Unit_Tests
{
    /// <summary>
    ///This is a test class for WaferExportKiCad_KiCadGraphicsTest and is intended
    ///to contain all WaferExportKiCad_KiCadGraphicsTest Unit Tests
    ///</summary>
    [TestClass()]
    public class KiCadGraphicsTest
    {
        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        /// <summary>
        ///A test for Arc
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Rotary Switch Designer.exe")]
        public void ArcTest()
        {
            // A 0 0 200 -1799 -1 4 0 0 N -200 0 200 0
            var target = new WaferExportKiCad_Accessor.KiCadGraphics();
            target.Unit = 4;
            Point center = new Point(0, 0);
            int radius = 200;
            float start_angle = -180F;
            float sweep_angle = 180F;
            bool filled = false;
            string result = target.Arc(center, radius, start_angle, sweep_angle, filled);
            Assert.AreEqual("A 0 0 200 -1800 0 4 0 0 N -200 0 200 0", result);
        }

        /// <summary>
        ///A test for Arc
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Rotary Switch Designer.exe")]
        public void ArcTest2()
        {
            // A 0 0 158 1084 715 0 1 0 N -50 150 50 150
            var target = new WaferExportKiCad_Accessor.KiCadGraphics();
            target.Unit = 0;
            Point center = new Point(0, 0);
            int radius = 158;
            float start_angle = 108.4F;
            float sweep_angle = 71.5F - 108.4f;
            bool filled = false;
            string result = target.Arc(center, radius, start_angle, sweep_angle, filled);
            Assert.AreEqual("A 0 0 158 1084 715 0 0 0 N -50 150 50 150", result);
        }

#if false
        /// <summary>
        ///A test for Arc
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Rotary Switch Designer.exe")]
        public void ArcTest2()
        {
            // A 0 0 158 1084 715 0 1 0 N -50 150 50 150
            var target = new WaferExportKiCad_Accessor.KiCadGraphics();
            target.Unit = 0;
            Point center = new Point(0, 0);
            int radius = 158;
            float start_angle = 108.4F;
            float sweep_angle = 71.5F - 108.4f;
            bool filled = false;
            string result = target.Arc(center, radius, start_angle, sweep_angle, filled);
            Assert.AreEqual("A 0 0 158 1084 715 0 0 0 N -50 150 50 150", result);
        }
#endif

        /// <summary>
        ///A test for Circle
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Rotary Switch Designer.exe")]
        public void CircleTest()
        {
            // C 100 150 50 5 0 0 F
            var target = new WaferExportKiCad_Accessor.KiCadGraphics();
            target.Unit = 5;
            Point center = new Point(100, 150);
            int radius = 50;
            bool filled = true;
            string result = target.Circle(center, radius, filled);
            Assert.AreEqual("C 100 150 50 5 0 0 F", result);
        }

        /// <summary>
        ///A test for Polygon
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Rotary Switch Designer.exe")]
        public void PolygonTest()
        {
            // P 5 2 0 0  -200 0  0 200  200 0  0 -150  -200 0 N
            var target = new WaferExportKiCad_Accessor.KiCadGraphics();
            target.Unit = 2;
            Point[] points = new Point[]
            {
                new Point(-200, 0),
                new Point(0, 200),
                new Point(200, 0),
                new Point(0, -150),
                new Point(-200, 0)
            };
            bool filled = false;
            string result = target.Polygon(points, filled);
            Assert.AreEqual("P 5 2 0 0  -200 0  0 200  200 0  0 -150  -200 0 N", result);
        }

        /// <summary>
        ///A test for Pin
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Rotary Switch Designer.exe")]
        public void PinTest()
        {
            var target = new WaferExportKiCad_Accessor.KiCadGraphics();
            target.Unit = 1;
            Point p = new Point(-550, 0);
            int number = 1;
            string name = "~";
            string expected = "X ~ 1 -550 0 0 L 60 60 1 0 I";
            string actual;
            actual = target.Pin(p, number, name);
            Assert.AreEqual(expected, actual);
        }

#if false
        /// <summary>
        ///A test for Result
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Rotary Switch Designer.exe")]
        public void ResultTest()
        {
            WaferExportKiCad_Accessor.KiCadGraphics target = new WaferExportKiCad_Accessor.KiCadGraphics(); // TODO: Initialize to an appropriate value
            target.Unit = 1;
            target.Name = "part";
            target.Client = new Rectangle(-300, -300, 600, 600);
            target.DrawString("john", new System.Drawing.Point(0, 0));
            string expected = "#\r\n# part\r\n#\r\nDEF part S 0 60 Y N 1 L N\r\nF0 \"S\" 0 -360 60 H V C CNN\r\nF1 \"part\" 0 300 60 H V C CNN\r\nF2 \"~\" 0 0 60 H V C CNN\r\nF3 \"~\" 0 0 60 H V C CNN\r\nDRAW\r\nT 0 0 0 60 1 0 john\r\nENDDRAW\r\nENDDEF\r\n";
            string actual = target.Result();
            Assert.AreEqual(expected, actual);
        }
#endif

        /// <summary>
        ///A test for TransformPosition
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Rotary Switch Designer.exe")]
        public void TransformPositionTest()
        {
            WaferExportKiCad_Accessor.KiCadGraphics target = new WaferExportKiCad_Accessor.KiCadGraphics(); // TODO: Initialize to an appropriate value
            target.Client = new Rectangle(-100, -100, 200, 200);
            Point position = new Point(2, 51); // TODO: Initialize to an appropriate value
            Point expected = new Point(2, -51); // TODO: Initialize to an appropriate value
            Point actual;
            actual = target.TransformPosition(position);
            Assert.AreEqual(expected, actual);
        }
    }
}
