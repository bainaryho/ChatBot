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
    public class IndustryDialog : IDialog<string>
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

            actions.Add(new CardAction() { Title = "1. 모집학과&전형일정", Value = "1", Type = ActionTypes.ImBack });
            actions.Add(new CardAction() { Title = "2. 지원자격", Value = "2", Type = ActionTypes.ImBack });
            actions.Add(new CardAction() { Title = "3. 제출서류", Value = "3", Type = ActionTypes.ImBack });
            actions.Add(new CardAction() { Title = "4. 선발방법&합격발표", Value = "4", Type = ActionTypes.ImBack });
            actions.Add(new CardAction() { Title = "5. 안내사항", Value = "5", Type = ActionTypes.ImBack });
            actions.Add(new CardAction() { Title = "6. 문의사항 연락처", Value = "6", Type = ActionTypes.ImBack });
            actions.Add(new CardAction() { Title = "이전으로", Value = "0", Type = ActionTypes.ImBack });


            message.Attachments.Add(                    //Create Hero Card & attachment
               new HeroCard
               {
                   Title = "산업체위탁전형 탭입니다. 메뉴를 선택해주세요!\n" +
               "산업체 위탁 원서 접수 : 2022.01.05 ~ 2022.01.26 오후 3시",
                   Buttons = actions
               }.ToAttachment()
           );

            await context.PostAsync(message);
            context.Wait(IndustrySelect);
        }

        public async Task IndustrySelect(IDialogContext context,
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
                SQLHelper.PulsQuery("update ChoiceData set count = count+1 Where name = 'inderstySchedule'"); //DB의 열에 +1
                actions.Add(new CardAction()
                {
                    Title = "상세보기"
,
                    Value = "https://www.inhatc.ac.kr/Web-home/plugin/pdfjs/web/viewer.html?file=%2Fsites%2Fipsi%2Fatchmnfl%2Fviewer%2F18%2F%2Ftemp_1644997995733100.tmp#page=3&zoom=auto,-31,841",
                    Type = ActionTypes.ShowImage
                }); 
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
                    Title = "전형일정",
                    Text = "● 원서 접수 : 2022.02.19(토) ~02.20(일) 13:00 ~ 14:00 까지\n" +
                    "2022.02.21(월) ~02.23(수) 10:00 ~ 15:00 까지\n" +
                      "● 서류 제출 : 접수기간 내 방문접수 및 제출[접수처 : 우리대학 5호관 B103호 입학팀]\n" +
                      "● 합격자 발표 : 2022.02.24(목) 14:00 예정\n" +
                      "● 등록 기간 : 2022.02.24(목) ~ 02.25(금) 15:00 까지\n"
                }.ToAttachment()
                );
                //context.Call(new previousDialog(), DialogResumeAfter);
                message.Attachments.Add(                    //Create Hero Card & attachment
                    new HeroCard
                    {
                        Title = "모집학과&전형일정 상세보기",
                        Buttons = actions
                    }.ToAttachment()
                    ); ;
                context.Wait(exit);
            }
            else if (strSelected == "2")
            {
                SQLHelper.PulsQuery("update ChoiceData set count = count+1 Where name = 'inderstyQualification'"); //DB의 열에 +1
                actions.Add(new CardAction()
                {
                    Title = "상세 보기"
,
                    Value = "https://www.inhatc.ac.kr/Web-home/plugin/pdfjs/web/viewer.html?file=%2Fsites%2Fipsi%2Fatchmnfl%2Fviewer%2F18%2F%2Ftemp_1644997995733100.tmp#page=4&zoom=auto,-31,841",
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
                 Title = "경력(재직 9개월 이상)",
                 Text = "● 산업체 재직경력 9개월 이상인자\n" +
                        " ※ 학기 개시일 기준(2022.3.1.) 4대 사회보험(국민, 건강, 고용, 산재) 가입경력 합산\n " +
                        " - 4대 사회보험 가입경력은 고등학교 졸업 또는 검정고시 합격일 이후의 경력만 반영 \n"
             }.ToAttachment()
             );
                message.Attachments.Add(
               new HeroCard
               {
                   Title = "비경력(재직 9개월 미만)",
                   Text = "● 산업체 재직경력 9개월 미만인자\n" +
                          "※ 특성화고·마이스터고 졸업자, 일반고 졸업자 중 위탁직업교육과정 이수자는 재직경력요건을 적용하지 않음(2022. 2월 졸업예정자 포함)\n" +
                          "단, 지원당시 산업체에 재직하고 있어야 하며 입학 시 협약을 맺은 산업체에서 6개월간 의무재직하여야함 (2022.9.1. 이전 퇴직 시 제적(퇴학)처리함)\n "
               }.ToAttachment()
               );
                message.Attachments.Add(
               new HeroCard
               {
                   Title = "유의사항",
                   Text = "● 위의 6개월, 9개월은 4대 보험 등 공적증명서상 취득일과 상실일 기준으로 산정함\n" +
                   "● 고등학교 재학기간 중 산업체 재직경력은 인정하지 않음\n" +
                   "● 산업체의 소재지(근무지)가 인천광역시 내 있거나, 행정구역은 다르나 근무지와 대학   간 실거리(직선거리 아님)가 70km 이내여야 함\n" +
                   "● 고용형태에 따른 지원불가 사유 ­ \n" +
                   "    - 일용근로자 및 1개월 소정근로시간 60시간 미만인 경우(단기 아르바이트 포함) ­ \n" +
                   "    - 가족회사 취업 등의 사유로 고용·산재보험에 가입되지 않은 경우  \n" +
                   "    - 추후 재직경력에 대한 근로소득 원천징수영수증 제출이 불가한 경우 \n"
               }.ToAttachment()
               );

                message.Attachments.Add(
                   new HeroCard
                   {
                       Title = "지원자격(경력 기준) 상세보기",
                       Buttons = actions
                   }.ToAttachment()
                   ); ;

                context.Wait(exit);

            }
            else if (strSelected == "3")
            {
                SQLHelper.PulsQuery("update ChoiceData set count = count+1 Where name = 'inderstyDocument'"); //DB의 열에 +1

                actions.Add(new CardAction()
                {
                    Title = "상세 보기"
,
                    Value = "https://www.inhatc.ac.kr/Web-home/plugin/pdfjs/web/viewer.html?file=%2Fsites%2Fipsi%2Fatchmnfl%2Fviewer%2F18%2F%2Ftemp_1644997995733100.tmp#page=5&zoom=auto,-31,841",
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
               Title = "전체공통",
               Text = "● 산업체 위탁교육 입학원서(지원자 서명 必) 1부\n" +
                      "● 산업체 위탁교육 협약서(산업체대표 직인 날인 必) 2부\n" +
                      "● 고등학교 졸업(예정)증명서 또는 검정고시 합격증명서 1부\n" +
                      "● 사업자등록증 사본(현 재직 산업체) 1부\n" +
                      "● 재직증명서(현 재직 산업체) 1부\n"
           }.ToAttachment()
           );


                message.Attachments.Add(
           new HeroCard
           {
               Title = "비경력 지원자 중위탁직업교육과정 이수자",
               Text = "● 고등학교 학교생활기록부 1부\n" +
                      "● 위탁직업교육 수료증 1부\n"
           }.ToAttachment()
           );
                message.Attachments.Add(
           new HeroCard
           {
               Title = "일반 산업체 재직자",
               Text = "● 건강보험 자격득실확인서(과거이력 포함) 1부\n" +
                      "● 국민연금 가입자 가입증명(과거이력 포함) 1부\n" +
                      "● 고용보험 자격이력내역서(근로자용)(과거이력 포함) 1부" +
                      "● 산재보험 자격이력내역서(근로자용)(과거이력 포함) 1부\n" +
                      "*건설업/벌목업 종사자 : 산업체 명의의 [고용/산재 가입증명원] 추가제출 "
           }.ToAttachment()
           );

                message.Attachments.Add(
           new HeroCard
           {
               Title = "공무원, 직업군인사립학교 교직원 등",
               Text = "● 연금법 적용 확인서 (재직기간 포함) 1부\n" +
                      "● 건강보험 자격득실확인서(과거이력 포함) 1부\n"
           }.ToAttachment()
           );
                message.Attachments.Add(
           new HeroCard
           {
               Title = "지원자 본인이영세 개인사업자인 경우",
               Text = "● 사업자등록증명, 소득금액증명, 납세증명(국세완납증명)  1부\n"
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
            else if (strSelected == "4")
            {
                SQLHelper.PulsQuery("update ChoiceData set count = count+1 Where name = 'inderstyPass'"); //DB의 열에 +1
                actions.Add(new CardAction()
                {
                    Title = "상세보기"
,
                    Value = "https://www.inhatc.ac.kr/Web-home/plugin/pdfjs/web/viewer.html?file=%2Fsites%2Fipsi%2Fatchmnfl%2Fviewer%2F18%2F%2Ftemp_1644997995733100.tmp#page=6&zoom=auto,-31,841",
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
                               Title = "선발방법",
                               Text = "● 무시험전형\n" +
                                      "● 모집인원 초과 시 선발기준\n" +
                                      "※ 1순위 - 현 직장 재직기준 장기근무자" +
                                      "※ 2순위 - 동일산업체 위탁생 수가 많은 경우" +
                                      "※ 3순위 - 동일고교 위탁생 수가 많은 경우" +
                                      "※ 4순위 - 연장자" +
                                      "※ 5순위 - 원서접수 순"
                           }.ToAttachment()
                           );

                message.Attachments.Add(
           new HeroCard
           {
               Title = "합격자 발표 및 등록",
               Text = "● 합격자 발표 - 2022.02.24(목) 14:00 예정\n" +
                      "● 등록금 납부 - 2022. 02.24.(목) ~ 2.25.(금) 오후 3시까지\n" +
                      "● 등록 절차 - 우리 대학 홈페이지 > 합격자 조회 > 합격증 및 등록금고지서 확인지정된 계좌번호로 등록금 납부\n" +
                      "● 등록 확인 - 우리 대학 홈페이지 > 합격자 조회 > 등록금 영수증 확인 (출력가능)\n" +
                      "● 추가합격자 발표 - 원서접수 시 입력한 번호로 개인별 전화통보- 등록금 납부 일정은 통보시 별도 안내함.\n" +
                      "● 비고 - 납부기간 내에 등록금을 납부하지 않으면 입학 허가가 취소됩니다.입금 부주의로 인한 불이익은 책임지지 않습니다.\n"
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
                SQLHelper.PulsQuery("update ChoiceData set count = count+1 Where name = 'inderstyGuide'"); //DB의 열에 +1
                actions.Add(new CardAction()
                {
                    Title = "입학포기 및 등록금 반환 / 불합격 처리 및 입학허가 취소 / 전형료 / 등록금 / 학사관리 / 유의사항"
,
                    Value = "https://www.inhatc.ac.kr/Web-home/plugin/pdfjs/web/viewer.html?file=%2Fsites%2Fipsi%2Fatchmnfl%2Fviewer%2F18%2F%2Ftemp_1644997995733100.tmp#page=7&zoom=auto,-31,841",
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
                   Title = "입학포기 및 등록금 반환",
                   Text = "● 포기신청 - 우리대학 홈페이지(www.inhatc.ac.kr)\n" +
                          "● 신청마감 - 2022. 2.25.(금) 오후 5시까지(토요일‧일요일·공휴일은 온라인 신청은 가능하나 반환되지 않음)\n" +
                          "※마감시간 이후 입학포기 및 반환신청을 접수하지 않으며, 자퇴 처리함\n" +
                          "● 반환방법 - 본인이 원서접수 시 입력한 계좌번호로 입금\n" +
                          " 오전 9시 이전에 신청한 경우에는 오후 4시경에 반환 오전 9시 이후에 신청한 경우에는 익일 오후 4시경에 반환 예정(토요일‧일요일·공휴일은 제외)\n" +
                          "※ 한국장학재단 대출자는 장학재단 반환지침에 따름\n" +
                          "● 반환금액 - 등록금 전액 \n" +
                          "※ 입학 이후에는 <대학 등록금에 관한 규칙>에 따름\n" +
                          "● 유의사항 - 입학 포기원을 제출하시면 번복하거나 취소할 수 없으므로 이점 유의하시기 바랍니다.\n"
               }.ToAttachment()
               );
                message.Attachments.Add(
               new HeroCard
               {
                   Title = "불합격 처리 및 입학허가 취소",
                   Text = "● 지원자격 : 전형별 지원자격에 미달된 자\n" +
                          "    -제출서류를 사실과 다르게 임의로 정정 또는 변조한 자\n" +
                          "    -제출기한 내 필수 제출서류 미제출자 \n" +
                          "    -학력 조회결과 지원자격 미달 또는 허위 사실이 확인된 자 \n" +
                          "    -부정한 방법으로 입학(합격)한 사실이 확인된 경우, 입학한 후라도 입학(합격)을 취소함\n" +
                          "● 등록 : 등록기간 내 등록을 하지 않은 자(‘6. 합격자 발표 및 등록’ 참조)\n " +
                          " -추가합격자 중 연락두절 자\n"
               }.ToAttachment()
               );
                message.Attachments.Add(
               new HeroCard
               {
                   Title = "전형료",
                   Text = "● 산업체 위탁교육생 모집 : 전 학과 대상 30,000원\n"
               }.ToAttachment()
               );
                message.Attachments.Add(
               new HeroCard
               {
                   Title = "등록금",
                   Text = "● 전 학과 : 입학금 269,000원 / 수업료 3,320,000원 / 합계 3,589,000원 \n"
               }.ToAttachment()
               );
                message.Attachments.Add(
               new HeroCard
               {
                   Title = "학사관리",
                   Text = "● 재직여부 확인 : 매학기 시작 전월<매년 2월, 8월(연2회)> 4대 보험 증명서로 위탁교육생의 재직여부를 확인 함­ \n" +
                          "     - 신입학의 경우 입학원서 접수 시 제출한 서류로 대체\n " +
                          "     - 2학기에는 전년도 원천징수영수증과 4대 보험의 가입증명서와의 일치여부를 검증함\n" +
                          "● 제적처리 기준\n" +
                          "● 학사 운영\n"
               }.ToAttachment()
               );
                message.Attachments.Add(
               new HeroCard
               {
                   Title = "유의사항",
                   Text = "● 입학원서 등 제출 서류의 기재사항이 사실과 다를 경우에는 합격 또는 입학을 취소할 수 있음\n" +
                          "● 접수된 입학원서(제출서류)는 반환하지 않음\n" +
                          "● 별도 확인이 필요할 경우 추가서류의 제출을 요구할 수 있음\n" +
                          "● 모집요강에 명시되지 아니한 사항은 [산업체위탁교육 심의위원회]에서 결정\n"
               }.ToAttachment()
               );
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
            else if (strSelected == "6")
            {
                SQLHelper.PulsQuery("update ChoiceData set count = count+1 Where name = 'inderstyPhone'"); //DB의 열에 +1
                actions.Add(new CardAction()
                {
                    Title = "상세 보기"
   ,
                    Value = "https://www.inhatc.ac.kr/Web-home/plugin/pdfjs/web/viewer.html?file=%2Fsites%2Fipsi%2Fatchmnfl%2Fviewer%2F18%2F%2Ftemp_1644997995733100.tmp#page=8&zoom=auto,-31,642",
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
                       Title = "문의사항 연락처",
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
                context.Call(new IndustryDialog(), null);
            }
        }


    }
}