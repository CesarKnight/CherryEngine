using OpenTK.Windowing.GraphicsLibraryFramework;
using PrimerFigura;
using System.Threading;

internal class Program
{
    private static Game? jueguito;
    private static bool isRunning = true;

    private static void Main(string[] args)
    {
        // Iniciamos a escuchar la consola en un hilo separado  
        Thread consoleThread = new Thread(ReadConsoleInput);
        consoleThread.IsBackground = true;
        consoleThread.Start();

        // Start game on main thread  
        using (jueguito = new Game(1280, 720, "Primer Figura"))
        {
            jueguito.Run();
        }

        isRunning = false;
    }

    private static void ReadConsoleInput()
    {
        Console.WriteLine("Game console initialized. Type 'help' for available commands.");

        while (isRunning)
        {
            if (Console.KeyAvailable)
            {
                string command = Console.ReadLine()?.Trim().ToLower() ?? "";
                ProcessCommand(command);
            }

            Thread.Sleep(100);
        }
    }

    private static void ProcessCommand(string command)
    {
        switch (command)
        {
            case "help":
                Console.WriteLine("Available commands:");
                Console.WriteLine("  save - Save the current scene");
                Console.WriteLine("  load - Load a saved scene");
                Console.WriteLine("  add sphere - Add a sphere to the scene");
                Console.WriteLine("  exit - Close the application");
                break;

            case "save":
                //jueguito?.Escenario?.GuardarEscenario("Escenario.json");  
                Console.WriteLine("Scene saved to Escenario.json");
                break;

            case "exit":
                jueguito?.Close();
                break;

            default:
                Console.WriteLine($"Unknown command: {command}");
                break;
        }
    }
}
