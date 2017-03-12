using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Common
{
    ///
    /// 控制函数执行时间,超时返回null不继续执行 
    /// 调用方法 
    /// FuncTimeout.EventNeedRun action = delegate(object[] param) 
    /// { 
    ///     //调用自定义函数 
    ///     return Test(param[0].ToString(), param[1].ToString(), (DateTime)param[2]); 
    /// };
    /// FuncTimeout ft = new FuncTimeout(action, 2000); 
    /// var result = ft.doAction("1", "2", DateTime.Now); 
    ///
    public class FuncTimeout
    {   ///
        /// 信号量 
        ///
        public ManualResetEvent manu = new ManualResetEvent(false);
        ///
        /// 是否接受到信号
        /// 
        public bool isGetSignal; 
        ///
        /// 设置超时时间
        /// 
        public int timeout; 
        ///
        /// 定义一个委托 ，输入参数可选，输出object
        /// 
        public delegate object EventNeedRun(params object[] param); 
        ///
        /// 要调用的方法的一个委托 
        ///
        private EventNeedRun FunctionNeedRun; 

        
        /// 构造函数，传入超时的时间以及运行的方法 
        /// 运行的方法 
        /// 超时的时间 
        public FuncTimeout(EventNeedRun _action, int _timeout) { 
            FunctionNeedRun = _action;
            timeout = _timeout;
        } 
        
        ///
        /// 回调函数 
        ///
        public void MyAsyncCallback(IAsyncResult ar)
        { 
            //isGetSignal为false,表示异步方法其实已经超出设置的时间，此时不再需要执行回调方法。 
            if (isGetSignal == false)
            { //放弃执行回调函数; 
                Thread.CurrentThread.Abort();
            }
        } 
        ///
        /// 调用函数
        /// 可选个数的输入参数
        /// 
        public object doAction(params object[] input)
        {
            EventNeedRun WhatTodo = CombineActionAndManuset;
            //通过BeginInvoke方法，在线程池上异步的执行方法。
            var r = WhatTodo.BeginInvoke(input, MyAsyncCallback, null);
            //设置阻塞,如果上述的BeginInvoke方法在timeout之前运行完毕，则manu会收到信号。此时isGetSignal为true。 
            //如果timeout时间内，还未收到信号，即异步方法还未运行完毕，则isGetSignal为false。
            isGetSignal = manu.WaitOne(timeout);
            if (isGetSignal == true)
            {
                return WhatTodo.EndInvoke(r);
            }
            else { return null; }
        }

        /// 把要传进来的方法，和 manu.Set()的方法合并到一个方法体。
        /// action方法运行完毕后，设置信号量，以取消阻塞。 
        /// 输入参数 
        public object CombineActionAndManuset(params object[] input)
        {
            var output = FunctionNeedRun(input);
            manu.Set();
            return output;
        }
    }
}
