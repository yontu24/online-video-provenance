'use strict';

function movieInfoController($routeParams, $location, getRequestMovieInfo, processMovieInfo) {
    var self = this;

    self.titleStr = $routeParams.title;
    
    self.$onInit = () => {
        self.triples = [];
        self.triplesNumber = 0;

        self.fetchData(self.titleStr);
    }

    self.fetchData = (title) => {
        if (title) {
            getRequestMovieInfo.get(title).then(
                (response) => {
                    self.triples = processMovieInfo.getMovieInfo(JSON.stringify(response.data));
                    self.triplesNumber = Object.keys(self.triples).length;
                },
                (error) => {
                    self.triples = error.statusText;
                    self.triplesNumber = 0;
                }
            );
        }
    }

    self.goToSearchPage = () => {
        $location.path('/search');
    }
}
