// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using SimpleInjector;

namespace Portal.Common.Interceptions
{
    // Code from http://simpleinjector.codeplex.com/wikipage?title=InterceptionExtensions&referringTitle=Advanced-scenarios

    public interface IInterceptor
    {
        void Intercept(IInvocation invocation);
    }

    public interface IInvocation
    {
        object InvocationTarget { get; }

        object[] Arguments { get; }

        object ReturnValue { get; set; }

        void Proceed();

        MethodBase GetConcreteMethod();
    }

    // Extension methods for interceptor registration
    // NOTE: These extension methods can only intercept interfaces, not abstract types.
    public static class InterceptorExtensions
    {
        public static void InterceptWith<TInterceptor>(this Container container,
            Func<Type, bool> predicate)
            where TInterceptor : class, IInterceptor
        {
            RequiresIsNotNull(container, "container");
            RequiresIsNotNull(predicate, "predicate");

            container.Options.ConstructorResolutionBehavior.GetConstructor(typeof (TInterceptor),
                typeof (TInterceptor));

            var interceptWith = new InterceptionHelper(container)
            {
                BuildInterceptorExpression =
                    e => BuildInterceptorExpression<TInterceptor>(container),
                Predicate = type => predicate(type)
            };

            container.ExpressionBuilt += interceptWith.OnExpressionBuilt;
        }

        public static void InterceptWith(this Container container,
            Func<IInterceptor> interceptorCreator,
            Func<Type, bool> predicate)
        {
            RequiresIsNotNull(container, "container");
            RequiresIsNotNull(interceptorCreator, "interceptorCreator");
            RequiresIsNotNull(predicate, "predicate");

            var interceptWith = new InterceptionHelper(container)
            {
                BuildInterceptorExpression =
                    e => Expression.Invoke(Expression.Constant(interceptorCreator)),
                Predicate = type => predicate(type)
            };

            container.ExpressionBuilt += interceptWith.OnExpressionBuilt;
        }

        public static void InterceptWith(this Container container,
            Func<ExpressionBuiltEventArgs, IInterceptor> interceptorCreator,
            Func<Type, bool> predicate)
        {
            RequiresIsNotNull(container, "container");
            RequiresIsNotNull(interceptorCreator, "interceptorCreator");
            RequiresIsNotNull(predicate, "predicate");

            var interceptWith = new InterceptionHelper(container)
            {
                BuildInterceptorExpression = e => Expression.Invoke(
                    Expression.Constant(interceptorCreator),
                    Expression.Constant(e)),
                Predicate = type => predicate(type)
            };

            container.ExpressionBuilt += interceptWith.OnExpressionBuilt;
        }

        public static void InterceptWith(this Container container,
            IInterceptor interceptor,
            Func<Type, bool> predicate)
        {
            RequiresIsNotNull(container, "container");
            RequiresIsNotNull(interceptor, "interceptor");
            RequiresIsNotNull(predicate, "predicate");

            var interceptWith = new InterceptionHelper(container)
            {
                BuildInterceptorExpression = e => Expression.Constant(interceptor),
                Predicate = predicate
            };

            container.ExpressionBuilt += interceptWith.OnExpressionBuilt;
        }

        [DebuggerStepThrough]
        private static Expression BuildInterceptorExpression<TInterceptor>(Container container)
            where TInterceptor : class
        {
            InstanceProducer interceptorRegistration = container.GetRegistration(typeof (TInterceptor));

            if (interceptorRegistration == null)
            {
                // This will throw an ActivationException
                container.GetInstance<TInterceptor>();
            }

            return interceptorRegistration.BuildExpression();
        }

        private static void RequiresIsNotNull(object instance, string paramName)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(paramName);
            }
        }

        private class InterceptionHelper
        {
            private static readonly MethodInfo NonGenericInterceptorCreateProxyMethod = (
                from method in typeof (Interceptor).GetMethods()
                where method.Name == "CreateProxy"
                where method.GetParameters().Length == 3
                select method)
                .Single();

            public InterceptionHelper(Container container)
            {
                Container = container;
            }

            private Container Container { get; set; }

            internal Func<ExpressionBuiltEventArgs, Expression> BuildInterceptorExpression { get; set; }

            internal Func<Type, bool> Predicate { get; set; }

            [DebuggerStepThrough]
            public void OnExpressionBuilt(object sender,
                ExpressionBuiltEventArgs e)
            {
                if (Predicate(e.RegisteredServiceType))
                {
                    ThrowIfServiceTypeIsNotAnInterface(e);

                    e.Expression = BuildProxyExpression(e);
                }
            }

            [DebuggerStepThrough]
            private static void ThrowIfServiceTypeIsNotAnInterface(
                ExpressionBuiltEventArgs e)
            {
                // NOTE: We can only handle interfaces, because 
                // System.Runtime.Remoting.Proxies.RealProxy 
                // only supports interfaces.
                if (!e.RegisteredServiceType.IsInterface)
                {
                    throw new NotSupportedException(
                        "Can't intercept type " +
                        e.RegisteredServiceType.Name +
                        " because it is not an interface.");
                }
            }

            [DebuggerStepThrough]
            private Expression BuildProxyExpression(ExpressionBuiltEventArgs e)
            {
                Expression interceptor = BuildInterceptorExpression(e);

                // Create call to 
                // (ServiceType)Interceptor.CreateProxy(Type, IInterceptor, object)
                UnaryExpression proxyExpression =
                    Expression.Convert(
                        Expression.Call(NonGenericInterceptorCreateProxyMethod,
                            Expression.Constant(e.RegisteredServiceType, typeof (Type)),
                            interceptor,
                            e.Expression),
                        e.RegisteredServiceType);

                if (e.Expression is ConstantExpression && interceptor is ConstantExpression)
                {
                    return Expression.Constant(CreateInstance(proxyExpression),
                        e.RegisteredServiceType);
                }

                return proxyExpression;
            }

            [DebuggerStepThrough]
            private static object CreateInstance(Expression expression)
            {
                Func<object> instanceCreator = Expression.Lambda<Func<object>>(expression,
                    new ParameterExpression[0])
                    .Compile();

                return instanceCreator();
            }
        }
    }

    public static class Interceptor
    {
        public static T CreateProxy<T>(IInterceptor interceptor, T realInstance)
        {
            return (T)CreateProxy(typeof (T), interceptor, realInstance);
        }

        [DebuggerStepThrough]
        public static object CreateProxy(Type serviceType, IInterceptor interceptor,
            object realInstance)
        {
            var proxy = new InterceptorProxy(serviceType, realInstance, interceptor);

            return proxy.GetTransparentProxy();
        }

        private sealed class InterceptorProxy : RealProxy
        {
            private readonly IInterceptor _interceptor;
            private readonly object _realInstance;

            [DebuggerStepThrough]
            public InterceptorProxy(Type classToProxy, object realInstance,
                IInterceptor interceptor)
                : base(classToProxy)
            {
                _realInstance = realInstance;
                _interceptor = interceptor;
            }

            [DebuggerStepThrough]
            public override IMessage Invoke(IMessage msg)
            {
                if (msg is IMethodCallMessage)
                {
                    return InvokeMethodCall((IMethodCallMessage)msg);
                }

                return msg;
            }

            [DebuggerStepThrough]
            private IMessage InvokeMethodCall(IMethodCallMessage message)
            {
                var invocation = new Invocation { Proxy = this, Message = message };

                invocation.Proceeding += (s, e) =>
                {
                    invocation.ReturnValue = message.MethodBase.Invoke(
                        _realInstance, message.Args);
                };

                _interceptor.Intercept(invocation);

                return new ReturnMessage(invocation.ReturnValue, null, 0, null, message);
            }

            private class Invocation : IInvocation
            {
                public InterceptorProxy Proxy { get; set; }

                public IMethodCallMessage Message { get; set; }

                public object[] Arguments { get; private set; }

                public object ReturnValue { get; set; }

                public object InvocationTarget
                {
                    get { return Proxy._realInstance; }
                }

                [DebuggerStepThrough]
                public void Proceed()
                {
                    if (Proceeding != null)
                    {
                        Arguments = Message.Args;
                        Proceeding(this, EventArgs.Empty);
                    }
                }

                public MethodBase GetConcreteMethod()
                {
                    return Message.MethodBase;
                }

                public event EventHandler Proceeding;
            }
        }
    }
}