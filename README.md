# WaterLogged
.net logging library

# Overall capabilities
The process of logging happens in a kind of "Logging Pipeline": \
`User writes message -> Log -> Formatter -> Each listener in the log`

The fastest/shortest way to start is simple:
```cs
var log = new WaterLogged.Log();
log.AddListener(new WaterLogged.Listeners.StandardOut());
log.WriteLine("Hello world!");
```

The Log by default uses a BasicFormatter, but that can be changed by setting the respective "Formatter" property.

Each message you send can also be sent with an accompanying "tag".
The tag is sent to the formatter and each listener to be consumed somehow.

Normally, the formatter is only called to transform the message once.
However, each listener contains a "FormatterArgs" property which is a `Dictionary<string, string>`.
If the log encounters a listener with items in this property while the log is sending a message, it will reformat for the current listener passing these args to the formatter.

When you output a message, you can optionally send a "tag". Tags are basically loglevels, but strings instead of enums with restrictive values.
Each listener has a TagFilter property which is an array of strings.
This filter whitelists tags that will be output to the listener.
So if you send a message with a tag "error" and a listener doesn't have an "error" item in its filter, the listener will never see the message.
Keep in mind that if a listener has an empty filter, all messages will be passed to it regardless of tags.
The tag is also passed to the Formatter.

# Basic formatting
The basic formatter is not as glorified as the logical formatter. Because who would use this since the other one is more featured?
Anyway, this formatter is included mostly because it was faster to write and start playing around with.
Its syntax is kind of like string interpolation: "${func:arg}". Where each function can only take a single parameter.
An example would be: "[${datetime}] [${tag}] [${builddate}] ${message}".
Although that's probably not the best example, since it doesn't contain anything that consumes an argument.

# "Logic" based formatting
## Uses NCalc to evaluate mathematical/boolean expressions from https://github.com/sklose/NCalc2
The output is best shown in a gif
![](https://raw.githubusercontent.com/icecream-burglar/WaterLogged/master/example.gif)

Here are the available formatting expressions:
  * %{text} - Just a string literal. Nothing special is pulled out of that except for text.
  * ${func:paramlist} - Invokes a function with the specified parameters. Parameters are separated with commas, naturally.
  * #{expression} - Evaluates an expression using ncalc. Boolean and mathematical expressions are resolved from these. These attempt to resolve functions used within them. So you could have like: \"#{1 + getnum}\".
  
You can use any combination of these to create your format string. You can even nest them within each-other like so: \"#{1 + ${getnumber}}\".

To see a list of functions available by default, check out the example which uses reflection to automatically spit out a list for you.

This is also pretty simple to setup:
```cs
var log = new WaterLogged.Log();
log.AddListener(new WaterLogged.Listeners.StandardOut());
log.Formatter = new WaterLogged.Supplement.LogicalFormatter("1 + 1 = #{1 + 1}. Oh, and by the way: ${message}");
log.WriteLine("Hello there!");
```
The output would be:
```
1 + 1 = 2. Oh, and by the way: Hello there!
```


Off of this, you can simply change the formatter's format string and output more advanced data:
```cs
var log = new WaterLogged.Log();
log.AddListener(new WaterLogged.Listeners.StandardOut());
log.Formatter = new WaterLogged.Supplement.LogicalFormatter("${when:${hasvalue:${tag}},[${upper:${tag}}] }${message}");
log.WriteLine("test1");
log.WriteLineTag("test2", "status");
```
The output would be:
```
test1
[STATUS] test2
```
Converted into a more easily read syntax:
`when(hasvalue(tag), "[" + upper(tag) + "] ") + message()` \
I'm sure you can infer that `when` is basically an if-statement. The first parameter is a condition and the second parameter is the output of the function if the condition is true.
Its c# decleration is
```cs
BaseContext.Functions.Add("when", new Func<bool, string, string>((b, s) =>
{
    if (b)
    {
        return s;
    }
    return "";
}));
```


# Serialization
Technically, XML deserialization is already supported. But I've not really used/tried it yet. So it may or may-not work.
Although there is a fairly abstract API to create your own deserializers.
To deserialize custom types you must implement WaterLogged.Serialization.StringConversion.IStringConverter and add it to the static "Converters" list in WaterLogged.Serialization.StringConversion.

More explanation of deserialization to come...
