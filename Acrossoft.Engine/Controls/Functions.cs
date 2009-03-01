using System.Collections.Generic;

namespace Acrossoft.Engine.Controls
{
    public static class Functions
    {
        private static int s_nextFreeFunctionId = 1;
        private static readonly Dictionary<int, string> s_allFunctions = new Dictionary<int, string>();

        public static ICollection<int> All
        {
            get { return s_allFunctions.Keys; }
        }

        public static int RegisterNewFunction(string functionName)
        {
            int functionId = s_nextFreeFunctionId++;
            s_allFunctions[functionId] = functionName;
            return functionId;
        }
    }
}