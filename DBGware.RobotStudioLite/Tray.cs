using System.Collections.ObjectModel;
using System.ComponentModel;
using DBGware.RobotStudioLite.Utilities;

namespace DBGware.RobotStudioLite
{
    public class Tray : INotifyPropertyChanging, INotifyPropertyChanged
    {
        private int rows;
        private int columns;
        private double rowSpacing;
        private double columnSpacing;
        private readonly ObservableCollection<int> ids = new();

        public int Rows
        {
            get => rows;
            set
            {
                if (rows == value) return;
                if (OnPropertyChanging(nameof(Rows), rows, value)) return;
                rows = value;
                OnPropertyChanged(nameof(Rows));
            }
        }

        public int Columns
        {
            get => columns;
            set
            {
                if (columns == value) return;
                if (OnPropertyChanging(nameof(Columns), columns, value)) return;
                columns = value;
                OnPropertyChanged(nameof(Columns));
            }
        }

        public double RowSpacing
        {
            get => rowSpacing;
            set
            {
                if (rowSpacing == value) return;
                rowSpacing = value;
                OnPropertyChanged(nameof(RowSpacing));
            }
        }

        public double ColumnSpacing
        {
            get => columnSpacing;
            set
            {
                if (columnSpacing == value) return;
                columnSpacing = value;
                OnPropertyChanged(nameof(ColumnSpacing));
            }
        }

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

        public event PropertyChangingEventHandler? PropertyChanging;
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual bool OnPropertyChanging<T>(string propertyName, T oldValue, T newValue)
        {
            CancellablePropertyChangingEventArgs<T> cancellablePropertyChangingEventArgs = new(propertyName, oldValue, newValue);
            PropertyChanging?.Invoke(this, cancellablePropertyChangingEventArgs);
            return cancellablePropertyChangingEventArgs.IsCancelled;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new(propertyName));
        }
    }
}