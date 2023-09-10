namespace CustomSchedularControl.Controls.CustomSchedular;

public partial class SchedularContentView
{
    public SchedularContentView(Color lineColor, string title, string description, DateTime fromDuration, DateTime toDuration)
    {
        InitializeComponent();
        BoxViewLineColor.Color = lineColor;
        LblTitle.Text = title;
        LblDescription.Text = description;
    }

    public SchedularContentView(SchedularItemModel schedularItemModel)
    {
        InitializeComponent();
        BoxViewLineColor.Color = schedularItemModel.CardLineColor;
        LblTitle.Text = schedularItemModel.Title;
        LblDescription.Text = schedularItemModel.Description;
    }
}