using System.Web.Configuration;

namespace TRock.Party
{
    public static class SiteGlobal
    {
        #region Constructors

        static SiteGlobal()
        {
            SpotifyUsername = WebConfigurationManager.AppSettings["SpotifyUsername"];
            SpotifyPassword = WebConfigurationManager.AppSettings["SpotifyPassword"];
            SpotifyEnabled = WebConfigurationManager.AppSettings["SpotifyEnabled"] == "true";
            GroovesharkEnabled = WebConfigurationManager.AppSettings["GroovesharkEnabled"] == "true";
        }

        #endregion Constructors

        #region Properties

        public static string SpotifyUsername
        {
            get; set;
        }

        public static string SpotifyPassword
        {
            get; set;
        }

        public static bool SpotifyEnabled
        {
            get; set;
        }

        public static bool GroovesharkEnabled
        {
            get; set;
        }

        #endregion Properties
    }
}