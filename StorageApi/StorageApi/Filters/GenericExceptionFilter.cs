using Microsoft.AspNetCore.Mvc.Filters;

namespace StorageApi.Filters
{
    public class GenericExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            Console.WriteLine(context.Exception.StackTrace);
        }
    }
}