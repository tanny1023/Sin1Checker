using System.Collections.Generic;

namespace Sin1Checker
{
    public class cLine
    {
        #region Property
        private int Number;
        private int StartPtNumeber;
        private int EndPtNumeber;
        private string Property;
        #endregion

        #region Constructor
        public cLine(int number, int startPt, int endPt)
        {
            Number = number;
            StartPtNumeber = startPt;
            EndPtNumeber = endPt;
        }
        #endregion

        #region Method
        //--加入屬性
        public void SetLineProperty(int number, string property)
        {
            if (Number == number)
                Property = property;
        }
        //--顯示Line
        public string PrintLine()
        {
            string line = "Line(" + Number + ")={" + StartPtNumeber + "," + EndPtNumeber + "};";
            return line;
        }
        //--輸出起點與終點間的線集合
        public List<cLine> GetLineList(int endNumber)
        {
            List<cLine> lineList = new List<cLine>();
            int stepNumber = endNumber - Number;
            for (int i = 1; i <= stepNumber; i++)
            {
                int number = Number + i;
                int startPt = StartPtNumeber+i;
                int endPt = startPt + 1;
                lineList.Add(new cLine(number, startPt, endPt));
            }
            return lineList;
        }
        #endregion
    }
}

