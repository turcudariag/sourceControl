using Songs.Common.Entities;
using System;
using System.Collections.Generic;
using System.Text;


namespace Songs.Interfaces
{
    public interface IUsersService
    {
        string Authenticate(string username, string password);
        IEnumerable<User> GetAll();
        User GetById(Guid id);
    }
}
