using System;

namespace DBGware.RobotStudioLite.Utilities
{
    public enum CategoryTypeEx
    {
        /// <summary>
        /// 公用。
        /// </summary>
        /// <remarks>除了内部的所有日志事件。</remarks>
        Common = 0,

        /// <summary>
        /// 操作。
        /// </summary>
        /// <remarks>操作事件。</remarks>
        Operational = 1,

        /// <summary>
        /// 系统。
        /// </summary>
        /// <remarks>系统事件。</remarks>
        System = 2,

        /// <summary>
        /// 硬件。
        /// </summary>
        /// <remarks>硬件事件。</remarks>
        Hardware = 3,

        /// <summary>
        /// 程序。
        /// </summary>
        /// <remarks>程序事件。</remarks>
        Program = 4,

        /// <summary>
        /// 动作。
        /// </summary>
        /// <remarks>动作事件。</remarks>
        Motion = 5,

        /// <summary>
        /// 操作者。
        /// </summary>
        /// <remarks>操作者事件。</remarks>
        Operator = 6,

        /// <summary>
        /// IO与通信。
        /// </summary>
        /// <remarks>IO与通信事件。</remarks>
        IOCommunication = 7,

        /// <summary>
        /// 用户。
        /// </summary>
        /// <remarks>用户定义的事件。</remarks>
        User = 8,

        /// <summary>
        /// 安全。
        /// </summary>
        /// <remarks>安全事件。</remarks>
        Safety = 9,

        /// <summary>
        /// 内部。
        /// </summary>
        /// <remarks>可选的生产者事件。这些事件已过时。</remarks>
        Internal = 10,

        /// <summary>
        /// 过程。
        /// </summary>
        /// <remarks>过程事件。</remarks>
        Process = 11,

        /// <summary>
        /// 配置。
        /// </summary>
        /// <remarks>配置事件。</remarks>
        Configuration = 12,

        /// <summary>
        /// 点焊。
        /// </summary>
        /// <remarks>点焊事件。此类别已不再使用。</remarks>
        [Obsolete("Category not in use any more", true)]
        SpotWeld = 12,

        /// <summary>
        /// 喷涂。
        /// </summary>
        /// <remarks>喷涂事件。此类别已不再使用。</remarks>
        [Obsolete("Category not in use any more", true)]
        Paint = 13,

        /// <summary>
        /// 拾取器。
        /// </summary>
        /// <remarks>拾取器事件。此类别已不再使用。</remarks>
        [Obsolete("Category not in use any more", true)]
        Picker = 14
    }
}