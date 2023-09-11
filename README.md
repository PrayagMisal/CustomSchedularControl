# CustomSchedularControl
In my past experience the client had requirement to display task in schedular...So, below is the custom designed schedular in MAUI.

Work done so far...
1) Cards gets arranged as expected for given start time to end time.
2) Red colored dashed line can be seen and is scrolled initially one time to current system's time. Two methods created to play around this functionality **StartUpdatingCurrentTimeDashedLine** and **StopUpdatingCurrentTimeDashedLine**

Some know issue and work in progress is
1) Overlapping of cards issue is pending.
2) If a card start at end of day then further space addition for the card is required.

Tested so far in android only...
# Screenshots
<img src="https://github.com/PrayagMisal/CustomSchedularControl/blob/master/pic1.png" width="280" height="500">
