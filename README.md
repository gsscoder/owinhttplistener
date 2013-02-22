Owin HTTP Listener 0.1.0.7 alfa.
===
Sample Owin HTTP listener for .NET 4.5. This library uses Owin.Types from [http://www.myget.org/F/owin/](http://www.myget.org/F/owin/)
and [HttpHelpers](https://github.com/gsscoder/httphelpers).

What is it:
---
- Plase note that this project is in **early stages of development**.
- Nothing more than sample, not fully compliant at the moment.

At glance:
---
```csharp
using AppFunc = Func<IDictionary<string, object>, Task>;

var listener = new OwinHttpListener(
    new OwinApplication().ProcessRequest, // with AppFunc signature
    8899);                                // localhost:8899
server.Start();

listener.ListenAsync().Wait();

Console.ReadKey();

listener.Stop();
```

Contacts:
---
Giacomo Stelluti Scala
  - gsscoder AT gmail DOT com
  - [Blog](http://gsscoder.blogspot.it)
  - [Twitter](http://twitter.com/gsscoder)
