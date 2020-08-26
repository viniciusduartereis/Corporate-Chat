using Microsoft.Extensions.Configuration;

namespace Corporate.Chat.Console.Client
{

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }

    }
}