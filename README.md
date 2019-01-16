# WaterLogged
.net logging library

## This library supports Structured/Templated log messages.
### See <https://messagetemplates.org/> for details and terminology

# Quickstart
The process of logging happens in a kind of "Logging Pipeline": \
`User writes message -> Log -> Formatter -> [Each output item in the log]`

The fastest/shortest way to start is simple:
```cs
var log = new WaterLogged.Log();
log.AddListener(new WaterLogged.Output.StandardOut());
log.WriteLine("Hello world!");
```

When you call the writeline, the log's assigned formatter transforms the message appropriately. Additionally, each listener contains a "FormatterArgs" property which is a `Dictionary<string, string>` (argument name/argument value pairs).
If the log encounters a listener with items in this property while sending a message through the listener, it will reformat for the current listener, passing these arguments to the formatter.

## Tags and Filters
### Tags (Think: LogLevels, but strings)
When you output a message, you can optionally specify a *tag* that applies to the message. Everything down the logging pipeline can use this tag to manipulate what that pipe does with the message. Here's an example of outputting with a tag:
```cs
log.WriteLineTag("Something bad happened!", "error");
```
### Filters
Both Listeners and Message Sinks (collectively known as *output items*) inherit `IOutput` somewhere down the line.
Within this interface is contained the common properties of both Listeners and Message Sinks.
Most notably, *Filters*.
A Filter, as its name suggests, allows calling-code to have fine-grained control over what messages an output item actually processes.
A Filter implements either *IFilter* or *ITemplatedMessageFilter*, depending on which type of output the filter applies to. (Ideally, an implementor will implement **both** interfaces).
Each output item contains a *FilterManager*, which handles validation of messages.
The currently implemented filters work on tags. Either by blacklisting or whitelisting tags.
```cs
var log = new WaterLogged.Log();
var listener = new WaterLogged.Output.StandardOut();

listener.FilterManager.Filters.Add(new WaterLogged.Filters.TagWhitelistFilter("debug", "warning"));
log.AddListener(listener);

//The following two will be printed.
log.WriteLineTag("Some debug info", "debug");
log.WriteLineTag("Something bad happened, but it isn't fatal", "warning");

//But this one won't, because it isn't in the whitelist.
log.WriteLineTag("Something fatal happened", "error");
```

## Listeners
A listener is one type of output item.
It outputs standard, string-based messages.
Right now there are only a few listeners implemented, with plans for more later on.

The current implementations are as follow:
  * StandardOutListener - Outputs to standard-out. It can optionally output in different colors based on tags.
  * FileOut - Appends output to a file.
  * TCPClientOut - Writes messages to a TCP client socket.
  * EmailOut - Outputs through SMTP - Thanks to [Ben Matthews](https://github.com/BenTMatthews)


## MessageSinks
A message sink is the other kind of output item.
It handles outputting StructuredMessages.
See the link at the top for details.
But basically it allows you to capture state information in a log message.

## Writing structured log messages
### Again, see <https://messagetemplates.org/> for details and terminology

*Note that WaterLogged's implementation differs from the "specs" found on messagetemplates.org in that WaterLogged supports both named holes and positional holes in the template string.*

By the way, whenever you print a Templated Message, you must supply a tag.
The tag can be empty or null if you like though.
Now let's see an example:
```cs
var log = new Log();

//Note how we can use names to match-up holes in WaterLogged.
log.WriteStructuredNamed("{name} is {age} years old. {name} says 'Hello'", "tag", ("name", "Bill"), ("age", 22));

//However the following line more closely matches what's found on
//messagetemplates.org in that each hole in the template has a
//1:1 matching with a value.
log.WriteStructured("{name} is {age} years old. {name} says 'Hello'", "tag", "Bill", 22, "Bill");
```

Logs support structured messages through four functions
  *  WriteStructured
  *  WriteStructuredNamed
  *  WriteStructuredParent 
  *  WriteStructuredStaticParent

`WriteStructured` takes in a template string, a tag and an array of values for the holes in the template. If a value is not used by a hole, it is still evaluated as part of the message. 

`WriteStructuredNamed` takes in a template string, a tag and an array of tuple values (string, object). The first item in each tuple correlates to a hole's name. The second value will be identified as the hole's actual value. If a tuple is unused, it will still be evaluated as part of the message.

`WriteStructuredParent` takes in a template string, a tag and a object of any type. The object's properties and fields will be evaulated as the hole values. The names of the properties/fields will correspond to the each hole's name. You can filter which properties and fields by using the `ParentObjectAttribute` and `ParentObjectValueAttribute` attributes. Unused properties/fields will indeed still be a part of the message.

`WriteStructuredStaticParent` - static variant of WriteStructuredParent. Takes in a template string, a tag and the type of the static class.


### Notes
There indeed exists a *TemplateRedirectSink* which will process the output StructuredMessage and re-output it to a *log.WriteTag* call.

The `WaterLogged.Templating.TemplateProcessor` type contains all necessary functions for processing templated messages. Including a *ProcessMessage* function which will convert a StructuredMessage into a string.


# Formatting
## Uses NCalc to evaluate mathematical/boolean expressions from https://github.com/sklose/NCalc2
The output is best shown in a gif
![](https://raw.githubusercontent.com/icecream-burglar/WaterLogged/master/example.gif)

Here are the available formatting expressions:
  * %{[text]} - Just a string literal. Nothing special is pulled out of that except for text.
  * ${[func]:[paramlist]} - Invokes a function with the specified parameters. Parameters are separated with commas, naturally.
  * \#{[expression]} - Evaluates an expression using ncalc. Boolean and mathematical expressions are resolved from these. These attempt to resolve functions used within them. So you could have like: \"#{1 + getnum}\".
  
You can use any combination of these to create your format string. You can even nest them within each-other like so: \"#{1 + ${getnumber}}\".

This is also pretty simple to setup:
```cs
var log = new WaterLogged.Log();
log.AddListener(new WaterLogged.Listeners.StandardOut());
log.Formatter = new WaterLogged.Formatting.LogicalFormatter("1 + 1 = #{1 + 1}. Oh, and by the way: ${message}");
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
Its c# declaration is
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
Note to self: Rewrite it, then document it here.
