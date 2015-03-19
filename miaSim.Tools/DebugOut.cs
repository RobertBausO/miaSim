using System;
using System.Collections.Generic;

namespace miaGame.Tools
{
    public static class DebugOut
    {
        private static IList<string> mList = new List<string>();

        [ThreadStatic]
        private static DateTime? mLastWriteLine;

        public static void WriteLine(string format, params object[] args)
        {
            var text = string.Format(format, args);
            WriteLine(text);
        }

        public static void WriteLine(string text)
        {
            lock(mList)
            {
                var diff = 0;

                if (mLastWriteLine.HasValue)
                {
                    diff = (int)DateTime.Now.Subtract(mLastWriteLine.Value).TotalMilliseconds;
                }

                mList.Add(string.Format("{0}@{1} ({2}): {3}", 
                    DateTime.Now.ToString("HH:mm:ss.fff"), 
                    System.Threading.Thread.CurrentThread.ManagedThreadId.ToString("00"), 
                    diff.ToString("0000"),
                    text));

                mLastWriteLine = DateTime.Now;
            }
        }

        public static void Flush()
        {
            lock(mList)
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\temp\DebugOut.txt"))
                {
                    foreach (string line in mList)
                    {
                        file.WriteLine(line);
                    }
                }
            }
        }
    }
}
