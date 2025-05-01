import gulp from 'gulp';
import dartSass from 'sass';
import gulpSass from 'gulp-sass';
import cleanCSS from 'gulp-clean-css';
import sourcemaps from 'gulp-sourcemaps';
import uglify from 'gulp-uglify';
import concat from 'gulp-concat';
import babel from 'gulp-babel';
import rename from 'gulp-rename';
import autoprefixer from 'gulp-autoprefixer';

const { src, dest, watch, series, parallel } = gulp;
const sass = gulpSass(dartSass)

// Paths
const paths = {
    scss: {
        src: 'wwwroot/src/scss/**/*.scss',
        dest: 'wwwroot/css'
    },
    js: {
        src: 'wwwroot/src/js/**/*.js',
        dest: 'wwwroot/js'
    }
};

// Compile SCSS
function styles() {
    return src(paths.scss.src)
        .pipe(sourcemaps.init())
        .pipe(sass().on('error', sass.logError))
        .pipe(autoprefixer({
            cascade: false
        }))
        .pipe(cleanCSS())
        .pipe(rename({ suffix: '.min' }))
        .pipe(sourcemaps.write('.'))
        .pipe(dest(paths.scss.dest));
}

// Bundle and minify JS
function scripts() {
    return src(paths.js.src)
        .pipe(sourcemaps.init())
        .pipe(babel({
            presets: ['@babel/preset-env']
        }))
        .pipe(concat('foundation.js'))
        .pipe(uglify())
        .pipe(rename({ suffix: '.min' }))
        .pipe(sourcemaps.write('.'))
        .pipe(dest(paths.js.dest));
}

// Watch files
export function watchFiles() {
    watch(paths.scss.src, styles);
    watch(paths.js.src, scripts);
}

// Build
export const build = series(
    parallel(styles, scripts)
);

export const dev = series(
    build,
    watchFiles
);

// Default when you run just "gulp"
export default build;