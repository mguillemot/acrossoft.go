using System.Collections.Generic;

namespace Acrossoft.Engine.Console
{
    public static class Console
    {
        private static readonly LinkedList<string> s_lines = new LinkedList<string>();
        private static int s_lineWidth = 80;

        public static int LineWidth
        {
            get { return s_lineWidth; }
            set { s_lineWidth = value; }
        }

        public static void WriteLine(string text, params object[] p)
        {
            text = string.Format(text, p);
            System.Console.WriteLine("CONSOLE: " + text);
            while (text.Length > LineWidth)
            {
                s_lines.AddLast(text.Substring(0, LineWidth));
                text = text.Substring(LineWidth, text.Length - LineWidth);
            }
            s_lines.AddLast(text);
            while (s_lines.Count > 100)
            {
                s_lines.RemoveFirst();
            }
        }

        internal static string GetFormattedText(int maxLines)
        {
            string result = "";
            var node = s_lines.Last;
            if (node == null)
            {
                return result;
            }
            for (int i = 0; i < maxLines; i++)
            {
                if (node.Previous == null)
                {
                    break;
                }
                node = node.Previous;
            }
            while (node != null)
            {
                result += node.Value + "\n";
                node = node.Next;
            }
            return result;
        }
    }
}