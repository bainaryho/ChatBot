using System.Threading.Tasks;
using System.Web.Http;

using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Dialogs;
using System.Web.Http.Description;
using System.Net.Http;
using System.Linq;
using System;

namespace GreatWall
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// receive a message from a user and send replies
        /// </summary>
        /// <param name="activity"></param>
        [ResponseType(typeof(void))]
        public virtual async Task<HttpResponseMessage> Post([FromBody] Activity activity)
        {
            // check if activity is of type message
            if (activity != null && activity.GetActivityType() == ActivityTypes.Message)
            {
                await Conversation.SendAsync(activity, () => new RootDialog());
            }
            else
            {
                HandleSystemMessage(activity);
            }
            return new HttpResponseMessage(System.Net.HttpStatusCode.Accepted);
        }

        private Activity HandleSystemMessage(Activity message)
        {
            string messageType = message.GetActivityType();

            if (messageType == ActivityTypes.DeleteUserData)

            {

                // Implement user deletion here

                // If we handle user deletion, return a real message

            }

            else if (messageType == ActivityTypes.ConversationUpdate)

            {

                //ê���� ���� ��ȭ�� �Ǵ�.

                if (message.MembersAdded.Any(o => o.Id == message.Recipient.Id))

                {

                    var reply = message.CreateReply("�ȳ��ϼ���!\n\n ���ϰ��� ����ó ê�� �ΰ��̿���!^^\n\n �ΰ��̸� �ҷ��ּ���!");

                    ConnectorClient connector = new ConnectorClient(new Uri(message.ServiceUrl));

                    connector.Conversations.ReplyToActivityAsync(reply);

                }

            }

            else if (messageType == ActivityTypes.ContactRelationUpdate)

            {

                var test = "";

                // Handle add/remove from contact lists

                // Activity.From + Activity.Action represent what happened

            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }
    }
}