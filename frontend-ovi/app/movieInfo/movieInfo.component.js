app.component('movieInfo', {
    bindings: {
        movies: '='
    },
    transclude: true,
    controller: function() {
        this.message = 'An Entity of Type: Class.';
    },
    controllerAs: 'info',
    templateUrl: 'http://localhost:8080/ovi/app/movieInfo/movieInfo.html'
});
