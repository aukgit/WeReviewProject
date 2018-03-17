
//var gulp = require("gulp");
//var concat = require("gulp-concat");
//var uglify = require("gulp-uglify");
//var minify = require("gulp-csso");
//var inject = require("gulp-inject");
//var config = require("./gulp.config")();

//// Task to minify JS
//gulp.task("min-all-js", function () {
//    return gulp
//        .src(config.js)
//        .pipe(concat(config.minJs))
//        .pipe(uglify())
//        .pipe(gulp.dest(config.minJsDestination));
//});
//// Task to minify CSS
//gulp.task("min-all-css", function () {
//    return gulp
//        .src(config.css)
//        .pipe(concat(config.minCss))
//        .pipe(minify())
//        .pipe(gulp.dest(config.minCssDestination));
//});

//// Task to inject minifed JS
//gulp.task("inject-min-js", function () {
//    return gulp
//        .src(config.htmlSource)
//        .pipe(inject(gulp.src(config.minJsDestination + config.minJs)))
//        .pipe(gulp.dest("."));
//});
//// Task to inject minifed CSS
//gulp.task("inject-min-css", function () {
//    return gulp
//        .src(config.htmlSource)
//        .pipe(inject(gulp.src(config.minCssDestination + config.minCss)))
//        .pipe(gulp.dest("."));
//});

//gulp.task('default', ['min-all-js', 'min-all-css', 'inject-min-js', 'inject-min-css']);

//function getFolders(dir) {
//    return fs.readdirSync(dir)
//        .filter(function (file) {
//            return fs.statSync(path.join(dir, file)).isDirectory();
//        });
//}

//gulp.task('scripts', function () {
//    var jsfolders = getFolders(scriptsPath);
//    var jstasks = jsfolders.map(function (jsfolders) {
//        return gulp.src(path.join(scriptsPath, jsfolders, '/**/*.js'))
//            .pipe(concat(jsfolders + '.js'))
//            .pipe(gulp.dest(scriptsPath))
//            .pipe(uglify())
//            .pipe(rename(jsfolders + '.min.js'))
//            .pipe(gulp.dest(scriptsPath));
//    });
//    return es.concat.apply(null, jstasks);
//});


//gulp.task('styles', function () {
//    var cssfolders = getFolders(stylesPath);
//    var csstasks = cssfolders.map(function (cssfolders) {
//        return gulp.src(path.join(stylesPath, cssfolders, '/**/*.css'))
//            .pipe(concat(cssfolders + '.css'))
//            .pipe(gulp.dest(stylesPath))
//            .pipe(uglify())
//            .pipe(rename(cssfolders + '.min.css'))
//            .pipe(gulp.dest(stylesPath));
//    });
//    return es.concat.apply(null, csstasks);
//});


//gulp.task('default', ['scripts','styles']);
//gulp.task('default', ['scripts']);


var fs = require('fs');
var path = require('path');
var es = require('event-stream');
var gulp = require('gulp');
var concat = require('gulp-concat');
var rename = require('gulp-rename');
var uglify = require('gulp-uglify');
var cssMin = require('gulp-css');




gulp.task('css',
    function () {
        gulp.src([
            './Content/styles/CompactCSS.css'
        ])
            .pipe(concat('app.css'))
            .pipe(cssMin())
            .pipe(gulp.dest('./Content/published/Styles'));

        gulp.src([
            './Content/styles/css/**/*.css'])
            .pipe(concat('libs.css'))
            .pipe(cssMin())
            .pipe(gulp.dest('./Content/published/Styles'));
    });


gulp.task('scripts',
    function () {
        gulp.src([
            './Content/scripts/JavaScript-Mvc-framework/CompactCSS.css',
            './Content/scripts/JavaScript-Mvc-framework/app.config.js',
            './Content/scripts/JavaScript-Mvc-framework/app.executeAfter.js',
            './Content/scripts/JavaScript-Mvc-framework/app.executeBefore.js',
            './Content/scripts/JavaScript-Mvc-framework/app.global.js',
            './Content/scripts/JavaScript-Mvc-framework/app.js',
            './Content/scripts/JavaScript-Mvc-framework/app.run.js',
            './Content/scripts/JavaScript-Mvc-framework/attachInitialize.js',
            './Content/scripts/JavaScript-Mvc-framework/byId.js',
            './Content/scripts/JavaScript-Mvc-framework/controllers.js',
            './Content/scripts/JavaScript-Mvc-framework/initialize.js',
            './Content/scripts/JavaScript-Mvc-framework/jQueryCaching.js',
            './Content/scripts/JavaScript-Mvc-framework/jQueryExtend.fn.js',
            './Content/scripts/JavaScript-Mvc-framework/jQueryExtend.js'])
            .pipe(concat('app.js'))
            .pipe(cssMin())
            .pipe(gulp.dest('./Content/published/Scripts'));

        gulp.src([
            './Content/scripts/JavaScript-Mvc-framework/Areas/**/*.js',
            './Content/scripts/JavaScript-Mvc-framework/component/**/*.js',
            './Content/scripts/JavaScript-Mvc-framework/controllers/**/*.js',
            './Content/scripts/JavaScript-Mvc-framework/events/**/*.js',
            './Content/scripts/JavaScript-Mvc-framework/extensions/**/*.js',
            './Content/scripts/JavaScript-Mvc-framework/libs/**/*.js',
            './Content/scripts/JavaScript-Mvc-framework/prototypes/**/*.js',
            './Content/scripts/JavaScript-Mvc-framework/schema/**/*.js'
        ])
            .pipe(concat('components.js'))
            .pipe(cssMin())
            .pipe(gulp.dest('./Content/published/Scripts'));
    });


gulp.task('default', ['css', 'scripts']);







