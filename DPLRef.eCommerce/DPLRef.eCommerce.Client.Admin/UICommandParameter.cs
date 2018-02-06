using System;

namespace DPLRef.eCommerce.Client
{
    abstract class UICommandParameter
    {
        public string Message { get; set; }

        public abstract void PromptForValue();
    }

    class IntUICommandParameter : UICommandParameter
    {
        public int Value { get; set; }

        public override void PromptForValue()
        {
            Console.WriteLine(Message);
            var str = Console.ReadLine();
            int tmp = 0;
            if (int.TryParse(str, out tmp))
            {
                Value = tmp;
            }
            else
            {
                throw new InvalidOperationException("Integer parameter could not be parsed.");
            }
        }
    }

    class DecimalUICommandParameter : UICommandParameter
    {
        public decimal Value { get; set; }

        public override void PromptForValue()
        {
            Console.WriteLine(Message);
            var str = Console.ReadLine();
            decimal tmp = 0.0M;
            if (decimal.TryParse(str, out tmp))
            {
                Value = tmp;
            }
            else
            {
                throw new InvalidOperationException("Decimal parameter could not be parsed.");
            }
        }
    }

    class StringUICommandParameter : UICommandParameter
    {
        public string Value { get; set; }

        public override void PromptForValue()
        {
            Console.WriteLine(Message);
            Value = Console.ReadLine();
        }
    }
}
