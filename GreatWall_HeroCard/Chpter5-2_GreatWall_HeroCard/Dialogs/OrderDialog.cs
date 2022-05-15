using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Threading.Tasks;           //Add to process Async Task
using Microsoft.Bot.Connector;          //Add for Activity Class
using Microsoft.Bot.Builder.Dialogs;    //Add for Dialog Class
using System.Net.Http;                  //Add for internet

namespace GreatWall
{
    [Serializable]
    public class OrderDialog : IDialog<string>
    {
        string strMessage;
        string strOrder;
        string strServerUrl = "http://localhost:3984/Images/";

        public async Task StartAsync(IDialogContext context)   
        {
            strMessage = null;
            strOrder = "[Order Menu List] \n";

            //Called MessageReceivedAsync() without user input message
            await this.MessageReceivedAsync(context, null);  
        }

        private async Task MessageReceivedAsync(IDialogContext context,
                                               IAwaitable<object> result)
        {
            if (result != null)
            {
                Activity activity = await result as Activity;

                if (activity.Text.Trim() == "Exit")
                {
                    await context.PostAsync(strOrder);    //return our reply to the user
                    strOrder = null;
                    context.Done("Order Completed");
                }
                else
                {
                    strMessage = string.Format("You ordered {0}.", activity.Text);
                    strOrder += activity.Text + "\n";
                    await context.PostAsync(strMessage);    //return our reply to the user

                    context.Wait(this.MessageReceivedAsync);
                }
            }
            else
            {
                strMessage = "[Food Order Menu] Select the menu you want to order.> ";
                await context.PostAsync(strMessage);    //return our reply to the user

                //Menu_01: 자장면 
                List<CardImage> memu01_images = new List<CardImage>();   //Create image object
                memu01_images.Add(new CardImage() { Url = this.strServerUrl + "menu_01.jpg" });

                //Create Button-01
                List<CardAction> memu01_Button = new List<CardAction>();   //Create Button object
                memu01_Button.Add(new CardAction()
                {
                    Title = "자장면",
                    Value = "자장면",
                    Type = ActionTypes.ImBack
                });

                //Create Hero Card-01
                HeroCard memu01_Card = new HeroCard()
                {
                    Title = "자장면",
                    Subtitle = "옛날 자장면",
                    Images = memu01_images,
                    Buttons = memu01_Button
                };

                //Menu_02: 짬뽕
                List<CardImage> memu02_images = new List<CardImage>();   //Create image object
                memu02_images.Add(new CardImage() { Url = this.strServerUrl + "menu_02.jpg" });

                //Create Button-02
                List<CardAction> memu02_Button = new List<CardAction>();   //Create Button object
                memu02_Button.Add(new CardAction()
                {
                    Title = "짬뽕",
                    Value = "짬뽕",
                    Type = ActionTypes.ImBack
                });

                //Create Hero Card-02
                HeroCard memu02_Card = new HeroCard()
                {
                    Title = "짬뽕",
                    Subtitle = "굴짬뽕",
                    Images = memu02_images,
                    Buttons = memu02_Button
                };

                //Menu_03: 탕수육
                List<CardImage> memu03_images = new List<CardImage>();   //Create image object
                memu03_images.Add(new CardImage() { Url = this.strServerUrl + "menu_03.jpg" });

                //Create Button-03
                List<CardAction> memu03_Button = new List<CardAction>();   //Create Button object
                memu03_Button.Add(new CardAction()
                {
                    Title = "탕수육",
                    Value = "탕수육",
                    Type = ActionTypes.ImBack
                });

                //Create Hero Card-03
                HeroCard memu03_Card = new HeroCard()
                {
                    Title = "탕수육",
                    Subtitle = "찹쌀 탕수육",
                    Images = memu03_images,
                    Buttons = memu03_Button
                };

                //Create Button-04
                List<CardAction> memu04_Button = new List<CardAction>();   //Create Button object
                memu04_Button.Add(new CardAction()
                {
                    Title = "Exit Order",
                    Value = "Exit",
                    Type = ActionTypes.ImBack
                });

                //Create Hero Card-04
                HeroCard memu04_Card = new HeroCard()
                {
                    Title = "Exit food order...",
                    Subtitle = null,
                    Buttons = memu04_Button
                };

                var message = context.MakeMessage();                    //Create message      
                message.Attachments.Add(memu01_Card.ToAttachment());    //Hero Card-01 attachment 
                message.Attachments.Add(memu02_Card.ToAttachment());    //Hero Card-02 attachment 
                message.Attachments.Add(memu03_Card.ToAttachment());    //Hero Card-03 attachment 
                message.Attachments.Add(memu04_Card.ToAttachment());    //Hero Card-04 attachment 
                await context.PostAsync(message);                       //Output message 

                context.Wait(this.MessageReceivedAsync);
            }
        }
    }
}