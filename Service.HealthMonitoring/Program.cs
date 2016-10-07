namespace MeetEric
{
    using Services;

    internal class Program : ServiceProgram
    {
        /// <summary>
        /// This is the entry point of the service host process.
        /// </summary>
        private static void Main()
        {
            RunStateless((context) => new HealthMonitoring(context));
        }
    }
}
