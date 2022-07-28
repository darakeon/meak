using RegEditA6;

if (!OperatingSystem.IsWindows())
{
	Console.WriteLine("This fixes regedit - only for windows");
	return 1;
}

var added = new RegEditPrinter(
		"Microsoft Print to PDF",
		Console.WriteLine
	)
	.AddPaperFormat("A6", 105, 148);

return added ? 0 : 1;
