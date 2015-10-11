using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Console3D
{
    class Program
    {
        static Camera SceneCamera;
        static char[,,] Matrix;
        static int timer;

        static void Main(string[] args)
        {
            Matrix = new char[20, 20, 20];
            for (int i = 0; i < 20; i++)
            {
                for (int o = 0; o < 20; o++)
                {
                    for (int p = 0; p < 20; p++)
                    {
                        /*if ((i == 0 && o == 9) || (i == 9 && o == 9) || (i == 0 && o == 0) || (i == 9 && o == 0) ||
                            (i == 0 && p == 9) || (i == 9 && p == 9) || (i == 0 && p == 0) || (i == 9 && p == 0) ||
                            (p == 0 && o == 9) || (p == 9 && o == 9) || (p == 0 && o == 0) || (p == 9 && o == 0)
                            )
                        */
                        if((int)Math.Sqrt(Math.Pow(i, 2) + Math.Pow(o,2) + Math.Pow(p, 2)) == 10)
                        {
                            Matrix[i, o, p] = '*';
                        }
                        else
                        {
                            Matrix[i, o, p] = ' ';
                        }
                    }
                }
            }
            SceneCamera = new Camera(new Vector3(-30, 5, 5),new Vector3(0, 0, (float)Math.PI/2));
            ConsoleKeyInfo Input;
            while(1==1)
            {
                if (Console.KeyAvailable)
                {
                    Input = Console.ReadKey(true);
                    if (Input.Key == ConsoleKey.LeftArrow) { SceneCamera.Rotation.X += -(float)Math.PI / 80f; }
                    else if (Input.Key == ConsoleKey.RightArrow) { SceneCamera.Rotation.X += (float)Math.PI/80f; }
                    else if (Input.Key == ConsoleKey.UpArrow) { SceneCamera.Rotation.Z += (float)Math.PI / 80f; }
                    else if (Input.Key == ConsoleKey.DownArrow) { SceneCamera.Rotation.Z += -(float)Math.PI / 80f; }
                    else if (Input.Key == ConsoleKey.A) { SceneCamera.Position.Y += 0.2f; }
                    else if (Input.Key == ConsoleKey.D) { SceneCamera.Position.Y += -0.2f; }
                    else if (Input.Key == ConsoleKey.W) { SceneCamera.Position.X += 0.2f; }
                    else if (Input.Key == ConsoleKey.S) { SceneCamera.Position.X += -0.2f; }

                }
                DrawScene();
                Thread.Sleep(200);
            }
        }
        static void DrawScene()
        {
            string drawBuffer = "";
            timer++;
            for (int i = 0; i < 25; i++)
            {
                for (int p = 0; p < 79; p++)
                {
                    if(timer > 9)
                    {
                        timer = 0;
                    }
                    if(i == 1 && p == 1)
                    {
                        drawBuffer += timer;
                    }
                    else
                    {
                        if (i == 0 || p == 0 || i == 24 || p == 78)
                        {
                            drawBuffer += "X";
                        }
                        else if (CheckRay(SceneCamera.Position, SceneCamera.Rotation, p, i))
                        {
                            drawBuffer += "*";
                        }
                        else
                        {
                            drawBuffer += " ";
                        }
                    }
                }
                if(i != 24)
                {
                    drawBuffer += "\n";
                }
            }
            Console.Clear();
            Console.Write(drawBuffer);
        }
        static bool CheckRay(Vector3 position, Vector3 rotation, int column, int row)
        {
            float Xoffset = ((column - 39f) * (45f / 39f) * (float)Math.PI / 180f);
            float Zoffset = ((row - 12f) * (45f / 12f) * (float)Math.PI / 180f);
            rotation.X += Xoffset;
            rotation.Z += Zoffset;
            Vector3 coord = new Vector3(0, 0, 0);
            for (int i = 0; i < 50; i++)
            {
                if(rotation.X > 0)
                {
                    coord.X = (int)(i * Math.Cos(rotation.X) * Math.Sin(rotation.Z) + SceneCamera.Position.X);
                }
                else
                {
                    coord.X = -(int)(i * Math.Cos(rotation.X) * Math.Sin(rotation.Z) + SceneCamera.Position.X);
                }
                if(rotation.Z > 0)
                {
                    coord.Y = (int)(i * Math.Sin(rotation.X) * Math.Sin(rotation.Z) + SceneCamera.Position.Y);
                }
                else
                {
                    coord.Y = -(int)(i * Math.Sin(rotation.X) * Math.Sin(rotation.Z) + SceneCamera.Position.Y);
                }
                coord.Z = (int)(i * Math.Cos(rotation.Z) + SceneCamera.Position.Z);
                if (Matrix.GetLength(0) > coord.X && Matrix.GetLength(1) > coord.Y && Matrix.GetLength(2) > coord.Z && coord.X >= 0 && coord.Y >= 0 && coord.Z >= 0)
                {
                    if (Matrix[(int)coord.X, (int)coord.Y, (int)coord.Z] != ' ')
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        struct Vector3
        {
            public Vector3(float x, float y, float z)
            {
                this.X = x;
                this.Y = y;
                this.Z = z;
            }
            public float X;
            public float Y;
            public float Z;
        }
        struct Camera
        {
            public Camera(Vector3 position, Vector3 rotation)
            {
                this.Position = position;
                this.Rotation = rotation;
            }
            public Vector3 Position;
            public Vector3 Rotation;
        }
    }
}
