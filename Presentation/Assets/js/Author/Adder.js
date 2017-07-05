function AddInputItem(obj) {
    var type = $(obj).attr("type");

    switch (type) {
        case "Piece":
            AddPiece(obj);
            break;
        case "Paragraph":
            AddParagraph(obj);
            break;
        default:
            alert("Unknown Adder.")
    }
}



function AddPiece(obj) {
    var subtype = $(obj).attr("subtype");
    var piece = NumericValueAttr(obj, "piece");
    var typedParagraph = NumericValueAttr(obj, subtype);

    $.post(adderPage, { type: "piece", subtype: subtype, piece: piece + 1, teller: typedParagraph, talk: typedParagraph }, function (data) {

        var paragraph = NumericValueAttr(obj, "paragraph");
        AjustNextPieces(paragraph, piece);


        var currentPiece = $("#" + subtype + typedParagraph + "_Piece" + piece);

        currentPiece.after(data);


        var newInputID = "#" + subtype + typedParagraph + "_Piece" + (piece + 1);
        AjustThisEditTextSize(newInputID);

    });
}

function AjustNextPieces(paragraph, piece) {
    $("#Paragraph" + paragraph + " .paragraphPiece").each(function () {
        AjustPiece(this, piece);
    });
}

function AjustPiece(obj, piece) {
    var currentID = $(obj).attr("id");

    var currentPieceNumber = currentID.split("_")[1].replace("Piece", "");
    currentPieceNumber = parseInt(currentPieceNumber);


    if (currentPieceNumber > piece) {
        GetInputValues(obj);
        AjustNumbers(obj, "piece", currentPieceNumber);
        SetInputValues(obj);
    }
}



function AddParagraph(obj) {
    var subtype = $(obj).attr("subtype").toLowerCase();
    var caller = $(obj).attr("caller").toLowerCase();

    var paragraph = NumericValueAttr(obj, "paragraph");
    var newParagraph = paragraph + 1;


    var talkParagraph = NumericValueAttr(obj, 'talk');
    var newTalkParagraph = talkParagraph +
        (caller == 'talk' ? 1 : 0);

    var tellerParagraph = NumericValueAttr(obj, 'teller');
    var newTellerParagraph = tellerParagraph +
        (caller == 'teller' ? 1 : 0);

    var newTypedParagraph = subtype == 'talk'
                                ? newTalkParagraph
                                : newTellerParagraph;


    $.post(adderPage, { type: "paragraph", subtype: subtype, paragraph: newParagraph, teller: newTellerParagraph, talk: newTalkParagraph }, function (data) {

        AjustNextParagraphs(paragraph, subtype);

        $("#Paragraph" + paragraph).after(data);

        AjustThisEditTextSize("#Paragraph" + newParagraph);

    });
}

function AjustNextParagraphs(paragraph, subtype) {
    $(".paragraph").each(function () {
        AjustParagraph(this, paragraph, subtype);
    });
}

function AjustParagraph(obj, paragraph, subtype) {
    var currentID = $(obj).attr("id");
    var currentParagraphNumber = currentID.replace("Paragraph", "");
    currentParagraphNumber = parseInt(currentParagraphNumber);

    if (currentParagraphNumber > paragraph) {
        GetInputValues(obj);

        
        AjustNumbers(obj, "paragraph", currentParagraphNumber);

        var currentTypedParagraphNumber = NumericValueAttr(obj, subtype);

        
        AjustNumbers(obj, subtype, currentTypedParagraphNumber);

        if (subtype == "talk") {
            AjustNumbers(obj, "character", currentTypedParagraphNumber);
        }

        AjustDivSubTypePosition(obj, subtype);


        SetInputValues(obj);
    }
}

function AjustDivSubTypePosition(obj, subtype) {
    var currentPosition = $(obj).attr(subtype);
    
    var rightPosition = parseInt(currentPosition) + 1;

    $(obj).attr(subtype, rightPosition);
}





function AjustNumbers(obj, preced, currentNumber) {
    var rightPieceNumber = currentNumber + 1;

    var current = new RegExp("(" + preced + "[^0-9 ]*)(" + currentNumber + ")", "gi");
    var right = "$1" + rightPieceNumber;

    $(obj).html(
            $(obj).html().replace(current, right)
        );

    $(obj).attr("id",
            $(obj).attr("id").replace(current, right)
        );
}

function AjustThisEditTextSize(newInputID) {
    var newInputTextID = newInputID + " .text";
    AjustEditTextSize(newInputTextID);
}




function GetInputValues(obj) {
    $("#" + obj.id + " input").each(function () {
        var value = $(this).attr("value");
        $(this).attr("editedValue", value);
    });
}

function SetInputValues(obj) {
    $("#" + obj.id + " input").each(function () {
        var editedValue = $(this).attr("editedValue");
        $(this).attr("value", editedValue);
    });
}
