namespace LibraryManagement.Exceptions
{     
    public class ErrorHandlingMiddleware: IMiddleware
    {
        ILogger<ErrorHandlingMiddleware> _Logger;

        public ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger)
        {
            _Logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, ex.Message);
            }
        }
    }
}
