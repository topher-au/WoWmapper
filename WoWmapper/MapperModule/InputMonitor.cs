using SlimDX;
using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using WoWmapper.Input;

namespace WoWmapper.MapperModule
{
    internal class InputMonitor : IDisposable
    {
        private IInputPlugin inputDevice;
        private InputMapper inputMapper;
        private bool[] moveKeys = new bool[4];
        private Thread movementThread, mouseThread;
        public InputMonitor(IInputPlugin Device, InputMapper Mapper)
        {
            inputMapper = Mapper;
            inputDevice = Device;
            movementThread = new Thread(MovementThread);
            mouseThread = new Thread(MouseThread);

            movementThread.Start();
            mouseThread.Start();
        }

        public void Dispose()
        {
            movementThread.Abort();
            mouseThread.Abort();
        }

        private double CalculateMagnitude(Vector2 Vector)
        {
            return Math.Sqrt(Vector.X * Vector.X + Vector.Y * Vector.Y);
        }

        private string GetKeyName(int i)
        {
            switch (i)
            {
                case 0:
                    return MoveBindName.Forward;

                case 1:
                    return MoveBindName.Backward;

                case 2:
                    return MoveBindName.Left;

                case 3:
                    return MoveBindName.Right;
            }
            return string.Empty;
        }

        private void MouseThread()
        {
            while (true)
            {
                int fRightSpeed = 0;
                if (inputDevice != null)
                {
                    // TODO Do sticky cursor check

                    Vector2 rightStick = new Vector2(inputDevice.GetAxis(InputAxis.RightStickX), inputDevice.GetAxis(InputAxis.RightStickY));
                    var curPos = Cursor.Position;

                    var fRightDead = inputDevice.RightDead;
                    fRightSpeed = (int)inputDevice.RightSpeed;
                    var fRightCurve = inputDevice.RightCurve;

                    double dRightMag = CalculateMagnitude(rightStick);

                    if (dRightMag > fRightDead)
                    {
                        Vector2 rightNorm = rightStick;
                        rightNorm.Normalize();
                        rightNorm *= (float)((dRightMag - fRightDead) / (1 - fRightDead));

                        float rightX = (rightNorm.X / 10) * (rightNorm.X < 0 ? -1 : 1);
                        float rightY = (rightNorm.Y / 10) * (rightNorm.Y < 0 ? -1 : 1);

                        double mathX = Math.Pow(fRightCurve * rightX, 2) + (fRightCurve * rightX);
                        double mathY = Math.Pow(fRightCurve * rightY, 2) + (fRightCurve * rightY);

                        if (rightStick.X < 0) mathX *= -1;
                        if (rightStick.Y < 0) mathY *= -1;

                        curPos.X += (int)mathX;
                        curPos.Y += (int)mathY;

                        Cursor.Position = curPos;
                    }
                }

                Thread.Sleep((11 - fRightSpeed));
            }
        }

        private void MovementThread()
        {
            while (true)
            {
                if (inputDevice != null)
                {
                    bool[] thisMove = new bool[4];

                    Point leftStick = new Point(inputDevice.GetAxis(InputAxis.LeftStickX), inputDevice.GetAxis(InputAxis.LeftStickY));

                    if (leftStick.Y < -40)
                    {
                        thisMove[(int)InputMapper.Direction.Forward] = true;
                    }

                    if (leftStick.Y > 40)
                    {
                        thisMove[(int)InputMapper.Direction.Backward] = true;
                    }

                    if (leftStick.X < -40)
                    {
                        thisMove[(int)InputMapper.Direction.Left] = true;
                    }

                    if (leftStick.X > 40)
                    {
                        thisMove[(int)InputMapper.Direction.Right] = true;
                    }

                    for (int i = 0; i < 4; i++)
                    {
                        if (thisMove[i] && !moveKeys[i])
                        {
                            // send key down
                            inputMapper.SendKeyDown(inputDevice.Keybinds.FromName(GetKeyName(i)).Key.Value);
                            moveKeys[i] = true;
                        }
                        if (!thisMove[i] && moveKeys[i])
                        {
                            // send key up
                            inputMapper.SendKeyUp(inputDevice.Keybinds.FromName(GetKeyName(i)).Key.Value);
                            moveKeys[i] = false;
                        }
                    }

                    Thread.Sleep(10);
                }
            }
        }
        public static class MoveBindName
        {
            public static string Backward = "LStickDown";
            public static string Forward = "LStickUp";
            public static string Left = "LStickLeft";
            public static string Right = "LStickRight";
        }
    }
}