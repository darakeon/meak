var legendOpened = true;

function AnimateLegend() {

    $("#legend").toggleClass("lightback");

    $("#legend .header").toggleClass("lightback");
    $("#legend .header").toggleClass("darkback");

    $("#legend .body").toggle();
    
    legendOpened = !legendOpened;
}