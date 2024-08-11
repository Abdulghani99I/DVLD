using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Classes
{
    public class clsFormat
    {
        public static string DateToShort(DateTime Dt1)
        {
            return Dt1.ToString("dd/MMM/yyyy");
        } 

        public static void StartPositionBottomCenterMainScreen(Form frm)
        {
            // الحصول على حجم الشاشة
            int screenWidth = Screen.PrimaryScreen.WorkingArea.Width;
            int screenHeight = Screen.PrimaryScreen.WorkingArea.Height;

            // حساب موقع الفورم ليكون في منتصف العرض وفي الأسفل
            int formWidth = frm.Width;
            int formHeight = frm.Height;

            int x = (screenWidth - formWidth) / 2;  // منتصف العرض
            int y = screenHeight - formHeight;      // أسفل الشاشة

            // تعيين موقع الفورم
            frm.Location = new Point(x, y);
        } 
    }
}
