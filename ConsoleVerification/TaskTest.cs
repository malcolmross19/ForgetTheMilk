using System;
using ForgetTheMilk.Controllers;


namespace ConsoleVerification
{
    using NUnit.Framework;

    [TestFixture()]
    public class TaskTest
    {
        [Test()]
        public void TestDescriptionAndNoDueDate()
        {
            var input = "Pickup the groceries";
            Console.WriteLine("Scenario: " + input);

            var task = new Task(input, default(DateTime));

            var descriptionShouldBe = input;
            DateTime? dueDateShouldBe = null;
            var success = descriptionShouldBe == task.Description
                         && dueDateShouldBe == task.DueDate;
            var failureMessage = "Description " + task.Description + " should be " + descriptionShouldBe
                                  + Environment.NewLine
                                  + "Due Date " + task.DueDate + " should be " + dueDateShouldBe;
            Assert.That(success, failureMessage);
        }

    }
}
