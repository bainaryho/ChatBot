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
    public class SusiDialog : IDialog<string>
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
            actions.Add(new CardAction() { Title = "2. 지원자격", Value = "2", Type = ActionTypes.ImBack });
            actions.Add(new CardAction() { Title = "3. 전형일정", Value = "3", Type = ActionTypes.ImBack });
            actions.Add(new CardAction() { Title = "4. 제출서류", Value = "4", Type = ActionTypes.ImBack });
            actions.Add(new CardAction() { Title = "5. 성적 반영 방법", Value = "5", Type = ActionTypes.ImBack });
            actions.Add(new CardAction() { Title = "6. 합격자 선발 및 발표", Value = "6", Type = ActionTypes.ImBack });
            actions.Add(new CardAction() { Title = "7. 원서 접수 비용", Value = "7", Type = ActionTypes.ImBack });
            actions.Add(new CardAction() { Title = "8. 전년도 입시결과 ", Value = "8", Type = ActionTypes.ImBack });
            actions.Add(new CardAction() { Title = "9. 안내 사항", Value = "9", Type = ActionTypes.ImBack });
            actions.Add(new CardAction() { Title = "이전으로", Value = "0", Type = ActionTypes.ImBack });


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
            context.Wait(SusiSelect);
        }

        public async Task SusiSelect(IDialogContext context,
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
                    Title = "상세보기"
,
                    Value = "https://ipsi.inhatc.ac.kr/Web-home/plugin/pdfjs/web/viewer.html?file=%2Fsites%2Fipsi%2Fatchmnfl%2Fviewer%2F13%2F%2Ftemp_1654674809045100.tmp#page=8&zoom=auto,-12,661",
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
                SQLHelper.PulsQuery("update ChoiceData set count = count+1 Where name = 'qualification'"); //DB의 열에 +1

                actions.Add(new CardAction()
                {
                    Title = "상세 보기"
,
                    Value = "https://ipsi.inhatc.ac.kr/Web-home/plugin/pdfjs/web/viewer.html?file=%2Fsites%2Fipsi%2Fatchmnfl%2Fviewer%2F13%2F%2Ftemp_1654674809045100.tmp#page=11&zoom=auto,-12,661",
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
                 Title = "일반고",
                 Text = "● 일반고(인문계)의 일반 과정 졸업(예정)자\n" +
                        "● 일반고 졸업(예정)자 중 위탁직업교육을 이수한 경우\n" +
                        "● 일반고 졸업(예정)자 중 중점교육(체육,음악,미술)을 이수한 경우\n" +
                        "● 학력인정평생교육시설,대안학교 등의 일반과정 졸업(예정)자\n" +
                        "● 특수목적고의 과학고, 국제고, 외국어고 졸업(예정)자\n" +
                        "● 검정고시 출신자"
             }.ToAttachment()
             );
                message.Attachments.Add(
               new HeroCard
               {
                   Title = "특성화고",
                   Text = "● 특성화고(전문계)의 전문과정 졸업(예정)자\n" +
                          "● 학력인정평생교육시설,대안학교 등의 전문과정 졸업(예정)자\n" +
                          "● 특수목적고의 예술고,체육고,마이스터고 졸업(예정)자\n",
               }.ToAttachment()
               );
                message.Attachments.Add(
               new HeroCard
               {
                   Title = "특기자(대회 입상자)",
                   Text = "● 국제기능올림픽대회 한국위원회 기능경기대회 입상자 또는 국가\n" +
                          "   기술 자격을 취득한 자\n" +
                          "(단, 모집학과에서 요구하는 직종 또는 기술자격과 일치해야 함)"
               }.ToAttachment()
               );
                message.Attachments.Add(
           new HeroCard
           {
               Title = "특기자(어학)",
               Text = "● 일반고(인문계)의 일반 과정 졸업 또는 예정자\n" +
                      "● 일반고 졸업(예정자) 중 위탁직업교육을 이수한 경우\n" +
                      "● 일반고 졸업(예정자) 중 중점교육(체육,음악,미술)을 이수한 경우\n" +
                      "● 학력인정평생교육시설,대안학교 등의 일반과정 졸업(예정자)\n" +
                      "● 특수목적고의 과학고, 국제고, 외국어고 졸업 또는 예정자\n" +
                      "● 검정고시 출신자"
           }.ToAttachment()
           );

                message.Attachments.Add(
                   new HeroCard
                   {
                       Title = "지원자격 상세보기",
                       Buttons = actions
                   }.ToAttachment()
                   ); ;

                context.Wait(exit);
            }
            else if (strSelected == "3")
            {
                SQLHelper.PulsQuery("update ChoiceData set count = count+1 Where name = 'schedule'"); //DB의 열에 +1

                actions.Add(new CardAction()
                {
                    Title = "상세 보기"
,
                    Value = "https://ipsi.inhatc.ac.kr/Web-home/plugin/pdfjs/web/viewer.html?file=%2Fsites%2Fipsi%2Fatchmnfl%2Fviewer%2F13%2F%2Ftemp_1654674809045100.tmp#page=10&zoom=auto,-12,789",
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
               Title = "수시 1차",
               Text = "● 원서 접수 : 2022.09.13(화) ~10.06(목) 22:00 까지\n" +
                      "● 서류 제출 : 2022.10.07(금) 17:00까지\n" +
                      "● 면접 예약 : 2022.10.08(토) 13:00 ~ 17:00\n" +
                      "● 면접 고사 : 2022.10.15(토) ~ 2022.10.16(일)\n" +
                      "● 합격자 발표 : 2022.11.04(금) 16:00\n" +
                      "● 예치금 납부 : 2022.12.16(금) ~ 12.19(월) 16:00 까지\n" +
                      "● 최종 등록기간 : 2023.02.07(화) ~ 02.09(목) 16:00 까지\n" +
                      "● 추가합격자 발표 : 2022.12.19(월) ~ 12.28(수) 21:00 까지\n"
           }.ToAttachment()
           );


                message.Attachments.Add(
           new HeroCard
           {
               Title = "수시 2차",
               Text = "● 원서 접수 : 2022.11.07(월) ~11.21(월) 22:00 까지\n" +
                      "● 서류 제출 : 2022.11.23(수) 17:00까지\n" +
                      "● 면접 예약 : 2022.11.26(토) 13:00 ~ 17:00\n" +
                      "● 면접 고사 : 2022.12.03(토) ~ 2022.12.04(일)\n" +
                      "● 합격자 발표 : 2022.12.15(목) 16:00\n" +
                      "● 예치금 납부 : 2022.12.16(금) ~ 12.19(월) 16:00 까지\n" +
                      "● 최종 등록기간 : 2023.02.07(화) ~ 02.09(목) 16:00 까지\n" +
                      "● 추가합격자 발표 : 2022.12.19(월) ~ 12.28(수) 21:00 까지\n"
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
                SQLHelper.PulsQuery("update ChoiceData set count = count+1 Where name = 'document'"); //DB의 열에 +1

                actions.Add(new CardAction()
                {
                    Title = "상세보기"
,
                    Value = "https://ipsi.inhatc.ac.kr/Web-home/plugin/pdfjs/web/viewer.html?file=%2Fsites%2Fipsi%2Fatchmnfl%2Fviewer%2F13%2F%2Ftemp_1654674809045100.tmp#page=13&zoom=auto,-21,806",
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
               Title = "고등학교 졸업 예정자",
               Text = "● 고등학교 학교생활기록부 1부 (학교장 직인날인)\n" +
                      "● 학교생활기록부 온라인 제공 동의자는 제출할 필요 없음\n" +
                      "※ 단, 2014년 이전 고등학교 졸업자는 온라인 제공"
           }.ToAttachment()
           );

                message.Attachments.Add(
           new HeroCard
           {
               Title = "검정고시 출신자",
               Text = "● 검정고시 성적증명서 1부\n" +
                      "● 검정고시성적 온라인 제공 동의자는 제출할 필요 없음\n" +
                      "※ 단, 2015년 이전 및 2022년 2회차 검정고시 응시자는 온라인 제공 불가"
           }.ToAttachment()
           );

                message.Attachments.Add(
          new HeroCard
          {
              Title = "외국계고교 출신자",
              Text = "● 고등학교 졸업증명서, 성적증명서 각 1부\n" +
                     "● 졸업, 성적증명서 모두 아포스티유 확인서 또는 영사확인을 받아 제출\n" +
                     "● 영어 이외에 외국어로 되어 있는 서류는 한국어 또는 영어로 번역공증을 받아 제출\n" +
                     "※ 특기자(어학) 전형 지원자에 한함"
          }.ToAttachment()
          );
                message.Attachments.Add(
        new HeroCard
        {
            Title = "농어촌 유형 |",
            Text = "● 학교장 지원자격 확인서(우리 대학 양식) 1부\n" +
                  "● 중,고등학교 학교생활기록부 1부\n" +
                  "● 지원자 본인의 주민등록초본 1부\n" +
                  "● 부모의 주민등록초본 각 1부\n" +
                  "● 지원자 본인기준 가족관계증명서 1부\n" +
                  "● 부모가 사망한 경우 부 또는 모의 기본증명서 1부\n" +
                  "● 부모가 이혼한 경우\n" +
                  "   - 지원자 본인 명의 기본증명서 1부\n" +
                  "   - 부 또는 모의 기본증명서 또는 제적등본 1부\n" +
                  "     ( 추가로 제출 )"
        }.ToAttachment()
       );

                message.Attachments.Add(
new HeroCard
{
    Title = "농어촌 유형 ||",
    Text = "● 학교장 지원자격 확인서(우리 대학 양식) 1부\n" +
           "● 중,고등학교 학교생활기록부 1부\n" +
           "● 지원자 본인의 주민등록초본 1부\n"
}.ToAttachment()
);

                message.Attachments.Add(
new HeroCard
{
    Title = "수급자",
    Text = "● 수급자 : 수급자 증명서 1부\n" +
           "● 차상위 계층 ( 아래 제출서류 중 택1 하여 제출 )\n" +
           "     - 한부모 가족 증명서\n" +
           "     - 장애인 연금, 장애수당, 장애아동수당 대상자 확인서\n" +
           "     - 차상위 본인부담경감대상자 증명서 \n" +
           "     - 자활근로자 확인서\n" +
           "     - 우선돌봄 차상위 확인서\n" +
           "     - 차상위 계층 확인서\n" +
           "※ 지원자 본인 명의의 서류가 아닌 경우 주민등록등본 추가 제출\n" +
           "※ 제출 서류들은 원서접수 시작일 부터 발급한 서류만 인정\n" +
           "※ 지원 자격 확인을 위해 추가 자료제출을 요구 할 수 있음"
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
                SQLHelper.PulsQuery("update ChoiceData set count = count+1 Where name = 'record'"); //DB의 열에 +1

                actions.Add(new CardAction()
                {
                    Title = "성적반영배점 , 생기부 성적 반영 , 출결 가산점 , 특기자 성적 반영"
,
                    Value = "https://ipsi.inhatc.ac.kr/Web-home/plugin/pdfjs/web/viewer.html?file=%2Fsites%2Fipsi%2Fatchmnfl%2Fviewer%2F13%2F%2Ftemp_1654674809045100.tmp#page=14&zoom=auto,-21,829",
                    Type = ActionTypes.ShowImage
                }); ;
                actions.Add(new CardAction()
                {
                    Title = "기타 어학 성적 반영 , 생기부 등급 산출 방법"
,
                    Value = "https://ipsi.inhatc.ac.kr/Web-home/plugin/pdfjs/web/viewer.html?file=%2Fsites%2Fipsi%2Fatchmnfl%2Fviewer%2F13%2F%2Ftemp_1654674809045100.tmp#page=15&zoom=auto,-21,829",
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
                       Title = "성적반영 방법 상세보기",
                       Buttons = actions
                   }.ToAttachment()
                   ); ;
                context.Wait(exit);
            }
            else if (strSelected == "6")
            {
                SQLHelper.PulsQuery("update ChoiceData set count = count+1 Where name = 'pass'"); //DB의 열에 +1

                message.Attachments.Add(
       new HeroCard
       {
           Title = "합격자 발표",
           Text = "● 수시 1차 : 2022.11.04(금)\n" +
                  "● 수시 2차 : 2022.12.15(목)\n" +
                  "● 추가합격자 발표 : 2022.12.19(월) ~ 12.28(수)\n" +
                  "● 추가 합격자 선발 방법\n   " +
                  "    - 일반고 전형의 모집인원이 미달될 경우 특성화고 전형에서 추가 선발\n" +
                  "    - 특성화고 전형의 모집인원이 미달될 경우 일반고 전형에서 추가 선발\n" +
                  "    - 특기자(어학) 전형의 모집인원이 미달될 경우 일반고 전형에서 추가 선발\n"
       }.ToAttachment()
       );
                actions.Add(new CardAction()
                {
                    Title = "상세 보기"
,
                    Value = "https://ipsi.inhatc.ac.kr/Web-home/plugin/pdfjs/web/viewer.html?file=%2Fsites%2Fipsi%2Fatchmnfl%2Fviewer%2F13%2F%2Ftemp_1654674809045100.tmp#page=17&zoom=auto,-21,707",
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
                       Title = "합격자 선발 및 발표 상세보기",
                       Buttons = actions
                   }.ToAttachment()
                   ); ;
                context.Wait(exit);

            }
            else if (strSelected == "7")
            {
                SQLHelper.PulsQuery("update ChoiceData set count = count+1 Where name = 'price'"); //DB의 열에 +1

                message.Attachments.Add(
       new HeroCard
       {
           Title = "원서접수 비용",
           Text = "● 공학계열 및 산업디자인학과 : 전형료 30,000원 \n" +
                  "● 인문사회계열 및 패션디자인과 : 전형료 30,000원 / 면접고사료 20,000원\n"
       }.ToAttachment()
       );

                message.Attachments.Add(
      new HeroCard
      {
          Title = "전형료 감면 및 환불 기준",
          Text = "● 감면 : 국가보훈대상자 , 수급자 , 대학의 장이 면제를 인정한 자\n" +
                 "● 환불\n" +
                 "    - 입학전형에 응시한 사람이 착오로 과납한 경우 \n" +
                 "    - 대학의 귀책사유로 입학전형에 응시하지 못한 경우\n" +
                 "    - 천재지변으로 인하영 입학전형에 응시하지 못한 경우\n" +
                 "    - 질병 또는 사고 및 본인의 사망으로 입학전형에 응시하지 못한 경우\n" +
                 "    - 단계적으로 실시하는 입학전형에 응시하였으나 최종단계전에 불합격한 경우\n"
      }.ToAttachment()
      );
                message.Attachments.Add(
new HeroCard
{
   Title = "전형료 감면 및 환불 신청",
   Text = "● 수급자 전형 지원자 : 원서 접수 시 감면 \n" +
          "● 수급자 전형 외 전형료 감면 및 환불 대상자 : 증빙서류 제출 시 환불"
}.ToAttachment()
);
                actions.Add(new CardAction()
                {
                    Title = "상세 보기"
,
                    Value = "https://ipsi.inhatc.ac.kr/Web-home/plugin/pdfjs/web/viewer.html?file=%2Fsites%2Fipsi%2Fatchmnfl%2Fviewer%2F13%2F%2Ftemp_1654674809045100.tmp#page=18&zoom=auto,-21,838",
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
                       Title = "원서접수 비용 상세보기",
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
                    Title = "수시 1차 정원내 특별전형"
                ,
                    Value = "https://ipsi.inhatc.ac.kr/Web-home/plugin/pdfjs/web/viewer.html?file=%2Fsites%2Fipsi%2Fatchmnfl%2Fviewer%2F13%2F%2Ftemp_1654674809045100.tmp#page=21&zoom=auto,-21,662",
                    Type = ActionTypes.ShowImage
                }); ;
                actions.Add(new CardAction()
                {
                    Title = "수시 2차 정원내 특별전형"
,
                    Value = "https://ipsi.inhatc.ac.kr/Web-home/plugin/pdfjs/web/viewer.html?file=%2Fsites%2Fipsi%2Fatchmnfl%2Fviewer%2F13%2F%2Ftemp_1654674809045100.tmp#page=22&zoom=auto,-21,662",
                    Type = ActionTypes.ShowImage
                }); ;
                actions.Add(new CardAction()
                {
                    Title = "정원 외 특별전형"
,
                    Value = "https://ipsi.inhatc.ac.kr/Web-home/plugin/pdfjs/web/viewer.html?file=%2Fsites%2Fipsi%2Fatchmnfl%2Fviewer%2F13%2F%2Ftemp_1654674809045100.tmp#page=24&zoom=auto,-21,662",
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
                        Title = "전년도 입시 결과 상세보기",
                        Buttons = actions
                    }.ToAttachment()
                    ); ;
                context.Wait(exit);
            }
            else if (strSelected == "9")
            {
                SQLHelper.PulsQuery("update ChoiceData set count = count+1 Where name = 'guide'"); //DB의 열에 +1
                message.Attachments.Add(
       new HeroCard
       {
           Title = "등록금",
           Text = "● 입학금 : 269,000원 \n" +
                  "● 수업료 : 3,320,000원\n" +
                  "● 합계 : 3,589,000원"
       }.ToAttachment()
       );
                message.Attachments.Add(
new HeroCard
{
Title = "입학학기 휴학",
Text = "● 신입생은 입학학기 (2023년도 1학기) 휴학 불가 \n" +
  "● 4주이상의 입원치료를 요하는 질별 , 병역의무 , 임신/출산/육아의 경우 제외"
}.ToAttachment()
);
                message.Attachments.Add(
new HeroCard
{
Title = "장학제도",
Text = "● 전체수석 ( 신입생 입학전형 전체 수석 ): 입학금+수업료 전액\n" +
 "● 전체차석 ( 신입생 입학전형 전체 차석 ): 입학금+수업료 전액\n" +
 "● 특기자(어학)전형 성적 우수자 : 입학학기 수업료 전액\n" +
 "● 일반고 전형 성적 우수자 : 입학학기 수업료 전액\n" +
 "● 특성화고 전형 성적 우수자 : 입학학기 수업료 전액\n" +
 "● 일반전형 성적 우수자 : 입학학기 수업료 전액\n"
}.ToAttachment()
);

                message.Attachments.Add(
new HeroCard
{
Title = "불합격 및 입학허가 취소 기준",
Text = "● 우리 대학 동일 차수에 복수로 지원한 자\n" +
"● 면접고사에 결시한 자\n" +
"● 제출서류를 사실과 다르게 임의로 정정 또는 변조한 자\n" +
"● 제출기한 내 필수 제출서류 미제출자\n" +
"● 등록기간 내 등록확인 예치금 및 최종 등록금을 납부하지 않은 자\n" +
"● 추가합격자 중 연락두절 자\n" +
"● 학력 조회결과 지원자격 미달 또는 허위 사실이 확인된 자\n" +
"● 정시지원 위반자\n" +
"● 이중 등록한 자\n" +
"● 부정한 방법으로 입학한 사실이 확인된 경우 입학 후라도 입학을 취소함"
}.ToAttachment()
);

                actions.Add(new CardAction()
                {
                    Title = "상세보기"
,
                    Value = "https://ipsi.inhatc.ac.kr/Web-home/plugin/pdfjs/web/viewer.html?file=%2Fsites%2Fipsi%2Fatchmnfl%2Fviewer%2F13%2F%2Ftemp_1654674809045100.tmp#page=19&zoom=auto,-21,797",
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
                       Title = "안내사항 상세보기",
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

            if (strSelected =="이전으로")
            {
                await this.MessageReceivedAsync(context, null);
            }
            else
            {
                context.Done("");
                context.Call(new SusiDialog(), null);
            }
        }
    }
}