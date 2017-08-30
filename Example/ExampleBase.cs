using WaterLogged;

namespace Example
{
    abstract class ExampleBase
    {
        public abstract string IntroText { get; }
        public abstract string Name { get; }

        protected Log _log;

        protected ExampleBase()
        {
            _log = new Log(GetType().Name);
        }

        public abstract void Echo(string text);
    }
}
