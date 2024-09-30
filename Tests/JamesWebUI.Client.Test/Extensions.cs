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

        [Fact]
        public void ToScreenBytes1()
        {
            Assert.Equal("1B", 1L.ToScreenBytes());
            Assert.Equal("1.0KB", 1024L.ToScreenBytes());
            Assert.Equal("1.0MB", ((long)Math.Pow(1024L, 2)).ToScreenBytes());
            Assert.Equal("1.0GB", ((long)Math.Pow(1024L, 3)).ToScreenBytes());
            Assert.Equal("1.0TB", ((long)Math.Pow(1024L, 4)).ToScreenBytes());
            Assert.Equal("1.0PB", ((long)Math.Pow(1024L, 5)).ToScreenBytes());
            Assert.Equal("1.0EB", ((long)Math.Pow(1024L, 6)).ToScreenBytes());
        }

        [Fact]
        public void ToScreenBytes2()
        {
            Assert.Equal("100B", 100L.ToScreenBytes());
            Assert.Equal("256.0KB", (256L*1024L).ToScreenBytes());
            Assert.Equal("25.6MB", ((long)(25.6*Math.Pow(1024L, 2))).ToScreenBytes());
            Assert.Equal("25.6GB", ((long)(25.6*Math.Pow(1024L, 3))).ToScreenBytes());
            Assert.Equal("25.6TB", ((long)(25.6*Math.Pow(1024L, 4))).ToScreenBytes());
            Assert.Equal("25.6PB", ((long)(25.6*Math.Pow(1024L, 5))).ToScreenBytes());
            Assert.Equal("2.1EB", ((long)(2.1*Math.Pow(1024L, 6))).ToScreenBytes());
        }

        [Fact]
        public void ToScreenBytes3()
        {
            Assert.Equal("1010B", 1010L.ToScreenBytes());
            Assert.Equal("1010.0KB", (1010L*1024L).ToScreenBytes());
            Assert.Equal("1010.0MB", (1010L*(long)Math.Pow(1024L, 2)).ToScreenBytes());
            Assert.Equal("1010.0GB", (1010L*(long)Math.Pow(1024L, 3)).ToScreenBytes());
            Assert.Equal("1010.0TB", (1010L*(long)Math.Pow(1024L, 4)).ToScreenBytes());
            Assert.Equal("1010.0PB", (1010L*(long)Math.Pow(1024L, 5)).ToScreenBytes());
        }
    }
}
