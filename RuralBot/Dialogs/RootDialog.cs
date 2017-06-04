using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HotelBot;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace RuralBot.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var buttonTextActivity = await result;
            // Comprobación de pulsación en herocard para elección del menú
            if (buttonTextActivity.Text.ToLower().Contains("zona"))
            {
                var lugares = Services.RuralServices.GetLugares();
                var lugarAttachments = new List<Attachment>();

                //recorremos los registros para crear un attachment por cada uno de ellos.

                if (lugares != null)
                    foreach (var lugar in lugares)
                    {
                        var action = new CardAction
                        {
                            Title = "Seleccionar",
                            Type = ActionTypes.PostBack,
                            Value = $"{lugar.lugarId}"
                        };

                        var atras = new CardAction
                        {
                            Title = "Atras",
                            Type = ActionTypes.ImBack,
                            Value = $"atras"
                        };

                        var actions = new List<CardAction> { action, atras };
                        var fullUrl = lugar.lugarImagenUrl;
                        lugarAttachments.Add(Factory.Factory.GetHeroCard(lugar.lugarNombre, null,
                            new CardImage(Uri.EscapeUriString(fullUrl)), actions));
                    }
                var lugaresActivity = context.MakeMessage();
                lugaresActivity.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                lugaresActivity.Attachments = lugarAttachments;
                await context.PostAsync(lugaresActivity);
                context.Wait(ZonaSelection);
            }
            else if (buttonTextActivity.Text.ToLower().Contains("casa"))
            {
                var casas = Services.RuralServices.GetCasas();
                List<Attachment> casasAttachments = MostarCasas(casas);
                var lugaresActivity = context.MakeMessage();
                lugaresActivity.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                lugaresActivity.Attachments = casasAttachments;
                await context.PostAsync(lugaresActivity);
                context.Wait(CasaSelection);
            }

            else if (buttonTextActivity.Text.ToLower().Contains("start"))
            {

            }
            else
            {
                await DoNotUnderstand(context);
                context.Wait(MessageReceivedAsync);
            }
        }

       

        private async Task ZonaSelection(IDialogContext context, IAwaitable<IMessageActivity> result)
        {

            var zoneSelection = await result;
            int value;
            if (zoneSelection.Text.ToLower().Contains("atras"))
            {
                await Resume(context, result);
            }
            else if (int.TryParse(zoneSelection.Text, out value))
            {
                var casas = Services.RuralServices.GetCasasLugar(value);
                List<Attachment> casasAttachments = MostarCasas(casas);
                var lugaresActivity = context.MakeMessage();
                lugaresActivity.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                lugaresActivity.Attachments = casasAttachments;
                await context.PostAsync(lugaresActivity);
                context.Wait(CasaSelection);

            }
            else
            {
                await DoNotUnderstand(context);
            }

        }

        private async Task CasaSelection(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var casaId = await result;
            StateHelper.SetUserCasa(context, casaId.Text);

            var casaDialog = new CasaDialog();
            await context.Forward(casaDialog, Resume, new Activity{Text = "CasaDialog"}, CancellationToken.None);
        }


        private async Task Resume(IDialogContext context, IAwaitable<object> result)
        {
            var replyToConversation = context.MakeMessage();

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
            replyToConversation.Attachments = new List<Attachment>();
            replyToConversation.Attachments.Add(menuCard);
            await context.PostAsync(replyToConversation);
            context.Wait(MessageReceivedAsync);
        }

        private static async Task DoNotUnderstand(IDialogContext context)
        {
            await context.PostAsync("Perdona, no te he entendido");
        }

        private static List<Attachment> MostarCasas(List<Models.Entity.Casas> casas)
        {
            var casasAttachments = new List<Attachment>();

            if (casas != null)
                foreach (var casa in casas)
                {

                    var alquilar = new CardAction
                    {
                        Title = "Alquilar",
                        Type = ActionTypes.PostBack,
                        Value = $"{casa.casaId}"
                    };

                    var actions = new List<CardAction> { alquilar };
                    var fullUrl = casa.imagenUrl;
                    casasAttachments.Add(Factory.Factory.GetHeroCard(casa.nombreCasa, $"Precio {casa.precio}€",
                        new CardImage(Uri.EscapeUriString(fullUrl)), actions));
                }

            return casasAttachments;
        }

    }
}