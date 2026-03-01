namespace ThirdWorker;

// Exemplo de interface de serviço
public interface IGreetingService
{
    string GetGreeting();
}

// Exemplo de implementação de serviço
public class GreetingService : IGreetingService
{
    public string GetGreeting()
    {
        return "Hello from ThirdWorker with Dependency Injection!";
    }
}