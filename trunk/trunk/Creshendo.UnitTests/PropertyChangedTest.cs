using System;
using System.Reflection;
using Creshendo.UnitTests.Model;
using Creshendo.Util;
using NUnit.Framework;

namespace Creshendo.UnitTests
{
    [TestFixture]
    public class PropertyChangedTest
    {
        private int eCnt = 0;

        private void PropertyHasChanged(object sender, PropertyChangedHandlerEventArgs e)
        {
            Console.WriteLine(String.Format("Property: {0}, Old value: {1}, New value: {2}", e.PropertyName, e.OldValue, e.NewValue));
            eCnt++;
        }

        [Test]
        public void BasicTest()
        {
            eCnt = 0;
            Account account = new Account();
            account.PropertyChanged += PropertyHasChanged;
            account.First = "Manny";
            account.First = "Moe";
            account.First = "Jack";

            Assert.AreEqual(3, eCnt);
            account.PropertyChanged -= PropertyHasChanged;
            eCnt = 0;
            account.First = "Manny";
            account.First = "Moe";
            account.First = "Jack";
            Assert.AreEqual(0, eCnt);
        }

        [Test]
        public void ReflectionTest()
        {
            eCnt = 0;

            Type acct = typeof (Account);
            Account account = (Account) Activator.CreateInstance(acct);

            EventInfo evPropertyChanged = acct.GetEvent("PropertyChanged");
            Type tDelegate = evPropertyChanged.EventHandlerType;

            MethodInfo miHandler = typeof (PropertyChangedTest).GetMethod("PropertyHasChanged", BindingFlags.NonPublic | BindingFlags.Instance);


            // Create an instance of the delegate. Using the overloads
            // of CreateDelegate that take MethodInfo is recommended.
            //
            Delegate d = Delegate.CreateDelegate(tDelegate, this, miHandler);

            // Get the "add" accessor of the event and invoke it late-
            // bound, passing in the delegate instance. This is equivalent
            // to using the += operator in C#, or AddHandler in Visual
            // Basic. The instance on which the "add" accessor is invoked
            // is the form; the arguments must be passed as an array.
            //
            MethodInfo addHandler = evPropertyChanged.GetAddMethod();
            Object[] addHandlerArgs = {d};
            addHandler.Invoke(account, addHandlerArgs);

            //account.PropertyChanged += PropertyHasChanged;
            account.First = "Manny";
            account.First = "Moe";
            account.First = "Jack";

            //account.PropertyChanged -= PropertyHasChanged;

            Assert.AreEqual(3, eCnt);


            MethodInfo removeHandler = evPropertyChanged.GetRemoveMethod();
            Object[] removeHandlerArgs = {d};
            removeHandler.Invoke(account, removeHandlerArgs);

            eCnt = 0;
            account.First = "Manny";
            account.First = "Moe";
            account.First = "Jack";
            Assert.AreEqual(0, eCnt);
        }
    }
}