using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Security.Cryptography;
using System.IO;

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
        private bool isDisarrangement;
        private Random rand = new Random();

        public bool IsMaskVisible
        {
            get => isMaskVisible;

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

        public ArrSeat(string ClassInput, bool isDisarrangement)
        {
            InitializeComponent();
            ClassSeats = new List<List<int>>();
            ClassMates = new List<Label>();
            StudentsNumber = Sum(ClassInput);
            IsMaskVisible = false;
            this.isDisarrangement = isDisarrangement;
            InitStudents();
            InitTable(ClassInput);
            PrintOnScreen();
        }

        public ArrSeat(string ClassInput, List<string> custom_class, bool isDisarrangement)
        {
            InitializeComponent();
            ClassSeats = new List<List<int>>();
            ClassMates = new List<Label>();
            StudentsNumber = Sum(ClassInput);
            this.isDisarrangement = isDisarrangement;
            IsMaskVisible = false;
            InitStudents(custom_class);
            InitTable(ClassInput);
            PrintOnScreen();
        }

        int MAX(List<int> list)
        {
            int M = list[0];
            foreach (int i in list)
            {
                M = M > i ? M : i;
            }
            return M;
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

        private void InitStudents()
        {
            for (int i = 0; i < StudentsNumber; i++)
            {
                Label student = new Label();
                student.Content = i + 1;
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
            for (int i = 0; i < column; i++)
            {
                for (int j = 0; j < row; j++)
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

        public void Shuffle(List<Label> list)
        {
            RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
            int n = list.Count;
            while (n > 1)
            {
                byte[] box = new byte[1];
                do provider.GetBytes(box);
                while (!(box[0] < n * (Byte.MaxValue / n)));
                int k = (box[0] % n);
                n--;
                Label value = list[k];
                list[k] = list[n];
                list[n] = value;
            }

            /*while (true) {
                var map = new List<int>();
                map.AddRange(Enumerable.Range(1, list.Count));
                for j in range(n - 1, -1, -1):
                    p = random.randint(0, j)
                    if v[p] == j:
                break
                        else:
                v[j], v[p] = v[p], v[j]
                else:
                if v[0] != 0:
                return tuple(v)
            }*/
        }


        /*https://epubs.siam.org/doi/pdf/10.1137/1.9781611972986.7*/
        public List<int> GenerateDisarragedList(int n)
        {
            List<int> A = Enumerable.Range(0, n).ToList();
            while (true)
            {
                for (int j = n - 1; j >= 0; j--)
                {
                    int p = rand.Next(0, j);
                    if (A[p] == j)
                    {
                        break;
                    }
                    else
                    {
                        Swap<int>(A, j, p);
                    }
                }
                if (A[0] != 0)
                {
                    return A;
                }
            }
        }


        public List<Label> Disarrange(ref List<Label> list)
        {
            var map = GenerateDisarragedList(list.Count);
            var combined = map.Zip(list, (x, y) => new Tuple<int, Label>(x, y)).ToList();
            combined.Sort((a, b) => a.Item1.CompareTo(b.Item1));
            list = combined.Select(_ => _.Item2).ToList();
            return list;
        }

        private void Swap<T>(List<T> list, int first, int second)
        {
            var temp = list[first];
            list[first] = list[second];
            list[second] = temp;
        }

        private void MixSeat()
        {
            if (isDisarrangement)
            {
                ClassMates = Disarrange(ref ClassMates);
            }
            else
            {
                Shuffle(ClassMates);
            }
            // ClassMates = ClassMates.OrderBy(a => Guid.NewGuid()).ToList();
            /*for (int i = ClassMates.Count - 1; i >= 0; i--)
            {
                int j = rng.Next(i+1);
                var temp = ClassMates[j];
                ClassMates[j] = ClassMates[i];
                ClassMates[i] = temp;
            }*/
        }



        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.S && Keyboard.Modifiers == ModifierKeys.Control)
            {
                StreamWriter sr = new StreamWriter("새로운 자리.txt");
                string temp = "새로운 자리배치표입니다.\n다음에 자리를 변경할 때 입력란에 아래 학생들 순서로 입력(또는 복사 후 붙여넣기) 해주시면 됩니다.\n새로운 자리:";
                ClassMates.ForEach(x => temp += (x.Content.ToString() + ','));
                temp = temp.Remove(temp.Length - 1, 1);
                sr.WriteLine(temp);
                sr.Close();
                MessageBox.Show("새로운 자리.txt에 현재 자리가 저장되었습니다.");
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
