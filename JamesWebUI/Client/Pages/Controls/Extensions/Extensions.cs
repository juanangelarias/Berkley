using Blazorise;
using Microsoft.AspNetCore.Components;

namespace JamesWebUI.Client.Controls.Extensions
{
    /// <summary>
    /// Generic static class for extension methods not worth putting in their own class
    /// </summary>
    public static class Extensions
    {
        public static string Page(this NavigationManager navigation)
        {
            return navigation.Uri.Substring(navigation.BaseUri.Length - 1);
        }

        public static string ToScreenText(this string? text) => text switch
        {
            null => "<NULL>",
            "" => "<BLANK>",
            _ => text
        };

        /// <summary>
        /// Displays human-readable text to display a number of bytes.  (5B, 4.3KB, etc.)
        /// </summary>
        /// <param name="bytes">number of bytes.  Should not be negative</param>
        /// <returns>Human readable text</returns>
        public static string ToScreenBytes(this long bytes)
        {
            //bytes should never be negative
            ArgumentOutOfRangeException.ThrowIfNegative(bytes, nameof(bytes));

            //long data type maxes out at about 9EB
            string[] abbreviations = ["B", "KB", "MB", "GB", "TB", "PB", "EB"];
            long factor = 1024; //This matches Windows file explorer
            decimal number = bytes;
            int magnitude =0;
            while (number >= factor)
            {
                number /= factor;
                magnitude++;
            }

            return magnitude == 0 ? $"{bytes}B" : $"{number:F1}{abbreviations[magnitude]}";
        }
    }
}
