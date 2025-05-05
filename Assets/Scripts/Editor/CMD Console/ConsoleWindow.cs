using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace CatchFire
{
    [DisallowMultipleComponent]
    [AddComponentMenu("Editor/Console Window")]
    public class ConsoleWindow : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] TargetConsoleBuild targetBuild = TargetConsoleBuild.Development;
        [SerializeField] ShowStackTrace showStackTrace = ShowStackTrace.Exception;
        [SerializeField] bool includeLogType = false;
        [SerializeField] bool includeTimestamp = true;
        [Header("Log Colours")]
        [SerializeField] Color32 infoTxtColour = Color.white;
        [SerializeField] Color32 warningTxtColour = Color.yellow;
        [SerializeField] Color32 errorTxtColour = Color.red;
        [SerializeField] Color32 exceptionTxtColour = Color.red;
        [SerializeField] Color32 assertTxtColour = Color.white;
        [SerializeField] Color32 stackTraceTxtColour = Color.red;
        ConsoleColor ccInfoTxtColour;
        ConsoleColor ccWarningTxtColour;
        ConsoleColor ccErrorTxtColour;
        ConsoleColor ccExceptionTxtColour;
        ConsoleColor ccAssertTxtColour;
        ConsoleColor ccStackTraceTxtColour;
        StreamWriter writer;
        readonly Dictionary<ConsoleColor, Color32> consoleColours = new()
        {
            { ConsoleColor.Black, new Color32(0, 0, 0, 255) },
            { ConsoleColor.DarkBlue, new Color32(0, 0, 139, 255) },
            { ConsoleColor.DarkGreen, new Color32(0, 100, 0, 255) },
            { ConsoleColor.DarkCyan, new Color32(0, 139, 139, 255) },
            { ConsoleColor.DarkRed, new Color32(139, 0, 0, 255) },
            { ConsoleColor.DarkMagenta, new Color32(139, 0, 139, 255) },
            { ConsoleColor.DarkYellow, new Color32(184, 134, 11, 255) },
            { ConsoleColor.Gray, new Color32(190, 190, 190, 255) },
            { ConsoleColor.DarkGray, new Color32(105, 105, 105, 255) },
            { ConsoleColor.Blue, new Color32(0, 0, 255, 255) },
            { ConsoleColor.Green, new Color32(0, 255, 0, 255) },
            { ConsoleColor.Cyan, new Color32(0, 255, 255, 255) },
            { ConsoleColor.Red, new Color32(255, 0, 0, 255) },
            { ConsoleColor.Magenta, new Color32(255, 0, 255, 255) },
            { ConsoleColor.Yellow, new Color32(255, 255, 0, 255) },
            { ConsoleColor.White, new Color32(255, 255, 255, 255) },
        };
#pragma warning disable UDR0001 // Domain Reload Analyzer
        static ConsoleWindow _instance;
#pragma warning restore UDR0001 // Domain Reload Analyzer

        public static ConsoleWindow Instance => _instance;
        public Color32 InfoTxtColour
        {
            get => infoTxtColour;
            set
            {
                infoTxtColour = value;
                ccInfoTxtColour = GetClosestConsoleColour(infoTxtColour);
            }
        }
        public Color32 WarningTxtColour
        {
            get => warningTxtColour;
            set
            {
                warningTxtColour = value;
                ccWarningTxtColour = GetClosestConsoleColour(warningTxtColour);
            }
        }
        public Color32 ErrorTxtColour
        {
            get => errorTxtColour;
            set
            {
                errorTxtColour = value;
                ccErrorTxtColour = GetClosestConsoleColour(errorTxtColour);
            }
        }
        public Color32 ExceptionTxtColour
        {
            get => exceptionTxtColour;
            set
            {
                exceptionTxtColour = value;
                ccExceptionTxtColour = GetClosestConsoleColour(exceptionTxtColour);
            }
        }
        public Color32 AssertTxtColour
        {
            get => assertTxtColour;
            set
            {
                assertTxtColour = value;
                ccAssertTxtColour = GetClosestConsoleColour(assertTxtColour);
            }
        }
        public Color32 StackTraceTxtColour
        {
            get => stackTraceTxtColour;
            set
            {
                stackTraceTxtColour = value;
                ccStackTraceTxtColour = GetClosestConsoleColour(stackTraceTxtColour);
            }
        }
        public bool IncludeLogType { get => includeLogType; set => includeLogType = value; }
        public bool IncludeTimestamp { get => includeTimestamp; set => includeTimestamp = value; }
        public ShowStackTrace ShowStackTrace { get => showStackTrace; set => showStackTrace = value; }
        public bool Pause { get; set; } = false;

#if UNITY_STANDALONE_WIN && !UNITY_EDITOR
        [DllImport("kernel32.dll")]
        static extern bool AllocConsole();
        [DllImport("kernel32.dll")]
        static extern bool FreeConsole();
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        static extern bool SetConsoleTitle(string lpConsoleTitle);
#endif

        public void Awake()
        {
            // init singleton to ensure only one console instance exists
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(this);
                return;
            }

            // check if console is allowed and this is enabled
            if (!IsBuildAllowed() || !enabled)
                return;

#if UNITY_STANDALONE_WIN && !UNITY_EDITOR
            // allocate new window console
            AllocConsole();
            consoleAllocated = true;

            if (!string.IsNullOrEmpty(consoleWindowName))
                SetConsoleTitle(consoleWindowName);
#endif

            // create writer and direct output to console
            writer = new StreamWriter(Console.OpenStandardOutput())
            {
                AutoFlush = true
            };
            Console.SetOut(writer);

            // convert colours to the cc varients
            ccInfoTxtColour = GetClosestConsoleColour(infoTxtColour);
            ccWarningTxtColour = GetClosestConsoleColour(warningTxtColour);
            ccErrorTxtColour = GetClosestConsoleColour(errorTxtColour);
            ccExceptionTxtColour = GetClosestConsoleColour(exceptionTxtColour);
            ccAssertTxtColour = GetClosestConsoleColour(assertTxtColour);
            ccStackTraceTxtColour = GetClosestConsoleColour(stackTraceTxtColour);

            // listen to log messages
            Application.logMessageReceived += HandleLog;

            // cleanup console on app exit in case of no monobehaviour exit method called
            AppDomain.CurrentDomain.ProcessExit += (sender, e) =>
            {
                CleanupConsole();
            };
        }

        void OnApplicationQuit() => CleanupConsole();

        public void OnDestroy() => CleanupConsole();

        void CleanupConsole()
        {
            Application.logMessageReceived -= HandleLog;

            if (writer != null)
            {
                writer.Flush();
                writer.Close();
                writer = null;
            }

#if UNITY_STANDALONE_WIN && !UNITY_EDITOR
            if (consoleAllocated)
            {
                try
                {
                    // try to dealloate console
                    FreeConsole();
                    consoleAllocated = false;
                }
                catch (Exception e)
                {
                    Debug.LogError($"!FATAL! Failed to deallocate the console: {e.Message}");
                }
            }
#endif
        }

        void HandleLog(string msg, string stackTrace, LogType type)
        {
            // dont log to console if paused
            if (Pause)
                return;

            // get original colour
            var ogColor = Console.ForegroundColor;
            var showStack = false;

            // apply colour based on log type
            switch (type)
            {
                case LogType.Log:
                    Console.ForegroundColor = ccInfoTxtColour;

                    if (showStackTrace.HasFlag(ShowStackTrace.Info))
                        showStack = true;
                    break;

                case LogType.Warning:
                    Console.ForegroundColor = ccWarningTxtColour;

                    if (showStackTrace.HasFlag(ShowStackTrace.Warning))
                        showStack = true;
                    break;

                case LogType.Error:
                    Console.ForegroundColor = ccErrorTxtColour;

                    if (showStackTrace.HasFlag(ShowStackTrace.Error))
                        showStack = true;
                    break;

                case LogType.Exception:
                    Console.ForegroundColor = ccExceptionTxtColour;

                    if (showStackTrace.HasFlag(ShowStackTrace.Exception))
                        showStack = true;
                    break;

                case LogType.Assert:
                    Console.ForegroundColor = ccAssertTxtColour;

                    if (showStackTrace.HasFlag(ShowStackTrace.Assert))
                        showStack = true;
                    break;

                default:
                    Console.ForegroundColor = ogColor;
                    break;
            }

            // create log type and timestamp if needed
            var logType = includeLogType ? $"[{type}] " : "";
            var timestamp = includeTimestamp ? $"[{DateTime.Now:HH:mm:ss}] " : "";

            // write to the streamwriter and reset colour
            writer.WriteLine($"{logType}{timestamp}{msg}");

            if (showStack && !string.IsNullOrEmpty(stackTrace))
            {
                Console.ForegroundColor = ccStackTraceTxtColour;
                writer.WriteLine(stackTrace);
            }

            Console.ForegroundColor = ogColor;
        }

        bool IsBuildAllowed()
        {
#if DEVELOPMENT_BUILD
            return targetBuild.HasFlag(TargetConsoleBuild.Development);
#else
            return targetBuild.HasFlag(TargetConsoleBuild.Standard);
#endif
        }

        // from https://stackoverflow.com/questions/1988833/converting-color-to-consolecolor
        ConsoleColor GetClosestConsoleColour(Color32 colour)
        {
            // init
            var consoleColour = ConsoleColor.White;
            var smallestDist = double.MaxValue;

            foreach (var cc in consoleColours)
            {
                // calculate distance between colour and target colour
                var distance =
                    Math.Pow(cc.Value.r - colour.r, 2.0) +
                    Math.Pow(cc.Value.g - colour.g, 2.0) +
                    Math.Pow(cc.Value.b - colour.b, 2.0);

                // check if distance is smaller than the current smallest distance
                if (distance < smallestDist)
                {
                    smallestDist = distance;
                    consoleColour = cc.Key;
                }
            }

            // return closest console colour
            return consoleColour;
        }
    }

    [Flags]
    public enum TargetConsoleBuild
    {
        None = 0,
        Development = 1 << 0,
        Standard = 1 << 1,
    }
    [Flags]
    public enum ShowStackTrace
    {
        None = 0,
        Info = 1 << 0,
        Warning = 1 << 1,
        Error = 1 << 2,
        Exception = 1 << 4,
        Assert = 1 << 8,
    }
}