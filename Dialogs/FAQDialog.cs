using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Threading.Tasks;           //Add to process Async Task
using Microsoft.Bot.Connector;          //Add for Activity Class
using Microsoft.Bot.Builder.Dialogs;    //Add for Dialog Class

using System.Threading;
using QnAMakerDialog.Models;
using QnAMakerDialog;

namespace GreatWall
{
    [Serializable]

    [QnAMakerService("https://greatwallqnas-843d.azurewebsites.net/qnamaker",
        "40120c6e-b42b-415e-8dff-45bbe1780a67", "78398334-23b5-46ec-a9e6-09461554d19b", MaxAnswers = 5)]
    public class FAQDialog : QnAMakerDialog<string>
    {
        //public async Task StartAsync(IDialogContext context)
        //{
        //    await context.PostAsync("FAQ Service: ");
        //    context.Wait(MessageReceivedAsync);
        //}

        //public async Task MessageReceivedAsync(IDialogContext context,
        //                                       IAwaitable<object> result)
        //{
        //    Activity activity = await result as Activity;

        //    if (activity.Text.Trim() == "Exit")
        //    {
        //        context.Done("Order Completed");
        //    }
        //    else
        //    {
        //        await context.PostAsync("FAQ Dialog.");    //return our reply to the user
        //        context.Wait(MessageReceivedAsync);
        //    }
        //}

        public override async Task NoMatchHandler(IDialogContext context, string originalQueryText)
        {
            await context.PostAsync($"Sorry, I couldn't find an answer for '{originalQueryText}'. ");

            context.Wait(MessageReceived);
        }

        public override async Task DefaultMatchHandler(IDialogContext context, string originalQuertText, QnAMakerResult result)
        {
            if (originalQuertText == "Exit")
            {
                context.Done("");
                return;
            }
            await context.PostAsync(result.Answers.First().Answer);

            context.Wait(MessageReceived);
        }

        [QnAMakerResponseHandler(0.5)]
        public async Task LowScoreHandler(IDialogContext context, string originalQueryText, QnAMakerResult result)
        {
            var messageActivity = ProcessResultAndCreateMessageActivity(context, ref result);
            messageActivity.Text = $"I found an answer that might help..." +
                                   $"{result.Answers.First().Answer}.";

            await context.PostAsync(messageActivity);
            context.Wait(MessageReceived);
        }
    }
}