using CommonData;
using LibraryManagement.Exceptions;
using LibraryManagement.Services;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddTransient<BookService>();
            services.AddTransient<UserService>();
            services.AddTransient<BookTransactionsService>();
            services.AddTransient<ValidationService>();
            services.AddTransient<ErrorHandlingMiddleware>();
        }
    }
}
