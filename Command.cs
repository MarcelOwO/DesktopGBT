using OpenAI_API;
using OpenAI_API.Chat;

namespace DesktopGBT;

public class Command
{
    private static Dictionary<string, Action>? _commands;
    private readonly List<ChatMessage> _histroy;
    private OpenAIAPI _aiApi;

    internal Command(OpenAIAPI aiapi, List<ChatMessage> histroy)
    {
        _commands = new Dictionary<string, Action>
        {
            {"help", HandleHelp},
            {"clearConsole", HandleClearConsole},
            {"clearHistory", HandleClearHistory},
            {"listCommands", HandleListCommands}
        };
        _aiApi = aiapi;
        _histroy = histroy;
    }

    public static bool HandleInput(string input)
    {
        if (_commands!.TryGetValue(input, out var command))
        {
            command();
            return true;
        }

        return false;
    }

    private static void HandleHelp()
    {
        Console.WriteLine("There is no help, lol");
    }

    private void HandleClearConsole()
    {
        Console.Clear();
    }

    private void HandleClearHistory()
    {
        _histroy.Clear();
    }

    private static void HandleListCommands()
    {
    }
}