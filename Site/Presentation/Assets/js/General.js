$(document).ready(LayoutAndWork);

function LayoutAndWork() {
	PutLink();

	ChooseBackground();

	//AnimateLegend();
	ShowStoryNames();

	AjustLogin();

	SceneChanger();
	FontResizer();

	$(window).resize(AjustLogin);
}