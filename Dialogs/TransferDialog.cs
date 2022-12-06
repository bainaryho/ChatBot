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
    public class TransferDialog : IDialog<string>
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

            actions.Add(new CardAction() { Title = "1. 모집인원", Value = "1", Type = ActionTypes.ImBack });
            actions.Add(new CardAction() { Title = "2. 전형일정 & 지원자격 & 제출서류", Value = "2", Type = ActionTypes.ImBack });
            actions.Add(new CardAction() { Title = "3. 성적 반영 방법 & 면접고사", Value = "3", Type = ActionTypes.ImBack });
            actions.Add(new CardAction() { Title = "4. 합격자 발표 및 등록", Value = "4", Type = ActionTypes.ImBack });
            actions.Add(new CardAction() { Title = "5. 입학 포기/등록금 반환/불합격 처리/입학허가 취소", Value = "5", Type = ActionTypes.ImBack });
            actions.Add(new CardAction() { Title = "6. 전형료, 등록금 및 장학제도", Value = "6", Type = ActionTypes.ImBack });
            actions.Add(new CardAction() { Title = "7. 지원자 유의사항", Value = "7", Type = ActionTypes.ImBack });
            actions.Add(new CardAction() { Title = "8. 편입학 전형 FAQ ", Value = "8", Type = ActionTypes.ImBack });
            actions.Add(new CardAction() { Title = "이전으로", Value = "0", Type = ActionTypes.ImBack });


            message.Attachments.Add(                    //Create Hero Card & attachment
               new HeroCard
               {
                   Title = "편입학 탭입니다. 메뉴를 선택해주세요!\n" +
               "편입학 원서 접수 : 2022.01.10 ~ 2022.01.26",
                   Buttons = actions
               }.ToAttachment()
           );

            await context.PostAsync(message);
            context.Wait(TransferSelect);
        }

        public async Task TransferSelect(IDialogContext context,
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
                SQLHelper.PulsQuery("update ChoiceData set count = count+1 Where name = 'transferPerson'"); //DB의 열에 +1
                actions.Add(new CardAction()
                {
                    Title = "상세 보기"
,
                    Value = "https://www.inhatc.ac.kr/Web-home/plugin/pdfjs/web/viewer.html?file=%2Fsites%2Fipsi%2Fatchmnfl%2Fviewer%2F28%2F%2Ftemp_1639532947239100.tmp#page=3&zoom=auto,-31,841",
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
                        Title = "모집인원 상세보기",
                        Buttons = actions
                    }.ToAttachment()
                    ); ;
                context.Wait(exit);

            }
            else if (strSelected == "2")
            {
                SQLHelper.PulsQuery("update ChoiceData set count = count+1 Where name = 'transferDocument'"); //DB의 열에 +1
                actions.Add(new CardAction()
                {
                    Title = "상세 보기"
,
                    Value = "",
                    Type = ActionTypes.ShowImage
                }); ;
                actions.Add(new CardAction()
                {
                    Title = "이전으로"
,
                    Value = "이전으로",
                    Type = ActionTypes.ImBack
                }); ;
                message.Attachments.Add(
             new HeroCard
             {
                 Title = "1차 전형일정",
                 Text = "● 원서접수 - 2022.01.10.(월) 오전 10시부터 01.26.(수) 오후 3시까지 / www.jinhakapply.com\n" +
                 "<원서접수 완료 후 반드시 수험표 상에서 지원학과 및 제출 서류 등의 내용을 확인하시기 바랍니다.>\n " +
                        "● 제출서류마감 - 2022.01.28.(금) 오후 3시까지 도착분 / (22212)인천광역시미추홀구인하로100인하공업전문대학입학팀\n" +
                        "● 면접일시(인문사회계열) - 2022.02.05.(토)<세부사항추후별도안내> / 별도 안내\n" +
                        "● 합격자발표 2022.02.15.(화) 오전 10시 예정 / 우리 대학 홈페이지\n" +
                        "● 등록기간 - 2022.02.15.(화) 오전 10시부터 ~ 02.18.(금) 오후 3시까지 / 고지서내지정계좌로입금\n"
             }.ToAttachment()
             );
                message.Attachments.Add(
             new HeroCard
             {
                 Title = "2차 전형일정",
                 Text = "● 원서접수 - 2022.2.18.(금)~2.21.(월) 오전 10시 까지 / www.jinhakapply.com\n" +
                 "<원서접수 완료 후 반드시 수험표 상에서 지원학과 및 제출 서류 등의 내용을 확인하시기 바랍니다.>\n " +
                        "● 제출서류마감 - 2022.2.22.(화) 오후 1시 까지 도착 분 / (22212)인천광역시미추홀구인하로100인하공업전문대학입학팀\n" +
                        "● 면접일시(인문사회계열) - 2022.02.05.(토)<세부사항추후별도안내> / 별도 안내\n" +
                        "● 합격자발표 2022.2.25.(금) 오전 11시 예정 / 우리 대학 홈페이지\n" +
                        "● 등록기간 - 2022.2.25.(금)오전 11시부터~오후 3시까지 / 고지서 내 지정 계좌로 입금\n"
             }.ToAttachment()
             );
                message.Attachments.Add(
               new HeroCard
               {
                   Title = "지원자격",
                   Text = "2학년 1학기\n" +
                          "● 국내 전문대학 또는 4년제 대학 졸업(예정)자\n" +
                          "● 국내 4년제대학 1학년 과정 이상 수료자(40학점이상취득필수)\n" +
                          "● 타 전문대학자퇴(제적) 후 2학기가 경과된자로서 1학년 과정 이상 수료자(40학점이상취득필수)\n" +
                          "● 시간제 등록 또는 학점은행제에 의한 학점 취득자 중 1학년 과정 40학점 이상 취득자\n" +
                          "● 타 법령에 의하여 위와 동등 이상의 자격이 있다고 인정되는 자\n",
               }.ToAttachment()
               );
                message.Attachments.Add(
               new HeroCard
               {
                   Title = "제출서류",
                   Text = "● 졸업(예정)자 - 입학원서 , 졸업(예정)증명서 / 성적증명서\n" +
                          "● 4년제대학수료자 - 입학원서, 수료증명서, 성적증명서\n" +
                          "● 전문대학 제적(자퇴)자 - 입학원서, 성적증명서, 자퇴(제적)증명서\n"
               }.ToAttachment()
               );
                message.Attachments.Add(
                   new HeroCard
                   {
                       Title = "전형일정 & 지원자격 & 제출서류 상세보기",
                       Buttons = actions
                   }.ToAttachment()
                   ); ;

                context.Wait(exit);
            }
            else if (strSelected == "3")
            {
                SQLHelper.PulsQuery("update ChoiceData set count = count+1 Where name = 'transferRecord'"); //DB의 열에 +1
                actions.Add(new CardAction()
                {
                    Title = "상세 보기"
,
                    Value = "https://www.inhatc.ac.kr/Web-home/plugin/pdfjs/web/viewer.html?file=%2Fsites%2Fipsi%2Fatchmnfl%2Fviewer%2F28%2F%2Ftemp_1639532947239100.tmp#page=5&zoom=auto,-31,765",
                    Type = ActionTypes.ShowImage
                }); ;
                actions.Add(new CardAction()
                {
                    Title = "이전으로"
,
                    Value = "이전으로",
                    Type = ActionTypes.ImBack
                }); ;
                message.Attachments.Add(
           new HeroCard
           {
               Title = "성적 반영 및 선발 방법",
               Text = "● 전형 요소별 배점 - 계열(학과) / 전형요소별 반영점수 / 총점\n" +
                      "● 출신 대학 성적 반영 - 전적(출신) 대학 전 학년 성적증명서의 백분위 점수를 본교 반영점수로 환산 , 모든 성적은 소수점 아래 3자리에서 반올림\n" +
                      "● 선발 방법 - 선발기준 1. 총점 순위에 따라 선발 2. 지우너자격 심사 결과 자격미달, 서류 미비 등 결격 사유가 있을 경우 불합격 처리, 성적이 동점일 경우에는 동점자처리 기준에 의해 선발/ 동점자 처리 기준\n"
           }.ToAttachment()
           );


                message.Attachments.Add(
           new HeroCard
           {
               Title = "면접 고사",
               Text = "● 대상학과 - 인문사회계열\n" +
                      "● 면접고사 - 학업 수행 능력과 수험생의 품성 및 태도 등을 평가\n" +
                      "● 면접고사기간 - 2022.02.05(토) , 개별 세부면접 시간은 추후 별도 공지 예정\n" +
                      "● 준비물(필수) - 1. 수험표(원서접수 사이트에서 출력) 2. 사진이 부착된 신분증 중 택1(신분증상에 주민등록번환 생년월일이 기재되어 있어야 함)\n" +
                      "● 복장 - 단정한 면접복장(정장)\n"
           }.ToAttachment()
           );
                //context.Call(new previousDialog(), DialogResumeAfter);
                message.Attachments.Add(                    //Create Hero Card & attachment
                   new HeroCard
                   {
                       Title = "전형일정 상세보기",
                       Buttons = actions
                   }.ToAttachment()
                   ); ;
                context.Wait(exit);

            }
            else if (strSelected == "4")
            {
                SQLHelper.PulsQuery("update ChoiceData set count = count+1 Where name = 'transferPass'"); //DB의 열에 +1
                actions.Add(new CardAction()
                {
                    Title = "상세보기"
,
                    Value = "https://www.inhatc.ac.kr/Web-home/plugin/pdfjs/web/viewer.html?file=%2Fsites%2Fipsi%2Fatchmnfl%2Fviewer%2F28%2F%2Ftemp_1639532947239100.tmp#page=6&zoom=auto,-31,841",
                    Type = ActionTypes.ShowImage
                }); ;
                actions.Add(new CardAction()
                {
                    Title = "이전으로"
,
                    Value = "이전으로",
                    Type = ActionTypes.ImBack
                }); ;
                message.Attachments.Add(
           new HeroCard
           {
               Title = "1차 합격자 발표 및 등록",
               Text = "● 합격자 발표 - 발표일시:2022.02.15.(화) 오전 10시 예정\n" +
                        "-발표방법 : 우리 대학 홈페이지 (www.inhatc.ac.kr)\n" +
                      "● 등록금 납부 - 납부기간 : 2022.02.15.(화) 오전 10시 부터 02.18.(금) 오후 3시까지\n" +
                      "● 등록절차 - 합격자는“합격증”및“등록금 납부고지서”를 우리 대학 홈페이지에서 출력하여 개인별로 부여된 지정 계좌로 등록금을 입금(이체) 하여야함\n" +
                      "● 등록확인 - 등록금 납부 후 반드시 우리 대학 홈페이지에서 등록 확인 해야함\n"
           }.ToAttachment()
           );
                message.Attachments.Add(
           new HeroCard
           {
               Title = "2차 합격자 발표 및 등록",
               Text = "● 합격자 발표 - 발표일시: 2022.2.25.(금) 오전 11시 예정\n" +
                        "-발표방법 : 우리 대학 홈페이지 (www.inhatc.ac.kr)\n" +
                      "● 등록금 납부 - 납부기간: 2022.2.25.(금) 오전 11시~ 오후 3시까지\n" +
                      "● 등록절차 - 합격자는“합격증”및“등록금 납부고지서”를 우리 대학 홈페이지에서 출력하여 개인별로 부여된 지정 계좌로 등록금을 입금(이체) 하여야함\n" +
                      "● 등록확인 - 등록금 납부 후 반드시 우리 대학 홈페이지에서 등록 확인 해야함\n"
           }.ToAttachment()
           );

                message.Attachments.Add(
           new HeroCard
           {
               Title = "1차 추가합격자 발표 및 등록",
               Text = "● 추가합격자 발표 및 등록 - 2022.02.21.(월)~02.25.(금)까지\n" +
                      "● 발표 방법 - 추가 합격대상자에게 개인별로 전화 통보 / 인터넷 원서 접수 시 기재한 본인 휴대전화번호, 자택전화번호, 추가전화번호(가족등)로  전화 통보하여 등록의사를 확인함\n" +
                      "추가 합격 통보시 아래의 경우는 등록포기로 간주하여 다음순위자에게 합격을 통보함 / 합격증 및 고지서 확인:우리 대학 홈페이지(www.inhatc.ac.kr) \n" +
                      "● 등록 절차 - 추가 합격자는“합격증 및 등록금납부고지서”를 우리 대학 홈페이지에서 출력하여 개인별  부여된 지정 계좌번호로 등록금을 입금(이체)하여야하며,등록기간내에 등록을 하지않으면   입학허가가 취소됨\n" +
                      "● 등록 확인 - 합격자 발표 및 등록’참조\n"
           }.ToAttachment()
           );
                message.Attachments.Add(
           new HeroCard
           {
               Title = "2차 추가합격자 발표 및 등록",
               Text = "● 추가합격자 발표 및 등록 - 2022.02.25.(금)~02.28.(월)까지\n" +
                      "● 발표 방법 - 추가 합격대상자에게 개인별로 전화 통보 / 인터넷 원서 접수 시 기재한 본인 휴대전화번호, 자택전화번호, 추가전화번호(가족등)로  전화 통보하여 등록의사를 확인함\n" +
                      "추가 합격 통보시 아래의 경우는 등록포기로 간주하여 다음순위자에게 합격을 통보함 / 합격증 및 고지서 확인:우리 대학 홈페이지(www.inhatc.ac.kr) \n" +
                      "● 등록 절차 - 추가 합격자는“합격증 및 등록금납부고지서”를 우리 대학 홈페이지에서 출력하여 개인별  부여된 지정 계좌번호로 등록금을 입금(이체)하여야하며,등록기간내에 등록을 하지않으면   입학허가가 취소됨\n" +
                      "● 등록 확인 - 합격자 발표 및 등록’참조\n"
           }.ToAttachment()
           );

                //context.Call(new previousDialog(), DialogResumeAfter);
                message.Attachments.Add(                    //Create Hero Card & attachment
                   new HeroCard
                   {
                       Title = "제출서류 상세보기",
                       Buttons = actions
                   }.ToAttachment()
                   ); ;
                context.Wait(exit);
            }
            else if (strSelected == "5")
            {
                SQLHelper.PulsQuery("update ChoiceData set count = count+1 Where name = 'transferReturn'"); //DB의 열에 +1

                actions.Add(new CardAction()
                {
                    Title = "입학 포기 및 등록금 반환"
,
                    Value = "https://www.inhatc.ac.kr/Web-home/plugin/pdfjs/web/viewer.html?file=%2Fsites%2Fipsi%2Fatchmnfl%2Fviewer%2F28%2F%2Ftemp_1639532947239100.tmp#page=7&zoom=auto,-31,841",
                    Type = ActionTypes.ShowImage
                }); ;
                actions.Add(new CardAction()
                {
                    Title = "불합격 처리 및 입학허가 취소"
,
                    Value = "https://www.inhatc.ac.kr/Web-home/plugin/pdfjs/web/viewer.html?file=%2Fsites%2Fipsi%2Fatchmnfl%2Fviewer%2F28%2F%2Ftemp_1639532947239100.tmp#page=7&zoom=auto,-31,841",
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
                       Title = "입학 포기 및 등록금 반환 & 불합격 처리 및 입학허가 취소",
                       Buttons = actions
                   }.ToAttachment()
                   ); ;
                context.Wait(exit);

            }
            else if (strSelected == "6")
            {
                SQLHelper.PulsQuery("update ChoiceData set count = count+1 Where name = 'transferCash'"); //DB의 열에 +1
                message.Attachments.Add(
               new HeroCard
               {
                   Title = "전형료",
                   Text = "● 편입학 - 전 학과 - 전형료 30,000원\n"
               }.ToAttachment()
               );

                message.Attachments.Add(
            new HeroCard
            {
                Title = "등록금 및 장학제도",
                Text = "● 전년도 (2021학년도) 입학금 및 한학기 수업료 - 전 학과 / 3,648,000원 \n" +
                     "● 장학제도 - 입학금 전액 면제 / 기타 장학제도는 우리 대학 규정 및 해당 기관 지침에 의거 하여 지급\n"
            }.ToAttachment()
            );
                actions.Add(new CardAction()
                {
                    Title = "상세 보기"
,
                    Value = "https://www.inhatc.ac.kr/Web-home/plugin/pdfjs/web/viewer.html?file=%2Fsites%2Fipsi%2Fatchmnfl%2Fviewer%2F28%2F%2Ftemp_1639532947239100.tmp#page=8&zoom=auto,-31,841",
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
                       Title = "전형료, 등록금 및 장학제도",
                       Buttons = actions
                   }.ToAttachment()
                   ); ;
                context.Wait(exit);
            }
            else if (strSelected == "7")
            {
                SQLHelper.PulsQuery("update ChoiceData set count = count+1 Where name = 'transferOK'"); //DB의 열에 +1
                message.Attachments.Add(
          new HeroCard
          {
              Title = "지원자 유의사항",
              Text = "● 합격자는 본 대학 홈페이지에서 합격증(등록금고지서포함)을 출력하고 등록에 관한 내용을 숙지한 후 기간 내 등록 하여야 함\n" +
                     "● 접수된 입학원서 및 제출 서류는 일절 반환하지 않으며, 별도 확인이 필요한 경우에는 추가 서류 제출을 요청 할 수 있고, 기간 내 서류를 제출하지 않으면 입학 사정대상에서 제외됨\n" +
                     "● 입학원서 등 제출서류의 기재사항이 사실과 다를경우에는 합격 또는 입학을 취소함\n" +
                     "● 모집요강에 구체적으로 명시되지 아니한 사항은 우리 대학 입학전형 관리위원회에서 심의함\n" +
                     "● 전문대학 졸업예정자가 전적 대학을 졸업하지 못한경우 합격을 취소함\n" +
                     "● 모든 전형이 종료된 후 입학 학기가 같은 2개 이상의 대학(4년제대학,산업대학,교육대학,전문대학)에이중학적을 금지하며,이를 위반시 입학이 취소함\n" +
                     "● 편입학 합격자는 입학학기 (2022학년도1학기)에 휴학불가\n" +
                     "● 고등교육법 시행령 제 15조에 의거, 타 대학 출신자의 전적 대학이 수학점 인정 범위는 졸업 필요학점의1/2을초과할 수 없음\n"
          }.ToAttachment()
          ); ;
                actions.Add(new CardAction()
                {
                    Title = "상세 보기"
,
                    Value = "https://www.inhatc.ac.kr/Web-home/plugin/pdfjs/web/viewer.html?file=%2Fsites%2Fipsi%2Fatchmnfl%2Fviewer%2F28%2F%2Ftemp_1639532947239100.tmp#page=8&zoom=auto,-31,841",
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
                       Title = "지원자 유의사항",
                       Buttons = actions
                   }.ToAttachment()
                   ); ;
                context.Wait(exit);

            }
            else if (strSelected == "8")
            {
                SQLHelper.PulsQuery("update ChoiceData set count = count+1 Where name = 'transferFAQ'"); //DB의 열에 +1
                actions.Add(new CardAction()
                {
                    Title = "상세보기"
,
                    Value = "https://www.inhatc.ac.kr/Web-home/plugin/pdfjs/web/viewer.html?file=%2Fsites%2Fipsi%2Fatchmnfl%2Fviewer%2F28%2F%2Ftemp_1639532947239100.tmp#page=9&zoom=auto,-31,841",
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
                        Title = "편입학 전형 FAQ",
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
                context.Call(new TransferDialog(), null);
            }
        }


    }
}