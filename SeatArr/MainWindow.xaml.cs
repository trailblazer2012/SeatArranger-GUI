using System;
using System.Collections.Generic;
using System.IO;
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

namespace SeatArr
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ProcessClassInformation();
            }
        }

        int Sum(string str)
        {
            int temp = 0;
            foreach (char i in str)
            {
                temp += i - 48;
            }
            return temp;
        }

        private void ProcessClassInformation()
        {
            if (IsValidClass())
            {
                if (CheckBox_CustomStudent.IsChecked == true)
                {
                    List<string> custom_class = GetCustomClass();
                    if (custom_class == null | Sum(ClassInput.Text) != custom_class.Count)
                    {
                        MessageBox.Show("'반, 이름 저장하기' 파일이 손상되었거나\n파일에 적혀있는 학생수와 입력한 학생수가 불일치합니다.", "quite fatal error", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    this.Hide();
                    ArrSeat arrSeat = new ArrSeat(ClassInput.Text, custom_class);
                    arrSeat.ShowDialog();
                }
                else
                {
                    this.Hide();
                    ArrSeat arrSeat = new ArrSeat(ClassInput.Text);
                    arrSeat.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show( "잘못된 입력값입니다! 다시 시도해주세요" , "not fatal error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        
        private bool IsValidClass()
        {
            if (ClassInput.Text == "")
            {
                return false;
            }

            foreach (Char i in ClassInput.Text)
            {
                if (!Char.IsDigit(i))
                {
                    return false;
                }
            }
            return true;
        }

        private List<string> GetCustomClass()
        {
            StreamReader sr = new StreamReader("반, 이름 지정하기.txt");
            string temp="";
            while (sr.Peek() >= 0) { 
                temp = sr.ReadLine();
            }
            sr.Close();
            List<string> custrom_class = new List<string>(temp.Split(':')[1].Split(','));
            return custrom_class;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ProcessClassInformation();
        }
    }
}
