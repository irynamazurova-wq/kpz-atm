using System.Collections.Generic;
using ATMSimulator.Models;

namespace ATMSimulator.Data
{
    public interface IDataStorage
    {
        List<User> LoadUsers();
        void SaveUsers(List<User> users);
    }
}