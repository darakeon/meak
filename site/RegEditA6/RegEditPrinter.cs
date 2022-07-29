#pragma warning disable CA1416 // this is for windows only

using Microsoft.Win32;

namespace RegEditA6;

public class RegEditPrinter
{
	private readonly String printerName;
	private readonly Action<String> log;
	private String? driversPath;
	private String? gpdPath;
	private String? pdcPath;

	private const Int32 optionFactor = 18000;

	public RegEditPrinter(String printerName, Action<String> log)
	{
		this.printerName = printerName;
		this.log = log;

		setPathsAndRegistries();
	}

	private void setPathsAndRegistries()
	{
		var printerRegistry = get(
			Registry.LocalMachine,
			"SOFTWARE",
			"Microsoft",
			"Windows NT",
			"CurrentVersion",
			"Print",
			"Printers",
			printerName
		);

		if (printerRegistry == null)
		{
			log($"Registry for printer \"{printerName}\" not found");
			return;
		}

		var driverGuid = printerRegistry
			.GetValue("PrintQueueV4DriverDirectory")
			?.ToString();

		if (driverGuid == null)
		{
			log($"Driver guid for printer \"{printerName}\" not found");
			return;
		}

		var driversPath = Path.Combine(
			"C:", "Windows", "System32", "spool", "V4Dirs", driverGuid
		);

		if (!Directory.Exists(driversPath))
		{
			log($"Driver folder for printer \"{printerName}\" not found");
			return;
		}

		var gpdRegistry = printerRegistry
			.OpenSubKey("PrinterDriverData")
			?.GetValue("V4_Merged_ConfigFile_Name")
			?.ToString();

		if (gpdRegistry == null)
		{
			log($"GPD registry for printer \"{printerName}\" not found");
			return;
		}

		var gpdPath = Path.Combine(driversPath, gpdRegistry);

		if (!File.Exists(gpdPath))
		{
			log($"GPD file for printer \"{printerName}\" not found");
			return;
		}

		var pdcPath = Path.Combine(driversPath, "pdc.xml");

		if (!File.Exists(pdcPath))
		{
			log($"PDC file for printer \"{printerName}\" not found");
			return;
		}

		this.driversPath = driversPath;
		this.gpdPath = gpdPath;
		this.pdcPath = pdcPath;
	}

	private RegistryKey? get(RegistryKey parent, params String[] keys)
	{
		if (keys.Length == 0)
			return parent;

		var subKey = parent.OpenSubKey(keys[0]);
		var otherKeys = keys.Skip(1).ToArray();

		if (subKey == null)
		{
			log($"\"{keys[0]}\" not found in \"{parent}\"");
			return null;
		}

		return get(subKey, otherKeys);
	}

	public Boolean AddPaperFormat(
		String paperFormatName,
		Int32 width, Int32 height
	)
	{
		if (
			driversPath == null
			|| gpdPath == null
			|| pdcPath == null
		) return false;

		var datetime = $"{DateTime.Now:yyyyMMddHHmmssffffff}";

		addToGPD(paperFormatName, width, height, datetime);
		addToPDC(paperFormatName, width, height, datetime);

		log($"\"{paperFormatName}\" format added to \"{printerName}\"");
		return true;
	}

	private void addToGPD(String paperFormatName, Int32 width, Int32 height, String datetime)
	{
		if (gpdPath == null) return;

		var oldLines = File.ReadAllLines(gpdPath);
		var newLines = new List<String>();

		foreach (var line in oldLines)
		{
			newLines.Add(line);
			if (line == "*DefaultOption: LETTER")
			{
				var optionWidth = width * optionFactor;
				var optionHeight = height * optionFactor;
				var newPaperOption = new[]
				{
					$"*Option: {paperFormatName}",
					"{",
					"*rcNameID: =RCID_DMPAPER_SYSTEM_NAME",
					$"*PrintSchemaKeywordMap: \"ISO{paperFormatName}\"",
					"*PrintableOrigin: PAIR(0, 0)",
					$"*PrintableArea: PAIR({optionWidth}, {optionHeight})",
					"}",
				};

				newPaperOption.ToList().ForEach(newLines.Add);
			}
			else if (line.Contains($"ISO{paperFormatName}"))
			{
				log($"\"{paperFormatName}\" already in GPD");
				return;
			}
		}

		if (newLines.Count > oldLines.Length)
		{
			var backup = $"{gpdPath}.backup{datetime}";
			File.Copy(gpdPath, backup);
			File.WriteAllLines(gpdPath, newLines);
		}
	}

	private void addToPDC(String paperFormatName, Int32 width, Int32 height, String datetime)
	{
		if (pdcPath == null) return;

		var oldLines = File.ReadAllLines(pdcPath);
		var newLines = new List<String>();

		foreach (var line in oldLines)
		{
			newLines.Add(line);
			if (line == "  <psk:PageMediaSize psf2:psftype=\"Feature\">")
			{
				var newPaperOption = new[]
				{
					$"    <psk:ISO{paperFormatName} psf2:psftype=\"Option\">",
					$"    <psk12:PortraitImageableSize psf2:psftype=\"Property\" xsi:type=\"psf2:ImageableAreaType\">0,0,{width}000,{height}000</psk12:PortraitImageableSize>",
					$"    <psk:MediaSizeHeight psf2:psftype=\"ScoredProperty\" xsi:type=\"xsd:integer\">{height}000</psk:MediaSizeHeight>",
					$"    <psk:MediaSizeWidth psf2:psftype=\"ScoredProperty\" xsi:type=\"xsd:integer\">{width}000</psk:MediaSizeWidth>",
					$"    </psk:ISO{paperFormatName}>",
				};

				newPaperOption.ToList().ForEach(newLines.Add);
			}
			else if (line.Contains($"ISO{paperFormatName}"))
			{
				log($"\"{paperFormatName}\" already in PDC");
				return;
			}
		}

		if (newLines.Count > oldLines.Length)
		{
			var backup = $"{pdcPath}.backup{datetime}";
			File.Copy(pdcPath, backup);
			File.WriteAllLines(pdcPath, newLines);
		}
	}
}
