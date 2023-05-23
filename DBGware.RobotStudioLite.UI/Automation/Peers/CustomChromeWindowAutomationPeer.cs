using System.Windows.Automation.Peers;

using DBGware.RobotStudioLite.UI.Controls;

namespace DBGware.RobotStudioLite.UI.Automation.Peers
{
    public class CustomChromeWindowAutomationPeer : WindowAutomationPeer
    {
        public CustomChromeWindowAutomationPeer(CustomChromeWindow owner) : base(owner)
        {
        }

        protected override string GetClassNameCore()
        {
            return "CustomChromeWindow";
        }
    }
}