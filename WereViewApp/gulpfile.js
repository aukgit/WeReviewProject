/*
This file is the main entry point for defining Gulp tasks and using Gulp plugins.
Click here to learn more. https://go.microsoft.com/fwlink/?LinkId=518007
*/


var gulp = require("gulp");
var concat = require("gulp-concat");
var uglify = require("gulp-uglify");
var minify = require("gulp-csso");
var inject = require("gulp-inject");
var config = require("./gulp.config")();

// Task to minify JS
gulp.task("min-all-js", function () {
    return gulp
        .src(config.js)
        .pipe(concat(config.minJs))
        .pipe(uglify())
        .pipe(gulp.dest(config.minJsDestination));
});
// Task to minify CSS
gulp.task("min-all-css", function () {
    return gulp
        .src(config.css)
        .pipe(concat(config.minCss))
        .pipe(minify())
        .pipe(gulp.dest(config.minCssDestination));
});

// Task to inject minifed JS
gulp.task("inject-min-js", function () {
    return gulp
        .src(config.htmlSource)
        .pipe(inject(gulp.src(config.minJsDestination + config.minJs)))
        .pipe(gulp.dest("."));
});
// Task to inject minifed CSS
gulp.task("inject-min-css", function () {
    return gulp
        .src(config.htmlSource)
        .pipe(inject(gulp.src(config.minCssDestination + config.minCss)))
        .pipe(gulp.dest("."));
});

gulp.task('default', ['min-all-js', 'min-all-css', 'inject-min-js', 'inject-min-css']);