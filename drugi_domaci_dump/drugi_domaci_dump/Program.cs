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

    static void ManageUserAccounts(int userId)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine($"Managing accounts for {users[userId].firstName} {users[userId].lastName}");
            Console.WriteLine("1 - View Accounts");
            Console.WriteLine("2 - Add Transaction");
            Console.WriteLine("3 - View Transactions");
            Console.WriteLine("4 - Edit Transactions");
            Console.WriteLine("5 - Delete Transactions");
            Console.WriteLine("6 - Transfer Funds");
            Console.WriteLine("7 - Delete Transactions by Criteria");
            Console.WriteLine("8 - Sort Transactions");
            Console.WriteLine("9 - Transfer Funds to Another User");
            Console.WriteLine("0 - Back to Previous Menu");
            Console.Write("Select an option: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    ViewAccounts(userId);
                    break;
                case "2":
                    AddTransaction(userId);
                    break;
                case "3":
                    ViewTransactions(userId);
                    break;
                case "4":
                    EditTransaction(userId);
                    break;
                case "5":
                    DeleteTransaction(userId);
                    break;
                case "6":
                    TransferFunds(userId);
                    break;
                case "7":
                    DeleteTransactionByCriteria(userId);
                    break;
                case "8":
                    SortTransactionsMenu(userId);
                    break;
                case "9":
                    TransferFundsToAnotherUser();
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


    static void ViewAccounts(int userId)
    {
        Console.Clear();
        Console.WriteLine($"Accounts for {users[userId].firstName} {users[userId].lastName}:");
        foreach (var (accountType, balance) in users[userId].accounts)
        {
            Console.WriteLine($"{accountType}: {balance:C}");
        }
        Console.WriteLine("Press Enter to continue...");
        Console.ReadLine();
    }

    static void AddTransaction(int userId)
    {
        Console.Clear();
        Console.WriteLine("Select account: Current, Giro, or Prepaid");
        string account = Console.ReadLine();

        if (!users[userId].accounts.ContainsKey(account))
        {
            Console.WriteLine("Invalid account type. Press Enter to return...");
            Console.ReadLine();
            return;
        }

        Console.Write("Enter transaction amount: ");
        decimal amount;
        while (!decimal.TryParse(Console.ReadLine(), out amount) || amount <= 0)
        {
            Console.Write("Invalid amount. Try again (must be positive): ");
        }

        Console.Write("Enter description: ");
        string description = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(description)) description = "Standard transaction";

        Console.Write("Enter type (Income/Expense): ");
        string type;
        while (true)
        {
            type = Console.ReadLine();
            if (type.Equals("Income", StringComparison.OrdinalIgnoreCase) || type.Equals("Expense", StringComparison.OrdinalIgnoreCase))
                break;
            Console.Write("Invalid type. Enter 'Income' or 'Expense': ");
        }

        Console.Write("Enter category: ");
        string category = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(category)) category = "Uncategorized";

        DateTime date = DateTime.Now;
        transactions[userId].Add((transactionIdCounter++, amount, description, type, category, date));
        users[userId].accounts[account] += type.Equals("Income", StringComparison.OrdinalIgnoreCase) ? amount : -amount;

        Console.WriteLine("Transaction added successfully. Press Enter to continue...");
        Console.ReadLine();
    }

    static void ViewTransactions(int userId)
    {
        Console.Clear();
        var userTransactions = transactions[userId];
        if (userTransactions.Count == 0)
        {
            Console.WriteLine("No transactions found.");
        }
        else
        {
            foreach (var (id, amount, description, type, category, date) in userTransactions)
            {
                Console.WriteLine($"ID: {id}, Amount: {amount:C}, Description: {description}, Type: {type}, Category: {category}, Date: {date:yyyy-MM-dd}");
            }
        }
        Console.WriteLine("Press Enter to continue...");
        Console.ReadLine();
    }
    static void SortTransactionsMenu(int userId)
    {
        Console.Clear();
        Console.WriteLine("Select the sorting method for transactions:");
        Console.WriteLine("1 - By amount (ascending)");
        Console.WriteLine("2 - By amount (descending)");
        Console.WriteLine("3 - By description (alphabetically)");
        Console.WriteLine("4 - By date (ascending)");
        Console.WriteLine("5 - By date (descending)");
        Console.Write("Choice: ");

        string choice = Console.ReadLine();
        var userTransactions = transactions[userId];

        switch (choice)
        {
            case "1":
                userTransactions = userTransactions.OrderBy(t => t.amount).ToList();
                break;

            case "2":
                userTransactions = userTransactions.OrderByDescending(t => t.amount).ToList();
                break;

            case "3":
                userTransactions = userTransactions.OrderBy(t => t.description).ToList();
                break;

            case "4":
                userTransactions = userTransactions.OrderBy(t => t.date).ToList();
                break;

            case "5":
                userTransactions = userTransactions.OrderByDescending(t => t.date).ToList();
                break;

            default:
                Console.WriteLine("Invalid choice.");
                return;
        }

        transactions[userId] = userTransactions;
        Console.WriteLine("Transactions have been sorted. Press Enter to display.");
        Console.ReadLine();
        ViewTransactions(userId);
    }


    static void EditTransaction(int userId)
    {
        Console.Clear();
        Console.Write("Enter transaction ID to edit: ");
        int transactionId;
        if (!int.TryParse(Console.ReadLine(), out transactionId))
        {
            Console.WriteLine("Invalid ID. Press Enter to return...");
            Console.ReadLine();
            return;
        }

        var transaction = transactions[userId].FirstOrDefault(t => t.id == transactionId);
        if (transaction.id == 0)
        {
            Console.WriteLine("Transaction not found. Press Enter to return...");
            Console.ReadLine();
            return;
        }

        Console.WriteLine($"Editing Transaction: {transaction}");
        Console.Write("Enter new description (leave blank to keep current): ");
        string description = Console.ReadLine();
        Console.Write("Enter new amount (leave blank to keep current): ");
        string amountInput = Console.ReadLine();
        Console.Write("Enter new category (leave blank to keep current): ");
        string category = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(amountInput) && decimal.TryParse(amountInput, out decimal amount))
        {
            transaction.amount = amount;
        }

        if (!string.IsNullOrWhiteSpace(description))
        {
            transaction.description = description;
        }

        if (!string.IsNullOrWhiteSpace(category))
        {
            transaction.category = category;
        }

        transactions[userId] = transactions[userId].Select(t => t.id == transactionId ? transaction : t).ToList();
        Console.WriteLine("Transaction updated successfully. Press Enter to continue...");
        Console.ReadLine();
    }
    static void DeleteTransactionByCriteria(int userId)
    {
        Console.Clear();
        Console.WriteLine("Select the criteria for deleting transactions:");
        Console.WriteLine("1 - All below the entered amount");
        Console.WriteLine("2 - All above the entered amount");
        Console.WriteLine("3 - All income transactions");
        Console.WriteLine("4 - All expense transactions");
        Console.WriteLine("5 - All transactions for the selected category");
        Console.Write("Choice: ");

        string choice = Console.ReadLine();
        decimal amount;
        string category;

        switch (choice)
        {
            case "1":
                Console.Write("Enter the maximum amount: ");
                if (decimal.TryParse(Console.ReadLine(), out amount))
                {
                    transactions[userId] = transactions[userId]
                        .Where(t => t.amount >= amount).ToList();
                    Console.WriteLine("Transactions below the entered amount have been deleted.");
                }
                else
                {
                    Console.WriteLine("Invalid input.");
                }
                break;

            case "2":
                Console.Write("Enter the minimum amount: ");
                if (decimal.TryParse(Console.ReadLine(), out amount))
                {
                    transactions[userId] = transactions[userId]
                        .Where(t => t.amount <= amount).ToList();
                    Console.WriteLine("Transactions above the entered amount have been deleted.");
                }
                else
                {
                    Console.WriteLine("Invalid input.");
                }
                break;

            case "3":
                transactions[userId] = transactions[userId]
                    .Where(t => !t.type.Equals("Income", StringComparison.OrdinalIgnoreCase)).ToList();
                Console.WriteLine("All income transactions have been deleted.");
                break;

            case "4":
                transactions[userId] = transactions[userId]
                    .Where(t => !t.type.Equals("Expense", StringComparison.OrdinalIgnoreCase)).ToList();
                Console.WriteLine("All expense transactions have been deleted.");
                break;

            case "5":
                Console.Write("Enter the category: ");
                category = Console.ReadLine();
                transactions[userId] = transactions[userId]
                    .Where(t => !t.category.Equals(category, StringComparison.OrdinalIgnoreCase)).ToList();
                Console.WriteLine("Transactions for the category have been deleted.");
                break;

            default:
                Console.WriteLine("Invalid choice.");
                break;
        }

        Console.WriteLine("Press Enter to return.");
        Console.ReadLine();
    }


    static void DeleteTransaction(int userId)
    {
        Console.Clear();
        Console.Write("Enter transaction ID to delete: ");
        int transactionId;
        if (!int.TryParse(Console.ReadLine(), out transactionId))
        {
            Console.WriteLine("Invalid ID. Press Enter to return...");
            Console.ReadLine();
            return;
        }

        var transaction = transactions[userId].FirstOrDefault(t => t.id == transactionId);
        if (transaction.id == 0)
        {
            Console.WriteLine("Transaction not found. Press Enter to return...");
            Console.ReadLine();
            return;
        }

        Console.Write($"Are you sure you want to delete this transaction? (y/n): ");
        if (Console.ReadLine().ToLower() == "y")
        {
            transactions[userId] = transactions[userId].Where(t => t.id != transactionId).ToList();
            Console.WriteLine("Transaction deleted successfully.");
        }
        else
        {
            Console.WriteLine("Operation canceled.");
        }

        Console.WriteLine("Press Enter to continue...");
        Console.ReadLine();
    }
    static void TransferFundsToAnotherUser()
    {
        Console.Clear();
        Console.Write("Enter your user ID: ");
        if (!int.TryParse(Console.ReadLine(), out int senderId) || !users.ContainsKey(senderId))
        {
            Console.WriteLine("Invalid sender ID. Press Enter to return...");
            Console.ReadLine();
            return;
        }

        Console.Write("Enter recipient user ID: ");
        if (!int.TryParse(Console.ReadLine(), out int receiverId) || !users.ContainsKey(receiverId))
        {
            Console.WriteLine("Invalid recipient ID. Press Enter to return...");
            Console.ReadLine();
            return;
        }

        Console.Write("Enter the amount to transfer: ");
        if (!decimal.TryParse(Console.ReadLine(), out decimal amount) || amount <= 0)
        {
            Console.WriteLine("Invalid amount. Press Enter to return...");
            Console.ReadLine();
            return;
        }

        Console.Write("Enter sender's account (Current/Giro/Prepaid): ");
        string senderAccount = Console.ReadLine();
        Console.Write("Enter recipient's account (Current/Giro/Prepaid): ");
        string receiverAccount = Console.ReadLine();

        if (!users[senderId].accounts.ContainsKey(senderAccount) || !users[receiverId].accounts.ContainsKey(receiverAccount))
        {
            Console.WriteLine("Invalid account types. Press Enter to return...");
            Console.ReadLine();
            return;
        }

        if (users[senderId].accounts[senderAccount] < amount)
        {
            Console.WriteLine("Insufficient funds. Press Enter to return...");
            Console.ReadLine();
            return;
        }

        users[senderId].accounts[senderAccount] -= amount;
        users[receiverId].accounts[receiverAccount] += amount;

        Console.WriteLine($"Successfully transferred {amount:C} from {users[senderId].firstName} to {users[receiverId].firstName}.");
        Console.WriteLine("Press Enter to continue...");
        Console.ReadLine();
    }

    static void TransferFunds(int userId)
    {
        Console.Clear();
        Console.WriteLine("Enter source account (Current/Giro/Prepaid): ");
        string sourceAccount = Console.ReadLine();
        Console.WriteLine("Enter target account (Current/Giro/Prepaid): ");
        string targetAccount = Console.ReadLine();

        if (!users[userId].accounts.ContainsKey(sourceAccount) || !users[userId].accounts.ContainsKey(targetAccount))
        {
            Console.WriteLine("Invalid accounts. Press Enter to return...");
            Console.ReadLine();
            return;
        }

        Console.Write("Enter amount to transfer: ");
        decimal amount;
        while (!decimal.TryParse(Console.ReadLine(), out amount) || amount <= 0 || amount > users[userId].accounts[sourceAccount])
        {
            Console.Write("Invalid amount. Ensure it's positive and within source account balance: ");
        }

        users[userId].accounts[sourceAccount] -= amount;
        users[userId].accounts[targetAccount] += amount;

        Console.WriteLine("Transfer successful. Press Enter to continue...");
        Console.ReadLine();
    }

    static void GenerateReportMenu()
    {
        Console.Clear();
        Console.Write("Enter user ID to generate report: ");
        int userId;
        if (int.TryParse(Console.ReadLine(), out userId) && users.ContainsKey(userId))
        {
            GenerateFinancialReport(userId);
        }
        else
        {
            Console.WriteLine("User not found. Press Enter to return...");
            Console.ReadLine();
        }
    }

    static void GenerateFinancialReport(int userId)
    {
        Console.Clear();
        var userTransactions = transactions[userId];

        decimal totalIncome = userTransactions.Where(t => t.type == "Income").Sum(t => t.amount);
        decimal totalExpense = userTransactions.Where(t => t.type == "Expense").Sum(t => t.amount);
        decimal balance = totalIncome - totalExpense;

        Console.WriteLine($"Financial Report for {users[userId].firstName} {users[userId].lastName}");
        Console.WriteLine($"Total Income: {totalIncome} m");
        Console.WriteLine($"Total Expense: {totalExpense} m");
        Console.WriteLine($"Balance: {balance} m");

        Console.Write("Enter month and year (MM/yyyy) for detailed report: ");
        string input = Console.ReadLine();
        if (DateTime.TryParseExact(input, "MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime date))
        {
            var monthlyTransactions = userTransactions
                .Where(t => t.date.Month == date.Month && t.date.Year == date.Year).ToList();

            decimal monthlyIncome = monthlyTransactions.Where(t => t.type == "Income").Sum(t => t.amount);
            decimal monthlyExpense = monthlyTransactions.Where(t => t.type == "Expense").Sum(t => t.amount);
            int transactionCount = monthlyTransactions.Count;
            decimal averageTransaction = transactionCount > 0 ? monthlyTransactions.Average(t => t.amount) : 0;

            Console.WriteLine($"Monthly Income: {monthlyIncome} m");
            Console.WriteLine($"Monthly Expense: {monthlyExpense} m");
            Console.WriteLine($"Average Transaction: {averageTransaction} m");
        }
        else
        {
            Console.WriteLine("Invalid date format.");
        }

        Console.WriteLine("Press Enter to continue...");
        Console.ReadLine();
    }


}
