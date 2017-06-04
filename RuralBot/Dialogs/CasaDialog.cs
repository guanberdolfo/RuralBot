using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HotelBot;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Connector;
using RuralBot.FormFlows;
using RuralBot.Models.Entity;
using static RuralBot.Services.RuralServices;

namespace RuralBot.Dialogs
{
    [Serializable]
    public class CasaDialog : IDialog
    {
        private static Casas casa;
        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(CasaSelected);
            //await CasaSelected(context,null);
        }

        private async Task CasaSelected(IDialogContext context, IAwaitable<IMessageActivity> result)
        {

            casa = GetCasa(Convert.ToInt32(StateHelper.GetUserCasa(context)));
            if (casa != null)
            {
                List<CardAction> loginregisterActions = new List<CardAction>();
                CardAction loginAction = new CardAction
                {
                    Title = "Si(Login)",
                    Type = ActionTypes.ImBack,
                    Value = "Si(Login)"

                };
                loginregisterActions.Add(loginAction);
                CardAction registroAction = new CardAction
                {
                    Title = "No(Registro)",
                    Type = ActionTypes.ImBack,
                    Value = "No(Registro)"

                };
                loginregisterActions.Add(registroAction);
                var card = Factory.Factory.GetHeroCard("¿Tienes una cuenta con nosotros?", null, null, loginregisterActions);

                var registroMensaje = context.MakeMessage();
                registroMensaje.Attachments = new List<Attachment>();
                registroMensaje.Attachments.Add(card);
                await context.PostAsync(registroMensaje);

                context.Wait(RegistroOrLogin);
            }
            else
            {
                //context.Done("casa no encontrada");
            }
        }

        private async Task RegistroOrLogin(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var loginReg = await result;
            if (loginReg.Text.ToLower().Contains("si"))
            {
                var login = new LoginFlow();
                var loginFormDialog = new FormDialog<LoginFlow>(login, LoginFlow.BuildForm, FormOptions.PromptInStart);
                context.Call(loginFormDialog, LoginHandler);
            }
            else if (loginReg.Text.ToLower().Contains("no"))
            {
                var registro = new RegistroFlow();
                var registroFormDialog = new FormDialog<RegistroFlow>(registro, RegistroFlow.BuildForm, FormOptions.PromptInStart);
                context.Call(registroFormDialog, RegistroHandler);
                //loginReg.Text = "si";
                //IAwaitable<IMessageActivity> loginsi = new AwaitableFromItem<IMessageActivity>(loginReg);
                //await RegistroOrLogin(context, loginsi);
            }
            else
            {
                //context.Done(this);
            }
        }


        private async Task RegistroHandler(IDialogContext context, IAwaitable<RegistroFlow> result)
        {
            var datosRegistro = await result;

            Users usuario = new Users();
            usuario.email = datosRegistro.email;
            usuario.fullName = datosRegistro.fullname;
            usuario.loginId = datosRegistro.loginId;
            usuario.loginPassword = datosRegistro.loginPass;
            usuario.userNation = datosRegistro.nation;

            Registro(usuario);



            await RegistroOrLogin(context, new AwaitableFromItem<IMessageActivity>(new Activity { Text = "Si" }));
        }

        private async Task LoginHandler(IDialogContext context, IAwaitable<LoginFlow> result)
        {
            var login = await result;

            var logeado = Login(login.loginId, login.loginPass);

            if (logeado)
            {
                //Seguir el proceso
                await context.PostAsync("Correcto");
                var user = GetUser(login.loginId);
                StateHelper.SetUserId(context, user.userId);

                await context.PostAsync("¿Cuantas personas van a ir?");
                context.Wait(ProcesoCompra);

            }
            else
            {
                await context.PostAsync("Usuario o contraseña incorrecto , intentelo de nuevo");

                //TODO PROBAR LA LINEA DE ABAJO
                await RegistroOrLogin(context, new AwaitableFromItem<IMessageActivity>(new Activity { Text = "si" }));
            }
        }

        private async Task ProcesoCompra(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var npersonas = await result;

            var casa = GetCasa(Convert.ToInt32(StateHelper.GetUserCasa(context)));

            if (Int32.Parse(npersonas.Text) > casa.numeroCamas)
            {
                await context.PostAsync("No hay tantas camas en esa casa para ese numero de personas");
                //TODO probar la linea de abajo
                context.Done("Que elija otra csasa");
            }
            else
            {
                var compra = new CompraFlow();
                compra.npersonas = npersonas.Text;
                var compraFormDialog = new FormDialog<CompraFlow>(compra, CompraFlow.BuildForm, FormOptions.PromptInStart);
                context.Call(compraFormDialog, TerminarCompra);
            }
        }

        private async Task TerminarCompra(IDialogContext context, IAwaitable<CompraFlow> result)
        {
            var datosCompra = await result;

            var venta = new Ventas
            {
                casaId = Convert.ToInt32(StateHelper.GetUserCasa(context)),
                userId = StateHelper.GetUserId(context),
                fechaEntrada = datosCompra.fechaEntrada.Date,
                fechaSalida = datosCompra.fechaSalida.Date,
                numeroPersonas = Convert.ToInt32(datosCompra.npersonas)
            };
            Alquiler(venta);
            await context.PostAsync("Disfrute de su estancia");
        }
    }
}