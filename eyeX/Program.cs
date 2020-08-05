using System;
using System.Threading;
using System.Windows.Forms;
using eyeX.Models.Apis;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace eyeX
{
    public class Program
    {

        [STAThread]
        public static async System.Threading.Tasks.Task Main(string[] args)
        {
            // Building Web Host for Core Api functionality
            await CreateWebHostBuilder(args).Build().RunAsync();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run();

            //await Initializer.InitializeAPIAsync();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
