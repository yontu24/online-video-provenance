'use strict';

function movieInfoController($routeParams, $location, getRequestMovieInfo, processMovieInfo) {
    var self = this;

    self.titleStr = $routeParams.title;
    
    self.$onInit = () => {
        self.triples = [];
        self.triplesNumber = 0;
        self.movieTitle = '';
        self.movieUri = '';

        self.fetchData(self.titleStr);
    }

    self.fetchData = (title) => {
        if (title) {
            getRequestMovieInfo.get(title).then(
                (response) => {
                    self.objectResponse = processMovieInfo.getMovieInfo(JSON.stringify(response.data));
                    self.triples = self.objectResponse.triples;
                    self.movieTitle = self.objectResponse.title;
                    self.movieUri = self.objectResponse.movieUri;
                    self.triplesNumber = self.triples == null ? 0 : Object.keys(self.triples).length;
                },
                (error) => {
                    self.triples = error.statusText;
                    self.triplesNumber = 0;
                }
            );
        }
    }

    self.showTriples = (property) => {
        property = encodeURIComponent(property);
        $location.path('/wade-ovi.org/' + property);
    }

    self.showPersons = (predicate, object) => {
        if (predicate.includes('starring') || predicate.includes('directedBy') || predicate.includes('writtenBy') || predicate.includes('producedBy')) {
            object = encodeURIComponent(object);
            $location.path('/person/' + object);
        }
    }

    self.goToSearchPage = () => {
        $location.path('/search');
    }
}
