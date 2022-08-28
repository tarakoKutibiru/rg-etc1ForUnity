using System;

namespace TarakoKutibiru.RG_ETC1.Samples
{
    public class StopWatch
    {
        DateTime startTime;
        DateTime stopTime;

        public void Start()
        {
            this.startTime = DateTime.Now;
        }

        public int Stop()
        {
            this.stopTime = DateTime.Now;
            return (this.stopTime - this.startTime).Milliseconds;
        }
    }
}
