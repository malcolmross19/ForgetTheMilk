using NUnit.Framework;
using Moq;
using System;
using ForgetTheMilks.Controllers;
namespace ForgetTheMilks.Tests
{
    [TestFixture()]
    public class TaskLinkTests : AssertionHelper
    {
        Mock<ILinkValidator> mockLinkValidator;

        LinkValidator systemUnderTest;

        class IgnoreLinkValidator : ILinkValidator
        {
            public void Validate(string link)
            {

            }
        }

        [Test()]
        public void CreateTask_DescriptionWithALink_SetLink()
        {
            var input = "test http://www.google.com";

            var task = new Task(input, default(DateTime), new IgnoreLinkValidator());

            Expect(task.Link, Is.EqualTo("http://www.google.com"));
        }

        [Test()]
        public void Validate_InvalidUrl_ThrowsException()
        {
            var input = "http://www.doesnotexistdotcom.com";

            Expect(() => new Task(input, default(DateTime), new LinkValidator()),
                  Throws.Exception.With.Message.EqualTo("Invalid Link " + input));
        }

        [Test()]
        public void Validate_ValidLink_SetLink()
        {
            var input = "http://www.google.com";

            mockLinkValidator = new Mock<ILinkValidator>();
            mockLinkValidator.Setup(o => o.Validate(input));

            systemUnderTest = new LinkValidator();

            Expect(() => new Task(input, default(DateTime), systemUnderTest),
                   Throws.Nothing);
        }
    }
}