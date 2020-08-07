using eyeX.Models.Apis;
using eyeX.Models.GazeData;
using eyeX.Models.Globals;
using eyeX.Properties;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Sockets;

namespace eyeX
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            Main();
        }
        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseMvc();
        }

        /// <summary>
        /// Program main start point
        /// </summary>
        private async void Main()
        {
            // TEST: Initialization and Client Connection

            //var client = new Client
            //{
            //    Name = "test-client",
            //    IPAddress = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 4444),
            //    GazeDataSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp),
            //    FixationDataSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
            //};
            //client.GazeDataSocket.Connect(client.IPAddress);
            //client.FixationDataSocket.Connect(client.IPAddress);
            //Globals.Clients.Add(client);
            //await ApiManager.InitializeApiAsync(Settings.Default.EyeTrackerApi);


        }
    }
}