using System.Collections.ObjectModel;

namespace CustomSchedularControl.Controls.CustomSchedular;

public partial class CustomSchedularControl
{
    List<RowDefinition> _rowDefinitions = new(), _rowDefinitionsWithWhiteSpace = new();
    List<BoxView> _separators = new();
    List<Label> _timingLabels = new();

    ObservableCollection<SchedularItemModel> _schedularItems;

    object _schedularItemManagementLock = new();

    public CustomSchedularControl()
    {
        InitializeComponent();

        _rowDefinitions = new List<RowDefinition>();
        //adding rowdefinitions for space and separators...
        for (int i = 0; i < 24; i++)
        {
            _rowDefinitions.Add(new RowDefinition(new GridLength(1, GridUnitType.Absolute)));
            RowDefinition rowDefinitionWithSpace = new(new GridLength(100, GridUnitType.Absolute));
            _rowDefinitions.Add(rowDefinitionWithSpace);
            _rowDefinitionsWithWhiteSpace.Add(rowDefinitionWithSpace);
        }

        //Adding rows to schedular grid...
        foreach (RowDefinition rowDefinition in _rowDefinitions)
            SchedularGrid.RowDefinitions.Add(rowDefinition);

        _separators.Add(BoxViewVerticalLine);
        //Adding separator line within white space rows...
        for (int i = 0; i < 24; i++)
        {
            BoxView seperator = new();
            Grid.SetRow(seperator, i * 2);
            Grid.SetColumnSpan(seperator, 3);
            _separators.Add(seperator);
            SchedularGrid.Children.Add(seperator);
        }

        //Adding timing label...
        for (int i = 0, j = i + 1; i < 24; i++, j++)
        {
            Label lblTiminigLabel = new() { Text = $"{string.Format("{0:00}", i)}:00" };
            Grid.SetRow(lblTiminigLabel, (j * 2) - 1);
            _timingLabels.Add(lblTiminigLabel);
            SchedularGrid.Children.Add(lblTiminigLabel);
        }

        SetSeperatorColor(Colors.LightGray);
    }

    public static readonly BindableProperty RowHeightPerIntervalProperty = BindableProperty.Create(nameof(RowHeightPerInterval), typeof(double), typeof(CustomSchedularControl), defaultBindingMode: BindingMode.TwoWay,
        propertyChanged: (bindable, oldVal, newVal) =>
        {
            if (bindable is CustomSchedularControl customSchedularControl && newVal is double val)
            {
                try
                {
                    if (val >= 100)
                    {
                        foreach (RowDefinition rowDefinition in customSchedularControl._rowDefinitionsWithWhiteSpace)
                            rowDefinition.Height = new GridLength(val, GridUnitType.Absolute);
                    }
                }
                catch (Exception exc)
                {

                }
            }
        }, defaultValue: 100d);


    public double RowHeightPerInterval
    {
        get
        {
            return (double)GetValue(RowHeightPerIntervalProperty);
        }
        set
        {
            SetValue(RowHeightPerIntervalProperty, value);
        }
    }

    public static readonly BindableProperty SeparatorColorProperty = BindableProperty.Create(nameof(SeparatorColor), typeof(Color), typeof(CustomSchedularControl), defaultValue: Colors.LightGray,
        propertyChanged: (bindable, oldVal, newVal) =>
        {
            try
            {
                if (bindable is CustomSchedularControl customSchedularControl && newVal is Color val)
                {
                    foreach (BoxView separator in customSchedularControl._separators)
                        separator.Color = val;
                }
            }
            catch (Exception exc)
            {

            }
        });

    public Color SeparatorColor
    {
        get
        {
            return (Color)GetValue(SeparatorColorProperty);
        }
        set
        {
            SetValue(SeparatorColorProperty, value);
        }
    }

    private void SetSeperatorColor(Color color)
    {
        foreach (BoxView separator in _separators)
            separator.Color = color;
    }

    public static readonly BindableProperty TimingLabelColorProperty = BindableProperty.Create(nameof(TimingLabelColor), typeof(Color), typeof(CustomSchedularControl), defaultValue: Colors.Black,
        propertyChanged: (bindable, oldVal, newVal) =>
        {
            if (bindable is CustomSchedularControl customSchedularControl && newVal is Color val)
                foreach (Label timingLabel in customSchedularControl._timingLabels)
                    timingLabel.TextColor = val;
        });

    public Color TimingLabelColor
    {
        get => (Color)GetValue(TimingLabelColorProperty);
        set
        {
            SetValue(TimingLabelColorProperty, value);
        }
    }

    public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(ObservableCollection<SchedularItemModel>), typeof(CustomSchedularControl),
        propertyChanged: (bindable, oldVal, newVal) =>
        {
            try
            {
                if (bindable is CustomSchedularControl customSchedularControl && newVal is ObservableCollection<SchedularItemModel> schedularItems)
                {
                    if (customSchedularControl._schedularItems is not null)
                        customSchedularControl._schedularItems.CollectionChanged -= customSchedularControl._schedularItems_CollectionChanged;
                    customSchedularControl._schedularItems = schedularItems;
                    schedularItems.CollectionChanged += customSchedularControl._schedularItems_CollectionChanged;

                    //code for adding schedular content items...
                    lock (customSchedularControl._schedularItemManagementLock)
                    {
                        foreach (SchedularItemModel schedularItem in schedularItems)
                            customSchedularControl.AdjustSchedularItem(schedularItem);
                    }
                }
            }
            catch (Exception exc)
            {

            }
        });

    public ObservableCollection<SchedularItemModel> ItemsSource
    {
        get => (ObservableCollection<SchedularItemModel>)GetValue(ItemsSourceProperty);
        set
        {
            SetValue(ItemsSourceProperty, value);
        }
    }

    private void _schedularItems_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        lock (_schedularItemManagementLock)
        {

        }
    }

    private void AdjustSchedularItem(SchedularItemModel schedularItemModel)
    {
        try
        {
            SchedularContentView schedularContentView = new(schedularItemModel);
            PlacementInfo placementInfo = GetStartAndEndRow(schedularItemModel);
            Grid.SetRow(schedularContentView, placementInfo.StartRowNumber);
            Grid.SetRowSpan(schedularContentView, placementInfo.RowSpan);
            Grid.SetColumn(schedularContentView, 2);
            schedularContentView.Margin = new Thickness(0, placementInfo.TopMargin, 0, placementInfo.BottomMargin);
            SchedularGrid.Children.Add(schedularContentView);
        }
        catch (Exception exc)
        {

        }
    }
    private void RemoveSchedularItem(SchedularItemModel schedularItemModel)
    {
        try
        {

        }
        catch (Exception exc)
        {

        }
    }

    private PlacementInfo GetStartAndEndRow(SchedularItemModel schedularItemModel)
    {
        DateTime startTime = schedularItemModel.StartTime;
        DateTime endTime = schedularItemModel.EndTime;

        int startRow, endRow, rowspan = 1;
        double topMargin, bottomMargin;

        startRow = startTime switch
        {
            { Hour: 0 } => 1,
            { Hour: var h } => h * 2 + 1
        };

        endRow = endTime switch
        {
            { Hour: 0 } => 1,
            { Hour: var h } => h * 2 + 1
        };

        topMargin = (startTime.Minute / 60) * 100;
        bottomMargin = (endTime.Minute / 60) * 100;

        int i = startRow;
        while (i < endRow)
        {
            rowspan++;
            i++;
        }

        if (rowspan > 1)
            rowspan = rowspan + (rowspan - 1);

        return new PlacementInfo
        {
            StartRowNumber = startRow,
            EndRowNumber = endRow,
            TopMargin = topMargin,
            BottomMargin = bottomMargin,
            RowSpan = rowspan
        };
    }
}