using System;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;

namespace RuralBot.FormFlows
{
    [Serializable]
    public class RegistroFlow
    {
        [Prompt("¿Cuál es tu nombre?(Apellidos incluidos) {||}")]
        public string fullname;
        [Prompt("¿Cuál es tu email de contacto?{||}")]
        public string email;
        [Prompt("¿Cuál es tu nacionalidad?{||}")]
        public string nation;
        [Prompt("Introduzca un Id para logearse{||}")]
        public string loginId;
        [Prompt("Ahora introduzca una contraseña{||}")]
        public string loginPass;

        public static IForm<RegistroFlow> BuildForm()
        {
            return new FormBuilder<RegistroFlow>()
               .OnCompletion(async (context, profileForm) =>
                {
                    // Tell the user that the form is complete
                    await context.PostAsync("Se ha creado su perfil , proceda a logearse y terminar el proceso de alquiler");
                })
                .Build();
        }
    }
}