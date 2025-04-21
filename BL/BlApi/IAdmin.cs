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
        public DateTime GetTime();
        public void ClockPromotion(TIMEUNIT unit); 
        public int GetRiskRange();//?????
        public void SetRiskRange(TimeSpan riskRange);
        public void ResetDB();
        public void initializeDB();

    }
}
