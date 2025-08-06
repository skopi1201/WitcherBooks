using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Domain.enums;

namespace Domain.Klase
{
    [Serializable]
    public class User
    {
        public string user { get; set; }
        public string pass { get; set; }
        public UserType type { get; set; }


        public User() { }
        public User(string username, string password, UserType type)
        {
            this.user = username;
            this.pass = password;
            this.type = type;
        }

    }
}


