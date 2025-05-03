using System;
using System.Collections.ObjectModel;
using System.Linq;
using WpfApp5.Models;
using WpfApp5.Models.Cards;

namespace WpfApp5.Services
{
    public class CardService
    {
        private readonly ObservableCollection<CardModel> _deck;
        public ReadOnlyObservableCollection<CardModel> Deck { get; }

        public CardService()
        {
            _deck = new ObservableCollection<CardModel>();
            Deck = new ReadOnlyObservableCollection<CardModel>(_deck);
            InitializeDeck();
        }

        private void InitializeDeck()
        {
            _deck.Add(new AddNormBreadCardModel());
            _deck.Add(new AddHeartyBreadCardModel());
            _deck.Add(new AddHarvestBreadCardModel());
            _deck.Add(new AddSimpleBreadCardModel());
            _deck.Add(new AddNormBeerCardModel());
            _deck.Add(new AddDarkBeerCardModel());
            _deck.Add(new AddLagerBeerCardModel());
            _deck.Add(new AddHolBeerCardModel());
            _deck.Add(new ExchangeGrainForWaterCardModel());
            _deck.Add(new ExchangeWaterForGrainCardModel());
            _deck.Add(new ExchangeStarterForHopsCardModel());
            _deck.Add(new ExchangeHopsForStarterCardModel());
        }

        public void Shuffle()
        {
            _deck.Clear();          // Очистити стару колоду
            InitializeDeck();       // Додати всі карти заново

            var rnd = new Random();
            var list = _deck.OrderBy(_ => rnd.Next()).ToList();
            _deck.Clear();
            foreach (var c in list) _deck.Add(c);
        }

        public CardModel DrawCard()
        {
            if (_deck.Count == 0) return null;
            var c = _deck[0];
            _deck.RemoveAt(0);
            return c;
        }

        public ObservableCollection<CardModel> DrawHand(int count)
{
            var hand = new ObservableCollection<CardModel>();
            int drawn = 0;

            while (_deck.Count > 0 && drawn < count)
            {
                var card = DrawCard();
                if (card != null)
                {
                    hand.Add(card);
                    drawn++;
                }
                else
                {
                    break; // Запас карт в колоді вичерпано
                }
            }

            return hand;
        }
        public void DrawHand(PlayerModel player, int count)
        {
            if (player.Inventory == null)
                player.Inventory = new ObservableCollection<CardModel>();
            else
                player.Inventory.Clear(); // Очистити стару руку

            int drawn = 0;

            while (drawn < count && _deck.Count > 0)
            {
                var card = DrawCard();
                if (card != null)
                {
                    player.Inventory.Add(card);
                    drawn++;
                }
                else
                {
                    break; // Якщо карт більше немає
                }
            }
        }

    }
}
