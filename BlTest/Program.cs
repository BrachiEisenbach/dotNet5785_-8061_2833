
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace BlTest
{

    internal class Program
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        static void Main(string[] args)
        {
            var services = new ServiceCollection();
            services.AddAutoMapper(typeof(MappingProfile));
        }
    }
}
