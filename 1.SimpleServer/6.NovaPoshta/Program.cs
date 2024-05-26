namespace _6.NovaPoshta
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Привіт козаки!");
            NovaPoshtaService nps = new NovaPoshtaService();
            nps.GetAreas();
        }
    }
}
