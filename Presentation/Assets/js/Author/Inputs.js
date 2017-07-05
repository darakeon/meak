function AjustEditTextSize(reference) {
    if ($(".auxi")[0]) {

        var main = $(".main").width();
        var auxis = Math.ceil(2 * ScaleWidth(".auxi:visible"));
        var adders = 3 * ScaleWidth("adder:visible");
        var scroll = 40;

        var text = main - auxis - adders - scroll;

        if (!reference) reference = ".paragraph .text";

        $(reference).width(text);
    }
}



function SaveButton() {
    $("input").keydown(function () {
        NeedSaveAgain($(this).closest("form").attr("id"));
    });
}



var pressedCtrl = false;
var pressedShift = false;
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
                    $(".scenes form:visible").submit();
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