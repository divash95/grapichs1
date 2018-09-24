using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace graph
{
    delegate double Funcdelegate(double x);
    public partial class Form1 : Form
    {
        private int height;
        private int width;
        public Form1()
        {
            InitializeComponent();
            height = Height;
            width = Width;
        }

        private bool redraw = false;
      
        private Funcdelegate function;
                   
        private Bitmap graph;
        private void drawFunction(Funcdelegate function)
        {
            double max = int.MinValue;
            double min = int.MaxValue;
            double from = double.Parse(textBox1.Text);
            double to = double.Parse(textBox2.Text);
            for (double i = from; i <= to; i += 0.1)
            {
                double res = function(i);
                if (res > max)
                    max = res;
                if (res < min)
                    min = res;
            }

            double intervals = 400;
            double step = Math.Abs(to - from) / intervals;
            double scaleX = pictureBox1.Width / Math.Abs(to - from);
            double scaleY = pictureBox1.Height / Math.Abs(max - min);
            graph = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics g = Graphics.FromImage(graph);
            g.Clear(Color.White);
            Pen p = new Pen(Color.Black, 2);
            Point p1 = new Point(0, pictureBox1.Size.Height - (int)Math.Truncate((function(from) - min) * scaleY));
            for (double i = from; i < to; i += step)
            {
                double f = function(i);
                int x = (int)Math.Truncate((i - from) * scaleX);
                int y = pictureBox1.Size.Height - (int)Math.Truncate((f - min) * scaleY);
                Point p2 = new Point(x, y);
                g.DrawLine(p, p1, p2);
                p1 = p2;
            }
            pictureBox1.Image = graph;
        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == -1)
                MessageBox.Show("Не выбрана функция", "Ошибка", MessageBoxButtons.OK);
            else
            {
                double from = double.Parse(textBox1.Text);
                double to = double.Parse(textBox2.Text);
                if (from > to)
                {
                    MessageBox.Show("Неправильный интервал", "Ошибка", MessageBoxButtons.OK);
                    return;
                }
                drawFunction(function);
                redraw = true;
            }
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
                return;
            int dy = Size.Height - height;
            int dx = Size.Width - width;
            pictureBox1.Size = new Size(pictureBox1.Size.Width + dx, pictureBox1.Size.Height + dy);
            comboBox1.Location = new Point(comboBox1.Location.X, comboBox1.Location.Y + dy);
            textBox1.Location = new Point(textBox1.Location.X, textBox1.Location.Y + dy);
            textBox2.Location = new Point(textBox2.Location.X, textBox2.Location.Y + dy);
            label1.Location = new Point(label1.Location.X, label1.Location.Y + dy);
            label2.Location = new Point(label2.Location.X, label2.Location.Y + dy);
            label3.Location = new Point(label3.Location.X, label3.Location.Y + dy);
            button1.Location = new Point(button1.Location.X, button1.Location.Y + dy);
            if (redraw)
                drawFunction(function);
            height = Height;
            width = Width;
        }

        private double pow2(double x)
        {
            return Math.Pow(x, 2);
        }

        private double funcX(double x)
        {
            return x;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int n = comboBox1.SelectedIndex;
            switch (n)
            {
                case 0:
                    function = Math.Sin;
                    break;
                case 1:
                    function = Math.Cos;
                    break;
                case 2:
                    function = Math.Tan;
                    break;
                case 3:
                    function = pow2;
                    break;
                case 4:
                    function = Math.Log;
                    break;
                case 5:
                    function = Math.Sqrt;
                    break;
                default:
                    function = funcX;
                    break;
            }
        }
    }
}
