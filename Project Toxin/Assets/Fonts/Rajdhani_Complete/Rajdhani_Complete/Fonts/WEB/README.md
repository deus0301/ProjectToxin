# Installing Webfonts
Follow these simple Steps.

## 1.
Put `rajdhani/` Folder into a Folder called `fonts/`.

## 2.
Put `rajdhani.css` into your `css/` Folder.

## 3. (Optional)
You may adapt the `url('path')` in `rajdhani.css` depends on your Website Filesystem.

## 4.
Import `rajdhani.css` at the top of you main Stylesheet.

```
@import url('rajdhani.css');
```

## 5.
You are now ready to use the following Rules in your CSS to specify each Font Style:
```
font-family: Rajdhani-Light;
font-family: Rajdhani-Regular;
font-family: Rajdhani-Medium;
font-family: Rajdhani-SemiBold;
font-family: Rajdhani-Bold;
font-family: Rajdhani-Variable;

```
## 6. (Optional)
Use `font-variation-settings` rule to controll axes of variable fonts:
wght 300.0

Available axes:
'wght' (range from 300.0 to 700.0

