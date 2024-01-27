using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Security.Authentication.ExtendedProtection;
using System.Text;
using System.Threading.Tasks;

namespace Robots
{
    public enum MyColors
    {
        Red,
        Green,
        Blue
    }
    internal class Robot
    {
       
        public MyColors _Color;
        public bool _isWorking;
        public int _num;
        public int _ms;
        public long _timeElapsed;
        private void workP()
        {
            Stopwatch sp = new Stopwatch();
            
            while (!(SharedData.CancellationTokenSource.Token).IsCancellationRequested)
            {
                sp.Restart();
                Tuple<int, MyColors, int> tup = new Tuple<int, MyColors, int>(_num, _Color, _ms); ////numer, color, ms, onbject
                _isWorking = true;
                (SharedData.mainQueue).Enqueue(tup);
                (SharedData.eventToCall[_num]).Reset();
                (SharedData.eventToCall[_num]).WaitOne();
                Thread.Sleep(_ms);
                _isWorking = false;
                switch (_Color)
                {
                    case MyColors.Red:
                        Scheduler._timeRed += (double)sp.ElapsedMilliseconds / (long)1000;
                        break;
                    case MyColors.Green:
                        Scheduler._timeGreen += (double)sp.ElapsedMilliseconds / (long)1000;
                        break;
                    case MyColors.Blue:
                        Scheduler._timeBlue += (double)sp.ElapsedMilliseconds / (long)1000;
                        break;
                }
                //sp.Reset();
                
                Debug.WriteLine("Continue robot: " + _Color);
            }
        }


        public Robot(MyColors Color, int num, int ms)
        {
            _Color = Color;
            _num = num;
            _ms = ms;
            Thread work = new Thread(() => workP());
            work.Start();
        }
    }
}
