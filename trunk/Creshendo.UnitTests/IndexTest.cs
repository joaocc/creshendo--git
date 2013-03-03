using Creshendo.UnitTests.Model;
using Creshendo.Util.Collections;
using Creshendo.Util.Rete;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Creshendo.UnitTests
{
    [TestClass]
    public class IndexTest
    {
        [TestMethod]
        public void testFiveFacts()
        {
            Defclass dc = new Defclass(typeof (TestBean2));
            Deftemplate dtemp = dc.createDeftemplate("testBean2");
            TestBean2 bean = new TestBean2();
            bean.Attr1 = "testString";
            bean.Attr2 = 1;
            short a3 = 3;
            bean.Attr3 = a3;
            long a4 = 101;
            bean.Attr4 = a4;
            float a5 = 10101;
            bean.Attr5 = a5;
            double a6 = 101.101;
            bean.Attr6 = a6;

            TestBean2 bean2 = new TestBean2();
            bean2.Attr1 = "testString2";
            bean2.Attr2 = 12;
            short a32 = 32;
            bean2.Attr3 = a32;
            long a42 = 1012;
            bean2.Attr4 = a42;
            float a52 = 101012;
            bean2.Attr5 = a52;
            double a62 = 101.1012;
            bean2.Attr6 = a62;

            TestBean2 bean3 = new TestBean2();
            bean3.Attr1 = "testString3";
            bean3.Attr2 = 13;
            short a33 = 33;
            bean3.Attr3 = a33;
            long a43 = 1013;
            bean3.Attr4 = a43;
            float a53 = 101013;
            bean3.Attr5 = a53;
            double a63 = 101.1013;
            bean3.Attr6 = a63;

            TestBean2 bean4 = new TestBean2();
            bean4.Attr1 = "testString4";
            bean4.Attr2 = 14;
            short a34 = 34;
            bean4.Attr3 = a34;
            long a44 = 1014;
            bean4.Attr4 = a44;
            float a54 = 101014;
            bean4.Attr5 = a54;
            double a64 = 101.1014;
            bean4.Attr6 = a64;

            TestBean2 bean5 = new TestBean2();
            bean5.Attr1 = "testString5";
            bean5.Attr2 = 15;
            short a35 = 35;
            bean5.Attr3 = a35;
            long a45 = 1015;
            bean5.Attr4 = a45;
            float a55 = 101015;
            bean5.Attr5 = a55;
            double a65 = 101.1015;
            bean5.Attr6 = a65;

            IFact fact = dtemp.createFact(bean, dc, 1);
            IFact fact2 = dtemp.createFact(bean2, dc, 1);
            IFact fact3 = dtemp.createFact(bean3, dc, 1);
            IFact fact4 = dtemp.createFact(bean4, dc, 1);
            IFact fact5 = dtemp.createFact(bean5, dc, 1);

            IFact[] list1 = new IFact[] {fact, fact2, fact3, fact4, fact5};
            IFact[] list2 = new IFact[] {fact, fact2, fact3, fact4, fact5};

            Index in1 = new Index(list1);
            Index in2 = new Index(list2);
            Assert.AreEqual(true, in1.Equals(in2));
        }

        [TestMethod]
        public void testHashMapIndex()
        {
            Defclass dc = new Defclass(typeof (TestBean2));
            Deftemplate dtemp = dc.createDeftemplate("testBean2");
            TestBean2 bean = new TestBean2();
            bean.Attr1 = "testString";
            bean.Attr2 = 1;
            short a3 = 3;
            bean.Attr3 = a3;
            long a4 = 101;
            bean.Attr4 = a4;
            float a5 = 10101;
            bean.Attr5 = a5;
            double a6 = 101.101;
            bean.Attr6 = a6;

            TestBean2 bean2 = new TestBean2();
            bean2.Attr1 = "testString2";
            bean2.Attr2 = 12;
            short a32 = 32;
            bean2.Attr3 = a32;
            long a42 = 1012;
            bean2.Attr4 = a42;
            float a52 = 101012;
            bean2.Attr5 = a52;
            double a62 = 101.1012;
            bean2.Attr6 = a62;

            TestBean2 bean3 = new TestBean2();
            bean3.Attr1 = "testString3";
            bean3.Attr2 = 13;
            short a33 = 33;
            bean3.Attr3 = a33;
            long a43 = 1013;
            bean3.Attr4 = a43;
            float a53 = 101013;
            bean3.Attr5 = a53;
            double a63 = 101.1013;
            bean3.Attr6 = a63;

            TestBean2 bean4 = new TestBean2();
            bean4.Attr1 = "testString4";
            bean4.Attr2 = 14;
            short a34 = 34;
            bean4.Attr3 = a34;
            long a44 = 1014;
            bean4.Attr4 = a44;
            float a54 = 101014;
            bean4.Attr5 = a54;
            double a64 = 101.1014;
            bean4.Attr6 = a64;

            TestBean2 bean5 = new TestBean2();
            bean5.Attr1 = "testString5";
            bean5.Attr2 = 15;
            short a35 = 35;
            bean5.Attr3 = a35;
            long a45 = 1015;
            bean5.Attr4 = a45;
            float a55 = 101015;
            bean5.Attr5 = a55;
            double a65 = 101.1015;
            bean5.Attr6 = a65;

            IFact fact = dtemp.createFact(bean, dc, 1);
            IFact fact2 = dtemp.createFact(bean2, dc, 1);
            IFact fact3 = dtemp.createFact(bean3, dc, 1);
            IFact fact4 = dtemp.createFact(bean4, dc, 1);
            IFact fact5 = dtemp.createFact(bean5, dc, 1);

            IFact[] list1 = new IFact[] {fact, fact2, fact3, fact4, fact5};
            IFact[] list2 = new IFact[] {fact, fact2, fact3, fact4, fact5};

            Index in1 = new Index(list1);
            Index in2 = new Index(list2);
            Assert.AreEqual(true, in1.Equals(in2));

            GenericHashMap<object, object> map = new GenericHashMap<object, object>();
            map.Put(in1, list1);
            // simple test to see if HashMap.ContainsKey(in1) works
            Assert.AreEqual(true, map.ContainsKey(in1));
            // now test with the second instance of index, this should return
            // true, since Index class overrides Equals() and GetHashCode().
            Assert.AreEqual(true, map.ContainsKey(in2));
        }

        [TestMethod]
        public void testObjectEquals()
        {
            long l1 = 2;
            long l2 = 2;
            Assert.AreEqual(l1, l2);
        }

        [TestMethod]
        public void testOneFact()
        {
            Defclass dc = new Defclass(typeof (TestBean2));
            Deftemplate dtemp = dc.createDeftemplate("testBean2");
            TestBean2 bean = new TestBean2();
            bean.Attr1 = "testString";
            bean.Attr2 = 1;
            short a3 = 3;
            bean.Attr3 = a3;
            long a4 = 101;
            bean.Attr4 = a4;
            float a5 = 10101;
            bean.Attr5 = a5;
            double a6 = 101.101;
            bean.Attr6 = a6;

            IFact fact = dtemp.createFact(bean, dc, 1);
            IFact[] list1 = new IFact[] {fact};
            IFact[] list2 = new IFact[] {fact};

            Index in1 = new Index(list1);
            Index in2 = new Index(list2);
            Assert.AreEqual(true, in1.Equals(in2));
        }
    }
}