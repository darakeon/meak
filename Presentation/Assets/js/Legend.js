var legendOpened = true;

function AnimateLegend() {

    $("#legend .header").toggleClass("lightback");
    $("#legend .header").toggleClass("darkback");
    $("#legend .body").toggle();
    
    legendOpened = !legendOpened;
}