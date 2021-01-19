$(document).ready(LayoutAndWork);

function LayoutAndWork() {
	PutLink();

	ChooseBackground();

	//AnimateLegend();
	ShowStoryNames();

	AjustLogin();

	BlockChanger();
	FontResizer();

	$(window).resize(AjustLogin);
}