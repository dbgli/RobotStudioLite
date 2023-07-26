using System.Timers;
using System.Collections.Generic;
using System.Windows.Media.Media3D;
using ABB.Robotics.Controllers;
using ABB.Robotics.Controllers.RapidDomain;
using ABB.Robotics.Controllers.MotionDomain;
using HelixToolkit.Wpf;

namespace DBGware.RobotStudioLite
{
    public class Robot
    {
        private Timer timer = new();

        public Controller? Controller { get; set; } = null;
        public Mastership? Mastership { get; set; } = null;
        public List<Joint> Joints { get; set; } = new();
        public JointTarget CurrentJointTarget { get; set; }
        public RobTarget CurrentRobTarget { get; set; }
        public bool IsRobotJointAnglesSynced { get => timer.Enabled; set => timer.Enabled = value; }

        public Robot()
        {
            timer.Elapsed += SyncRobotPosition;
        }

        private void SyncRobotPosition(object? sender, ElapsedEventArgs e)
        {
            // 不要用DispatcherTimer，因为同在UI线程，耗时操作影响性能
            // 用System.Timers.Timer，在独立线程中触发事件，执行耗时操作
            // 用Dispatcher.Invoke方法处理UI线程中的事务
            if (Controller == null) return;
            CurrentJointTarget = Controller.MotionSystem.ActiveMechanicalUnit.GetPosition();
            CurrentRobTarget = Controller.MotionSystem.ActiveMechanicalUnit.GetPosition(CoordinateSystemType.World);

            List<double> newJointValues = new() { CurrentJointTarget.RobAx.Rax_1,
                                                  CurrentJointTarget.RobAx.Rax_2,
                                                  CurrentJointTarget.RobAx.Rax_3,
                                                  CurrentJointTarget.RobAx.Rax_4,
                                                  CurrentJointTarget.RobAx.Rax_5,
                                                  CurrentJointTarget.RobAx.Rax_6 };
            CurrentRobTarget.Rot.ToEulerAngles(out double rx, out double ry, out double rz);
            List<double> newLinearValues = new() { CurrentRobTarget.Trans.X,
                                                   CurrentRobTarget.Trans.Y,
                                                   CurrentRobTarget.Trans.Z,
                                                   rx,
                                                   ry,
                                                   rz };

            App.Current.Dispatcher.Invoke(() => UpdateRobotJointValues(newJointValues));
            App.Current.Dispatcher.Invoke(() => UpdateRobotLinearValues(newLinearValues));
        }

        private void UpdateRobotJointValues(List<double> newJointValues)
        {
            ((MainWindow)App.Current.MainWindow).robotJointJogPanel.JointValues = newJointValues;

            // TODO
            for (int i = 0; i < Joints.Count; i++)
            {
                Joints[i].Angle = newJointValues[i];
            }

            // 注意变换矩阵的结合顺序，矩阵乘法没有交换律
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
        }

        private void UpdateRobotLinearValues(List<double> newLinearValues)
        {
            ((MainWindow)App.Current.MainWindow).robotLinearJogPanel.LinearValues = newLinearValues;
        }

        public Point3D CalculateForwardKinematics(List<double> angles)
        {
            // TODO
            return new();
        }

        public List<double> CalculateInverseKinematics(Point3D target)
        {
            // TODO
            return new();
        }
    }
}