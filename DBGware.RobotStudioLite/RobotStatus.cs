using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ABB.Robotics.Controllers;
using ABB.Robotics.Controllers.RapidDomain;

namespace DBGware.RobotStudioLite
{
    public class RobotStatus
    {
        public string Name { get; set; } = string.Empty;
        public Guid SystemId { get; set; }
        public ControllerOperatingMode OperatingMode { get; set; }
        public bool IsMaster { get; set; }
        public ControllerState State { get; set; }
        public ExecutionStatus ExecutionStatus { get; set; }
        public JointTarget JointPosition { get; set; }
        public RobTarget LinearPosition { get; set; }
    }
}