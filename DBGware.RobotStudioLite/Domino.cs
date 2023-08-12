using System.ComponentModel;

namespace DBGware.RobotStudioLite
{
    public class Domino : INotifyPropertyChanged
    {
        private int id = 1;
        private DominoSize size = new();
        private DominoPosition position = new();

        public int ID
        {
            get => id;
            set
            {
                if (id == value) return;
                id = value;
                OnPropertyChanged(nameof(ID));
            }
        }

        public DominoSize Size
        {
            get => size;
            set
            {
                if (size == value) return;
                size = value;
                OnPropertyChanged(nameof(Size));
            }
        }

        public DominoPosition Position
        {
            get => position;
            set
            {
                if (position == value) return;
                position = value;
                OnPropertyChanged(nameof(Position));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new(propertyName));
        }
    }
}