using Blazorise.Animate;
using Microsoft.AspNetCore.Components;

namespace JamesWebUI.Client.Classes
{
    public class SlidingComponent : LayoutComponentBase
    {
        private int currentIndex;

        public IAnimation Direction { get; set; } = Animations.SlideUp;

        public void HandleChangeDirection(int arg)
        {
            if (currentIndex < arg)
            {
                Direction = Animations.SlideUp;
            }
            else
            {
                Direction = Animations.SlideDown;
            }
            currentIndex = arg;
        }
    }
}
