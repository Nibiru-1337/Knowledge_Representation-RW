#define DEBUG_TEXT
namespace RW_backend
{
	public static class Logger
	{
        public static void Log(string message)
		{
#if DEBUG && DEBUG_TEXT
			System.Console.WriteLine(message);
#endif
		}
    }
}
