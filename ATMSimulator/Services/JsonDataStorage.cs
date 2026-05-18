using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using ATMSimulator.Interfaces;
using ATMSimulator.Models;

namespace ATMSimulator.Services
{
    public class JsonDataStorage : IDataStorage
    {
        private static JsonDataStorage _instance;
        private static readonly object _lock = new object();
        private readonly string _filePath = "users.json";

        private JsonDataStorage()
        {
            if (!File.Exists(_filePath))
            {
                SeedInitialData();
            }
        }

        public static JsonDataStorage Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new JsonDataStorage();
                    }
                    return _instance;
                }
            }
        }

        public List<User> LoadUsers()
{
    try
    {
        if (!File.Exists(_filePath)) return new List<User>();
        
        string jsonString = File.ReadAllText(_filePath);
        var users = JsonSerializer.Deserialize<List<User>>(jsonString);

        if (users == null || users.Count == 0)
        {
            SeedInitialData();
            return LoadUsers();
        }
        return users;
    }
    catch (Exception)
    {

        SeedInitialData();
        string jsonString = File.ReadAllText(_filePath);
        return JsonSerializer.Deserialize<List<User>>(jsonString) ?? new List<User>();
    }
}

        public void SaveUsers(List<User> users)
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string jsonString = JsonSerializer.Serialize(users, options);
                File.WriteAllText(_filePath, jsonString);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка збереження даних: {ex.Message}");
            }
        }

        private void SeedInitialData()
        {
            var testUsers = new List<User>
            {
                new User("1", "Максим", "Остапчук", 
                    new Card("1111222233334444", "1234"), 
                    new Account("UA123456", 5000m)),    
                
                new User("2", "Оксана", "Седляр", 
                    new Card("5555666677778888", "4321"), 
                    new Account("UA654321", 12500m))       
            };
            SaveUsers(testUsers);
        }
    }
}