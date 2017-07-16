using System;
using System.Reflection;
using System.Windows.Input;
using Xamarin.Forms;

namespace XfMvvmLight.Behaviors
{
    public class EventToCommandBehavior : BehaviorBase<VisualElement>
    {
        private Delegate _eventHandler;


        public static readonly BindableProperty EventNameProperty = 
            BindableProperty.Create("EventName", typeof(string), typeof(EventToCommandBehavior), null, propertyChanged: OnEventNameChanged);


        public string EventName
        {
            get { return (string)GetValue(EventNameProperty); }
            set { SetValue(EventNameProperty, value); }
        }


        public static readonly BindableProperty CommandProperty = 
            BindableProperty.Create("Command", typeof(ICommand), typeof(EventToCommandBehavior), null);

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly BindableProperty CommandParameterProperty = 
            BindableProperty.Create("CommandParameter", typeof(object), typeof(EventToCommandBehavior), null);

        public object CommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }


        public static readonly BindableProperty ConverterProperty = 
            BindableProperty.Create("Converter", typeof(IValueConverter), typeof(EventToCommandBehavior), null);

        
        public IValueConverter Converter
        {
            get { return (IValueConverter)GetValue(ConverterProperty); }
            set { SetValue(ConverterProperty, value); }
        }


        void RaiseEvent(object sender, object eventArgs)
        {
            if (this.Command == null)
                return;

            object passedParameter;
            if (this.CommandParameter != null)
            {
                passedParameter = this.CommandParameter;
            }
            else if (Converter != null)
            {
                passedParameter = this.Converter.Convert(eventArgs, typeof(object), null, null);
            }
            else
            {
                passedParameter = eventArgs;
            }

            if (this.Command.CanExecute(passedParameter))
                this.Command.Execute(passedParameter);
        }


        void Register(string name)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
                return;

            EventInfo eventInfo = this.AssociatedObject.GetType().GetRuntimeEvent(name);

            if (eventInfo == null)
            {
                throw new ArgumentException($"EventToCommandBehavior: Can't register the '{this.EventName}' event.");
            }
            MethodInfo methodInfo = typeof(EventToCommandBehavior).GetTypeInfo().GetDeclaredMethod("RaiseEvent");
            _eventHandler = methodInfo.CreateDelegate(eventInfo.EventHandlerType, this);
            eventInfo.AddEventHandler(this.AssociatedObject, _eventHandler);
        }

        void Deregister(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return;
            }

            if (_eventHandler == null)
            {
                return;
            }
            EventInfo eventInfo = this.AssociatedObject.GetType().GetRuntimeEvent(name);
            if (eventInfo == null)
            {
                throw new ArgumentException(string.Format("EventToCommandBehavior: Can't de-register the '{0}' event.", EventName));
            }
            eventInfo.RemoveEventHandler(this.AssociatedObject, _eventHandler);
            _eventHandler = null;
        }









        protected override void OnAttachedTo(VisualElement element)
        {
            base.OnAttachedTo(element);
            Register(this.EventName);
        }

        protected override void OnDetachingFrom(VisualElement element)
        {
            Deregister(this.EventName);
            base.OnDetachingFrom(element);
        }



        private static void OnEventNameChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var current = (EventToCommandBehavior)bindable;
            if (current.AssociatedObject == null) return;

            current.Deregister((string)oldValue);
            current.Register((string)newValue);
        }


    }
}
