function SaveBegin(formId) {
    DisableForm(formId);
    SaveBeginColor("#save_status");
}

function SaveBeginColor(id) {
    $(id).removeClass("greenSave");
    $(id).removeClass("redSave");
    $(id).addClass("yellowSave");
}



function SaveSuccess(formId, data) {
    alert(data);
    EnableForm(formId);
    SaveSuccessColor("#save_status");
}

function SaveSuccessColor(id) {
    $(id).removeClass("yellowSave");
    $(id).addClass("greenSave");
}



function SaveFailure(formId) {
    EnableForm(formId);
    SaveFailureColor("#save_status");
}

function SaveFailureColor(id) {
    $(id).removeClass("yellowSave");
    $(id).addClass("redSave");
}



function NeedSaveAgain(formId) {
    NeedSaveAgainColor("#save_status");
}

function NeedSaveAgainColor(id) {
    $(id).removeClass("greenSave");
    $(id).removeClass("yellowSave");
    $(id).removeClass("redSave");
}



function DisableForm(formId) {
    ToggleForm(formId, true);
}

function EnableForm(formId) {
    ToggleForm(formId, false);
}

function ToggleForm(formId, toggle) {
    $("#" + formId).toggleClass("disabled", toggle);
    $("#" + formId + " input").attr('disabled', toggle);
    $("#" + formId + " button").attr("disabled", toggle);
}