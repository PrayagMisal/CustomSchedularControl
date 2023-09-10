using CustomSchedularControl.Controls.CustomSchedular;
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
                EndTime = new DateTime(2023, 9, 10, 3, 0, 0),
                Title = "Title 2",
                Description = "Lorem Ipsum Dolor sit amet, Lorem Ipsum dolor sit amet..."
            });
            SchedularItems.Add(new SchedularItemModel
            {
                CardLineColor = Colors.Brown,
                StartTime = new DateTime(2023, 9, 10, 16, 0, 0),
                EndTime = new DateTime(2023, 9, 10, 17, 25, 0),
                Title = "Title 3",
                Description = "Lorem Ipsum Dolor sit amet, Lorem Ipsum dolor sit amet..."
            });
            //AddItemCommand = new Command(() => 
            //{
            //    SchedularItems.Add(new());
            //});
            RowHeight = RowHeightItems.FirstOrDefault();
        }
    }
}