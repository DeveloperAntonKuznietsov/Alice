using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using ApiAiSDK;
using ApiAiSDK.Model;


namespace Alice
{
    class Program
    {
        static TelegramBotClient bot;
        static ApiAi apiAi;


        static void Main(string[] args)
        {
            bot = new TelegramBotClient("593138289:AAGkHnmJfrlG-b2-1muYS2nKCPPHN5zhG0k");
            AIConfiguration configuration = new AIConfiguration("f558e47a291c47c1961653b8d8f802fd", SupportedLanguage.Russian);
            apiAi = new ApiAi(configuration);

            var me = bot.GetMeAsync().Result;

            bot.OnMessage += Bot_OnMessage;
            bot.OnCallbackQuery += Bot_OnCallbackQuery;

            Console.WriteLine(me.FirstName);
            bot.StartReceiving();
            Console.ReadLine();
            bot.StopReceiving();
        }

        private static void Bot_OnCallbackQuery(object sender, Telegram.Bot.Args.CallbackQueryEventArgs e)
        {

            
        }

        private  static  void Bot_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            var message = e.Message;

            if (message==null || message.Type != MessageType.Text)
                return;

            
            string name = $"{message.From.FirstName }{message.From.LastName}";

            Console.WriteLine($"{name} написал сообщение: { message.Text}");

            var responce = apiAi.TextRequest(message.Text);
            string answer = responce.Result.Fulfillment.Speech;
            if (answer == "")
            {
                answer = "Не понятно же...\nперефразируй";
            }
            Console.WriteLine("Ответ: " + answer);
            bot.SendTextMessageAsync(message.From.Id, answer);
        }

        
    }
}
