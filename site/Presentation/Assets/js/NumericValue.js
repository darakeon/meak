function NumericValueCss(elementIdentifier, property) {
	if ($(elementIdentifier)[0]) {
		var css = $(elementIdentifier)
				.css(property)
				.replace('px', '');

		return parseInt(css);
	}
	else {
		return 0;
	}
}

function NumericValueAttr(elementIdentifier, property) {
	if ($(elementIdentifier)[0]) {
		var attr = $(elementIdentifier)
				.attr(property);

		return parseInt(attr);
	}
	else {
		return 0;
	}
}