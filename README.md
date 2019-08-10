## WebSocketWrapper

library for Wrapping WebSocket routing in .Net Core

### Using :
you just need to add to Startup.cs _in case of **.netCore MVC**_ or call from **IApplicationBuilder** 
in both cases first you need to add to the top of the cs file <br>
`using Weerly.WebSocket.Wrapper.Builder`<br>
and then add next:
```C#
IApplicationBuilder.UseWebSocketRoutes();
```

## Parameters of the _UseWebSocketRoutes_ method :

```markdown
1. it's lambda expression which represents an instance of `IWebSocketRouteBuilder`
   and includes in itself next fields :
   - `string` name
   - `string` template
   - `Weerly.WebSocketWrapper.Enums.WebSocketEnums.CommonType` type (optional) **Default value is  CommonType.Controller** 
   - `string` classNamespace (optional) **uses for single route when type equals CommonType.Class**
2. `Weerly.WebSocketWrapper.Enums.WebSocketEnums.CommonType` commonType (optional) **Default value is CommonType.Controller**
3. `string` commonClassName(optional) **uses when you  want to change default Controller to Call**
4. `string` classNamespace (optional) **uses for all routes when commonType equals CommonType.Class**
```

## Rules of routing
```markdown
1. names should not:
   - equals to null, empty or consists only of white-space characters 
   - duplicate each other
2. templates have follow patterns:
   - "Controller(or Class)/method" **it should be specified if class is using by choosing CommonType.Class**
   - "/method" **by Default WebSocketController will be called**
   - "{controller=ControllerName}/{action=methodName}"
   - "{class=ClassName}/{action=methodName}" **in this case you need add class namespace without class name itself**
   - "/{action=methodName}" **by Default WebSocketController will be called**
```
## Examples
```C#
1) app.UseWebSocketRoutes(
		routes =>
		{
			routes.MapWSRoute(
				name: "default", 
				template: "Test/About",
				type: CommonType.Class,
				classNamespace: "Models.TestClass"
				);
			routes.MapWSRoute(
				name: "default3", 
				template: "Test/About",
				);
			routes.MapWSRouteAsync(
				name: "default1",
				template: "{controller=Dynma}/{action=Json}");
			routes.MapWSRouteAsync(
				name: "default2",
				template: "/About");
			routes.MapWSRouteAsync(
				name: "default3",
				template: "/Ajason");
		});
```

## Describe of Examples
1
You can use the [editor on GitHub](https://github.com/Weerly/WebSocketWrapper/edit/master/README.md) to maintain and preview the content for your website in Markdown files.

Whenever you commit to this repository, GitHub Pages will run [Jekyll](https://jekyllrb.com/) to rebuild the pages in your site, from the content in your Markdown files.

### Markdown

Markdown is a lightweight and easy-to-use syntax for styling your writing. It includes conventions for

```markdown
Syntax highlighted code block

# Header 1
## Header 2
### Header 3

- Bulleted
- List

1. Numbered
2. List

**Bold** and _Italic_ and `Code` text

[Link](url) and ![Image](src)
```

For more details see [GitHub Flavored Markdown](https://guides.github.com/features/mastering-markdown/).

### Jekyll Themes

Your Pages site will use the layout and styles from the Jekyll theme you have selected in your [repository settings](https://github.com/Weerly/WebSocketWrapper/settings). The name of this theme is saved in the Jekyll `_config.yml` configuration file.

### Support or Contact

Having trouble with Pages? Check out our [documentation](https://help.github.com/categories/github-pages-basics/) or [contact support](https://github.com/contact) and we’ll help you sort it out.

