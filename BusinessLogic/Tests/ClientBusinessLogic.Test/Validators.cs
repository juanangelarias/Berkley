using ClientBusinessLogic;

namespace ClientBusinessLogic.Test
{
    public class Validators
    {
        [Fact]
        public void ValidateEmailTest()
        {
            string blankEmail = string.Empty;
            string validEmail = "test@test.com";
            string validEmail2 = "test@test.com.au";
            string invalidEmail = "Test.com";
            string invalidEmail2 = "test@test@com";
            string invalidEmail3 = "test@test.com@test";

            Assert.True(ClientBusinessLogic.Validators.ValidateEmail(validEmail));
            Assert.True(ClientBusinessLogic.Validators.ValidateEmail(validEmail2));
            Assert.False(ClientBusinessLogic.Validators.ValidateEmail(blankEmail));
            Assert.False(ClientBusinessLogic.Validators.ValidateEmail(invalidEmail));
            Assert.False(ClientBusinessLogic.Validators.ValidateEmail(invalidEmail2));
            Assert.False(ClientBusinessLogic.Validators.ValidateEmail(invalidEmail3));

        }
    }
}