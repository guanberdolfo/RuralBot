using System;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace HotelBot
{
    public static class StateHelper
    {

        public static async void SetUserCasa(Activity activity, string casaId)
        {
            try
            {
                StateClient stateClient = activity.GetStateClient();
                BotData userData = stateClient.BotState.GetUserData(activity.ChannelId, activity.From.Id);

                userData.SetProperty<string>("CasaId", casaId);
                await stateClient.BotState.SetUserDataAsync(activity.ChannelId, activity.From.Id, userData);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static void SetUserCasa(IDialogContext context, string casaId)
        {
            try
            {
                context.UserData.SetValue("CasaId", casaId);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static string GetUserCasa(Activity activity)
        {
            try
            {
                StateClient stateClient = activity.GetStateClient();
                BotData userData = stateClient.BotState.GetUserData(activity.ChannelId, activity.From.Id);

                var languageCode = userData.GetProperty<string>("CasaId");

                return languageCode;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static string GetUserCasa(IDialogContext context)
        {
            try
            {
                string result;
                context.UserData.TryGetValue("CasaId", out result);

                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public static async void SetUserId(Activity activity, int userId)
        {
            try
            {
                StateClient stateClient = activity.GetStateClient();
                BotData userData = stateClient.BotState.GetUserData(activity.ChannelId, activity.From.Id);

                userData.SetProperty<int>("UserId", userId);
                await stateClient.BotState.SetUserDataAsync(activity.ChannelId, activity.From.Id, userData);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static void SetUserId(IDialogContext context, int userId)
        {
            try
            {
                context.UserData.SetValue("UserId", userId);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static int GetUserId(Activity activity)
        {
            try
            {
                StateClient stateClient = activity.GetStateClient();
                BotData userData = stateClient.BotState.GetUserData(activity.ChannelId, activity.From.Id);

                var languageCode = userData.GetProperty<int>("UserId");

                return languageCode;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static int GetUserId(IDialogContext context)
        {
            try
            {
                int result;
                context.UserData.TryGetValue("UserId", out result);

                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

    }
}