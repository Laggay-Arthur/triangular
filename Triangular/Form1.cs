using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Collections.Generic;


namespace Triangular
{
    public partial class Form1 : Form
    {
        Graphics g;
        Bitmap bitmap;
        Polygon polygon; //многоугольник
        int numFigure = 6;//Номер фигуры которую нужно отрисовать
        public Form1() => InitializeComponent();
        void Draw_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(textBox1.Text, out int step) || step < 1) { MessageBox.Show("Неверный шаг!"); return; }
            float[] vertex = null; //х и y координаты многоугольника против часовой стрелки
            label1.Text = numFigure.ToString();
            switch (numFigure)
            {
                case 1://4
                    vertex = new float[]
                    {    80,80,
                         80,140,
                         200,140,
                         200,260,
                         230,260,
                         230,80,
                         200,80,
                         200,110,
                         110,110,
                         110,80
                    }; break;
                case 2://3
                    vertex = new float[]
                    {
                        148,117,
                        148,154,
                        214,154,
                        181,208,
                        214,269,
                        148,269,
                        148,306,
                        251,306,
                        251,269,
                        208,208,
                        251,154,
                        251,117
                    }; break;
                case 3://9
                    vertex = new float[]
                   {
                       120,80,
                       60,80,
                       60,120,
                       90,120,
                       90,180,
                       60,180,
                       60,200,
                       120,200
                   }; break;
                case 4://5
                    vertex = new float[]
                   {
                       120,80,
                       60,80,
                       60,120,
                       90,120,
                       90,200,
                       40,200,
                       40,220,
                       120,220,
                       120,110,
                       90,110,
                       90,100,
                       120,100
                   }; break;
                case 5://Л
                    vertex = new float[]
                   {
                       120,80,
                       80,80,
                       40,200,
                       60,200,
                       80,120,
                       120,120,
                       140,200,
                       160,200
                   }; break;
                case 6://9
                    vertex = new float[]
                   {
                        535.5f, 157.5f,
                        409.5f,  157.5f,
                        346.5f,  220.5f,
                        346.5f,  315f,
                        409.5f,  378f,
                        535.5f, 378f ,
                        535.5f, 441f ,
                        504f, 472.5f,
                        441f, 472.5f,
                        409.5f,441f,
                        409.5f, 409.5f,
                        346.5f, 409.5f,
                        346.5f, 472.5f,
                        409.5f, 535.5f,
                        535.5f, 535.5f,
                        598.5f, 472.5f,
                        598.5f, 220.5f,

                   }; break;
            }
            List<float> vertex_step = new List<float>();
            List<float> vertex_step_save = new List<float>();

            vertex_step.AddRange(vertex);
            vertex_step_save.AddRange(vertex);
            if (step > 1)
            {
                for (int j = 1; j <= step; j++)
                {
                    vertex_step.Clear();

                    for (int i = 0; i < vertex_step_save.Count - 2; i += 2)
                    {
                        vertex_step.Insert(i * 2, vertex_step_save[i]);
                        vertex_step.Insert(i * 2 + 1, vertex_step_save[i + 1]);
                        vertex_step.Insert(i * 2 + 2, (vertex_step_save[i] + vertex_step_save[i + 2]) / 2);
                        vertex_step.Insert(i * 2 + 3, (vertex_step_save[i + 1] + vertex_step_save[i + 3]) / 2);
                    }

                    vertex_step.Add(vertex_step_save[vertex_step_save.Count - 2]);
                    vertex_step.Add(vertex_step_save[vertex_step_save.Count - 1]);

                    vertex_step.Add((vertex_step_save[0] + vertex_step_save[vertex_step_save.Count - 2]) / 2);
                    vertex_step.Add((vertex_step_save[1] + vertex_step_save[vertex_step_save.Count - 1]) / 2);

                    vertex_step_save.Clear();
                    vertex_step_save.AddRange(vertex_step);
                }
            }
            polygon = new Polygon(vertex_step.ToArray());//Вершины многоугольника против часовой стрелки
            bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(bitmap);
            g.SmoothingMode = SmoothingMode.AntiAlias;//Ставим сглаживание
            drawPolygon();
            pictureBox1.Image = bitmap;
        }

        void drawPolygon()//Отрисовка многоугольника разбитого треугольниками
        {
            g.TranslateTransform(0, 20); //Ставим фигуру по центру
            PointF[] points = polygon.Vertecs;

            g.DrawLines(new Pen(Color.Black, 2), points);//Рисуем контур многоугольника
            g.DrawLine(new Pen(Color.Black, 2), points[0], points[points.Length - 1]);

            if (checkBox1.Checked)
            {
                Triangle[] trians = polygon.Triangles;
                if (trians == null) return;
                GraphicsPath p = new GraphicsPath();

                Color color; Random rand = new Random();
                foreach (Triangle t in trians)
                {// Заливка триугольников указанным цветом
                    if (t == null) continue;
                    p.Reset();

                    p.AddLine(t.a, t.b);
                    p.AddLine(t.b, t.c);
                    p.AddLine(t.c, t.a);

                    color = Color.FromArgb(rand.Next(0, 225), rand.Next(0, 225), rand.Next(0, 225));
                    g.FillPath(new SolidBrush(color), p);
                }
                label3.Text = trians.Length + " треугольников";
            }
        }
        //Увеличивает номер фигурый которую будем триангулировать
        void Encrease_Click(object sender, EventArgs e)
        {
            if (numFigure < 6) numFigure++;
            label1.Text = numFigure.ToString();
        }
        //Уменьшает номер фигурый которую будем триангулировать
        void Reduce_Click(object sender, EventArgs e)
        {
            if (numFigure > 1) numFigure--;
            label1.Text = numFigure.ToString();
        }
    }
}