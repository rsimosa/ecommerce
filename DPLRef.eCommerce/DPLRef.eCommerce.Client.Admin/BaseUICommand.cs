using System;
using DPLRef.eCommerce.Common.Contracts;
using DPLRef.eCommerce.Common.Shared;

namespace DPLRef.eCommerce.Client
{
    abstract class BaseUICommand
    {
        protected AmbientContext Context { get; set; }

        public BaseUICommand(AmbientContext ambientContext)
        {
            Context = ambientContext;
        }

        public abstract string Name { get;  }

        protected virtual UICommandParameter[] Parameters { get; set; }

        public virtual void Run()
        {
            try
            {
                Console.WriteLine("Running Command " + Name);

                if (Parameters != null && Parameters.Length > 0)
                {
                    foreach (var p in Parameters)
                    {
                        p.PromptForValue();
                    }
                }

                CallManager();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        protected void ShowResponse(ResponseBase response, String result)
        {
            Console.WriteLine($"Result: { response.Success}");
            Console.WriteLine($"Message: {response.Message}");
            Console.WriteLine(result);
        }

        protected abstract void CallManager();
    }
}
