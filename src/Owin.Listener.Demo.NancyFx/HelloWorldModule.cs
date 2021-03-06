﻿namespace Owin.Listener.Demo.NancyFx
{
    using Nancy;

    public class HelloWorldModule : NancyModule
    {
        public HelloWorldModule()
        {
            Get["/"] = _ => "<a href=\"/hello\">go here</a>";

            Get["/hello"] = _ => "<p>Hello, Nancy OWIN!<br/>" +
                "<a href=\"/\">back home</a></p>";
        }
    }
}