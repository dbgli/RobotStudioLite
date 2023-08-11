using System.ComponentModel;

namespace DBGware.RobotStudioLite
{
    public class DominoSize : INotifyPropertyChanged
    {
        private double length;
        private double width;
        private double height;

        public double Length
        {
            get => length;
            set
            {
                if (length == value) return;
                length = value;
                OnPropertyChanged(nameof(Length));
            }
        }

        public double Width
        {
            get => width;
            set
            {
                if (width == value) return;
                width = value;
                OnPropertyChanged(nameof(Width));
            }
        }

        public double Height
        {
            get => height;
            set
            {
                if (height == value) return;
                height = value;
                OnPropertyChanged(nameof(Height));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new(propertyName));
        }
    }
}