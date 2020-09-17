using System.Collections.Generic;

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
        //--輸出點
        public string PrintPt()
        {
            string point = "Point(" + Number + ")={" + X + "," + Y + "," + Z + "};";
            return point;
        }
        //--輸出起點與終點間的點集合
        public List<cPoint> GetPointList(int endNumber, double endX, double endY, double endZ)
        {
            List<cPoint> pointList = new List<cPoint>();
            int stepNumber = endNumber - Number;
            for (int i = 1; i <= stepNumber; i++)
            {
                int number = Number + i;
                double x = X + i * ((endX - X) / stepNumber);
                double y = Y + i * ((endY - Y) / stepNumber);
                double z = Z + i * ((endZ - Z) / stepNumber);
                pointList.Add(new cPoint(number, x, y, z));
            }
            return pointList;
        }

        #endregion
    }
}
