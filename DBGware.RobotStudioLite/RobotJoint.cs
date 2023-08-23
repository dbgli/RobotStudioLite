using System.ComponentModel;
using System.Windows.Media.Media3D;

namespace DBGware.RobotStudioLite
{
    public class RobotJoint : INotifyPropertyChanged
    {
        public Model3D Link { get; set; }
        public double Angle { get; set; }
        public double MinAngle { get; set; }
        public double MaxAngle { get; set; }
        public Vector3D RotationAxis { get; set; }
        public Point3D RotationPoint { get; set; }
        public Transform3D GlobalTransform
        {
            get => Link.Transform;
            set
            {
                if (Link.Transform == value) return;
                Link.Transform = value;
                OnPropertyChanged(nameof(GlobalTransform));
            }
        }
        public Transform3D LocalTransform => new RotateTransform3D(new AxisAngleRotation3D(RotationAxis, Angle), RotationPoint);

        public event PropertyChangedEventHandler? PropertyChanged;

        public RobotJoint(Model3D link)
        {
            Link = link;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new(propertyName));
        }
    }
}