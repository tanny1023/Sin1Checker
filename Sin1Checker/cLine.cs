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
        #endregion
    }
}

