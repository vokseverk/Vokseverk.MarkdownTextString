angular.module("umbraco").controller("MarkdownTextStringController", function($scope, $element, localizationService) {
	var strongRE = /(\*\*[^\*]+?\*\*)/;
	var emphRE = /(\*[^\*]+?\*)/;
	var hint = $element.find('.vv-md-textstring-hint')
	
	if (hint != null) {
		localizationService.localize('mdText_hint').then(function(value) {
			if (value != '' && value.indexOf('*') >= 0) {
				value = value.replace('(*)', '(-o-)')
				value = value.replace(strongRE, '<strong>$1</strong>')
				value = value.replace(emphRE, '<em>$1</em>')
				value = value.replace('(-o-)', '(*)')
				hint.html(value)
			}
		})
	}
})
