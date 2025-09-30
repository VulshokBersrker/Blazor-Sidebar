
public interface IDataService : IDisposable
{
    public record CalendarDayDetails(DateTime Date, int numEvents);
    Task<string> GetValueAsync();
    Task<bool> SaveValueAsync(string value);

}


public class CalendarDataService : IDataService
{
    public record CalendarEvents(DateTime Date, string Name, string eventType);
    // This will be expanded to add the list of events happening each day for the event markers on the calendar
    public record CalendarDayDetails(DateTime Date, int numEvents, IEnumerable<CalendarEvents> dayEvents);
    public record CalendarLayoutInfo(List<CalendarDayDetails> monthView, string monthName, int month, int year);

    // ----------- Calendar Values ----------- 
    protected string selectedDate { get; set; }
    protected int monthsAway = 0;
    protected List<CalendarEvents> events = new List<CalendarEvents>();


    // Events to update the calendars when certain values are updated
    public event EventHandler<string> selectedDateChange = (sender, value) => { };
    public event EventHandler<int> monthsAwayChange = (sender, value) => { };

    public CalendarDataService()
    {
        Console.WriteLine("DataService constructed!");
        selectedDate = "";

        events.Add(new CalendarEvents(new DateTime(2025, 10, 2), "Test day 1", "venue-event"));
        events.Add(new CalendarEvents(new DateTime(2025, 9, 30), "Test day 2", "venue-event"));
        events.Add(new CalendarEvents(new DateTime(2025, 9, 30), "Test day 33", "venue-event"));
        events.Add(new CalendarEvents(new DateTime(2025, 9, 30), "Test day 44", "venue-event"));
        events.Add(new CalendarEvents(new DateTime(2025, 9, 30), "Test day 55", "venue-event"));
        events.Add(new CalendarEvents(new DateTime(2025, 9, 30), "Test day 3", "venue-event"));
        events.Add(new CalendarEvents(new DateTime(2025, 9, 29), "Test day 4", "venue-event"));
        events.Add(new CalendarEvents(new DateTime(2025, 9, 28), "Test day 5", "venue-event"));
		events.Add(new CalendarEvents(new DateTime(2025, 10, 31), "Test day 6", "holiday"));
		events.Add(new CalendarEvents(new DateTime(2025, 10, 25), "Christmas", "birthday"));
		events.Add(new CalendarEvents(new DateTime(2025, 10, 1), "New Years", "important-event"));
        
        events.Add(new CalendarEvents(new DateTime(2025, 10, 2), "Test day 7", "venue-event"));
		events.Add(new CalendarEvents(new DateTime(2025, 10, 30), "Test day 8", "holiday"));
		events.Add(new CalendarEvents(new DateTime(2025, 10, 12), "Test Day 9", "birthday"));
        events.Add(new CalendarEvents(new DateTime(2025, 10, 16), "Test Day 10", "important-event"));
    }

    public void Dispose()
    {
        Console.WriteLine("DataService disposed!");
    }

    public CalendarLayoutInfo generateCalendarLayout(bool isFirstCalendar)
    {
        // This List is to store all the info needed for the calendar layout
        List<CalendarDayDetails> monthView = new List<CalendarDayDetails>();

        // Create a temp data to get the month and year
        DateTime tempDate = DateTime.Now.AddMonths(monthsAway);
        if (isFirstCalendar == false)
        {
            tempDate = DateTime.Now.AddMonths(monthsAway + 1);
        }
        int month = tempDate.Month;
        int year = tempDate.Year;

        // Set the start and end days for the month
        DateTime monthStart = new DateTime(year, month, 1);
        DateTime monthEnd = monthStart.AddMonths(1).AddDays(-1);
        string monthName = monthStart.Month switch
        {
            1 => "January",
            2 => "February",
            3 => "March",
            4 => "April",
            5 => "May",
            6 => "June",
            7 => "July",
            8 => "August",
            9 => "September",
            10 => "October",
            11 => "November",
            12 => "December",
            _ => ""
        };


        // ---- This Section is for the days that will appear on the calendar outside the current month ---- 

        // Previous Month Section

        // Get the previous month values
        int prevMonthLastDay = DateTime.DaysInMonth(monthEnd.AddMonths(-1).Year, monthEnd.AddMonths(-1).Month);
        DateTime temp = new DateTime(monthEnd.AddMonths(-1).Year, monthEnd.AddMonths(-1).Month, prevMonthLastDay);
        // Now set the values
        for (int i = 0; i <= (int)temp.DayOfWeek; i++)
        {
            // Find all the events happening on each day
            string tempD = monthEnd.AddMonths(-1).Month + "/" + (prevMonthLastDay - i) + "/" + monthEnd.AddMonths(-1).Year;
            IEnumerable<CalendarEvents>? eventsOnThisDay = events.Where(n =>  n.Date.ToShortDateString() == DateTime.ParseExact(tempD, "d", null).ToString("d") );

            monthView.Add(
                new CalendarDayDetails( new DateTime(monthEnd.AddMonths(-1).Year, monthEnd.AddMonths(-1).Month, prevMonthLastDay - i),
                eventsOnThisDay.Count(),
                eventsOnThisDay
            ) );
        }

        // Current Month
        for (int i = 1; i <= DateTime.DaysInMonth(year, month); i++)
        {
            string tempD = monthEnd.Month + "/" + i + "/" + monthEnd.Year;
            IEnumerable<CalendarEvents>? eventsOnThisDay = events.Where(n =>  n.Date.ToShortDateString() == DateTime.ParseExact(tempD, "d", null).ToString("d") );

            monthView.Add(
                new CalendarDayDetails( new DateTime(year, month, i),
                eventsOnThisDay.Count(),
                eventsOnThisDay
            ));
        }

        // Next Month Section

        // Get the next month values
        int nextMonthLastDayName = (int)new DateTime(monthEnd.AddMonths(1).Year, monthEnd.AddMonths(1).Month, 1).DayOfWeek;
        // Now set the values
        for (int i = 0; i < (14 - nextMonthLastDayName); i++)
        {
            // Stop the list from getting longer than 6 weeks (42 days)
            if (monthView.Count() >= 42) { break; }

            string tempD = monthEnd.AddMonths(1).Month + "/" + (i + 1) + "/" + monthEnd.AddMonths(1).Year;
            IEnumerable<CalendarEvents>? eventsOnThisDay = events.Where(n =>  n.Date.ToShortDateString() == DateTime.ParseExact(tempD, "d", null).ToString("d") );

            monthView.Add(
                new CalendarDayDetails( new DateTime(monthEnd.AddMonths(1).Year, monthEnd.AddMonths(1).Month, i + 1),
                eventsOnThisDay.Count(),
                eventsOnThisDay
            ));
        }

        return new CalendarLayoutInfo(monthView, monthName, month, year);
    }

    public int[] createForecast()
    {
        int[] forecastArray = new int[13];

        foreach(CalendarEvents item in events)
        {
            for (int j = 0; j < forecastArray.Length; j++)
            {
                if (item.Date.ToString("MM/dd/yyyy") == DateTime.ParseExact(selectedDate, "d", null).AddDays(j).ToString("MM/dd/yyyy"))
                {
                    forecastArray[j]++;
                    break;
                }
            }
        }
        return forecastArray;
    }

    // Only get the events that happen in the forecast range
    public List<CalendarEvents> getEventsForecast()
    {
        List<CalendarEvents> temp = new List<CalendarEvents>();

        foreach (CalendarEvents item in events)
        {
            for (int i = 0; i < 13; i++)
            {
                if (item.Date.ToString("MM/dd/yyyy") == DateTime.ParseExact(selectedDate, "d", null).AddDays(i).ToString("MM/dd/yyyy"))
                {
                    temp.Add(item);
                    break;
                }
            }
        }
        return temp;
    }

    public List<CalendarEvents> getEventsCalendar()
    {
        return events;
    }

    // Changes the selected Date from the string gotten from the element
    public void setSelectedDate(string value)
    {
        selectedDate = value;
        selectedDateChange?.Invoke(this, value);
    }

    // Send the new selectedDate value to the other calendar
    public string onSelectedDateChange()
    {
        return selectedDate;
    }

    public void setMonthsAway(int value, bool isFirstCalendar)
    {
        monthsAway += value;
        monthsAwayChange?.Invoke(this, value);
    }
    

    // Placeholder functions that can be used for fetches on the event data
    public Task<string> GetValueAsync()
    {
        Console.WriteLine("TODO: Get value from database...");
        return Task.FromResult("Hello World!");
    }
    public Task<bool> SaveValueAsync(string value)
    {
        Console.WriteLine($"TODO: Save value '{value}' to database...");
        return Task.FromResult(true);
    }

}