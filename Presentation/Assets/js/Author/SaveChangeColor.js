function SaveBegin() {
    $("#save").removeClass("greenSave");
    $("#save").removeClass("redSave");
    $("#save").addClass("yellowSave");
}

function SaveSuccess() {
    $("#save").removeClass("yellowSave");
    $("#save").addClass("greenSave");
}

function SaveFailure() {
    $("#save").removeClass("yellowSave");
    $("#save").addClass("redSave");
}