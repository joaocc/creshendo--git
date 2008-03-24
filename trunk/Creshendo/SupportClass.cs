using System;
using System.Collections;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

public interface IThreadRunnable
{
    void Run();
}

public class SupportClass
{
    public static Random Random = new Random();

    public static int URShift(int number, int bits)
    {
        if (number >= 0)
            return number >> bits;
        else
            return (number >> bits) + (2 << ~bits);
    }

    public static int URShift(int number, long bits)
    {
        return URShift(number, (int) bits);
    }

    public static long URShift(long number, int bits)
    {
        if (number >= 0)
            return number >> bits;
        else
            return (number >> bits) + (2L << ~bits);
    }

    public static long URShift(long number, long bits)
    {
        return URShift(number, (int) bits);
    }

    /*******************************/

    public static void WriteStackTrace(Exception throwable, TextWriter stream)
    {
        stream.Write(throwable.Message);
        stream.Flush();
    }

    /*******************************/

    /// <summary>
    /// This method is used as a dummy method to simulate VJ++ behavior
    /// </summary>
    /// <param name="literal">The literal to return</param>
    /// <returns>The received value</returns>
    public static long Identity(long literal)
    {
        return literal;
    }

    /// <summary>
    /// This method is used as a dummy method to simulate VJ++ behavior
    /// </summary>
    /// <param name="literal">The literal to return</param>
    /// <returns>The received value</returns>
    public static ulong Identity(ulong literal)
    {
        return literal;
    }

    /// <summary>
    /// This method is used as a dummy method to simulate VJ++ behavior
    /// </summary>
    /// <param name="literal">The literal to return</param>
    /// <returns>The received value</returns>
    public static float Identity(float literal)
    {
        return literal;
    }

    /// <summary>
    /// This method is used as a dummy method to simulate VJ++ behavior
    /// </summary>
    /// <param name="literal">The literal to return</param>
    /// <returns>The received value</returns>
    public static double Identity(double literal)
    {
        return literal;
    }

    /*******************************/

    /// <summary>
    /// Adds an element to the top end of a Stack instance.
    /// </summary>
    /// <param name="stack">The Stack instance</param>
    /// <param name="element">The element to Add</param>
    /// <returns>The element added</returns>  
    public static Object StackPush(Stack stack, Object element)
    {
        stack.Push(element);
        return element;
    }

    /*******************************/

    /// <summary>
    /// Creates an instance of a received Type
    /// </summary>
    /// <param name="classType">The Type of the new class instance to return</param>
    /// <returns>An Object containing the new instance</returns>
    public static Object CreateNewInstance(Type classType)
    {
        ConstructorInfo[] constructors = classType.GetConstructors();

        if (constructors.Length == 0)
            return null;

        ParameterInfo[] firstConstructor = constructors[0].GetParameters();
        int countParams = firstConstructor.Length;

        Type[] constructor = new Type[countParams];
        for (int i = 0; i < countParams; i++)
            constructor[i] = firstConstructor[i].ParameterType;

        return classType.GetConstructor(constructor).Invoke(new Object[] {});
    }

    /*******************************/

    /*******************************/

    public static Object PutElement(Hashtable hashTable, Object key, Object newValue)
    {
        Object element = hashTable[key];
        hashTable[key] = newValue;
        return element;
    }

    /*******************************/

    /*******************************/

    public static string FormatDateTime(DateTimeFormatInfo format, DateTime date)
    {
        string timePattern = DateTimeFormatManager.manager.GetTimeFormatPattern(format);
        string datePattern = DateTimeFormatManager.manager.GetDateFormatPattern(format);
        return date.ToString(datePattern + " " + timePattern, format);
    }

    /*******************************/

    public static DateTimeFormatInfo GetDateTimeFormatInstance(int dateStyle, int timeStyle, CultureInfo culture)
    {
        DateTimeFormatInfo format = culture.DateTimeFormat;

        switch (timeStyle)
        {
            case -1:
                DateTimeFormatManager.manager.SetTimeFormatPattern(format, "");
                break;

            case 0:
                DateTimeFormatManager.manager.SetTimeFormatPattern(format, "h:mm:ss 'o clock' tt zzz");
                break;

            case 1:
                DateTimeFormatManager.manager.SetTimeFormatPattern(format, "h:mm:ss tt zzz");
                break;

            case 2:
                DateTimeFormatManager.manager.SetTimeFormatPattern(format, "h:mm:ss tt");
                break;

            case 3:
                DateTimeFormatManager.manager.SetTimeFormatPattern(format, "h:mm tt");
                break;
        }

        switch (dateStyle)
        {
            case -1:
                DateTimeFormatManager.manager.SetDateFormatPattern(format, "");
                break;

            case 0:
                DateTimeFormatManager.manager.SetDateFormatPattern(format, "dddd, MMMM dd%, yyy");
                break;

            case 1:
                DateTimeFormatManager.manager.SetDateFormatPattern(format, "MMMM dd%, yyy");
                break;

            case 2:
                DateTimeFormatManager.manager.SetDateFormatPattern(format, "d-MMM-yy");
                break;

            case 3:
                DateTimeFormatManager.manager.SetDateFormatPattern(format, "M/dd/yy");
                break;
        }

        return format;
    }

    #region Nested type: CalendarManager

    public class CalendarManager
    {
        public const int DATE = 2;
        public const int DAY_OF_MONTH = 7;
        public const int HOUR = 3;
        public const int HOUR_OF_DAY = 8;
        public const int MILLISECOND = 6;
        public const int MINUTE = 4;
        public const int MONTH = 1;
        public const int SECOND = 5;
        public const int YEAR = 0;

        public static CalendarHashTable manager = new CalendarHashTable();

        #region Nested type: CalendarHashTable

        public class CalendarHashTable : Hashtable
        {
            public DateTime GetDateTime(Calendar calendar)
            {
                if (this[calendar] != null)
                    return ((CalendarProperties) this[calendar]).dateTime;
                else
                {
                    CalendarProperties tempProps = new CalendarProperties();
                    tempProps.dateTime = DateTime.Now;
                    Add(calendar, tempProps);
                    return GetDateTime(calendar);
                }
            }

            public void SetDateTime(Calendar calendar, DateTime date)
            {
                if (this[calendar] != null)
                {
                    ((CalendarProperties) this[calendar]).dateTime = date;
                }
                else
                {
                    CalendarProperties tempProps = new CalendarProperties();
                    tempProps.dateTime = date;
                    Add(calendar, tempProps);
                }
            }

            public void Set(Calendar calendar, int field, int fieldValue)
            {
                if (this[calendar] != null)
                {
                    DateTime tempDate = ((CalendarProperties) this[calendar]).dateTime;
                    switch (field)
                    {
                        case DATE:
                            tempDate = tempDate.AddDays(fieldValue - tempDate.Day);
                            break;
                        case HOUR:
                            tempDate = tempDate.AddHours(fieldValue - tempDate.Hour);
                            break;
                        case MILLISECOND:
                            tempDate = tempDate.AddMilliseconds(fieldValue - tempDate.Millisecond);
                            break;
                        case MINUTE:
                            tempDate = tempDate.AddMinutes(fieldValue - tempDate.Minute);
                            break;
                        case MONTH:
                            //Month value is 0-based. e.g., 0 for January
                            tempDate = tempDate.AddMonths(fieldValue - (tempDate.Month + 1));
                            break;
                        case SECOND:
                            tempDate = tempDate.AddSeconds(fieldValue - tempDate.Second);
                            break;
                        case YEAR:
                            tempDate = tempDate.AddYears(fieldValue - tempDate.Year);
                            break;
                        case DAY_OF_MONTH:
                            tempDate = tempDate.AddDays(fieldValue - tempDate.Day);
                            break;
                        case HOUR_OF_DAY:
                            tempDate = tempDate.AddHours(fieldValue - tempDate.Hour);
                            break;

                        default:
                            break;
                    }
                    ((CalendarProperties) this[calendar]).dateTime = tempDate;
                }
                else
                {
                    CalendarProperties tempProps = new CalendarProperties();
                    tempProps.dateTime = DateTime.Now;
                    Add(calendar, tempProps);
                    Set(calendar, field, fieldValue);
                }
            }

            public void Set(Calendar calendar, int year, int month, int day)
            {
                if (this[calendar] != null)
                {
                    Set(calendar, YEAR, year);
                    Set(calendar, MONTH, month);
                    Set(calendar, DATE, day);
                }
                else
                {
                    CalendarProperties tempProps = new CalendarProperties();
                    //Month value is 0-based. e.g., 0 for January
                    tempProps.dateTime = new DateTime(year, month + 1, day);
                    Add(calendar, tempProps);
                }
            }

            public void Set(Calendar calendar, int year, int month, int day, int hour, int minute)
            {
                if (this[calendar] != null)
                {
                    Set(calendar, YEAR, year);
                    Set(calendar, MONTH, month);
                    Set(calendar, DATE, day);
                    Set(calendar, HOUR, hour);
                    Set(calendar, MINUTE, minute);
                }
                else
                {
                    CalendarProperties tempProps = new CalendarProperties();
                    //Month value is 0-based. e.g., 0 for January
                    tempProps.dateTime = new DateTime(year, month + 1, day, hour, minute, 0);
                    Add(calendar, tempProps);
                }
            }

            public void Set(Calendar calendar, int year, int month, int day, int hour, int minute, int second)
            {
                if (this[calendar] != null)
                {
                    Set(calendar, YEAR, year);
                    Set(calendar, MONTH, month);
                    Set(calendar, DATE, day);
                    Set(calendar, HOUR, hour);
                    Set(calendar, MINUTE, minute);
                    Set(calendar, SECOND, second);
                }
                else
                {
                    CalendarProperties tempProps = new CalendarProperties();
                    //Month value is 0-based. e.g., 0 for January
                    tempProps.dateTime = new DateTime(year, month + 1, day, hour, minute, second);
                    Add(calendar, tempProps);
                }
            }

            public int Get(Calendar calendar, int field)
            {
                if (this[calendar] != null)
                {
                    switch (field)
                    {
                        case DATE:
                            return ((CalendarProperties) this[calendar]).dateTime.Day;
                        case HOUR:
                            int tempHour = ((CalendarProperties) this[calendar]).dateTime.Hour;
                            return tempHour > 12 ? tempHour - 12 : tempHour;

                        case MILLISECOND:
                            return ((CalendarProperties) this[calendar]).dateTime.Millisecond;
                        case MINUTE:
                            return ((CalendarProperties) this[calendar]).dateTime.Minute;
                        case MONTH:
                            //Month value is 0-based. e.g., 0 for January
                            return ((CalendarProperties) this[calendar]).dateTime.Month - 1;
                        case SECOND:
                            return ((CalendarProperties) this[calendar]).dateTime.Second;
                        case YEAR:
                            return ((CalendarProperties) this[calendar]).dateTime.Year;
                        case DAY_OF_MONTH:
                            return ((CalendarProperties) this[calendar]).dateTime.Day;
                        case HOUR_OF_DAY:
                            return ((CalendarProperties) this[calendar]).dateTime.Hour;

                        default:
                            return 0;
                    }
                }
                else
                {
                    CalendarProperties tempProps = new CalendarProperties();
                    tempProps.dateTime = DateTime.Now;
                    Add(calendar, tempProps);
                    return Get(calendar, field);
                }
            }

            public void SetTimeInMilliseconds(Calendar calendar, long milliseconds)
            {
                if (this[calendar] != null)
                {
                    ((CalendarProperties) this[calendar]).dateTime = new DateTime(milliseconds);
                }
                else
                {
                    CalendarProperties tempProps = new CalendarProperties();
                    tempProps.dateTime = new DateTime(TimeSpan.TicksPerMillisecond*milliseconds);
                    Add(calendar, tempProps);
                }
            }

            public DayOfWeek GetFirstDayOfWeek(Calendar calendar)
            {
                if (this[calendar] != null && ((CalendarProperties) this[calendar]).dateTimeFormat != null)
                {
                    return ((CalendarProperties) this[calendar]).dateTimeFormat.FirstDayOfWeek;
                }
                else
                {
                    CalendarProperties tempProps = new CalendarProperties();
                    tempProps.dateTimeFormat = new DateTimeFormatInfo();
                    tempProps.dateTimeFormat.FirstDayOfWeek = DayOfWeek.Sunday;
                    Add(calendar, tempProps);
                    return GetFirstDayOfWeek(calendar);
                }
            }

            public void SetFirstDayOfWeek(Calendar calendar, DayOfWeek firstDayOfWeek)
            {
                if (this[calendar] != null && ((CalendarProperties) this[calendar]).dateTimeFormat != null)
                {
                    ((CalendarProperties) this[calendar]).dateTimeFormat.FirstDayOfWeek = firstDayOfWeek;
                }
                else
                {
                    CalendarProperties tempProps = new CalendarProperties();
                    tempProps.dateTimeFormat = new DateTimeFormatInfo();
                    Add(calendar, tempProps);
                    SetFirstDayOfWeek(calendar, firstDayOfWeek);
                }
            }

            public void Clear(Calendar calendar)
            {
                if (this[calendar] != null)
                    Remove(calendar);
            }

            public void Clear(Calendar calendar, int field)
            {
                if (this[calendar] != null)
                    Remove(calendar);
                else
                    Set(calendar, field, 0);
            }

            #region Nested type: CalendarProperties

            private class CalendarProperties
            {
                public DateTime dateTime;
                public DateTimeFormatInfo dateTimeFormat;
            }

            #endregion
        }

        #endregion
    }

    #endregion

    #region Nested type: DateTimeFormatManager

    public class DateTimeFormatManager
    {
        public static DateTimeFormatHashTable manager = new DateTimeFormatHashTable();

        #region Nested type: DateTimeFormatHashTable

        public class DateTimeFormatHashTable : Hashtable
        {
            public void SetDateFormatPattern(DateTimeFormatInfo format, String newPattern)
            {
                if (this[format] != null)
                    ((DateTimeFormatProperties) this[format]).DateFormatPattern = newPattern;
                else
                {
                    DateTimeFormatProperties tempProps = new DateTimeFormatProperties();
                    tempProps.DateFormatPattern = newPattern;
                    Add(format, tempProps);
                }
            }

            public string GetDateFormatPattern(DateTimeFormatInfo format)
            {
                if (this[format] == null)
                    return "d-MMM-yy";
                else
                    return ((DateTimeFormatProperties) this[format]).DateFormatPattern;
            }

            public void SetTimeFormatPattern(DateTimeFormatInfo format, String newPattern)
            {
                if (this[format] != null)
                    ((DateTimeFormatProperties) this[format]).TimeFormatPattern = newPattern;
                else
                {
                    DateTimeFormatProperties tempProps = new DateTimeFormatProperties();
                    tempProps.TimeFormatPattern = newPattern;
                    Add(format, tempProps);
                }
            }

            public string GetTimeFormatPattern(DateTimeFormatInfo format)
            {
                if (this[format] == null)
                    return "h:mm:ss tt";
                else
                    return ((DateTimeFormatProperties) this[format]).TimeFormatPattern;
            }

            #region Nested type: DateTimeFormatProperties

            private class DateTimeFormatProperties
            {
                public string DateFormatPattern = "d-MMM-yy";
                public string TimeFormatPattern = "h:mm:ss tt";
            }

            #endregion
        }

        #endregion
    }

    #endregion

    #region Nested type: GraphicsManager

    public class GraphicsManager
    {
        public static GraphicsHashTable manager = new GraphicsHashTable();

        public static Graphics CreateGraphics(Graphics oldGraphics)
        {
            IntPtr hdc = oldGraphics.GetHdc();
            oldGraphics.ReleaseHdc(hdc);
            return Graphics.FromHdc(hdc);
        }

        /// <summary>
        /// This method draws a Bezier curve.
        /// </summary>
        /// <param name="graphics">It receives the Graphics instance</param>
        /// <param name="array">An array of (x,y) pairs of coordinates used to draw the curve.</param>
        public static void Bezier(Graphics graphics, int[] array)
        {
            Pen pen;
            pen = manager.GetPen(graphics);
            graphics.DrawBezier(pen, array[0], array[1], array[2], array[3], array[4], array[5], array[6], array[7]);
        }

        public static Point GetTextSize(Graphics graphics, Font graphicsFont, String text)
        {
            Point textSize;
            SizeF tempSizeF;
            tempSizeF = graphics.MeasureString(text, graphicsFont);
            textSize = new Point();
            textSize.X = (int) tempSizeF.Width;
            textSize.Y = (int) tempSizeF.Height;
            return textSize;
        }

        public static Point GetTextSize(Graphics graphics, Font graphicsFont, String text, Int32 width, StringFormat format)
        {
            Point textSize;
            SizeF tempSizeF;
            tempSizeF = graphics.MeasureString(text, graphicsFont, width, format);
            textSize = new Point();
            textSize.X = (int) tempSizeF.Width;
            textSize.Y = (int) tempSizeF.Height;
            return textSize;
        }

        #region Nested type: GraphicsHashTable

        public class GraphicsHashTable : Hashtable
        {
            public Graphics GetGraphics(Control control)
            {
                Graphics graphic;
                if (control.Visible == true)
                {
                    graphic = control.CreateGraphics();
                    if (this[graphic] == null)
                    {
                        GraphicsProperties tempProps = new GraphicsProperties();
                        tempProps.color = control.ForeColor;
                        tempProps.BackColor = control.BackColor;
                        tempProps.TextColor = control.ForeColor;
                        tempProps.GraphicFont = control.Font;
                        Add(graphic, tempProps);
                    }
                }
                else
                {
                    graphic = null;
                }
                return graphic;
            }

            public void SetBackColor(Graphics graphic, Color color)
            {
                if (this[graphic] != null)
                    ((GraphicsProperties) this[graphic]).BackColor = color;
                else
                {
                    GraphicsProperties tempProps = new GraphicsProperties();
                    tempProps.BackColor = color;
                    Add(graphic, tempProps);
                }
            }

            public Color GetBackColor(Graphics graphic)
            {
                if (this[graphic] == null)
                    return Color.White;
                else
                    return ((GraphicsProperties) this[graphic]).BackColor;
            }

            public void SetTextColor(Graphics graphic, Color color)
            {
                if (this[graphic] != null)
                    ((GraphicsProperties) this[graphic]).TextColor = color;
                else
                {
                    GraphicsProperties tempProps = new GraphicsProperties();
                    tempProps.TextColor = color;
                    Add(graphic, tempProps);
                }
            }

            public Color GetTextColor(Graphics graphic)
            {
                if (this[graphic] == null)
                    return Color.White;
                else
                    return ((GraphicsProperties) this[graphic]).TextColor;
            }

            public void SetBrush(Graphics graphic, SolidBrush brush)
            {
                if (this[graphic] != null)
                    ((GraphicsProperties) this[graphic]).GraphicBrush = brush;
                else
                {
                    GraphicsProperties tempProps = new GraphicsProperties();
                    tempProps.GraphicBrush = brush;
                    Add(graphic, tempProps);
                }
            }

            public SolidBrush GetBrush(Graphics graphic)
            {
                if (this[graphic] == null)
                    return new SolidBrush(Color.Black);
                else
                    return ((GraphicsProperties) this[graphic]).GraphicBrush;
            }

            public void SetPen(Graphics graphic, Pen pen)
            {
                if (this[graphic] != null)
                    ((GraphicsProperties) this[graphic]).GraphicPen = pen;
                else
                {
                    GraphicsProperties tempProps = new GraphicsProperties();
                    tempProps.GraphicPen = pen;
                    Add(graphic, tempProps);
                }
            }

            public Pen GetPen(Graphics graphic)
            {
                if (this[graphic] == null)
                    return Pens.Black;
                else
                    return ((GraphicsProperties) this[graphic]).GraphicPen;
            }

            public void SetFont(Graphics graphic, Font font)
            {
                if (this[graphic] != null)
                    ((GraphicsProperties) this[graphic]).GraphicFont = font;
                else
                {
                    GraphicsProperties tempProps = new GraphicsProperties();
                    tempProps.GraphicFont = font;
                    Add(graphic, tempProps);
                }
            }

            public Font GetFont(Graphics graphic)
            {
                if (this[graphic] == null)
                    return new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, ((Byte) (0)));
                else
                    return ((GraphicsProperties) this[graphic]).GraphicFont;
            }

            public void SetColor(Graphics graphic, Color color)
            {
                if (this[graphic] != null)
                {
                    ((GraphicsProperties) this[graphic]).GraphicPen.Color = color;
                    ((GraphicsProperties) this[graphic]).GraphicBrush.Color = color;
                    ((GraphicsProperties) this[graphic]).color = color;
                }
                else
                {
                    GraphicsProperties tempProps = new GraphicsProperties();
                    tempProps.GraphicPen.Color = color;
                    tempProps.GraphicBrush.Color = color;
                    tempProps.color = color;
                    Add(graphic, tempProps);
                }
            }

            public Color GetColor(Graphics graphic)
            {
                if (this[graphic] == null)
                    return Color.Black;
                else
                    return ((GraphicsProperties) this[graphic]).color;
            }

            /// <summary>
            /// This method gets the TextBackgroundColor of a Graphics instance
            /// </summary>
            /// <param name="graphic">The graphics instance</param>
            /// <returns></returns>
            public Color GetTextBackgroundColor(Graphics graphic)
            {
                if (this[graphic] == null)
                    return Color.Black;
                else
                {
                    return ((GraphicsProperties) this[graphic]).TextBackgroundColor;
                }
            }

            /// <summary>
            /// This method set the TextBackgroundColor of a Graphics instace
            /// </summary>
            /// <param name="graphic">The graphics instace</param>
            /// <param name="color">The System.Color to set the TextBackgroundColor</param>
            public void SetTextBackgroundColor(Graphics graphic, Color color)
            {
                if (this[graphic] != null)
                {
                    ((GraphicsProperties) this[graphic]).TextBackgroundColor = color;
                }
                else
                {
                    GraphicsProperties tempProps = new GraphicsProperties();
                    tempProps.TextBackgroundColor = color;
                    Add(graphic, tempProps);
                }
            }

            #region Nested type: GraphicsProperties

            private class GraphicsProperties
            {
                public Color BackColor = Color.White;
                public Color color = Color.Black;
                public SolidBrush GraphicBrush = new SolidBrush(Color.Black);
                public Font GraphicFont = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, ((Byte) (0)));
                public Pen GraphicPen = new Pen(Color.Black);
                public Color TextBackgroundColor = Color.Black;
                public Color TextColor = Color.Black;
            }

            #endregion
        }

        #endregion
    }

    #endregion

    #region Nested type: ThreadClass
    /*
    public class ThreadClass : IThreadRunnable
    {
        private Thread threadField;

        public ThreadClass()
        {
            threadField = new Thread(new ThreadStart(Run));
            threadField.Name = "MessageRouter";
        }

        public ThreadClass(ThreadStart p1)
        {
            threadField = new Thread(p1);
        }

        public Thread Instance
        {
            get { return threadField; }
            set { threadField = value; }
        }

        public String Name
        {
            get { return threadField.Name; }
            set
            {
                if (threadField.Name == null)
                    threadField.Name = value;
            }
        }

        public ThreadPriority Priority
        {
            get { return threadField.Priority; }
            set { threadField.Priority = value; }
        }

        public bool IsAlive
        {
            get { return threadField.IsAlive; }
        }

        public bool IsBackground
        {
            get { return threadField.IsBackground; }
            set { threadField.IsBackground = value; }
        }

        #region IThreadRunnable Members

        public virtual void Run()
        {
        }

        #endregion

        public virtual void Start()
        {
            threadField.Start();
        }

        public void Join()
        {
            threadField.Join();
        }

        public void Join(long p1)
        {
            lock (this)
            {
                threadField.Join(new TimeSpan(p1*10000));
            }
        }

        public void Join(long p1, int p2)
        {
            lock (this)
            {
                threadField.Join(new TimeSpan(p1*10000 + p2*100));
            }
        }

        public void Resume()
        {
            threadField.Resume();
        }

        public void Abort()
        {
            threadField.Abort();
        }

        public void Abort(Object stateInfo)
        {
            lock (this)
            {
                threadField.Abort(stateInfo);
            }
        }

        public void Suspend()
        {
            threadField.Suspend();
        }

        public override String ToString()
        {
            return "Thread[" + Name + "," + Priority.ToString() + "," + "" + "]";
        }

        public static ThreadClass Current()
        {
            ThreadClass CurrentThread = new ThreadClass();
            CurrentThread.Instance = Thread.CurrentThread;
            return CurrentThread;
        }
    }
    */
    #endregion
}