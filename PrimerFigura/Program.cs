using PrimerFigura;

internal class Program
{
    private static void Main(string[] args)
    {
        // obtener la resolucion de la pantalla y usar ese para crear la ventana

        using(Game jueguito = new Game(800, 600, "Primer Figura"))
        {
            jueguito.Run();
        }
    }
}