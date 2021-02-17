REM # create sample images
setlocal enabledelayedexpansion

cd image_work
mkdir convert

for %%f in (*.png) do (
  magick convert %%f -swirl 240 convert/%%~nf.png
)

cd ../