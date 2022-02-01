'use strict';

function showTriplesController($routeParams, $location, getRequestTriples) {
    var self = this;

    self.propertyUri = decodeURIComponent($routeParams.property);

    self.$onInit = () => {
        self.triples = [];
        self.triplesNumber = 0;
        self.fetchData(self.propertyUri);
    }

    self.fetchData = (uri) => {
        getRequestTriples.get(uri).then(
            (response) => {
                self.triples = response.data;
                self.triplesNumber = self.triples.length;
            },
            (error) => {
                self.triples = error.statusText;
            }
        );
    }
    
    self.showMovieInfo = (subject) => {
        if (subject && subject.includes('movie')) {
            $location.path('/movieInfo/' + encodeURIComponent(subject));
        }
    }

    self.showPerson = (predicate, object) => {
        if (predicate.includes('starring') || predicate.includes('directedBy') || predicate.includes('writtenBy') || predicate.includes('producedBy')) {
            object = encodeURIComponent(object);
            $location.path('/person/' + object);
        }
    }
}
