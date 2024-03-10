using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Xaml.Behaviors;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BingdingCommandTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }
    }

    class CustomTextBox: TextBox
    {
        public bool IsHighLight
        {
            get { return (bool)GetValue(IsHighLightProperty); }
            set { SetValue(IsHighLightProperty, value); }
        }

        public static readonly DependencyProperty IsHighLightProperty =
            DependencyProperty.Register("IsHighLight", typeof(bool), typeof(CustomTextBox), new PropertyMetadata(false));


        public bool HasTest => (bool)GetValue(HasTestProperty);

        public static readonly DependencyProperty HasTestProperty;
        public static readonly DependencyPropertyKey HasTestPropertyKey;

        static CustomTextBox()
        {
            HasTestPropertyKey = DependencyProperty.RegisterReadOnly("HasTest", typeof(bool), typeof(CustomTextBox), new PropertyMetadata(false));
            HasTestProperty = HasTestPropertyKey.DependencyProperty;
        }
            
    }

    class TextBoxHelper
    {


        public static bool GetHasText(DependencyObject obj)
        {
            return (bool)obj.GetValue(HasTextProperty);
        }

        protected static void SetHasText(DependencyObject obj, bool value)
        {
            obj.SetValue(HasTextPropertyKey, value);
        }


        public static readonly DependencyProperty HasTextProperty;

        public static readonly DependencyPropertyKey HasTextPropertyKey;

        static TextBoxHelper()
        {
            HasTextPropertyKey = DependencyProperty.RegisterAttachedReadOnly("HasText", typeof(bool), typeof(TextBoxHelper), new PropertyMetadata(false));
            HasTextProperty = HasTextPropertyKey.DependencyProperty;
        }

        public static bool GetHasTextMoniter(DependencyObject obj)
        {
            return (bool)obj.GetValue(HasTextMoniterProperty);
        }

        public static void SetHasTextMoniter(DependencyObject obj, bool value)
        {
            obj.SetValue(HasTextMoniterProperty, value);
        }

        public static readonly DependencyProperty HasTextMoniterProperty =
            DependencyProperty.RegisterAttached("HasTextMoniter", typeof(bool), typeof(TextBoxHelper), new PropertyMetadata(false,MoniterTextBoxChanged));

        private static void MoniterTextBoxChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is not TextBox box)
                throw new NotSupportedException();

            if((bool)e.NewValue)
            {
                box.TextChanged += Box_TextChanged;
            }
            else
            {
                box.TextChanged -= Box_TextChanged;
            }
        }

        private static void Box_TextChanged(object sender, TextChangedEventArgs e)
        {
            var box = sender as TextBox;
            SetHasText(box!, !string.IsNullOrEmpty(box!.Text));
        }
    }

    class MyBehavior: Behavior<Border>
    {
        protected override void OnAttached()
        {
            AssociatedObject.MouseEnter += (object _, MouseEventArgs _) => AssociatedObject.Background = Brushes.Orchid;
            AssociatedObject.MouseLeave += (object _, MouseEventArgs _) => AssociatedObject.Background = Brushes.White;
        }


        protected override void OnDetaching()
        {
            
        }
    }

    class ClearTextBehavior:Behavior<Button> 
    {
        public TextBox Target
        {
            get { return (TextBox)GetValue(TargetProperty); }
            set { SetValue(TargetProperty, value); }
        }

        public static readonly DependencyProperty TargetProperty =
            DependencyProperty.Register("Target", typeof(TextBox), typeof(ClearTextBehavior), new PropertyMetadata(null));

        protected override void OnAttached()
        {
            AssociatedObject.Click += (object _, RoutedEventArgs _) =>
            {
                Target?.Clear();
            };
        }


    }

    class TextBoxMouseWheelBehavior:Behavior<TextBox>
    {
        public int MaxValue { get; set; }
        public int MinValue { get; set; }
        public int Scale { get; set; } = 1;

        protected override void OnAttached()
        {
            AssociatedObject.MouseWheel += (object _, MouseWheelEventArgs e) =>
            {
                int value;
                try
                {
                    value = int.Parse(AssociatedObject.Text);
                }
                catch { value = MinValue; }
                if(e.Delta > 0) { value += Scale; }
                else { value -= Scale; }
                if(value > MaxValue) { value = MaxValue; }
                else if(value < MinValue) {  value = MinValue; }
                AssociatedObject.Text = value.ToString();
            };
        }
    }
}