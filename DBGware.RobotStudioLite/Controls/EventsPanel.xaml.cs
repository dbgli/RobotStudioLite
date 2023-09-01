using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using ABB.Robotics.Controllers.EventLogDomain;

namespace DBGware.RobotStudioLite.Controls
{
    /// <summary>
    /// Interaction logic for EventsPanel.xaml
    /// </summary>
    public partial class EventsPanel : UserControl
    {
        public ObservableCollection<EventLogMessage> EventLogMessages { get; set; } = new();
        public ICollectionView CollectionView { get; set; }

        public EventsPanel()
        {
            InitializeComponent();
            CollectionView = CollectionViewSource.GetDefaultView(EventLogMessages);
        }

        #region 排序事件日志消息命令

        private void SortEventLogMessagesCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            // 按照属性名称排序
            if (e.Parameter is not string propertyName) return;

            // 先赋值一个初始值，默认升序排列
            ListSortDirection listSortDirection = ListSortDirection.Ascending;

            // 如果之前已经点击排序过，先清空SortDescriptions
            if (CollectionView.SortDescriptions.Count > 0)
            {
                // 如果是同一个属性，那么根据上一次排序的方向决定这次排序的方向，方向相反
                if (CollectionView.SortDescriptions[0].PropertyName == propertyName)
                {
                    listSortDirection = CollectionView.SortDescriptions[0].Direction == ListSortDirection.Ascending ? ListSortDirection.Descending
                                                                                                                    : ListSortDirection.Ascending;
                }

                CollectionView.SortDescriptions.Clear();
            }

            CollectionView.SortDescriptions.Add(new(propertyName, listSortDirection));
        }

        private void SortEventLogMessagesCommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        #endregion

        public void EventLog_MessageWritten(object? sender, MessageWrittenEventArgs e)
        {
            EventLogMessages.Add(e.Message);
        }

        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is not ListViewItem listViewItem) return;
            if (listViewItem.Content is not EventLogMessage eventLogMessage) return;
            EventLogMessageDetailsWindow eventLogMessageDetailsWindow = new(eventLogMessage);
            eventLogMessageDetailsWindow.Show();
        }
    }
}