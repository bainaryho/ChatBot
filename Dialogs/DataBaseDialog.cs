using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Threading.Tasks;           //Add to process Async Task
using Microsoft.Bot.Connector;          //Add for Activity Class
using Microsoft.Bot.Builder.Dialogs;    //Add for Dialog Class
using System.Net.Http;                  //Add for internet
using GreatWall.Helpers;                //Add for CardHelper

using System.Data;                      //Add for DB Connection
using System.Data.SqlClient;            //Add for DB Connection
using GreatWall.Model;                  //Add for Model
using System.Web.Helpers;
using AdaptiveCards;
using System.Configuration;

namespace GreatWall
{
    [Serializable]
    public class DataBaseDialog : IDialog<string>
    {
        List<OrderItem> ranklist = new List<OrderItem>();   //Create list object
        public async Task StartAsync(IDialogContext context)
        {
            await this.MessageReceivedAsync(context, null);
        }

        private async Task MessageReceivedAsync(IDialogContext context,
                                               IAwaitable<object> result)
        {
            var actions = new List<CardAction>();
            var message = context.MakeMessage();
            actions.Add(new CardAction() { Title = "1. 입학전형 인기순위", Value = "1", Type = ActionTypes.ImBack });
            actions.Add(new CardAction() { Title = "2. 수시전형 인기검색어", Value = "2", Type = ActionTypes.ImBack });
            actions.Add(new CardAction() { Title = "3. 정시전형 인기검색어", Value = "3", Type = ActionTypes.ImBack });
            actions.Add(new CardAction() { Title = "4. 산업체위탁 인기검색어", Value = "4", Type = ActionTypes.ImBack });
            actions.Add(new CardAction() { Title = "5. 편입학 인기검색어", Value = "5", Type = ActionTypes.ImBack });
            actions.Add(new CardAction() { Title = "6. 전공심화 인기검색어", Value = "6", Type = ActionTypes.ImBack });
            actions.Add(new CardAction() { Title = "이전으로", Value = "0", Type = ActionTypes.ImBack });



            message.Attachments.Add(                    //Create Hero Card & attachment
                new HeroCard { Title = "인기검색어를 확인할 수 있습니다", Buttons = actions }.ToAttachment()
            );

            await context.PostAsync(message);           //return our reply to the user
            context.Wait(SendWelcomeMessageAsync);
        }
        public async Task SendWelcomeMessageAsync(IDialogContext context,
                                       IAwaitable<object> result)
        {
            var actions = new List<CardAction>();
            var message = context.MakeMessage();
            Activity activity = await result as Activity;
            string strSelected = activity.Text.Trim();
            string RankPage = "SELECT korea FROM choicedata where kind='page' order by count DESC offset 0 rows fetch next 5 rows only";
            string Ranking1 = "SELECT count FROM choicedata where  kind='page' order by count DESC";
            string RankPage1 = "SELECT korea FROM choicedata where kind='page1' order by count DESC offset 0 rows fetch next 5 rows only";
            string RankPage3 = "SELECT korea FROM choicedata where kind='page3' order by count DESC offset 0 rows fetch next 5 rows only";
            string RankPage4 = "SELECT korea FROM choicedata where kind='page4' order by count DESC offset 0 rows fetch next 5 rows only";

            int rank = 1;
            if (strSelected == "0")
            {
                context.Done(context);
                context.Done(context);
                context.Call(new RootDialog(), null);
            }
            else if (strSelected == "1")
            {
                message.Attachments.Add(                    //Create Hero Card & attachment
                    new HeroCard { Title = "입학전형 인기순위입니다", Buttons = actions }.ToAttachment()
                );
                DataSet DB_DS1 = SQLHelper.RunSQL(RankPage);
                foreach (DataRow row in DB_DS1.Tables[0].Rows)
                {
                    message.Attachments.Add(
                     new HeroCard
                     {
                         Text = rank +"위 : <"+row["korea"].ToString()+">"
                     }.ToAttachment()
                     );
                    rank++;
                }
            }
            else if (strSelected == "2")
            {
                message.Attachments.Add(new HeroCard { Title = "수시전형 인기검색어입니다", }.ToAttachment());
                DataSet DB_DS2 = SQLHelper.RunSQL(RankPage1);
                foreach (DataRow row in DB_DS2.Tables[0].Rows)
                {
                    message.Attachments.Add(
                     new HeroCard
                     {
                         Text = rank + "위 : <" + row["korea"].ToString() + ">"
                     }.ToAttachment()
                     );
                    rank++;
                }
            }
            else if (strSelected == "3")
            {
                message.Attachments.Add(new HeroCard { Title = "정시전형 인기검색어입니다", }.ToAttachment());
                
            }
            else if (strSelected == "4")
            {
                message.Attachments.Add(new HeroCard { Title = "산업체전형 인기검색어입니다", }.ToAttachment());
                DataSet DB_DS2 = SQLHelper.RunSQL(RankPage3);
                foreach (DataRow row in DB_DS2.Tables[0].Rows)
                {
                    message.Attachments.Add(
                     new HeroCard
                     {
                         Text = rank + "위 : <" + row["korea"].ToString() + ">"
                     }.ToAttachment()
                     );
                    rank++;
                }
            }
            else if (strSelected == "5")
            {
                message.Attachments.Add(new HeroCard { Title = "편입학전형 인기검색어입니다", }.ToAttachment());
                DataSet DB_DS2 = SQLHelper.RunSQL(RankPage4);
                foreach (DataRow row in DB_DS2.Tables[0].Rows)
                {
                    message.Attachments.Add(
                     new HeroCard
                     {
                         Text = rank + "위 : <" + row["korea"].ToString() + ">"
                     }.ToAttachment()
                     );
                    rank++;
                }
            }

            await context.PostAsync(message);
            
        }
    }
}