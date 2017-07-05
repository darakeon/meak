function SaveBegin() {
    SaveBeginColor("#save_status");
}

function SaveBeginColor(id) {
    $(id).removeClass("greenSave");
    $(id).removeClass("redSave");
    $(id).addClass("yellowSave");
}



function SaveSuccess() {
    SaveSuccessColor("#save_status");
}

function SaveSuccessColor(id) {
    $(id).removeClass("yellowSave");
    $(id).addClass("greenSave");
}



function SaveFailure() {
    SaveFailureColor("#save_status");
}

function SaveFailureColor(id) {
    $(id).removeClass("yellowSave");
    $(id).addClass("redSave");
}



function NeedSaveAgain() {
    NeedSaveAgainColor("#save_status");
}

function NeedSaveAgainColor(id) {
    $(id).removeClass("greenSave");
    $(id).removeClass("yellowSave");
    $(id).removeClass("redSave");
}