using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MagicKeypad
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        int dificalty; //количество символов, выбираемых из массива char[] letters, устанавливается слайдером
        int quantity = 10; //количество знаков, которые нужно ввести на тренажере
        bool checkbox; // с помощью чекбокса задаем массив символов с регистром или без

        int fails; // счетчик ошибок

        int count; // счетчик нажатых клавиш в TextBlock_KeyDown
        DispatcherTimer timer = new DispatcherTimer(); // секундомер
        DateTime date; // время старта секундомера

        private void checkBox1_Checked(object sender, RoutedEventArgs e)
        { checkbox = true; }

        private void checkBox1_Unchecked(object sender, RoutedEventArgs e)
        { checkbox = false; }

        // Создаем массив символов, которые будут рандомно генерироваться:
        private char[] ArrayInitializer()
        {
            char[] letters = new char[0];
            if (checkbox is true)
            {
                // массив с верхним регистром:
                letters = "~!@#$%^&*()_+QWERTYUIOP{}¬ASDFGHJKL:\"ZXCVBNM<> ?`1234567890 -=qwertyuiop[]\\asdfghjkl;'zxcvbnm,.\x020/".ToCharArray();
            }
            else if (checkbox is false)
            {
                // массив без регистра:
                letters = "`1234567890-=qwertyuiop[]\\asdfghjkl;'zxcvbnm,.\x020/".ToCharArray();
            }
            return letters;
        }

        private void Start_Button_Click(object sender, RoutedEventArgs e)
        {
            string str1 = "";
            string str2 = ""; // генерируемая строка
            char[] letters = ArrayInitializer();

            // Генерируем случайные символы:
            Random rand = new Random();
            for (int i = 1; i <= dificalty; i++)
            {
                int letter_num = rand.Next(0, letters.Length); // !?если Length - 1, то последний знак (/) не используется
                str1 += letters[letter_num];
            }

            // Генеруруем случайную последовательность из ранее определенных символов:
            for (int j = 1; j <= quantity; j++)
            {
                int letter_num = rand.Next(0, str1.Length);
                str2 += str1[letter_num];
            }

            textBlock1.Text = str2;

            // Запускаем секундомер (!?при вынесении в отдельный класс не отображает значения)
            date = DateTime.Now;
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();

            // после каждого нажатия старт обнуляем счетчик, сгенерированную строку, введенные символы и ошибки:
            count = 0;
            str2 = "";
            textBlock2.Text = "";
            fails = 0;
            textBox2.Text = "";
        }

        public void timer_Tick(object sender, EventArgs e)
        {
            // сам методы нужен для подсчета Tick, но содержимое можно не задавать
            //Time.Content = DateTime.Now.ToString("HH:mm:ss"); // для проверки будет отображать текущее время в Label Time
        }

        private void Stop_Button_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            double seconds = Convert.ToInt32(DateTime.Now.Ticks - date.Ticks) / 10000000; // !? 10 млн
            double charsMin = count / (seconds / 60);
            textBox1.Text = Convert.ToString(charsMin); // отображается количество введенных в минуту символов
        }

        private void TextBlock_KeyDown(object sender, KeyEventArgs e)
        {
            // !?в настоящем примере событие KeyDown обрабатывать не нужно
            // вся обработка происходит в TextInput

            string key = e.Key.ToString();

            foreach (var item in ContentControl.Content(Grid)) // обходим все элементы формы
            {
                if (item is Label && item.Content == key) // проверяем, что это кнопка
                {
                    Color color = item.Red;
                    item.Background = new SolidColorBrush(color);
                }
            }
        }

        //специально для обработки пробела:
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true; // если пробел, отклоняем ввод/Enter
                textBlock2.Text += " ";
                count++;
            }
            FailCatcher();
        }

        private void Window_TextInput(object sender, TextCompositionEventArgs e)
        {
            string key = e.Text;
            textBlock2.Text += key;

            count++; // подсчет нажатых (со значением) клавиш

            FailCatcher();
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ((Slider)sender).SelectionEnd = e.NewValue; // затенение участка слайдера

            dificalty = Convert.ToInt32(e.NewValue); // устанавливаем сложность, т.е. кол-во используемых знаков
            textBox3.Text = Convert.ToString(Convert.ToInt32(e.NewValue)); // отображаем значение слайдера в текстбоксе
        }

        // проверка на ошибки при вводе:
        private void FailCatcher()
        {
            fails = 0; // при каждом нажатии клавиши цикл начинается заново, поэтому предыдущие ошибки нужно обнулить
            for (int i = 0, j = 0; i <= count - 1; i++)
            {
                if (textBlock1.Text[i] != textBlock2.Text[j])
                {
                    fails++;
                    textBox2.Text = fails.ToString();
                }
                j++;
            }
        }

    }
}
