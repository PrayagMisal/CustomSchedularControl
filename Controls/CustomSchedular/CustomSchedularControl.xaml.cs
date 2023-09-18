using Microsoft.Maui.Controls.Shapes;
using System.Collections.ObjectModel;
using Timer = System.Timers.Timer;

namespace CustomSchedularControl.Controls.CustomSchedular;

public partial class CustomSchedularControl
{
    List<RowDefinition> _rowDefinitions = new(), _rowDefinitionsWithWhiteSpace = new();
    List<BoxView> _separators = new();
    List<Label> _timingLabels = new();
    List<Frame> _schedularItemsFrame = new();

    Line _currentTimeDashedLine = new()
    {
        Stroke = new SolidColorBrush(Color.FromArgb("#b3ff0000")),
        StrokeDashArray = new DoubleCollection(new double[] { 4, 4 }),
        StrokeDashOffset = 1,
        StrokeThickness = 1.5,
        ZIndex = 2
    };

    bool _firstTimeScrollingDone = false;
    Timer _timer = new(60000);

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

        _timer.Elapsed += _timer_Elapsed;
    }

    private void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
        MainThread.InvokeOnMainThreadAsync(() =>
        {
            AddCurrentTimeLine();
        });
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
            if (e.NewItems is not null and { Count: > 0 })
            {
                foreach (SchedularItemModel schedularItemToAdd in e.NewItems)
                    AdjustSchedularItem(schedularItemToAdd);
            }
            if (e.OldItems is not null and { Count: > 0 })
            {
                foreach (SchedularItemModel schedularItemToRemove in e.OldItems)
                    RemoveSchedularItem(schedularItemToRemove);
            }
        }
    }

    private void AdjustSchedularItem(SchedularItemModel schedularItemModel)
    {
        try
        {
            //EndTime should be greater then start time...
            if (schedularItemModel.EndTime <= schedularItemModel.StartTime)
                return;

            PlacementInfo placementInfo = GetPlacementInfo(schedularItemModel);
            Frame schedularContentView = placementInfo.HeightOfCard < 70 ? new SchedularContentViewForLessDuration(schedularItemModel)
                : new SchedularContentView(schedularItemModel);
            Grid.SetRow(schedularContentView, placementInfo.StartRowNumber);
            Grid.SetRowSpan(schedularContentView, schedularContentView is SchedularContentView ? placementInfo.RowSpan : 2);
            Grid.SetColumn(schedularContentView, 2);
            schedularContentView.Margin = new Thickness(placementInfo.LeftMargin, placementInfo.TopMargin, 0, schedularContentView is SchedularContentView ? placementInfo.BottomMargin : 0);
            SchedularGrid.Children.Add(schedularContentView);
            //Right now we are not using mvvm pattern in view but, it's for finding the item...
            schedularContentView.BindingContext = schedularItemModel;
            _schedularItemsFrame.Add(schedularContentView);
        }
        catch (Exception exc)
        {

        }
    }
    private void RemoveSchedularItem(SchedularItemModel schedularItemModel)
    {
        try
        {
            Frame itemToRemove = _schedularItemsFrame.FirstOrDefault(s => s.BindingContext == schedularItemModel);
            if (itemToRemove is not null)
            {
                SchedularGrid.Children.Remove(itemToRemove);
                _schedularItemsFrame.Remove(itemToRemove);
            }
        }
        catch (Exception exc)
        {

        }
    }

    private void AddCurrentTimeLine()
    {
        try
        {
            TimeOnly currentTime = TimeOnly.FromDateTime(DateTime.Now);
            int startRow = currentTime switch
            {
                { Hour: 0 } => 1,
                { Hour: var h } => h * 2 + 1
            };
            double topPercentage = ((double)currentTime.Minute / 60) * 100;
            double topMargin = (topPercentage * RowHeightPerInterval) / 100;

            _currentTimeDashedLine.X1 = 0;
            _currentTimeDashedLine.X1 = 500;
            _currentTimeDashedLine.Y1 = topMargin;
            _currentTimeDashedLine.Y2 = topMargin;

            Grid.SetRow(_currentTimeDashedLine, startRow);
            Grid.SetColumnSpan(_currentTimeDashedLine, 3);
            if (SchedularGrid.Children.Contains(_currentTimeDashedLine))
                SchedularGrid.Children.Remove(_currentTimeDashedLine);
            SchedularGrid.Children.Add(_currentTimeDashedLine);

            if (_firstTimeScrollingDone == false)
            {
                _firstTimeScrollingDone = true;
                Task.Run(async () =>
                {
                    await Task.Delay(1000);
                    await MainThread.InvokeOnMainThreadAsync(async () =>
                    {
                        await BaseScrollView.ScrollToAsync(_currentTimeDashedLine, ScrollToPosition.Center, true);
                    });
                });
            }
        }
        catch (Exception exc)
        {

        }
    }

    private PlacementInfo GetPlacementInfo(SchedularItemModel schedularItemModel)
    {
        DateTime startTime = schedularItemModel.StartTime;
        DateTime endTime = schedularItemModel.EndTime;

        int startRow, endRow, rowspan = 1;
        double topMargin, bottomMargin, heightOfCard, leftMargin = 0;

        startRow = startTime switch
        {
            { Hour: 0 } => 1,
            { Hour: var h } => h * 2 + 1
        };

        endRow = endTime switch
        {
            { Hour: 0 } => 1,
            { Hour: var h, Minute: var m } when m == 0 => h * 2 - 1,
            { Hour: var h } => h * 2 + 1
        };

        //getting percentage to add appropriate margin to our card
        double topPercentage = ((double)startTime.Minute / 60) * 100;
        double bottomPercentage = ((double)endTime.Minute / 60) * 100;

        bottomPercentage = bottomPercentage > 0 ? 100 - bottomPercentage : bottomPercentage;

        topMargin = (topPercentage * RowHeightPerInterval) / 100;
        bottomMargin = (bottomPercentage * RowHeightPerInterval) / 100;

        //SchedularItemModel itemToBeOverlapped = _schedularItems.FirstOrDefault(s => (startTime >= s.StartTime && startTime <= s.EndTime) || (endTime >= s.StartTime && endTime <= s.EndTime));
        List<SchedularItemModel> schedularItems = _schedularItemsFrame.Select(s => s.BindingContext as SchedularItemModel).ToList();
        SchedularItemModel itemToBeOverlapped = schedularItems.LastOrDefault(s => (startTime >= s.StartTime && startTime <= s.EndTime) || (endTime >= s.StartTime && endTime <= s.EndTime));
        if (itemToBeOverlapped is not null)
        {
            Frame frameToBeOverlapped = _schedularItemsFrame.FirstOrDefault(s => s.BindingContext == itemToBeOverlapped);
            if (frameToBeOverlapped is not null)
                leftMargin = frameToBeOverlapped.Margin.Left + 10;
        }

        //Getting how many rows it will cover...
        int i = startRow;
        while (i < endRow)
        {
            rowspan++;
            i++;
        }

        int actualRowspanForConsideration = rowspan > 1 ? (rowspan - (rowspan / 2)) : rowspan;
        //Calculating the height of card...
        heightOfCard = (RowHeightPerInterval * actualRowspanForConsideration) - (topMargin + bottomMargin);

        //if (rowspan > 1)
        //    rowspan = rowspan + (rowspan - 1);

        return new PlacementInfo
        {
            StartRowNumber = startRow,
            EndRowNumber = endRow,
            TopMargin = topMargin,
            BottomMargin = bottomMargin,
            RowSpan = rowspan,
            HeightOfCard = heightOfCard,
            LeftMargin = leftMargin
        };
    }

    public void StartUpdatingCurrentTimeDashedLine()
    {
        AddCurrentTimeLine();
        _timer.Start();
    }
    public void StopUpdatingCurrentTimeDashedLine()
    {
        _timer.Stop();
    }
}