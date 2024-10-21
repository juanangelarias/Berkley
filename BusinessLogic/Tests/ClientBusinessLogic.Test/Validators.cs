using ClientBusinessLogic;
using James.Shared.Model;
using SharedBusinessLogic;

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

        [Fact]
        public void ValidateAddressTest()
        {
            Address validAddress = new Address()
            {
                Address1 = "Test",
                Address2 = "Test",
                City = "Test",
                StateCode = "IA",
                PostalCode = "12345"
            };
            Address invalidAddress1 = new Address()
            {
                Address1 = "",
                Address2 = "",
                City = "",
                StateCode = "",
                PostalCode = "12345"
            };

            Assert.True(ClientBusinessLogic.Validators.ValidateAddress(validAddress));
            Assert.False(ClientBusinessLogic.Validators.ValidateAddress(invalidAddress1));
        }

        [Theory]
        [InlineData("_test")]
        [InlineData("t/est")]
        [InlineData("t#est")]
        [InlineData("t%est")]
        [InlineData("te&st")]
        [InlineData("tes{t")]
        [InlineData("t}est")]
        [InlineData("t<est")]
        [InlineData("t>est")]
        [InlineData("te*st")]
        [InlineData("tes?t")]
        [InlineData("t$est")]
        [InlineData("te!st")]
        [InlineData("tes't")]
        [InlineData("t\"est")]
        [InlineData("te@st")]
        [InlineData("tes+t")]
        [InlineData("t|est")]
        [InlineData("te=st")]
        public async Task InvalidFileNameCharacters(string filenameWithoutExtension)
        {
            Assert.False(ClientBusinessLogic.Validators.ValidateFileName(filenameWithoutExtension + ".doc"));
        }
        [Theory]
        [InlineData("test")]
        [InlineData("t\\st")]
        [InlineData("t.est")]
        [InlineData("te_st")]
        [InlineData("tes-t")]
        public async Task ValidFileNameCharacters(string filenameWithoutExtension)
        {
            Assert.True(ClientBusinessLogic.Validators.ValidateFileName(filenameWithoutExtension + ".doc"));
        }


    }
}