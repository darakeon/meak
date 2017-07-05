function SaveBegin(letter) {
    $("#save_" + letter).removeClass("greenSave");
    $("#save_" + letter).removeClass("redSave");
    $("#save_" + letter).addClass("yellowSave");
}

function SaveSuccess(letter) {
    $("#save_" + letter).removeClass("yellowSave");
    $("#save_" + letter).addClass("greenSave");
}

function SaveFailure(letter) {
    $("#save_" + letter).removeClass("yellowSave");
    $("#save_" + letter).addClass("redSave");
}