using System.Collections.Generic;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Umbraco.Core;
using Umbraco.Core.PropertyEditors;
using Umbraco.Core.Models.PublishedContent;

namespace Vokseverk {
	// TODO: Change Type her
	[PropertyValueType(typeof(string))]
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
				MarkdownValue = Markdownify(text);
			}
			
			public string TextValue { get; set; }
			public HtmlString MarkdownValue {
				get {
					return Markdownify(TextValue);
				}
			}
			
			public override HtmlString ToString() {
				return MarkdownValue;
			}
			
			/// Simple (very) Markdown parsing for headers etc.
			/// Currently handles *emphasis* and **strong emphasis**
			private HtmlString Markdownify(string text) {
				var patternStrong = @"\*\*([^\*]+)\*\*";
				var replaceStrong = "<strong>$1</strong>";

				var patternEmph = @"\*([^\*]+)\*";
				var replaceEmph = "<em>$1</em>";
		
				var strongRE = new Regex(patternStrong);
				var emphRE = new Regex(patternEmph);
		
				var parsed = strongRE.Replace(text, replaceStrong);
				parsed = emphRE.Replace(parsed, replaceEmph);
		
				return new HtmlString(parsed);
			}
		}
	}
}
