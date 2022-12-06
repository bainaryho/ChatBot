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
    public class JungsiDialog : IDialog<string>
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
            actions.Add(new CardAction() { Title = "2. 전형일정 & 면접고사", Value = "2", Type = ActionTypes.ImBack });
            actions.Add(new CardAction() { Title = "3. 지원자격", Value = "3", Type = ActionTypes.ImBack });
            actions.Add(new CardAction() { Title = "4. 제출서류", Value = "4", Type = ActionTypes.ImBack });
            actions.Add(new CardAction() { Title = "5. 성적 반영 방법", Value = "5", Type = ActionTypes.ImBack });
            actions.Add(new CardAction() { Title = "6. 합격자 발표 및 충원", Value = "6", Type = ActionTypes.ImBack });
            actions.Add(new CardAction() { Title = "7. 원서 접수 비용", Value = "7", Type = ActionTypes.ImBack });
            actions.Add(new CardAction() { Title = "8. 안내 사항 ", Value = "8", Type = ActionTypes.ImBack });
            actions.Add(new CardAction() { Title = "9. 전년도 입시결과", Value = "9", Type = ActionTypes.ImBack });
            actions.Add(new CardAction() { Title = "이전으로", Value = "0", Type = ActionTypes.ImBack });


            message.Attachments.Add(                    //Create Hero Card & attachment
               new HeroCard
               {
                   Title = "정시전형 탭입니다. 메뉴를 선택해주세요!\n" +
               "정시 원서접수 2022.12.29 ~ 2023.01.12"
               ,
                   Buttons = actions
               }.ToAttachment()
           );

            await context.PostAsync(message);
            context.Wait(JungsiSelect);
        }

        public async Task JungsiSelect(IDialogContext context,
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
                    Value = "https://ipsi.inhatc.ac.kr/Web-home/plugin/pdfjs/web/viewer.html?file=%2Fsites%2Fipsi%2Fatchmnfl%2Fviewer%2F15%2F%2Ftemp_1654674907706100.tmp#page=8&zoom=auto,-223,842",
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
                        Title = "모집인원 입니다.",
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
                    Value = "https://ipsi.inhatc.ac.kr/Web-home/plugin/pdfjs/web/viewer.html?file=%2Fsites%2Fipsi%2Fatchmnfl%2Fviewer%2F15%2F%2Ftemp_1654674907706100.tmp#page=10&zoom=auto,-15,842",
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
                       Title = "전형일정 & 면접고사 입니다.",
                       Text = "● 원서 접수 : 2022.12.29 (목) ~ 2023.01.12(목) 22:00 까지\n" +
                                "● 서류제출(해당자) : 2023.01.13 (금) 17:00 까지\n" +
                                "● 면접예약 : 2023.01.16 (월) 13:00 ~ 17:00\n" +
                                "● 면접고사 : 2023.01.28 (토)"
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
                    Value = "https://ipsi.inhatc.ac.kr/Web-home/plugin/pdfjs/web/viewer.html?file=%2Fsites%2Fipsi%2Fatchmnfl%2Fviewer%2F15%2F%2Ftemp_1654674907706100.tmp#page=11&zoom=auto,-15,842",
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
                       Title = "일반고",
                       Text = "● 1. 일반고(인문계)의 일반과정 졸업(예정)자\n" +
                       "일반고 졸업(예정)자 중 위탁직업교육을 이수한 경우 포함\n" +
                       "일반고 졸업(예정)자 중 중점교육(체육, 음악, 미술 등)을 이수한 경우 포함\n" +
                       "● 2. 종합고, 학력인정평생교육시설, 대안학교 등의 일반과정 졸업(예정)자\n" +
                       "● 3. 특수목적고의 과학고, 국제고, 외국어고 졸업(예정)자\n" +
                       "● 4. 검정고시 출신자"
                   }.ToAttachment()
                   );
                message.Attachments.Add(                    //Create Hero Card & attachment
                   new HeroCard
                   {
                       Title = "특성화고",
                       Text = "● 1. 특성화고(전문계)의 전무과정 졸업(예정)자\n" +
                                "● 2. 종합고, 학력인정 평생교육시설, 대안학교 등의 전무과정 졸업(예정)자\n" +
                                "● 3. 특수목적고의 예술고, 체육고, 마이스터고 졸업(예정)자"
                   }.ToAttachment()
                   );
                message.Attachments.Add(                    //Create Hero Card & attachment
                   new HeroCard
                   {
                       Title = "특기자(어학)",
                       Text = "● 공학계열, 예능계열   TOEIC:600점이상   TOEIC Speaking: 120점 이상  TOEIC(iBT): 65점 TEPS(New) : 258점 이상 JPT : 600점 이상   HSK : 4급 이상\n" +
                                "● 인문사회계열   TOEIC:700점이상   TOEIC Speaking: 130점 이상  TOEIC(iBT): 75점 TEPS(New) : 300점 이상 JPT : 700점 이상   HSK : 5급 이상"
                   }.ToAttachment()
                   );
                message.Attachments.Add(                    //Create Hero Card & attachment
                   new HeroCard
                   {
                       Title = "정시",
                       Text = "● 2023학년도 대학수학능력시험 응시자"
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
            else if (strSelected == "4")
            {
                SQLHelper.PulsQuery("update ChoiceData set count = count+1 Where name = 'document'"); //DB의 열에 +1
                actions.Add(new CardAction()
                {
                    Title = "차트 보기"
,
                    Value = "https://ipsi.inhatc.ac.kr/Web-home/plugin/pdfjs/web/viewer.html?file=%2Fsites%2Fipsi%2Fatchmnfl%2Fviewer%2F15%2F%2Ftemp_1654674907706100.tmp#page=13&zoom=auto,-15,842",
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
                message.Attachments.Add(
                    new HeroCard
                    {
                        Title = "전문대졸 이상",
                        Text = "● 전문대학, 일반(4년제)대학 졸업(예정)자 : 졸업(예정)증명서 1부\n" +
                                "● 일반(4년제)대학 2학년 이상 수료자: 수료, 성적증명서 각 1부\n" +
                                "● 해외대학 졸업자 : 졸업, 성적증명서 각 1부\n" +
                                "※ 졸업, 성적증명서 모두 아포스티유 확인서 또는 영사확인을 받아 제출\n" +
                                "※ 영어 이외에 외국어로 되어 있는 서류는 한국어 또는 영어로 번역하여 공증을 받아 제출"
                    }.ToAttachment()
                    );
                message.Attachments.Add(
                   new HeroCard
                   {
                       Title = "기타",
                       Text = "● 성명 불일치자 : 주민등록 초본 1부\n" +
                                "※ 성명정정 사항 및 주민등록번호 표기\n" +
                                "● 수능성적 온라인 수신 실패자 : 정시 지원자 중 대학수학능력시험 온라인 제공 미수신자는 수능 성적표 원본을 제출하여야 함"
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
                    Value = "https://ipsi.inhatc.ac.kr/Web-home/plugin/pdfjs/web/viewer.html?file=%2Fsites%2Fipsi%2Fatchmnfl%2Fviewer%2F13%2F%2Ftemp_1635721183237100.tmp#page=15&zoom=auto,-15,842",
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
                       Title = "성적반영 방법 입니다.",
                       Buttons = actions
                   }.ToAttachment()
                   ); ;
                context.Wait(exit);

            }
            else if (strSelected == "6")
            {
                SQLHelper.PulsQuery("update ChoiceData set count = count+1 Where name = 'pass'"); //DB의 열에 +1
                actions.Add(new CardAction()
                {
                    Title = "차트 보기"
,
                    Value = "https://ipsi.inhatc.ac.kr/Web-home/plugin/pdfjs/web/viewer.html?file=%2Fsites%2Fipsi%2Fatchmnfl%2Fviewer%2F15%2F%2Ftemp_1654674907706100.tmp#page=17&zoom=auto,-15,842",
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
                       Title = "합격자 발표 및 등록",
                       Text = "● 합격자 발표 : 2023.02.06 (월)\n" +
                                "● 추가 합격자 발표 : 2023.02.09 (월)~ 02.28 (화)\n" +
                                "● 추가 합격자 선발방버\n" +
                                "-전형별 모집단위에서 미달될 경우 타 학과에서 추가 선발\n" +
                                "-전형 전체에서 미달될 경우 타 전형에서 추가 선발(농어촌 <-> 수급자)"
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
                message.Attachments.Add(                    //Create Hero Card & attachment
                   new HeroCard
                   {
                       Title = "상세보기",
                       Buttons = actions
                   }.ToAttachment()
                   );
                context.Wait(exit);
            }
            else if (strSelected == "8")
            {
                SQLHelper.PulsQuery("update ChoiceData set count = count+1 Where name = 'previous'"); //DB의 열에 +1
                actions.Add(new CardAction()
                {
                    Title = "차트 보기"
                ,
                    Value = "https://ipsi.inhatc.ac.kr/Web-home/plugin/pdfjs/web/viewer.html?file=%2Fsites%2Fipsi%2Fatchmnfl%2Fviewer%2F15%2F%2Ftemp_1654674907706100.tmp#page=19&zoom=auto,-15,842",
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
                message.Attachments.Add(                    //Create Hero Card & attachment
                   new HeroCard
                   {
                       Title = "상세보기",
                       Buttons = actions
                   }.ToAttachment()
                   );
                context.Wait(exit);

            }
            else if (strSelected == "9")
            {
                SQLHelper.PulsQuery("update ChoiceData set count = count+1 Where name = 'guide'"); //DB의 열에 +1
                actions.Add(new CardAction()
                {
                    Title = "차트 보기"
,
                    Value = "https://ipsi.inhatc.ac.kr/Web-home/plugin/pdfjs/web/viewer.html?file=%2Fsites%2Fipsi%2Fatchmnfl%2Fviewer%2F15%2F%2Ftemp_1654674907706100.tmp#page=21&zoom=auto,-15,768",
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
                       Title = "전년도 입시결과 입니다.",
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
                context.Call(new JungsiDialog(), null);
            }
        }
    }
}