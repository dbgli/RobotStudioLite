using System.Timers;
using System.Collections.Generic;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using ABB.Robotics.Controllers;
using ABB.Robotics.Controllers.MotionDomain;
using HelixToolkit.Wpf;

namespace DBGware.RobotStudioLite
{
    public class Robot
    {
        public Controller? Controller { get; set; } = null;
        public Mastership? Mastership { get; set; } = null;
        public RobotStatus? StatusCache { get; set; } = null;
        public Timer StatusCacheRefreshTimer { get; set; } = new();
        public List<RobotJoint> Joints { get; set; } = new();
        public bool IsStatusCacheRefreshing { get; private set; } = false;

        public Robot()
        {
            // 不要用DispatcherTimer，因为同在UI线程，耗时操作影响性能
            // 用System.Timers.Timer，在独立线程中触发事件，执行耗时操作
            StatusCacheRefreshTimer.Elapsed += RefreshStatusCache;
        }

        private void RefreshStatusCache(object? sender, ElapsedEventArgs e)
        {
            // 防止重入
            if (IsStatusCacheRefreshing || Controller == null || StatusCache == null) return;
            IsStatusCacheRefreshing = true;

            // TODO 用事件刷新状态
            StatusCache.Name = Controller.Name;
            StatusCache.SystemId = Controller.SystemId;
            StatusCache.OperatingMode = Controller.OperatingMode;
            StatusCache.IsMaster = Controller.IsMaster;
            StatusCache.State = Controller.State;
            StatusCache.ExecutionStatus = Controller.Rapid.ExecutionStatus;
            StatusCache.JointPosition = Controller.MotionSystem.ActiveMechanicalUnit.GetPosition();
            StatusCache.LinearPosition = Controller.MotionSystem.ActiveMechanicalUnit.GetPosition(CoordinateSystemType.World);

            Joints[0].Angle = StatusCache.JointPosition.RobAx.Rax_1;
            Joints[1].Angle = StatusCache.JointPosition.RobAx.Rax_2;
            Joints[2].Angle = StatusCache.JointPosition.RobAx.Rax_3;
            Joints[3].Angle = StatusCache.JointPosition.RobAx.Rax_4;
            Joints[4].Angle = StatusCache.JointPosition.RobAx.Rax_5;
            Joints[5].Angle = StatusCache.JointPosition.RobAx.Rax_6;

            // 用Dispatcher.Invoke方法处理UI线程中的事务
            App.Current.Dispatcher.Invoke(() =>
            {
                // 在异步操作后，手动刷新命令可用性，注意只有在UI线程中调用才起作用
                CommandManager.InvalidateRequerySuggested();

                ((MainWindow)App.Current.MainWindow).robotJointJogPanel.JointPosition = StatusCache.JointPosition;
                ((MainWindow)App.Current.MainWindow).robotLinearJogPanel.LinearPosition = StatusCache.LinearPosition;

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
            });

            IsStatusCacheRefreshing = false;
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