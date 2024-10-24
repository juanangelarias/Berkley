using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedBusinessLogic.Test
{
    public class ImagingBusinessLogicTests
    {
        public static IEnumerable<object[]> GetAllowedImagingFileTypes()
        {
            foreach (var fileType in ImagingBusinessLogic.AllowedFileTypes)
                yield return new object[]{ fileType};
        }

        [Theory]
        [MemberData(nameof(GetAllowedImagingFileTypes))]
        public async Task ValidImagingFileTypes(string extension)
        {
            Assert.True(ImagingBusinessLogic.IsValidImagingFileName("ValidFileName."+extension));
        }
        [Theory]
        [InlineData("3mf")]
        [InlineData("cs")]
        [InlineData("vb")]
        [InlineData("exe")]
        [InlineData("dll")]
        [InlineData("polarbear")]
        [InlineData(".doubledot")]
        [InlineData("double.ext")]
        [InlineData("a/b")]
        public async Task InvalidImagingFileTypes(string extension)
        {
            Assert.False(ImagingBusinessLogic.IsValidImagingFileName("ValidFileName." + extension));
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
            Assert.False(ImagingBusinessLogic.IsValidImagingFileName(filenameWithoutExtension + ".doc"));
        }
        [Theory]
        [InlineData("test")]
        [InlineData("0test")]
        [InlineData("t\\st")]
        [InlineData("t.est")]
        [InlineData("te_st")]
        [InlineData("tes-t")]
        public async Task ValidFileNameCharacters(string filenameWithoutExtension)
        {
            Assert.True(ImagingBusinessLogic.IsValidImagingFileName(filenameWithoutExtension + ".doc"));
        }
    }
}
