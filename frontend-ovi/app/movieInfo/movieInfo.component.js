app.component('movieInfo', {
    bindings: {
        movies: '='
    },
    transclude: true,
    controller: movieInfoController,
    controllerAs: 'info',
    templateUrl: 'http://localhost:8080/ovi/app/movieInfo/movieInfo.html'
});
