using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Robots
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public static class SharedData {
        public static CancellationTokenSource CancellationTokenSource = new CancellationTokenSource();
        public static List<AutoResetEvent> eventToCall = new List<AutoResetEvent>();
        public static Queue<Tuple<int, MyColors, int>> mainQueue = new Queue<Tuple<int, MyColors, int>>();
        
    }

    public partial class MainWindow : Window
    {
        //public static Queue<Tuple<int, MyColors, int, object>> mainQueue; //numer, color, ms, onbject

       
     
        
        public MainWindow()
        {
            InitializeComponent();
        }
        int msRed;
        int msBlue;
        int msGreen;

        int cR;
        int cB;
        int cG;
        int count;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            


        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            msRed = Convert.ToInt32(TimeRed.Text);
            msBlue = Convert.ToInt32(TimeBlue.Text);
            msGreen = Convert.ToInt32(TimeGreen.Text);
            cR = Convert.ToInt32(RedBotsCount.Text);
            cB = Convert.ToInt32(BlueBotsCount.Text);
            cG = Convert.ToInt32(GreenBotsCount.Text);
            count = Convert.ToInt32(Count.Text);

            List<Toy> toy = new List<Toy>();
            List<Robot> robots = new List<Robot>();
            int numerFroSync = 0;

            for (int i = 0; i < count; i++)
            {
                Toy t = new Toy();
                toy.Add(t);
            }

            for (int i = 0; i < cR + cB + cG; i++)
            {
                SharedData.eventToCall.Add(new AutoResetEvent(false));
            }

            for (int i = 0; i < cR; i++)
            {
                Robot n = new Robot(MyColors.Red, numerFroSync, msRed);
                robots.Add(n);
                numerFroSync++;
            }
            for (int i = 0; i < cB; i++)
            {
                Robot n = new Robot(MyColors.Blue, numerFroSync, msBlue);
                robots.Add(n);
                numerFroSync++;
            }
            for (int i = 0; i < cG; i++)
            {
                Robot n = new Robot(MyColors.Green, numerFroSync, msGreen);
                robots.Add(n);
                numerFroSync++;
            }

            Scheduler sch = new Scheduler(this, ref robots, ref toy);
            Thread add = new Thread(() => sch.stats(this));
            add.Start();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            SharedData.CancellationTokenSource.Cancel();
        }
    }
}