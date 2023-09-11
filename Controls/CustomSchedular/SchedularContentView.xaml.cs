namespace CustomSchedularControl.Controls.CustomSchedular;

public partial class SchedularContentView
{
    public SchedularContentView(SchedularItemModel schedularItemModel)
    {
        InitializeComponent();
        BoxViewLineColor.Color = schedularItemModel.CardLineColor;
        LblTitle.Text = schedularItemModel.Title;
        LblDescription.Text = schedularItemModel.Description;
        BaseGrid.BackgroundColor = Helper.ReduceAlpha(schedularItemModel.CardLineColor);
        LblDuration.Text = $"{schedularItemModel.StartTime.ToString("HH:mm")} - {schedularItemModel.EndTime.ToString("HH:mm")}";
        PathClockIcon.Fill = new SolidColorBrush(schedularItemModel.CardLineColor);
    }
}