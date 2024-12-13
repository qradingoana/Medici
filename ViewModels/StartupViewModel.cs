using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace Medici.ViewModels
{
    public class StartupViewModel : INotifyPropertyChanged
    {
        private object _currentView;

        public StartupViewModel()
        {
            // Default view
            CurrentView = new Views.LoginWindow(); // Or set to the default view

            // Initialize commands
            ShowLoginCommand = new RelayCommand(ShowLogin);
            ShowSignupCommand = new RelayCommand(ShowSignup);
        }

        // Property to hold the current view
        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged(nameof(CurrentView));
            }
        }

        // Commands to switch views
        public ICommand ShowLoginCommand { get; }
        public ICommand ShowSignupCommand { get; }

        // Method to show a warning message box every time a view switch is attempted (except when already on that view)
        private bool ConfirmSwitch()
        {
            MessageBoxResult result = MessageBox.Show(
                "Any unsaved changes will be lost. Continue?",
                "Warning",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning
            );

            // If the user clicks "Yes," allow switching the view
            return result == MessageBoxResult.Yes;
        }

        // Methods to switch views
        private void ShowLogin(object obj)
        {
            // Check if we are already on the Login view, avoid switching if so
            if (CurrentView is Views.LoginWindow)
                return;

            // Show the warning if trying to switch views
            if (ConfirmSwitch())
            {
                CurrentView = new Views.LoginWindow();
            }
        }

        private void ShowSignup(object obj)
        {
            // Check if we are already on the Signup view, avoid switching if so
            if (CurrentView is Views.SignupWindow)
                return;

            // Show the warning if trying to switch views
            if (ConfirmSwitch())
            {
                CurrentView = new Views.SignupWindow();
            }
        }

        // INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (!Equals(field, newValue))
            {
                field = newValue;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                return true;
            }

            return false;
        }

        private System.Collections.IEnumerable lineItems;

        public System.Collections.IEnumerable LineItems { get => lineItems; set => SetProperty(ref lineItems, value); }
    }
}
