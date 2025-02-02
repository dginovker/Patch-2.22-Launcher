﻿using System;
using System.Reflection;

namespace PatchLauncher
{
    internal class AssemblyNameHelper
    {
        internal static readonly string BFMELauncherGameName = AssemblyName.GetAssemblyName(Assembly.GetExecutingAssembly().Location).Name!;
        internal static readonly Version BFMELauncherGameVerion = Assembly.GetEntryAssembly()!.GetName().Version!;
        internal static int ExternalInstallerReturnCode { get; set; }
    }
}