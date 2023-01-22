using System;
using System.Web;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Html;
using Umbraco.Extensions;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace Vokseverk {
	public class MarkdownTextStringPropertyConverter : PropertyValueConverterBase {

		public override Type GetPropertyValueType(IPublishedPropertyType propertyType) {
			return typeof(MarkdownString);
		}

		public override PropertyCacheLevel GetPropertyCacheLevel(IPublishedPropertyType propertyType) {
			return PropertyCacheLevel.Element;
		}

		public override bool IsConverter(IPublishedPropertyType propertyType) {
			return propertyType.EditorAlias.Equals("Vokseverk.MarkdownTextString");
		}

		public override object ConvertSourceToIntermediate(IPublishedElement owner, IPublishedPropertyType propertyType, object source, bool preview) {
			var attemptConvertString = source.TryConvertTo<string>();

			if (attemptConvertString.Success) {
				return new MarkdownString(attemptConvertString.Result);
			}

			return null;
		}

		public object ConvertIntermediateToXPath(IPublishedElement owner, IPublishedPropertyType propertyType, PropertyCacheLevel referenceCacheLevel, object inter, bool preview) {
			if (inter == null) {
				return null;
			}

			return inter.ToString();
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
