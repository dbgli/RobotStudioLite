using System.ComponentModel;

namespace DBGware.RobotStudioLite
{
    public class DominoPosition : INotifyPropertyChanged
    {
        private double x;
        private double y;
        private double z;
        private double r;

        public double X
        {
            get => x;
            set
            {
                if (x == value) return;
                x = value;
                OnPropertyChanged(nameof(X));
            }
        }

        public double Y
        {
            get => y;
            set
            {
                if (y == value) return;
                y = value;
                OnPropertyChanged(nameof(Y));
            }
        }

        public double Z
        {
            get => z;
            set
            {
                if (z == value) return;
                z = value;
                OnPropertyChanged(nameof(Z));
            }
        }

        public double R
        {
            get => r;
            set
            {
                if (r == value) return;
                r = value;
                OnPropertyChanged(nameof(R));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new(propertyName));
        }
    }
}