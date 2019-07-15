using System.Drawing;
using System.Collections.Generic;


namespace Triangular
{
    class Triangle //треугольник
    {
        public Triangle(PointF a, PointF b, PointF c)
        { this.a = a; this.b = b; this.c = c; }
        public PointF a { get; }
        public PointF b { get; }
        public PointF c { get; }
    }

    class VertexPoint
    {
        public VertexPoint(float x, float y) => point = new PointF(x, y);

        public PointF point { get; }
        public bool Taken { get; set; } = false;
    }

    class Polygon
    {
        List<VertexPoint> Vertex;//Вершины многоугольника
        List<Triangle> triangles; //треугольники, на которые разбит наш многоугольник
        public Polygon(float[] vertecs) //vertecs - х и y координаты
        {
            if (vertecs.Length % 2 == 1 || vertecs.Length < 6)
            { System.Windows.Forms.MessageBox.Show("Вы задали не многоугольник!"); return; }
            Vertex = new List<VertexPoint>();
            triangles = new List<Triangle>();
            for (int i = 0; i < vertecs.Length; i += 2)//Преобразование координат в вершины
                Vertex.Add(new VertexPoint(vertecs[i], vertecs[i + 1]));

            triangulate();
        }

        void triangulate() //триангуляция
        {
            //Начальные вершины рассматриваемого треугольника
            int ai = NextNotTaken(0),
                bi = NextNotTaken(ai + 1),
                ci = NextNotTaken(bi + 1),
                count = 0, //количество шагов
                leftvertecs = Vertecs.Length; //сколько осталось рассмотреть вершин

            if (ai == -1 || bi == -1 || ci == -1)
                System.Windows.Forms.MessageBox.Show("Not triangulared!");
            while (leftvertecs > 3) //пока не остался один треугольник
            {
                if (isLeft(Vertex[ai].point, Vertex[bi].point, Vertex[ci].point) && canBuildTriangle(ai, bi, ci)) //если можно построить треугольник
                {
                    triangles.Add(new Triangle(Vertex[ai].point, Vertex[bi].point, Vertex[ci].point)); //Добавляет новый треугольник
                    Vertex[bi].Taken = true;
                    leftvertecs--;
                    bi = ci;
                    ci = NextNotTaken(ci + 1); //берем следующую вершину
                }
                else
                {// берем следущие три вершины
                    ai = NextNotTaken(ai + 1);
                    bi = NextNotTaken(ai + 1);
                    ci = NextNotTaken(bi + 1);
                }

                if (count > Vertex.Count * Vertex.Count)
                {// Триангуляцию провести невозможно (например, многоугольник задан по часовой стрелке)
                    triangles = null;
                    isGood = false;
                    System.Windows.Forms.MessageBox.Show("Bad vertex!");
                    break;
                }
                count++;
            }
            if (triangles != null) //если триангуляция была проведена успешно
                triangles.Add(new Triangle(Vertex[ai].point, Vertex[bi].point, Vertex[ci].point));//Добавляем последний треугольник*
        }
        bool isGood = true;//Была ли триангуляция успешной
        int NextNotTaken(int startPos) //найти следущую нерассмотренную вершину
        {
            startPos %= Vertex.Count;
            if (!Vertex[startPos].Taken) return startPos;

            int i = (startPos + 1) % Vertex.Count;
            while (i != startPos)
            {
                if (!Vertex[i].Taken) return i;
                i = (i + 1) % Vertex.Count;
            }
            return -1;
        }
        bool isLeft(PointF a, PointF b, PointF c) //левая ли тройка векторов
        {
            float abX = b.X - a.X;
            float abY = b.Y - a.Y;

            float acX = c.X - a.X;
            float acY = c.Y - a.Y;

            return abX * acY - acX * abY < 0;
        }
        bool isPointInside(PointF a, PointF b, PointF c, PointF p) //находится ли точка p внутри треугольника abc
        {
            float ab = (a.X - p.X) * (b.Y - a.Y) - (b.X - a.X) * (a.Y - p.Y);
            float bc = (b.X - p.X) * (c.Y - b.Y) - (c.X - b.X) * (b.Y - p.Y);
            float ca = (c.X - p.X) * (a.Y - c.Y) - (a.X - c.X) * (c.Y - p.Y);

            return (ab >= 0 && bc >= 0 && ca >= 0) || (ab <= 0 && bc <= 0 && ca <= 0);
        }
        bool canBuildTriangle(int ai, int bi, int ci) //false - если внутри есть вершина
        {
            for (int i = 0; i < Vertecs.Length; i++) //рассмотрим все вершины многоугольника
                if (i != ai && i != bi && i != ci)   //кроме троих вершин текущего треугольника
                    if (isPointInside(Vertecs[ai], Vertecs[bi], Vertecs[ci], Vertecs[i]))
                        return false;
            return true;
        }
        //Возвращает вершины
        public PointF[] Vertecs
        {
            get
            {
                int index = 0;
                PointF[] points = new PointF[Vertex.Count];
                foreach (VertexPoint vp in Vertex)
                    points[index++] = vp.point;
                return points;
            }
        }
        //Возвращает треугольники для отображения
        public Triangle[] Triangles
        {
            get
            {
                if (isGood) return triangles.ToArray();
                return null;
            }
        }
    }
}