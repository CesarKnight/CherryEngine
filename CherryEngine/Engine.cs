using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Threading;
using CherryEngine.Editor;
using CherryEngine.Components;
using CherryEngine.Core;

internal class Engine
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
                Console.WriteLine("  editar - editar los elementos de la escena");
                Console.WriteLine("  controles - ver las teclas usadas para el motor grafico");
                Console.WriteLine("  archivo - Selecciona el archivo donde el escenario se cargara y guardara, Usa los de la carpeta Escenarios porfa");
                Console.WriteLine("  cargar - cargar una escena guardada");
                Console.WriteLine("  salir - Close the application");
                break;

            case "archivo":
                ComandoArchivoDireccion();
                break;

            case "editar":
                ComandoEditar();
                break;

            case "controles":
                ComandoControles();
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
                jueguito!.editor.currentTransformationMode = null;
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
                jueguito!.editor.currentTransformationMode = TransformationEditor.TransformationEditMode.Rotacion;
                break;
            case "trasladar":
                Console.WriteLine("Moviendo escenario...");
                jueguito!.editor.currentTransformationMode = TransformationEditor.TransformationEditMode.Traslacion;
                break;
            case "escalar":
                Console.WriteLine("Escalando escenario...");
                jueguito!.editor.currentTransformationMode = TransformationEditor.TransformationEditMode.Escalado;
                break;
            case "ayuda":
                Console.WriteLine("Comandos disponibles: ");
                Console.WriteLine("  objetos - Listar los objetos de la escena");
                Console.WriteLine("  seleccionar - seleccionar un objeto para editar");
                Console.WriteLine("  rotar - rotar la escena ");
                Console.WriteLine("  trasladar - trasladar la escena");
                Console.WriteLine("  escalar - escalar la escena");
                Console.WriteLine("  salir - salir de edicion de escenario");
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
                jueguito!.editor.currentTransformationMode = null;
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
                jueguito!.editor.currentTransformationMode = TransformationEditor.TransformationEditMode.Rotacion;
                break;
            case "trasladar":
                Console.WriteLine("Moviendo objeto...");
                jueguito!.editor.currentTransformationMode = TransformationEditor.TransformationEditMode.Traslacion;
                break;
            case "escalar":
                Console.WriteLine("Escalando objeto...");
                jueguito!.editor.currentTransformationMode = TransformationEditor.TransformationEditMode.Escalado;
                break;
            case "ayuda":
                Console.WriteLine("Comandos disponibles: ");
                Console.WriteLine("  partes - Listar las partes del objeto");
                Console.WriteLine("  seleccionar - seleccionar un parte para editar");
                Console.WriteLine("  rotar - rotar el objeto");
                Console.WriteLine("  trasladar - trasladar el objeto");
                Console.WriteLine("  escalar - escalar el objeto");
                Console.WriteLine("  salir - salir de edicion de escenario");
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
                jueguito!.editor.currentTransformationMode = null;
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
                jueguito!.editor.currentTransformationMode = TransformationEditor.TransformationEditMode.Rotacion;
                break;
            case "trasladar":
                Console.WriteLine("Moviendo Parte...");
                jueguito!.editor.currentTransformationMode = TransformationEditor.TransformationEditMode.Traslacion;
                break;
            case "escalar":
                Console.WriteLine("Escalando Parte...");
                jueguito!.editor.currentTransformationMode = TransformationEditor.TransformationEditMode.Escalado;
                break;
            case "ayuda":
                Console.WriteLine("Comandos disponibles: ");
                Console.WriteLine("  rotar - rotar la parte");
                Console.WriteLine("  trasladar - trasladar la parte");
                Console.WriteLine("  escalar - escalar la parte");
                Console.WriteLine("  salir - salir de edicion de escenario");
                break;
            default:
                Console.WriteLine($"Comando no reconocido: {command}");
                break;
        }
    }

    public static void ComandoArchivoDireccion()
    {
        Console.WriteLine("Introduzca el nombre del archivo");
        string? fileName = Console.ReadLine();
        if (fileName != null)
        {
            System.Console.WriteLine($"Seleccionando escenario desde {fileName}");
            jueguito!.editor.EscenarioFileName = fileName;
        }
    }


    public static void ComandoControles()
    {
        System.Console.Clear();
        string controles = """
            ------------Controles dentro del juego-------------
            W - Mover en el eje  Z (adelante)
            S - Mover en el eje -Z (atras)
            A - Mover en el eje  X (derecha)
            D - Mover en el eje -X (izquierda)
            ESPACIO - Mover en el eje  Y (arriba)
            CTRL - Mover en el eje -Y (abajo)
            TAB - Cambiar entre modo tranformacion y camara

            G - Guardar en el archivo seleccionado
            H - Cargar del archivo seleccionado
            
            -------------modo transformacion-----------------
            Se sigue usando las mismas teclas para transformar los ejes respectivos
            Usa la consola para cambiar de elemento o de modo de transformacion (trasladar, rotar, escalar)


            Presiona cualquier tecla para continuar...
        """;
        System.Console.WriteLine(controles);
        System.Console.ReadLine();
        System.Console.Clear();
        Console.WriteLine("Consola de Juego inicializada correctamente. Escribe 'ayuda' para ver comandos");
    }
}
