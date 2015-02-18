// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Runtime.Remoting.Contexts;
using System.Security.Principal;
using System.Threading;

namespace Wrappers
{
    public class ThreadStaticWrapper
    {
        public virtual Context CurrentContext
        {
            get { return Thread.CurrentContext; }
        }

        public virtual IPrincipal CurrentPrincipal
        {
            get { return Thread.CurrentPrincipal; }
            set { Thread.CurrentPrincipal = value; }
        }

        public virtual Thread CurrentThread
        {
            get { return Thread.CurrentThread; }
        }

        public virtual LocalDataStoreSlot AllocateDataSlot()
        {
            throw new NotImplementedException();
        }

        public virtual LocalDataStoreSlot AllocateNamedDataSlot(string name)
        {
            throw new NotImplementedException();
        }

        public virtual void BeginCriticalRegion()
        {
            throw new NotImplementedException();
        }

        public virtual void BeginThreadAffinity()
        {
            throw new NotImplementedException();
        }

        public virtual void EndCriticalRegion()
        {
            throw new NotImplementedException();
        }

        public virtual void EndThreadAffinity()
        {
            throw new NotImplementedException();
        }

        public virtual void FreeNamedDataSlot(string name)
        {
            throw new NotImplementedException();
        }

        public virtual object GetData(LocalDataStoreSlot slot)
        {
            throw new NotImplementedException();
        }

        public virtual AppDomain GetDomain()
        {
            throw new NotImplementedException();
        }

        public virtual int GetDomainID()
        {
            throw new NotImplementedException();
        }

        public virtual LocalDataStoreSlot GetNamedDataSlot(string name)
        {
            throw new NotImplementedException();
        }

        public virtual void MemoryBarrier()
        {
            throw new NotImplementedException();
        }

        public virtual void ResetAbort()
        {
            throw new NotImplementedException();
        }

        public virtual void SetData(LocalDataStoreSlot slot, object data)
        {
            throw new NotImplementedException();
        }

        public virtual void Sleep(int millisecondsTimeout)
        {
            Thread.Sleep(millisecondsTimeout);
        }

        public virtual void Sleep(TimeSpan timeout)
        {
            throw new NotImplementedException();
        }

        public virtual void SpinWait(int iterations)
        {
            throw new NotImplementedException();
        }

        public virtual byte VolatileRead(ref byte address)
        {
            throw new NotImplementedException();
        }

        public virtual double VolatileRead(ref double address)
        {
            throw new NotImplementedException();
        }

        public virtual float VolatileRead(ref float address)
        {
            throw new NotImplementedException();
        }

        public virtual int VolatileRead(ref int address)
        {
            throw new NotImplementedException();
        }

        public virtual IntPtr VolatileRead(ref IntPtr address)
        {
            throw new NotImplementedException();
        }

        public virtual long VolatileRead(ref long address)
        {
            throw new NotImplementedException();
        }

        public virtual object VolatileRead(ref object address)
        {
            throw new NotImplementedException();
        }

        public virtual sbyte VolatileRead(ref sbyte address)
        {
            throw new NotImplementedException();
        }

        public virtual short VolatileRead(ref short address)
        {
            throw new NotImplementedException();
        }

        public virtual uint VolatileRead(ref uint address)
        {
            throw new NotImplementedException();
        }

        //public virtual UIntPtr VolatileRead(ref UIntPtr address)
        //{
        //    throw new NotImplementedException();
        //}

        public virtual ulong VolatileRead(ref ulong address)
        {
            throw new NotImplementedException();
        }

        public virtual ushort VolatileRead(ref ushort address)
        {
            throw new NotImplementedException();
        }

        public virtual void VolatileWrite(ref byte address, byte value)
        {
            throw new NotImplementedException();
        }

        public virtual void VolatileWrite(ref double address, double value)
        {
            throw new NotImplementedException();
        }

        public virtual void VolatileWrite(ref float address, float value)
        {
            throw new NotImplementedException();
        }

        public virtual void VolatileWrite(ref int address, int value)
        {
            throw new NotImplementedException();
        }

        public virtual void VolatileWrite(ref IntPtr address, IntPtr value)
        {
            throw new NotImplementedException();
        }

        public virtual void VolatileWrite(ref long address, long value)
        {
            throw new NotImplementedException();
        }

        public virtual void VolatileWrite(ref object address, object value)
        {
            throw new NotImplementedException();
        }

        public virtual void VolatileWrite(ref sbyte address, sbyte value)
        {
            throw new NotImplementedException();
        }

        public virtual void VolatileWrite(ref short address, short value)
        {
            throw new NotImplementedException();
        }

        public virtual void VolatileWrite(ref uint address, uint value)
        {
            throw new NotImplementedException();
        }

        //public virtual void VolatileWrite(ref UIntPtr address, UIntPtr value)
        //{
        //    throw new NotImplementedException();
        //}

        public virtual void VolatileWrite(ref ulong address, ulong value)
        {
            throw new NotImplementedException();
        }

        public virtual void VolatileWrite(ref ushort address, ushort value)
        {
            throw new NotImplementedException();
        }

        public virtual bool Yield()
        {
            throw new NotImplementedException();
        }
    }
}