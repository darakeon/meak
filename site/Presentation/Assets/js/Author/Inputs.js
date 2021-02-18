function SaveButton() {
	$("input").keydown(function () {
		NeedSaveAgain($(this).closest("form").attr("id"));
	});

	$(".titleChange input, .titleChange textarea").focus(function () {
		saveTitle = true;
	});

	$(".titleChange input, .titleChange textarea").blur(function () {
		saveTitle = false;
	});
}



var pressedCtrl = false;
var pressedShift = false;
var saveTitle = false;
var canSubmit = true;

function SaveKey() {
	if ($(".auxi")[0]) {

		$(document).keyup(function (e) {
			Pressed(e.which, false);
		});

		$(document).keydown(function (e) {
			Pressed(e.which, true);

			if (canSubmit) {
				if (pressedCtrl && e.which == 89) {
					SaveCurrentForm();
					pressedCtrl = false;
				}
			}
			else {
				alert('WAIT!');
			}

		});

	}
}

function Pressed(key, set) {
	switch (key) {
		case (17): pressedCtrl = set; break;
		case (16): pressedShift = set; break;
	}
}

function SaveCurrentForm() {
	if (saveTitle) {
		$(".titleChange").parent("form").submit();
	} else {
		SaveCurrentBlock();
	}
}

function SaveCurrentBlock() {
	$(".blocks form:visible").submit();
}