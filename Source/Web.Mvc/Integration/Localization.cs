namespace ReusableLibrary.Web.Mvc.Integration
{
    public static class Localization
    {
        static Localization()
        {
            DefaultLanguage = "en";
            Languages = new[] { DefaultLanguage };
        }

        public static string DefaultLanguage { get; set; }

        public static string[] Languages { get; set; }
    }
}
