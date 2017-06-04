using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using RuralBot.Dialogs;
using RuralBot.Models.Entity;

namespace RuralBot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        /// 
        public ConnectorClient Connector;
        //string strCurrentUrl;

        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            Connector = new ConnectorClient(new Uri(activity.ServiceUrl));

            if (activity.Type == ActivityTypes.Message && (activity.Text.ToLower().Contains("start") || activity.Text.ToLower().Contains("ask")))
            {
                //Con las siguientes 4 lineas mostramos mientras el bot cargue el siguiente mensaje el texto "escribiendo"
                var connector = new ConnectorClient(new Uri(activity.ServiceUrl));
                var isTypingReply = activity.CreateReply();
                isTypingReply.Type = ActivityTypes.Typing;
                await connector.Conversations.ReplyToActivityAsync(isTypingReply);

                //borramos el estado anterior cuando se pulse star
                activity.GetStateClient().BotState.DeleteStateForUser(activity.ChannelId, activity.From.Id);

                //enviamos intrucciones para reiniciar
                Activity replyToConversation = menuCard(activity);
                replyToConversation.Text = "Escribe \"start\" en cualquier momento para reiniciar el Bot";

                Services.RuralServices.Alquiler(new Ventas());

                await Connector.Conversations.ReplyToActivityAsync(replyToConversation);
                //strCurrentUrl = Url.Request.RequestUri.AbsoluteUri.Replace(@"api/messages", "");
                //arrancamos el dialog con el menu
                await Conversation.SendAsync(activity, () => new RootDialog());
            }

            //parate para webchat y skype
            else if (activity.Type == ActivityTypes.Message)
            {
                //Lineas para que salga que el bot esta escribiendo
                var connector = new ConnectorClient(new Uri(activity.ServiceUrl));
                var isTypingReply = activity.CreateReply();
                isTypingReply.Type = ActivityTypes.Typing;
                await connector.Conversations.ReplyToActivityAsync(isTypingReply);

                //strCurrentUrl = Url.Request.RequestUri.AbsoluteUri.Replace(@"api/messages", "");
                await Conversation.SendAsync(activity, () => new RootDialog());
            }
            else
            {
                var replyMessage = HandleSystemMessage(activity);
                if (replyMessage != null)
                {
                    replyMessage.Text = "Escribe \"start\" en cualquier momento para reiniciar el Bot";
                    await Connector.Conversations.ReplyToActivityAsync(replyMessage);
                }
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
                if (message.MembersAdded.Any(miembro => miembro.Id == message.Recipient.Id))
                {
                    Activity replyToConversation = menuCard(message);

                    return replyToConversation;
                }
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
                //Aqui va para que funcione en Skype

                Activity replyToConversation = menuCard(message);

                return replyToConversation;
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
        private Activity menuCard(Activity message)
        {
            //strCurrentUrl = Url.Request.RequestUri.AbsoluteUri.Replace(@"api/messages", "");
            var replyToConversation = message.CreateReply();
            replyToConversation.Recipient = message.From;
            replyToConversation.Type = "message";
            replyToConversation.Attachments = new List<Attachment>();
            //var strCard = $@"{strCurrentUrl}/{"Content/Images/Capture.PNG"}";

            var buttonList = new List<CardAction>();

            var btnInfo = new CardAction
            {
                Type = ActionTypes.PostBack,
                Title = "Casas",
                Value = "Casas"
            };

            buttonList.Add(btnInfo);

            var btnIdioma = new CardAction
            {
                Type = ActionTypes.ImBack,
                Title = "Zonas",
                Value = "Zonas"
            };

            buttonList.Add(btnIdioma);


            var menuCard = Factory.Factory.GetHeroCard("Bienvenido a RuralBot. Seleccione una opción", null, null, buttonList);
            replyToConversation.Attachments.Add(menuCard);
            return replyToConversation;
        }
    }
}