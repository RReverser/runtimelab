﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Runtime.InteropServices;

namespace System.Transactions.DtcProxyShim.DtcInterfaces;

// https://docs.microsoft.com/previous-versions/windows/desktop/ms681790(v=vs.85)
[ComImport, Guid(Guids.IID_IResourceManager), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
internal interface IResourceManager
{
    internal void Enlist(
        [MarshalAs(UnmanagedType.Interface)] ITransaction pTransaction,
        [MarshalAs(UnmanagedType.Interface)] ITransactionResourceAsync pRes,
        out Guid pUOW,
        out OletxTransactionIsolationLevel pisoLevel,
        [MarshalAs(UnmanagedType.Interface)] out ITransactionEnlistmentAsync ppEnlist);

    internal void Reenlist(
        [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] byte[] pPrepInfo,
        uint cbPrepInfom,
        uint lTimeout,
        [MarshalAs(UnmanagedType.I4)] out OletxXactStat pXactStat);

    void ReenlistmentComplete();

    void GetDistributedTransactionManager(
        in Guid riid,
        [MarshalAs(UnmanagedType.Interface)] out object ppvObject);
}
