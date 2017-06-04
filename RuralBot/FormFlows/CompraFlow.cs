using System;

using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;

namespace RuralBot.FormFlows
{
    [Serializable]
    public class CompraFlow
    {
        [Prompt("Introduzca la fecha de entrada")]
        public DateTime fechaEntrada;
        [Prompt("Introduzca la fecha de salida")]
        public DateTime fechaSalida;

        public string npersonas;
        public static IForm<CompraFlow> BuildForm()
        {
            return new FormBuilder<CompraFlow>()
                .OnCompletion(async (context, profileForm) =>
                {
                    // Tell the user that the form is complete
                    await context.PostAsync("Se ha realizado el proceso de alquiler correctamente , nos pondremos en contacto contigo para realizar el pago.");
                })
                .Build();
        }
    }
}