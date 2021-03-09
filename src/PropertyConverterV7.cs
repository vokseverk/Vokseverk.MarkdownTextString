using System.Collections.Generic;
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
		// TODO: Change class name above
		
		public override bool IsConverter(PublishedPropertyType propertyType) {
			// TODO: Change alias
			return propertyType.PropertyEditorAlias.Equals("Vokseverk.MarkdownTextString");
		}
		
		public override object ConvertDataToSource(PublishedPropertyType propertyType, object data, bool preview) {
			// TODO: Implement
		}
	}
}
