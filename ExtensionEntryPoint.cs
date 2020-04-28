using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using TwitchPlaysArmA3.Functions;

namespace TwitchPlaysArmA3
{
    public delegate int ExtensionCallback([MarshalAs(UnmanagedType.LPStr)] string name, [MarshalAs(UnmanagedType.LPStr)] string function, [MarshalAs(UnmanagedType.LPStr)] string data);

    public class ExtensionEntryPoint
    {
        [DllExport("RVExtensionRegisterCallback", CallingConvention = CallingConvention.Winapi)]
        public static void RVExtensionRegisterCallback([MarshalAs(UnmanagedType.FunctionPtr)] ExtensionCallback func)
        {
            FunctionCaller.SetCallback(func);
        }

        [DllExport("RVExtensionVersion", CallingConvention = CallingConvention.Winapi)]
        public static void RvExtensionVersion(StringBuilder output, int outputSize)
        {
            output.Append("TwitchPlaysArma3 v0.0.1");
        }

        /*
         *  Function: start
         *  Starts the bot
         *  
         *  Function: stop
         *  Stop the bot
         */
        [DllExport("RVExtension", CallingConvention = CallingConvention.Winapi)]
        public static void RvExtension(StringBuilder output, int outputSize,
            [MarshalAs(UnmanagedType.LPStr)] string function)
        {
            FunctionCaller.Call(function, output);
        }

        /*
         *  Function: configure_twitch_chat
         *  Arguments: [string twitch_username, string access_token]
         *  
         *  Function: configure_bot_action_interval
         *  Arguments: action_interval
         *  
         *  Function: add_spawn_class
         *  Arguments: [string class_name, int area_count, int type_count, string[] areas, string[] types] (flattened)
         *  
         */
        [DllExport("RVExtensionArgs", CallingConvention = CallingConvention.Winapi)]
        public static int RvExtensionArgs(StringBuilder output, int outputSize,
            [MarshalAs(UnmanagedType.LPStr)] string function,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr, SizeParamIndex = 4)] string[] args, int argCount)
        {
            return FunctionCaller.Call(function, output, args);
        }
    }
}
