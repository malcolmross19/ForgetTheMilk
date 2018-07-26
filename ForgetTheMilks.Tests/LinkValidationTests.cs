using NUnit.Framework;
using System;
using ForgetTheMilks.Controllers;

namespace ForgetTheMilks.Tests
{
    [TestFixture()]
    public class LinkValidationTests : AssertionHelper
    {
        [Test()]
        public void Validate_InvalidUrl_ThrowsException()
        {
            var invalidLink = "http://www.doesnotexistdotcom.com";

            Expect(() => new LinkValidator().Validate(invalidLink),
                   Throws.Exception.With.Message.EqualTo("Invalid Link " + invalidLink));
        }

        [Test()]
        public void Validate_ValidUrl_DoesNotThrowException()
        {
            var validLink = "http://www.google.com";

            Expect(() => new LinkValidator().Validate(validLink), Throws.Nothing);
        }
    }
}
