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

            var contains75 = field.Style.Contains("width: 75%");
            var notContains100 = !field.Style.Contains("width: 100%");
            // Assert
            Assert.True(contains75);
            Assert.True(notContains100);
        }
        
        [Fact]
        public void Constructor_Sets_User_Style_WithoutWidth()
        {
            // Act
            var field = new JamesFormField
            {
                Style = "color: red;"
            };

            var contains100 = field.Style.Contains("width: 100%");
            var containsColor = field.Style.Contains("color: red");
            // Assert
            Assert.True(contains100);
            Assert.True(containsColor);
        }
    }
}