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
        Console.WriteLine("Consola de Juego inicializada correctamente. Escribe 'ayuda' para ver comandos");

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
            case "ayuda":
                Console.WriteLine("Comandos disponibles: ");
                Console.WriteLine("  guardar - guardar escena actual ");
                Console.WriteLine("  cargar - cargar una escena guardada");
                Console.WriteLine("  editar - editar los elementos de la escena");
                Console.WriteLine("  exit - Close the application");
                break;

            case "guardar":
                ComandoGuardar();
                break;

            case "cargar":
                ComandoCargar();
                break;

            case "editar":
                ComandoEditar();
                break;

            case "salir":
                jueguito?.Close();
                break;

            default:
                Console.WriteLine($"Comando no reconocido: {command}");
                break;
        }
    }

    public static void ComandoEditar()
    {
        System.Console.WriteLine("Entrando en modo edición. Escriba 'salir' para salir del modo edición");
        System.Console.WriteLine("Escenario seleccionado");
        while (isRunning)
        {           
            string command = Console.ReadLine()?.Trim().ToLower() ?? "";
            if(command == "salir")
            {
                Console.WriteLine("Saliendo de edición");
                break;
            }
            ProcesoEditarEscenario(command);
        }
    }

    public static void ProcesoEditarEscenario(string command)
    {
        switch (command)
        {
            case "objetos":
                Dictionary<string, Objeto> Objetos = jueguito!.escenario.Objetos;
                System.Console.WriteLine("Objetos disponibles:");
                foreach (var objeto in Objetos)
                {
                    System.Console.WriteLine($"  {objeto.Key}");
                }
                break;
            case "seleccionar":
                System.Console.WriteLine("Introduzca el nombre del objeto a seleccionar");
                string? nombreObjeto = Console.ReadLine();
                if (nombreObjeto != null)
                {
                    ComandoSelecionarObjeto(nombreObjeto);
                }
                break;
            case "rotar":
                Console.WriteLine("Rotando escenario...");
                jueguito!.editor.currentTransformationMode = Editor.TransformationEditMode.Rotacion;
                break;
            case "mover":
                Console.WriteLine("Moviendo escenario...");
                jueguito!.editor.currentTransformationMode = Editor.TransformationEditMode.Traslacion;
                break;
            case "escalar":
                Console.WriteLine("Escalando escenario...");
                jueguito!.editor.currentTransformationMode = Editor.TransformationEditMode.Escalado;
                break;
            default:
                Console.WriteLine($"Comando no reconocido: {command}");
                break;
        }
    }

    public static void ComandoSelecionarObjeto(string NombreObjeto)
    {
        Objeto? objeto;
        if (! jueguito!.escenario.Objetos.TryGetValue(NombreObjeto, out objeto))
        {
            System.Console.WriteLine("El objeto no existe");
            return;
        }

        jueguito.editor.SelectedObjeto = NombreObjeto;
        System.Console.WriteLine($"Objeto seleccionado para editar: {NombreObjeto}");
        while (isRunning)
        {
            string command = Console.ReadLine()?.Trim().ToLower() ?? "";
            if (command == "salir")
            {
                jueguito!.editor.SelectedObjeto = "";
                Console.WriteLine($"Saliendo de la edición del objeto: {NombreObjeto}");
                break;
            }
            ProcesoEditarObjeto(objeto, command);
        }
    }

    public static void ProcesoEditarObjeto(Objeto objeto,string command)
    {
        switch (command)
        {
            case "partes":
                Dictionary<string, Parte> Partes = objeto.PartesLista;
                System.Console.WriteLine("Partes disponibles:");
                foreach (var parte in Partes)
                {
                    System.Console.WriteLine($"  {parte.Key}");
                }
                break;
            case "seleccionar":
                System.Console.WriteLine("Introduzca el nombre de la parte a seleccionar");
                string? nombreParte = Console.ReadLine();
                if (nombreParte != null)
                {
                    ComandoSelecionarParte(objeto, nombreParte);
                }
                break;
            case "rotar":
                Console.WriteLine("Rotando objeto...");
                jueguito!.editor.currentTransformationMode = Editor.TransformationEditMode.Rotacion;
                break;
            case "mover":
                Console.WriteLine("Moviendo objeto...");
                jueguito!.editor.currentTransformationMode = Editor.TransformationEditMode.Traslacion;
                break;
            case "escalar":
                Console.WriteLine("Escalando objeto...");
                jueguito!.editor.currentTransformationMode = Editor.TransformationEditMode.Escalado;
                break;
            default:
                Console.WriteLine($"Comando no reconocido: {command}");
                break;
        }
    }

    public static void ComandoSelecionarParte(Objeto objetoPadre, string NombreParte)
    {

        Parte? Parte;
        if (! objetoPadre.PartesLista.TryGetValue(NombreParte, out Parte))
        {
            System.Console.WriteLine("La parte no existe");
            return;
        }

        jueguito!.editor.SelectedParte = NombreParte;
        System.Console.WriteLine($"Parte seleccionado para editar: {NombreParte}");
        while (isRunning)
        {
            string command = Console.ReadLine()?.Trim().ToLower() ?? "";
            if (command == "salir")
            {
                jueguito!.editor.SelectedParte = "";
                Console.WriteLine($"Saliendo de la edición del Parte: {NombreParte}");
                break;
            }
            ProcesoEditarParte(Parte, command);
        }
    }

    public static void ProcesoEditarParte(Parte parte, string command)
    {
        switch (command)
        {
            case "rotar":
                Console.WriteLine("Rotando Parte...");
                jueguito!.editor.currentTransformationMode = Editor.TransformationEditMode.Rotacion;
                break;
            case "mover":
                Console.WriteLine("Moviendo Parte...");
                jueguito!.editor.currentTransformationMode = Editor.TransformationEditMode.Traslacion;
                break;
            case "escalar":
                Console.WriteLine("Escalando Parte...");
                jueguito!.editor.currentTransformationMode = Editor.TransformationEditMode.Escalado;
                break;
            default:
                Console.WriteLine($"Comando no reconocido: {command}");
                break;
        }
    }

    public static void ComandoCargar()
    {
        //Console.WriteLine("Introduzca el nombre del archivo");
        //string? fileName = Console.ReadLine();
        //if (fileName != null)
        //{
        //    System.Console.WriteLine($"Cargando escenario desde {fileName}");
        //    jueguito?.escenario.CargarEscenario(fileName);
        //}   

        jueguito!.escenario!.CargarEscenario("Escenario.json");
    }

    public static void ComandoGuardar()
    {
        //Console.WriteLine("Introduzca el nombre del archivo");
        //string? fileName = Console.ReadLine();
        //if (fileName != null)
        //{
        //    System.Console.WriteLine($"Guardando escenario en {fileName}");
        //    jueguito?.escenario.GuardarEscenario(fileName);
        //}
        jueguito!.escenario.GuardarEscenario("Escenario.json");
    }
}
