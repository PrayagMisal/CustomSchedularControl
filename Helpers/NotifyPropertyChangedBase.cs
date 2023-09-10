using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CustomSchedularControl.Helpers
{
    public class NotifyPropertyChangedBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            try
            {
                if (EqualityComparer<T>.Default.Equals(field, value)) return false;
                field = value;
                MainThread.BeginInvokeOnMainThread(() => { OnPropertyChanged(propertyName); });
                return true;
            }
            catch (Exception ex)
            {
            }
            return false;
        }
    }
}