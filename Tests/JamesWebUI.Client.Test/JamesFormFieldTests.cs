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
                Style = "color:red; Width: 75%;"
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
                Style = "color:red; min-width: 100px; MAX-width: 200px;"
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

        [Fact]
        public void Constructor_Sets_User_Style_WithOtherWidthPropertiesNoWidth()
        {
            // Act
            var field = new JamesFormField
            {
                Style = "color:red;"+ WidthProperties()
            };
            
            var contains001 = field.Style.Contains("border-block-end-width:10px;");
            var contains002 = field.Style.Contains("border-block-start-width:10px;");
            var contains003 = field.Style.Contains("border-block-width:10px;");
            var contains004 = field.Style.Contains("border-bottom-width:10px;");
            var contains005 = field.Style.Contains("border-image-width:10px;");
            var contains006 = field.Style.Contains("border-inline-end-width:10px;");
            var contains007 = field.Style.Contains("border-inline-start-width:10px;");
            var contains008 = field.Style.Contains("border-inline-width:10px;");
            var contains009 = field.Style.Contains("border-left-width:10px;");
            var contains010 = field.Style.Contains("border-right-width:10px;");
            var contains011 = field.Style.Contains("border-top-width:10px;");
            var contains012 = field.Style.Contains("border-width:10px;");
            var contains013 = field.Style.Contains("column-rule-width:10px;");
            var contains014 = field.Style.Contains("column-width:10px;");
            var contains015 = field.Style.Contains("contain-intrinsic-width:10px;");
            var contains016 = field.Style.Contains("font-width:10px;");
            var contains017 = field.Style.Contains("mask-border-width:10px;");
            var contains018 = field.Style.Contains("max-width:10px;");
            var contains019 = field.Style.Contains("min-width:10px;");
            var contains020 = field.Style.Contains("outline-width:10px;");
            var contains021 = field.Style.Contains("row-rule-width:10px;");
            var contains022 = field.Style.Contains("rule-width:10px;");
            var contains023 = field.Style.Contains("scrollbar-width:10px;");
            var contains024 = field.Style.Contains("stroke-width:10px;");
            var contains025 = field.Style.Contains("width: 100%");

            Assert.True(contains001);
            Assert.True(contains002);
            Assert.True(contains003);
            Assert.True(contains004);
            Assert.True(contains005);
            Assert.True(contains006);
            Assert.True(contains007);
            Assert.True(contains008);
            Assert.True(contains009);
            Assert.True(contains010);
            Assert.True(contains011);
            Assert.True(contains012);
            Assert.True(contains013);
            Assert.True(contains014);
            Assert.True(contains015);
            Assert.True(contains016);
            Assert.True(contains017);
            Assert.True(contains018);
            Assert.True(contains019);
            Assert.True(contains020);
            Assert.True(contains021);
            Assert.True(contains022);
            Assert.True(contains023);
            Assert.True(contains024);
            Assert.True(contains025);
        }
        
        [Fact]
        public void Constructor_Sets_User_Style_WithAllWidthProperties()
        {
            // Act
            var field = new JamesFormField
            {
                Style = "color:red;"+ WidthProperties()+"width: 80px;"
            };
            
            var contains001 = field.Style.Contains("border-block-end-width:10px;");
            var contains002 = field.Style.Contains("border-block-start-width:10px;");
            var contains003 = field.Style.Contains("border-block-width:10px;");
            var contains004 = field.Style.Contains("border-bottom-width:10px;");
            var contains005 = field.Style.Contains("border-image-width:10px;");
            var contains006 = field.Style.Contains("border-inline-end-width:10px;");
            var contains007 = field.Style.Contains("border-inline-start-width:10px;");
            var contains008 = field.Style.Contains("border-inline-width:10px;");
            var contains009 = field.Style.Contains("border-left-width:10px;");
            var contains010 = field.Style.Contains("border-right-width:10px;");
            var contains011 = field.Style.Contains("border-top-width:10px;");
            var contains012 = field.Style.Contains("border-width:10px;");
            var contains013 = field.Style.Contains("column-rule-width:10px;");
            var contains014 = field.Style.Contains("column-width:10px;");
            var contains015 = field.Style.Contains("contain-intrinsic-width:10px;");
            var contains016 = field.Style.Contains("font-width:10px;");
            var contains017 = field.Style.Contains("mask-border-width:10px;");
            var contains018 = field.Style.Contains("max-width:10px;");
            var contains019 = field.Style.Contains("min-width:10px;");
            var contains020 = field.Style.Contains("outline-width:10px;");
            var contains021 = field.Style.Contains("row-rule-width:10px;");
            var contains022 = field.Style.Contains("rule-width:10px;");
            var contains023 = field.Style.Contains("scrollbar-width:10px;");
            var contains024 = field.Style.Contains("stroke-width:10px;");
            var contains025 = field.Style.Contains("width: 80px");

            Assert.True(contains001);
            Assert.True(contains002);
            Assert.True(contains003);
            Assert.True(contains004);
            Assert.True(contains005);
            Assert.True(contains006);
            Assert.True(contains007);
            Assert.True(contains008);
            Assert.True(contains009);
            Assert.True(contains010);
            Assert.True(contains011);
            Assert.True(contains012);
            Assert.True(contains013);
            Assert.True(contains014);
            Assert.True(contains015);
            Assert.True(contains016);
            Assert.True(contains017);
            Assert.True(contains018);
            Assert.True(contains019);
            Assert.True(contains020);
            Assert.True(contains021);
            Assert.True(contains022);
            Assert.True(contains023);
            Assert.True(contains024);
            Assert.True(contains025);
        }

        private string WidthProperties()
        {
            return "border-block-end-width:10px;border-block-start-width:10px;border-block-width:10px;" +
                   "border-bottom-width:10px;border-image-width:10px;border-inline-end-width:10px;" +
                   "border-inline-start-width:10px;border-inline-width:10px;border-left-width:10px;" +
                   "border-right-width:10px;border-top-width:10px;border-width:10px;column-rule-width:10px;" +
                   "column-width:10px;contain-intrinsic-width:10px;font-width:10px;mask-border-width:10px;" +
                   "max-width:10px;min-width:10px;outline-width:10px;row-rule-width:10px;rule-width:10px;" +
                   "scrollbar-width:10px;stroke-width:10px;";
        }
        
    }
}