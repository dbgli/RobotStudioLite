using ABB.Robotics.Controllers.EventLogDomain;
using DBGware.RobotStudioLite.UI.Controls;
using System.Xml.Linq;

namespace DBGware.RobotStudioLite
{
    /// <summary>
    /// Interaction logic for EventLogMessageDetailsWindow.xaml
    /// </summary>
    public partial class EventLogMessageDetailsWindow : CustomChromeWindow
    {
        public EventLogMessageDetailsWindow(EventLogMessage eventLogMessage)
        {
            InitializeComponent();
            try
            {
                XDocument xDocument = XDocument.Parse(eventLogMessage.Body);
                titleParagraph.Text = $"{eventLogMessage.CategoryId}{eventLogMessage.Number:D4}: {xDocument.Root?.Element("Title")?.Value}";
                descriptionParagraph.Text = xDocument.Root?.Element("Description")?.Value;
                consequencesParagraph.Text = xDocument.Root?.Element("Consequences")?.Value;
                causesParagraph.Text = xDocument.Root?.Element("Causes")?.Value;
                actionsParagraph.Text = xDocument.Root?.Element("Actions")?.Value;
            }
            catch
            {
                // xml格式错误
            }
        }
    }
}