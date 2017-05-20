using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Cards
{
	/// <summary>
	/// Interaction logic for Card.xaml
	/// </summary>
	public partial class Card : UserControl, INotifyPropertyChanged
	{
		public enum suit { Hearts = 1, Diamonds = 2, Clubs = 3, Spades = 4 }
		public enum value { Ace = 1, Two = 2, Three = 3, Four = 4, Five = 5, Six = 6, Seven = 7, Eight = 8, Nine = 9, Ten = 10, Jack = 11, Queen = 12, King = 13, Joker = 14 }

		private double _top;
		private double _left;
		private int _shadow;
		private bool _faceShowing;

		private suit _suit;
		private value _value;

		private int _sizeX = 250;
		private int _sizeY = 400;

		public Card(suit s, value v, CardWindow c)
		{
			InitializeComponent();
			_suit = s;
			_value = v;

			_faceShowing = false;

			Width = _sizeX;
			Height = _sizeY;

			cardBorder.CornerRadius = new CornerRadius(_sizeX / 10);

			_myCanvas = c;
		}


		private CardWindow _myCanvas;

		public double canvasTop { get { return _top; } set { _top = value; OnPropertyChanged(); } }
		public double canvasLeft { get { return _left; } set { _left = value; OnPropertyChanged(); } }

		public event PropertyChangedEventHandler PropertyChanged;


		private void cardBorder_MouseDown(object sender, MouseButtonEventArgs e)
		{
			cardShadow.ShadowDepth = 5;

			_myCanvas.MoveToFront(this);
			_myCanvas.SelectCard(this);
		}

		private void cardBorder_MouseUp(object sender, MouseButtonEventArgs e)
		{
			UnSelectCard();
		}

		private void cardBorder_MouseWheel(object sender, MouseWheelEventArgs e)
		{
			if (e.Delta > 0)
			{
				FlipUp();
			}
			else
			{
				FlipDown();
			}
		}

		public void FlipUp()
		{
			if(!Dispatcher.CheckAccess())
			{
				Dispatcher.Invoke(new Action(FlipUp));
			}
			else
			{
				if (!_faceShowing)
				{
					Storyboard s = (Storyboard)TryFindResource("FlipCardLeft");
				
					s.Begin();
					_faceShowing = true;
				}
			}
		}

		public void FlipDown()
		{
			if (!Dispatcher.CheckAccess())
			{
				Dispatcher.Invoke(new Action(FlipDown));
			}
			else
			{
				if (_faceShowing)
				{
					Storyboard s = (Storyboard)TryFindResource("FlipCardRight"); ;
					s.Begin();
					_faceShowing = false;
				}
			}
		}

		public void Flip()
		{
			if (_faceShowing)
			{
				FlipDown();
			}
			else
			{
				FlipUp();
			}
		}

		public void UnSelectCard()
		{
			cardShadow.ShadowDepth = 0;
		}

		protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}



		public suit Suit
		{
			get
			{
				return _suit;
			}

		}

		public int BlackJackValue
		{
			get
			{
				switch (_value)
				{
					case value.Ace:
						return 11;
					case value.Jack:
					case value.Queen:
					case value.King:
						return 10;
					default:
						return (int)_value;
				}
			}

		}

		public bool IsFaceCard
		{
			get
			{
				return _value >= value.Jack && _value <= value.King;
			}
		}

		public string StringValue
		{
			get
			{
				switch (_value)
				{
					case value.Ace:
						return "A";
					case value.Jack:
						return "J";
					case value.Queen:
						return "Q";
					case value.King:
						return "K";
					case value.Joker:
						return "U";
					default:
						return ((int)_value).ToString();
				}
			}
		}

		public bool IsFaceShowing
		{
			get
			{
				return _faceShowing;
			}
		}

		public String SuitImage
		{
			get
			{
				return $"X:\\SourceCode\\Template\\Source\\Cards\\Images\\Suits\\Default\\{_suit.ToString()}.png";
			}
		}

		public String FaceImage
		{
			get
			{
				return $"X:\\SourceCode\\Template\\Source\\Cards\\Images\\Faces\\Default\\{_value.ToString()}_{_suit.ToString()}.png";
			}
		}

		public string SuitColor
		{
			get
			{
				switch (_suit)
				{
					case suit.Diamonds:
					case suit.Hearts:
						return "#D40000";
					default:
						return "Black";
				}
			}
		}

		public string ShowSideCorner
		{
			get
			{
				if (_value >= value.Four)
				{
					return "1*";
				}

				return "0";
			}
		}
		public string ShowSideCenterUp
		{
			get
			{
				if (_value >= value.Six)
				{
					return "1*";
				}

				return "0";
			}
		}

		public string ShowSideCenterBlank
		{
			get
			{
				if (_value == value.Four || _value == value.Five)
				{
					return "1*";
				}

				return "0";
			}
		}

		public string ShowSideCenterDown
		{
			get
			{
				if (_value >= value.Nine)
				{
					return "1*";
				}

				return "0";
			}
		}

		public string ShowCenterTop
		{
			get
			{
				if (_value != value.Four && _value != value.Six)
				{
					return "1*";
				}

				return "0";
			}
		}

		public string ShowCenterTopBlank
		{
			get
			{
				if (_value == value.Eight)
				{
					return "0.5*";
				}

				return "0";
			}
		}

		public string ShowCenterCenter
		{
			get
			{
				if (_value == value.Three)
				{
					return "1*";
				}

				return "0";
			}
		}

		public string ShowCenterCenterBlank
		{
			get
			{
				if (_value == value.Two)
				{
					return "1*";
				}

				return "0";
			}
		}

		public string ShowCenterBottom
		{
			get
			{
				if (_value == value.Two || _value == value.Three || _value == value.Eight || _value == value.Ten)
				{
					return "1*";
				}

				return "0";
			}
		}

		public string ShowCenterBottomBlank
		{
			get
			{
				if (_value == value.Seven || _value == value.Eight)
				{
					return "0.5*";
				}


				return "0";
			}
		}
	}
}
