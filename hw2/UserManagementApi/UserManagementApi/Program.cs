using UserManagementApi.Data;
using UserManagementApi.Repository;
using UserManagementApi.Services;
using UserManagementApi.Validation;

using var context = new AppDbContext();

var repository = new UserRepository(context);
var validator = new RegistrationValidator();
var service = new RegistrationService(
    validator,
    repository);

while (true)
{
    Console.WriteLine();
    Console.WriteLine("=== User Management ===");
    Console.WriteLine("1. Регистрация");
    Console.WriteLine("2. Авторизация");
    Console.WriteLine("3. Редактирование пользователя");
    Console.WriteLine("4. Удаление пользователя");
    Console.WriteLine("0. Выход");
    Console.Write("Выберите действие: ");

    var choice = Console.ReadLine();

    Console.WriteLine();

    switch (choice)
    {
        case "1":
            Register();
            break;

        case "2":
            Login();
            break;

        case "3":
            Edit();
            break;

        case "4":
            Delete();
            break;

        case "0":
            return;

        default:
            Console.WriteLine("Неизвестная команда.");
            break;
    }
}

void Register()
{
    Console.WriteLine("=== Регистрация ===");

    Console.Write("Имя: ");
    var name = Console.ReadLine() ?? "";

    Console.Write("Логин: ");
    var login = Console.ReadLine() ?? "";

    Console.Write("Email: ");
    var email = Console.ReadLine() ?? "";

    Console.Write("Пароль: ");
    var password = Console.ReadLine() ?? "";

    var errors = service.Register(
        name,
        login,
        email,
        password);

    if (errors.Count == 0)
    {
        Console.WriteLine("Пользователь успешно зарегистрирован!");
    }
    else
    {
        Console.WriteLine("Ошибки:");

        foreach (var error in errors)
        {
            Console.WriteLine($"- {error}");
        }
    }
}

void Login()
{
    Console.WriteLine("=== Авторизация ===");

    Console.Write("Логин: ");
    var login = Console.ReadLine() ?? "";

    Console.Write("Пароль: ");
    var password = Console.ReadLine() ?? "";

    var user = service.Login(login, password);

    if (user is null)
    {
        Console.WriteLine("Неверный логин или пароль.");
    }
    else
    {
        Console.WriteLine($"Добро пожаловать, {user.Name}!");
        Console.WriteLine($"Ваш ID: {user.Id}");
    }
}

void Edit()
{
    Console.WriteLine("=== Редактирование ===");

    Console.Write("ID пользователя: ");
    var idInput = Console.ReadLine();

    if (!int.TryParse(idInput, out var id))
    {
        Console.WriteLine("Некорректный ID.");
        return;
    }

    Console.Write("Новое имя: ");
    var name = Console.ReadLine() ?? "";

    Console.Write("Новый логин: ");
    var login = Console.ReadLine() ?? "";

    Console.Write("Новый email: ");
    var email = Console.ReadLine() ?? "";

    var errors = service.Edit(
        id,
        name,
        login,
        email);

    if (errors.Count == 0)
    {
        Console.WriteLine("Пользователь успешно изменён!");
    }
    else
    {
        Console.WriteLine("Ошибки:");

        foreach (var error in errors)
        {
            Console.WriteLine($"- {error}");
        }
    }
}

void Delete()
{
    Console.WriteLine("=== Удаление ===");

    Console.Write("ID пользователя: ");
    var idInput = Console.ReadLine();

    if (!int.TryParse(idInput, out var id))
    {
        Console.WriteLine("Некорректный ID.");
        return;
    }

    var deleted = service.Delete(id);

    if (deleted)
    {
        Console.WriteLine("Пользователь удалён.");
    }
    else
    {
        Console.WriteLine("Пользователь не найден.");
    }
}