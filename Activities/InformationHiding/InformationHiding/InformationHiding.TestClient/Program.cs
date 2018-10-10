using System;
using System.Diagnostics;
using InformationHiding.Dates;
using InformationHiding.Math;
using InformationHiding.Parsing;
using InformationHiding.Strings;

namespace InformationHiding.TestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            IDayOfWeek dow;
            IStringCharacters sc;
            IRemainder r;
            IDateFromOffset dfo;
            string firstDate;
            string secondDate;
            int num;
            int denom;
            int rem;
            string newDate;

            Console.WriteLine("Test 1: 10/10/2018 and 10/12/2018 Return 10/13/2018");
            Console.WriteLine("Hit ENTER to run the test");
            Console.ReadLine();

            //Test 1
            dow = new DayOfWeekService();
            sc = new StringCharacterService();
            r = new RemainderService();
            dfo = new DateFromOffsetService();
            try
            {
                firstDate = dow.GetDayOfWeek("10/10/2018");
                Debug.Assert(firstDate == "Wednesday");

                secondDate = dow.GetDayOfWeek("10/12/2018");
                Debug.Assert(secondDate == "Friday");

                num = sc.CountVowelsInString(firstDate);
                Debug.Assert(num == 3);

                denom = sc.CountVowelsInString(secondDate);
                Debug.Assert(denom == 2);

                rem = r.FindRemainder(num, denom);
                Debug.Assert(rem == 1);

                newDate = dfo.GetDateFromOffset("10/12/2018", rem);
                Debug.Assert(newDate == "10/13/2018");

                Console.WriteLine($"WooHoo!!! New date: '{newDate}'");
                Console.WriteLine();
            }
            catch (Exception e)
            {
                Console.WriteLine("BOOM!!! Exception encountered...");
                Console.WriteLine(e);
            }


            // Test 2
            Console.ReadLine();
            Console.WriteLine("Test 2: Let's test some edge cases");
            Console.WriteLine();

            // Parsing Tests
            try
            {
                Console.WriteLine("Hit ENTER to test parsing by passing an empty string...");
                Console.ReadLine();

                dow = new DayOfWeekService();

                // Passing an empty string
                var result = dow.GetDayOfWeek("");
                Console.WriteLine($"Passing an empty string for the date returns '{result}'");
                Console.WriteLine();
            }
            catch (Exception e)
            {
                Console.WriteLine("BOOM!!! Exception encountered...");
                Console.WriteLine(e);
            }

            // String Character Tests
            try
            {
                Console.ReadLine();
                Console.WriteLine("Hit ENTER to test string characters by passing a null...");
                Console.ReadLine();

                sc = new StringCharacterService();

                // Passing a null value
                var result = sc.CountVowelsInString(null);
                Console.WriteLine($"Passing null value for the string returns {result}");
                Console.WriteLine();
            }
            catch (Exception e)
            {
                Console.WriteLine("BOOM!!! Exception encountered...");
                Console.WriteLine(e);
            }


            try
            {
                Console.ReadLine();
                Console.WriteLine("Hit ENTER to test string characters by passing a mixed-case string...");
                Console.ReadLine();

                sc = new StringCharacterService();

                // Passing a null value
                var result = sc.CountVowelsInString("AaBbCcDdEe");
                Debug.Assert(result == 4);
                Console.WriteLine($"Passing 'AaBbCcDdEe' value for the string returns {result}");
                Console.WriteLine();
            }
            catch (Exception e)
            {
                Console.WriteLine("BOOM!!! Exception encountered...");
                Console.WriteLine(e);
            }

            // Math Tests
            try
            {
                Console.ReadLine();
                Console.WriteLine("Hit ENTER to test math by passing 0 for denominator...");
                Console.ReadLine();

                r = new RemainderService();

                // Passing a 0 for the denominator
                var result = r.FindRemainder(1, 0);
                Console.WriteLine($"Passing 0 for the denominator returns {result}");
                Console.WriteLine();
            }
            catch (Exception e)
            {
                Console.WriteLine("BOOM!!! Exception encountered...");
                Console.WriteLine(e);
            }

            // Date Tests
            try
            {
                Console.ReadLine();
                Console.WriteLine("Hit ENTER to test date by passing an invalid date string...");
                Console.ReadLine();

                dfo = new DateFromOffsetService();

                // Passing invalid string for date
                var result = dfo.GetDateFromOffset("blah", 0);
                Console.WriteLine($"Passing invalid date string returns '{result}'");
                Console.WriteLine();
            }
            catch (Exception e)
            {
                Console.WriteLine("BOOM!!! Exception encountered...");
                Console.WriteLine(e);
            }

            Console.WriteLine("All Done! Hit ENTER to quit");
            Console.ReadLine();
        }
    }
}
