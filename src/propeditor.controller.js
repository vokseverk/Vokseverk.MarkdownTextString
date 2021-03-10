angular.module("umbraco").controller("MarkdownTextStringController", function($scope, $element, localizationService) {
	var strongRE = /(\*\*[^\*]+?\*\*)/;
	var emphRE = /(\*[^\*]+?\*)/;
	var hint = $element.querySelector('.vv-md-textstring-hint')
	
	localizationService.localize('mdText_hint').then(function(value) {
		if (value != '' && value.indexOf('*') >= 0) {
			value = value.replace(strongRE, '<strong>$1</strong>')
			value = value.replace(emphRE, '<em>$1</em>')
			hint.innerHTML = value
		}
	})
	
	// Set the property editor's value...
	// $scope.model.value = 'Some computed value'
})
