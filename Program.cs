using System.Collections;
using System.Runtime.InteropServices;
using DesktopGBT;
using OpenAI_API;
using OpenAI_API.Chat;

if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
{
    return;
}

string? apiKey = String.Empty;

OpenAIAPI openAiapi;

if (!Crypt.DoesEnvironmentVariableExist("EncryptedApiKey"))
{
    while (true)
    {
        Console.WriteLine("Please enter the OpenAi API key:");
        
        apiKey = Console.ReadLine();
    
        openAiapi = new(new APIAuthentication(apiKey));
        
        if (!openAiapi.Auth.ValidateAPIKey().GetAwaiter().GetResult())
            Console.WriteLine("Key is incorrect, retry");
        else
            break;
    }
    var encryptionKey = Crypt.GenerateRandomString(32);
    var iv = Crypt.GenerateRandomString(16); 
    var encryptedKey = Crypt.Encrypt(apiKey, encryptionKey, iv);
        
    Environment.SetEnvironmentVariable("EncryptedApiKey", Convert.ToBase64String(encryptedKey), EnvironmentVariableTarget.User);
    Environment.SetEnvironmentVariable("EncryptionKey", encryptionKey, EnvironmentVariableTarget.User);
    Environment.SetEnvironmentVariable("Iv", iv, EnvironmentVariableTarget.User);
}
else
{
    var loadedEncryptedKey = Convert.FromBase64String(Environment.GetEnvironmentVariable("EncryptedApiKey", EnvironmentVariableTarget.User));
    var loadedEncryptionKey = Environment.GetEnvironmentVariable("EncryptionKey", EnvironmentVariableTarget.User);
    var loadedIv = Environment.GetEnvironmentVariable("Iv", EnvironmentVariableTarget.User);
    var decryptedKey = Crypt.Decrypt(loadedEncryptedKey, loadedEncryptionKey, loadedIv);
    apiKey = decryptedKey;
    openAiapi = new(new APIAuthentication(apiKey));
    
}


Console.WriteLine("Starting ChatGBT");
List<ChatMessage> history = new List<ChatMessage>();
Command com = new(openAiapi,history);
Console.WriteLine("Start the conversation:");




while (true)
{
    Console.WriteLine("_____________________________________________");

    Console.WriteLine("User: ");
    
    var input = Console.ReadLine();
    
    if(Command.HandleInput(input))
        continue;

    history.Add(new ChatMessage(ChatMessageRole.User, input));

    var t = await openAiapi.Chat.CreateChatCompletionAsync(history);
    
    Console.WriteLine($"assistant:\n{t.Choices[0]}");
    
    history.Add(new ChatMessage(ChatMessageRole.Assistant, t.Choices[0].ToString()));
    
}


























