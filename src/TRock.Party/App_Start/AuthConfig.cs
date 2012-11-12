using Microsoft.Web.WebPages.OAuth;

namespace TRock.Party
{
    public static class AuthConfig
    {
        public static void RegisterAuth()
        {
            // To let users of this site log in using their accounts from other sites such as Microsoft, Facebook, and Twitter,
            // you must update this site. For more information visit http://go.microsoft.com/fwlink/?LinkID=252166

            //OAuthWebSecurity.RegisterMicrosoftClient(
            //    clientId: "",
            //    clientSecret: "");

            //OAuthWebSecurity.RegisterTwitterClient(
            //    consumerKey: "",
            //    consumerSecret: "");

            OAuthWebSecurity.RegisterFacebookClient(
                appId: "149854101797103",
                appSecret: "e11e84f86ea0a3aa094b00b2e5fc4e62");

            OAuthWebSecurity.RegisterGoogleClient();
        }
    }
}
