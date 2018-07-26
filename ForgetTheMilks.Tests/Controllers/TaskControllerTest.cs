using NUnit.Framework;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using ForgetTheMilks;
using ForgetTheMilks.Controllers;

namespace ForgetTheMilks.Tests.Controllers
{
    [TestFixture()]
    public class CreateTaskTests : AssertionHelper
    {
        class IgnoreLinkValidator : ILinkValidator
        {
            public void Validate(string link)
            {

            }
        }

        [Test()]
        public void DescriptionAndNoDueDate()
        {
            var input = "Pickup the groceries";

            var task = new Task(input, default(DateTime));


            Expect(task.Description, Is.EqualTo(input));
            Assert.AreEqual(null, task.DueDate);
        }

        [Test()]
        [TestCase("Pickup the groceries may 5 - as of 2015-05-31")]
        [TestCase("Pickup the groceries apr 5 - as of 2015-05-31")]
        public void MayDueDateDoesWrapYear(string input)
        {
            var today = new DateTime(2015, 5, 31);

            var task = new Task(input, today);

            Expect(task.DueDate.Value.Year, Is.EqualTo(2016));
        }

        [Test()]
        public void MayDueDateDoesNotWrapYear()
        {
            var input = "Pickup the groceries may 5 - as of 2015-05-04";
            var today = new DateTime(2015, 5, 4);

            var task = new Task(input, today);

            Expect(task.DueDate, Is.EqualTo(new DateTime(2015, 5, 5)));
        }

        [Test()]
        [TestCase("Groceries jan 5", 1)]
        [TestCase("Groceries feb 5", 2)]
        [TestCase("Groceries mar 5", 3)]
        [TestCase("Groceries apr 5", 4)]
        [TestCase("Groceries may 5", 5)]
        [TestCase("Groceries jun 5", 6)]
        [TestCase("Groceries jul 5", 7)]
        [TestCase("Groceries aug 5", 8)]
        [TestCase("Groceries sep 5", 9)]
        [TestCase("Groceries oct 5", 10)]
        [TestCase("Groceries nov 5", 11)]
        [TestCase("Groceries dec 5", 12)]
        public void DueDate(string input, int expectedMonth)
        {
            var today = new DateTime(2015, 5, 31);

            var task = new Task(input, today);

            Expect(task.DueDate, Is.Not.Null);
            Expect(task.DueDate.Value.Month, Is.EqualTo(expectedMonth));
        }

        [Test()]
        public void TwoDigitDay_ParseBothDigits()
        {
            var input = "Groceries apr 10";
            var task = new Task(input, default(DateTime));

            Expect(task.DueDate.Value.Day, Is.EqualTo(10));
        }

        [Test()]
        public void DayIsPastTheLastDayOfTheMonth_DoesNotParseDueDate()
        {
            var input = "Groceries apr 44";
            var task = new Task(input, default(DateTime));

            Expect(task.DueDate, Is.Null);
        }

        [Test()]
        public void AddFeb29TaskInMarchOfYearBeforeLeapYear_ParsesDueDate()
        {
            var input = "Groceries feb 29";
            var today = new DateTime(2015, 3, 1);

            var task = new Task(input, today);

            Expect(task.DueDate.Value, Is.EqualTo(new DateTime(2016, 2, 29)));
        }

        [Test()]
        [TestCase("Buy Groceries !1", "1")]
        [TestCase("Buy Groceries !2", "2")]
        [TestCase("Buy Groceries !3", "3")]
        [TestCase("Buy Groceries", "Not Set")]
        public void Priority(string input, string expectedPriority)
        {
            var task = new Task(input, default(DateTime));

            Expect(task.Priority, Is.EqualTo(expectedPriority));
        }

        [Test()]
        public void PriorityAddedWithLink_ParsesPriorityAndLink()
        {
            var input = "Buy Groceries !3 http://www.google.com";
            var task = new Task(input, default(DateTime), new IgnoreLinkValidator());

            Expect(task.Priority, Is.EqualTo("3"));
            Expect(task.Link, Is.EqualTo("http://www.google.com"));
        }

        [Test()]
        public void PriorityAddedWithLinkAndDueDate_ParsesPriorityLinkAndDueDate()
        {
            var input = "Buy Groceries sep 28 !2 http://www.google.com";
            var today = new DateTime(2018, 7, 26);
            var task = new Task(input, today, new IgnoreLinkValidator());

            Expect(task.DueDate, Is.EqualTo(new DateTime(2018, 9, 28)));
            Expect(task.Priority, Is.EqualTo("2"));
            Expect(task.Link, Is.EqualTo("http://www.google.com"));
        }
    }
}
