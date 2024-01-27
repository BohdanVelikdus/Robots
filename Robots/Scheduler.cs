using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Robots
{
    internal class Scheduler
    {
        public double _time = 0;
        public static double _timeRed = 0;
        public int  _rBU = 0;     
        public static double _timeGreen = 0;
        public int  _gBU = 0;
        public static double _timeBlue = 0;
        public int  _bBU = 0;
        public bool _frt = true;
        public bool _TWFlag = true;
        public List<Robot> robots;
        public List<Toy> toy;
        
       
        public void stats(MainWindow mainWindow) {
            while (!SharedData.CancellationTokenSource.Token.IsCancellationRequested) {
                int r = 0, b = 0, g = 0;
                foreach (var rb in robots)
                {
                    switch (rb._Color)
                    {
                        case MyColors.Red:
                            r += (rb._isWorking) ? 1 : 0;
                        break;
                        case MyColors.Green:
                            g += (rb._isWorking) ? 1 : 0;
                        break;
                        case MyColors.Blue:
                            b += (rb._isWorking) ? 1 : 0;
                        break;
                    }
                }
                _rBU = r;
                _gBU = g;
                _bBU = b;

                mainWindow.Dispatcher.Invoke(() => {
                    var timeEl = (Label)mainWindow.FindName("LTimeElapsed");
                    timeEl.Content = _time.ToString();
                    var timeR = (Label)mainWindow.FindName("LProcessedRed");
                    timeR.Content = (_timeRed).ToString();
                    var timeB = (Label)mainWindow.FindName("LProcessedBlue");
                    timeB.Content = (_timeBlue).ToString();
                    var timeG = (Label)mainWindow.FindName("LProcessedGreen");
                    timeG.Content = (_timeGreen ).ToString();
                    var remainItems = (Label)mainWindow.FindName("LLeft");
                    remainItems.Content = toy.Count.ToString();
                    var Rr = (Label)mainWindow.FindName("LRed");
                    Rr.Content = r.ToString() + "/All";
                    var Rb = (Label)mainWindow.FindName("LBlue");
                    Rb.Content = b.ToString() + "/All";
                    var Rg = (Label)mainWindow.FindName("LGreen");
                    Rg.Content = g.ToString() + "/All";
                });
                if (toy.Count == 0)
                {
                    SharedData.CancellationTokenSource.Cancel();
                }

                Debug.WriteLine("Continue stats thread");
                Thread.Sleep(1000/60);
            }
        }
        public void wr(MainWindow mainWindow) {
            Stopwatch sp = new Stopwatch();
            int offset = 0;
            sp.Start();
            while (_TWFlag && !SharedData.CancellationTokenSource.Token.IsCancellationRequested)
            {
                Debug.WriteLine("Continue scheduler thread");

                if (SharedData.mainQueue.Count == 0)
                {
                    Thread.Sleep(1);
                    continue;

                }
                var call = (SharedData.mainQueue).Dequeue();
                if (call == null) continue;
                int numb = call.Item1;
                MyColors color = call.Item2;
                int ms = call.Item3;
                bool flg = false;
                int ifNT = 0;



                




                switch (color)
                {
                    case MyColors.Blue:


                        do
                        {
                            offset++;
                            if (offset >= toy.Count) offset = 0;
                            ifNT++;
                        } while ( (toy[offset]._isUsed || toy[offset].IsBlue) && ifNT < toy.Count() );
                        if (ifNT  == toy.Count()) {
                            break;
                        }

                        //if ( )
                        //{
                            flg = true;
                            toy[offset].IsBlue = true;
                            toy[offset]._isUsed = true;
                            toy[offset].IsUsedFlag(ms);
                            (SharedData.eventToCall[numb]).Set();
                            _time = (double)sp.ElapsedMilliseconds / (long)1000;

                            
                        //}
                        break;
                    case MyColors.Green:
                        do
                        {
                            offset++;
                            if (offset >= toy.Count) offset = 0;
                            ifNT++;
                        } while ((toy[offset]._isUsed || toy[offset].IsGreen) && ifNT < toy.Count());
                        if (ifNT == toy.Count())
                        {
                            break;
                        }

                        //if (!toy[offset].IsGreen)
                        //{
                            flg = true;
                            toy[offset].IsGreen = true;
                            toy[offset]._isUsed = true;
                            toy[offset].IsUsedFlag(ms);
                            (SharedData.eventToCall[numb]).Set();
                            _time = (double)sp.ElapsedMilliseconds / (long)1000;
                        //}
                        break;
                    case MyColors.Red:
                        do
                        {
                            offset++;
                            if (offset >= toy.Count) offset = 0;
                            ifNT++;
                        } while ((toy[offset]._isUsed || toy[offset].IsRed) && ifNT < toy.Count());
                        if (ifNT == toy.Count())
                        {
                            break;
                        }

                        //if (!toy[offset].IsRed)
                        //{
                            flg = true;
                            toy[offset].IsRed = true;
                            toy[offset]._isUsed = true;
                            toy[offset].IsUsedFlag(ms);
                            (SharedData.eventToCall[numb]).Set();
                            _time = (double)sp.ElapsedMilliseconds / (long)1000;
                        //}
                        break;
                }
                if (toy[offset].IsBlue && toy[offset].IsRed && toy[offset].IsGreen)
                {
                    toy.RemoveAt(offset);
                    offset = (offset == 0) ? 0: --offset;

                }
                if (!flg)
                {
                    (SharedData.mainQueue).Enqueue(call);
                }
                _frt = !_frt;       
                if (toy.Count == 0)
                {
                    //(SharedData.CancellationTokenSource).Cancel();
                    _TWFlag = false;
                    
                }
                Thread.Sleep(3);
                
            } 
        }
        public Scheduler(MainWindow mainWindow,ref List<Robot> robots, ref List<Toy> toy) {
            this.robots = robots;
            this.toy = toy;
            Thread thr = new Thread(() => wr(mainWindow));
            thr.Start();
        }
    }
}
