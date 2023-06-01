using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using ToolsStoreLab.View;

namespace ToolsStoreLab
{
    public static class MyContentControl
    {       
        public static Frame myContent;

        public static void SetFrame(Frame content)
        {
            myContent = content;
        }     

        public static Frame GetFrame()
        {
            return myContent;
        }
    }
}
