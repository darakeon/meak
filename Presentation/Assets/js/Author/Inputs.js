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
                    SaveCurrentScene()
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

function SaveCurrentScene() {
    $(".scenes form:visible").submit();
}