﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AudioSwitcher.AudioApi.Observables
{
    public sealed class AsyncBroadcaster<T> : BroadcasterBase<T>
    {
        private readonly HashSet<IObserver<T>> _observers;
        private readonly object _observerLock = new object();
        private bool _isDisposed;

        public AsyncBroadcaster()
        {
            _observers = new HashSet<IObserver<T>>();
        }

        public override bool HasObservers
        {
            get
            {
                return _observers.Count > 0;
            }
        }

        public override bool IsDisposed
        {
            get
            {
                return _isDisposed;
            }
        }

        public override void OnNext(T value)
        {
            if (IsDisposed)
                return;

            RaiseAllObserversAsync(x =>
            {
                try
                {
                    x.OnNext(value);
                }
                catch (Exception ex)
                {
                    x.OnError(ex);
                }
            });
        }

        public override void OnError(Exception error)
        {
            if (IsDisposed)
                return;

            RaiseAllObserversAsync(x => x.OnError(error));
        }

        public override void OnCompleted()
        {
            if (IsDisposed)
                return;

            RaiseAllObserversAsync(x => x.OnCompleted());
        }

        private void RaiseAllObserversAsync(Action<IObserver<T>> observerAction)
        {
            List<IObserver<T>> lObservers;
            lock (_observerLock)
            {
                lObservers = _observers.ToList();
            }

            Task.Factory.StartNew(() =>
            {
                Parallel.ForEach(lObservers, observerAction);
            });
        }

        public override IDisposable Subscribe(IObserver<T> observer)
        {
            lock (_observerLock)
            {
                _observers.Add(observer);
            }

            return new DelegateDisposable(() =>
            {
                lock (_observerLock)
                {
                    _observers.Remove(observer);
                }
            });
        }

        public override void Dispose()
        {
            OnCompleted();
            _isDisposed = true;

            lock (_observerLock)
            {
                _observers.Clear();
            }
        }
    }
}
