using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ABB.Robotics.Controllers;

namespace DBGware.RobotStudioLite
{
    public class ControllerInfoWrapper
    {
        public ControllerInfo ControllerInfo { get; }

        public Availability Availability => ControllerInfo.Availability;

        public string ControllerName => ControllerInfo.ControllerName;

        public string Id => ControllerInfo.Id;

        public Level RunLevel => ControllerInfo.RunLevel;

        public Guid SystemId => ControllerInfo.SystemId;

        public string SystemName => ControllerInfo.SystemName;

        public string MacAddress => ControllerInfo.MacAddress;

        public int NetscanId => ControllerInfo.NetscanId;

        public string HostName => ControllerInfo.HostName;

        public DirectoryInfo BaseDirectory => ControllerInfo.BaseDirectory;

        public Version Version => ControllerInfo.Version;

        public string VersionName => ControllerInfo.VersionName;

        public IPAddress IPAddress => ControllerInfo.IPAddress;

        public bool IsVirtual => ControllerInfo.IsVirtual;

        public int WebServicesPort => ControllerInfo.WebServicesPort;

        public int RobApiPort => ControllerInfo.RobApiPort;

        public string DisplayName
        {
            get
            {
                if (IsVirtual || Version.Major < 7)
                {
                    return SystemName;
                }
                return ControllerName;
            }
        }

        public string DisplayLocation
        {
            get
            {
                if (IsVirtual)
                {
                    return Environment.MachineName;
                }
                if (Version.Major < 7 && !string.IsNullOrEmpty(ControllerName))
                {
                    return ControllerName;
                }
                return IPAddress.ToString();
            }
        }

        public ControllerInfoWrapper(ControllerInfo controllerInfo)
        {
            ControllerInfo = controllerInfo;
        }

        public override string ToString()
        {
            return $"Availability: {Availability}\n" +
                   $"ControllerName: {ControllerName}\n" +
                   $"Id: {Id}\n" +
                   $"RunLevel: {RunLevel}\n" +
                   $"SystemId: {SystemId}\n" +
                   $"SystemName: {SystemName}\n" +
                   $"MacAddress: {MacAddress}\n" +
                   $"NetscanId: {NetscanId}\n" +
                   $"HostName: {HostName}\n" +
                   $"BaseDirectory: {BaseDirectory}\n" +
                   $"Version: {Version}\n" +
                   $"VersionName: {VersionName}\n" +
                   $"IPAddress: {IPAddress}\n" +
                   $"IsVirtual: {IsVirtual}\n" +
                   $"WebServicesPort: {WebServicesPort}\n" +
                   $"RobApiPort: {RobApiPort}";
        }
    }
}