using JamesWebUI.Client.Shared;
using Radzen;
using Xunit;

namespace JamesWebUI.Client.Test
{
    public class JamesFormFieldTests
    {
        [Fact]
        public void Constructor_Sets_Default_Variant_Text()
        {
            // Act
            var field = new JamesFormField();

            // Assert
            Assert.Equal(Variant.Text, field.Variant);
        }

        [Fact]
        public void Constructor_Sets_Style_Including_Width100()
        {
            // Act
            var field = new JamesFormField();
            field.Style = "width: 100%";

        }

        [Fact]
        public void Constructor_Sets_Default_Style_Width100()
        {
            // Act
            var field = new JamesFormField();

            // Assert
            Assert.Equal("width: 100%", field.Style);
        }
        
        [Fact]
        public void Constructor_Sets_User_Style_WithWidth()
        {
            // Act
            var field = new JamesFormField
            {
                Style = "color:red; width: 75%;"
            };

            var contains = field.Style.Contains("width: 75%");
            // Assert
            Assert.True(contains);
        }
        
        [Fact]
        public void Constructor_Sets_User_Style_WithoutWidth()
        {
            // Act
            var field = new JamesFormField
            {
                Style = "color:red;"
            };

            var contains100 = field.Style.Contains("width: 100%");
            // Assert
            Assert.True(contains100);
        }
    }
}