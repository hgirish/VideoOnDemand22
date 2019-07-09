using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VOD.Admin.TagHelpers
{
    [HtmlTargetElement("alert")]
    public class AlertTagHelper : TagHelper
    {
        // Possible Bootstrap classes: primary, secondary, success,
        // danger, warning, info, light, dark.
        public string AlertType { get; set; } = "success";

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (output == null)
                throw new ArgumentNullException(nameof(output));

            var content = output.GetChildContentAsync().Result.GetContent();
            if (content.Trim().Equals(string.Empty))
            {
                return;
            }
            var close = $"<button type='button' class='close' " +
                $"data-dismiss='alert' aria-label='Close'>" +
                $"<span aria-hidden='true'>&times;</span></button>";

            var html = $"{content}{close}";
            output.TagName = "div";
            output.Attributes.Add("class",
                $"alert alert-{AlertType} alert-dismissible fade show");
            output.Attributes.Add("role", "alert");
            output.Attributes.Add("style",
                "border-radius: 0px; margin-bottom: 0;");
            output.Content.SetHtmlContent(html);
            output.Content.AppendHtml("</div>");

            base.Process(context, output);
        }
    }
}
