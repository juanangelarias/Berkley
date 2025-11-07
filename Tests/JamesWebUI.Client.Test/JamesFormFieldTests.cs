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
        
        [Fact]
        public void Constructor_Sets_User_Style_WithoutWidth_WithMinAndMaxWidth()
        {
            // Act
            var field = new JamesFormField
            {
                Style = "color:red; min-width: 100px; max-width: 200px;"
            };

            var containsMin = field.Style.Contains("min-width: 100px");
            var containsMax = field.Style.Contains("max-width: 200px");
            var containsColor = field.Style.Contains("color:red");
            var containsWidth100 = field.Style.Contains("width: 100%");
            // Assert
            Assert.True(containsMin);
            Assert.True(containsMax);
            Assert.True(containsColor);
            Assert.True(containsWidth100);
        }
        
        [Fact]
        public void Constructor_Sets_User_Style_WithoutWidth_WithMinWidth()
        {
            // Act
            var field = new JamesFormField
            {
                Style = "color:red; min-width: 100px"
            };

            var containsMin = field.Style.Contains("min-width: 100px");
            var containsMax = field.Style.Contains("max-width: 200px");
            var containsColor = field.Style.Contains("color:red");
            var containsWidth100 = field.Style.Contains("width: 100%");
            // Assert
            Assert.True(containsMin);
            Assert.False(containsMax);
            Assert.True(containsColor);
            Assert.True(containsWidth100);
        }
        
        [Fact]
        public void Constructor_Sets_User_Style_WithoutWidth_WithMaxWidth()
        {
            // Act
            var field = new JamesFormField
            {
                Style = "color:red; max-width: 200px;"
            };

            var containsMin = field.Style.Contains("min-width: 100px");
            var containsMax = field.Style.Contains("max-width: 200px");
            var containsColor = field.Style.Contains("color:red");
            var containsWidth100 = field.Style.Contains("width: 100%");
            // Assert
            Assert.False(containsMin);
            Assert.True(containsMax);
            Assert.True(containsColor);
            Assert.True(containsWidth100);
        }
        
        // With
        [Fact]
        public void Constructor_Sets_User_Style_WithWidth_WithMinAndMaxWidth()
        {
            // Act
            var field = new JamesFormField
            {
                Style = "color:red; min-width: 100px; max-width: 200px; width: 75%;"
            };

            var containsMin = field.Style.Contains("min-width: 100px");
            var containsMax = field.Style.Contains("max-width: 200px");
            var containsColor = field.Style.Contains("color:red");
            var containsWidth75 = field.Style.Contains("width: 75%");
            // Assert
            Assert.True(containsMin);
            Assert.True(containsMax);
            Assert.True(containsColor);
            Assert.True(containsWidth75);
        }
        
        [Fact]
        public void Constructor_Sets_User_Style_WithWidth_WithMinWidth()
        {
            // Act
            var field = new JamesFormField
            {
                Style = "color:red; min-width: 100px; width: 75%;"
            };

            var containsMin = field.Style.Contains("min-width: 100px");
            var containsMax = field.Style.Contains("max-width: 200px");
            var containsColor = field.Style.Contains("color:red");
            var containsWidth75 = field.Style.Contains("width: 75%");
            // Assert
            Assert.True(containsMin);
            Assert.False(containsMax);
            Assert.True(containsColor);
            Assert.True(containsWidth75);
        }
        
        [Fact]
        public void Constructor_Sets_User_Style_WithWidth_WithMaxWidth()
        {
            // Act
            var field = new JamesFormField
            {
                Style = "color:red; max-width: 200px; width: 75%;"
            };

            var containsMin = field.Style.Contains("min-width: 100px");
            var containsMax = field.Style.Contains("max-width: 200px");
            var containsColor = field.Style.Contains("color:red");
            var containsWidth75 = field.Style.Contains("width: 75%");
            // Assert
            Assert.False(containsMin);
            Assert.True(containsMax);
            Assert.True(containsColor);
            Assert.True(containsWidth75);
        }
    }
}