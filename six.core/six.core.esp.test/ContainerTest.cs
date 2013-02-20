using six.core.esp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using six.core.interfaces.esp;

namespace six.core.esp.test
{


    /// <summary>
    ///This is a test class for ContainerTest and is intended
    ///to contain all ContainerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ContainerTest
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


        [TestMethod()]
        public void put_single_by_type()
        {
            var container = new Container();
            var stringClassObj = "test";
            container.Put(stringClassObj);
            var actual = container.Get<string>();
            Assert.AreSame(stringClassObj, actual);
        }

        [TestMethod()]
        public void put_single_by_name()
        {
            var container = new Container();
            var stringClassObj = "test";
            container.Put(stringClassObj, "stringClassObj");
            var actual = container.Get<string>("stringClassObj");
            Assert.AreSame(stringClassObj, actual);
        }

        [TestMethod()]
        public void put_two_of_same_type_diffrent_name_should_be_ok()
        {
            var container = new Container();
            var stringClassObj1 = "test1";
            var stringClassObj2 = "test2";
            container.Put(stringClassObj1, "stringClassObj1");
            container.Put(stringClassObj2, "stringClassObj2");
            var actual1 = container.Get<string>("stringClassObj1");
            Assert.AreSame(stringClassObj1, actual1);
            var actual2 = container.Get<string>("stringClassObj2");
            Assert.AreSame(stringClassObj2, actual2);

            // first should be registered as by type;
            var actual3 = container.Get<string>();
            Assert.AreSame(stringClassObj1, actual3);
        }

        [TestMethod()]
        public void put_two_of_same_name_should_fail()
        {
            var container = new Container();
            var obj1 = new object();
            var obj2 = new object();
            container.Put(obj1, "obj");
            try
            {
                container.Put(obj2, "obj");
            }
            catch (ArgumentException)
            {
                return;
            }
            catch
            {

            }
            Assert.Fail("adding second object of the same name is not allowed");
        }

        [TestMethod()]
        public void put_two_of_same_type_no_name_should_fail()
        {
            var container = new Container();
            var stringClassObj1 = "test1";
            var stringClassObj2 = "test2";
            container.Put(stringClassObj1);
            try
            {
                container.Put(stringClassObj2);
            }
            catch (ArgumentException)
            {
                return;
            }
            catch
            {

            }
            Assert.Fail("adding second object of the same type is not allowed");
        }

        interface IIfc1
        {
            void Fun1();
        }

        interface IIfc2
        {
            void Fun2();
        }

        abstract class CImpl1 : IIfc1
        {
            public abstract void Fun1();
        }

        class CImpl2 : CImpl1, IIfc2
        {
            public override void Fun1()
            {
            }

            public void Fun2()
            {
            }
        }

        [TestMethod()]
        public void get_by_interface()
        {
            var container = new Container();
            var actual = new CImpl2();
            container.Put(actual);
            var expected1 = container.Get<IIfc1>();
            Assert.AreSame(expected1,actual);
            var expected2 = container.Get<IIfc2>();
            Assert.AreSame(expected2, actual);
            var expected3 = container.Get<CImpl1>();
            Assert.AreSame(expected3, actual);
        }
    }
}
