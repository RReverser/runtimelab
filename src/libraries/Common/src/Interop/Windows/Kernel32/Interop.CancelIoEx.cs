// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Runtime.InteropServices;
using System.Threading;

internal static partial class Interop
{
    internal static partial class Kernel32
    {
        [GeneratedDllImport(Libraries.Kernel32, SetLastError = true)]
        internal static unsafe partial bool CancelIoEx(SafeHandle handle, NativeOverlapped* lpOverlapped);

        [GeneratedDllImport(Libraries.Kernel32, SetLastError = true)]
        internal static unsafe partial bool CancelIoEx(IntPtr handle, NativeOverlapped* lpOverlapped);
    }
}
