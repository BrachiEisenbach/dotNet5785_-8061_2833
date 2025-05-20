using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlApi
{
    public interface IAdmin
    {
        public DateTime GetClock();
        public void ClockPromotion(TIMEUNIT unit); 
        public TimeSpan GetRiskRange();
        public void SetRiskRange(TimeSpan riskRange);
        public void ResetDB();
        public void initializeDB();

        #region Stage 5
        void AddConfigObserver(Action configObserver);
        void RemoveConfigObserver(Action configObserver);
        void AddClockObserver(Action clockObserver);
        void RemoveClockObserver(Action clockObserver);
        void SetClock(DateTime currentTime);
        #endregion Stage 5

    }
}
