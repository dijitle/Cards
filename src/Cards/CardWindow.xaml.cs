using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;

namespace Cards
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class CardWindow : Window
	{
		public CardWindow()
		{
			InitializeComponent();

			for (int suit = 1; suit <= 4; suit++)
			{
				for (int value = 1; (value <= 13); value++)
				{
					Card c = new Card((Card.suit)suit, (Card.value)value, this);
					c.canvasTop = suit * 175;
					c.canvasLeft = value * 125;

					cardCanvas.Children.Add(c);
				}
			}			
		}


		private Card _selectCard = null;
		private Point _mousePosition;
		private Point _cardInitialPosition;

		public void MoveToFront(Card c)
		{
			cardCanvas.Children.Remove(c);
			cardCanvas.Children.Add(c);
		}

		public void SelectCard(Card c)
		{
			_selectCard = c;
			_cardInitialPosition = new Point(c.canvasLeft, c.canvasTop);
		}

		private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
		{		
			_mousePosition = e.GetPosition(cardCanvas);			
		}

		private void Canvas_MouseMove(object sender, MouseEventArgs e)
		{
			if (_selectCard != null)
			{
				Point p = e.GetPosition(cardCanvas);

				_selectCard.canvasTop = p.Y - _mousePosition.Y + _cardInitialPosition.Y;
				_selectCard.canvasLeft = p.X - _mousePosition.X + _cardInitialPosition.X;
			}
		}

		private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
		{
			_selectCard?.UnSelectCard();
			_selectCard = null;
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			List<Card> cards = new List<Card>();
			foreach (Card c in cardCanvas.Children)
			{
				cards.Add(c);
			}
			Thread t = new Thread(() => FlipAllCards(cards.ToArray()));
			t.Start();
		}

		private void FlipAllCards(Card[] cards)
		{
			foreach (Card c in cards)
			{
				Thread t = new Thread(() => c.Flip());
				t.Start();
				Thread.Sleep(100);
			}
		}
	}
}
