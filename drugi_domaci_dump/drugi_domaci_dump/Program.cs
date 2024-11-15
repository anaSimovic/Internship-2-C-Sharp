using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static Dictionary<int, (string firstName, string lastName, DateTime birthDate, Dictionary<string, decimal> accounts)> users = new();
    static Dictionary<int, List<(int id, decimal amount, string description, string type, string category, DateTime date)>> transactions = new();
    static int userIdCounter = 1;
    static int transactionIdCounter = 1;

    static void Main(string[] args)
    {
        InitializeSampleData();
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Finance Management App");
            Console.WriteLine("1 - Manage Users");
            Console.WriteLine("2 - Manage Accounts");
            Console.WriteLine("3 - Generate Financial Reports");
            Console.WriteLine("0 - Exit");
            Console.Write("Select an option: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    ManageUsersMenu();
                    break;
                case "2":
                    ManageAccountsMenu();
                    break;
                case "3":
                    GenerateReportMenu();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Invalid option. Press Enter to continue...");
                    Console.ReadLine();
                    break;
            }
        }
    }

    static void InitializeSampleData()
    {
        users[userIdCounter] = ("Marko", "Markić", new DateTime(1981, 6, 5), new Dictionary<string, decimal>
        {
            { "Current", 100.00m },
            { "Giro", 0.00m },
            { "Prepaid", 0.00m }
        });
        transactions[userIdCounter++] = new List<(int, decimal, string, string, string, DateTime)>();

        users[userIdCounter] = ("Ivana", "Ivić", new DateTime(1997, 11, 26), new Dictionary<string, decimal>
        {
            { "Current", 100.00m },
            { "Giro", 0.00m },
            { "Prepaid", 0.00m }
        });
        transactions[userIdCounter++] = new List<(int, decimal, string, string, string, DateTime)>();
    }

    static void ManageUsersMenu()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("User Management");
            Console.WriteLine("1 - Add New User");
            Console.WriteLine("2 - Delete User");
            Console.WriteLine("3 - View Users");
            Console.WriteLine("4 - Filter Users");
            Console.WriteLine("0 - Back to Main Menu");
            Console.Write("Select an option: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AddUser();
                    break;
                case "2":
                    DeleteUser();
                    break;
                case "3":
                    ViewUsers();
                    break;
                case "4":
                    FilterUsersMenu();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Invalid option. Press Enter to continue...");
                    Console.ReadLine();
                    break;
            }
        }
    }

    static void AddUser()
    {
        Console.Clear();
        Console.Write("Enter first name: ");
        string firstName = Console.ReadLine();
        Console.Write("Enter last name: ");
        string lastName = Console.ReadLine();
        Console.Write("Enter birth date (yyyy-MM-dd): ");
        DateTime birthDate;
        while (!DateTime.TryParse(Console.ReadLine(), out birthDate))
        {
            Console.Write("Invalid date. Try again: ");
        }

        users[userIdCounter] = (firstName, lastName, birthDate, new Dictionary<string, decimal>
        {
            { "Current", 100.00m },
            { "Giro", 0.00m },
            { "Prepaid", 0.00m }
        });
        transactions[userIdCounter++] = new List<(int, decimal, string, string, string, DateTime)>();
        Console.WriteLine("User added successfully. Press Enter to continue...");
        Console.ReadLine();
    }

    static void DeleteUser()
    {
        Console.Clear();
        Console.Write("Enter user ID to delete: ");
        int id;
        if (int.TryParse(Console.ReadLine(), out id) && users.ContainsKey(id))
        {
            Console.Write($"Are you sure you want to delete user {users[id].firstName} {users[id].lastName}? (y/n): ");
            if (Console.ReadLine().ToLower() == "y")
            {
                users.Remove(id);
                transactions.Remove(id);
                Console.WriteLine("User deleted successfully.");
            }
            else
            {
                Console.WriteLine("Operation canceled.");
            }
        }
        else
        {
            Console.WriteLine("User not found.");
        }
        Console.WriteLine("Press Enter to continue...");
        Console.ReadLine();
    }


    static void ViewUsers()
    {
        Console.Clear();
        Console.WriteLine("List of Users:");
        foreach (var (id, (firstName, lastName, birthDate, _)) in users.OrderBy(u => u.Value.lastName))
        {
            Console.WriteLine($"ID: {id}, Name: {firstName} {lastName}, Birth Date: {birthDate:yyyy-MM-dd}");
        }
        Console.WriteLine("Press Enter to continue...");
        Console.ReadLine();
    }

    static void ManageAccountsMenu()
    {
        Console.Clear();
        Console.Write("Enter user ID to manage accounts: ");
        int userId;
        if (int.TryParse(Console.ReadLine(), out userId) && users.ContainsKey(userId))
        {
            ManageUserAccounts(userId);
        }
        else
        {
            Console.WriteLine("User not found. Press Enter to return...");
            Console.ReadLine();
        }
    }
    static void FilterUsersMenu()
    {
        Console.Clear();
        Console.WriteLine("Select a criteria for displaying users:");
        Console.WriteLine("1 - Older than 30 years old");
        Console.WriteLine("2 - Users with accounts in negative balance");
        Console.Write("Choice: ");

        string choice = Console.ReadLine();
        switch (choice)
        {
            case "1":
                var olderUsers = users.Where(u => (DateTime.Now - u.Value.birthDate).TotalDays / 365 > 30);
                Console.WriteLine("Users older than 30 years old:");
                foreach (var (id, user) in olderUsers)
                {
                    Console.WriteLine($"ID: {id}, {user.firstName} {user.lastName}, Birthday: {user.birthDate:yyyy-MM-dd}");
                }
                break;

            case "2":
                var usersInDebt = users.Where(u => u.Value.accounts.Values.Any(balance => balance < 0));
                Console.WriteLine("Users with accounts in negative balance:");
                foreach (var (id, user) in usersInDebt)
                {
                    Console.WriteLine($"ID: {id}, {user.firstName} {user.lastName}");
                }
                break;

            default:
                Console.WriteLine("Invalid choice.");
                break;
        }

        Console.WriteLine("Press Enter to return.");
        Console.ReadLine();
    }
}

 
    
