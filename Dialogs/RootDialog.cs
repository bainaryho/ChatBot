using System;
using System.Threading.Tasks;

using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Dialogs;
using System.Collections.Generic;
using GreatWall.Dialogs;
using GreatWall.Helpers;
using System.Data.SqlClient;
using System.Data;
//Add for List<>

namespace GreatWall
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        protected int count = 1;
        string strMessage;
        private string strWelcomeMessage = "�޴��� �������ּ���";

        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
            //return Task.CompletedTask;
        }

        public async Task MessageReceivedAsync(IDialogContext context,
                                               IAwaitable<object> result)
        {
            //await context.PostAsync(strWelcomeMessage);    //return our reply to the user


            var message = context.MakeMessage();        //Create message
            var actions = new List<CardAction>();       //Create List

            actions.Add(new CardAction() { Title = "1. ��������", Value = "1", Type = ActionTypes.ImBack });
            actions.Add(new CardAction() { Title = "2. ��������", Value = "2", Type = ActionTypes.ImBack });
            actions.Add(new CardAction() { Title = "3. ���ü��Ź", Value = "3", Type = ActionTypes.ImBack });
            actions.Add(new CardAction() { Title = "4. ������", Value = "4", Type = ActionTypes.ImBack });
            actions.Add(new CardAction() { Title = "5. ������ȭ", Value = "5", Type = ActionTypes.ImBack });
            actions.Add(new CardAction() { Title = "6. ������ �α� �˻���", Value = "6", Type = ActionTypes.ImBack });

            message.Attachments.Add(                    //Create Hero Card & attachment
                new HeroCard { Title = "����ó�� ���� �������� ȯ���մϴ�.", Buttons = actions }.ToAttachment()
            );

            await context.PostAsync(message);           //return our reply to the user

            context.Wait(SendWelcomeMessageAsync);
        }

        public async Task SendWelcomeMessageAsync(IDialogContext context,
                                       IAwaitable<object> result)
        {
            Activity activity = await result as Activity;
            string strSelected = activity.Text.Trim();

            if (strSelected == "1")
            {
                //SQLHelper.PlusQuery("update UserChoice set susi = susi+1");
                context.Call(new SusiDialog(), DialogResumeAfter);
                SQLHelper.PulsQuery("update ChoiceData set count = count+1 Where name = 'susi'"); //DB�� ���� +1
            }
            else if (strSelected == "2")
            {
                context.Call(new JungsiDialog(), DialogResumeAfter);
                SQLHelper.PulsQuery("update ChoiceData set count = count+1 Where name = 'jungsi'"); //DB�� ���� +1
            }
            else if (strSelected == "3")
            {
                context.Call(new IndustryDialog(), DialogResumeAfter);
                SQLHelper.PulsQuery("update ChoiceData set count = count+1 Where name = 'indersty'"); //DB�� ���� +1

            }
            else if (strSelected == "4")
            {
                context.Call(new TransferDialog(), DialogResumeAfter);
                SQLHelper.PulsQuery("update ChoiceData set count = count+1 Where name = 'transfer'"); //DB�� ���� +1

            }
            else if (strSelected == "5")
            {
                context.Call(new DeepCourseDialog(), DialogResumeAfter);
                SQLHelper.PulsQuery("update ChoiceData set count = count+1 Where name = 'deepcourse'"); //DB�� ���� +1

            }
            else if(strSelected == "6")
            {
                context.Call(new DataBaseDialog(), DialogResumeAfter);
            }

            else
            {
                strMessage = "You have made a mistake. Please select again...";
                await context.PostAsync(strMessage);
                context.Wait(SendWelcomeMessageAsync);
            }
        }

        public async Task DialogResumeAfter(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                strMessage = await result;

                //await context.PostAsync(WelcomeMessage); ;
                await this.MessageReceivedAsync(context, result);
            }
            catch (TooManyAttemptsException)
            {
                await context.PostAsync("Error occurred....");
            }
        }

        public async Task AfterResetAsync(IDialogContext context, IAwaitable<bool> argument)
        {
            var confirm = await argument;
            if (confirm)
            {
                this.count = 1;
                await context.PostAsync("Reset count.");
            }
            else
            {
                await context.PostAsync("Did not reset count.");
            }
            context.Wait(MessageReceivedAsync);
        }
    }
}
