using System;
using System.Collections.Generic;
using System.Windows.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace MagicKeypad
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        int dificalty; //количество символов, выбираемых из массива char[] letters, устанавливается слайдером
        int quantity = 100; //количество знаков, которые нужно ввести на тренажере
        bool checkbox; // с помощью чекбокса задаем массив символов с регистром или без

        Brush oldBackground; // предыдущий цвет клавиши, используется в KeyUp и KetDown
        bool mixColourPrevent = false; // !?цвет клавиш при быстром нажатии продолжает путаться

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

            // после каждого нажатия старт обнуляем счетчик, сгенерированную строку, введенные символы, ошибки т текстблок для закраски строки:
            count = 0;
            str2 = "";
            textBlock2.Text = "";
            fails = 0;
            textBox2.Text = "";
            textBlockAbove1st.Text = "";
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

        //private void TextBlock_KeyDown(object sender, KeyEventArgs e)
        //{
        //    string key = e.Key.ToString().ToLower();

        //    foreach (var item in myGrid.Children) // обходим все элементы формы
        //    {
        //        if (item is Label lbl && lbl.Content.ToString() == key) // проверяем, что это лэйбл и сравниваем его контент с контентом клавиши в нижнем регистре
        //        {
        //            oldBackground = lbl.Background; // запоминаем предыдущий цвет
        //            lbl.Background = Brushes.Red; // меняем цвет клавиши
        //        }
        //    }
        //}



        //private void Window_KeyUp(object sender, KeyEventArgs e)
        //{
        //    string key = e.Key.ToString();

        //    foreach (var item in myGrid.Children) // обходим все элементы формы
        //    {
        //        if (item is Label lbl && lbl.Content.ToString() == key.ToLower()) // проверяем, что это лэйбл и сравниваем его контент с контентом клавиши в нижнем регистре
        //        {
        //            lbl.Background = oldBackground; // возвращаем предыдущий цвет
        //        }
        //    }
        //}

        private void TextBlock_KeyDown(object sender, KeyEventArgs e)
        {
            string key = e.Key.ToString();

            Dictionary<string, string> dict = new Dictionary<string, string>
            {
                ["Oem8"] = "`",
                ["OemMinus"] = "-",
                ["OemPlus"] = "=",
                ["OemOpenBrackets"] = "[",
                ["Oem6"] = "]",
                ["OemQuotes"] = "#",
                ["Oem1"] = ";",
                ["Oem3"] = "'",
                ["OemComma"] = ",",
                ["OemPeriod"] = ".",
                ["OemQuestion"] = "/",

                //["LeftShift"] = "LShift",
                //["RightShift"] = "RShift",
                //["Back"]
                //["Tab"]
                //["Capital"]
                //["LeftCtrl"]
                //["RightCtrl"]
                //["System"] // alt левый
                //["LeftCtrl"] // alt правый
                //["LWin"] // win левый
                //["Apps"] // win правый

                ["D0"] = "0",
                ["D1"] = "1",
                ["D2"] = "2",
                ["D3"] = "3",
                ["D4"] = "4",
                ["D5"] = "5",
                ["D6"] = "6",
                ["D7"] = "7",
                ["D8"] = "8",
                ["D9"] = "9",

                ["A"] = "a",
                ["B"] = "b",
                ["C"] = "c",
                ["D"] = "d",
                ["E"] = "e",
                ["F"] = "f",
                ["G"] = "g",
                ["H"] = "h",
                ["I"] = "i",
                ["J"] = "j",
                ["K"] = "k",
                ["L"] = "l",
                ["M"] = "m",
                ["N"] = "n",
                ["O"] = "o",
                ["P"] = "p",
                ["Q"] = "q",
                ["R"] = "r",
                ["S"] = "s",
                ["T"] = "t",
                ["U"] = "u",
                ["V"] = "v",
                ["W"] = "w",
                ["X"] = "x",
                ["Y"] = "y",
                ["Z"] = "z",
            };
            dict.TryGetValue(key, out string value);

            foreach (var item in myGrid.Children) // обходим все элементы формы
            {
                if (item is Label lbl && lbl.Content.ToString() == value && mixColourPrevent == false) // проверяем, что это лэйбл и сравниваем его контент с контентом клавиши в нижнем регистре
                {
                    oldBackground = lbl.Background; // запоминаем предыдущий цвет
                    lbl.Background = Brushes.Red; // меняем цвет клавиши
                    mixColourPrevent = true;
                }
            }
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            string key = e.Key.ToString();

            Dictionary<string, string> dict = new Dictionary<string, string>
            {
                ["Oem8"] = "`",
                ["OemMinus"] = "-",
                ["OemPlus"] = "=",
                ["OemOpenBrackets"] = "[",
                ["Oem6"] = "]",
                ["OemQuotes"] = "#",
                ["Oem1"] = ";",
                ["Oem3"] = "'",
                ["OemComma"] = ",",
                ["OemPeriod"] = ".",
                ["OemQuestion"] = "/",

                ["D0"] = "0",
                ["D1"] = "1",
                ["D2"] = "2",
                ["D3"] = "3",
                ["D4"] = "4",
                ["D5"] = "5",
                ["D6"] = "6",
                ["D7"] = "7",
                ["D8"] = "8",
                ["D9"] = "9",

                ["A"] = "a",
                ["B"] = "b",
                ["C"] = "c",
                ["D"] = "d",
                ["E"] = "e",
                ["F"] = "f",
                ["G"] = "g",
                ["H"] = "h",
                ["I"] = "i",
                ["J"] = "j",
                ["K"] = "k",
                ["L"] = "l",
                ["M"] = "m",
                ["N"] = "n",
                ["O"] = "o",
                ["P"] = "p",
                ["Q"] = "q",
                ["R"] = "r",
                ["S"] = "s",
                ["T"] = "t",
                ["U"] = "u",
                ["V"] = "v",
                ["W"] = "w",
                ["X"] = "x",
                ["Y"] = "y",
                ["Z"] = "z",
            };
            dict.TryGetValue(key, out string value);

            foreach (var item in myGrid.Children) // обходим все элементы формы
            {
                if (item is Label lbl && lbl.Content.ToString() == value && mixColourPrevent == true) // проверяем, что это лэйбл и сравниваем его контент с контентом клавиши в нижнем регистре
                {
                    lbl.Background = oldBackground; // возвращаем предыдущий цвет
                    mixColourPrevent = false;
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
                textBlockAbove1st.Text += textBlock1.Text[count];
                count++;
            }
            FailCatcher();
        }

        private void Window_TextInput(object sender, TextCompositionEventArgs e)
        {
            string key = e.Text;
            textBlock2.Text += key;
            //textBlock2.Inlines.Append(new Run { Text = e.Text, Background = Brushes.Green }); // не получилось сделать закраску строк с помощью Run, пришлось использовать дополнительные текстблоки

            textBlockAbove1st.Text += textBlock1.Text[count]; // закрашиваем верхнюю строку

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
