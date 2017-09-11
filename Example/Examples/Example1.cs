
namespace Example.Examples
{
    class Example1 : ExampleBase
    {
        public override string IntroText => "This example demonstrates simple log usage. It simply echoes back.";
        public override string Name => "Simple log usage.";

        public Example1()
        {
            _log.AddListener(new WaterLogged.Listeners.StandardOut());
        }

        public override void Echo(string text)
        {
            _log.WriteLine(text);
        }

        public override void Selected()
        {
        }
    }
}
