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

namespace GreatWall.Dialogs
{
    [Serializable]
    public class DeepCourseDialog : IDialog<string>
    {
        string strMessage;
        public async Task StartAsync(IDialogContext context)
        {
            await this.MessageReceivedAsync(context, null);
        }

        private async Task MessageReceivedAsync(IDialogContext context,
                                               IAwaitable<object> result)
        {
            var message = context.MakeMessage();
            var actions = new List<CardAction>();

            actions.Add(new CardAction() { Title = "1. 학사학위 안내", Value = "1", Type = ActionTypes.ImBack });
            actions.Add(new CardAction() { Title = "2. 모집인원 & 전형일정", Value = "2", Type = ActionTypes.ImBack });
            actions.Add(new CardAction() { Title = "3. 지원자격 & 제출서류", Value = "3", Type = ActionTypes.ImBack });
            actions.Add(new CardAction() { Title = "4. 선발방법 & 면접고사", Value = "4", Type = ActionTypes.ImBack });
            actions.Add(new CardAction() { Title = "5. 합격자 발표 & 입학포기", Value = "5", Type = ActionTypes.ImBack });
            actions.Add(new CardAction() { Title = "6. 안내사항", Value = "6", Type = ActionTypes.ImBack });
            actions.Add(new CardAction() { Title = "7. 관련학과 기준표", Value = "7", Type = ActionTypes.ImBack });
            actions.Add(new CardAction() { Title = "8. 전공심화 2차 ", Value = "10", Type = ActionTypes.ImBack });


            message.Attachments.Add(                    //Create Hero Card & attachment
               new HeroCard
               {
                   Title = "전공심화과정 탭입니다. 메뉴를 선택해주세요!\n" +
               "전공심화 원서접수 1차 : 2022.01.10 ~ 2022.01.26 \n" +
               "전공심화 원서접수 2차 : 2022.02.21 ~ 2022.02.22"
               ,
                   Buttons = actions
               }.ToAttachment()
           );

            await context.PostAsync(message);
            context.Wait(DeepCourseSelect);
        }

        public async Task DeepCourseSelect(IDialogContext context,
                                     IAwaitable<object> result)
        {
            Activity activity = await result as Activity;
            string strSelected = activity.Text.Trim();
            var message = context.MakeMessage();
            var actions = new List<CardAction>();

            if (strSelected == "0")
            {
                context.Done("");

            }
            else if (strSelected == "1")
            {
                SQLHelper.PulsQuery("update ChoiceData set count = count+1 Where name = 'person'"); //DB의 열에 +1
                actions.Add(new CardAction()
                {
                    Title = "차트 보기"
,
                    Value = "https://ipsi.inhatc.ac.kr/Web-home/plugin/pdfjs/web/viewer.html?file=%2Fsites%2Fipsi%2Fatchmnfl%2Fviewer%2F21%2F%2Ftemp_1635720793289100.tmp#page=3&zoom=auto,-15,841",
                    Type = ActionTypes.ShowImage
                }); ;
                actions.Add(new CardAction()
                {
                    Title = "이전으로"
,
                    Value = "이전으로",
                    Type = ActionTypes.ImBack
                }); ;
                //context.Call(new previousDialog(), DialogResumeAfter);
                message.Attachments.Add(                    //Create Hero Card & attachment
                    new HeroCard
                    {
                        Title = "학사학위 안내 입니다.",
                        Buttons = actions
                    }.ToAttachment()
                    ); ;
                context.Wait(exit);

            }
            else if (strSelected == "2")
            {
                SQLHelper.PulsQuery("update ChoiceData set count = count+1 Where name = 'qualification'"); //DB의 열에 +1
                actions.Add(new CardAction()
                {
                    Title = "차트 보기"
,
                    Value = "https://ipsi.inhatc.ac.kr/Web-home/plugin/pdfjs/web/viewer.html?file=%2Fsites%2Fipsi%2Fatchmnfl%2Fviewer%2F21%2F%2Ftemp_1635720793289100.tmp#page=4&zoom=auto,-15,841",
                    Type = ActionTypes.ShowImage
                }); ;
                actions.Add(new CardAction()
                {
                    Title = "이전으로"
,
                    Value = "이전으로",
                    Type = ActionTypes.ImBack
                }); ;
                
                //context.Call(new previousDialog(), DialogResumeAfter);
                message.Attachments.Add(                    //Create Hero Card & attachment
                   new HeroCard
                   {
                       Title = "모집인원",
                       Text = "● 컴퓨터시스템공학과 : 20명\n" +
                               "● 컴퓨터정보공학과 : 25명\n" +
                               "● 실내건축학과 : 20명\n" +
                               "● 건축학과 : 20명\n" +
                               "● 자동차공학과 : 15명\n" +
                               "● 건설환경공학과 : 15명"
                   }.ToAttachment()
                   ); ;
                message.Attachments.Add(                    //Create Hero Card & attachment
   new HeroCard
   {
       Title = "전형일정 1차",
       Text = "● 원서접수 : 2022.01.10 (월) 10:00 ~ 01.26 (수) 15:00까지\n" +
                "● 서류제출 : 2022.01.28 (금) 15:00까지\n" +
                "● 면접일시 : 2022.02.05 (토) 13:00 예정\n" +
                "● 합격발표 : 2022.02.11 (금) 10:00 예정\n" +
                "● 등록기간 : 2022.02.11 (금) ~ 02.16 (수) 15:00까지"
   }.ToAttachment()
   ); ;
                message.Attachments.Add(                    //Create Hero Card & attachment
   new HeroCard
   {
       Title = "상세보기",
       Buttons = actions
   }.ToAttachment()
   );
                context.Wait(exit);
            }
            else if (strSelected == "3")
            {
                SQLHelper.PulsQuery("update ChoiceData set count = count+1 Where name = 'schedule'"); //DB의 열에 +1
                actions.Add(new CardAction()
                {
                    Title = "차트 보기"
,
                    Value = "https://ipsi.inhatc.ac.kr/Web-home/plugin/pdfjs/web/viewer.html?file=%2Fsites%2Fipsi%2Fatchmnfl%2Fviewer%2F21%2F%2Ftemp_1635720793289100.tmp#page=5&zoom=auto,-15,841",
                    Type = ActionTypes.ShowImage
                }); ;
                actions.Add(new CardAction()
                {
                    Title = "이전으로"
,
                    Value = "이전으로",
                    Type = ActionTypes.ImBack
                }); ;
                //context.Call(new previousDialog(), DialogResumeAfter);
                message.Attachments.Add(                    //Create Hero Card & attachment
                   new HeroCard
                   {
                       Title = "지원자격 입니다",
                       Buttons = actions
                   }.ToAttachment()
                   ); ;
                message.Attachments.Add(                    //Create Hero Card & attachment
                  new HeroCard
                  {
                      Title = "제출서류",
                      Text = "● 우리 대학 졸업(예정)자 : 없음\n" +
                               "● 타 대학 졸업(예정)자 : 졸업(예정)증명서 1부, 성적증명서 1부\n" +
                               "● 4년제 대학 수료자 : 수료증명서 1부, 성적증명서 1부"
                  }.ToAttachment()
                  ); ;
                message.Attachments.Add(                    //Create Hero Card & attachment
   new HeroCard
   {
       Title = "상세보기",
       Buttons = actions
   }.ToAttachment()
   );
                context.Wait(exit);

            }
            else if (strSelected == "4")
            {
                SQLHelper.PulsQuery("update ChoiceData set count = count+1 Where name = 'document'"); //DB의 열에 +1
                actions.Add(new CardAction()
                {
                    Title = "차트 보기"
,
                    Value = "https://ipsi.inhatc.ac.kr/Web-home/plugin/pdfjs/web/viewer.html?file=%2Fsites%2Fipsi%2Fatchmnfl%2Fviewer%2F21%2F%2Ftemp_1635720793289100.tmp#page=6&zoom=auto,-15,841",
                    Type = ActionTypes.ShowImage
                }); ;
                actions.Add(new CardAction()
                {
                    Title = "이전으로"
,
                    Value = "이전으로",
                    Type = ActionTypes.ImBack
                }); ;
                //context.Call(new previousDialog(), DialogResumeAfter);
                message.Attachments.Add(                    //Create Hero Card & attachment
                   new HeroCard
                   {
                       Title = "선발방법",
                       Text = "● 총점 순위에 따라 선발\n" +
                                "- 전적대학 성적 : 70점, 면점고사 : 30점"
                   }.ToAttachment()
                   ); ;
                message.Attachments.Add(                    //Create Hero Card & attachment
                   new HeroCard
                   {
                       Title = "면접고사",
                       Text = "● 면접고사 : 지원자가 모집정원을 초과하는 경우에 시행\n" +
                                "● 평가내용 : 학업수행능력과 수험생의 품성 및 태도 등을 평가\n" +
                                "● 면접일시 : 2022.02.05 (토) 13:00부터 진행 예정\n" +
                                "● 준비물 : 수험표, 사진이 부착된 신분증\n" +
                                "● 복장 : 단정한 자유복"
                   }.ToAttachment()
                   );
                message.Attachments.Add(                    //Create Hero Card & attachment
   new HeroCard
   {
       Title = "상세보기",
       Buttons = actions
   }.ToAttachment()
   );
                context.Wait(exit);

            }
            else if (strSelected == "5")
            {
                SQLHelper.PulsQuery("update ChoiceData set count = count+1 Where name = 'record'"); //DB의 열에 +1
                actions.Add(new CardAction()
                {
                    Title = "차트 보기"
,
                    Value = "https://ipsi.inhatc.ac.kr/Web-home/plugin/pdfjs/web/viewer.html?file=%2Fsites%2Fipsi%2Fatchmnfl%2Fviewer%2F21%2F%2Ftemp_1635720793289100.tmp#page=7&zoom=auto,-15,841",
                    Type = ActionTypes.ShowImage
                }); ;
                actions.Add(new CardAction()
                {
                    Title = "이전으로"
,
                    Value = "이전으로",
                    Type = ActionTypes.ImBack
                }); ;
                //context.Call(new previousDialog(), DialogResumeAfter);
                message.Attachments.Add(                    //Create Hero Card & attachment
                   new HeroCard
                   {
                       Title = "합격자발표",
                       Text = "● 합격자 발표 : 2022.02.11 (금)\n" +
                                "● 등록금 납부 : 2022.02.11 (금) ~ 02.16 (수) 15:00까지\n" +
                                "● 발표방법 : 우리 대학 입학 홈페이지\n" +
                                "● 추가합격자 발표 : 2022.02.16 (수) ~"
                   }.ToAttachment()
                   ); ;
                message.Attachments.Add(                    //Create Hero Card & attachment
                   new HeroCard
                   {
                       Title = "입학포기",
                       Text = "● 포기신청 : 우리대학 홈페이지\n" +
                                "● 신청마감 : 2022.02.24 (목) 11:00시까지\n" +
                                "● 반환방법 : 반환 신청은 본인이 원서접수 시 입력한 계좌번호로 입금\n" +
                                "● 반환금액 : 등록금 전액"
                   }.ToAttachment()
                   );
                message.Attachments.Add(                    //Create Hero Card & attachment
   new HeroCard
   {
       Title = "상세보기",
       Buttons = actions
   }.ToAttachment()
   );
                context.Wait(exit);
            }
            else if (strSelected == "6")
            {
                SQLHelper.PulsQuery("update ChoiceData set count = count+1 Where name = 'pass'"); //DB의 열에 +1
                actions.Add(new CardAction()
                {
                    Title = "차트 보기"
,
                    Value = "https://ipsi.inhatc.ac.kr/Web-home/plugin/pdfjs/web/viewer.html?file=%2Fsites%2Fipsi%2Fatchmnfl%2Fviewer%2F21%2F%2Ftemp_1635720793289100.tmp#page=8&zoom=auto,-15,841",
                    Type = ActionTypes.ShowImage
                }); ;
                actions.Add(new CardAction()
                {
                    Title = "이전으로"
,
                    Value = "이전으로",
                    Type = ActionTypes.ImBack
                }); ;
                //context.Call(new previousDialog(), DialogResumeAfter);
                message.Attachments.Add(                    //Create Hero Card & attachment
                   new HeroCard
                   {
                       Title = "전형료",
                       Text = "● 학사학위 전공심화과정(전 학과) : 30000원"
                   }.ToAttachment()
                   ); ;
                message.Attachments.Add(                    //Create Hero Card & attachment
                   new HeroCard
                   {
                       Title = "등록금",
                       Text = "● 입학금 : 378000원\n" +
                                "● 수업료 : 3270000원\n" +
                                "● 합계 : 3648000원"
                   }.ToAttachment()
                   );
                message.Attachments.Add(                    //Create Hero Card & attachment
                   new HeroCard
                   {
                       Title = "입학학기 휴학",
                       Text = "● 신입생은 입학학기 휴학 불가"
                   }.ToAttachment()
                   );
                message.Attachments.Add(                    //Create Hero Card & attachment
                   new HeroCard
                   {
                       Title = "불합격 처리 및 입학허가 취소",
                       Text = "● 지원자격에 미달된 자\n" +
                                "● 면접고사에 결시한 자\n" +
                                "● 제출서류를 사실과 다르게 임의로 정정 또는 변조한 자\n" +
                                "● 제출기한 내 필수 제출서류 미제출차\n" +
                                "● 등록기간 내 등록확인 예치금 및 최종 등록금을 납부하지 않은 자\n" +
                                "● 추가합격자 중 연락두절 자\n" +
                                "● 학력 조회결과 지원자격 미달 또는 허위 사실이 확인 된자\n" +
                                "● 부정한 방법으로 입학(합격)한 사실이 확인된 경우에는 입학한 후라도 입학(합격)을 취소함"
                   }.ToAttachment()
                   );
                message.Attachments.Add(                    //Create Hero Card & attachment
                   new HeroCard
                   {
                       Title = "문의사항 연락처",
                       Text = "● 원서접수, 서류제출 등 : (032)870-2404~5\n" +
                                "● 국가장학금, 학자금대출 : (032)870-2641~3"
                   }.ToAttachment()
                   );
                message.Attachments.Add(                    //Create Hero Card & attachment
   new HeroCard
   {
       Title = "상세보기",
       Buttons = actions
   }.ToAttachment()
   );
                context.Wait(exit);
            }
            else if (strSelected == "7")
            {
                SQLHelper.PulsQuery("update ChoiceData set count = count+1 Where name = 'price'"); //DB의 열에 +1
                actions.Add(new CardAction()
                {
                    Title = "차트 보기"
,
                    Value = "https://ipsi.inhatc.ac.kr/Web-home/plugin/pdfjs/web/viewer.html?file=%2Fsites%2Fipsi%2Fatchmnfl%2Fviewer%2F15%2F%2Ftemp_1654674907706100.tmp#page=18&zoom=auto,-15,842",
                    Type = ActionTypes.ShowImage
                });
                actions.Add(new CardAction()
                {
                    Title = "이전으로"
,
                    Value = "이전으로",
                    Type = ActionTypes.ImBack
                }); ;
                //context.Call(new previousDialog(), DialogResumeAfter);
                message.Attachments.Add(                    //Create Hero Card & attachment
                   new HeroCard
                   {
                       Title = "관련학과 기준표 입니다.",
                       Buttons = actions
                   }.ToAttachment()
                   ); ;
                context.Wait(exit);
            }
            else if (strSelected == "8")
            {
                SQLHelper.PulsQuery("update ChoiceData set count = count+1 Where name = 'previous'"); //DB의 열에 +1
                actions.Add(new CardAction()
                {
                    Title = "차트 보기"
                ,
                    Value = "https://ipsi.inhatc.ac.kr/Web-home/plugin/pdfjs/web/viewer.html?file=%2Fsites%2Fipsi%2Fatchmnfl%2Fviewer%2F30%2F%2Ftemp_1644544195116100.tmp#page=1&zoom=auto,-15,842",
                    Type = ActionTypes.ShowImage
                }); ;
                actions.Add(new CardAction()
                {
                    Title = "이전으로"
 ,
                    Value = "이전으로",
                    Type = ActionTypes.ImBack
                }); ;
                //context.Call(new previousDialog(), DialogResumeAfter);
                message.Attachments.Add(                    //Create Hero Card & attachment
                    new HeroCard
                    {
                        Title = "전공심화 2차 입니다.",
                        Buttons = actions
                    }.ToAttachment()
                    ); ;
                context.Wait(exit);
            }
            await context.PostAsync(message);
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

        public async Task exit(IDialogContext context, IAwaitable<object> result)
        {
            Activity activity = await result as Activity;
            string strSelected = activity.Text.Trim();

            if (strSelected == "이전으로")
            {
                await this.MessageReceivedAsync(context, null);
            }
            else
            {
                context.Done("");
                context.Call(new DeepCourseDialog(), null);
            }
        }
    }
}