using System.Collections.Generic;
using System.Text;
using System.Linq;
using System;

namespace TwitchPlaysArmA3.Functions
{
    static class FunctionCaller
    {
        private static TwitchBot _bot = new TwitchBot();
        private static IDictionary<string, IFunction> _functionMap = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(x => x.GetTypes())
            .Where(x => typeof(IFunction).IsAssignableFrom(x) && !x.IsAbstract)
            .Select(x => (IFunction) Activator.CreateInstance(x))
            .Select(x => { x.SetBot(_bot); return x; })
            .ToDictionary(x => x.GetName());

        public static void Call(string function, StringBuilder output)
        {
            if (!_functionMap.TryGetValue(function, out var toCall))
            {
                output.Append($"Function {function} not defined. Defined functions are: {string.Join(", ", _functionMap.Keys)}");
                return;
            }

            toCall.Call(output, new string[] { });
        }

        public static int Call(string function, StringBuilder output, string[] arguments)
        {
            if (!_functionMap.TryGetValue(function, out var toCall))
            {
                output.Append($"Function {function} not defined. Defined functions are: {string.Join(", ", _functionMap.Keys)}");
                return -1;
            }

            return toCall.Call(output, arguments);
        }
    }
}
