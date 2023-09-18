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
                StartTime = DateTime.Now.Date.AddHours(1), //new DateTime(2023, 9, 10, 1, 0, 0),
                EndTime = DateTime.Now.Date.AddHours(2), //new DateTime(2023, 9, 10, 2, 0, 0),
                Title = "Title 1",
                Description = "Lorem Ipsum Dolor sit amet, Lorem Ipsum dolor sit amet..."
            });
            SchedularItems.Add(new SchedularItemModel
            {
                CardLineColor = Colors.Red,
                StartTime = DateTime.Now.Date.AddHours(2).AddMinutes(30), //new DateTime(2023, 9, 10, 2, 30, 0),
                EndTime = DateTime.Now.Date.AddHours(2).AddMinutes(31), //new DateTime(2023, 9, 10, 2, 31, 0),
                Title = "Title 2",
                Description = "Lorem Ipsum Dolor sit amet, Lorem Ipsum dolor sit amet..."
            });
            SchedularItems.Add(new SchedularItemModel
            {
                CardLineColor = Colors.Brown,
                StartTime = DateTime.Now.Date.AddHours(3), //new DateTime(2023, 9, 10, 3, 0, 0),
                EndTime = DateTime.Now.Date.AddHours(4).AddMinutes(25), //new DateTime(2023, 9, 10, 4, 25, 0),
                Title = "Title 3",
                Description = "Lorem Ipsum Dolor sit amet, Lorem Ipsum dolor sit amet..."
            });
            SchedularItems.Add(new SchedularItemModel
            {
                CardLineColor = Colors.Yellow,
                StartTime = DateTime.Now.Date.AddHours(6).AddMinutes(0),
                EndTime = DateTime.Now.Date.AddHours(7).AddMinutes(10),
                Title = "Title 4",
                Description = "Lorem Ipsum Dolor sit amet, Lorem Ipsum dolor sit amet..."
            });
            SchedularItems.Add(new SchedularItemModel
            {
                CardLineColor = Colors.HotPink,
                StartTime = DateTime.Now.Date.AddHours(6).AddMinutes(10),
                EndTime = DateTime.Now.Date.AddHours(6).AddMinutes(55),
                Title = "Title 5",
                Description = "Lorem Ipsum Dolor sit amet, Lorem Ipsum dolor sit amet..."
            });
            SchedularItems.Add(new SchedularItemModel
            {
                CardLineColor = Colors.Coral,
                StartTime = DateTime.Now.Date.AddHours(15).AddMinutes(55),
                EndTime = DateTime.Now.Date.AddHours(16).AddMinutes(55),
                Title = "Title 6",
                Description = "Lorem Ipsum Dolor sit amet, Lorem Ipsum dolor sit amet..."
            });
            SchedularItems.Add(new SchedularItemModel
            {
                CardLineColor = Colors.Khaki,
                StartTime = DateTime.Now.Date.AddHours(6).AddMinutes(59),
                EndTime = DateTime.Now.Date.AddHours(7).AddMinutes(25),
                Title = "Title 7",
                Description = "Lorem Ipsum Dolor sit amet, Lorem Ipsum dolor sit amet..."
            });
            SchedularItems.Add(new SchedularItemModel
            {
                CardLineColor = Colors.Black,
                StartTime = DateTime.Now.Date.AddHours(6).AddMinutes(57),
                EndTime = DateTime.Now.Date.AddHours(7).AddMinutes(10),
                Title = "Title 8",
                Description = "Lorem Ipsum Dolor sit amet, Lorem Ipsum dolor sit amet..."
            });
            SchedularItems.Add(new SchedularItemModel
            {
                CardLineColor = Colors.Red,
                StartTime = DateTime.Now.Date.AddHours(7).AddMinutes(0),
                EndTime = DateTime.Now.Date.AddHours(7).AddMinutes(30),
                Title = "Exp 1",
                Description = "Lorem Ipsum Dolor sit amet, Lorem Ipsum dolor sit amet..."
            });
            SchedularItems.Add(new SchedularItemModel
            {
                CardLineColor = Colors.Sienna,
                StartTime = DateTime.Now.Date.AddHours(7).AddMinutes(29),
                EndTime = DateTime.Now.Date.AddHours(8).AddMinutes(0),
                Title = "Exp 2",
                Description = "Lorem Ipsum Dolor sit amet, Lorem Ipsum dolor sit amet..."
            });
            SchedularItems.Add(new SchedularItemModel
            {
                CardLineColor = Colors.DarkOliveGreen,
                StartTime = DateTime.Now.Date.AddHours(6).AddMinutes(56),
                EndTime = DateTime.Now.Date.AddHours(7).AddMinutes(20),
                Title = "Exp 3",
                Description = "Lorem Ipsum Dolor sit amet, Lorem Ipsum dolor sit amet..."
            });
            //SchedularItems.Add(new SchedularItemModel
            //{
            //    CardLineColor = Colors.DarkMagenta,
            //    StartTime = new DateTime(2023, 9, 10, 7, 40, 0),
            //    EndTime = new DateTime(2023, 9, 10, 8, 25, 0),
            //    Title = "Exp 4",
            //    Description = "Lorem Ipsum Dolor sit amet, Lorem Ipsum dolor sit amet..."
            //});
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