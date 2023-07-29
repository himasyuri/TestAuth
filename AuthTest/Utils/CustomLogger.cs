using AuthTest.Interfaces;

namespace AuthTest.Utils
{
    public class CustomLogger : ICustomLogger
    {
        private string _logsPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;

        public void Log(Exception ex)
        {
            Console.WriteLine(ex);
            Console.WriteLine(ex.Message);

            using (var sw = new StreamWriter(Path.Combine(_logsPath, "logs.txt"), true))
            {
                Console.WriteLine("=======================================================");
                Console.WriteLine("=======================================================");
                Console.WriteLine($"Time: {DateTime.UtcNow} Exception message: {ex.Message}");
                Console.WriteLine(ex.ToString());
                Console.WriteLine();
            }
        }
    }
}
