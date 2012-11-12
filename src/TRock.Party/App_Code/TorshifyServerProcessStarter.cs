using System;
using System.IO;
using Autofac;

namespace TRock.Party
{
    public class TorshifyServerProcessStarter : IStartable
    {
        public void Start()
        {
            var appDomainSetup = AppDomain.CurrentDomain.SetupInformation;
            var applicationBase = appDomainSetup.PrivateBinPath ?? appDomainSetup.ApplicationBase;
            var torshifyLocation = Path.Combine(applicationBase, "App_Data", "Torshify", "TRock.Music.Torshify.Server.exe");

            var process = new Music.Torshify.TorshifyServerProcessHandler();
            process.TorshifyServerLocation = torshifyLocation;
            process.CloseServerTogetherWithClient = true;
            process.UserName = SiteGlobal.SpotifyUsername;
            process.Password = SiteGlobal.SpotifyPassword;
            process.Hidden = false;
            process.AutostartIfCrashed = true;
            process.Start();
        }
    }
}