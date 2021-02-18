# ePub

## Files

You need to put all the following files at same folder.

### Your content

XHTML file(s) for the book, can be something like one for chapter or
just one file.

### mimetype (no extension)

```
application/epub+zip
```

### Stylesheet

#### page_styles.css

```css
@page {
	margin-bottom: 5pt;
	margin-top: 5pt
}
```

#### stylesheet.css

All other styles of your book.

### Cover image

No more than 64KB, JPG format.

### titlepage.xhtml

Replace {{cover.jpeg}} by your image.

```xhtml
<?xml version='1.0' encoding='utf-8'?>
<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en">
	<head>
		<meta http-equiv="Content-Type" content="text/html; charset=UTF-8"/>
		<title>Cover</title>
		<style type="text/css" title="override_css">
			@page {
				padding: 0;
				margin: 0;
			}
			body {
				text-align: center;
				padding: 0;
				margin: 0;
			}
		</style>
	</head>
	<body>
		<div>
			<svg xmlns="http://www.w3.org/2000/svg"
				xmlns:xlink="http://www.w3.org/1999/xlink" 
				version="1.1" width="100%" height="100%"
				viewBox="0 0 425 616" preserveAspectRatio="none">
				<image width="425" height="616" xlink:href="cover.jpeg"/>
			</svg>
		</div>
	</body>
</html>
```

### toc.ncx

This is a table of contents. Each xhtml files with book content must be
referenced here. A xhtml file can have more than one reference, using
anchor \(#\).

```xml
<?xml version='1.0' encoding='utf-8'?>
<ncx xmlns="http://www.daisy.org/z3986/2005/ncx/" version="2005-1" xml:lang="eng">
	<head>
		<meta content="0c159d12-f5fe-4323-8194-f5c652b89f5c" name="dtb:uid"/>
		<meta content="2" name="dtb:depth"/>
		<meta content="calibre (0.8.68)" name="dtb:generator"/>
		<meta content="0" name="dtb:totalPageCount"/>
		<meta content="0" name="dtb:maxPageNumber"/>
	</head>
	<docTitle>
		<text>How to Build a Website</text>
	</docTitle>
	<navMap>
		<navPoint id="a1" playOrder="0">
			<navLabel>
				<text>Hosting</text>
			</navLabel>
			<content src="{{your-file}}.html#{{your-anchor}}"/>
		</navPoint>
	</navMap>
</ncx>
```

### META-INF/container.xml

```xml
<?xml version="1.0"?>
<container version="1.0" xmlns="urn:oasis:names:tc:opendocument:xmlns:container">
	<rootfiles>
		<rootfile full-path="content.opf" media-type="application/oebps-package+xml"/>
	</rootfiles>
</container>
```

### content.opf

Parts you need to change:
- language;
- title;
- author-surname-first:
	- put surname, a comma, then the name.
- author-name-first;
	- put the name as you usually write.
- date:
	- `year`-`month`-`day`T`hour`:`minute`:`second``timezone`;
	- Example: **2020-09-12T03:49:09-03:00**;
	- *-03:00* is São Paulo/Brasil timezone.
- unique-id:
	- a generated ID, you can:
		- go to [https://www.uuidgenerator.net/];
		- search for UUID generator on internet.
	- the ID format is `********-****-****-****-************`;
	- the characters that replaces `*` are hexadecimals:
		- from 0 to 9 and from a to f.
- your-file:
	- the name of the files that has the book content;
	- if there is more than one, repeat the tag.

```xml
<?xml version='1.0' encoding='utf-8'?>
<package xmlns="http://www.idpf.org/2007/opf" version="2.0" unique-identifier="uuid_id">
	<metadata xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
		xmlns:opf="http://www.idpf.org/2007/opf"
		xmlns:dcterms="http://purl.org/dc/terms/"
		xmlns:calibre="http://calibre.kovidgoyal.net/2009/metadata"
		xmlns:dc="http://purl.org/dc/elements/1.1/">

		<dc:language>{{language}}</dc:language>
		<dc:title>{{title}}</dc:title>
		<dc:creator opf:file-as="{{author-surname-first}}" opf:role="aut">{{author-name-first}}</dc:creator>

		<meta name="cover" content="cover"/>

		<dc:date>{{date}}</dc:date>
		<dc:contributor opf:role="bkp"></dc:contributor>
		<dc:identifier id="uuid_id" opf:scheme="uuid">{{unique-id}}</dc:identifier>

	</metadata>

	<manifest>
		<item href="cover.jpeg" id="cover" media-type="image/jpeg"/>
		<item href="{{your-file}}.html" id="id1" media-type="application/xhtml+xml"/>
		<item href="page_styles.css" id="page_css" media-type="text/css"/>
		<item href="stylesheet.css" id="css" media-type="text/css"/>
		<item href="titlepage.xhtml" id="titlepage" media-type="application/xhtml+xml"/>
		<item href="toc.ncx" media-type="application/x-dtbncx+xml" id="ncx"/>
	</manifest>

	<spine toc="ncx">
		<itemref idref="titlepage"/>
		<itemref idref="id1"/>
	</spine>

	<guide>
		<reference href="titlepage.xhtml" type="cover" title="Cover"/>
	</guide>
</package>
```

## Zip it!

Create a Zip file (using some zip program) with the name:

```
{{book-name}} — {{authors names}}.zip
```

Rename the file to:

```
{{book-name}} — {{authors names}}.epub
```

Ignore if your computer says this is a dangerous change.

Now you need to test it at an ebook reader. If you have any problems,
[open a issue here](/../../issues) so we can fix this tutorial.

## Credits

This tutorial was made based on
https://www.lifewire.com/create-epub-file-from-html-and-xml-3467282
