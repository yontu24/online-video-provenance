app.component('findTitles', {
    bindings: {
        titles: '='
    },
    transclude: true,
    controller: findTitlesController,
    controllerAs: 'titlesCtrl',
    templateUrl: 'http://localhost:8080/ovi/app/findTitles/findTitles.html'
});
