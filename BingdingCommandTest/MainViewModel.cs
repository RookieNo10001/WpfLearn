using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BingdingCommandTest
{
    public class MainViewModel : ObservableObject
    {
        private string? name;
        public string? Name
        {
            get { return name; }
            set { name = value; OnPropertyChanged(); }
        }

        public Mycommand Awakencommand {  get; set; }
        public Mycommand Cancelcommand { get; set; }

        public MainViewModel() 
        {
            Awakencommand = new(awaken);
            Cancelcommand = new(cancel);
            LoadedCommand = new AsyncRelayCommand(Loaded);
        }

        public void awaken()
        {
            Name = "thinks for your awaken";
        }

        public void cancel()
        {
            Name = "";
        }

        string? text;
        public string? Text
        {
            get => text;
            set => SetProperty(ref text, value);
        }

        public AsyncRelayCommand LoadedCommand { get; }

        async Task Loaded()
        {
            await Task.Delay(1000);
            Text = "Hello World";
        }
    }
}
