using System.Text.Encodings.Web;

using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace YallaMasar.TagHelpers.UI
{
	public class DropdownTagHelper : TagHelper
	{
		private readonly string _data = $@"{{
            open: false,
            toggle() {{
                if (this.open) {{
                    return this.close()
                }}

                this.$refs.button.focus()

                this.open = true
            }},
            close(focusAfter) {{
                if (! this.open) return

                this.open = false

                focusAfter && focusAfter.focus()
            }}
        }}";

		public string Icon { get; set; }

		public override void Process(TagHelperContext context, TagHelperOutput output)
		{
			output.TagName = "div";    // Replaces <email> with <a> tag

			output.AddClass("relative", HtmlEncoder.Default);

			output.Attributes.SetAttribute("x-data", _data);
			output.Attributes.SetAttribute("x-id", "['dropdown-button']");
			output.Attributes.SetAttribute("x-on:keydown.escape.prevent.stop", "close($refs.button)");
			output.Attributes.SetAttribute("x-on:focusin.window", "! $refs.panel.contains($event.target) && close()");
		}
	}

	public class DropdownPanelTagHelper : TagHelper
	{
		private readonly string _classes = $@"absolute ltr:right-0 rtl:left-0 mt-2 w-40 bg-white shadow-md text-xl";


		public override void Process(TagHelperContext context, TagHelperOutput output)
		{
			output.TagName = "div";    // Replaces <email> with <a> tag

			foreach (var cls in _classes.Split(" "))
			{
				output.AddClass(cls, HtmlEncoder.Default);
			}

			output.Attributes.SetAttribute("style", "{display: none;}");
			output.Attributes.SetAttribute("x-ref", "panel");
			output.Attributes.SetAttribute("x-show", "open");
			output.Attributes.SetAttribute("x-transition.origin.top.left", "");
			output.Attributes.SetAttribute("x-on:click.outside", "close($refs.button)");
			output.Attributes.SetAttribute(":id", "$id('dropdown-button')");
		}
	}

	[HtmlTargetElement("dropdown-button", TagStructure = TagStructure.NormalOrSelfClosing)]
	public class DropdownButtonTagHelper : TagHelper
	{
		private readonly string _classes = $@"flex items-center text-white text-xl";

		public string Icon { get; set; }

		public override void Process(TagHelperContext context, TagHelperOutput output)
		{
			output.TagName = "button";

			output.Content.AppendHtml($@"<i class=""{Icon}""></i>");

			foreach (var cls in _classes.Split(" "))
			{
				output.AddClass(cls, HtmlEncoder.Default);
			}

			output.Attributes.SetAttribute("type", "button");
			output.Attributes.SetAttribute("x-ref", "button");
			output.Attributes.SetAttribute("x-on:click", "toggle()");
			output.Attributes.SetAttribute(":aria-controls", "$id('dropdown-button')");
			output.Attributes.SetAttribute(":aria-expanded", "open");
		}
	}
}
