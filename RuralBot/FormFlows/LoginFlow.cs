using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;

namespace RuralBot.FormFlows
{
    [Serializable]
    public class LoginFlow
    {
        
        [Prompt("Introduzca un usuario{||}")]
        public string loginId;
        [Prompt("Ahora introduzca la contraseña{||}")]
        public string loginPass;

        public static IForm<LoginFlow> BuildForm()
        {
            return new FormBuilder<LoginFlow>()
                .OnCompletion(async (context, profileForm) =>
                {
                    // Tell the user that the form is complete
                    await context.PostAsync("Se ha logeado correctamente , ahora le haremos algunas preguntas sobre la compra");
                })
                .Build();
        }
    }
}