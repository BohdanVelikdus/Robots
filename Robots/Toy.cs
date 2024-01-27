using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace Robots
{
    internal class Toy
    {
        private bool _isRed;
        private bool _isBlue;
        private bool _isGreen;
        public bool  _isUsed;


        public void wt(int time) {
            Thread.Sleep(time);
            _isUsed = false;
        }

        public void IsUsedFlag(int time) {
            Thread thr = new Thread(() => wt(time));
            thr.Start();
        }

        // Getter and setter for IsRed
        public bool IsRed
        {
            get { return _isRed; }
            set { _isRed = value; }
        }

        // Getter and setter for IsBlue
        public bool IsBlue
        {
            get { return _isBlue; }
            set { _isBlue = value; }
        }

        // Getter and setter for IsGreen
        public bool IsGreen
        {
            get { return _isGreen; }
            set { _isGreen = value; }
        }


        // Method to activate the red color
        public void ActivateRed()
        {
            IsRed = true;
        }

        // Method to activate the blue color
        public void ActivateBlue()
        {
            IsBlue = true;
        }

        // Method to activate the green color
        public void ActivateGreen()
        {
            IsGreen = true;
        }
    }
}
