using AudioEqualizer.Core;
using AudioEqualizer.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioEqualizer.MVVM.ViewModel
{
    class MainViewModel : ObservableObject
    {
        public RelayCommand HomeViewCommand { get; set; }
        public RelayCommand SettingsViewCommand { get; set; }

        public HomeViewModel HomeVM { get; set; }
        public SettingsViewModel SettingsVM { get; set; }

        private object _currentView;
        public object CurrentView
        {
            get { return _currentView; }
            set { _currentView = value; OnPropertyChanged(); }
        }

        private bool _connected = false;
        public bool Connected
        {
            get { return _connected; }
            set { if (value != _connected) { 
                    _connected = value;
                    Visibility = _connected ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible; 
                    OnPropertyChanged("Connected"); 
                } 
            }
        }

        System.Windows.Visibility _visibility = System.Windows.Visibility.Visible;
        public System.Windows.Visibility Visibility
        {
            get { return _visibility; }
            set { if (_visibility != value) { _visibility = value; OnPropertyChanged("Visibility"); } }
        }

        public MainViewModel()
        {
            HomeVM = new HomeViewModel();
            SettingsVM = new SettingsViewModel();

            Messenger.Default.Register<bool>(action => Connected = action);

            CurrentView = HomeVM;

            HomeViewCommand = new RelayCommand(o => 
            {
                CurrentView = HomeVM;
            });

            SettingsViewCommand = new RelayCommand(o => 
            {
                CurrentView = SettingsVM;
            });
        }
    }
}
