'use strict';

function showTriplesController($routeParams, $location, getRequestTriples) {
    var self = this;

    self.propertyUri = decodeURIComponent($routeParams.property);

    self.$onInit = () => {
        self.triples = [];
        self.triplesNumber = 0;
        self.isNodeLiteral = false;
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
    
    self.showResourceInfo = (subject) => {
        if (subject) {
            if (subject.includes('movie'))
                $location.path('/movieInfo/' + encodeURIComponent(subject));
            else if (!subject.includes('http'))
                self.isNodeLiteral = true;
            else
                $location.path('/wade-ovi.org/' + encodeURIComponent(subject));
        }
    }

    self.showPerson = (predicate, object) => {
        if (predicate.includes('dbpediaReference'))
            $location.path('/person/' + encodeURIComponent(object));
        else if (!object.includes('http'))  // literal
            self.isNodeLiteral = true;
        else
            $location.path('/wade-ovi.org/' + encodeURIComponent(object));
    }
    
    self.goToSearchPage = () => {
        $location.path('/search');
    }
}
