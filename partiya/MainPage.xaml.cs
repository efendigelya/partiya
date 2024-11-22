using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace partiya
{
    public partial class MainPage : ContentPage
    {
        private readonly TodoListViewModel _viewModel;

        public MainPage()
        {
            InitializeComponent();
            //BindingContext = new TodoListViewModel();
            _viewModel = new TodoListViewModel();
            BindingContext = _viewModel;
        }



        private async void OnColorCircleClicked(object sender, EventArgs e)
        {
            string action = await DisplayActionSheet("Выберите метку", "Отмена", null,
                "Срочно и важно", "Не срочно, но важно", "Срочно, но не важно", "Не срочно и не важно");

            switch (action)
            {
                case "Срочно и важно":
                    _viewModel.SelectedColor = Colors.Tomato; // Цвет для "Срочно и важно"
                    break;
                case "Не срочно, но важно":
                    _viewModel.SelectedColor = Colors.Khaki; // Цвет для "Не срочно, но важно"
                    break;
                case "Срочно, но не важно":
                    _viewModel.SelectedColor = Colors.SkyBlue; // Цвет для "Срочно, но не важно"
                    break;
                case "Не срочно и не важно":
                    _viewModel.SelectedColor = Colors.MediumOrchid; // Цвет для "Не срочно и не важно"
                    break;
            }
        }
    }

    public class TodoItem : INotifyPropertyChanged
    {
        private bool _isCompleted;
        public required string Text { get; set; }
        //
        public Color TextColor { get; set; }
        //
        public Color ItemColor { get; set; } = Colors.MediumOrchid; // Цвет заметки
        public DateTime CreatedAt { get; private set; } = DateTime.Now; // Дата и время создания заметки



        public bool IsCompleted
        {
            get => _isCompleted;
            set
            {
                if (_isCompleted != value)
                {
                    _isCompleted = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class TodoListViewModel : INotifyPropertyChanged
    {
        public TodoListViewModel()
        {
            Items = new ObservableCollection<TodoItem>();
            AddCommand = new Command<string>(AddItem, CanAddItem);
            RemoveCommand = new Command<TodoItem>(RemoveItem);
        }

        public ObservableCollection<TodoItem> Items { get; }

        private string _newItemText;
        public string NewItemText
        {
            get => _newItemText;
            set => SetField(ref _newItemText, value);
        }



        private Color _selectedColor = Colors.MediumOrchid;
        public Color SelectedColor
        {
            get => _selectedColor;
            set => SetField(ref _selectedColor, value);
        }



        public ICommand AddCommand { get; }
        private void AddItem(string itemText)
        {
            if (!string.IsNullOrWhiteSpace(itemText))
            {
                Items.Add(new TodoItem
                {
                    Text = itemText,
                    ItemColor = SelectedColor,
                    TextColor = SelectedColor,
                });
                NewItemText = string.Empty;
                // Обновляем все коллекции
                OnPropertyChanged(nameof(ImportantItems));
                OnPropertyChanged(nameof(UrgentItems));
                OnPropertyChanged(nameof(IdeaItems));
                OnPropertyChanged(nameof(NormalItems));
            }
        }
        private bool CanAddItem(string itemText) => !string.IsNullOrWhiteSpace(itemText);


        public ICommand RemoveCommand { get; }
        private void RemoveItem(TodoItem item)
        {
            if (item != null && Items.Contains(item))
            {
                Items.Remove(item);
                // Обновление представлений, хотя они и связываются с общей коллекцией
                OnPropertyChanged(nameof(ImportantItems));
                OnPropertyChanged(nameof(UrgentItems));
                OnPropertyChanged(nameof(IdeaItems));
                OnPropertyChanged(nameof(NormalItems));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }





        public ObservableCollection<TodoItem> ImportantItems =>
            new ObservableCollection<TodoItem>(Items.Where(i => i.ItemColor == Colors.Tomato)); // Срочно и важно

        public ObservableCollection<TodoItem> UrgentItems =>
            new ObservableCollection<TodoItem>(Items.Where(i => i.ItemColor == Colors.Khaki)); // Не срочно, но важно

        public ObservableCollection<TodoItem> IdeaItems =>
            new ObservableCollection<TodoItem>(Items.Where(i => i.ItemColor == Colors.SkyBlue)); // Срочно, но не важно

        public ObservableCollection<TodoItem> NormalItems =>
            new ObservableCollection<TodoItem>(Items.Where(i => i.ItemColor == Colors.MediumOrchid)); // Не срочно и не важно




        
    }
}
