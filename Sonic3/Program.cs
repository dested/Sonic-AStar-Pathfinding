//#define DONTDRAW

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;

namespace Sonic3 {
    public struct Point {
        public int X;
        public int Y;
        public Point(int x, int y) {
            X = x;
            Y = y;
        }

    }
    static class Program {

        public static string Repeat(this string ms, int mz) {
            string am = "";
            for (int i = 0; i < mz; i++) {
                am += ms;
            }
            return am;
        }


        private static void writeString(char[,] current, int x, int y, string s) {
            int curX = x;
            foreach (string c in s.Replace("\n", "").Split('\r')) {
                foreach (var ch in c) {
                    current[x, y] = ch;
                    x++;
                }
                x = curX;
                y++;
            }

        }
        static void rrrd() {



            Game gg = new Game(
   @"000222220
0001111110
0222000001
0222000001
0222000000
1000000000
0000000001
0000022220
1000022220
1000022220
1011000000"
   );

            gg.start(new Point(gg.W / 2, gg.H / 2));



            var s =
@"**  *** ********
*    *   **XX@@*
          **XX@*
           **XX*
 ****  *    **X*
 *X*  ***    ***
 **  BBBBB    **
 *  *BBBBB*    *
 * **BBBBB**
    *BBBBB*    *
     BBBBB    **
   B  ***  *   *
  BOB  *  **
*  B     *X*
**      ****   *
@**           **";


            string dl = "";

            string[] m = s.Replace("\n", "").Split('\r');
            for (int i = 0; i < m.Length; i++) {
                for (int a = 0; a < m[i].Length; a += 1) {
                    switch (m[i][a]) {
                        case 'O':
                        case ' ':
                        case '_':
                        case '|':
                            dl += "0";
                            break;
                        case 'B':
                            dl += "2";
                            break;
                        case 'X':
                        case '@':
                        case '*':
                            dl += "1";
                            break;
                    }
                }
                dl += "\r\n";
            }

            Game g = new Game(dl);

            g.start(new Point(gg.W / 2, gg.H / 2));


        }

        public static void runit(string s, int x, int y) {
            Bitmap b;
            Game g2;

            b = new Bitmap(s);
            g2 = new Game(b);
            g2.previewH = (int)(Console.WindowHeight / 1.0);
            g2.previewW = Console.WindowWidth;
            g2.drawChar += DrawChar;
            //  try {
            g2.start(new Point(x, y));
            //   }
            //   catch (Exception re) {
            //   }
        }

        static void Main(string[] args) {
            Console.WindowHeight = Console.LargestWindowHeight;


            runit("sonic8.jpg", 3, 5);
            runit("sonic7.gif", 3, 5);
            runit("sonic5.gif", 100, 36);
            runit("sonic.gif", 19, 4);
            runit("sonic2.gif", 18, 2);
            runit("sonic3.gif", 4, 4);
            runit("sonic4.gif", 3, 5);

        }

        private static void DrawChar(int arg1, int arg2, char arg3) {
            Console.SetCursorPosition(arg1, arg2);
            Console.Write(arg3);

        }
    }

    public enum Ball {
        Empty = 0,
        Red = 1,
        Blue = 2,
        Wall = 3
    }
    public enum Direction {
        Up = 0,
        Down = 2,
        Left = 1,
        Right = 3
    }
    public class Game {
        public int previewW;
        public int previewH;
        private char[] current;

        public int W;
        public int H;

        private Ball[] Balls;
        public bool checkV(Color c,int r, int g, int b) {
            int m = 9;
            return c.R > r - m && c.R < r + m && c.G > g - m && c.G < g + m && c.B > b - m && c.B < b + m;
        }
        public Game(Bitmap b) {

            StringBuilder sb = new StringBuilder();



            for (int y = 6; y < b.Height; y += 12) {
                for (int x = 6; x < b.Width; x += 12) {
                    var v = b.GetPixel(x, y);
                    if (checkV(v,255,0,0)) {
                        sb.Append("1");
                    }
                    else    if (checkV(v, 128, 128, 128)) {
                        sb.Append("3");
                    }
                    else if (checkV(v,0,0,255)) {
                        sb.Append("2");
                    }
                    else {

                        sb.Append("0");

                    }
                }
                sb.Append("\r\n");
            }
            load(sb.ToString().Trim('\n').Trim('\r'));
        }

        public Game(int w, int h) {
            W = w;
            H = h;
            Balls = new Ball[w * h];
            for (int x = 0; x < w; x++) {
                for (int y = 0; y < h; y++) {
                    Balls[y * w + x] = Ball.Empty;
                }
            }

        }
        public Game(string map) {
            load(map);
        }

        private void load(string map) {
            map = map.Replace("\r", "");
            var s = map.Split('\n');

            W = s[0].Length;
            H = s.Length;
#if DONTDRAW
#else
            try {
                Console.WindowHeight = H + 5 > 83 ? 83 : H + 5;
            }
            catch (Exception e) {

            }
#endif


            Balls = new Ball[W * H];
            for (int x = 0; x < W; x++) {
                for (int y = 0; y < H; y++) {
                    switch (s[y][x]) {
                        case '0':
                            Balls[y * W + x] = Ball.Empty;
                            break;
                        case '1':
                            Balls[y * W + x] = Ball.Red;
                            break;
                        case '2':
                            Balls[y * W + x] = Ball.Blue;
                            break;
                        case '3':
                            Balls[y * W + x] = Ball.Wall;
                            break;
                    }
                }
            }



            Vectors = new Point[4];
            for (Direction d = Direction.Up; d <= Direction.Right; d++) {
                switch (d) {
                    case Direction.Up:
                        Vectors[(int)d] = new Point(0, -1);
                        break;
                    case Direction.Down:
                        Vectors[(int)d] = new Point(0, 1);
                        break;
                    case Direction.Left:
                        Vectors[(int)d] = new Point(-1, 0);
                        break;
                    case Direction.Right:
                        Vectors[(int)d] = new Point(1, 0);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

        }

        private int pad = 1;

        private int Moves = 0;
        public void start(Point start) {
            previouss = new char[previewW * previewH];
            current = new char[W * pad * H];

            //            try {
            Point p = start;

            Direction oldD = Direction.Up;
            Move cur;
            while (!boardEmpty()) {
                Moves++;
                switch (Balls[p.Y * W + p.X]) {
                    case Ball.Empty:
                        cur = move(oldD, ref p);
                        oldD = cur.initial;
                        break;
                    case Ball.Red:
                        p.Y -= 1;
                        continue;
                    case Ball.Wall:
                        p.Y -= 1;
                        continue;
                    case Ball.Blue:
                        Balls[p.Y * W + p.X] = Ball.Empty;
                        cur = move(oldD, ref p);
                        oldD = cur.initial;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                drawm(p, cur, LastMap, true);

            }
            //   }catch (Exception e) {Console.Write(e.Message);Console.Read();}
            //  WriteStr(current, 0, 0, "||" + Moves + "||");
        }

        private int[] LastMap;
        private void drawm(Point p, Move cur, int[] map, bool ok) {

#if DONTDRAW
#else
            Draw(p);
          drawMap(map, cur.current > 0 ? cur.current : 1, p);
            
           // if (ok) 
           // drawPath(p, cur.myPath);

            drawIt(p);
            if (update != null)
                update();


            Thread.Sleep(0);
#endif
        }

        private void Draw(Point ep) {
            var p = ep;

            var m = W;
            var n = H;
            //
            //            for (int x = 0; x < m; x++) {
            //                for (int y = 0; y < n; y++) {
            //                    Console.SetCursorPosition(x, y);
            //                    Console.Write(Program.current[x, y]);
            //                }
            //            }

            //   Console.Clear();

            p.X -= previewW / 2;
            p.Y -= previewH / 2;

            for (int y = 0; y < previewH; y += 1) {

                for (int x = 0; x < previewW; x += 1) {


                    var ind = ((y + p.Y + n) % n) * W + ((x + p.X + m) % m);

                    switch (Balls[ind]) {
                        case Ball.Empty:
                            current[ind] = ' ';
                            //                            WriteChar(current, x, y, ' ');
                            break;
                        case Ball.Red:
                            current[ind] = '*';
                            //                            WriteChar(current, x, y, '*');
                            break;
                        case Ball.Blue:
                            current[ind] = '@';
                            //                          WriteChar(current, x, y, '@');
                            break;
                        case Ball.Wall:
                            current[ind] = (char)126;
                            //                            WriteChar(current, x, y, (char)126);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }


            current[(p.Y + previewH / 2) * W + p.X + previewW / 2] = 's';

        }

        public char[] previouss;
        private void drawIt(Point ecur) {
            var cur = ecur;

            cur.X -= previewW / 2;
            cur.Y -= previewH / 2;

            for (int y = 0; y < previewH; y += 1) {

                for (int x = 0; x < previewW; x += 1) {
                    char d;




                    //          d = y + curY >= n || x + curX >= m || y + curY <= 0 || x + curX <= 0 ? ' ' : current[x + curX, y + curY];


                    d = current[((y + cur.Y + H) % H) * W + ((x + cur.X + W) % W)];
                    int m = y * previewW + x;

                    if (previouss[m] != d) {
                        previouss[m] = d;
                        drawChar(x, y, d);
                    }

                }

            }




        }

        public Action<int, int, char> drawChar;

        private Point[] Vectors;

        private Move move(Direction oldD, ref Point point) {
            var l = moveToClosestBlue(oldD, point);
            point = GetMovedXY(l.initial, point);
            return l;
        }

        private bool boardEmpty() {
            foreach (var ball in Balls) {
                if (ball == Ball.Blue) {
                    return false;
                }
            }
            return true;
        }

        private Move moveToClosestBlue(Direction previous, Point p) {
            int[] map_ = new int[W * H];

            Move[] myMoves = new Move[4];

            int count = 0;
            int red = 0;
            for (Direction d = Direction.Up; d <= Direction.Right; d++) {
                //                if (previous == d) 
                //                    continue;

                Point moved = GetMovedXY(d, p);


                switch (Balls[moved.Y * W + moved.X]) {
                    case Ball.Blue:
                        if (Balls[moved.Y * W + moved.X] == Ball.Red)
                            runForSquares(p.X, p.Y, moved.X, moved.Y);
                        LastMap = map_;
                        return new Move() { initial = d, myPath = new Point[0] };
                    case Ball.Red:
                        red++;
                        continue;
                    case Ball.Wall:
                        red++;
                        continue;
                    default:

                        map_[moved.Y * W + moved.X] = 0;
                        myMoves[count++] = new Move() { initial = d, current = 0, last = d, point = moved, myPath = new[] { moved, } };
                        break;
                }
            }
            if (red == 4) {
                Point m = new Point(p.X, p.Y - 1);
                map_[m.Y * W + m.X] = 0;
                LastMap = map_;

                return new Move() { initial = previous, current = 0, last = previous, point = m, myPath = new[] { m, } };
            }

            while (count > 0) {


                Move[] mz = new Move[count];
                Array.Copy(myMoves, mz, count);

                myMoves = new Move[count * 4];
                count = 0;
                foreach (var move1 in mz) {

                    Move[] smoves = new Move[4];
                    int current_;
                    if (findClosestBlue(move1, getOpposite(move1.last), ref map_, ref smoves, out current_)) {
                        LastMap = map_;

                        return move1;
                    }
                    for (int i = 0; i < current_; i++)
                        myMoves[count++] = smoves[i];

                    //      drawm(p,move1,map_,false); 
                }
            }

            // LastMap = map_;

            //  throw new Exception("Done");
            return new Move() { current = 0, initial = Direction.Down, last = Direction.Down, point = p, };
        }


        private bool runForSquares(int startx, int starty, int x, int y) {
            int timer = 0;
            Ball[] balls = (Ball[])Balls.Clone();

            List<Point> peremeter = new List<Point>();
            peremeter.Add(new Point(startx, starty));
        foos:
            peremeter.Add(new Point(x, y));

            Ball[] b = new Ball[4];

#if DONTDRAW
#else
            current[(y + SIDEPAD) * W + (x + SIDEPAD)] = 'M';
            //            WriteChar(current, x + SIDEPAD, y + SIDEPAD, 'M');
            Thread.Sleep(timer);
#endif
            for (Direction d = Direction.Up; d <= Direction.Right; d++) {

                var moved = GetMovedXY(d, new Point(x, y));

                b[(int)d] = balls[moved.Y * W + moved.X];
            }
            if (b.All(a => a == Ball.Blue)) {
                return false;
            }
            if (b.Count(a => a == Ball.Blue) == b.Length - 1) {
                return false;
            }
            int m = b.ToList().IndexOf(Ball.Red);
            if (m == -1) {
                return false;
            }
            var o = getOpposite((Direction)m);
            if (b[(int)o] == Ball.Blue) {
                balls[y * W + x] = Ball.Red;

                var l = GetMovedXY(o, new Point(x, y));

                x = l.X;
                y = l.Y;
                goto foos;
            }

            var perp = GetPerp((Direction)m);
            if (b[(int)perp] == Ball.Blue) {
                balls[y * W + x] = Ball.Red;

                var l = GetMovedXY(o, new Point(x, y));
                x = l.X;
                y = l.Y;
                goto foos;
            }

            for (int index = 0; index < b.Length; index++) {
                var ball = b[index];
                if (ball != Ball.Red) {
                    continue;
                }
                var l3 = GetMovedXY(o, new Point(x, y));

                if (l3.X == startx && l3.Y == starty) {
                    var vm = peremeter;//.OrderBy(a => a.Y).OrderBy(a => a.X);

                    bool run = true;
                    foreach (var point in vm.GroupBy(a => a.Y).OrderBy(a => a.Key)) {
                        var f = point.OrderBy(a => a.X).First().X;
                        var l = point.OrderBy(a => a.X).Last().X;
                        foreach (var g in peremeter.Where(a => a.X >= f && a.X <= l).GroupBy(a => a.X).OrderBy(a => a.Key)) {
                            if (!(Balls[point.Key * W + g.Key] == Ball.Blue || Balls[point.Key * W + g.Key] == Ball.Red)) {
                                run = false;
                            }

                        }
                    }
                    if (run) {

                        foreach (var point in vm.GroupBy(a => a.Y).OrderBy(a => a.Key)) {
                            var f = point.OrderBy(a => a.X).First().X;
                            var l = point.OrderBy(a => a.X).Last().X;
                            foreach (var g in peremeter.Where(a => a.X >= f && a.X <= l).GroupBy(a => a.X).OrderBy(a => a.Key)) {
                                Balls[point.Key * W + g.Key] = Ball.Empty;

                                //Draw(startx, starty);
#if DONTDRAW
#else
                                Thread.Sleep(timer);
#endif
                            }
                        }
                    }
                    else {
                        foreach (var point in peremeter) {
                            Balls[point.Y * W + point.X] = Ball.Red;
                            //   Draw(startx, starty);
#if DONTDRAW
#else
                            Thread.Sleep(timer);
#endif
                        }
                    }
                }
                else
                    continue;

                Moves += peremeter.Count;
                return true;
            }

            o = getOpposite(perp);
            if (b[(int)o] == Ball.Blue) {
                balls[y * W + x] = Ball.Red;
                var l = GetMovedXY(o, new Point(x, y));
                x = l.X;
                y = l.Y;
                goto foos;
            }
            // Draw(startx, starty);
            return false;

        }

        public Direction GetPerp(Direction m) {
            switch (m) {
                case Direction.Up:
                case Direction.Down:
                    return Direction.Left;
                    break;
                case Direction.Left:
                case Direction.Right:
                    return Direction.Up;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("m");
            }
        }

        private Point GetMovedXY(Direction d, Point p) {
            var m = p;
            switch (d) {
                case Direction.Up:
                    m.Y = (p.Y + H - 1) % H;
                    return m;
                case Direction.Down:
                    m.Y = (p.Y + H + 1) % H;
                    return m;
                case Direction.Left:
                    m.X = (p.X + W - 1) % W;
                    return m;
                case Direction.Right:
                    m.X = (p.X + W + 1) % W;
                    return m;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            var mj = (int)d;
            m.X = (Vectors[mj].X + p.X + W) % W;
            m.Y = (Vectors[mj].Y + p.Y + H) % H;
            return m;
        }

        private void traceParemeter(Direction last, Point start, Point current) {


        }

        private bool findClosestBlue(Move nnn, Direction previous, ref int[] Map, ref Move[] moves, out int current_) {

            current_ = 0;
            //  Point[] p = new Point[nnn.myPath.Length + 1];
            //  Array.Copy(nnn.myPath, p, nnn.myPath.Length);

            for (Direction d = Direction.Up; d <= Direction.Right; d++) {

                if (previous == d) {
                    continue;
                }
                Point moved = GetMovedXY(d, nnn.point);

                var rd = moved.Y * W + moved.X;
                if (Map[rd] > 0)
                    continue;

                switch (Balls[rd]) {
                    case Ball.Blue:
                        return true;
                        break;
                    case Ball.Red:
                        continue;
                    case Ball.Wall:
                        continue;
                    default:
                        //var map = (bool[,])Map.Clone();
                        Map[rd] = nnn.current;
                        // p[nnn.myPath.Length] = moved;
                        moves[current_++] = new Move() { initial = nnn.initial, current = nnn.current + 1, last = d, point = moved };
                        break;
                }
            }
            return false;
        }

        public class Move {
            // public int[] map;
            public int current;
            public Point point;
            public Direction last;
            public Direction initial;
            public Point[] myPath;
        }
        int SIDEPAD = 0;
        public Action update;

        private void drawMap(int[] map, int max, Point ecur) {
            var cur = ecur;
            cumm++;
            cur.X -= previewW / 2;
            cur.Y -= previewH / 2;

            for (int y = 0; y < previewH; y += 1) {
                for (int x = 0; x < previewW; x += 1) {
                    var v = ((y + cur.Y + H) % H) * W + ((x + cur.X + W) % W);
                    if (map[v] == 0) {
                        continue;
                    }
                    //current[v] = 'X';
                    current[v] = (char)('0' + (map[v] * 9 / max));
                    //current[v] = (char)(((int)'a') + (((map[v] * 26 / max))));
                }
            }




            return;

        }
        int cumm = 0;

        private void drawPath(Point start, Point[] map) {

            foreach (Point point in map) {
                current[point.Y * W + point.X] = 'L';
            }
        }

        [DebuggerStepThrough]
        private Direction getOpposite(Direction direction) {
            int v = (int)direction + 2;
            var c = v % 4;
            return (Direction)(c);
            switch (direction) {
                case Direction.Up:
                    return Direction.Down;
                case Direction.Down:
                    return Direction.Up;
                case Direction.Left:
                    return Direction.Right;
                case Direction.Right:
                    return Direction.Left;
                default:
                    throw new ArgumentOutOfRangeException("direction");
            }
        }
    }
}
