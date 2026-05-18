using System.Collections.Generic;
using ATMSimulator.Models;

namespace ATMSimulator.Interfaces
{
    public interface IDataStorage
    {
        List<User> LoadUsers();
        void SaveUsers(List<User> users);
    }
}