using System;
using System.Collections.Generic;
using System.Text;

namespace GamePluginLauncher.Utils
{
    public class EventHandlerParamProxy<SenderType, EventArgsType> where SenderType : class where EventArgsType : EventArgs
    {
        public SenderType Sender { get; set; }
        public EventArgsType EventArgs { get; set; }

        public EventHandlerParamProxy() { }

        public EventHandlerParamProxy(SenderType sender, EventArgsType e)
        {
            Sender = sender;
            EventArgs = e;
        }
    }

    public class EventHandlerParamProxy : EventHandlerParamProxy<object, EventArgs>
    {
        public EventHandlerParamProxy() : base() { }

        public EventHandlerParamProxy(object sender, EventArgs e) : base(sender, e) { }
    }
}
