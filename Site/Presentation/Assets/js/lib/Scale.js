function ScaleHeight(elementIdentifier) {

	var height = $(elementIdentifier).height();

	return height + ScaleVerticalEdge(elementIdentifier);
}

function ScaleVerticalEdge(elementIdentifier) {
	var margin = MarginV(elementIdentifier);
	var padding = PaddingV(elementIdentifier);
	var border = BorderV(elementIdentifier);

	return margin + padding + border;
}

function MarginV(elementIdentifier) {
	return NumericValueCss(elementIdentifier, 'margin-top') + NumericValueCss(elementIdentifier, 'margin-bottom');
}

function PaddingV(elementIdentifier) {
	return NumericValueCss(elementIdentifier, 'padding-top') + NumericValueCss(elementIdentifier, 'padding-bottom');
}

function BorderV(elementIdentifier) {
	return NumericValueCss(elementIdentifier, 'border-top-width') + NumericValueCss(elementIdentifier, 'border-bottom-width');
}



function ScaleWidth(elementIdentifier) {
	var width = $(elementIdentifier).width();

	return width + ScaleHorizontalEdge(elementIdentifier);
}

function ScaleHorizontalEdge(elementIdentifier) {
	var margin = MarginH(elementIdentifier);
	var padding = PaddingH(elementIdentifier);
	var border = BorderH(elementIdentifier);

	return margin + padding + border;
}

function MarginH(elementIdentifier) {
	return NumericValueCss(elementIdentifier, 'margin-left') + NumericValueCss(elementIdentifier, 'margin-right');
}

function PaddingH(elementIdentifier) {
	return NumericValueCss(elementIdentifier, 'padding-left') + NumericValueCss(elementIdentifier, 'padding-right');
}

function BorderH(elementIdentifier) {
	return NumericValueCss(elementIdentifier, 'border-left-width') + NumericValueCss(elementIdentifier, 'border-right-width');
}