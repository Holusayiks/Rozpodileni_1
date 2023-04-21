using System;
using System.Threading;
using System.Windows.Forms;

namespace LAB1
{
    public partial class Form1 : Form
    {
        static double[] x = { 1, 2, 3, 4, 5 };
        static double[] y = { 7.1, 27.8, 62.1, 110, 161 };
        static int n = 0;

        static double a1, b1, a2, b2;

        static double d1, d2;

        private double start = 0;
        private double finish = 10;

        delegate double Func(double x);

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        public Form1()
        {
            InitializeComponent();
            if (x.Length == y.Length)
            {
                n = x.Length;
            };
            // Нормалізація 
            for (int i = 0; i < n; i++)
            {
                x[i] = Math.Log(x[i]);
            }
            // Створюємо потоки
            Thread thread1 = new Thread(ThreadFunction1);
            thread1.Start();
            Thread thread2 = new Thread(ThreadFunction2);
            thread2.Start();

            // формула з найменшою похибкою
            if (d1 < d2)
            {
                Console.WriteLine("Result Point Vector: ");
                Console.WriteLine("y = " + a1 + "* lnx +" + b1);
                DrawChart((double x) => a1 * Math.Log(Math.E) + b1);
            }
            else
            {
                Console.WriteLine("Result Point Vector: ");
                Console.WriteLine("y = " + Math.Pow(Math.E, a2) + " * x^ " + b2);
                DrawChart((double x) => Math.Pow(Math.E, a2) * Math.Pow(x, b2));
            }
        }

        private void DrawChart(Func func)
        {
            double x_d = 0;
            double y_d = 0;
            double step = 1;

            x_d = start;
            chart1.Series[0].Points.Clear();
            while (x_d < finish)
            {
                y_d = func(x_d);
                chart1.Series[0].Points.AddXY(x_d, y_d);
                x_d += step;
            }
        }

        void ThreadFunction1()
        {
            double Xi = 0;
            double Xi2 = 0;
            double XiYi = 0;
            double Yi = 0;
            // Знайдемо компоненти
            for (int i = 0; i < n; i++)
            {
                Xi += x[i];
                Xi2 += x[i] * x[i];
                XiYi += x[i] * y[i];
                Yi += y[i];
            }
            // підсумкові коефіцієнти і похибка
            a1 = (Yi * Xi2 * n - XiYi * n * Xi) / (Xi2 * n * n - n * Xi * Xi);
            b1 = (XiYi * n - Yi * Xi) / (Xi2 * n - Xi * Xi);
            d1 = Math.Sqrt(((Yi - a1 * Xi - b1) * (Yi - a1 * Xi - b1)) / (Yi * Yi));
            Console.WriteLine("d1 = " + d1);
        }

        void ThreadFunction2()
        {
            double Xi = 0;
            double Xi2 = 0;
            double XiYi = 0;
            double Yi = 0;
            // Нормалізація даних для y = a*x^b
            for (int i = 0; i < n; i++)
            {
                y[i] = Math.Log(y[i]);
            }
            // Знайдемо необхідні компоненти для вирішення системи
            for (int i = 0; i < n; i++)
            {
                Xi += x[i];
                Xi2 += x[i] * x[i];
                XiYi += x[i] * y[i];
                Yi += y[i];
            }
            // Знайдемо підсумкові коефіцієнти і похибку
            a2 = (Yi * Xi2 * n - XiYi * n * Xi) / (Xi2 * n * n - n * Xi * Xi);
            b2 = (XiYi * n - Yi * Xi) / (Xi2 * n - Xi * Xi);
            d2 = Math.Sqrt(((Yi - a2 * Xi - b2) * (Yi - a2 * Xi - b2)) / (Yi * Yi));
            Console.WriteLine("d2 = " + d2);
        }
    }
}
