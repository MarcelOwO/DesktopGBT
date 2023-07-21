using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using OpenAI_API;
using OpenAI_API.Chat;

namespace DesktopGBT;

public class Command
{
    private static Dictionary<String, Action> commands;
    private OpenAIAPI aiApi;
    private List<ChatMessage> histroy;

    internal Command(OpenAIAPI aiapi,List<ChatMessage> histroy)
    {

        commands = new Dictionary<string, Action>
        {
            {"help", HandleHelp},
            {"clearConsole", HandleClearConsole},
            {"clearHistory", HandleClearHistory},
            {"listCommands", HandleListCommands},

        };
        this.aiApi = aiapi;
        this.histroy = histroy;
    }
    public static bool HandleInput(string input)
    {
        if (commands.TryGetValue(input, out var command))
        {
            command();
            return true;
        }
        else
        {
            return false;
        }
    }

    public static void HandleHelp()
    {
        Console.WriteLine("There is no help, lol");
        
    }
    public void HandleClearConsole()
    {
        Console.Clear();
        
        
    }
    public void HandleClearHistory()
    {
        histroy.Clear();
        
    }
    public static void HandleListCommands()
    {
        
        
    }


}
