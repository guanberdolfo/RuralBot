using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Connector;

namespace RuralBot.Factory
{
    [Serializable]
    public static class Factory
    {
        public static Attachment GetHeroCard(string title, string subtitle, CardImage cardImage,
            List<CardAction> cardAction)
        {
            var heroCard = new HeroCard
            {
                Title = title,
                Subtitle = subtitle,
                Buttons = cardAction
            };
            if (cardImage != null) heroCard.Images = new List<CardImage> {cardImage};
            return heroCard.ToAttachment();
        }
    }
}