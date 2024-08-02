using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blazorise.Extensions;
using JamesWebUI.Client.Controls.Extensions;

namespace JamesWebUI.Client.Test
{
    public class Extensions
    {
        [Fact]
        public void ToScreenText()
        {
            Assert.Equal("Normal Text", "Normal Text".ToScreenText());
            Assert.Equal("<BLANK>", "".ToScreenText());
            Assert.Equal("<NULL>", ((string?)null).ToScreenText());
        }
    }
}
