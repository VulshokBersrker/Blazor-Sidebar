

public interface IDataService : IDisposable
{
    public record CalendarDayDetails(DateTime Date, int numEvents);
    Task<string> GetValueAsync();
    Task<bool> SaveValueAsync(string value);

}


public class CalendarDataService : IDataService
{

    protected record CalendarEvents(DateTime Date, string Name, string eventType);
    public record CalendarDayDetails(DateTime Date, int numEvents);

    // ----------- Calendar Values ----------- 
    protected string selectedDate { get; set; }
    protected string monthName = "";
    DateTime monthEnd;
    protected int monthsAway = 0;    protected int year = 0;
    protected int month = 0;
    protected List<CalendarEvents> events = new List<CalendarEvents>();


    // This List is to store all the info needed for the calendar layout<<<>
    public List<CalendarDayDetails> monthView = new List<CalendarDayDetails>();


    // Events to update the calendars when certain values are updated
    public event EventHandler<string> selectedDateChange = (sender, value) => { };

    public CalendarDataService()
    {
        Console.WriteLine("DataService constructed!");
        selectedDate = "";
        monthView = GenerateCalendarLayout();
    }

    public void Dispose()
    {
        Console.WriteLine("DataService disposed!");
    }
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


    public List<CalendarDayDetails> GenerateCalendarLayout()
    {
        // Create a temp data to get the month and year
        DateTime tempDate = DateTime.Now.AddMonths(monthsAway);
        month = tempDate.Month;
        year = tempDate.Year;

        // Set the start and end days for the month
        DateTime monthStart = new DateTime(year, month, 1);
        monthEnd = monthStart.AddMonths(1).AddDays(-1);
        monthName = monthStart.Month switch
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
            monthView.Add(new CalendarDayDetails(new DateTime(monthEnd.AddMonths(-1).Year, monthEnd.AddMonths(-1).Month, (prevMonthLastDay - i)), 0));
        }

        // Current Month
        for (int i = 1; i <= DateTime.DaysInMonth(year, month); i++)
        {
            monthView.Add(new CalendarDayDetails(new DateTime(year, month, i), 0));
        }

        // Next Month Section

        // Get the next month values
        int nextMonthLastDayName = (int)new DateTime(monthEnd.AddMonths(1).Year, monthEnd.AddMonths(1).Month, 1).DayOfWeek;
        // Now set the values
        for (int i = 0; i < (14 - nextMonthLastDayName); i++)
        {
            // Stop the list from getting longer than 6 weeks (42 days)
            if (monthView.Count() >= 42) { break; }
            monthView.Add(new CalendarDayDetails(new DateTime(monthEnd.AddMonths(1).Year, monthEnd.AddMonths(1).Month, (i + 1)), 0));
        }

        return monthView;
    }

    public List<CalendarDayDetails> UpdateCalendarLayout(int newMonth, int newYear)
    {
        Console.WriteLine(newMonth + " / " + newYear);

        // Clear the list to keep the formatting correct
        monthView.Clear();

        // Create a temp data to get the month and year
        DateTime tempDate = DateTime.Now.AddMonths(monthsAway);
        month = tempDate.Month;
        year = tempDate.Year;

        // Set the start and end days for the month
        DateTime monthStart = new DateTime(year, month, 1);
        monthEnd = monthStart.AddMonths(1).AddDays(-1);
        monthName = monthStart.Month switch
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
            monthView.Add(new CalendarDayDetails(new DateTime(monthEnd.AddMonths(-1).Year, monthEnd.AddMonths(-1).Month, (prevMonthLastDay - i)), 0));
        }

        // Current Month
        for (int i = 1; i <= DateTime.DaysInMonth(year, month); i++)
        {
            monthView.Add(new CalendarDayDetails(new DateTime(year, month, i), 0));
        }

        // Next Month Section

        // Get the next month values
        int nextMonthLastDayName = (int)new DateTime(monthEnd.AddMonths(1).Year, monthEnd.AddMonths(1).Month, 1).DayOfWeek;
        // Now set the values
        for (int i = 0; i < (14 - nextMonthLastDayName); i++)
        {
            // Stop the list from getting longer than 6 weeks (42 days)
            if (monthView.Count() >= 42) { break; }
            monthView.Add(new CalendarDayDetails(new DateTime(monthEnd.AddMonths(1).Year, monthEnd.AddMonths(1).Month, (i + 1)), 0));
        }

        return (List<CalendarDayDetails>)monthView;
    }





    // --------------- Getters --------------- 

    // Get the value to see if this Calendar is the first Calendar or not

    // Get current Month
    public int getMonth()
    {
        return month;
    }

    // Get current Year
    public int getYear()
    {
        return year;
    }

    // Get dayList

    // Get


    // --------------- Setters --------------- 
    // Set 
    public void setDirection(int dir)
    {

    }
    // Changes the selected Date from the string gotten from the element
    public void setSelectedDate(string value)
    {
        selectedDate = value;
        Console.WriteLine("DataService - " + selectedDate);
        selectedDateChange?.Invoke(this, value);
    }


    // Send the new selectedDate value to the other calendar
    public string onSelectedDateChange()
    {
        return selectedDate;
    }

}
