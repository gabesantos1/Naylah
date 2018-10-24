﻿namespace Naylah.App.UI.UX
{
    public class HapticFeedback
    {
        public static IHapticFeedback Instance { get; set; }

        static HapticFeedback()
        {
            Instance = new DefaultHapticFeedback();
        }
    }

    internal class DefaultHapticFeedback : IHapticFeedback
    {
        public void Run(HapticFeedbackType hapticFeedbackType)
        {
            throw new System.Exception("Not initialized in device platforms isbrubles");
        }
    }

    public interface IHapticFeedback
    {
        void Run(HapticFeedbackType hapticFeedbackType);
    }

    public enum HapticFeedbackType
    {
        Softy,
        Medium,
        Heavy
    }
}
