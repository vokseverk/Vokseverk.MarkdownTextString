VERSION=`grep -o ' packageVersion \"\(.*\)\"' src/package.ent | awk '{print $2}' | sed 's/"//g'`
NAMESPACE=`grep -o ' orgNamespace \"\(.*\)\"' src/package.ent | awk '{print $2}' | sed 's/"//g'`
PACKAGE=`grep -o ' propertyEditorAlias \"\(.*\)\"' src/package.ent | awk '{print $2}' | sed 's/"//g'`
PKG_NAME="${NAMESPACE}.${PACKAGE}"

# Create the dist directory if needed
if [[ ! -d dist ]]; then
	mkdir -p dist/package-v7
	mkdir -p dist/package-v8
else
	rm dist/package-v7/*.*
	rm dist/package-v8/*.*
fi

# Copy files for v7
cp src/*.css dist/package-v7/
cp src/*.js dist/package-v7/
cp src/*.html dist/package-v7/
cp src/lang/*.xml dist/package-v7/

# Copy files for v8
cp src/*.css dist/package-v8/
cp src/*.js dist/package-v8/
cp src/*.html dist/package-v8/
cp src/lang/*.xml dist/package-v8/

# Copy the Value Converters
cp src/*V7.cs dist/package-v7/
cp src/*V8.cs dist/package-v8/

# Transform the package XML files
xsltproc --novalid --xinclude --output dist/package-v7/package.xml lib/packager.xslt src/package-v7.xml
xsltproc --novalid --xinclude --output dist/package-v8/package.xml lib/packager.xslt src/package-v8.xml

# Transform the manifest XML files
xsltproc --novalid --xinclude --output dist/package-v7/package.manifest lib/manifester.xslt src/manifest-v7.xml
xsltproc --novalid --xinclude --output dist/package-v8/package.manifest lib/manifester.xslt src/manifest-v8.xml


# Build the ZIP files
zip -j "dist/${PKG_NAME}-${VERSION}_v7.zip" dist/package-v7/* -x \*.DS_Store
zip -j "dist/${PKG_NAME}-${VERSION}_v8.zip" dist/package-v8/* -x \*.DS_Store
