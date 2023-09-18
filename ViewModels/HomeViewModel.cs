using CustomSchedularControl.Controls.CustomSchedular;
using CustomSchedularControl.Views;
using Mopups.Services;
using System.Collections.ObjectModel;

namespace CustomSchedularControl.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        private double _rowHeight;
        public double RowHeight
        {
            get => _rowHeight;
            set
            {
                SetField(ref _rowHeight, value);
            }
        }

        public ObservableCollection<double> RowHeightItems { get; } = new() { 100, 200, 300 };

        public ObservableCollection<SchedularItemModel> SchedularItems { get; } = new();

        public Command AddCommand { get; }
        public Command RemoveCommand { get; }

        //public Command AddItemCommand { get; }
        public HomeViewModel()
        {
            SchedularItems.Add(new SchedularItemModel
            {
                CardLineColor = Colors.Blue,
                StartTime = new DateTime(2023, 9, 10, 1, 0, 0),
                EndTime = new DateTime(2023, 9, 10, 2, 0, 0),
                Title = "Title 1",
                Description = "Lorem Ipsum Dolor sit amet, Lorem Ipsum dolor sit amet..."
            });
            SchedularItems.Add(new SchedularItemModel
            {
                CardLineColor = Colors.Red,
                StartTime = new DateTime(2023, 9, 10, 2, 30, 0),
                EndTime = new DateTime(2023, 9, 10, 2, 31, 0),
                Title = "Title 2",
                Description = "Lorem Ipsum Dolor sit amet, Lorem Ipsum dolor sit amet..."
            });
            SchedularItems.Add(new SchedularItemModel
            {
                CardLineColor = Colors.Brown,
                StartTime = new DateTime(2023, 9, 10, 3, 0, 0),
                EndTime = new DateTime(2023, 9, 10, 4, 25, 0),
                Title = "Title 3",
                Description = "Lorem Ipsum Dolor sit amet, Lorem Ipsum dolor sit amet..."
            });
            SchedularItems.Add(new SchedularItemModel
            {
                CardLineColor = Colors.Yellow,
                StartTime = new DateTime(2023, 9, 10, 6, 0, 0),
                EndTime = new DateTime(2023, 9, 10, 7, 10, 0),
                Title = "Title 4",
                Description = "Lorem Ipsum Dolor sit amet, Lorem Ipsum dolor sit amet..."
            });
            SchedularItems.Add(new SchedularItemModel
            {
                CardLineColor = Colors.HotPink,
                StartTime = new DateTime(2023, 9, 10, 6, 10, 0),
                EndTime = new DateTime(2023, 9, 10, 6, 55, 0),
                Title = "Title 5",
                Description = "Lorem Ipsum Dolor sit amet, Lorem Ipsum dolor sit amet..."
            });
            SchedularItems.Add(new SchedularItemModel
            {
                CardLineColor = Colors.Coral,
                StartTime = new DateTime(2023, 9, 10, 15, 55, 0),
                EndTime = new DateTime(2023, 9, 10, 16, 55, 0),
                Title = "Title 6",
                Description = "Lorem Ipsum Dolor sit amet, Lorem Ipsum dolor sit amet..."
            });
            SchedularItems.Add(new SchedularItemModel
            {
                CardLineColor = Colors.Khaki,
                StartTime = new DateTime(2023, 9, 10, 6, 59, 0),
                EndTime = new DateTime(2023, 9, 10, 7, 25, 0),
                Title = "Title 7",
                Description = "Lorem Ipsum Dolor sit amet, Lorem Ipsum dolor sit amet..."
            });
            SchedularItems.Add(new SchedularItemModel
            {
                CardLineColor = Colors.Black,
                StartTime = new DateTime(2023, 9, 10, 6, 57, 0),
                EndTime = new DateTime(2023, 9, 10, 7, 10, 0),
                Title = "Title 8",
                Description = "Lorem Ipsum Dolor sit amet, Lorem Ipsum dolor sit amet..."
            });
            //AddItemCommand = new Command(() => 
            //{
            //    SchedularItems.Add(new());
            //});
            RowHeight = RowHeightItems.FirstOrDefault();
            AddCommand = new(AddCommandMethod);
            RemoveCommand = new(RemoveCommandMethod);
        }

        private void AddCommandMethod()
        {
            try
            {
                MopupService.Instance.PushAsync(new AddSchedularItemPage()
                {
                    BindingContext = new AddSchedularItemViewModel
                    {
                        HomeViewModel = this
                    }
                });
            }
            catch (Exception exc)
            {

            }
        }
        private void RemoveCommandMethod()
        {
            try
            {
                SchedularItems.Remove(SchedularItems.First());
            }
            catch (Exception exc)
            {

            }
        }
        public void AddSchedularItem(SchedularItemModel model)
        {
            SchedularItems.Add(model);
        }
    }
}