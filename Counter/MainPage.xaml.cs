using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Text.Json;

namespace Counter
{
    public partial class MainPage : ContentPage
    {
        private readonly string FilePath = Path.Combine(FileSystem.AppDataDirectory, "counters.json");


        public ObservableCollection<Counter> Counters { get; set; } = new ObservableCollection<Counter>();

        public MainPage()
        {
            InitializeComponent();
            BindingContext = this;

            LoadCounters();
        }

        private void OnAddCounterClicked(object sender, EventArgs e)
        {
            string name = CounterNameEntry.Text?.Trim();
            if (!string.IsNullOrEmpty(name))
            {
                Counters.Add(new Counter { Name = name, Value = 0 });
                CounterNameEntry.Text = string.Empty;
            }
        }

        private void OnIncreaseClicked(object sender, EventArgs e)
        {
            var counter = (sender as Button)?.CommandParameter as Counter;
            if (counter != null)
            {
                counter.Value++;
            }
        }

        private void OnDecreaseClicked(object sender, EventArgs e)
        {
            var counter = (sender as Button)?.CommandParameter as Counter;
            if (counter != null)
            {
                counter.Value--;
            }
        }

        private void OnResetClicked(object sender, EventArgs e)
        {
            var counter = (sender as Button)?.CommandParameter as Counter;
            if (counter != null)
            {
                counter.Value = 0;
            }
        }

        private void OnRemoveClicked(object sender, EventArgs e)
        {
            var counter = (sender as Button)?.CommandParameter as Counter;
            if (counter != null)
            {
                Counters.Remove(counter);
            }
        }

        private void OnSaveClicked(object sender, EventArgs e)
        {
            var json = JsonSerializer.Serialize(Counters);
            File.WriteAllText(FilePath, json);
        }

        private void LoadCounters()
        {
            if (File.Exists(FilePath))
            {
                var json = File.ReadAllText(FilePath);
                var counters = JsonSerializer.Deserialize<List<Counter>>(json);
                if (counters != null)
                {
                    Counters.Clear();
                    foreach (var counter in counters)
                    {
                        Counters.Add(counter);
                    }
                }
            }
        }
    }

    public class Counter : INotifyPropertyChanged
    {
        private int _value;

        public string Name { get; set; }

        public int Value
        {
            get => _value;
            set
            {
                if (_value != value)
                {
                    _value = value;
                    OnPropertyChanged(nameof(Value));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
