<h1 align="center">ZapArtist</h1>
<h4 align="center">WhatsApp Sticker Automation</h4>
<div align="center">
	<a href="https://github.com/davicr/ZapArtist/blob/master/LICENSE">
		<img src="https://img.shields.io/github/license/davicr/ZapArtist.svg"/>
	</a>
</div>
<div align="center">
	compress, convert, update JSON. quick.
</div>

## Information
ZapArtist is an application used for automating the workflow for creating a WhatsApp sticker. With a 512x512 PNG, you'll be able to obtain a
compressed and ready for WhatsApp WebP. It'll also attempt to insert the new WebP in the configuration JSON, relieving you from the pain of editing
the file manually.

## Usage
```
...\ZapArtist\bin\Release> .\ZapArtist.exe <path to PNG image> <path to directory containing the configuration JSON> <sticker pack name>
``` 

Disclaimer: There is no functionality for creating a new sticker pack in the JSON file. You should probably use an existing one, and if needed,
create one manually. :(
