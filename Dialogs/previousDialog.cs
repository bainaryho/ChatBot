using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace GreatWall.Dialogs
{
    public class previousDialog : IDialog<string>
    {
        public async Task StartAsync(IDialogContext context)
        {
            await this.MessageReceivedAsync(context, null);
        }

        private async Task MessageReceivedAsync(IDialogContext context, object p)
        {
            var message = context.MakeMessage();
            var actions = new List<CardAction>();

            actions.Add(new CardAction()
            {

                Title = "차트 보기"
                ,
                Value = "https://ipsi.inhatc.ac.kr/Web-home/plugin/pdfjs/web/viewer.html?file=%2Fsites%2Fipsi%2Fatchmnfl%2Fviewer%2F13%2F%2Ftemp_1635721183237100.tmp#page=22&zoom=auto,-15,766",
                Type = ActionTypes.ShowImage
            }); ;


            message.Attachments.Add(                    //Create Hero Card & attachment
            new HeroCard
            {
                Title = "수시전형 탭입니다. 메뉴를 선택해주세요!\n" +
                "수시 1차 원서 접수 : 2022.09.13 ~ 2022.10.06\n" +
                "수시 2차 원서 접수 : 2022.11.07 ~ 2022.11.21"
                ,
                Buttons = actions
            }.ToAttachment()
            );
            await context.PostAsync(message);
        }
    }
}