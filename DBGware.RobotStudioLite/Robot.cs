using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using ABB.Robotics.Controllers;
using HelixToolkit.Wpf;

namespace DBGware.RobotStudioLite
{
    public class Robot
    {
        public Controller? Controller { get; set; } = null;
        public List<Joint> Joints { get; set; } = new();

        public Robot()
        {
        }

        public Point3D CalculateForwardKinematics(List<double> angles)
        {
            for (int i = 0; i < Joints.Count; i++)
            {
                Joints[i].Angle = angles[i];
            }

            Joints[0].GlobalTransform = Joints[0].LocalTransform;
            Joints[1].GlobalTransform = Transform3DHelper.CombineTransform(Joints[1].LocalTransform,
                                                                           Joints[0].GlobalTransform);
            Joints[2].GlobalTransform = Transform3DHelper.CombineTransform(Joints[2].LocalTransform,
                                                                           Joints[1].GlobalTransform);
            Joints[3].GlobalTransform = Transform3DHelper.CombineTransform(Joints[3].LocalTransform,
                                                                           Joints[2].GlobalTransform);
            Joints[4].GlobalTransform = Transform3DHelper.CombineTransform(Joints[4].LocalTransform,
                                                                           Joints[3].GlobalTransform);
            Joints[5].GlobalTransform = Transform3DHelper.CombineTransform(Joints[5].LocalTransform,
                                                                           Joints[4].GlobalTransform);

            return Joints[5].Link.Bounds.Location;
        }

        public List<double> CalculateInverseKinematics(Point3D target)
        {
            // TODO
            return new();
        }
    }
}