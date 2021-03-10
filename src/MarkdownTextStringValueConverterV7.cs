using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Umbraco.Core;
using Umbraco.Core.PropertyEditors;
using Umbraco.Core.Models.PublishedContent;

namespace Vokseverk {
	[PropertyValueType(typeof(MarkdownString))]
	[PropertyValueCache(PropertyCacheValue.All, PropertyCacheLevel.Content)]
	public class MarkdownTextStringPropertyConverter : PropertyValueConverterBase {
		
		public override bool IsConverter(PublishedPropertyType propertyType) {
			return propertyType.PropertyEditorAlias.Equals("Vokseverk.MarkdownTextString");
		}
		
		public override object ConvertDataToSource(PublishedPropertyType propertyType, object data, bool preview) {
			var attemptConvertString = data.TryConvertTo<string>();
			
			if (attemptConvertString.Success) {
				return new MarkdownString(attemptConvertString.Result);
			}
			
			return null;
		}
		
		public class MarkdownString {
			public MarkdownString(string text) {
				TextValue = text;
			}
			
			public string TextValue { get; set; }
			public HtmlString HtmlValue {
				get {
					return Markdownify(TextValue);
				}
			}
			
			public override string ToString() {
				return TextValue;
			}
			
			public HtmlString ToHtml() {
				return HtmlValue;
			}
			public HtmlString ToHTML() {
				return ToHtml();
			}
			
			// Simple (very) Markdown parsing for headers etc.
			// Currently handles *emphasis*, _alternate emphasis_,
			// **strong emphasis**, <URL> and [link text](URL).
			private HtmlString Markdownify(string text) {
				var patternStrong = @"\*\*([^\*]+)\*\*";
				var replaceStrong = "<strong>$1</strong>";

				var patternEmph1 = @"\*([^\*]+)\*";
				var patternEmph2 = @"_([^_]+)_";
				var replaceEmph = "<em>$1</em>";
				
				var patternUrl = @"<(https?:\/\/[^ ]+?)>";
				var replaceUrl = "<a href=\"$1\" target=\"_blank\" rel=\"noopener\">$1</a>";
				
				var patternLink = @"\[([^\]]+?)\]\(([^\)]+?)\)";
				var replaceLink = "<a href=\"$2\">$1</a>";
				
				var strongRE = new Regex(patternStrong);
				var emphRE1 = new Regex(patternEmph1);
				var emphRE2 = new Regex(patternEmph2);
				var urlRE = new Regex(patternUrl);
				var linkRE = new Regex(patternLink);
				
				var parsed = strongRE.Replace(text, replaceStrong);
				parsed = emphRE1.Replace(parsed, replaceEmph);
				parsed = emphRE2.Replace(parsed, replaceEmph);
				parsed = urlRE.Replace(parsed, replaceUrl);
				parsed = linkRE.Replace(parsed, replaceLink);
				
				return new HtmlString(parsed);
			}
		}
	}
}
