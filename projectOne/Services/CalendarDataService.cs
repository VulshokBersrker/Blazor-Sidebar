
public interface IDataService : IDisposable
{
    Task<string> GetValueAsync();
    Task<bool> SaveValueAsync(string value);
  
}
public class CalendarDataService : IDataService
{
    public CalendarDataService()  => Console.WriteLine("DataService constructed!");  // not a proper IDisposable implementation,
    // but it'll do for demo purposes
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
}


public class CalendarService
{
    protected record CalendarEvents(DateTime Date, string Name, string eventType);
    protected record CalendarDayDetails(DateTime Date, int numEvents);

    protected bool isFirstCalendar { get; set; }
    protected string selectedDate { get; set; }

    // Events record
    protected List<CalendarEvents> events = new List<CalendarEvents>();

    // Today's Date
    protected string today = DateTime.Now.ToShortDateString();

    // Current Month Values
    protected string monthName = "";
    DateTime monthEnd;
    protected int monthsAway = 0;
    protected int year = 0;
    protected int month = 0;

    // Previous Month Values
    protected int[] prevMonthDays = new int[7];
    // Next Month Values
    protected int[] nextMonthDays = new int[14];
    // Value to stop at the sixth week row
    protected int calendarLength = 0;


    protected List<int> testLength = new List<int>();

    // This List is to store all the info needed for the calendar layout
    protected List<CalendarDayDetails> monthView = new List<CalendarDayDetails>();

    // Constructor
    public CalendarService(bool isFirstCalendar, int currentMonth, int currentYear)
    {
        this.isFirstCalendar = isFirstCalendar;
        this.month = currentMonth;
        this.year = currentYear;
        this.selectedDate = today;

        generateCalendarLayout();
    }


    // --------------- Methods --------------- 

    protected void generateCalendarLayout()
    {
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
    }



    // Changes the selected Date from the string gotten from the element
    protected void changeSelectedDate(string value)
    {
        this.selectedDate = value;
    }







    // --------------- Getters --------------- 

    // Get the value to see if this Calendar is the first Calendar or not
    protected bool getIsFirstCalendar()
    {
        return this.isFirstCalendar;

    }
    // Get current Month
    public int getMonth()
    {
        return this.month;
    }

    // Get current Year
    public int getYear()
    {
        return this.year;
    }

    // Get dayList

    // Get


    // --------------- Setters --------------- 
    // Set 
    protected void setDirection()
    {

    }

    protected void setEvents()
    {


    }
}