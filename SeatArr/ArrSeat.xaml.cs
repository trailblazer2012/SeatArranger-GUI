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
using System.Windows.Shapes;

namespace SeatArr
{
    /// <summary>
    /// ArrSeat.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ArrSeat : Window
    {
        private List<List<int>> ClassSeats;
        private List<Label> ClassMates;
        private int row;
        private int column;
        private int StudentsNumber;
        private bool isMaskVisible;

        public bool IsMaskVisible { get => isMaskVisible;
            
            set 
            { 
                isMaskVisible = value; 
                MaskLabel.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
            } 
        
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.Shutdown();
        }

        public ArrSeat(string ClassInput)
        {
            InitializeComponent();
            ClassSeats = new List<List<int>>();
            ClassMates = new List<Label>();
            StudentsNumber = Sum(ClassInput);
            IsMaskVisible = false;
            InitStudents();
            InitTable(ClassInput);
            PrintOnScreen();
        }

        public ArrSeat(string ClassInput, List<string> custom_class)
        {
            InitializeComponent();
            ClassSeats = new List<List<int>>();
            ClassMates = new List<Label>();
            StudentsNumber = Sum(ClassInput);
            IsMaskVisible = false;
            InitStudents(custom_class);
            InitTable(ClassInput);
            PrintOnScreen();
        }

        int MAX(List<int> list)
        {
            int M = list[0];
            foreach(int i in list)
            {
                M = M > i ? M : i;
            }
            return M;
        }

        int Sum(string str)
        {
            int temp=0;
            foreach(char i in str)
            {
                temp += i-48;
            }
            return temp;
        }

        private void InitStudents()
        {
            for(int i = 0; i < StudentsNumber; i++)
            {
                Label student = new Label();
                student.Content = i+1;
                student.FontSize = 30;
                student.Background = new SolidColorBrush(Color.FromRgb(237, 197, 85));
                ClassMates.Add(student);
                ClassRoom.Children.Add(student);
            }
        }

        private void InitStudents(List<string> c_s)
        {
            for (int i = 0; i < StudentsNumber; i++)
            {
                Label student = new Label();
                student.Content = c_s[i];
                student.FontSize = 30;
                student.Background = new SolidColorBrush(Color.FromRgb(237, 197, 85));
                ClassMates.Add(student);
                ClassRoom.Children.Add(student);
            }
        }

        private void InitTable(string classinput)
        {
            List<int> SeatSetting = new List<int>();

            /*this part make temporary list needed for making table*/
            for (int i = 0; i < classinput.Length; i++)
            {
                SeatSetting.Add(classinput[i] - 48);
            }

            /*this part makes seats*/
            int student_no = 0;

            row = SeatSetting.Count;
            column = MAX(SeatSetting);

            for (int i = 0; i < row; i++)
            {
                List<int> temp = new List<int>();
                for (int j = 0; j < column; j++)
                {
                    if (j < SeatSetting[i])
                    {
                        temp.Add(student_no);
                        student_no++;
                    }
                    else
                    {
                        temp.Add(-1);
                    }
                }
                ClassSeats.Add(temp);
            }

            /*this part sets up the grid*/
            for (int i = 0; i < column; i++)
            {
                ClassRoom.RowDefinitions.Add(new RowDefinition());
            }
            for (int i = 0; i < row; i++)
            {
                ClassRoom.ColumnDefinitions.Add(new ColumnDefinition());
            }
        }

        /*private void PrintSeat()
        {
            Console.WriteLine(GetSeatSheet());
        }*/

        private void PrintOnScreen()
        {
            for (int i=0; i<column; i++)
            {
                for(int j=0; j < row; j++)
                {
                    int student_number = ClassSeats[j][i];
                    if (student_number == -1)
                    {
                        continue;
                    }
                    Label class_mate = ClassMates[student_number];
                    Grid.SetRow(class_mate, i);
                    Grid.SetColumn(class_mate, j);
                }
            }
        }


        private void MixSeat()
        {
            ClassMates = ClassMates.OrderBy(a => Guid.NewGuid()).ToList();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.S && Keyboard.Modifiers == ModifierKeys.Control)
            {
                MessageBox.Show("CTRL + S");
                return;
            }

            if (e.Key == Key.H)
            {
                IsMaskVisible = !IsMaskVisible;
            }

            if (e.Key == Key.Enter || e.Key == Key.Space)
            {
                MixSeat();
            }
            PrintOnScreen();
        }

        /*private int SaveFile()
        {
            HwpActionClass hwpActionClass = new HwpActionClass();
            hwpActionClass.Execute("");
        }*/
    }
}
