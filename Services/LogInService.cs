
using Domain.Interfaces;
using Domain.Klase;
using Domain.enums;
using System.Collections.ObjectModel;



namespace Services
{
    public class LogInService : ILogInService
    {
        public ObservableCollection<User> users;
        DataIO serializer = new DataIO();

        public LogInService() 
        {
            users = serializer.DeSerializeObject<ObservableCollection<User>>("UserLogin.xml");
            if (users == null)
            {
                users = new ObservableCollection<User>();
            }

        }


        public (bool,UserType) LogIN(string user, string pass)
        {
            foreach (User u in users)
            {
                if (user==u.user)
                {
                    if (pass == u.pass)
                    {
                        return (true,u.type);
                    }
                }
            }
            return (false,UserType.Consumer);
        }
    }
}
