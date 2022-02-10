namespace VismaMeetingsTask.Handlers
{
    static class ErrorHandler
    {
        public static void Handle(Action fn)
        {
            try
            {
                fn();
            }
            catch (Exception ex)
            {
                Console.WriteLine("There was an error. Error message:");
                Console.WriteLine(ex.Message);
            }
        }
    }
}
