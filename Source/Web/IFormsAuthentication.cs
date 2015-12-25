namespace ReusableLibrary.Web
{
    public interface IFormsAuthentication
    {
        void SignIn(string userName, bool createPersistentCookie);

        void SignIn(string userName, bool createPersistentCookie, string userData);

        string UserData();
        
        void SignOut();
    }
}
