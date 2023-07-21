using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace DBGware.RobotStudioLite
{
    public class Joint
    {
        public Model3D Link { get; set; }
        public double Angle { get; set; }
        public double MinAngle { get; set; }
        public double MaxAngle { get; set; }
        public Vector3D RotationAxis { get; set; }
        public Point3D RotationPoint { get; set; }
        public Transform3D GlobalTransform { get { return Link.Transform; } set { Link.Transform = value; } }
        public Transform3D LocalTransform => new RotateTransform3D(new AxisAngleRotation3D(RotationAxis, Angle), RotationPoint);

        public Joint(Model3D link)
        {
            Link = link;
        }
    }
}