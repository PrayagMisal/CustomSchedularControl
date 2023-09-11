namespace CustomSchedularControl.Controls.CustomSchedular;

public partial class SchedularContentViewForLessDuration
{
	public SchedularContentViewForLessDuration(SchedularItemModel schedularItemModel)
	{
		InitializeComponent();
        BoxViewLineColor.Color = schedularItemModel.CardLineColor;
        LblDescription.Text = schedularItemModel.Description;
        BaseGrid.BackgroundColor = Helper.ReduceAlpha(schedularItemModel.CardLineColor);
        LblDuration.Text = $"{schedularItemModel.StartTime.ToString("HH:mm")} - {schedularItemModel.EndTime.ToString("HH:mm")}";
        PathClockIcon.Fill = new SolidColorBrush(schedularItemModel.CardLineColor);
    }
}