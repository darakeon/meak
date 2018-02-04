function GetSuggestionBox(obj) {
	var rel = $(obj).attr("rel");
	var suggestionBoxId = $(obj).attr("suggestion");

	var suggestion = $("#" + suggestionBoxId).html();

	$("#" + rel).html(suggestion);
	$("#" + rel).show();

	$("#" + rel + " .suggestionItem").click(function () {
		ChooseOption(this, obj);

		var parent = $(this).closest(".paragraphPiece");
		var paragraphBox = parent.find("span").first();

		var oldClass = paragraphBox.attr("class");
		var typeIndex = oldClass.indexOf("_") + 1;
		var newClass = oldClass.substr(0, typeIndex) + $(this).html();

		paragraphBox
			.removeClass(oldClass)
			.addClass(newClass);
	});
}

function ChooseOption(obj, auxi) {
	var text = $(obj).html();
	auxi.value = text;
}

function ExitSuggestionBox(obj) {
	var rel = $(obj).attr("rel");

	//The Blur event occurs before click in another object
	//Then, the cleaning of the suggestion box need to be delayed
	setTimeout('$("#' + rel + '").html("")', 200);
}