<img align="right" src="images/markdowntextstring-icon.png" width="120" height="120" alt="An input field with some Markdown text on top of the Vokseværk 'fire-heart' logo" />

# Markdown TextString for Umbraco

A property editor for those times where one just needs to be able to
emphasize a word or two in a property that's created as a textstring.

The accompanying PropertyValueConverter allows for rendering either the raw
text value or the parsed HTML. Both are readily available.

**NOTE:**
Since the real value is in the PropertyValueConverter, it's installed with the
property editor - as a `*.cs` file in the `~/App_Code/` folder. This also means
there's two different versions of this package - one for Umbraco 7 and another
for Umbraco 8 - make sure you install the right one for your site.

## Screenshot

![Markdown TextString Editor](images/markdowntextstring-editor.jpg)

## Rendering

The raw value stored in the property editor is a `string` of the Markdown text.
To convert it to HTML you call `.ToHTML()`:

```razor
<h1>@Model.PageTitle.ToHTML()</h1>
```

(That method's been aliased as `.ToHtml()` in case you prefer that... :-)

## Developing & Building

On macOS you can run the `build.sh` script from the terminal, which will
build two ZIP files in the `dist` folder that is installable from
Umbraco 8's _Packages_ section or Umbraco 7's _Developer > Packages_ section.

The build script versions the files so it's easier to test the package inside
an Umbraco installation by uninstalling the existing version and then
installing a new build. Existing data-types keep their data as long as their
alias and/or storage type isn't changed.

To update the version number, increment the `packageVersion` entity in the
`src/package.ent` file.
