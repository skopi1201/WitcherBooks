
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Klase;
using Domain.enums;


namespace Domain.Interfaces
{
    public interface ILogInService
    {
        public (bool,UserType) LogIN(string user, string pass);
    }
}
