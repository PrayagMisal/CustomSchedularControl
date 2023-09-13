using CustomSchedularControl.Controls.CustomSchedular;
using Mopups.Services;

namespace CustomSchedularControl.ViewModels
{
    public class AddSchedularItemViewModel : BaseViewModel
    {
        private string _title;
        public string Title
        {
            get => _title;
            set
            {
                SetField(ref _title, value);
            }
        }

        private string _description;
        public string Description
        {
            get => _description;
            set
            {
                SetField(ref _description, value);
            }
        }

        private TimeSpan _fromTime;
        public TimeSpan FromTime
        {
            get => _fromTime;
            set
            {
                SetField(ref _fromTime, value);
            }
        }

        private TimeSpan _toTime;
        public TimeSpan ToTime
        {
            get => _toTime;
            set
            {
                SetField(ref _toTime, value);
            }
        }

        private string _colorInHex;
        public string ColorInHex
        {
            get => _colorInHex;
            set
            {
                SetField(ref _colorInHex, value);
            }
        }

        public Command CancelCommand { get; }
        public Command AddCommand { get; }

        HomeViewModel _homeViewModel;
        public HomeViewModel HomeViewModel
        {
            private get => _homeViewModel;
            set
            {
                _homeViewModel = value;
            }
        }

        public AddSchedularItemViewModel()
        {
            CancelCommand = new Command(CancelCommandMethod);
            AddCommand = new Command(AddCommandMethod);
        }

        private void CancelCommandMethod()
        {
            MopupService.Instance.PopAsync();
        }
        private async void AddCommandMethod()
        {
            if (string.IsNullOrEmpty(Title) || string.IsNullOrWhiteSpace(Title))
            {
                await App.Current.MainPage.DisplayAlert("Required", "Please enter title.", "Ok");
                return;
            }
            if (string.IsNullOrEmpty(Description) || string.IsNullOrWhiteSpace(Title))
            {
                await App.Current.MainPage.DisplayAlert("Required", "Please type description.", "Ok");
                return;
            }
            if (FromTime >= ToTime)
            {
                await App.Current.MainPage.DisplayAlert("Invalid", "Please select valid value for FromTime and ToTime, FromTime should be less then ToTime.", "Ok");
                return;
            }
            if (string.IsNullOrEmpty(ColorInHex) || string.IsNullOrWhiteSpace(ColorInHex))
            {
                await App.Current.MainPage.DisplayAlert("Required", "Please type valid color in hex.", "Ok");
                return;
            }

            SchedularItemModel schedularItemModel = new()
            {
                Title = Title,
                Description = Description,
                StartTime = DateTime.Now.Date.Add(FromTime),
                EndTime = DateTime.Now.Date.Add(ToTime),
                CardLineColor = Color.FromArgb(ColorInHex)
            };

            HomeViewModel.AddSchedularItem(schedularItemModel);
            await MopupService.Instance.PopAsync();
        }
    }
}