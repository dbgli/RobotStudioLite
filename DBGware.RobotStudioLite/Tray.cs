using System.Collections.ObjectModel;

namespace DBGware.RobotStudioLite
{
    public class Tray
    {
        private readonly ObservableCollection<int> ids = new();

        public int Rows { get; set; }
        public int Columns { get; set; }
        public double RowSpacing { get; set; }
        public double ColumnSpacing { get; set; }
        public ObservableCollection<int> IDs
        {
            get
            {
                if (ids.Count == Rows * Columns) return ids;
                ids.Clear();
                for (int id = 1; id <= Rows * Columns; id++) ids.Add(id);
                return ids;
            }
        }
    }
}