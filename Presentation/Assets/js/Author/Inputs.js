function AjustEditTextSize(reference) {
    if ($(".auxi")[0]) {

        var main = $(".main").width();
        var auxis = 2 * ScaleWidth(".auxi");
        var adders = 3 * ScaleWidth("adder:visible");
        var scroll = 25;

        var text = main - auxis - adders - scroll;

        if (!reference) reference = ".paragraph .text";

        $(reference).width(text);
    }
}



function SaveButton() {
    $("input").keydown(function () {
        NeedSaveAgain();
    });

    /*
    $("#save_status").click(function () {
        $("form:visible").submit();
    });
    */
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
                if (pressedCtrl && e.which == 89)
                    $("form:visible").submit();
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