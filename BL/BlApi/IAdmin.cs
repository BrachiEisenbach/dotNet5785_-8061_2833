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
        //public void ClockPromotion()// חסר ערך ENUM שמציין שעה דקה שניה
        public TimeSpan GetRiskRange();//?????
        public void DefinitionRiskRange(TimeSpan riskRange);
        public void ResetDB();
        public void initializDB();

    }
}
