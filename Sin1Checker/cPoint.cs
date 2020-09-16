using System.Windows.Forms;

namespace Sin1Checker
{
    public class cPoint
    {
        #region Property
        private int Number;
        private double X;
        private double Y;
        private double Z;
        #endregion
        #region Constructor
        public cPoint(int number, double x, double y, double z)
        {
            Number = number;
            X = x;
            Y = y;
            Z = z;
            //ShowPt();
        }
        #endregion
        #region Method
        public string PrintPt()
        {
            string point = "Point(" + Number + ")={" + X + "," + Y + "," + Z + "};";
            return point;
        }
        #endregion
    }
}
